using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SimpleCarController : MonoBehaviour
{ 
    public List<CarInfo> carInfo;
    public float maxMotorTorque;
    public float maxSteeringAngle;
   
    //signPost-path finding
    private SignPost colorChange;
    private bool signPostActive;
    private GameObject tempGameObject;
    private float colorTimer;
    private float waitingTime = 3.0f;
    private Color org;

    //coins
    private int score;
    public Text scoreText;

    //gameTime
    private float gameTime = 60.0f;
    public Text timerText;

    //state machine
    public NormalState normalState = new NormalState();
    public PedestrianState pedestrianState = new PedestrianState();
    public AIState state;
    public float initialMaxMotor;

    private void Start()
    {
        SetScoreText();
        initialMaxMotor = maxMotorTorque;
        SetState(normalState);
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");


        foreach (CarInfo info in carInfo)
        {
            if (info.steering)
            {
                info.leftWheel.steerAngle = steering;
                info.rightWheel.steerAngle = steering;
            }
            if (info.motor)
            {
                info.leftWheel.motorTorque = motor;
                info.rightWheel.motorTorque = motor;

            }

            ApplyLocalPositionToVisuals(info.leftWheel);
            ApplyLocalPositionToVisuals(info.rightWheel);
        }
    }


    private void Update()
    {
        gameTime -= Time.deltaTime;
        SetTimerText();

        if (gameTime < 0.0f)
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;

        }

        if (signPostActive)
        {
            colorTimer += Time.deltaTime;

            if (colorTimer > waitingTime)
            {
                colorChange.tile.GetComponentInChildren<Renderer>().material.color = org;

                // Remove the recorded 2 seconds.
                colorTimer = colorTimer - waitingTime;
                colorChange = null;
                signPostActive = false;
                tempGameObject.SetActive(true);
            }
        }

    }

    void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.CompareTag("SignPost"))
        {
            TriggerSignPost(other);
        }

        else if (other.gameObject.CompareTag("Coin"))
        {
            TriggerCoin(other);
        }

        else if (other.gameObject.CompareTag("Pedestrian"))
        {
            SetState(pedestrianState);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pedestrian"))
        {
            SetState(normalState);
        }
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void SetTimerText()
    {
        timerText.text = "Time: " + gameTime.ToString("0");

    }

    void SetState (AIState newState)
    {
        state = newState;
        state.EnterState(this);
        Debug.Log(state);
    }

    void TriggerSignPost(Collider other)
    {
        if (score >= 1)
        {
            tempGameObject = other.gameObject;
            other.gameObject.SetActive(false);
            colorChange = other.gameObject.GetComponent<SignPost>();
            org = colorChange.tile.GetComponentInChildren<Renderer>().material.color;
            colorChange.tile.GetComponentInChildren<Renderer>().material.color = Color.blue;

            signPostActive = true;
            score--;
            SetScoreText();
        }
        else
        {
            //TODO: implement alert 
        }
    }

    void TriggerCoin (Collider other)
    {
        other.gameObject.SetActive(false);
        score++;
        SetScoreText();
    }




}
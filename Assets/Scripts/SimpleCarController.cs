using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
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
    //public AIState state = new AIState();
    enum AIState { 
        Normal,
        Pedestrian
    };

    AIState state = AIState.Normal;
    float initialMaxMotor;

    private void Start()
    {
        SetScoreText();
        initialMaxMotor = maxMotorTorque;
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


        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;

            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }


    private void Update()
    {
        gameTime -= Time.deltaTime;
        SetTimerText();

        StateMachine();

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
            SetState(AIState.Pedestrian);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pedestrian"))
        {
            SetState(AIState.Normal);
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
        //state.Enter();
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



    void StateMachine()
    {
       switch (state)
        {
            case AIState.Normal:
                maxMotorTorque = initialMaxMotor;
                break;


            case AIState.Pedestrian:
                foreach (AxleInfo axleInfo in axleInfos) {
                    axleInfo.leftWheel.brakeTorque = 100000f;
                    axleInfo.rightWheel.brakeTorque = 100000f;
                }

                foreach (AxleInfo axleInfo in axleInfos)
                {
                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.brakeTorque = 0;
                }

                maxMotorTorque = 5.0f;
                break;
        };

    }


}
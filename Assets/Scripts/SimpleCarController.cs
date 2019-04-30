using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField]
    public SignPost[] intersections;
    public float maxMotorTorque;
    public float maxSteeringAngle;
   
    //signPost
    private SignPost colorChange;
    private bool signPostActive;
    private GameObject tempGameObject;
    private float colorTimer;
    private float waitingTime = 2.0f;
    private Color org;

    //coins
    private int score;

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
            if(score >= 1)
            {
                tempGameObject = other.gameObject;
                other.gameObject.SetActive(false);
                colorChange = other.gameObject.GetComponent<SignPost>();
                org = colorChange.tile.GetComponentInChildren<Renderer>().material.color;
                colorChange.tile.GetComponentInChildren<Renderer>().material.color = Color.blue;

                signPostActive = true;
                score--;
            }
            else
            {
                //TODO: implement alert 
            }

                


        }

        else if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            score++;
        }
    }


}
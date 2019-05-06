using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PedestrianState: AIState
{ 

    public override void EnterState(SimpleCarController car)
    {
        foreach (CarInfo info in car.carInfo)
        {
            info.leftWheel.brakeTorque = 10000000f;
            info.rightWheel.brakeTorque = 10000000f;
        }

        foreach (CarInfo info in car.carInfo)
        {
            info.leftWheel.brakeTorque = 0;
            info.rightWheel.brakeTorque = 0;
        }

        car.maxMotorTorque = 5.0f;
        Debug.Log(car.maxMotorTorque);
    }
}

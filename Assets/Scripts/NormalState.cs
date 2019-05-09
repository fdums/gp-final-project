using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NormalState: AIState
{

    public override void EnterState(SimpleCarController car)
    {
        car.maxMotorTorque = car.initialMaxMotor;
        car.isPed = false;
        foreach (CarInfo info in car.carInfo)
        {
            info.leftWheel.brakeTorque = 0;
            info.rightWheel.brakeTorque = 0;
        }
        car.pedestrianWarning.enabled = false;

    }
}


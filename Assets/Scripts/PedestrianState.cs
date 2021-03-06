﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PedestrianState: AIState
{ 

    public override void EnterState(SimpleCarController car)
    {
        car.isPed = true;
        car.pedestrianWarning.enabled = true;

     
        foreach (CarInfo info in car.carInfo)
        {
            info.leftWheel.brakeTorque = 0;
            info.rightWheel.brakeTorque = 0;
        }



        car.maxMotorTorque = 5.0f;

    }
}

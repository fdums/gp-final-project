using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NormalState: AIState
{

    public override void EnterState(SimpleCarController car)
    {
        car.maxMotorTorque = car.initialMaxMotor;
        Debug.Log(car.maxMotorTorque);
        Debug.Log("ai normal");

    }
}


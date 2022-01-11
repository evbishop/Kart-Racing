using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] Transform sphere;
    [SerializeField] float forwardAccel = 8f, reverseAccel = 4f, turnStrength = 180f;
    [SerializeField] CarGroundHandler groundChecker;

    protected CarGroundHandler GroundChecker { get { return groundChecker; } }
    protected float ForwardAccel { get { return forwardAccel; } }
    protected float ReverseAccel { get { return reverseAccel; } }
    protected float TurnStrength { get { return turnStrength; } }
    public float TurnInput { get; protected set; }
    public float Speed { get; protected set; }
    public CarState State { get; protected set; } = CarState.OnGroundAndNotMoving;

    protected void LateUpdate()
    {
        transform.position = sphere.position;
    }
}

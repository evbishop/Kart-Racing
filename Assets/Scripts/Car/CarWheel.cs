using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel : MonoBehaviour
{
    [SerializeField] Car car;
    [SerializeField] bool isRightWheel;
    [SerializeField] float maxWheelTurn = 25f;

    void LateUpdate()
    {
        RotateTheWheel();
    }

    void RotateTheWheel()
    {
        transform.localRotation = Quaternion.Euler(
            transform.localRotation.eulerAngles.x,
            car.TurnInput * maxWheelTurn - (isRightWheel ? 0 : 180),
            transform.localRotation.eulerAngles.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSphere : MonoBehaviour
{
    [SerializeField] Car car;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] float gravityForce = 10f, dragOnGround = 3f, dragInAir = 0.1f;

    public Car Car { get { return car; } }

    void Start()
    {
        transform.parent = null;
    }

    void FixedUpdate()
    {
        if (Car.OnGround)
        {
            rigidbody.drag = dragOnGround;
            if (Mathf.Abs(Car.Speed) > 0)
                rigidbody.AddForce(Car.transform.forward * Car.Speed);
        }
        else
        {
            rigidbody.drag = dragInAir;
            rigidbody.AddForce(Vector3.down * gravityForce * 100f);
        }
    }
}

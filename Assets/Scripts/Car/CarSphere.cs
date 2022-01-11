using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSphere : MonoBehaviour
{
    [SerializeField] Car car;
    [SerializeField] CarGroundHandler groundChecker;
    [SerializeField] Rigidbody rb;
    [SerializeField] float gravityForce = 10f, dragOnGround = 3f, dragInAir = 0.1f;

    public Car Car { get { return car; } }

    void Start()
    {
        name = $"{transform.parent.name} Sphere";
        transform.parent = null;
    }

    void FixedUpdate()
    {
        if (groundChecker.OnGround)
        {
            rb.drag = dragOnGround;
            if (Mathf.Abs(Car.Speed) > 0)
                rb.AddForce(Car.transform.forward * Car.Speed);
        }
        else
        {
            rb.drag = dragInAir;
            rb.AddForce(Vector3.down * gravityForce * 100f);
        }
    }
}

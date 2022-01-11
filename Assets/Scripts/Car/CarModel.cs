using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModel : MonoBehaviour
{
    [SerializeField] Rigidbody sphereRB, modelRB;

    void FixedUpdate()
    {
        if (modelRB.velocity.magnitude > 0)
            sphereRB.velocity += modelRB.velocity;
    }
}

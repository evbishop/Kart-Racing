using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGroundChecker : MonoBehaviour
{
    [SerializeField] Transform groundRayPoint;
    [SerializeField] LayerMask ground;
    [SerializeField] float groundRayLength = 0.5f;

    public bool OnGround { get; private set; }

    void Update()
    {
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out RaycastHit hit, groundRayLength, ground))
        {
            OnGround = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        else
        {
            OnGround = false;
        }
    }
}

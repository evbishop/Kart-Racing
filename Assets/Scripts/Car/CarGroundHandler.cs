using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGroundHandler : MonoBehaviour
{
    [SerializeField] Transform groundRayPoint;
    [SerializeField] LayerMask ground;
    [SerializeField] float groundRayLength = 0.5f;
    RaycastHit hit;

    public bool OnGround { get; private set; }

    void Update()
    {
        CheckForGround();
    }

    void CheckForGround()
    {
        if (Physics.Raycast(groundRayPoint.position, -transform.up,
            out hit, groundRayLength, ground))
        {
            OnGround = true;
            RotateOnSlopes();
        }
        else OnGround = false;
    }

    void RotateOnSlopes()
    {
        transform.rotation = Quaternion.FromToRotation
            (transform.up, hit.normal) * transform.rotation;
    }
}

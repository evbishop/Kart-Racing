using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGroundChecker : MonoBehaviour
{
    [SerializeField] Transform groundRayPoint;
    [SerializeField] LayerMask ground;
    [SerializeField] float groundRayLength = 0.5f;
    RaycastHit hit;

    public bool OnGround { get; private set; }

    void Update()
    {
        if (Physics.Raycast(groundRayPoint.position, -transform.up, 
            out hit, groundRayLength, ground))
            OnGround = true;
        else OnGround = false;
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
    }
}

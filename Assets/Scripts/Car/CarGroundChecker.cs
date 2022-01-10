using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGroundChecker : MonoBehaviour
{
    [SerializeField] Transform groundRayPoint;
    [SerializeField] LayerMask ground;
    [SerializeField] float groundRayLength = 0.5f;

    public bool OnGround { get; private set; }

    void Start()
    {
        Car.RotateCarOnSlopes += HandleRotation;
    }

    void OnDestroy()
    {
        Car.RotateCarOnSlopes -= HandleRotation;
    }

    void HandleRotation()
    {
        if (!Physics.Raycast(groundRayPoint.position, -transform.up,
            out RaycastHit hit, groundRayLength, ground)) return;
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
    }

    void Update()
    {
        if (Physics.Raycast(groundRayPoint.position, -transform.up, 
            out RaycastHit hit, groundRayLength, ground))
            OnGround = true;
        else OnGround = false;
    }
}

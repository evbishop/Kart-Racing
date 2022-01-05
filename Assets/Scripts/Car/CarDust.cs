using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDust : MonoBehaviour
{
    [SerializeField] Car car;
    [SerializeField] ParticleSystem particle;
    [SerializeField] float maxEmission = 30f;
    float emissionRate;

    void LateUpdate()
    {
        switch (car.State)
        {
            case CarState.OnGroundAndMovingForward:
                emissionRate = 1f;
                break;
            case CarState.OnGroundAndMovingBackward:
                emissionRate = 0.5f;
                break;
            case CarState.OnGroundAndNotMoving:
                emissionRate = 0f;
                break;
            case CarState.OffGround:
                emissionRate = 0f;
                break;
        }
        var emissionModule = particle.emission;
        emissionModule.rateOverTime = emissionRate * maxEmission;
    }
}

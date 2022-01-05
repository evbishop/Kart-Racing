using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDust : MonoBehaviour
{
    [SerializeField] Car car;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] float maxEmission = 30f;

    void Update()
    {
        var emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = car.EmissionRate * maxEmission;
    }
}

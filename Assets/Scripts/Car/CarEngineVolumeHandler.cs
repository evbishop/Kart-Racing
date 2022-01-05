using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngineVolumeHandler : MonoBehaviour
{
    [SerializeField] Car car;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        audioSource.volume = 0;
        LapHandler.OnGameOver += HandleGameOver;
    }

    void OnDestroy()
    {
        LapHandler.OnGameOver -= HandleGameOver;
    }

    void HandleGameOver(string textForUI)
    {
        audioSource.volume = 0;
    }

    void LateUpdate()
    {
        switch (car.State)
        {
            case CarState.OnGroundAndMovingForward:
                if (audioSource.volume < 1)
                    audioSource.volume += Time.deltaTime;
                break;
            case CarState.OnGroundAndMovingBackward:
                if (audioSource.volume < 0.5)
                    audioSource.volume += Time.deltaTime;
                else audioSource.volume -= Time.deltaTime;
                break;
            case CarState.OnGroundAndNotMoving:
                if (audioSource.volume > 0.1)
                    audioSource.volume -= 2 * Time.deltaTime;
                break;
            case CarState.OffGround:
                if (audioSource.volume > 0.2)
                    audioSource.volume -= 0.5f * Time.deltaTime;
                break;
        }
    }
}

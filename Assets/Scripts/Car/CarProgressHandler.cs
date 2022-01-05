using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarProgressHandler : MonoBehaviour
{
    public int CurrentCheckpoint { get; private set; }
    public int CurrentLap { get; private set; }

    void Start()
    {
        CurrentCheckpoint = -1;
        CurrentLap = 1;
        Checkpoint.OnCarCrossedCheckpoint += HandleCarCrossedCheckpoint;
        LapHandler.OnCarFinishedLap += HandleCarFinishedLap;
    }

    void OnDestroy()
    {
        Checkpoint.OnCarCrossedCheckpoint -= HandleCarCrossedCheckpoint;
        LapHandler.OnCarFinishedLap -= HandleCarFinishedLap;
    }

    void HandleCarCrossedCheckpoint
        (CarProgressHandler carThatCrossedCheckpoint, int checkpointIndex)
    {
        if (this != carThatCrossedCheckpoint) return;
        CurrentCheckpoint = checkpointIndex;
    }

    void HandleCarFinishedLap(CarProgressHandler carThatFinishedLap)
    {
        if (this != carThatFinishedLap) return;
        CurrentCheckpoint = -1;
        CurrentLap++;
    }
}

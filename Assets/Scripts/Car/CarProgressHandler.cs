using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarProgressHandler : MonoBehaviour
{
    public int CurrentCheckpoint { get; set; }
    public int CurrentLap { get; set; }

    void Start()
    {
        CurrentCheckpoint = -1;
        CurrentLap = 1;
        Checkpoint.OnCarCrossedCheckpoint += HandleCarCrossedCheckpoint;
    }

    void OnDestroy()
    {
        Checkpoint.OnCarCrossedCheckpoint -= HandleCarCrossedCheckpoint;
    }

    void HandleCarCrossedCheckpoint(CarProgressHandler carThatCrossed, int checkpointIndex)
    {
        if (this != carThatCrossed) return;
        CurrentCheckpoint = checkpointIndex;
    }
}

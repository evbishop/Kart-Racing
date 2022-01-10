using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarProgressHandler : MonoBehaviour
{
    [SerializeField] Car car;

    public int CurrentCheckpoint { get; private set; }
    public int CurrentLap { get; private set; }

    public static event Action<int> OnPlayerCrossedCheckpoint;
    public static event Action<int> OnPlayerFinishedLap;
    public event Action<CarBot, int> OnBotCrossedCheckpoint;
    public event Action<CarBot> OnBotFinishedLap;

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
        if (car is CarPlayer)
            OnPlayerCrossedCheckpoint?.Invoke(checkpointIndex);
        else if (car is CarBot bot)
            OnBotCrossedCheckpoint?.Invoke(bot, checkpointIndex);
    }

    void HandleCarFinishedLap(CarProgressHandler carThatFinishedLap)
    {
        if (this != carThatFinishedLap) return;
        CurrentCheckpoint = -1;
        CurrentLap++;
        if (car is CarPlayer)
            OnPlayerFinishedLap?.Invoke(CurrentLap);
        else if (car is CarBot bot)
            OnBotFinishedLap?.Invoke(bot);
    }
}

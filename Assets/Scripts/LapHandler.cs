using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapHandler : MonoBehaviour
{
    [SerializeField] int maxLaps = 3;
    [SerializeField] Checkpoint[] checkpoints;
    [SerializeField] MeshRenderer meshRenderer;

    public Checkpoint[] Checkpoints { get { return checkpoints; } }
    public int MaxLaps { get { return maxLaps; } }
    public int FinalCheckpoint { get; private set; }
    
    public static event Action<CarProgressHandler> OnCarFinishedLap;
    public static event Action<string> OnGameOver;
    public static event Action<string, int> OnLapsTextUpdated;

    void Start()
    {
        CarProgressHandler.OnPlayerCrossedCheckpoint += HandlePlayerCrossedCheckpoint;
        OnLapsTextUpdated?.Invoke($"Lap: {1}/{MaxLaps}", checkpoints.Length);
        FinalCheckpoint = checkpoints.Length - 1;
        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i].Index = i;
    }

    void OnDestroy()
    {
         CarProgressHandler.OnPlayerCrossedCheckpoint -= HandlePlayerCrossedCheckpoint;
    }

    void HandlePlayerCrossedCheckpoint(int crossedIndex)
    {
        if (crossedIndex == FinalCheckpoint)
            meshRenderer.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        CarProgressHandler carProgress = other.GetComponent<CarProgressHandler>();
        if (carProgress.CurrentCheckpoint != FinalCheckpoint) return;

        Car car = other.GetComponent<CarSphere>().Car;
        if (carProgress.CurrentLap >= MaxLaps)
            OnGameOver?.Invoke(car.gameObject.name);
        else if (car is CarPlayer)
            meshRenderer.enabled = false;

        OnCarFinishedLap?.Invoke(carProgress);
    }
}

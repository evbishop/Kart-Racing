using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapHandler : MonoBehaviour
{
    [SerializeField] int numOfLaps = 1;
    [SerializeField] Checkpoint[] checkpoints;
    [SerializeField] MeshRenderer meshRenderer;

    public Checkpoint[] Checkpoints { get { return checkpoints; } }
    public int NumOfLaps 
    { 
        get { return numOfLaps; }
        private set 
        { 
            numOfLaps = value;
            OnLapsTextUpdated?.Invoke(1, numOfLaps, checkpoints.Length);
        }
    }
    public int FinalCheckpoint { get; private set; }
    
    public static event Action<CarProgressHandler> OnCarFinishedLap;
    public static event Action<string> OnGameOver;
    public static event Action<int, int, int> OnLapsTextUpdated;

    void Start()
    {
        CarProgressHandler.OnPlayerCrossedCheckpoint += HandlePlayerCrossedCheckpoint;
        StartGameButton.OnStartGame += HandleStartGame;
        
        FinalCheckpoint = checkpoints.Length - 1;
        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i].Index = i;
    }

    void OnDestroy()
    {
        CarProgressHandler.OnPlayerCrossedCheckpoint -= HandlePlayerCrossedCheckpoint;
        StartGameButton.OnStartGame -= HandleStartGame;
    }

    void HandlePlayerCrossedCheckpoint(int crossedIndex)
    {
        if (crossedIndex == FinalCheckpoint)
            meshRenderer.enabled = true;
    }

    void HandleStartGame(int numOfLaps)
    {
        NumOfLaps = numOfLaps;
    }

    void OnTriggerEnter(Collider other)
    {
        CarProgressHandler carProgress = other.GetComponent<CarProgressHandler>();
        if (carProgress.CurrentCheckpoint != FinalCheckpoint) return;

        Car car = other.GetComponent<CarSphere>().Car;
        if (carProgress.CurrentLap >= NumOfLaps)
            OnGameOver?.Invoke(car.gameObject.name);
        else if (car is CarPlayer)
            meshRenderer.enabled = false;

        OnCarFinishedLap?.Invoke(carProgress);
    }
}

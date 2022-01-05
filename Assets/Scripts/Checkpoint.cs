using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    MeshRenderer meshRenderer;
    int index;

    public int Index 
    { 
        get { return index; } 
        set
        {
            index = value;
            if (index == 0)
                LapHandler.OnPlayerFinishedLap += HandlePlayerFinishedLap;
        } 
    }

    public static event Action<int> OnPlayerCrossedCheckpoint;
    public static event Action<CarProgressHandler, int> OnCarCrossedCheckpoint;

    void Start()
    {
        OnPlayerCrossedCheckpoint += HandlePlayerCrossedCheckpoint;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnDestroy()
    {
        OnPlayerCrossedCheckpoint -= HandlePlayerCrossedCheckpoint;
        if (Index == 0)
            LapHandler.OnPlayerFinishedLap -= HandlePlayerFinishedLap;
    }

    void HandlePlayerFinishedLap(string textForUI)
    {
        meshRenderer.enabled = true;
    }

    void HandlePlayerCrossedCheckpoint(int crossedIndex)
    {
        if (crossedIndex == Index - 1) meshRenderer.enabled = true;
        else if (crossedIndex == Index) meshRenderer.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        CarProgressHandler carProgress = other.GetComponent<CarProgressHandler>();
        if (carProgress.CurrentCheckpoint != Index - 1) return;
        OnCarCrossedCheckpoint?.Invoke(carProgress, Index);
        
        Car car = other.GetComponent<CarSphere>().Car;
        if (car.IsPlayer) 
            OnPlayerCrossedCheckpoint?.Invoke(Index);
        else
        {
            LapHandler lapHandler = FindObjectOfType<LapHandler>();
            car.Destination = lapHandler.FinalCheckpoint == Index
                ? lapHandler.gameObject.transform.position
                : lapHandler.Checkpoints[Index + 1].gameObject.transform.position;
            car.Destination = new Vector3(
                car.Destination.x + UnityEngine.Random.Range(-car.RandomOffset, car.RandomOffset), 
                car.Destination.y,
                car.Destination.z);
        }
    }
}

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
                LapHandle.OnPlayerFinishedLap += HandlePlayerFinishedLap;
        } 
    }

    public static event Action<int> OnPlayerCrossedCheckpoint;

    void Start()
    {
        OnPlayerCrossedCheckpoint += HandlePlayerCrossedCheckpoint;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnDestroy()
    {
        OnPlayerCrossedCheckpoint -= HandlePlayerCrossedCheckpoint;
        if (Index == 0)
            LapHandle.OnPlayerFinishedLap -= HandlePlayerFinishedLap;
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
        Car car = other.GetComponent<Sphere>().Car;
        if (car.CurrentCheckpoint != Index - 1) return;
        LapHandle lapHandle = FindObjectOfType<LapHandle>();

        car.CurrentCheckpoint = Index;
        if (car.IsPlayer) OnPlayerCrossedCheckpoint?.Invoke(Index);
        else
        {
            car.Destination = lapHandle.FinalCheckpoint == Index
                ? lapHandle.gameObject.transform.position
                : lapHandle.Checkpoints[Index + 1].gameObject.transform.position;
            car.Destination = new Vector3(
                car.Destination.x + UnityEngine.Random.Range(-car.RandomOffset, car.RandomOffset), 
                car.Destination.y,
                car.Destination.z);
        }
    }
}

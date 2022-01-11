using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] float randomOffset = 3;
    int index;

    public int Index 
    { 
        get { return index; } 
        set
        {
            index = value;
            if (index == 0)
                CarProgressHandler.OnPlayerFinishedLap += HandlePlayerFinishedLap;
        } 
    }

    public static event Action<CarProgressHandler, int> OnCarCrossedCheckpoint;

    void Start()
    {
        CarProgressHandler.OnPlayerCrossedCheckpoint += HandlePlayerCrossedCheckpoint;
    }

    void OnDestroy()
    {
        CarProgressHandler.OnPlayerCrossedCheckpoint -= HandlePlayerCrossedCheckpoint;
        if (Index == 0)
            CarProgressHandler.OnPlayerFinishedLap -= HandlePlayerFinishedLap;
    }

    void HandlePlayerFinishedLap(int currentLap)
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
    }

    public Vector3 GetRandomDestination()
    {
        return new Vector3(
            transform.localPosition.x,
            transform.localPosition.y,
            transform.localPosition.z)
            + transform.right * UnityEngine.Random.Range(-randomOffset, randomOffset);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapHandle : MonoBehaviour
{
    [SerializeField] int maxLaps = 3;
    [SerializeField] Checkpoint[] checkpoints;
    ProgressUIHandler gm;
    MeshRenderer meshRenderer;

    public Checkpoint[] Checkpoints { get { return checkpoints; } }

    public int FinalCheckpoint { get; private set; }

    public static event Action<string> OnPlayerFinishedLap;
    public static event Action<string> OnGameOver;

    void Start()
    {
        Checkpoint.OnPlayerCrossedCheckpoint += HandlePlayerCrossedCheckpoint;
        meshRenderer = GetComponent<MeshRenderer>();
        gm = FindObjectOfType<ProgressUIHandler>();
        gm.LapText = $"Lap: {1}/{maxLaps}";
        gm.ProgressSliderMaxValue = checkpoints.Length;
        FinalCheckpoint = checkpoints.Length - 1;
        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i].Index = i;
    }

    void OnDestroy()
    {
        Checkpoint.OnPlayerCrossedCheckpoint -= HandlePlayerCrossedCheckpoint;
    }

    void HandlePlayerCrossedCheckpoint(int crossedIndex)
    {
        if (crossedIndex == checkpoints.Length - 1)
            meshRenderer.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Car car = other.GetComponent<Sphere>().Car;
        if (car.CurrentCheckpoint != FinalCheckpoint) return;
        car.CurrentLap++;
        car.CurrentCheckpoint = -1;
        if (car.CurrentLap > maxLaps) OnGameOver?.Invoke(car.gameObject.name);
        else
        {
            if (car.IsPlayer)
            {
                OnPlayerFinishedLap?.Invoke($"Lap: {car.CurrentLap}/{maxLaps}");
                meshRenderer.enabled = false;
            }
            else
            {
                car.Destination = checkpoints[0].gameObject.transform.position;
                car.Destination = new Vector3(
                    car.Destination.x + UnityEngine.Random.Range(-car.RandomOffset, car.RandomOffset),
                    car.Destination.y,
                    car.Destination.z);
            }
        }
    }
}

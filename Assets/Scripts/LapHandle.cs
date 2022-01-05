using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapHandle : MonoBehaviour
{
    [SerializeField] int maxLaps = 3;
    [SerializeField] Checkpoint[] checkpoints;
    GameManager gm;

    public Checkpoint[] Checkpoints { get { return checkpoints; } }

    public int FinalCheckpoint { get; private set; }

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        gm.LapText = $"Круг: {1}/{maxLaps}";
        gm.ProgressSliderMaxValue = checkpoints.Length;
        FinalCheckpoint = checkpoints.Length - 1;
        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i].Index = i;
    }

    void OnTriggerEnter(Collider other)
    {
        Car car = other.GetComponent<Sphere>().Car;
        if (car.CurrentCheckpoint != FinalCheckpoint) return;
        car.CurrentLap++;
        car.CurrentCheckpoint = -1;
        if (car.CurrentLap > maxLaps) gm.GameOver(car.gameObject.name);
        else
        {
            if (car.IsPlayer)
            {
                gm.LapText = $"����: {car.CurrentLap}/{maxLaps}";
                gm.ProgressSliderValue = 0;
                checkpoints[0].gameObject.GetComponent<MeshRenderer>().enabled = true;
                GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                car.Destination = checkpoints[0].gameObject.transform.position;
                car.Destination = new Vector3(
                    car.Destination.x + Random.Range(-car.RandomOffset, car.RandomOffset),
                    car.Destination.y,
                    car.Destination.z);
            }
        }
    }
}

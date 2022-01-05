using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int Index { get; set; }

    void OnTriggerEnter(Collider other)
    {
        Car car = other.GetComponent<Sphere>().Car;
        if (car.CurrentCheckpoint != Index - 1) return;
        LapHandle lapHandle = FindObjectOfType<LapHandle>();

        car.CurrentCheckpoint = Index;
        if (car.IsPlayer)
        {
            FindObjectOfType<GameManager>().ProgressSliderValue++;
            if (lapHandle.FinalCheckpoint == Index) lapHandle.gameObject.GetComponent<MeshRenderer>().enabled = true;
            else lapHandle.Checkpoints[Index + 1].gameObject.GetComponent<MeshRenderer>().enabled = true;
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            car.Destination = lapHandle.FinalCheckpoint == Index
                ? lapHandle.gameObject.transform.position
                : lapHandle.Checkpoints[Index + 1].gameObject.transform.position;
            car.Destination = new Vector3(
                car.Destination.x + Random.Range(-car.RandomOffset, car.RandomOffset), 
                car.Destination.y, 
                car.Destination.z);
        }
    }
}

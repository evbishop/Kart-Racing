using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBot : Car
{
    [SerializeField] float botTurnStrength = 5f, randomOffset = 4f;
    
    public float RandomOffset { get { return randomOffset; } }
    public Vector3 Destination { get; set; }

    void Start()
    {
        CarProgressHandler.OnBotFinishedLap += HandleBotFinishedLap;
        CarProgressHandler.OnBotCrossedCheckpoint += HandleBotCrossedCheckpoint;
        Destination = FindObjectOfType<LapHandler>().Checkpoints[0].gameObject.transform.position;
        Destination = new Vector3(
            Destination.x + Random.Range(-randomOffset, randomOffset),
            Destination.y,
            Destination.z);
    }

    void OnDestroy()
    {
        CarProgressHandler.OnBotFinishedLap -= HandleBotFinishedLap;
        CarProgressHandler.OnBotCrossedCheckpoint -= HandleBotCrossedCheckpoint;
    }

    void HandleBotFinishedLap(CarBot bot)
    {
        if (bot != this) return;
        LapHandler lapHandler = FindObjectOfType<LapHandler>();
        var checkpoints = lapHandler.Checkpoints;
        Destination = checkpoints[0].gameObject.transform.position;
        Destination = new Vector3(
            Destination.x + UnityEngine.Random.Range(-RandomOffset, RandomOffset),
            Destination.y,
            Destination.z);
    }

    void HandleBotCrossedCheckpoint(CarBot bot, int checkpointIndex)
    {
        if (bot != this) return;
        LapHandler lapHandler = FindObjectOfType<LapHandler>();
        var checkpoints = lapHandler.Checkpoints;
        Destination = lapHandler.FinalCheckpoint == checkpointIndex
            ? lapHandler.gameObject.transform.position
            : checkpoints[checkpointIndex + 1].gameObject.transform.position;
        Destination = new Vector3(
            Destination.x + UnityEngine.Random.Range(-RandomOffset, RandomOffset),
            Destination.y,
            Destination.z);
    }

    void Update()
    {
        BotMove();
        base.Update();
    }

    void BotMove()
    {
        Speed = ForwardAccel * 1000f;
        if (GroundChecker.OnGround) State = CarState.OnGroundAndMovingForward;
        else State = CarState.OffGround;
        RotateBotTowardsDestination();
    }

    void RotateBotTowardsDestination()
    {
        Vector3 relativePos = Destination - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePos);
        float randomTurnStrength = Random.Range(botTurnStrength, botTurnStrength * 2);

        Vector3 oldRotation = transform.rotation.eulerAngles;

        if (GroundChecker.OnGround)
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * randomTurnStrength);

        if ((oldRotation - transform.rotation.eulerAngles).y > 0.05f)
            TurnInput = Mathf.Lerp(TurnInput, -1, Time.deltaTime * 10);
        else if ((oldRotation - transform.rotation.eulerAngles).y < -0.05f)
            TurnInput = Mathf.Lerp(TurnInput, 1, Time.deltaTime * 10);
        else TurnInput = Mathf.Lerp(TurnInput, 0, Time.deltaTime * 10);
    }
}

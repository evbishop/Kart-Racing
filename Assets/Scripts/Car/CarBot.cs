using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBot : Car
{
    [SerializeField] float botTurnStrength = 5f;
    [SerializeField] CarProgressHandler progressHandler;
    LapHandler lapHandler;
    
    public Vector3 Destination { get; set; }

    void Start()
    {
        progressHandler.OnBotFinishedLap += HandleBotFinishedLap;
        progressHandler.OnBotCrossedCheckpoint += HandleBotCrossedCheckpoint;
        lapHandler = FindObjectOfType<LapHandler>();
        Destination = lapHandler.Checkpoints[0].GetRandomDestination();
    }

    void OnDestroy()
    {
        progressHandler.OnBotFinishedLap -= HandleBotFinishedLap;
        progressHandler.OnBotCrossedCheckpoint -= HandleBotCrossedCheckpoint;
    }

    void HandleBotFinishedLap(CarBot bot)
    {
        var checkpoints = lapHandler.Checkpoints;
        Destination = checkpoints[0].GetRandomDestination();
    }

    void HandleBotCrossedCheckpoint(CarBot bot, int checkpointIndex)
    {
        var checkpoints = lapHandler.Checkpoints;
        Destination = lapHandler.FinalCheckpoint == checkpointIndex ?
            lapHandler.gameObject.transform.position :
            checkpoints[checkpointIndex + 1].GetRandomDestination();
    }

    void LateUpdate()
    {
        BotMove();
        base.LateUpdate();
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

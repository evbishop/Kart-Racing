using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] Transform sphere;
    [SerializeField] float forwardAccel = 8f, reverseAccel = 4f, turnStrength = 180f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] CarGroundChecker groundChecker;

    [Header("Bots:")]
    [SerializeField] float botTurnStrength = 5f;
    [SerializeField] float randomOffset = 4f;

    public Vector3 Destination { get; set; }

    public float RandomOffset { get { return randomOffset; } }

    public float TurnInput { get; private set; }

    public float EmissionRate { get; private set; }

    public float Speed { get; private set; }

    public bool IsPlayer { get { return isPlayer; } }

    public int CurrentCheckpoint { get; set; }

    public int CurrentLap { get; set; }

    void Start()
    {
        audioSource.volume = 0;
        CurrentCheckpoint = -1;
        CurrentLap = 1;
        if (!isPlayer)
        {
            Destination = FindObjectOfType<LapHandle>().Checkpoints[0].gameObject.transform.position;
            Destination = new Vector3(Destination.x + Random.Range(-randomOffset, randomOffset),
                Destination.y, Destination.z);
        }
    }
    
    void Update()
    {
        if (isPlayer) PlayerMove();
        else BotMove();
        transform.position = sphere.position;
    }

    void PlayerMove()
    {
        if (Time.timeScale == 0) audioSource.volume = 0;

        if (!groundChecker.OnGround)
        {
            if (audioSource.volume > 0.1) audioSource.volume -= Time.deltaTime;
            EmissionRate = 0f;
        }

        float speedInput = Input.GetAxis("Vertical");
        if (speedInput > 0)
        {
            Speed = speedInput * forwardAccel * 1000f;
            if (audioSource.volume < 1) audioSource.volume += Time.deltaTime;
            if (groundChecker.OnGround) EmissionRate = 1f;
        }
        else if (speedInput < 0)
        {
            Speed = speedInput * reverseAccel * 1000f;
            if (audioSource.volume < 0.5) audioSource.volume += Time.deltaTime;
            else audioSource.volume -= Time.deltaTime;
            if (groundChecker.OnGround) EmissionRate = 0.5f;
        }
        else if (groundChecker.OnGround)
        {
            EmissionRate = 0f;
            if (audioSource.volume > 0.1) audioSource.volume -= 2 * Time.deltaTime;
        }

        TurnInput = Input.GetAxis("Horizontal");
        if (groundChecker.OnGround) transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3
            (0f, TurnInput * turnStrength * Time.deltaTime * speedInput, 0f));
    }

    void BotMove()
    {
        Speed = forwardAccel * 1000f;

        if (groundChecker.OnGround)
        {
            EmissionRate = 1f;
        }
        else
        {
            EmissionRate = 0f;
        }

        Vector3 relativePos = Destination - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePos);
        float randomTurnStrength = Random.Range(botTurnStrength, botTurnStrength * 2);

        Vector3 oldRotation = transform.rotation.eulerAngles;

        if (groundChecker.OnGround) transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * randomTurnStrength);

        if ((oldRotation - transform.rotation.eulerAngles).y > 0.05f) TurnInput = Mathf.Lerp(TurnInput, -1, Time.deltaTime * 10);
        else if ((oldRotation - transform.rotation.eulerAngles).y < -0.05f) TurnInput = Mathf.Lerp(TurnInput, 1, Time.deltaTime * 10);
        else TurnInput = Mathf.Lerp(TurnInput, 0, Time.deltaTime * 10);

        //transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
    }
}

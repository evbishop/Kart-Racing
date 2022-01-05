using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] Rigidbody sphere;
    [SerializeField] float forwardAccel = 8f, reverseAccel = 4f, turnStrength = 180f,
        gravityForce = 10f, groundRayLength = 0.5f, dragOnGround = 3f, dragInAir = 0.1f;
    [SerializeField] Transform groundRayPoint;
    [SerializeField] LayerMask ground;
    AudioSource audioSource;
    float speed, speedInput, turnInput;
    bool onGround;

    [Header("Wheels:")]
    [SerializeField] float maxWheelTurn = 25f;
    [SerializeField] Transform leftFrontWheel;
    [SerializeField] Transform rightFrontWheel;

    [Header("Dust:")]
    [SerializeField] float maxEmission = 30f;
    [SerializeField] ParticleSystem[] dustParticles;
    float emissionRate;

    [Header("Bots:")]
    [SerializeField] float botTurnStrength = 5f;
    [SerializeField] float randomOffset = 4f;

    public Vector3 Destination { get; set; }

    public float RandomOffset { get { return randomOffset; } }

    public bool IsPlayer { get { return isPlayer; } }

    public int CurrentCheckpoint { get; set; }

    public int CurrentLap { get; set; }

    void Start()
    {
        sphere.transform.parent = null;
        audioSource = GetComponent<AudioSource>();
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

        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x,
            turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x,
            turnInput * maxWheelTurn - 180, leftFrontWheel.localRotation.eulerAngles.z);

        foreach (ParticleSystem particle in dustParticles)
        {
            var emissionModule = particle.emission;
            emissionModule.rateOverTime = emissionRate;
        }
        transform.position = sphere.transform.position;
    }

    void PlayerMove()
    {
        if (Time.timeScale == 0) audioSource.volume = 0;

        RaycastHit hit;
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, ground))
        {
            onGround = true;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        else onGround = false;

        speedInput = Input.GetAxis("Vertical");
        if (speedInput > 0)
        {
            speed = speedInput * forwardAccel * 1000f;
            if (audioSource.volume < 1) audioSource.volume += Time.deltaTime;
            emissionRate = maxEmission;
        }
        else if (speedInput < 0)
        {
            speed = speedInput * reverseAccel * 1000f;
            if (audioSource.volume < 0.5) audioSource.volume += Time.deltaTime;
            else audioSource.volume -= Time.deltaTime;
            emissionRate = maxEmission / 2;
        }
        else
        {
            if (onGround && audioSource.volume > 0.1) audioSource.volume -= 2 * Time.deltaTime;
            emissionRate = 0;
        }
        if (!onGround)
        {
            if (audioSource.volume > 0.1) audioSource.volume -= Time.deltaTime;
            emissionRate = 0;
        }

        turnInput = Input.GetAxis("Horizontal");
        if (onGround) transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3
            (0f, turnInput * turnStrength * Time.deltaTime * speedInput, 0f));
    }

    void BotMove()
    {
        speed = forwardAccel * 1000f;

        RaycastHit hit;
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, ground))
        {
            onGround = true;
            emissionRate = maxEmission;
        }
        else
        {
            onGround = false;
            emissionRate = 0;
        }

        Vector3 relativePos = Destination - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePos);
        float randomTurnStrength = Random.Range(botTurnStrength, botTurnStrength * 2);

        Vector3 oldRotation = transform.rotation.eulerAngles;

        if (onGround) transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * randomTurnStrength);

        if ((oldRotation - transform.rotation.eulerAngles).y > 0.05f) turnInput = Mathf.Lerp(turnInput, -1, Time.deltaTime * 10);
        else if ((oldRotation - transform.rotation.eulerAngles).y < -0.05f) turnInput = Mathf.Lerp(turnInput, 1, Time.deltaTime * 10);
        else turnInput = Mathf.Lerp(turnInput, 0, Time.deltaTime * 10);

        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
    }

    void FixedUpdate()
    {
        if (onGround)
        {
            sphere.drag = dragOnGround;
            if (Mathf.Abs(speed) > 0)
            {
                sphere.AddForce(transform.forward * speed);
            }
        }
        else
        {
            sphere.drag = dragInAir;
            sphere.AddForce(Vector3.down * gravityForce * 100f);
        }
    }
}

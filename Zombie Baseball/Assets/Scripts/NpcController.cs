using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [Header("Seeding Settings")]
    public Vector2 speedRange;
    public Vector2 randomTurningRange;
    public float zombieChance = 0.5f;
    public float smoothTime = 1f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody rb;

    private float randomTurning;
    private float speed = 0.3f;
    private Vector3 travelDirection;
    private bool infected = false;
    private bool agitaged = false;
    private ZombieController target = null;

    private Vector3 travelVelocity;

    [Header("Collision Settings")]
    public Vector3 raycastOffset;
    public float raycastLength;

    private float internalTimer;
    private float unagitatedTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        seedZombie();
    }

    private void Update()
    {
        if (infected)
        {
            if (target != null)
            {
                setTravelDirection(target.transform.position - transform.position);
                rb.MovePosition(rb.position + travelDirection * speed * Time.deltaTime);
            }
        }
        else
        {
            if (target != null)
            {
                setTravelDirection(transform.position - target.transform.position);
                rb.MovePosition(rb.position + travelDirection * speed * Time.deltaTime);
            }
        }
        // rb.velocity = rb.velocity.normalized * speed;

        //randomWaltz();
    }

    void randomWaltz()
    {
        if (!agitaged)
        {
            internalTimer += Time.deltaTime;
            if (internalTimer > unagitatedTimer)
            {
                internalTimer = 0;
                setTravelDirection((new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f))).normalized);
            }
        }
    }


    void seedZombie()
    {
        seedTravelDirection();

        speed = Random.Range(speedRange.x, speedRange.y);
        randomTurning = Random.Range(randomTurningRange.x, randomTurningRange.y);
        unagitatedTimer = Random.Range(0.9f, 5f);

        setZombie((Random.Range(0f, 1f) < zombieChance) ? (true) : (false));
    }

    void seedTravelDirection()
    {
        travelDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        travelDirection.Normalize();
    }

    public bool isZombie()
    {
        return infected;
    }

    public void setZombie(bool isInfected)
    {
        infected = isInfected;

        if (infected)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.green;
        }
    }
    public void setAgitated(bool isAgitated)
    {
        agitaged = isAgitated;
    }

    public void setTravelDirection(Vector3 inTravelDirection)
    {
        travelDirection = Vector3.SmoothDamp(travelDirection, inTravelDirection.normalized, ref travelVelocity, smoothTime);
    }

    public void setTarget(ZombieController inTarget)
    {
        target = inTarget;
    }
}

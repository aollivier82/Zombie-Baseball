using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    enum MovementType
    {
        Walking,
        Dashing
    }

    [Header("Walking Settings")]
    public Vector2 speed;
    public float slowFactor = 2.0f;
    public float slowRate = 0.5f;
    public Vector2 maxSpeed;

    private bool isSprinting = false;
    private float _speed;
    private float _maxSpeed;

    [Header("Dash Settings")]
    public float dashMultiplier = 6f;
    public float slowFactorDash = 2.0f;
    public float walkSpeedThreshold = 0.5f;
    public float dashLengthInSecs = 1.0f;
    public AnimationCurve dashCurve;
    public ParticleSystem particleSystem;

    private int consecutiveBounces;
    private Vector3 dashVelo;
    private Vector3 dashDirection;
    private float dashTimer;

    [Header("Misc Settings")]
    public GameObject art;

    private Rigidbody rb;
    private Vector3 desiredMovement = new Vector3();
    private bool isSlowed = false;
    private Vector3 lastFrameMovement;
    private MovementType movementType;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementType = MovementType.Walking;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFacingDirection();
        UpdatingSprintingValues();

        isSprinting = Input.GetKeyDown(KeyCode.LeftShift);

        //Calculate movement
        if(MovementType.Walking == movementType)
        {
            desiredMovement.x = Input.GetAxisRaw("Horizontal");
            desiredMovement.z = Input.GetAxisRaw("Vertical");
            desiredMovement.Normalize();

            rb.AddForce(desiredMovement * _speed * Time.deltaTime * ((isSlowed) ? (slowRate) : (1)));
            Vector3 newVelocity = rb.velocity;
            if (desiredMovement.x == 0)
            {
                newVelocity.x = Mathf.Lerp(newVelocity.x, 0, Time.deltaTime * slowFactor);
            }
            if (desiredMovement.z == 0)
            {
                newVelocity.z = Mathf.Lerp(newVelocity.z, 0, Time.deltaTime * slowFactor);
            }
            rb.velocity = Vector3.ClampMagnitude(newVelocity, (_maxSpeed) * ((isSlowed) ? (slowRate) : (1)));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Dash();
            }
        }
        if(MovementType.Dashing == movementType)
        {
            dashTimer += Time.deltaTime;

            rb.velocity = dashCurve.Evaluate(dashTimer / dashLengthInSecs) * dashDirection;

            if(rb.velocity.magnitude < walkSpeedThreshold)
            {
                movementType = MovementType.Walking;
                consecutiveBounces = 0;

                SetEmissionRate(0);
            }
        }

        lastFrameMovement = rb.velocity;
    }

    private void SetEmissionRate(float value)
    {
        var emission = particleSystem.emission;
        emission.rateOverTime = 0;
    }

    private void UpdatingSprintingValues()
    {
        isSprinting = !Input.GetKey(KeyCode.LeftShift);
        _maxSpeed = (isSprinting) ? (maxSpeed.y) : (maxSpeed.x);
        _speed = (isSprinting) ? (speed.y) : (speed.x);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (MovementType.Dashing == movementType)
        {
            foreach (var item in collision.contacts)
            {
                Vector3 normalNoY = item.normal;
                normalNoY.y = 0;
                normalNoY.Normalize();

                desiredMovement = lastFrameMovement.normalized - 2 * (Vector3.Dot(lastFrameMovement.normalized, normalNoY)) * normalNoY;

                Dash();
            }
        }
    }

    private void Dash()
    {
        SetEmissionRate(20f);
        dashVelo = Vector3.zero;
        dashTimer = 0;
        dashDirection = dashMultiplier * _maxSpeed * desiredMovement / ((consecutiveBounces > 0) ? (consecutiveBounces) : (1));
        movementType = MovementType.Dashing;

        consecutiveBounces++;
    }

    private void UpdateFacingDirection()
    {
        Vector3 trans = art.transform.localScale;
        trans.x = Mathf.Sign(rb.velocity.x) * Mathf.Abs(trans.x);
        art.transform.localScale = trans;
    }
}

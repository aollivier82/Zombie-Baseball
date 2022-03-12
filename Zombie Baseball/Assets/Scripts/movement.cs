using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10f;
    bool hasHandledInputThisFrame = false;
    public float dash_force = 1000f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }
    }

    void FixedUpdate()
    {
        // Capture analog stick direction (yes, this works fine in FixedUpdate).
        Vector3 analog = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.AddForce(analog * speed * Time.deltaTime,ForceMode.Acceleration);
    }
    void Dash()
    {
        Vector3 current_direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))*4;
        rb.AddForce(current_direction * dash_force,ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall"){
            Vector3 opposite_dir = -rb.velocity;
            rb.AddForce(opposite_dir * dash_force,ForceMode.Impulse);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackPlayer : MonoBehaviour
{
    public Transform player;

    public float smoothTime = 0.2f;
    public Vector3 cameraOffset;
    private Vector3 cameraVelocity;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + cameraOffset, ref cameraVelocity, smoothTime);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(0);
        }
    }
}

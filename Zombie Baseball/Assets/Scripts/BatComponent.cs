using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatComponent : MonoBehaviour
{
    public AnimationCurve batSpeedCurve;
    public GameObject bat;
    private Camera cam;
    public float strikeForce;
    private bool swinging;
    private float timer;
    public float animationSpeed;
    public float animationScale;
    private float startYRotation;
    public float swingTimeBeforeDamage;
    
    private AudioSource sound;
    public AudioClip swing;
    public AudioClip whack;


    private void Start()
    {
        cam = Camera.main;
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (swinging)
        {
            timer += Time.deltaTime * animationSpeed;
            if (timer > 1) 
            {
                swinging = false;
                bat.SetActive(false);
                timer = 0f;
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, startYRotation + batSpeedCurve.Evaluate(timer)*animationScale, transform.rotation.z);
            }
        }
        if (Input.GetMouseButtonDown(0) && !swinging)
        {
            Swing();
        }
        

        if (swinging && timer > swingTimeBeforeDamage)
        {
            CheckForCollisions();
        }
    }

    void CheckForCollisions()
    {
        Collider[] colliders = Physics.OverlapSphere(bat.transform.position, 1.3f);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Zombie" || c.gameObject.tag == "Survivor")
            {
                sound.Stop();
                sound.clip = whack;
                sound.Play();
                BaseAgent ba = c.gameObject.GetComponentInChildren<BaseAgent>();
                Vector3 relative = (c.transform.position - transform.position) * strikeForce;
                relative.y = 0;
                ba.HitRagdoll(relative);
            }
        }
    }

    void Swing()
    {
        sound.clip = swing;
        sound.Play();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float angle = 0;
        if (Physics.Raycast(ray, out hit) && swinging == false)
        {
            Vector3 target = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Debug.DrawLine(transform.position, target);
            Vector3 reference_angle = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
            angle = Vector3.SignedAngle(Vector3.right, target - transform.position, Vector3.up);
            Debug.DrawLine(transform.position, reference_angle);
        }

        timer = 0f;
        bat.SetActive(true);
        swinging = true;
        startYRotation = angle;
    }
}

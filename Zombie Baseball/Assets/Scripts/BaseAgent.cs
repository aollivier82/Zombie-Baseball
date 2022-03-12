using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseAgent : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;
    protected Rigidbody rb;

    protected bool isRagdoll;
    protected float minRagdollSpeed = 0.5f;
    private Vector3 lastPosition;

    protected virtual void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
    }

    protected virtual void EarlyUpdate()
    {
        if (isRagdoll)
        {
            if(Vector3.Distance(transform.position, lastPosition) < minRagdollSpeed)
            {
                EnableRagdoll(false);
            }
        };
        lastPosition = transform.position;
    }

    public void HitRagdoll(Vector3 force)
    {
        force.y = 0;
        EnableRagdoll(true);
        rb.AddForce(force);
        rb.velocity = force;
    }

    private void EnableRagdoll(bool _isRagdoll)
    {
        isRagdoll = _isRagdoll;
        rb.isKinematic = !_isRagdoll;
        navMeshAgent.enabled = !_isRagdoll;
    }


    private void OnCollisionEnter(Collision other)
    {
        //if (other.gameObject.tag == "Zombie" || other.gameObject.tag == "Survivor")
        //{
        //    BaseAgent ba = other.gameObject.GetComponent<BaseAgent>();
        //    Vector3 force = (other.transform.position - transform.position) * rb.velocity.magnitude/2;
        //    ba.HitRagdoll(force);
        //}
        if (other.gameObject.tag != "Floor")
        {
            EnableRagdoll(false);
        }
    }
}

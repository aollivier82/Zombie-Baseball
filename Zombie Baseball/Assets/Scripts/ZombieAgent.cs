using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAgent : BaseAgent
{
    private GameObject target;
    private GameObject player;
    private FoodGenerator hunger;

    public float attackRange = 1;
    public float dmg = 0.5f;

    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
        hunger = GameObject.Find("FoodGenerator").GetComponent<FoodGenerator>();
    }

    protected override void EarlyUpdate()
    {
        base.EarlyUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRagdoll)
        {
            return;
        }

        if (target != null)
        {
            navMeshAgent.SetDestination(target.transform.position);
        }

        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist < attackRange)
        {
            Eat(player);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
    //Todo when hit zombie, disable the navmeshAgent until rigidbody is done doing its thing


    public void Eat(GameObject player)
    {
        hunger.Hurt(dmg);
    }
}

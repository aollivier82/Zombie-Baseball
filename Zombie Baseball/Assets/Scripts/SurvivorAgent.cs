using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SurvivorAgent : BaseAgent
{
    public enum SurvivorState
    {
        SeekingFood,
        CircleFood,
        FightingPlayer,
    }

    public enum FightingState
    {
        Approach,
        Wary,
        Swing
    }

    public GameObject food;
    public SurvivorState survivorState;

    [Header("Search state")]
    public float minFoodDistance = 0.5f;

    [Header("Circle state")]
    public float timePerRotation = 2.0f;
    public float rotations = 2f;
    public float distance = 1.0f;
    private float timer = 0;

    [Header("Fighting state")]
    public FightingState fightingState;
    private float combatTimer = 0;
    private GameObject player;

    protected override void Start()
    {
        base.Start();
        survivorState = SurvivorState.SeekingFood;
        player = GameObject.FindGameObjectWithTag("Player");
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

        if(SurvivorState.SeekingFood == survivorState)
        {
            if (food == null)
            {
                FindNearestFood();
            }

            if (Vector3.Distance(food.transform.position, transform.position) < minFoodDistance)
            {
                survivorState = SurvivorState.CircleFood;
                timer = 0f;
            }
            CheckForPlayer();
        }
        else if(SurvivorState.CircleFood == survivorState)
        {
            timer += Time.deltaTime / timePerRotation;
            navMeshAgent.SetDestination(food.transform.position + Mathf.Cos(2 * Mathf.PI * timer) * Vector3.forward + Mathf.Sin(2 * Mathf.PI * timer) * Vector3.right);
            if(timer > rotations)
            {
                food = null;
                survivorState = SurvivorState.SeekingFood;
            }
            CheckForPlayer();
        }
        else if(SurvivorState.FightingPlayer == survivorState)
        {
            timer += Time.deltaTime / timePerRotation;
            navMeshAgent.SetDestination(food.transform.position + Mathf.Cos(2 * Mathf.PI * timer) * Vector3.forward + Mathf.Sin(2 * Mathf.PI * timer) * Vector3.right);
            if (timer > rotations)
            {
                food = null;
                survivorState = SurvivorState.SeekingFood;
            }

            if(FightingState.Approach == fightingState)
            {

            }
            else if (FightingState.Wary == fightingState)
            {

            }
            else if (FightingState.Swing == fightingState)
            {
                //Todo
            }
        }




        //Check for death
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, 1 << LayerMask.NameToLayer("Zombie"));
        if (colliders.Length > 0)
        {
            KillSurvivor();
        }

    }

    void FindNearestFood()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");

        if (foods != null && foods.Length > 0)
        {
            food = foods[Random.Range(0, foods.Length)];
            navMeshAgent.SetDestination(food.transform.position);
        }
    }

    void CheckForPlayer()
    {
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, Mathf.Infinity))
        //{
        //    if(hit.rigidbody.gameObject.tag == "Player")
        //    {
        //        fightingState = FightingState.Wary;
        //    }
        //}
    }

    void KillSurvivor()
    {
        transform.position = new Vector3(Random.Range(-9f, 9f), 0, Random.Range(-9f, 9f));
    }
}

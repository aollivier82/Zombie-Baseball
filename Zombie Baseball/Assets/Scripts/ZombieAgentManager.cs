using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAgentManager : MonoBehaviour
{
    List<List<ZombieAgent>> zombieBatches;
    List<SurvivorAgent> survivors;
    
    [Header("Prefabs")]
    public GameObject zombiePrefab;
    public GameObject survivorPrefab;

    [Header("Spawning")]
    public int batches = 7;
    public Vector2 batchMaxMinSize = new Vector2(4, 10);
    public int numSurvivors = 4;

    private GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        survivors = new List<SurvivorAgent>();
        for (int i = 0; i < numSurvivors; i++)
        {
            SurvivorAgent survivor = Instantiate(survivorPrefab, new Vector3(Random.Range(-45f, 45f), 0, Random.Range(-45f, 45f)), Quaternion.identity).GetComponent<SurvivorAgent>();
            survivors.Add(survivor);
        }

        zombieBatches = new List<List<ZombieAgent>>();
        for(int i = 0; i < batches; i++)
        {
            zombieBatches.Add(new List<ZombieAgent>());
            int batchSize = Random.Range((int)batchMaxMinSize.x, (int)batchMaxMinSize.y + 1);

            for(int j = 0; j < batchSize; j++)
            {
                ZombieAgent zombie = Instantiate(zombiePrefab, new Vector3(Random.Range(-45f, 45f), 0, Random.Range(-45f, 45f)), Quaternion.identity).GetComponent<ZombieAgent>();
                zombieBatches[i].Add(zombie);
            }

            StartCoroutine("UpdateBatch", i);
        }
    }

    IEnumerator UpdateBatch(int batch)
    {
        float minDist = Mathf.Infinity;
        GameObject target = null;
        float dist;
        foreach (SurvivorAgent survivor in survivors)
        {
            dist = Vector3.Distance(survivor.transform.position, zombieBatches[batch][0].transform.position);
            if(dist < minDist)
            {
                minDist = dist;
                target = survivor.gameObject;
            }
        }
        dist = Vector3.Distance(player.transform.position, zombieBatches[batch][0].transform.position);
        if (dist < minDist)
        {
            minDist = dist;
            target = player;
        }

        foreach (ZombieAgent zombie in zombieBatches[batch])
        {
            zombie.SetTarget(target);
        }

        yield return new WaitForSeconds(Random.Range(0.2f, 1.2f));
    }

    IEnumerator ZombieBatchRandomization()
    {
        for(int i = 0; i < batches * batchMaxMinSize.x / 2; i++)
        {
            //Select two random zombies from all batches and trade their batches
            int batchOne = Random.Range(0, batches);
            int batchTwo = Random.Range(0, batches);

            int indexOne = Random.Range(0, zombieBatches[batchOne].Count);
            int indexTwo = Random.Range(0, zombieBatches[batchTwo].Count);

            ZombieAgent zombieOne = zombieBatches[batchOne][indexOne];
            zombieBatches[batchOne][indexOne] = zombieBatches[batchTwo][indexTwo];
            zombieBatches[batchTwo][indexTwo] = zombieOne;
        }

        yield return new WaitForSeconds(Random.Range(0.2f, 1.2f));
    }
}

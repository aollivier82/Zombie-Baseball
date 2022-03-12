using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHordeManager : MonoBehaviour
{
    public List<ZombieController> zombies = new List<ZombieController>();
    private List<ZombieController> npcs = new List<ZombieController>();
    private List<ZombieController> deadPool = new List<ZombieController>();
    public GameObject zombiePrefab;

    public int numZombies = 50;
    public float conversionDist = 0.1f;
    public float minZombieNoticeDist = 5f;

    private bool canKill = false;

    private void Start()
    {
        for(int i = 0; i < numZombies; i++)
        {
            summonZombie();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(0);
        }

        if (zombies.Count > 0 && npcs.Count > 0)
        {
            //Update zombie targets
            foreach (ZombieController zombie in zombies)
            {
                float minDist = Mathf.Infinity;
                ZombieController nearestNpc = null;

                foreach(ZombieController npc in npcs)
                {
                    float currDist = Vector3.Distance(zombie.transform.position, npc.transform.position);
                    if(currDist < minDist)
                    {
                        minDist = currDist;
                        nearestNpc = npc;
                    }
                }

                zombie.setTarget(nearestNpc);
            }

            List<ZombieController> npcsToConvert = new List<ZombieController>();
           
            foreach (ZombieController npc in npcs)
            {
                float minDist = Mathf.Infinity;
                ZombieController nearestZombie = null;

                foreach (ZombieController zombie in zombies)
                {
                    float currDist = Vector3.Distance(npc.transform.position, zombie.transform.position);
                    if (currDist < minDist)
                    {
                        minDist = currDist;
                        nearestZombie = zombie;
                    }
                }
                if(minDist < conversionDist)
                {
                    npcsToConvert.Add(npc);
                }

                npc.setTarget(nearestZombie);
            }
            convertBatch(npcsToConvert);
        }

        //Kill zombies that are in the circle
        if (canKill)
        {
          for (int x = 0; x < deadPool.Count; x++)
            {
                ZombieController controller = deadPool[x];
                zombies.Remove(controller);
                controller.die();
            }
            deadPool.Clear();
            canKill = false;
        }
      
    }


    void convertBatch(List<ZombieController> npcsToConvert)
    {
        foreach(ZombieController npc in npcsToConvert)
        {
            npc.setZombie(true);
            zombies.Add(npc);
            npcs.Remove(npc);
        }
    }
    void summonZombie()
    {
        ZombieController zombie = Instantiate(zombiePrefab, new Vector3(Random.Range(-18f, 18f), 0, Random.Range(-38f, 18f)), Quaternion.identity).GetComponent<ZombieController>();
        if (zombie.isZombie())
        {
            zombies.Add(zombie);
            zombie.setZombie(true);
        }
        else
        {
            npcs.Add(zombie);
            zombie.setZombie(false);
        }
    }

    public void quarantineZombie (ZombieController zombie) 
    {
        deadPool.Add(zombie);
    }

    public void killZombies()
    {
        canKill = true;
    }

    //Spawn zombies first

    //Update thread will constantly just check zombies that are nearby other zombies and infect them. 
    //Zombies movement variables will also be updated constantly by this thread to boid them
    //NPCs will try to run away from zombies. 
    //Zombies will try to run towards NPCs

    //If NPC is within x range of zombie turn him into zombie. 
}

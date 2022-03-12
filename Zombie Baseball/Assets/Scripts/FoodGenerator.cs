using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodGenerator : MonoBehaviour
{
    public GameObject foodPrefab;
    public int foodCount = 100;
    public bool randomGeneration = false;
    public List<Food> foodList;
    private Manager manager;

    public Slider slider;
    public int totalHunger = 50;
    float currentHunger;
    public float respawnTime = 3f;
    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        foodList = new List<Food>();
        currentHunger = totalHunger;

        if (randomGeneration)
        {
            for (int x = 0; x  < foodCount; x++)
            {
                Food food = Instantiate(foodPrefab, new Vector3(Random.Range(-90f, 90f), 0, Random.Range(-90f, 90f)), Quaternion.identity).GetComponent<Food>();
                    food.transform.Rotate(new Vector3(90, 0, 0));
            }

        }

        GameObject[] foodObjects = GameObject.FindGameObjectsWithTag("Food");

            for (int x = 0; x < foodObjects.Length; x++)
            {
                foodList.Add(foodObjects[x].GetComponent<Food>());  
            }


        
    }

    void Update()
    {
        currentHunger -= Time.deltaTime;


        if (currentHunger <= 0)
        {
            manager.EndGame();
        }

        slider.value = currentHunger / totalHunger;
    }

    public void Eat(Food food)
    {
        currentHunger += 10;
        if (currentHunger > totalHunger) { currentHunger = totalHunger; }

        audio.Play();
        StartCoroutine(Respawn(food));
        food.gameObject.SetActive(false);


    }

    IEnumerator Respawn(Food food)
    {
        yield return new WaitForSeconds(respawnTime);
        food.gameObject.SetActive(true);
    }

    public void Hurt(float dmg)
    {
        currentHunger -= dmg;
        if (currentHunger < 0)
        {
            //End Game
        }
    }

}

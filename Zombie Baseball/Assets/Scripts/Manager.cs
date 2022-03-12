using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    private Text timer;
    private Text score;
    private GameObject healthUI;
    private GameObject respawnMenu;
    private float time;
    private bool isDead = false;
    private PlayerController player;

    [SerializeField]
    private GameObject[] spawnPoints;


    // Start is called before the first frame update
    void Start()
    {
        healthUI = GameObject.Find("HealthUI");
        timer = GameObject.Find("Timer").GetComponent<Text>();
        score = GameObject.Find("Score").GetComponent<Text>();
        respawnMenu = GameObject.Find("RespawnMenu");
        respawnMenu.SetActive(false);
        
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        //Set position to a random point from an array
        int spawn = Random.Range(0, spawnPoints.Length);
        player.transform.position = spawnPoints[spawn].transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            time += Time.deltaTime;
            timer.text = time.ToString("0.00");

        }

        if (isDead)
        {
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene("Main Level");
            }
        }
    }


    public void EndGame()
    {
        isDead = true;
        healthUI.SetActive(false);
        respawnMenu.SetActive(true);
        score.text = time.ToString("0.00" + " seconds.");
        player.gameObject.SetActive(false);
    }
}

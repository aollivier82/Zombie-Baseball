using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initialize_npc : MonoBehaviour
{
    [SerializeField] private List<GameObject> maskList;
    [SerializeField] private List<GameObject> hair;
    [SerializeField] private List<GameObject> drip;
    [SerializeField] private List<GameObject> skin_color;
    [SerializeField] private GameObject symptoms;

    // Start is called before the first frame update
    void Start()
    {
        //Randomly select sprites for each part 
        GameObject selected_mask = maskList[Random.Range(0, maskList.Count)];
        GameObject selected_hair = hair[Random.Range(0, hair.Count)];
        GameObject selected_drip = drip[Random.Range(0, drip.Count)];
        GameObject selected_skin_color = skin_color[Random.Range(0, skin_color.Count)];

        //Instatiate each part as child of object
        //NEED TO SPAWN AS CHILDREN
        Instantiate(selected_mask, transform);
        Instantiate(selected_hair, transform);
        Instantiate(selected_drip, transform);
        Instantiate(selected_skin_color, transform);
        //zombify(false);
    }

    public void zombify(bool isInfected)
    {
        symptoms.SetActive(isInfected);
    }
}

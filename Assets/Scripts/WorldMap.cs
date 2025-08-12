using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    public GameObject worldMap;
    
    

    void Start()
    {
        worldMap.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (worldMap.activeInHierarchy)
            {
                worldMap.SetActive(false);
            }
            else
            {
                worldMap.SetActive(true);
            }
        }
    }
}
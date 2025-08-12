using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public RenderTexture miniMap1;
    public RenderTexture miniMap2;
    public RenderTexture miniMap3;
    public RenderTexture miniMap4;

    public GameObject playerCube1;
    public GameObject playerCube2;
    public GameObject playerCube3;
    public GameObject playerCube4;

    public bool isTown;
    public bool isForest;
    public bool isGorge;
    public bool isRuin;

    public RawImage rawImage;

    void Start()
    {
        isTown = true;
        isForest = false;
        isGorge = false;
        isRuin = false;
        
        playerCube1.SetActive(false);
        playerCube2.SetActive(false);
        playerCube3.SetActive(false);
        playerCube4.SetActive(false);
    }

    void Update()
    {
        if (isTown)
        {
            playerCube1.SetActive(false);
            playerCube2.SetActive(false);
            playerCube3.SetActive(false);
            playerCube4.SetActive(false);
            rawImage.texture = miniMap1;
            playerCube1.SetActive(true);
        }

        else if (isForest)
        {
            playerCube1.SetActive(false);
            playerCube2.SetActive(false);
            playerCube3.SetActive(false);
            playerCube4.SetActive(false);
            rawImage.texture = miniMap2;
            playerCube2.SetActive(true);
        }

        else if (isRuin)
        {
            playerCube1.SetActive(false);
            playerCube2.SetActive(false);
            playerCube3.SetActive(false);
            playerCube4.SetActive(false);
            rawImage.texture = miniMap3;
            playerCube3.SetActive(true);
        }

        else if (isGorge)
        {
            playerCube1.SetActive(false);
            playerCube2.SetActive(false);
            playerCube3.SetActive(false);
            playerCube4.SetActive(false);
            rawImage.texture = miniMap4;
            playerCube4.SetActive(true);
        }
    }
}

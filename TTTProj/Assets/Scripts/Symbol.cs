using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Symbol : MonoBehaviour
{
    public Button button;
    public Image image;
    public string GridNum;

    private string P1key;
    private string P2key;

    private GManager Gm;
    private AIGManager AIGM;
    public void SetSymbolInGrid()
    {
        image.gameObject.SetActive(true);
        image.sprite = Gm.GetPlayerSprite();
        if(Gm.GetPlayerTurn() == 1)
        {
            SavePlayerMove(P1key);
            PlayerPrefs.Save();
        }
        else
        {
            SavePlayerMove(P2key);
            PlayerPrefs.Save();
        }
        button.interactable = false;
        Gm.ChangeTurn();
    }

    public void SetSymbolInGrid2()
    {
        if (AIGM.GetPlayerTurn())
        {
            image.gameObject.SetActive(true);
            image.sprite = AIGM.GetPlayerSprite();
            SavePlayerMove(P1key);
            PlayerPrefs.Save();
            button.interactable = false;
            AIGM.SetPlayerTurn(false);
            AIGM.ChangeTurn();
        }
    }

    public void SetReference(GManager gManager)
    {
        Gm = gManager;
    }

    public void SetReference(AIGManager gmanager)
    {
        AIGM = gmanager;
    }

    public void SavePlayerMove(string key)
    {
        int temp;
        temp = PlayerPrefs.GetInt(key);
        PlayerPrefs.SetInt(key, ++temp);
    }

    void Start()
    {
        P1key = "Player1: Grid " + GridNum + "-> ";
        P2key = "Player2: Grid " + GridNum + "-> ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

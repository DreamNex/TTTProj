using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    public Sprite player1;
    public Sprite player2;
    public bool PlayComputer;


    private int gamemode; //Set scene
    private int countm;

    public void IncrementCount()
    {
        countm++;
    }
    public int GetCountM()
    {
        return countm;
    }
    public void ResetCount()
    {
        countm = 0;
    }
    private void Start()
    {
        countm = 0;
        gamemode = 0;   
        PlayComputer = false;
        
    }

    //Set Functions
    public void SetPlayer1Img(Sprite sprite)
    {
        player1 = sprite;
    }
    public void SetPlayer2Img(Sprite sprite)
    {
        player2 = sprite;        
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameSceneManager>().SetScene(gamemode);
    }
    public void SetGameMode(int index)
    {
        gamemode = index;
    }
    //Get Functions
    public Sprite GetPlayer1Img()
    {
        return player1;
    }
    public Sprite GetPlayer2Img()
    {
        return player2;
    }
    public int GetGamemode()
    {
        return gamemode;
    }
}

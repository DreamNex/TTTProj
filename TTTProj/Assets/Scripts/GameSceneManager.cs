using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    public GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetScene(string str)
    {
        SceneManager.LoadScene(str);
    }

    public void SetScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void SetSceneWithData(int gamemode)
    {
        go.GetComponent<GameData>().SetGameMode(gamemode);
        if(gamemode == 4)
        {
            go.GetComponent<GameData>().PlayComputer = true;
        }
    }

    public Scene GetCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }
}

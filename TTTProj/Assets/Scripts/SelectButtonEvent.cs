using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButtonEvent : MonoBehaviour
{
    private Button btn;
    private GameObject Data_obj;
    private GameObject SManager;
    private Sprite symbol1;
    private Sprite symbol2;
    private Sprite symbol3;
    private Sprite symbol4;

    public GameObject imge;
    // Start is called before the first frame update
    void Start()
    {
        SManager = GameObject.FindGameObjectWithTag("SceneManager");
        Data_obj = GameObject.FindGameObjectWithTag("GameData");

        symbol1 = Resources.Load<Sprite>("ui/symbol-chip");
        symbol2 = Resources.Load<Sprite>("ui/symbol-club");
        symbol3 = Resources.Load<Sprite>("ui/symbol-diamond");
        symbol4 = Resources.Load<Sprite>("ui/symbol-heart");

        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(ClickEvent);
    }

    void ClickEvent()
    {
        //Set the sprite to the game data
        if (!Data_obj.GetComponent<GameData>().PlayComputer)
        {
            if (Data_obj.GetComponent<GameData>().GetPlayer1Img() == null)
            {
                Data_obj.GetComponent<GameData>().SetPlayer1Img(imge.GetComponent<Image>().sprite);
            }
            else
            {
                Data_obj.GetComponent<GameData>().SetPlayer2Img(imge.GetComponent<Image>().sprite);
            }
            this.GetComponent<Button>().interactable = false;
        }
        else
        {
            if (Data_obj.GetComponent<GameData>().GetPlayer1Img() == null)
            {
                Data_obj.GetComponent<GameData>().SetPlayer1Img(imge.GetComponent<Image>().sprite);
                if(imge.GetComponent<Image>().sprite == symbol1)
                {
                    Data_obj.GetComponent<GameData>().SetPlayer2Img(symbol2);
                }
                else
                {
                    Data_obj.GetComponent<GameData>().SetPlayer2Img(symbol1);
                }
            }
        }
    }
}

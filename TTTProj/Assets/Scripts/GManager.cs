using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    public GameObject GOPanel;
    public GameObject IndicatorImg;
    public GameObject InText;

    private Image[,] buttons;
    private int row, column;
    public Image[] buttonlist;

    private GameObject StoredData;
    private GameObject SMref;
    private Sprite Sp1;
    private const string IndicatorText = "Turn: Player ";

    void Awake()
    {
        //Get references

        StoredData = GameObject.FindGameObjectWithTag("GameData");
        SMref = GameObject.FindGameObjectWithTag("SceneManager");

        if (StoredData.GetComponent<GameData>().GetGamemode() == 2)
        {
            row = 3;
            column = 3;
        }
        else if (StoredData.GetComponent<GameData>().GetGamemode() == 3)
        {
            row = 4;
            column = 4;
        }

        buttons = new Image[column, row];

        int x = 0;
        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                buttons[i, j] = buttonlist[x];
                ++x;
            }
        }


        //Set the symbols
        this.GetComponent<GameData>().player1 = StoredData.GetComponent<GameData>().GetPlayer1Img();
        this.GetComponent<GameData>().player2 = StoredData.GetComponent<GameData>().GetPlayer2Img();

        //Setup Game
        GOPanel.SetActive(false);
        SetReferenceOnButton();
        Sp1 = this.GetComponent<GameData>().player1;
        SetPlayerIndicator(IndicatorText + "1", Sp1);

    }
    //Set Functions
    void SetReferenceOnButton()
    {
        for(int i = 0; i < buttonlist.Length; i++)
        {
            buttonlist[i].GetComponentInParent<Symbol>().SetReference(this);
        }
    }
    public void SetSprite(Sprite sprite)
    {
        Sp1 = sprite;
    }
    void SetGameOverText(string stng)
    {
        GOPanel.SetActive(true);
        GOPanel.GetComponentInChildren<Text>().text = stng;
    }
    void SetBoardInteractable(bool bb)
    {
        for (int i = 0; i < buttonlist.Length; i++)
        {
            buttonlist[i].GetComponentInParent<Button>().interactable = bb;
        }
    }
    public void SetPlayerIndicator(string str, Sprite sprite)
    {
        InText.GetComponent<Text>().text = str;
        IndicatorImg.GetComponent<Image>().sprite = sprite;
    }

    //Get Functions
    public Sprite GetPlayerSprite()
    {
        return Sp1;
    }
    public int GetPlayerTurn()
    {
        if(this.GetComponent<GameData>().player1 == Sp1)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
   
    //Game Functions
    public void ChangeTurn()
    {
        this.GetComponent<GameData>().IncrementCount();

        //Check top left to bottom right
        if(CheckTopLeftBottomRight())
        {
            EndGame();
        }
        else if(CheckBottomLeftTopRight())
        {
            EndGame();
        }
        
        for(int i = row -1; i >= 0 ; i--)
        {
            if (CheckRow(i) == true)
            {
                EndGame();
            }
            else if(CheckCol(i) == true)
            {
                EndGame();
            }
            else if(this.GetComponent<GameData>().GetCountM() >= 9)
            {
                TieGame();
            }
        }

        NextTurn();
    }
    void NextTurn()
    {

        if (Sp1 == this.GetComponent<GameData>().player1)
        {
            Sp1 = this.GetComponent<GameData>().player2;
            SetPlayerIndicator(IndicatorText + "2", Sp1);
        }
        else if (Sp1 == this.GetComponent<GameData>().player2)
        {
            Sp1 = this.GetComponent<GameData>().player1;
            SetPlayerIndicator(IndicatorText + "1", Sp1);
        }

    }
    void EndGame()
    {
        SetBoardInteractable(false);
        //Gameover 
        if (Sp1 == this.GetComponent<GameData>().player1)
        {
            SetGameOverText("Player 1 Wins!");
        }
        else if (Sp1 == this.GetComponent<GameData>().player2)
        {
            SetGameOverText("Player 2 Wins!");
        }
    }
    void TieGame()
    {
        SetGameOverText("ITS A TIE");
    }
    public void ResetGame()
    {

        GOPanel.SetActive(false);
        Sp1 = this.GetComponent<GameData>().player1;
        SetPlayerIndicator(IndicatorText + "1", Sp1);
        this.GetComponent<GameData>().ResetCount();
        //Reset buttons
        SetBoardInteractable(true);
        for (int i = 0; i < buttonlist.Length; i++)
        {
            buttonlist[i].sprite = null;
            buttonlist[i].gameObject.SetActive(false);
        }
    }
    public void BackToMenu()
    {
        SMref.GetComponent<GameSceneManager>().SetScene(0);
        Destroy(SMref);
        Destroy(StoredData);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Destroy(buttons[i,j].gameObject);
            }
        }
    }
    bool CheckRow(int r)
    {
        int temp = 1;
        if (buttons[r, 0].sprite == buttons[r, temp].sprite &&
            !buttons[r, 0].GetComponentInParent<Button>().interactable)
        {
            for (; temp < row; temp++)
            {
                if (buttons[r, temp].sprite == buttons[r, 0].sprite &&
                    !buttons[r, temp].GetComponentInParent<Button>().interactable)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            if (temp >= column)
            {
                return true;
            }
        }

        return false;
    }
    bool CheckCol(int col)
    {
        int temp = 1;
        if (buttons[0, col].sprite == buttons[temp, col].sprite &&
            !buttons[0, col].GetComponentInParent<Button>().interactable)
        {
            for (; temp < row; temp++)
            {
                if (buttons[temp, col].sprite == buttons[0, col].sprite &&
                    !buttons[temp, col].GetComponentInParent<Button>().interactable)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            if (temp >= column)
            {
                return true;
            }
        }
        return false;
    }
    bool CheckTopLeftBottomRight()
    {
        int temp = 1;

        if (!buttons[0, 0].GetComponentInParent<Button>().interactable &&
        buttons[0, 0].sprite == buttons[temp, temp].sprite)
        {
            for (; temp < row; temp++)
            {
                if(buttons[temp,temp].sprite == buttons[0,0].sprite)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            if(temp >= row)
            {
                return true;
            }
        }
        return false;
    }
    bool CheckBottomLeftTopRight()
    {
        int firstindex = row - 1; //2
        int firsttemp = firstindex - 1; //1

        int secondindex = 0;
        int secondtemp = secondindex + 1; //1

        if(!buttons[firstindex, secondindex].GetComponentInParent<Button>().interactable)
        {
            for(;firsttemp >= 0; firsttemp--)
            {

                if (!buttons[firsttemp, secondtemp].GetComponentInParent<Button>().interactable
                    && buttons[firsttemp, secondtemp].sprite == buttons[firstindex, secondindex].sprite)
                {
                    secondtemp++;
                    continue;
                }
                else
                {
                    return false;
                }

            }

            if(secondtemp >= row)
            {
                return true;
            }
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIGManager : MonoBehaviour
{
    public GameObject GOPanel;
    public GameObject IndicatorImg;
    public GameObject InText;
    public Image[] buttonlist;

    private bool IsPlayerTurn;
    private float delay;
    private Image[,] buttons;
    private int row, column;
    private GameObject StoredData;
    private GameObject SMref;
    private Sprite Sp1;
    private const string IndicatorText = "Turn: ";

    // Start is called before the first frame update
    void Start()
    {
        StoredData = GameObject.FindGameObjectWithTag("GameData");
        SMref = GameObject.FindGameObjectWithTag("SceneManager");
        row = 3;
        column = 3;
        buttons = new Image[row, column];

        int x = 0;
        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                buttons[i, j] = buttonlist[x];
                ++x;
            }
        }

        this.GetComponent<GameData>().player1 = StoredData.GetComponent<GameData>().GetPlayer1Img();
        this.GetComponent<GameData>().player2 = StoredData.GetComponent<GameData>().GetPlayer2Img();

        //Setup Game
        delay = 10.0f;
        IsPlayerTurn = true;
        GOPanel.SetActive(false);
        SetReferenceOnButton();
        Sp1 = this.GetComponent<GameData>().player1;
        SetPlayerIndicator(IndicatorText + "Player 1", Sp1);
    }

    //Set Fuctions
    void SetReferenceOnButton()
    {
        for (int i = 0; i < buttonlist.Length; i++)
        {
            buttonlist[i].GetComponentInParent<Symbol>().SetReference(this);
        }
    }
    public void SetPlayerIndicator(string str, Sprite sprite)
    {
        InText.GetComponent<Text>().text = str;
        IndicatorImg.GetComponent<Image>().sprite = sprite;
    }
    public void SetPlayerTurn(bool turn)
    {
        IsPlayerTurn = turn;
    }
    void SetBoardInteractable(bool bb)
    {
        for (int i = 0; i < buttonlist.Length; i++)
        {
            buttonlist[i].GetComponentInParent<Button>().interactable = bb;
        }
    }
    void SetGameOverText(string stng)
    {
        GOPanel.SetActive(true);
        GOPanel.GetComponentInChildren<Text>().text = stng;
    }

    //Get Functions
    public bool GetPlayerTurn()
    {
        return IsPlayerTurn;
    }
    public Sprite GetPlayerSprite()
    {
        return Sp1;
    }
    public void ChangeTurn()
    {
        this.GetComponent<GameData>().IncrementCount();

        //Check top left to bottom right
        if (CheckTopLeftBottomRight())
        {
            EndGame();
        }
        else if (CheckBottomLeftTopRight())
        {
            EndGame();
        }

        for (int i = row - 1; i >= 0; i--)
        {
            if (CheckRow(i) == true)
            {
                EndGame();
            }
            else if (CheckCol(i) == true)
            {
                EndGame();
            }
            else if (this.GetComponent<GameData>().GetCountM() >= 9)
            {
                SetGameOverText("ITS A TIE");
            }
        }

        NextTurn();
    }
    void NextTurn()
    {

        if (!IsPlayerTurn)
        {
            Sp1 = this.GetComponent<GameData>().player2;
            SetPlayerIndicator(IndicatorText + "Computer", Sp1);
        }
        else
        {
            Sp1 = this.GetComponent<GameData>().player1;
            SetPlayerIndicator(IndicatorText + "Player", Sp1);
        }

    }
    void EndGame()
    {
        SetBoardInteractable(false);
        if (Sp1 == this.GetComponent<GameData>().player1)
        {
            SetGameOverText("Player 1 Wins!");
        }
        else if (Sp1 == this.GetComponent<GameData>().player2)
        {
            SetGameOverText("Computer Wins!");
        }
    }
    public void ResetGame()
    {
        delay = 10.0f;
        IsPlayerTurn = true;
        GOPanel.SetActive(false);
        Sp1 = this.GetComponent<GameData>().player1;
        SetPlayerIndicator(IndicatorText + "Player", Sp1);
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
                Destroy(buttons[i, j].gameObject);
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
                if (buttons[temp, temp].sprite == buttons[0, 0].sprite)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            if (temp >= row)
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

        if (!buttons[firstindex, secondindex].GetComponentInParent<Button>().interactable)
        {
            for (; firsttemp >= 0; firsttemp--)
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

            if (secondtemp >= row)
            {
                return true;
            }
        }
        return false;
    }

    bool CheckR(Sprite sp)
    {
        for (int x = 0; x < row; x++)
        {
            if (buttons[x, 0].sprite == sp
                && buttons[x, 1].sprite == sp
                && buttons[x, 2].GetComponentInParent<Button>().interactable
                && !buttons[x, 0].GetComponentInParent<Button>().interactable)
            {
                buttons[x, 2].gameObject.SetActive(true);
                buttons[x, 2].sprite = Sp1;
                buttons[x, 2].GetComponentInParent<Button>().interactable = false;
                return true;
            }
            else if (buttons[x, 0].sprite == sp
                && buttons[x, 2].sprite == sp
                && buttons[x, 1].GetComponentInParent<Button>().interactable
                 && !buttons[x, 0].GetComponentInParent<Button>().interactable)
            {
                buttons[x, 1].gameObject.SetActive(true);
                buttons[x, 1].sprite = Sp1;
                buttons[x, 1].GetComponentInParent<Button>().interactable = false;
                return true;
            }
            else if (buttons[x, 1].sprite == sp
                && buttons[x, 2].sprite == sp
                && buttons[x, 0].GetComponentInParent<Button>().interactable
                 && !buttons[x, 1].GetComponentInParent<Button>().interactable)
            {
                buttons[x, 0].gameObject.SetActive(true);
                buttons[x, 0].sprite = Sp1;
                buttons[x, 0].GetComponentInParent<Button>().interactable = false;
                return true;
            }
        }
        return false;
    }
    bool CheckC(Sprite sp)
    {
        for (int x = 0; x < column; x++)
        {
            if (buttons[0, x].sprite == this.GetComponent<GameData>().player2
                && buttons[1, x].sprite == this.GetComponent<GameData>().player2
                && buttons[2, x].GetComponentInParent<Button>().interactable
                 && !buttons[0, x].GetComponentInParent<Button>().interactable)
            {
                buttons[2, x].gameObject.SetActive(true);
                buttons[2, x].sprite = Sp1;
                buttons[2, x].GetComponentInParent<Button>().interactable = false;
                return true;
            }
            else if (buttons[0, x].sprite == this.GetComponent<GameData>().player2
                && buttons[2, x].sprite == this.GetComponent<GameData>().player2
                && buttons[1, x].GetComponentInParent<Button>().interactable
                && !buttons[0, x].GetComponentInParent<Button>().interactable)
            {
                buttons[1, x].gameObject.SetActive(true);
                buttons[1, x].sprite = Sp1;
                buttons[1, x].GetComponentInParent<Button>().interactable = false;
                return true;
            }
            else if (buttons[1, x].sprite == this.GetComponent<GameData>().player2
                && buttons[2, x].sprite == this.GetComponent<GameData>().player2
                && buttons[0, x].GetComponentInParent<Button>().interactable
                && !buttons[1, x].GetComponentInParent<Button>().interactable)
            {
                buttons[0, x].gameObject.SetActive(true);
                buttons[0, x].sprite = Sp1;
                buttons[0, x].GetComponentInParent<Button>().interactable = false;
                return true;
            }
        }
        return false;
    }
    bool CheckD(Sprite sp)
    {
        if (buttons[0, 0].sprite == sp
            && buttons[1, 1].sprite == sp
            && !buttons[0, 0].GetComponentInParent<Button>().interactable
            && buttons[2, 2].GetComponentInParent<Button>().interactable)
        {
            buttons[2, 2].gameObject.SetActive(true);
            buttons[2, 2].sprite = Sp1;
            buttons[2, 2].GetComponentInParent<Button>().interactable = false;
            return true;
        }
        else if (buttons[0, 2].sprite == sp
            && buttons[1, 1].sprite == sp
            && !buttons[0, 2].GetComponentInParent<Button>().interactable
            && buttons[2, 0].GetComponentInParent<Button>().interactable)
        {
            buttons[2, 0].gameObject.SetActive(true);
            buttons[2, 0].sprite = Sp1;
            buttons[2, 0].GetComponentInParent<Button>().interactable = false;
            return true;
        }

        else if (buttons[2, 2].sprite == sp
            && buttons[1, 1].sprite == sp
            && !buttons[2, 2].GetComponentInParent<Button>().interactable
            && buttons[0, 0].GetComponentInParent<Button>().interactable)
        {
            buttons[0, 0].gameObject.SetActive(true);
            buttons[0, 0].sprite = Sp1;
            buttons[0, 0].GetComponentInParent<Button>().interactable = false;
            return true;
        }
        else if (buttons[2, 0].sprite == sp
            && buttons[1, 1].sprite == sp
            && !buttons[2, 0].GetComponentInParent<Button>().interactable
            && buttons[0, 2].GetComponentInParent<Button>().interactable)
        {
            buttons[0, 2].gameObject.SetActive(true);
            buttons[0, 2].sprite = Sp1;
            buttons[0, 2].GetComponentInParent<Button>().interactable = false;
            return true;
        }

        else if (buttons[0, 0].sprite == sp
            && buttons[2, 2].sprite == sp
            && !buttons[0, 0].GetComponentInParent<Button>().interactable
            && buttons[1, 1].GetComponentInParent<Button>().interactable)
        {
            buttons[1, 1].gameObject.SetActive(true);
            buttons[1, 1].sprite = Sp1;
            buttons[1, 1].GetComponentInParent<Button>().interactable = false;
            return true;
        }
        else if (buttons[0, 2].sprite == sp
            && buttons[2, 0].sprite == sp
            && !buttons[0, 2].GetComponentInParent<Button>().interactable
            && buttons[1, 1].GetComponentInParent<Button>().interactable)
        {
            buttons[1, 1].gameObject.SetActive(true);
            buttons[1, 1].sprite = Sp1;
            buttons[1, 1].GetComponentInParent<Button>().interactable = false;
            return true;
        }
        return false;
    }
    void CheckBoard()
    {
        
        //First move
        if (this.GetComponent<GameData>().GetCountM() < 2)
        {
            buttons[1, 1].gameObject.SetActive(true);
            buttons[1, 1].sprite = Sp1;
            buttons[1, 1].GetComponentInParent<Button>().interactable = false;
            delay = 10.0f;
            IsPlayerTurn = true;
            ChangeTurn();
        }
        //Check rows for win
        else if (CheckR(this.GetComponent<GameData>().player2))
        {
            IsPlayerTurn = true;
            delay = 10.0f;
            ChangeTurn();
        }
        //Check columns for win
        else if (CheckC(this.GetComponent<GameData>().player2))
        {
            IsPlayerTurn = true;
            delay = 10.0f;
            ChangeTurn();
        }
        //Diagonals for win
        else if (CheckD(this.GetComponent<GameData>().player2))
        {
            IsPlayerTurn = true;
            delay = 10.0f;
            ChangeTurn();
        }

        //Check rows for win
        else if (CheckR(this.GetComponent<GameData>().player1))
        {
            IsPlayerTurn = true;
            delay = 10.0f;
            ChangeTurn();
        }
        //Check columns for win
        else if (CheckC(this.GetComponent<GameData>().player1))
        {
            IsPlayerTurn = true;
            delay = 10.0f;
            ChangeTurn();
        }
        //Diagonals for win
        else if (CheckD(this.GetComponent<GameData>().player1))
        {
            IsPlayerTurn = true;
            delay = 10.0f;
            ChangeTurn();
        }

        else
        {
            int value = Random.Range(0, 8);
            if (buttonlist[value].GetComponentInParent<Button>().interactable)
            {
                buttonlist[value].gameObject.SetActive(true);
                buttonlist[value].sprite = Sp1;
                buttonlist[value].GetComponentInParent<Button>().interactable = false;
                delay = 10.0f;
                IsPlayerTurn = true;
                ChangeTurn();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerTurn == false)
        {
            delay += delay * Time.deltaTime;
            if (delay >= 100)
            {
                CheckBoard();
            }
        }
    }
}

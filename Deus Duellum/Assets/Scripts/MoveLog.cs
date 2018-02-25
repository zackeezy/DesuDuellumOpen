using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveLog : MonoBehaviour {

    public Text whiteLog;
    public Text blackLog;

    public Text smallWhiteLog;
    public Text smallBlackLog;

    public GameObject bigLog;
    public GameObject smallLog;

    public Scrollbar vertical;

    public Text logButtonText;

    private string recentWhite;
    private string recentBlack;
    private int count = 0;

    //private List<string> whiteMoves;
    //private List<string> blackMoves;

	// Use this for initialization
	void Start () {
        smallLog.SetActive(true);
        bigLog.SetActive(false);
        logButtonText.text = "Expand Log";
    }
	
	// Update is called once per frame
	void Update () {

	}

    public string CoordsToNotations(int x,int y)
    {
        string notation = "";
        switch (x)
        {
            case 0:
                notation += "A";
                break;
            case 1:
                notation += "B";
                break;
            case 2:
                notation += "C";
                break;
            case 3:
                notation += "D";
                break;
            case 4:
                notation += "E";
                break;
            case 5:
                notation += "F";
                break;
            case 6:
                notation += "G";
                break;
            case 7:
                notation += "H";
                break;
        }
        int num = y + 1;
        notation += num;
        return notation;
    }

    public void ShowNotations(string notation, bool isWhite)
    {
        string notations;
        if (isWhite)
        {
            notations = whiteLog.text;
            //notations += '\n';
            //notations += notation;
            notations = notation + '\n' + notations;

            whiteLog.text = notations;
            recentWhite = notation;
            smallWhiteLog.text = recentWhite;
        }
        else
        {
            notations = blackLog.text;
            //notations += '\n';
            //notations += notation;
            notations = notation + '\n' + notations;

            blackLog.text = notations;
            recentBlack = notation;
            smallBlackLog.text = recentBlack;
        }
    }

    public void ShowWin(bool white)
    {
        string notations;
        if (white)
        {
            notations = whiteLog.text;
            smallWhiteLog.text += '#';
        }
        else
        {
            notations = blackLog.text;
            smallWhiteLog.text += '#';
        }

        notations += '#';

        if (white)
        {
           whiteLog.text = notations;
        }
        else
        {
            blackLog.text = notations;
        }
    }

    public void ToggleLog()
    {
        //switch the active log
        bigLog.SetActive(!bigLog.activeSelf);
        smallLog.SetActive(!smallLog.activeSelf);
        if (bigLog.activeSelf)
        {
            logButtonText.text = "Minimize Log";
        }
        else
        {
            logButtonText.text = "Expand Log";
        }
    }

    //changes white and black to reflect character selection
    public void SetCharacters(int white, int black)
    {
        Text bigWhiteText = bigLog.transform.GetChild(0).GetComponent<Text>();
        Text bigBlackText = bigLog.transform.GetChild(1).GetComponent<Text>();
        Text smallWhiteText = smallLog.transform.GetChild(0).GetComponent<Text>();
        Text smallBlackText = smallLog.transform.GetChild(1).GetComponent<Text>();

        if (white == 0)
        {
            bigWhiteText.text = "Athena";
            smallWhiteText.text = "Athena";
        }
        else if (white == 1)
        {
            bigWhiteText.text = "Ra";
            smallWhiteText.text = "Ra";
        }
        else if (white == 2)
        {
            bigWhiteText.text = "Thor";
            smallWhiteText.text = "Thor";
        }

        if (black == 0)
        {
            bigBlackText.text = "Athena";
            smallBlackText.text = "Athena";
        }
        else if (black == 1)
        {
            bigBlackText.text = "Ra";
            smallBlackText.text = "Ra";
        }
        else if (black == 2)
        {
            bigBlackText.text = "Thor";
            smallBlackText.text = "Thor";
        }
    }
}

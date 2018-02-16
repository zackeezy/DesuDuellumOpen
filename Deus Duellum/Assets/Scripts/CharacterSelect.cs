using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour {

    public int player = 1;
    public Text characterSelectText;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }

    public void setCharacter(int character)
    {
        //add visual effect to show it was picked

        if (player == 2)
        {
            //let player2 pick a character for local game
            PlayerPrefs.SetInt("Player2Character", character);
        }
        else
        {
            PlayerPrefs.SetInt("Player1Character", character);
            //let player2 pick a character for a local game
            int gameIndex = SceneManager.GetActiveScene().buildIndex;
            if (gameIndex == 1)
            {
                characterSelectText.text = "Player 2 Select a character";
                //show a cancel button? to go back to player1?
                player = 2;
            }
        }
    }

    public void setFirst(bool first)
    {
        //add visual effect to show it was picked

        if (first)
        {
            //player1 is white
            PlayerPrefs.SetInt("player1", 0);
        }
        else
        {
            //player1 is black
            PlayerPrefs.SetInt("player1", 1);
        }
    }

    public void setDifficulty(bool easy)
    {
        //add visual effect to show it was picked

        if (easy)
        {
            //easy
            PlayerPrefs.SetInt("difficulty", 0);
        }
        else
        {
            //hard
            PlayerPrefs.SetInt("difficulty", 1);
        }
    }
}

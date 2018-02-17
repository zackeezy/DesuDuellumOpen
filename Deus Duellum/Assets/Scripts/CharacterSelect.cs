using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour {

    public int player = 1;
    int player1character;
    public Text characterSelectText;
    public Button playButton;

    private bool charactersSelected = false;
    private bool difficultySelected = false;
    private bool turnSelected = false;
    private int gameIndex;
    private bool canPlay = false;

	// Use this for initialization
	void Start () {
        gameIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canPlay)
        {
            CheckSettings();
        }
    }

    public void setCharacter(int character)
    {
        //add visual effect to show it was picked

        if (player == 2)
        {
            //let player2 pick a character for local game
            //if (character == player1character)
            //{
            //    characterSelectText.text = "Player 2 Select ANOTHER character";
            //}
            //else
            //{
                characterSelectText.text = "Press Play";
                PlayerPrefs.SetInt("Player2Character", character);
                charactersSelected = true;
            //}
        }
        else
        {
            player1character = character;
            PlayerPrefs.SetInt("Player1Character", character);
            setFirst(true);
            //let player2 pick a character for a local game
            if (gameIndex == 1)
            {
                characterSelectText.text = "Player 2 Select a character";
                //show a cancel button? to go back to player1?
                player = 2;
            }
            else
            {
                charactersSelected = true;
            }
        }
    }

    public void setFirst(bool first)
    {
        turnSelected = true;

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
        difficultySelected = true;

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

    public void CheckSettings()
    {
        //if local, only characters must be selected
        if (gameIndex == 1 && charactersSelected)
        {
            canPlay = true;
        }
        //if network, must also choose turn
        else if (gameIndex == 2 && charactersSelected && turnSelected)
        {
            canPlay = true;
        }
        //if ai, must also choose difficulty
        else if (gameIndex == 3 && charactersSelected && turnSelected && difficultySelected)
        {
            canPlay = true;
        }

        playButton.interactable = canPlay;
    }
}

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

    private List<GameObject> CharacterHighlights;
    private GameObject difficultyHighlight;
    private GameObject turnHighlight;

    private bool charactersSelected = false;
    private bool difficultySelected = false;
    private bool turnSelected = false;
    private int gameIndex;
    private bool canPlay = false;

	// Use this for initialization
	void Start () {
        gameIndex = SceneManager.GetActiveScene().buildIndex;
        CharacterHighlights = new List<GameObject>();
        difficultyHighlight = null;
        turnHighlight = null;
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
                setCharacterHighlight(character, false);
                charactersSelected = true;
                setFirst(true);
            //}
        }
        else
        {
            player1character = character;
            PlayerPrefs.SetInt("Player1Character", character);

            setCharacterHighlight(character, true);

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
        Button btn;
        if (first)
        {
            //player1 is white
            PlayerPrefs.SetInt("player1", 0);
            btn = this.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Button>();
        }
        else
        {
            //player1 is black
            PlayerPrefs.SetInt("player1", 1);
            btn = this.transform.GetChild(1).GetChild(2).gameObject.GetComponent<Button>();
        }

        //add visual effect to show it was picked
        if (turnHighlight != null)
        {
            turnHighlight.SetActive(false);
            turnHighlight = null;
        }
        GameObject highlight;
        highlight = btn.transform.GetChild(1).gameObject;
        highlight.SetActive(true);
        turnHighlight = highlight;
    }

    public void setDifficulty(bool easy)
    {
        difficultySelected = true;
        Button btn;

        if (easy)
        {
            //easy
            PlayerPrefs.SetInt("difficulty", 0);
            btn = this.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>();
        }
        else
        {
            //hard
            PlayerPrefs.SetInt("difficulty", 1);
            btn = this.transform.GetChild(2).GetChild(2).gameObject.GetComponent<Button>();
        }

        //add visual effect to show it was picked
        if (difficultyHighlight != null)
        {
            difficultyHighlight.SetActive(false);
            difficultyHighlight = null;
        }
        GameObject highlight;
        highlight = btn.transform.GetChild(1).gameObject;
        highlight.SetActive(true);
        difficultyHighlight = highlight;

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

    public void setCharacterHighlight(int character, bool player1)
    {
        Button btn = this.transform.GetChild(0).GetChild(character + 1).gameObject.GetComponent<Button>();
        GameObject highlight;
        if (player1)
        {
            foreach(GameObject h in CharacterHighlights)
            {
                h.SetActive(false);
            }
            highlight = btn.transform.GetChild(0).gameObject;
            //Image btnImg = highlight.GetComponent<Image>();
            //btnImg.sprite = highlight1;
        }
        else
        {
            highlight = btn.transform.GetChild(1).gameObject;
            //Image btnImg = highlight.GetComponent<Image>();
            //btnImg.sprite = highlight2;
        }
        highlight.SetActive(true);
        CharacterHighlights.Add(highlight);
    }
}

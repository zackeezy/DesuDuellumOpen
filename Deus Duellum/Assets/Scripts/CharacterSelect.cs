using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using System;

public class CharacterSelect : MonoBehaviour {

    public int player = 1;
    int player1character;
    public Text characterSelectText;
    public Button playButton;

    private List<GameObject> CharacterHighlights;
    private GameObject player1Highlight;
    private GameObject player2Highlight;

    private GameObject difficultyHighlight;
    private GameObject turnHighlight;

    //private bool charactersSelected = false;
    private bool p1Selected = false;
    private bool p2Selected = false;
    private bool difficultySelected = false;
    private bool turnSelected = false;
    private int gameIndex;
    private bool canPlay = false;
    private bool localChose = false;
    private bool opponentChose = false;
    private GameObject waitPanel;
    private GameObject timeoutPanel;
    private NetworkControl netController;
    public bool PlayClicked = false;
    private bool netTimeout = false;

    private Thread characterWaitThread;

	// Use this for initialization
	void Start () {
        gameIndex = SceneManager.GetActiveScene().buildIndex;
        CharacterHighlights = new List<GameObject>();
        difficultyHighlight = null;
        turnHighlight = null;

        GameObject net = GameObject.FindGameObjectWithTag("network");
        if (net)
        {
            netController = net.GetComponent<NetworkControl>();
            netController.SetCharacterSelect(this);
            if (netController.isClient)
            {
                //player1 is black
                PlayerPrefs.SetInt("player1", 1);
            }
            else
            {
                //player1 is white
                PlayerPrefs.SetInt("player1", 0);
            }
        }

        waitPanel = GameObject.FindGameObjectWithTag("waitPanel");
        if(waitPanel){
            waitPanel.SetActive(false);
        }

        timeoutPanel = GameObject.FindGameObjectWithTag("timeoutPanel");
        if (timeoutPanel)
        {
            timeoutPanel.SetActive(false);
        }
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
        if (netTimeout)
        {
            //dont let anything happen if they click a character after net disconnected
            return;
        }
        //add visual effect to show it was picked
        if (gameIndex == 2)
        {
            if (player == 2)
            {
                characterSelectText.text = "Press Play";
                PlayerPrefs.SetInt("Player2Character", character);
                SetCharacterHighlight(character, false);
                p2Selected = true;
                setFirst(true);
                player = 3;
                if (!p1Selected)
                {
                    player = 1;
                }
                Button backButton = transform.GetChild(0).GetChild(5).GetComponent<Button>();
                backButton.interactable = true;
            }
            else if (player == 1)
            {
                player1character = character;
                PlayerPrefs.SetInt("Player1Character", character);

                if (!p1Selected)
                {
                    SetCharacterHighlight(character, true);
                    p1Selected = true;
                }

                //let player2 pick a character for a local game
                if (gameIndex == 2 && !p2Selected)
                {
                    characterSelectText.text = "Player Two: Select a character";
                    //show a cancel button? to go back to player1?
                    player = 2;

                    Button backButton = transform.GetChild(0).GetChild(5).GetComponent<Button>();
                    backButton.interactable = true;
                }
                else
                {
                    //charactersSelected = true;
                    characterSelectText.text = "Press Play";
                    player = 3;
                }
            }
            else
            {
                //???
            }
        }
        else
        {
            if (!PlayClicked)
            {
                p1Selected = true;
                player1character = character;
                PlayerPrefs.SetInt("Player1Character", character);
                SetCharacterHighlight(character, true);
                characterSelectText.text = "Press Play";
            }
        }     
    }

    public void setFirst(bool first)
    {
        turnSelected = true;
        Button btn;
        if (gameIndex != 2)
        {
            if (first)
            {
                //player1 is white
                PlayerPrefs.SetInt("player1", 0);
                btn = this.transform.GetChild(2).GetChild(1).gameObject.GetComponent<Button>();
            }
            else
            {
                //player1 is black
                PlayerPrefs.SetInt("player1", 1);
                btn = this.transform.GetChild(2).GetChild(2).gameObject.GetComponent<Button>();
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
        else
        {
            //player1 is white, no buttons to highlight
            PlayerPrefs.SetInt("player1", 0);
        }
    }

    public void setDifficulty(bool easy)
    {
        difficultySelected = true;
        Button btn;

        if (easy)
        {
            //easy
            PlayerPrefs.SetInt("difficulty", 0);
            btn = this.transform.GetChild(3).GetChild(1).gameObject.GetComponent<Button>();
        }
        else
        {
            //hard
            PlayerPrefs.SetInt("difficulty", 1);
            btn = this.transform.GetChild(3).GetChild(2).gameObject.GetComponent<Button>();
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
        if (gameIndex == 2 && p1Selected && p2Selected)
        {
            canPlay = true;
        }
        //if network
        else if (gameIndex == 4 && p1Selected)
        {
            canPlay = true;
        }
        //if ai, must also choose difficulty
        else if (gameIndex == 5 && p1Selected && turnSelected && difficultySelected)
        {
            canPlay = true;
        }

        playButton.interactable = canPlay;
    }

    public void SetCharacterHighlight(int character, bool player1)
    {
        Button btn = this.transform.GetChild(0).GetChild(character + 2).gameObject.GetComponent<Button>();
        GameObject highlight;
        if (player1)
        {
            if (gameIndex != 2)
            {
                foreach (GameObject h in CharacterHighlights)
                {
                    h.SetActive(false);
                }
            }
            highlight = btn.transform.GetChild(0).gameObject;
            player1Highlight = highlight;
            //Image btnImg = highlight.GetComponent<Image>();
            //btnImg.sprite = highlight1;
            highlight.SetActive(true);
            CharacterHighlights.Add(highlight);
        }
        else
        {
            if (player2Highlight)
            {
                player2Highlight.SetActive(false);
            }
            highlight = btn.transform.GetChild(1).gameObject;
            player2Highlight = highlight;
            //Image btnImg = highlight.GetComponent<Image>();
            //btnImg.sprite = highlight2;
            highlight.SetActive(true);
            CharacterHighlights.Add(highlight);
        }
    }

    public void BackCharacterSelect()
    {
        if (player2Highlight)
        {
            player2Highlight.SetActive(false);
            player2Highlight = null;
            characterSelectText.text = "Player Two: Select a Character";
            playButton.interactable = false;
            player = 2;
            if (!p1Selected)
            {
                player = 1;
            }
            p2Selected = false;
            canPlay = false;
            
        }
        else if (player1Highlight)
        {
            player1Highlight.SetActive(false);
            player1Highlight = null;
            characterSelectText.text = "Player One: Select a Character";
            player = 1;
            p1Selected = false;
            Button backButton = transform.GetChild(0).GetChild(5).GetComponent<Button>();
            backButton.interactable = false;
            canPlay = false;
        }
        if(!player1Highlight && !player2Highlight)
        {
            Button backButton = transform.GetChild(0).GetChild(5).GetComponent<Button>();
            backButton.interactable = false;
        }
        //charactersSelected = false;
    }

    public void HighlightClicked(bool p1)
    {
        if (p1)
        {
            player1Highlight.SetActive(false);
            player1Highlight = null;
            characterSelectText.text = "Player One: Select a Character";
            player = 1;
            p1Selected = false;
            //Button backButton = transform.GetChild(0).GetChild(5).GetComponent<Button>();
            //backButton.interactable = false;
            canPlay = false;
            playButton.interactable = false;
        }
        else
        {
            player2Highlight.SetActive(false);
            player2Highlight = null;
            characterSelectText.text = "Player Two: Select a Character";
            playButton.interactable = false;
            player = 2;
            p2Selected = false;
            canPlay = false;
            playButton.interactable = false;
        }
        if (!player1Highlight && !player2Highlight)
        {
            Button backButton = transform.GetChild(0).GetChild(5).GetComponent<Button>();
            backButton.interactable = false;
        }
        //charactersSelected = false;
    }

    public void OtherPlayerSelected(int character)
    {
        opponentChose = true;
        PlayerPrefs.SetInt("Player2Character", character);
        CheckNetworkSelections();
    }

    public void MoveToNetworkGame()
    {
        PlayClicked = true;

        //send the character
        string tosend = "character|" + player1character;
        netController.Send(tosend);
        localChose = true;

        CheckNetworkSelections();
    }

    private void CheckNetworkSelections()
    {
        if (localChose && opponentChose)
        {
            //move to next scene
            LoadSceneOnClick scenechanger = GetComponent<LoadSceneOnClick>();
            scenechanger.LoadByIndex(7);
            CancelInvoke();
        }
        else if (!opponentChose)
        {
            //popup
            waitPanel.SetActive(true);
            Invoke("SetTimeoutPanelToActive", 10);
        }
    }

    public void Disconnect()
    {
        netController.Disconnect();
    }

    public void GoToMainMenu()
    {
        //SceneManager.LoadScene(1);
        timeoutPanel.transform.GetChild(1).GetComponent<LoadSceneOnClick>().LoadByIndex(1);
    }

    public void CancelGoTo()
    {
        CancelInvoke();
    }

    public void SetTimeoutPanelToActive()
    {
        timeoutPanel.SetActive(true);
        playButton.interactable = false;
        Invoke("GoToMainMenu", 7);
    }
}

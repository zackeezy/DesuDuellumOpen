using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Breakthrough;

public class BoardManager : MonoBehaviour {

	public static BoardManager Instance{ set; get;}

    public Camera blackCam;
    public GameObject Notations;
    public GameObject altNotations;

    public Image player1img;
    public Image player2img;
    public Sprite AthenaImg;
    public Sprite RaImg;
    public Sprite ThorImg;

    public Text turnText;
	public GameObject gameOverPanel;
    public bool isWhiteTurn = true;
    public PlayerType whitePlayer;
    public PlayerType blackPlayer;
    public PlayerType gameMode;

    public Token selectedToken;
    public int selectionX = -1;
	public int selectionY = -1;

    private List<GameObject> activeToken;
    private bool whiteWon = false;
	private bool blackWon = false;
    private bool tokenCaptured = false;
    private GameObject notationsToggleUI;
    private Toggle notationsToggle;

    private Move _move;

    // Use this for initialization
    void Start () {
		Instance = this;
        blackCam.enabled = false;
        Camera.main.enabled = true;

        notationsToggleUI = GameObject.FindGameObjectWithTag("NotationsToggle");
        notationsToggle = notationsToggleUI.GetComponent<Toggle>();

        _move = new Move();
        setPrefs();
	}

	// Update is called once per frame
	void Update ()
    {
        
	}

    public void TokenClicked(int x, int y, Token selected)
    {
        if (whiteWon || blackWon || gameMode != PlayerType.Local)
        {
            return;
        }
        //if the game is not over and same team
        if (!whiteWon && !blackWon && selected.isWhite == isWhiteTurn)
        {
            //Debug.Log("selected: " + x + ", " + y);
            //select that token
            selectionX = x;
            selectionY = y;
            selectedToken = selected;

            if (selectedToken.isWhite)
            {
                _move.whiteFromX = selectionX;
                _move.whiteFromY = selectionY;

                //Check Left
                _move.whiteToX = _move.whiteFromX - 1;
                _move.whiteToY = _move.whiteFromY + 1;
                bool isAllowed = _move.allowMove();
                //Highlight

                //Check Right
                _move.whiteToX = _move.whiteFromX + 1;
                _move.whiteToY = _move.whiteFromY + 1;
                isAllowed = _move.allowMove();
                //Highlight

                //Check Forward
                _move.whiteToX = _move.whiteFromX;
                _move.whiteToY = _move.whiteFromY + 1;
                isAllowed = _move.allowMove();
                //Highlight
            }
            else if(!selectedToken.isWhite)
            {
                _move.blackFromX = selectionX;
                _move.blackFromY = selectionY;

                //Check Left
                _move.blackToX = _move.blackFromX + 1;
                _move.blackToY = _move.blackFromY - 1;
                bool isAllowed = _move.allowMove();
                //Highlight

                //Check Right
                _move.blackToX = _move.blackFromX + 1;
                _move.blackToY = _move.blackFromY + 1;
                isAllowed = _move.allowMove();
                //Highlight

                //Check Forward
                _move.blackToX = _move.blackFromX;
                _move.blackToY = _move.blackFromY - 1;
                isAllowed = _move.allowMove();
                //Highlight
            }
        }
        //if game is not over and different team
        else if(!whiteWon && !blackWon && selected.isWhite != isWhiteTurn)
        {
            if (selectedToken != null && selectedToken.isWhite == isWhiteTurn)
            {
                if(AttemptMove(x, y, selected.transform.position.x, selected.transform.position.z))
                {
                    tokenCaptured = true;
                }
            }
        }
    }

    public void TileClicked(int x, int y, float xPos, float zPos)
    {
        if (whiteWon || blackWon || gameMode != PlayerType.Local)
        {
            return;
        }
        //if a token is selected
        if (selectedToken != null)
        {
            Debug.Log("moving to: " + x + ", " + y);
            //if that spot is a valid move
            AttemptMove(x, y, xPos, zPos);
        }
    }

    public bool AttemptMove(int x, int y, float xPos, float zPos)
    {
        bool isAllowed = false;
        if (isWhiteTurn)
        {
            _move.whiteFromX = selectedToken.currentX;
            _move.whiteFromY = selectedToken.currentY;
            _move.whiteToX = x;
            _move.whiteToY = y;
            isAllowed = _move.allowMove();
        }
        else if (!isWhiteTurn)
        {
            _move.blackFromX = selectedToken.currentX;
            _move.blackFromY = selectedToken.currentY;
            _move.blackToX = x;
            _move.blackToY = y;
            isAllowed = _move.allowMove();
        }

        if (isAllowed)
        {
            if (GameCoreToken.tokens[x, y] != ' ')
            {
                tokenCaptured = true;
            }
            //tell the game core about the move
            _move.RearrangeTokens();

            //actually move the token object visually
            MoveToken(x, y, xPos, zPos);
        }
        return isAllowed;
    }

	private void MoveToken(int x, int y, float tileXPos, float tileZPos)
    { 
        //Tween the position 
        Vector3 newPosition = new Vector3();
        newPosition.x = tileXPos;
        newPosition.y = selectedToken.gameObject.transform.position.y;
        newPosition.z = tileZPos;

        LeanTween.move(selectedToken.gameObject, newPosition, .3f);

        selectedToken.SetBoardPosition (x, y);

		//send the token's position to the log
        
        //destroy the captured token
		if(tokenCaptured){
            //destroy the captured gameobject
            GameObject[] otherPieces = null;
            if (isWhiteTurn)
            {
                otherPieces = GameObject.FindGameObjectsWithTag("BlackPieces");
            }
            else
            {
                otherPieces = GameObject.FindGameObjectsWithTag("WhitePieces");
            }
            foreach (GameObject othertoken in otherPieces)
            {
                Token other = othertoken.GetComponent<Token>();
                if (other.currentX == x && other.currentY == y)
                {
                    Destroy(other.gameObject);
                }
            }

            //add an x to the the log
        }

        //ask the game core if the game is over 
        GameWon();

        //toggle the turn
        ChangeTurn();
		BoardHighlights.Instance.HideHighlights ();
		selectedToken = null;
	}

    private void ChangeTurn()
    {
        isWhiteTurn = !isWhiteTurn;
        if (isWhiteTurn)
        {
            turnText.text = "White Player's Turn";
            if (whitePlayer == PlayerType.AI)
            {
                gameMode = PlayerType.AI;
                //ask for an AI move from the game core
                //use the AI's move received from core to show a move was made

            }
            if (whitePlayer == PlayerType.Network)
            {
                gameMode = PlayerType.Network;
                //ask for an network move from the game core
                //use the net's move received from core to show a move was made

            }
        }
        else if(!isWhiteTurn)
        {
            turnText.text = "Black Player's Turn";
            if (blackPlayer == PlayerType.AI)
            {
                gameMode = PlayerType.AI;
                //ask for an AI move from the game core
                //use the AI's move received from core to show a move was made

            }
            if (blackPlayer == PlayerType.Network)
            {
                gameMode = PlayerType.Network;
                //ask for an network move from the game core
                //use the net's move received from core to show a move was made

            }
        }
    }

    private void GameWon()
    {
        //ask the game core if the game has been won
        //show that the game was won and who won
        //gameOverPanel.SetActive(true);
    }

    private void setPrefs()
    {
        //set gameMode, whiteplayer, and blackplayer variables based on input from character select screen
        int player1character = PlayerPrefs.GetInt("Player1Character", 0);
        int player2character = 1;
        //set the player portraits
        setCharacterImage(1, player1character);
        int gameIndex = SceneManager.GetActiveScene().buildIndex;
        if (gameIndex == 4)
        {
            gameMode = PlayerType.Local;
            //set the player2 character
            player2character = PlayerPrefs.GetInt("Player2Character", 1);
        }
        else if (gameIndex == 5)
        {
            gameMode = PlayerType.Network;
            //ask for the character they are playing as
            //player2character =;
        }
        else if (gameIndex == 6)
        {
            gameMode = PlayerType.AI;
            //pick a random character for the AI
            int aiCharacter = Random.Range(0, 2);
            while (aiCharacter == player1character)
            {
                //so ai and player will not be same character
                aiCharacter = Random.Range(0, 2);
            }
            player2character = aiCharacter;
            int difficulty = PlayerPrefs.GetInt("difficulty", 0);
            if (difficulty == 0)
            {
                //tell the game core to use the easy AI
                Debug.Log("easy AI selected");
            }
            else if (difficulty == 1)
            {
                //tell the game core to use the harder AI
                Debug.Log("hard AI selected");
            }
        }
        setCharacterImage(2, player2character);

        int player1white = PlayerPrefs.GetInt("player1", 0);
        if (player1white == 0)
        {
            //player1 is white
            whitePlayer = PlayerType.Local;
            blackPlayer = gameMode;
            SetNotations(true);
        }
        else if(player1white != 0 && gameMode != PlayerType.Local)
        {
            //player1 is black
            blackPlayer = PlayerType.Local;
            whitePlayer = gameMode;
            blackCam.enabled = true;
            Camera.main.enabled = false;
            SetNotations(false);
        }
        //testfirst();

        //set the tokens
        TokenSetter tokenScript = GetComponent<TokenSetter>();
        tokenScript.SetTokens(player1white, player1character, player2character);
    }

    private void setCharacterImage(int player, int character)
    {
        if (player == 1)
        {
            if (character == 0)
            {
                //Debug.Log("player " + player + " is Athena");
                player1img.sprite = AthenaImg;
            }
            else if (character == 1)
            {
                //Debug.Log("player " + player + " is Ra");
                player1img.sprite = RaImg;
            }
            else if (character == 2)
            {
                //Debug.Log("player " + player + " is Thor");
                player1img.sprite = ThorImg;
            }
        }
        else if (player == 2)
        {
            if (character == 0)
            {
                player2img.sprite = AthenaImg;
            }
            else if (character == 1)
            {
                player2img.sprite = RaImg;
            }
            else if (character == 2)
            {
                player2img.sprite = ThorImg;
            }
        }
    }

    //choose which notations to use
    private void SetNotations(bool first)
    {
        if (first)
        {
            notationsToggle.onValueChanged.AddListener(toggleNotations);
        }
        else
        {
            notationsToggle.onValueChanged.AddListener(toggleAltNotations);
        }
    }

    //listener to toggle normal notations
    public void toggleNotations(bool on)
    {
        ToggleOpen openScript = notationsToggle.GetComponent<ToggleOpen>();
        openScript.toggleOpen(Notations);
    }

    //listener to toggle alternative notations
    public void toggleAltNotations(bool on)
    {
        ToggleOpen openScript = notationsToggle.GetComponent<ToggleOpen>();
        openScript.toggleOpen(altNotations);
    }

    private void testfirst()
    {
        if (gameMode == PlayerType.Local)
        {
            Debug.Log("Local Game, player 1 goes first");
        }
        else if (gameMode == PlayerType.Network)
        {
            if (whitePlayer == PlayerType.Local)
            {
                Debug.Log("white is local, black is network");
            }
            else
            {
                Debug.Log("white is network, black is local");
            }
        }
        else if (gameMode == PlayerType.AI)
        {
            if (whitePlayer == PlayerType.Local)
            {
                Debug.Log("white is local, black is ai");
            }
            else
            {
                Debug.Log("white is ai, black is local");
            }
        }
    }
}



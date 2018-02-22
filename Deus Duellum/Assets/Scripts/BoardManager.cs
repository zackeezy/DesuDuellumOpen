using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using System.Threading;

public class BoardManager : MonoBehaviour {

	public static BoardManager Instance{ set; get;}

    public Camera blackCam;
    public GameObject Notations;
    public GameObject altNotations;

    private int _awaitMoveX;
    private int _awaitMoveY;
    private Direction _awaitMoveDirection;
    private bool _foreignMoveCompleted;

    public Image player1img;
    public Image player2img;
    public Sprite AthenaImg;
    public Sprite RaImg;
    public Sprite ThorImg;

    public Text turnText;
	public GameObject gameOverPanel;
    public bool isWhiteTurn = true;
    public bool isBlackTurn
    {
        get { return !isWhiteTurn; }
    }
    public PlayerType whitePlayer;
    public PlayerType blackPlayer;
    public PlayerType gameMode;

    public Token selectedToken;
    public int selectionX = -1;
	public int selectionY = -1;

    public bool muted = false;

    public List<GameObject> boardTokens;

    private bool whiteWon = false;
	private bool blackWon = false;
    private GameObject _capturedPiece;
    private GameObject notationsToggleUI;
    private Toggle notationsToggle;

    private GameCore _core;

    private MoveLog log;

    private float[] tilePositionX =
    {
        -4.371f, -3.121f, -1.871f, -0.621f, 0.629f, 1.879f, 3.129f, 4.379f, 
    };

    private float[] tilePositionZ =
    {
        -7.65f, -6.4f, -5.15f, -3.9f, -2.65f, -1.4f, -0.1500001f, 1.1f, 
    };

    // Use this for initialization
    void Start () {
		Instance = this;
        blackCam.enabled = false;
        Camera.main.enabled = true;

        notationsToggleUI = GameObject.FindGameObjectWithTag("NotationsToggle");
        notationsToggle = notationsToggleUI.GetComponent<Toggle>();

        log = GetComponent<MoveLog>();

        setPrefs();
        _core = new GameCore(whitePlayer, blackPlayer, boardTokens);
        _capturedPiece = null;
        _foreignMoveCompleted = false;
    }

	// Update is called once per frame
	void Update ()
    {
        if (_foreignMoveCompleted)
        {
            if (_core.IsMoveAllowed(_awaitMoveX, _awaitMoveY, _awaitMoveDirection))
            {
                GameObject movingPiece = _core.Board[_awaitMoveX, _awaitMoveY];
                _capturedPiece = _core.MakeMove(_awaitMoveX, _awaitMoveY, _awaitMoveDirection);
                _core.MoveCoordinates(ref _awaitMoveX, ref _awaitMoveY, _awaitMoveDirection, isWhiteTurn);
                selectedToken = movingPiece.GetComponent<Token>();

                MoveToken(_awaitMoveX, _awaitMoveY, tilePositionX[_awaitMoveX], tilePositionZ[_awaitMoveY]);
            }
            _foreignMoveCompleted = false;
            gameMode = PlayerType.Local;
        }
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
            if (selectedToken != null)
            {
                BoardHighlights.Instance.HideHighlights();
                //TODO: if already a selected token, make it not float

            }

            //Debug.Log("selected: " + x + ", " + y);
            //select that token
            selectionX = x;
            selectionY = y;
            selectedToken = selected;

            //TODO: make the token float a little

            //Check East
            if (_core.IsMoveAllowed(selectionX, selectionY, Direction.East))
            {
                //Highlight
                if (isWhiteTurn)
                {
                    BoardHighlights.Instance.HighlightTile(x + 1, y + 1);
                }
                else if(isBlackTurn)
                {
                    BoardHighlights.Instance.HighlightTile(x + 1, y - 1);
                }
            }

            //Check Forward
            if (_core.IsMoveAllowed(selectionX, selectionY, Direction.Forward))
            {
                //Highlight
                if (isWhiteTurn)
                {
                    BoardHighlights.Instance.HighlightTile(x, y + 1);
                }
                else if (isBlackTurn)
                {
                    BoardHighlights.Instance.HighlightTile(x, y - 1);
                }

            }

            //Check West
            if (_core.IsMoveAllowed(selectionX, selectionY, Direction.West))
            {
                //Highlight
                if (isWhiteTurn)
                {
                    BoardHighlights.Instance.HighlightTile(x - 1, y + 1);
                }
                else if (isBlackTurn)
                {
                    BoardHighlights.Instance.HighlightTile(x - 1, y - 1);
                }
            }
        }
        else
        {
            if(!whiteWon && !blackWon && selected.isWhite != isWhiteTurn)
            {
                //they are trying to capture a piece
                TileClicked(x, y, selected.transform.position.x, selected.transform.position.z);
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
            //Debug.Log("moving to: " + x + ", " + y);
            //if that spot is a valid move
            AttemptMove(x, y, xPos, zPos);
        }
    }

    public bool AttemptMove(int x, int y, float xPos, float zPos)
    {
        bool isAllowed = false;
        if (isWhiteTurn && y == selectedToken.currentY + 1)
        {
            if (x == selectedToken.currentX - 1)
            {
                isAllowed = _core.IsMoveAllowed(selectedToken.currentX, selectedToken.currentY, Direction.West);
            }
            else if (x == selectedToken.currentX)
            {
                isAllowed = _core.IsMoveAllowed(selectedToken.currentX, selectedToken.currentY, Direction.Forward);
            }
            else if (x == selectedToken.currentX + 1)
            {
                isAllowed = _core.IsMoveAllowed(selectedToken.currentX, selectedToken.currentY, Direction.East);
            }
        }
        if (isBlackTurn && y == selectedToken.currentY - 1)
        {
            if (x == selectedToken.currentX - 1)
            {
                isAllowed = _core.IsMoveAllowed(selectedToken.currentX, selectedToken.currentY, Direction.West);
            }
            else if (x == selectedToken.currentX)
            {
                isAllowed = _core.IsMoveAllowed(selectedToken.currentX, selectedToken.currentY, Direction.Forward);
            }
            else if (x == selectedToken.currentX + 1)
            {
                isAllowed = _core.IsMoveAllowed(selectedToken.currentX, selectedToken.currentY, Direction.East);
            }
        }
        if (isAllowed)
        {
            //tell the game core about the move
            if (x < selectedToken.currentX)
            {
                _capturedPiece = _core.MakeMove(selectedToken.currentX, selectedToken.currentY, Direction.West);
            }
            else if (x == selectedToken.currentX)
            {
                _capturedPiece = _core.MakeMove(selectedToken.currentX, selectedToken.currentY, Direction.Forward);
            }
            else if (x > selectedToken.currentX)
            {
                _capturedPiece = _core.MakeMove(selectedToken.currentX, selectedToken.currentY, Direction.East);
            }

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

        string moveforLog = log.CoordsToNotations(selectedToken.currentX, selectedToken.currentY);

        selectedToken.SetBoardPosition (x, y);
        
        //destroy the captured token
		if(_capturedPiece != null)
        {
            
            Destroy(_capturedPiece);
            _capturedPiece = null;

            //add an x to the the log
            moveforLog += "x";
        }

        moveforLog += log.CoordsToNotations(x, y);
        //send the move to the log
        log.ShowNotations(moveforLog, isWhiteTurn);

        //Debug.Log(moveforLog);
    
        //toggle the turn
        ChangeTurn();
		BoardHighlights.Instance.HideHighlights ();
		selectedToken = null;
        GameWon(y);
    }

    private void GetMove()
    {
        int x = 0, y = 0;
        Direction direction = Direction.East;

        _core.GetMove(ref x, ref y, ref direction);

        _awaitMoveX = x;
        _awaitMoveY = y;
        _awaitMoveDirection = direction;
        _foreignMoveCompleted = true;
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
                _core.PrepForForeignMove();
                ThreadStart aiRef = new ThreadStart(GetMove);
                Thread aiThread = new Thread(aiRef);
                aiThread.Start();

            }
            else if (whitePlayer == PlayerType.Network)
            {
                gameMode = PlayerType.Network;
                //ask for an network move from the game core
                //use the net's move received from core to show a move was made

            }
            else if (whitePlayer == PlayerType.Local && blackPlayer == PlayerType.Local)
            {
                //enable white's emote button
                GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
                Button emote = player1.transform.GetChild(2).gameObject.GetComponent<Button>();
                emote.interactable = true;

                //disable black's emote button 
                GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
                Button emote2 = player2.transform.GetChild(2).gameObject.GetComponent<Button>();
                emote2.interactable = false;
            }
        }
        else if(isBlackTurn)
        {
            turnText.text = "Black Player's Turn";
            if (blackPlayer == PlayerType.AI)
            {
                gameMode = PlayerType.AI;
                _core.PrepForForeignMove();
                ThreadStart aiRef = new ThreadStart(GetMove);
                Thread aiThread = new Thread(aiRef);
                aiThread.Start();
            }
            else if (blackPlayer == PlayerType.Network)
            {
                gameMode = PlayerType.Network;
                //ask for an network move from the game core
                //use the net's move received from core to show a move was made

            }
            else if (whitePlayer == PlayerType.Local && blackPlayer == PlayerType.Local)
            {
                //disable white's emote button
                GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
                Button emote = player1.transform.GetChild(2).gameObject.GetComponent<Button>();
                emote.interactable = false;

                //enable blacks's emote button 
                GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
                Button emote2 = player2.transform.GetChild(2).gameObject.GetComponent<Button>();
                emote2.interactable = true;
            }
        }
    }

    private void GameWon(int y)
    {
        //ask the game core if the game has been won
        _core.HasWon(y);

        //change the winnertext
        GameObject winTextobj = gameOverPanel.transform.GetChild(0).gameObject;
        Text winText = winTextobj.GetComponent<Text>();

        if (_core.whiteWon)
        {
            if (whitePlayer == PlayerType.Local && blackPlayer == PlayerType.Local)
            {
                //player1 win
                winText.text = "Player One Wins";
            }
            else if (whitePlayer == PlayerType.Local)
            {
                //you win
                winText.text = "You Won!";
            }
            else
            {
                //you lost
                winText.text = "You Lost!";
            }

            log.ShowWin(true);
            
            //show that the game was won and who won
            gameOverPanel.SetActive(true);
        }
        else if (_core.blackWon)
        {
            if (blackPlayer == PlayerType.Local && whitePlayer == PlayerType.Local)
            {
                //player2 win
                winText.text = "Player Two Wins";
            }
            else if (blackPlayer == PlayerType.Local)
            {
                //you win
                winText.text = "You Won!";
            }
            else
            {
                //you lost
                winText.text = "You Lost!";
            }

            log.ShowWin(false);

            //show that the game was won and who won
            gameOverPanel.SetActive(true);
        }


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
        gameMode = whitePlayer;
        //testfirst();

        //set the tokens
        TokenSetter tokenScript = GetComponent<TokenSetter>();
        tokenScript.SetTokens(player1white, player1character, player2character);

        //set the log's characters
        log.SetCharacters(player1character, player2character);
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



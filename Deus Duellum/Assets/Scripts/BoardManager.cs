using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using System.Threading;
using UnityEngine.EventSystems;

public class BoardManager : MonoBehaviour {

	public static BoardManager Instance{ set; get;}

    public Camera blackCam;
    public GameObject Notations;
    public GameObject altNotations;

    private int _awaitMoveX;
    private int _awaitMoveY;
    private Direction _awaitMoveDirection;
    private bool _foreignMoveCompleted;

    public GameObject player1img;
    public GameObject player2img;
    public Sprite AthenaImg;
    public Sprite AthenaWinImg;
    public Sprite AthenaLoseImg;
    public Sprite AthenaBorder;
    public Sprite RaImg;
    public Sprite RaWinImg;
    public Sprite RaLoseImg;
    public Sprite RaBorder;
    public Sprite ThorImg;
    public Sprite ThorWinImg;
    public Sprite ThorLoseImg;
    public Sprite ThorBorder;

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
    NetworkControl netController;

    private float floatTokenYpos = 1.8f;
    private float flatTokenYpos = 1.084f;

    private MoveLog log;

    private AudioSource effectSource;
    public AudioClip captureSound;
    public AudioClip moveSound;
    public AudioClip athenaTurnSound;
    public AudioClip raTurnSound;
    public AudioClip thorTurnSound;

    private EmoteController player1emote;
    private EmoteController player2emote;
    private int whiteCharacter;
    private int blackCharacter;
    private bool isPlayer1White;

    private string player1;
    private string player2;

    public int timeLeft = 60;
    public Text countdownText;

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

        //starting timer
       // StartCoroutine("LoseTime");

        notationsToggleUI = GameObject.FindGameObjectWithTag("NotationsToggle");
        notationsToggle = notationsToggleUI.GetComponent<Toggle>();

        log = GetComponent<MoveLog>();

        GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
        player1emote = player1.GetComponent<EmoteController>();
        GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
        player2emote = player2.GetComponent<EmoteController>();

        setPrefs();

        _core = new GameCore(whitePlayer, blackPlayer, boardTokens);
        _capturedPiece = null;
        _foreignMoveCompleted = false;

        if (whitePlayer == PlayerType.Network || blackPlayer == PlayerType.Network)
        {
            netController = GameObject.FindGameObjectWithTag("network").GetComponent<NetworkControl>();
            SetNetworkStuff();
        }

        if (whitePlayer != PlayerType.Local)
        {
            GetMove();
            AnimateAvatars(2);
        }
        else
        {
            AnimateAvatars(1);
        }


        //set the clip the effectSource uses
        //WILL NOT WORK IF DO NOT START AT MAIN MENU
        GameObject Audio = GameObject.FindGameObjectWithTag("Audio");
        effectSource = Audio.GetComponent<MusicInfo>().effectsSource.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
        //countdownText.text = ("Time Left: \n " + timeLeft + " seconds");

        //if (timeLeft <= 0)
        //{
        //    StopCoroutine("LoseTime");
        //    countdownText.text = "Time's up!";
        //}

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
    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("the scene got destroyed");
    }

    public void TokenClicked(int x, int y, Token selected)
    {
        player1emote.CloseEmoteButtons();
        player2emote.CloseEmoteButtons();
        bool sameToken = false;

        if (whiteWon || blackWon || gameMode != PlayerType.Local || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        int rotationId = 0;

        //if the game is not over and same team
        if (!whiteWon && !blackWon && selected.isWhite == isWhiteTurn)
        {
            if (selectedToken != null)
            {
                LeanTween.cancelAll();
                LeanTween.rotate(selectedToken.gameObject, new Vector3(0, 0, 0), .01f);

                BoardHighlights.Instance.HideHighlights();
                Vector3 flatpos;
                flatpos.x = selectedToken.transform.position.x;
                flatpos.y = flatTokenYpos;
                flatpos.z = selectedToken.transform.position.z;
                //if already a selected token, make it stop floating                  
                LeanTween.move(selectedToken.gameObject, flatpos, 0.25f);
                
                //check to see if this is the same token
                if (selectedToken.currentX == selected.currentX && selectedToken.currentY == selected.currentY)
                {
                    sameToken = true;
                    selectedToken = null;
                }
            }

            //make the token start floating a little
            //unless it was the previously selected token
            if (!sameToken)
            {
                //Debug.Log("selected: " + x + ", " + y);
                //select that token
                selectionX = x;
                selectionY = y;
                selectedToken = selected;

                Vector3 floatpos;
                floatpos.x = selectedToken.transform.position.x;
                floatpos.y = floatTokenYpos;
                floatpos.z = selectedToken.transform.position.z;
                LeanTween.move(selectedToken.gameObject, floatpos, .25f);

                //make it spin
                rotationId = LeanTween.rotateAround(selectedToken.gameObject, Vector3.up, 360f, 3).setLoopClamp().id;

                //Check East
                if (_core.IsMoveAllowed(selectionX, selectionY, Direction.East))
                {
                    //Highlight
                    if (isWhiteTurn)
                    {
                        BoardHighlights.Instance.HighlightTile(x + 1, y + 1);
                    }
                    else if (isBlackTurn)
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
        player1emote.CloseEmoteButtons();
        player2emote.CloseEmoteButtons();

        if (whiteWon || blackWon || gameMode != PlayerType.Local || EventSystem.current.IsPointerOverGameObject())
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
        Direction sendDirection;
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
                sendDirection = Direction.West;
            }
            else if (x == selectedToken.currentX)
            {
                _capturedPiece = _core.MakeMove(selectedToken.currentX, selectedToken.currentY, Direction.Forward);
                sendDirection = Direction.Forward;
            }
            else /*if (x > selectedToken.currentX)*/
            {
                _capturedPiece = _core.MakeMove(selectedToken.currentX, selectedToken.currentY, Direction.East);
                sendDirection = Direction.East;
            }

            int inallcapsx = selectedToken.currentX;
            int inallcapsy = selectedToken.currentY;

            //actually move the token object visually
            MoveToken(x, y, xPos, zPos);

            //Send move over network if it's a network game
            if (gameMode == PlayerType.Network || blackPlayer == PlayerType.Network || whitePlayer == PlayerType.Network)
            {
                _core.SendNetworkMove(inallcapsx, inallcapsy, sendDirection);
            }
        }
        return isAllowed;
    }

	private void MoveToken(int x, int y, float tileXPos, float tileZPos)
    {
        //Tween the position 
        Vector3 newPosition = new Vector3();
        newPosition.x = tileXPos;
        newPosition.y = flatTokenYpos;
        newPosition.z = tileZPos;

        LeanTween.cancelAll();
        if (isPlayer1White)
        {
            LeanTween.rotate(selectedToken.gameObject, new Vector3(0, 0, 0), .01f);
        }
        else
        {
            LeanTween.rotate(selectedToken.gameObject, new Vector3(0, 180, 0), .01f);
        }
        LeanTween.move(selectedToken.gameObject, newPosition, .3f);
       // timeLeft = 60;
        
        string moveforLog = log.CoordsToNotations(selectedToken.currentX, selectedToken.currentY);

        selectedToken.SetBoardPosition (x, y);

        //destroy the captured token
        if (_capturedPiece != null)
        {
            Destroy(_capturedPiece);
            _capturedPiece = null;

            //add an x to the the log
            moveforLog += "x";

            //play the captured sound effect
            PlayMovementSoundEffect(true);
        }
        else
        {
            //play the normal sound effect
            PlayMovementSoundEffect(false);
        }

        moveforLog += log.CoordsToNotations(x, y);
        //send the move to the log
        log.ShowNotations(moveforLog, isWhiteTurn);

        //Debug.Log(moveforLog);
    
		BoardHighlights.Instance.HideHighlights ();
		selectedToken = null;

        if(!GameWon(y))
        {
            //toggle the turn
            ChangeTurn();
        }
        
    }

    private void GetMove()
    {
        //Split into AI and Network paths.
        if (gameMode == PlayerType.AI)
        {
            _core.PrepForForeignMove();
            ThreadStart aiRef = new ThreadStart(GetMoveHelper);
            Thread aiThread = new Thread(aiRef);
            aiThread.Start();
        }
        else if (gameMode == PlayerType.Network)
        {
            ThreadStart netRef = new ThreadStart(GetMoveHelper);
            Thread netThread = new Thread(netRef);
            netThread.Start();
        }
    }

    private void GetMoveHelper()
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
            if (isPlayer1White)
            {
                turnText.text = player1 + "'s Turn";
                AnimateAvatars(1);
            }
            else
            {
                turnText.text = player2 + "'s Turn";
                AnimateAvatars(2);
            }

            if (whitePlayer == PlayerType.AI)
            {
                gameMode = PlayerType.AI;
                GetMove();
            }
            else if (whitePlayer == PlayerType.Network)
            {
                gameMode = PlayerType.Network;              
                GetMove();
            }
            else if (whitePlayer == PlayerType.Local && blackPlayer == PlayerType.Local)
            {
                //enable white's emote button
                GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
                Button emote = player1.transform.GetChild(3).GetChild(2).gameObject.GetComponent<Button>();
                emote.interactable = true;

                //disable black's emote button 
                GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
                Button emote2 = player2.transform.GetChild(3).GetChild(2).gameObject.GetComponent<Button>();
                emote2.interactable = false;
            }
            //play a sound for the character
            //PlayTurnChangeSoundEffect(whiteCharacter);
        }
        else if(isBlackTurn)
        {
            if (!isPlayer1White)
            {
                turnText.text = player1 + "'s Turn";
                AnimateAvatars(1);
            }
            else
            {
                turnText.text = player2 + "'s Turn";
                AnimateAvatars(2);
            }
            if (blackPlayer == PlayerType.AI)
            {
                gameMode = PlayerType.AI;
                GetMove();
            }
            else if (blackPlayer == PlayerType.Network)
            {
                gameMode = PlayerType.Network;
                GetMove();
            }
            else if (whitePlayer == PlayerType.Local && blackPlayer == PlayerType.Local)
            {
                //disable white's emote button
                GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
                Button emote = player1.transform.GetChild(3).GetChild(2).gameObject.GetComponent<Button>();
                emote.interactable = false;

                //enable blacks's emote button 
                GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
                Button emote2 = player2.transform.GetChild(3).GetChild(2).gameObject.GetComponent<Button>();
                emote2.interactable = true;
            }
            //play a sound for the character
            //PlayTurnChangeSoundEffect(blackCharacter);
        }
    }

    private bool GameWon(int y)
    {
        //ask the game core if the game has been won
        _core.HasWon(y);

        //change the winnertext
        GameObject winTextobj = gameOverPanel.transform.GetChild(3).gameObject;
        Text winText = winTextobj.GetComponent<Text>();

        GameObject player1 = GameObject.FindWithTag("Player1");
        GameObject player2 = GameObject.FindWithTag("Player2");

        if (_core.whiteWon)
        {
            if (whitePlayer == PlayerType.Local && blackPlayer == PlayerType.Local)
            {
                //player1 win
                winText.text = "Player One Wins";
                SetCharacterWinLoseImage(true, true, whiteCharacter);
                SetCharacterWinLoseImage(false, false, blackCharacter);
                // StopCoroutine("LoseTime");
            }
            else if (whitePlayer == PlayerType.Local)
            {
                //you win
                winText.text = "You Won!";
                SetCharacterWinLoseImage(true, true, whiteCharacter);
                SetCharacterWinLoseImage(false, false, blackCharacter);
                //StopCoroutine("LoseTime");
            }
            else
            {
                //you lost
                winText.text = "You Lost!";
                SetCharacterWinLoseImage(true, false, whiteCharacter);
                SetCharacterWinLoseImage(false, true, blackCharacter);
            }

            if (whitePlayer == PlayerType.Local)
            {
                LeanTween.scale(player1, player1.transform.localScale * 1.5f, 0.1f);
                Vector3 moveTo;
                moveTo.x = player1.transform.position.x + 20;
                moveTo.y = player1.transform.position.y + 30;
                moveTo.z = player1.transform.position.z;
                LeanTween.move(player1, moveTo, 0.1f);
                //show/play win emote
                player1emote.PlayVictoryEmote();
            }
            else
            {
                LeanTween.scale(player2, player2.transform.localScale * 1.5f, 0.1f);
                Vector3 moveTo;
                moveTo.x = player2.transform.position.x - 20;
                moveTo.y = player2.transform.position.y - 5;
                moveTo.z = player2.transform.position.z;
                LeanTween.move(player2, moveTo, 0.1f);
                //show/play win emote
                player2emote.PlayVictoryEmote();
            }

            log.ShowWin(true);
            
            //show that the game was won and who won
            gameOverPanel.SetActive(true);
            return true;
        }
        else if (_core.blackWon)
        {
            if (blackPlayer == PlayerType.Local && whitePlayer == PlayerType.Local)
            {
                //player2 win
                winText.text = "Player Two Wins";
                SetCharacterWinLoseImage(true, false, whiteCharacter);
                SetCharacterWinLoseImage(false, true, blackCharacter);
                //StopCoroutine("LoseTime");
            }
            else if (blackPlayer == PlayerType.Local)
            {
                //you win
                winText.text = "You Won!";
                SetCharacterWinLoseImage(true, true, whiteCharacter);
                SetCharacterWinLoseImage(false, false, blackCharacter);
                //StopCoroutine("LoseTime");
            }
            else
            {
                //you lost
                winText.text = "You Lost!";
                SetCharacterWinLoseImage(true, false, whiteCharacter);
                SetCharacterWinLoseImage(false, true, blackCharacter);
            }

            if (blackPlayer == PlayerType.Local)
            {
                LeanTween.scale(player2, player2.transform.localScale * 1.15f, 0.1f);
                Vector3 moveTo;
                moveTo.x = player2.transform.position.x - 20;
                moveTo.y = player2.transform.position.y -5;
                moveTo.z = player2.transform.position.z;
                LeanTween.move(player2, moveTo, 0.1f);
                //show/play win emote
                player2emote.PlayVictoryEmote();
            }
            else
            {
                LeanTween.scale(player1, player1.transform.localScale * 1.15f, 0.1f);
                Vector3 moveTo;
                moveTo.x = player1.transform.position.x + 20;
                moveTo.y = player1.transform.position.y + 30;
                moveTo.z = player1.transform.position.z;
                LeanTween.move(player1, moveTo, 0.1f);
                //show/play win emote
                player1emote.PlayVictoryEmote();
            }

            log.ShowWin(false);

            //show that the game was won and who won
            gameOverPanel.SetActive(true);
            return true;
        }

        return false;
        //return _core.HasWon(y);
    }

    private void SetCharacterWinLoseImage(bool player1, bool won, int character)
    {
        Image playerImg = null;

        //setting player1's image
        if (player1)
        {
            playerImg = player1img.transform.GetChild(1).GetComponent<Image>();
        }
        //setting player2's image
        else
        {
            playerImg = player2img.transform.GetChild(1).GetComponent<Image>();
        }

        if (won)
        {
            //win pose
            if (character == 0)
            {
                //athena
                playerImg.sprite = AthenaWinImg;
            }
            else if (character == 1)
            {
                //ra
                playerImg.sprite = RaWinImg;
            }
            else
            {
                //thor
                playerImg.sprite = ThorWinImg;
            }
        }
        else
        {
            ////loss pose
            ////TODO: add in loss sprites when get them from Colin
            //if (character == 0)
            //{
            //    //athena
            //    playerImg.sprite = AthenaLoseImg;
            //}
            //else if (character == 1)
            //{
            //    //ra
            //    playerImg.sprite = RaLoseImg;
            //}
            //else
            //{
            //    //thor
            //    playerImg.sprite = ThorLoseImg;
            //}
        }
    }

    private void setPrefs()
    {
        //set gameMode, whiteplayer, and blackplayer variables based on input from character select screen
        int player1character = PlayerPrefs.GetInt("Player1Character");
        int player2character = PlayerPrefs.GetInt("Player2Character");

        //set the player portraits
        setCharacterImage(1, player1character);
        int gameIndex = SceneManager.GetActiveScene().buildIndex;
        if (gameIndex == 6)
        {
            gameMode = PlayerType.Local;
            //set the player2 character
            player2character = PlayerPrefs.GetInt("Player2Character");
        }
        else if (gameIndex == 7)
        {
            gameMode = PlayerType.Network;
            //more in another function because need the game core by then
        }
        else if (gameIndex == 8)
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
            //blackCam.enabled = false;
            //Camera.main.enabled = true;
            SetNotations(true);
            whiteCharacter = player1character;
            blackCharacter = player2character;
            isPlayer1White = true;
        }
        else if(player1white != 0 && gameMode != PlayerType.Local)
        {
            //player1 is black
            blackPlayer = PlayerType.Local;
            whitePlayer = gameMode;
            blackCam.enabled = true;
            Camera.main.enabled = false;
            SetNotations(false);
            blackCharacter = player1character;
            whiteCharacter = player2character;
            isPlayer1White = false;
        }
        gameMode = whitePlayer;
        //testfirst();

        //set the tokens
        TokenSetter tokenScript = GetComponent<TokenSetter>();
        tokenScript.SetTokens(player1white, player1character, player2character);

        //set the character for the emotes
        player1emote.SetCharacter(player1character);
        player2emote.SetCharacter(player2character);

        //set the log's characters
        log.SetCharacters(player1character, player2character);
    }

    private void setCharacterImage(int player, int character)
    {
        Image playerImg = null;
        Image playerBorder = null;
        float borderWidth = 0f;
        float borderHeight = 0f;

        bool isplayer1 = false;

        if (player == 1)
        {
            playerImg = player1img.transform.GetChild(1).GetComponent<Image>();
            playerBorder = player1img.transform.GetChild(2).GetComponent<Image>();
            isplayer1 = true;
        }
        else if (player == 2)
        {
            playerImg = player2img.transform.GetChild(1).GetComponent<Image>();
            playerBorder = player2img.transform.GetChild(2).GetComponent<Image>();
        }

        if (character == 0)
        {
            //Debug.Log("player " + player + " is Athena");
            playerImg.sprite = AthenaImg;
            playerBorder.sprite = AthenaBorder;
            borderWidth = 260;
            borderHeight = 260;

            //set the text for the turn indicator
            if (isplayer1)
            {
                player1 = "Athena";
            }
            else
            {
                player2 = "Athena";
            }
        }
        else if (character == 1)
        {
            //Debug.Log("player " + player + " is Ra");
            playerImg.sprite = RaImg;
            playerBorder.sprite = RaBorder;
            borderWidth = 275;
            borderHeight = 275;

            //set the text for the turn indicator
            if (isplayer1)
            {
                player1 = "Ra";
            }
            else
            {
                player2 = "Ra";
            }
        }
        else if (character == 2)
        {
            //Debug.Log("player " + player + " is Thor");
            playerImg.sprite = ThorImg;
            playerBorder.sprite = ThorBorder;
            borderWidth = 230;
            borderHeight = 230;

            //set the text for the turn indicator
            if (isplayer1)
            {
                player1 = "Thor";
            }
            else
            {
                player2 = "Thor";
            }
        }

        //set the turn indicator banner
        turnText.text = player1 + "'s Turn";

        //set the banners for each character
        Text player1BannerText = GameObject.FindGameObjectWithTag("Player1").transform.GetChild(2).GetComponent<Text>();
        player1BannerText.text = player1;

        Text player2BannerText = GameObject.FindGameObjectWithTag("Player2").transform.GetChild(2).GetComponent<Text>();
        player2BannerText.text = player2;

        playerBorder.rectTransform.sizeDelta = new Vector2(borderWidth, borderHeight);
    }

    //choose which notations and background to use
    private void SetNotations(bool first)
    {
        GameObject backgrounds = GameObject.FindGameObjectWithTag("Background");

        if (first)
        {
            notationsToggle.onValueChanged.AddListener(toggleNotations);
            //use normal background
            backgrounds.transform.GetChild(0).gameObject.SetActive(true);
            backgrounds.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            notationsToggle.onValueChanged.AddListener(toggleAltNotations);
            //use upside-down background
            backgrounds.transform.GetChild(0).gameObject.SetActive(false);
            backgrounds.transform.GetChild(1).gameObject.SetActive(true);
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

    public void PlayMovementSoundEffect(bool captured)
    {
        //make sure it will be able to play
        if (effectSource)
        {
            if (captured)
            {
                //choose the capture sound effect
                effectSource.clip = captureSound;
            }
            else
            {
                //choose the regular sound effect
                effectSource.clip = moveSound;
            }
            //play the clip
            effectSource.Play();
        }
    }

    public void PlayTurnChangeSoundEffect(int character)
    {
        //make sure it will be able to play
        if (effectSource)
        {
            if (character == 0)
            {
                //choose the athena turn change sound effect
                effectSource.clip = athenaTurnSound;
            }
            else if (character == 1)
            {
                //choose the ra turn change sound effect
                effectSource.clip = raTurnSound;
            }
            else if (character == 2)
            {
                //choose the thor turn change sound effect
                effectSource.clip = thorTurnSound;
            }
            //play the clip
            effectSource.Play();
        }
    }

    public GameCore GetGameCore()
    {
        return _core;
    }

    private void SetNetworkStuff()
    {
        netController.SetCore(_core);

        if (netController.isClient)
        {
            PlayerPrefs.SetInt("player1", 1);
        }
        else
        {
            PlayerPrefs.SetInt("player1", 0);
        }
    }

    private void AnimateAvatars(int player)
    {
        GameObject player1 = GameObject.FindWithTag("Player1");
        GameObject player2 = GameObject.FindWithTag("Player2");

        if (player == 1)
        {
            Vector3 oldScale = player1.transform.localScale;
            LeanTween.scale(player1, player1.transform.localScale * 1.1f, 0.1f);
            LeanTween.scale(player2, oldScale, 0.1f);
        }
        else if (player == 2)
        {
            Vector3 oldScale = player2.transform.localScale;
            LeanTween.scale(player2, player2.transform.localScale * 1.1f, 0.1f);
            LeanTween.scale(player1, oldScale, 0.1f);
        }
        else
        {
            //SHOULD NOT GET HERE
        }
    }

    public void Disconnect()
    {
        netController.Disconnect();
    }

    private void OnApplicationQuit()
    {
        if(netController && netController.IsConnected())
            Disconnect();
    }

    public void DestroyCoreOnAiGameOver()
    {
        _core = null;
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
    }
}



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
using Assets.Scripts;

public class BoardManager : MonoBehaviour {
    
    public static BoardManager Instance{ set; get;}

    public float[] xPosition = new float[8]
    {
        -4.371f, -3.121f, -1.871f, -0.621f, 0.629f, 1.879f, 3.129f, 4.379f, 
    };

    public float[] zPosition = new float[8]
    {
        -7.65f, -6.4f, -5.15f, -3.9f, -2.65f, -1.4f, -0.1500001f, 1.1f 
    };

    public Token selectedToken;
    public int selectionX = -1;
	public int selectionY = -1;

	private List<GameObject> activeToken;

	public bool isWhiteTurn = true;
    public PlayerType whitePlayer;
    public PlayerType blackPlayer;
    public PlayerType gameMode;

    private bool whiteWon = false;
	private bool blackWon = false;
    private bool tokenCaptured = false;

    private GameCore _core;

    // Use this for initialization
    void Start () {
		Instance = this;
        //set gameMode, whiteplayer, and blackplayer variables based on input from character select screen
        gameMode = PlayerType.Local;
        whitePlayer = PlayerType.Local;
        blackPlayer = PlayerType.Local;
        _core = new GameCore(whitePlayer, blackPlayer, );
	}

	// Update is called once per frame
	void Update ()
    {
        
	}

    public void TokenClicked(int x, int y, Token selected)
    {
        GameObject obj = activeToken[0];
        Token thisToken = obj.GetComponent<Token>();
        thisToken.x

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
		if(tokenCaptured){
            //capture the piece

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
            if (whitePlayer == PlayerType.AI)
            {
                gameMode = PlayerType.AI;
                //ask for an AI move from the game core
            }
            if (whitePlayer == PlayerType.Network)
            {
                gameMode = PlayerType.Network;
                //ask for an network move from the game core
            }
        }
        else if(!isWhiteTurn)
        {
            if (blackPlayer == PlayerType.AI)
            {
                gameMode = PlayerType.AI;
                //ask for an AI move from the game core
                List<int> whiteCoordinates = new List<int>();
                List<int> blackCoordinates = new List<int>();

                for (int row = 0; row < 8; row++)
                {
                    for(int col = 0; col < 8; col++)
                    {
                        if (GameCoreToken.tokens[row, col] == GameCoreToken.WHITE_SYMBOL)
                        {
                            whiteCoordinates.Add(row);
                            whiteCoordinates.Add(col);
                        }
                        else if (GameCoreToken.tokens[row, col] == GameCoreToken.BLACK_SYMBOL)
                        {
                            blackCoordinates.Add(row);
                            blackCoordinates.Add(col);
                        }
                    }
                }
                int fromX = 0, fromY = 0, toX = 0, toY = 0;
                GetMove(whiteCoordinates.ToArray(), whiteCoordinates.Count, blackCoordinates.ToArray(), blackCoordinates.Count,
                    ref fromX, ref fromY, ref toX, ref toY, 1);

                _move.blackFromX = fromX;
                _move.blackFromY = fromY;
                _move.blackToX = toX;
                _move.blackToY = toY;

                _move.RearrangeTokens();

                selectedToken = 
                Vector3 newPosition = new Vector3();
                newPosition.x = xPosition[toX];
                newPosition.y = selectedToken.gameObject.transform.position.y;
                newPosition.z = zPosition[toY];

            }
            if (blackPlayer == PlayerType.Network)
            {
                gameMode = PlayerType.Network;
                //ask for an network move from the game core
            }
        }
    }

    private void GameWon()
    {
        //show that the game was won and who won, ask about a rematch?
    }
}



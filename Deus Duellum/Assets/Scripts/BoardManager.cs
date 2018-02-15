using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {

    //CHANGE TO WORK WITH GAME CORE...

	public static BoardManager Instance{ set; get;}
	private bool[,] AllowedMoves{ set; get;}

	public Token selectedToken;

    public int selectionX = -1;
	public int selectionY = -1;

	private List<GameObject> activeToken;

	public bool isWhiteTurn = true;
	private bool whiteWon = false;
	private bool blackWon = false;

    // Use this for initialization
    void Start () {
		Instance = this;
	}

	// Update is called once per frame
	void Update () {

	}

    public void TokenClicked(int x, int y, Token selected)
    {
        //if the game is not over and same team
        if (!whiteWon && !blackWon && selected.isWhite == isWhiteTurn)
        {
            Debug.Log("selected: " + x + ", " + y);
            //select that token
            selectionX = x;
            selectionY = y;
            selectedToken = selected;

            //get the valid moves from game core

            //highlight those moves
        }
    }

    public void TileClicked(int x, int y, float xPos, float zPos)
    {
        //if a token is selected
        if (selectedToken != null)
        {
            Debug.Log("moving to: " + x + ", " + y);
            //if that spot is a valid move, then TWEEN the selected token there
            //also remove the other token if it was captured
            MoveToken(x, y, xPos, zPos);
        }
    }

    private void GameWon()
    {
        //show that the game was won and who won, ask about a rematch?
    }

	private void MoveToken(int x, int y, float tileXPos, float tileZPos)
    {
		bool tokenCaptured = false;
        //check with the gamecore that this is a valid move

        //ask the game core if a piece was captured and destroy it

        //Tween the position 
        Vector3 newPosition = new Vector3();
        newPosition.x = tileXPos;
        newPosition.y = selectedToken.gameObject.transform.position.y;
        newPosition.z = tileZPos;

        LeanTween.move(selectedToken.gameObject, newPosition, .3f);

        selectedToken.SetBoardPosition (x, y);

		//send the token's position to the log
		if(tokenCaptured){
			//add an x to the the log
		}

        //ask the game core if the game is over 
        GameWon();

        //toggle the turn
		isWhiteTurn = !isWhiteTurn;
		BoardHighlights.Instance.HideHighlights ();
		selectedToken = null;
	}
}



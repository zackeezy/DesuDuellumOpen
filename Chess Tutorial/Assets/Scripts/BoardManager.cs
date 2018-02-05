using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {
	public static BoardManager Instance{ set; get;}
	private bool[,] allowedMoves{ set; get;}

	public Token[,] Tokens{ set; get; }
	private Token selectedToken;

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;

	private int selectionX = -1;
	private int selectionY = -1;

	public List<GameObject> tokenPrefabs;
	private List<GameObject> activeToken;

	public bool isWhiteTurn = true;
	private bool whiteWon = false;
	private bool blackWon = false;

	public Text winText;
    public GameObject GameWonPanel;
    public GameObject GameWonText;

	// Use this for initialization
	void Start () {
		//SpawnToken (0, GetTileCenter(0, 0));
		Instance = this;
		SpawnAll();
        GameWonPanel.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		DrawBoard ();
		UpdateSelection ();

		if (Input.GetMouseButtonDown (0) && !whiteWon && !blackWon) {
			if (selectionX >= 0 && selectionY >= 0) {
				if (selectedToken == null) {
				//Select the token
					SelectToken(selectionX,selectionY);
				} 
				else {
				//move token
					MoveToken(selectionX,selectionY);
				}
			}
		}
	}

    private void YouWon()
    {
        GameWonText.SetActive(true);
        GameWonPanel.SetActive(true);
    }

	private void SelectToken(int x, int y){
		if (Tokens [x, y] == null) {
			return;
		}
			
		if (Tokens [x, y].isWhite != isWhiteTurn) {
			return;
		}
		allowedMoves = Tokens [x, y].PossibleMove ();
		selectedToken = Tokens [x, y];
		BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves);
	}

	private void MoveToken(int x, int y){
		bool tokenCaptured = false;
		if (allowedMoves[x,y]) {
			Token c = Tokens [x, y];
		
			if (c != null && c.isWhite != isWhiteTurn) {
				
				activeToken.Remove (c.gameObject);
				Destroy (c.gameObject);
				tokenCaptured = true;
			}

			Tokens [selectedToken.currentX, selectedToken.currentY] = null;
			selectedToken.transform.position = GetTileCenter (x, y);
			selectedToken.SetPosition (x, y);
			Tokens [x, y] = selectedToken;

			//send the token's position to the log
			if(tokenCaptured){
				//add an x to the the log
			}

			//If it reaches the enemy end line...
			if(isWhiteTurn && y == 7){
				//game over, white wins
				whiteWon = true;
				winText.text = "White Wins!";
                YouWon();
                //reset everything back to its place
                foreach (GameObject gameObject in activeToken)
                {
                    Destroy(gameObject);
                }
                isWhiteTurn = true;
                BoardHighlights.Instance.HideHighlights();
                SpawnAll();
            }
			if (!isWhiteTurn && y == 0) {
				//game over, black wins
				blackWon = true;
				winText.text = "Black Wins!";
                YouWon();
                //reset everything back to its place
                foreach (GameObject gameObject in activeToken)
                {
                    Destroy(gameObject);
                }
                isWhiteTurn = true;
                BoardHighlights.Instance.HideHighlights();
                SpawnAll();
            }
			isWhiteTurn = !isWhiteTurn;
		}
		BoardHighlights.Instance.HideHighlights ();
		selectedToken = null;
	}

	private void SpawnToken(int index, int x, int y){
		GameObject go = Instantiate (tokenPrefabs [index], GetTileCenter(x,y), Quaternion.identity) as GameObject;
		go.transform.SetParent (transform);
		Tokens[x,y]=go.GetComponent<Token>();
		Tokens [x, y].SetPosition (x, y);
		activeToken.Add (go);
	}

	private Vector3 GetTileCenter(int x, int y){
		Vector3 origin = Vector3.zero;
		origin.x += (TILE_SIZE * x) + TILE_OFFSET;
		origin.z += (TILE_SIZE * y) + TILE_OFFSET;
		return origin;
	}

	private void SpawnAll(){
		activeToken = new List<GameObject> ();
		Tokens = new Token[8, 8];

		//spawn "white"
		for(int i = 0; i < 8; i++){
			SpawnToken (1, i, 0);
			SpawnToken (1, i, 1);
		}

		//spawn "black"
		for(int i = 0; i < 8; i++){
			SpawnToken (2, i, 6);
			SpawnToken (2, i, 7);
		}
	}

	private void UpdateSelection(){
		if (!Camera.main) {
			return;
		}

		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 50.0f, LayerMask.GetMask ("BoardPlane"))) {
			//Debug.Log (hit.point);
			selectionX = (int)hit.point.x;
			selectionY = (int)hit.point.z;
		} else {
			selectionX = -1;
			selectionY = -1;
		}
			
	}
			
	private void DrawBoard(){
		Vector3 widthline = Vector3.right * 8;
		Vector3 heightline = Vector3.forward * 8;

		for (int i = 0; i <= 8; i++) {
			Vector3 start = Vector3.forward * i;
			Debug.DrawLine (start, start + widthline);

			for (int j = 0; j <= 8; j++) {
				start = Vector3.right * j;
				Debug.DrawLine (start, start + heightline);
			}
		}

		//draw the selection
		if(selectionX >=0 && selectionY >= 0){
			Debug.DrawLine (
				Vector3.forward * selectionY + Vector3.right * selectionX,
				Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

			Debug.DrawLine (
				Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
				Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
		}
	}
}



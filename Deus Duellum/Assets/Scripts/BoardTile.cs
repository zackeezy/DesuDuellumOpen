using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour {

    public int xCoordinate;
    public int yCoordinate;

	// Use this for initialization
	void Start () {
		
	}

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //they clicked on a tile!
            //tell the game core its coordinates, get valid moves back to highlight and tell the highlightmanager
            BoardManager.Instance.TileClicked(xCoordinate, yCoordinate, transform.position.x, transform.position.z);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}

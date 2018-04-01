using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardHighlights : MonoBehaviour {

	public static BoardHighlights Instance{ get; set;}
	public GameObject highlightPrefabLight;
    public GameObject highlightPrefabDark;

    private List<GameObject> highlights;

	private void Start(){
        Instance = this;
        highlights = new List<GameObject>();
    }
     
    public void HighlightTile(int x, int y)
    {
        //get the tile at that spot, activate the highlight gameobject
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tiles");
        foreach(GameObject t in tiles)
        {
            BoardTile tile = t.GetComponent<BoardTile>();
            if(tile.xCoordinate == x && tile.yCoordinate == y)
            {
                GameObject highlight = t.transform.GetChild(0).gameObject;
                highlight.SetActive(true);
                highlights.Add(highlight);
            }
        }
    }
    //still need the tag name for pieces...

    //public void HighlightPiece(int x, int y,Token selected)
    //{
    //    //get the tile at that spot, activate the highlight gameobject
    //    GameObject[] pieces = GameObject.FindGameObjectsWithTag("WhitePieces");
    //    foreach (GameObject p in pieces)
    //    {
    //        BoardPiece piece = p.GetComponent<BoardPiece>();
    //        if (piece.xCoordinate == x && piece.yCoordinate == y)
    //        {
    //            GameObject highlight = piece.transform.GetChild(0).gameObject;
    //            highlight.SetActive(true);
    //            highlights.Add(highlight);
    //        }
    //    }
    //}

    public void HideHighlights()
    {
        foreach (GameObject gameObject in highlights)
        {
            gameObject.SetActive(false);
        }
    }
}

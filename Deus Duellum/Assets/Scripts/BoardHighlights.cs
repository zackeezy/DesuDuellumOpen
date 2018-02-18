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

    public void HideHighlights()
    {
        foreach (GameObject gameObject in highlights)
        {
            gameObject.SetActive(false);
        }
    }
}

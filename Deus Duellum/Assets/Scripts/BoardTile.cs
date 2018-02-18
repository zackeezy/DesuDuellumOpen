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
            BoardManager.Instance.TileClicked(xCoordinate, yCoordinate, transform.position.x, transform.position.z);
        }
        //Debug.Log(transform.position.x + ", " + transform.position.z);
    }

    // Update is called once per frame
    void Update () {
		
	}
}

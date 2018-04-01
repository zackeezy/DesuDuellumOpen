using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece : MonoBehaviour {

    public int xCoordinate;
    public int yCoordinate;
    public Token selected;

    // Use this for initialization
    void Start()
    {

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //they clicked on a piece!
            BoardManager.Instance.TokenClicked(xCoordinate,yCoordinate,selected);
            //Debug.Log(transform.position.x + ", " + transform.position.z);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

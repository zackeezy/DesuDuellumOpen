using UnityEngine;
using System.Collections;


public class Token : MonoBehaviour
{
    //CHANGE TO WORK WITH GAME CORE...

    public int currentX;
    public int currentY;
    public bool isWhite;
    public bool isBlack
    {
        get { return !isWhite; }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //they clicked on a token!
            BoardManager.Instance.TokenClicked(currentX, currentY, this);
        }
    }

    public void SetBoardPosition(int x, int y)
    {
        currentX = x;
        currentY = y;
    }
}

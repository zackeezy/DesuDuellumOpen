using UnityEngine;
using System.Collections;

public class Token : MonoBehaviour {

	public int currentX{ get; set;}
	public int currentY{ get; set;}
	public bool isWhite;

	public void SetPosition(int x, int y)
	{
		currentX = x;
		currentY = y;
	}
	public bool[,] PossibleMove(){
		bool[,]r = new bool [8,8] ;
		Token c;

		//White team move
		if (isWhite) {
			//Diagonal Left
			if (currentX != 0 && currentY != 7) {
				c = BoardManager.Instance.Tokens [currentX - 1, currentY + 1];
				//if c!=null
				if (c != null && !c.isWhite) {
					r [currentX - 1, currentY + 1] = true;
				} else if (c == null) {
					r [currentX - 1, currentY + 1] = true;
				}
			}
			//Diagonal right
			if (currentX != 7 && currentY != 7) {
				c = BoardManager.Instance.Tokens [currentX + 1, currentY + 1];
				//if c!=null CHECK THIS LATER DAWG
				if (c!=null && !c.isWhite) {
					r [currentX + 1, currentY + 1] = true;
				} else if (c == null) {
					r [currentX + 1, currentY + 1] = true;
				}
			}
			//Middle
			if (currentY != 7) {
				c = BoardManager.Instance.Tokens [currentX, currentY + 1];
				if (c == null) {
					r [currentX, currentY + 1] = true;
				}
			}

		} else {
			//Diagonal Left
			if(currentX != 0 && currentY != 0){
				c = BoardManager.Instance.Tokens [currentX - 1, currentY - 1];
				//if c!=null CHECK THIS LATER DAWG
				if (c!=null && c.isWhite) {
					r [currentX - 1, currentY - 1]=true;
				} else if (c == null) {
					r [currentX - 1, currentY - 1] = true;
				}
			}
			//Diagonal right
			if(currentX != 7 && currentY != 0){
				c = BoardManager.Instance.Tokens [currentX + 1, currentY - 1];
				//if c!=null CHECK THIS LATER DAWG
				if (c!=null && c.isWhite) {
					r [currentX + 1, currentY - 1]=true;
				} else if (c == null) {
					r [currentX + 1, currentY - 1] = true;
				}

			}
			//Middle
			if(currentY != 0){
				c = BoardManager.Instance.Tokens [currentX, currentY - 1];
				if (c == null) {
					r [currentX, currentY - 1] = true;
				}
			}
		}
		return r;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenSetter : MonoBehaviour {

    public GameObject AthenaPrefabLight;
    public GameObject AthenaPrefabDark;
    public GameObject RaPrefabLight;
    public GameObject RaPrefabDark;
    public GameObject ThorPrefabLight;
    public GameObject ThorPrefabDark;

    private BoardManager manager;

    //hey git i made a change

    public void SetTokens(int player1First, int player1Character, int player2Character)
    {
        GameObject[] blackPieces = GameObject.FindGameObjectsWithTag("BlackPieces");
        GameObject[] whitePieces = GameObject.FindGameObjectsWithTag("WhitePieces");

        manager = GetComponent<BoardManager>();

        //set players' tokens to light or dark for their chosen character
        if (player1First == 0)
        {
            //player 1 is light
            if (player1Character == 0)
            {
                //Athena light
                foreach(GameObject oldPiece in whitePieces)
                {
                    GameObject AthenaLight = Instantiate(AthenaPrefabLight, oldPiece.transform.position, oldPiece.transform.rotation);
                    AthenaLight.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    AthenaLight.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    AthenaLight.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    AthenaLight.gameObject.tag = "WhitePieces";
                    manager.boardTokens.Add(AthenaLight);
                    //add to the game core

                    Destroy(oldPiece);
                }
            }
            else if (player1Character == 1)
            {
                //Ra light
                foreach (GameObject oldPiece in whitePieces)
                {
                    GameObject RaLight = Instantiate(RaPrefabLight, oldPiece.transform.position, oldPiece.transform.rotation);
                    RaLight.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    RaLight.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    RaLight.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    RaLight.gameObject.tag = "WhitePieces";
                    manager.boardTokens.Add(RaLight);

                    //add to the game core

                    Destroy(oldPiece);
                }
            }
            else if (player1Character == 2)
            {
                //Thor light
                foreach (GameObject oldPiece in whitePieces)
                {
                    GameObject ThorLight = Instantiate(ThorPrefabLight, oldPiece.transform.position, oldPiece.transform.rotation);
                    ThorLight.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    ThorLight.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    ThorLight.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    ThorLight.gameObject.tag = "WhitePieces";
                    manager.boardTokens.Add(ThorLight);

                    //add to the game core

                    Destroy(oldPiece);
                }
            }

            //player 2 is dark 
            if (player2Character == 0)
            {
                //Athena dark
                foreach (GameObject oldPiece in blackPieces)
                {
                    GameObject AthenaDark = Instantiate(AthenaPrefabDark, oldPiece.transform.position, oldPiece.transform.rotation);
                    AthenaDark.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    AthenaDark.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    AthenaDark.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    AthenaDark.gameObject.tag = "BlackPieces";
                    manager.boardTokens.Add(AthenaDark);

                    //add to the game core

                    Destroy(oldPiece);
                }
            }
            else if (player2Character == 1)
            {
                //Ra dark
                foreach (GameObject oldPiece in blackPieces)
                {
                    GameObject RaDark = Instantiate(RaPrefabDark, oldPiece.transform.position, oldPiece.transform.rotation);
                    RaDark.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    RaDark.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    RaDark.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    RaDark.gameObject.tag = "BlackPieces";
                    manager.boardTokens.Add(RaDark);

                    //add to the game core

                    Destroy(oldPiece);
                }
            }
            else if (player2Character == 2)
            {
                //Thor dark
                foreach (GameObject oldPiece in blackPieces)
                {
                    GameObject ThorDark = Instantiate(ThorPrefabDark, oldPiece.transform.position, oldPiece.transform.rotation);
                    ThorDark.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    ThorDark.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    ThorDark.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    ThorDark.gameObject.tag = "BlackPieces";
                    manager.boardTokens.Add(ThorDark);

                    //add to the game core

                    Destroy(oldPiece);
                }
            }
        }
        else
        {
            //player1 uses dark, player2 uses light
            //player 1 is dark
            if (player1Character == 0)
            {
                //Athena dark
                foreach (GameObject oldPiece in blackPieces)
                {
                    GameObject AthenaDark = Instantiate(AthenaPrefabDark, oldPiece.transform.position, oldPiece.transform.rotation);
                    AthenaDark.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    AthenaDark.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    AthenaDark.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    AthenaDark.gameObject.tag = "BlackPieces";
                    manager.boardTokens.Add(AthenaDark);

                    //add to the game core

                    Destroy(oldPiece);
                }
            }
            else if (player1Character == 1)
            {
                //Ra dark
                foreach (GameObject oldPiece in blackPieces)
                {
                    GameObject RaDark = Instantiate(RaPrefabDark, oldPiece.transform.position, oldPiece.transform.rotation);
                    RaDark.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    RaDark.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    RaDark.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    RaDark.gameObject.tag = "BlackPieces";
                    manager.boardTokens.Add(RaDark);

                    //add to the game core

                    Destroy(oldPiece);
                }
            }
            else if (player1Character == 2)
            {
                //Thor dark
                foreach (GameObject oldPiece in blackPieces)
                {
                    GameObject ThorDark = Instantiate(ThorPrefabDark, oldPiece.transform.position, oldPiece.transform.rotation);
                    ThorDark.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    ThorDark.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    ThorDark.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    ThorDark.gameObject.tag = "BlackPieces";
                    manager.boardTokens.Add(ThorDark);

                    //add to the game core

                    Destroy(oldPiece);
                }
            }

            //player 2 is light 
            if (player2Character == 0)
            {
                //Athena light
                foreach (GameObject oldPiece in whitePieces)
                {
                    GameObject AthenaLight = Instantiate(AthenaPrefabLight, oldPiece.transform.position, oldPiece.transform.rotation);
                    AthenaLight.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    AthenaLight.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    AthenaLight.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    AthenaLight.gameObject.tag = "WhitePieces";
                    manager.boardTokens.Add(AthenaLight);

                    //add to the game core

                    Destroy(oldPiece);
                }
            }
            else if (player2Character == 1)
            {
                //Ra light
                foreach (GameObject oldPiece in whitePieces)
                {
                    GameObject RaLight = Instantiate(RaPrefabLight, oldPiece.transform.position, oldPiece.transform.rotation);
                    RaLight.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    RaLight.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    RaLight.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    RaLight.gameObject.tag = "WhitePieces";
                    manager.boardTokens.Add(RaLight);

                    //add to the game core

                    Destroy(oldPiece);
                }
            }
            else if (player2Character == 2)
            {
                //Thor light
                foreach (GameObject oldPiece in whitePieces)
                {
                    GameObject ThorLight = Instantiate(ThorPrefabLight, oldPiece.transform.position, oldPiece.transform.rotation);
                    ThorLight.GetComponent<Token>().currentX = oldPiece.GetComponent<Token>().currentX;
                    ThorLight.GetComponent<Token>().currentY = oldPiece.GetComponent<Token>().currentY;
                    ThorLight.GetComponent<Token>().isWhite = oldPiece.GetComponent<Token>().isWhite;
                    ThorLight.gameObject.tag = "WhitePieces";
                    manager.boardTokens.Add(ThorLight);

                    //add to the game core

                    Destroy(oldPiece);
                }
            }
        }
    }
}

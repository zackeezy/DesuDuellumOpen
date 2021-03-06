﻿using System.Collections;
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

    public void SetTokens(int player1First, int player1Character, int player2Character)
    {
        GameObject[] blackPieces = GameObject.FindGameObjectsWithTag("BlackPieces");
        GameObject[] whitePieces = GameObject.FindGameObjectsWithTag("WhitePieces");

        manager = GetComponent<BoardManager>();

        //set players' tokens to light or dark for their chosen character
        if (player1First == 0)
        {
            Debug.Log("player1 is white");

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
                    //add to the game core
                    manager.boardTokens.Add(AthenaLight);

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
                    //add to the game core
                    manager.boardTokens.Add(RaLight);

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
                    //add to the game core
                    manager.boardTokens.Add(ThorLight);

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
                    //add to the game core
                    manager.boardTokens.Add(AthenaDark);

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
                    //add to the game core
                    manager.boardTokens.Add(RaDark);

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
                    //add to the game core
                    manager.boardTokens.Add(ThorDark);

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
                    AthenaDark.transform.Rotate(0, 180, 0);
                    AthenaDark.gameObject.tag = "BlackPieces";
                    //add to the game core
                    manager.boardTokens.Add(AthenaDark);

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
                    RaDark.transform.Rotate(0, 180, 0);
                    //add to the game core
                    manager.boardTokens.Add(RaDark);

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
                    ThorDark.transform.Rotate(0, 180, 0);
                    //add to the game core
                    manager.boardTokens.Add(ThorDark);

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
                    AthenaLight.transform.Rotate(0, 180, 0);
                    //add to the game core
                    manager.boardTokens.Add(AthenaLight);

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
                    RaLight.transform.Rotate(0, 180, 0);
                    //add to the game core
                    manager.boardTokens.Add(RaLight);

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
                    ThorLight.transform.Rotate(0, 180, 0);
                    //add to the game core
                    manager.boardTokens.Add(ThorLight);

                    Destroy(oldPiece);
                }
            }
        }
    }
}

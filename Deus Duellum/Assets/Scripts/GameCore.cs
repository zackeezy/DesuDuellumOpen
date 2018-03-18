using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Runtime.InteropServices;



namespace Assets.Scripts
{
    public enum Direction
    {
        East = 0,
        Forward = 1,
        West = 2,
    }

    public class GameCore
    {
        public readonly PlayerType WhitePlayer;
        public readonly PlayerType BlackPlayer;

        public bool IsWhiteTurn;
        public bool IsBlackTurn
        {
            get { return !IsWhiteTurn; }
        }
        public bool whiteWon = false;
        public bool blackWon = false;
        public int whiteCapturedPieces = 0;
        public int blackCapturedPieces = 0;
        public GameObject[,] Board;

        public GameCore(PlayerType white, PlayerType black, List<GameObject> tokens)
        {
            WhitePlayer = white;
            BlackPlayer = black;

            Board = new GameObject[8, 8];

            foreach (GameObject token in tokens)
            {
                Board[token.GetComponent<Token>().currentX, token.GetComponent<Token>().currentY] = token;
            }

            IsWhiteTurn = true;
        }

        public void MoveCoordinates(ref int x, ref int y, Direction direction, bool white)
        {
            if (white)
            {
                y += 1; 
            }
            else
            {
                y -= 1;
            }
               
            if (direction == Direction.East)
            {
                x += 1;
            }
            else if (direction == Direction.West)
            {
                x -= 1;
            }
            else if (direction == Direction.Forward)
            {
                //Do Nothing
            }
        }

        public void PrepForForeignMove()
        {
            if (IsWhiteTurn)
            {
                if (WhitePlayer == PlayerType.AI)
                {
                    foreach (GameObject space in Board)
                    {
                        if (space != null)
                        {
                            if (space.GetComponent<Token>().isWhite)
                            {
                                FillOrigin(space.GetComponent<Token>().currentX, space.GetComponent<Token>().currentY, 0);
                            }
                            else if (space.GetComponent<Token>().isBlack)
                            {
                                FillOrigin(space.GetComponent<Token>().currentX, space.GetComponent<Token>().currentY, 1);
                            }
                        }
                    }
                }
                else if (WhitePlayer == PlayerType.Network)
                {
                    //Prep for Network Call on Main Thread.
                }
                else
                {
                    throw new Exception("That Player Is Local.");
                }
            }
            else if (IsBlackTurn)
            {
                if (BlackPlayer == PlayerType.AI)
                {
                    foreach (GameObject space in Board)
                    {
                        if (space != null)
                        {
                            if (space.GetComponent<Token>().isWhite)
                            {
                                FillOrigin(space.GetComponent<Token>().currentX, space.GetComponent<Token>().currentY, 0);
                            }
                            else if (space.GetComponent<Token>().isBlack)
                            {
                                FillOrigin(space.GetComponent<Token>().currentX, space.GetComponent<Token>().currentY, 1);
                            }
                        }
                    }
                }
                else if (BlackPlayer == PlayerType.Network)
                {
                    //Prep for Network Call on Main Thread.
                }
                else
                {
                    throw new Exception("That Player Is Local.");
                }
            }
            
        }

        public void GetMove(ref int x, ref int y, ref Direction direction)
        {
            if (IsWhiteTurn)
            {
                if (WhitePlayer == PlayerType.AI)
                {
                    GetAiMove(ref x, ref y, ref direction);
                }
                else if(WhitePlayer == PlayerType.Network)
                {
                    GetNetworkMove(ref x, ref y, ref direction);
                }
                else
                {
                    throw new Exception("That Player Is Local.");
                }
            }
            else if (IsBlackTurn)
            {
                if (BlackPlayer == PlayerType.AI)
                {
                    GetAiMove(ref x, ref y, ref direction);
                }
                else if (BlackPlayer == PlayerType.Network)
                {
                    GetNetworkMove(ref x, ref y, ref direction);
                }
                else
                {
                    throw new Exception("That Player Is Local.");
                }
            }
        }

        private void GetAiMove(ref int x, ref int y, ref Direction direction)
        {
            int fromX = 0, fromY = 0, dir = 0, color = 0;

            color = IsWhiteTurn ? 0 : 1;

            GenerateMove(ref fromX, ref fromY, ref dir, color);
            x = fromX;
            y = fromY;

            direction = (Direction)dir;
        }

        [DllImport("AI_CPP", CallingConvention = CallingConvention.StdCall)]
        public static extern void GenerateMove(ref int fromX, ref int fromY, ref int direction, int color);

        [DllImport("AI_CPP", CallingConvention = CallingConvention.StdCall)]
        public static extern void FillOrigin(int x, int y, int color);

        private void GetNetworkMove(ref int x, ref int y, ref Direction direction)
        {
            throw new NotImplementedException("We Dont Have Networking Yet!!!!!!1!?;)");
        }

        public bool IsMoveAllowed(int x, int y, Direction direction)
        {
            bool isAllowed = false;

            if (IsWhiteTurn && Board[x,y] != null && Board[x,y].GetComponent<Token>().isWhite)
            {
                if (direction == Direction.Forward)
                {
                    if (y != 7 && Board[x, y + 1] == null)
                    {
                        isAllowed = true;
                    }
                }
                else if (x != 7 && y != 7 && direction == Direction.East)
                {
                    if (Board[x + 1, y + 1] == null || Board[x + 1, y + 1].GetComponent<Token>().isBlack)
                    {
                        isAllowed = true;
                    }
                }
                else if (x != 0 && y != 7 && direction == Direction.West)
                {
                    if (Board[x - 1, y + 1] == null || Board[x - 1, y + 1].GetComponent<Token>().isBlack)
                    {
                        isAllowed = true;
                    }
                }
            }
            else if(IsBlackTurn && Board[x, y] != null && Board[x, y].GetComponent<Token>().isBlack)
            {
                if (y != 0 && direction == Direction.Forward)
                {
                    if (Board[x, y - 1] == null)
                    {
                        isAllowed = true;
                    }
                }
                else if (y != 0 && x != 7 && direction == Direction.East)
                {
                    if (Board[x + 1,y - 1] == null || Board[x + 1, y - 1].GetComponent<Token>().isWhite)
                    {
                        isAllowed = true;
                    }
                }
                else if (y != 0 && x != 0 && direction == Direction.West)
                {
                    if (Board[x - 1, y - 1] == null || Board[x - 1, y - 1].GetComponent<Token>().isWhite)
                    {
                        isAllowed = true;
                    }
                }
            }

            return isAllowed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Will return null if a piece was not captured, and the captured piece if it a piece was captured.</returns>
        public GameObject MakeMove(int x, int y, Direction direction)
        {
            GameObject capturedPiece = null;

            if (IsWhiteTurn && IsMoveAllowed(x, y, direction))
            {
                GameObject temp = Board[x, y];
                Board[x, y] = null;
                if (direction == Direction.Forward)
                {
                    if(Board[x, y + 1] != null)
                    {
                        capturedPiece = Board[x, y + 1];
                        whiteCapturedPieces++;
                    }
                    Board[x, y + 1] = temp;
                }
                else if (direction == Direction.East)
                {
                    if (Board[x + 1, y + 1] != null)
                    {
                        capturedPiece = Board[x + 1, y + 1];
                        whiteCapturedPieces++;
                    }
                    Board[x + 1, y + 1] = temp;
                }
                else if (direction == Direction.West)
                {
                    if (Board[x - 1, y + 1] != null)
                    {
                        capturedPiece = Board[x - 1, y + 1];
                        whiteCapturedPieces++;
                    }
                    Board[x - 1, y + 1] = temp;
                }

                IsWhiteTurn = !IsWhiteTurn;
            }
            else if (IsBlackTurn && IsMoveAllowed(x, y, direction))
            {
                GameObject temp = Board[x, y];
                Board[x, y] = null;
                if (direction == Direction.Forward)
                {
                    if (Board[x, y - 1] != null)
                    {
                        capturedPiece = Board[x, y - 1];
                        blackCapturedPieces++;
                    }
                    Board[x, y - 1] = temp;
                }
                else if (direction == Direction.East)
                {
                    if (Board[x + 1, y - 1] != null)
                    {
                        capturedPiece = Board[x + 1, y - 1];
                        blackCapturedPieces++;
                    }
                    Board[x + 1, y - 1] = temp;
                }
                else if (direction == Direction.West)
                {
                    if (Board[x - 1, y - 1] != null)
                    {
                        capturedPiece = Board[x - 1, y - 1];
                        blackCapturedPieces++;
                    }
                    Board[x - 1, y - 1] = temp;
                }

                IsWhiteTurn = !IsWhiteTurn;
            }
            else
            {
                throw new ArgumentException("You attempted an illegal move.");
            }

            return capturedPiece;
        }

        //check if white or black won 
        public bool HasWon(int y)
        {
            //if it is white's turn and y is equal to 7 than white has won
            if (IsBlackTurn && y == 7 || whiteCapturedPieces == 16)
            {
                whiteWon = true;
            }
            //if it is black's turn and y is equal to 0 than black has won
            else if (IsWhiteTurn && y == 0 || blackCapturedPieces == 16)
            {
                blackWon = !whiteWon;
            }
            return whiteWon;
        }
    }
}

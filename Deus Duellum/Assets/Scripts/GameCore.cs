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

        public readonly bool IsWhiteTurn;
        public bool IsBlackTurn
        {
            get { return !IsWhiteTurn; }
        }

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
            List<int> whiteCoords = new List<int>();
            List<int> blackCoords = new List<int>();

            foreach (GameObject space in Board)
            {
                if (space != null)
                {
                    if (space.GetComponent<Token>().isWhite)
                    {
                        whiteCoords.Add(space.GetComponent<Token>().currentX);
                        whiteCoords.Add(space.GetComponent<Token>().currentY);
                    }
                    else if (space.GetComponent<Token>().isBlack)
                    {
                        blackCoords.Add(space.GetComponent<Token>().currentX);
                        blackCoords.Add(space.GetComponent<Token>().currentY);
                    }
                }
            }

            int fromX = 0, fromY = 0, dir = 0, color = 0;

            color = IsWhiteTurn ? 0 : 1;

            AccessAiForAMove(whiteCoords.ToArray(), whiteCoords.Count, blackCoords.ToArray(), blackCoords.Count,
               ref fromX, ref fromY, ref dir, color);
            x = fromX;
            y = fromY;

            direction = (Direction)dir;
        }

        [DllImport("AI_CPP", CallingConvention = CallingConvention.StdCall)]
        public static extern int AccessAiForAMove(int[] whiteCoordinates, int whiteCount,
        int[] blackCoordinates, int blackCount,
        ref int fromX, ref int fromY, ref int direction,
        int color);

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
                    if (Board[x, y + 1] == null)
                    {
                        isAllowed = true;
                    }
                }
                else if (direction == Direction.East)
                {
                    if (Board[x + 1, y + 1] == null || Board[x + 1, y + 1].GetComponent<Token>().isBlack)
                    {
                        isAllowed = true;
                    }
                }
                else if (direction == Direction.West)
                {
                    if (Board[x - 1, y + 1] == null || Board[x - 1, y + 1].GetComponent<Token>().isBlack)
                    {
                        isAllowed = true;
                    }
                }
            }
            else if(IsBlackTurn && Board[x, y] != null && Board[x, y].GetComponent<Token>().isBlack)
            {
                if (direction == Direction.Forward)
                {
                    if (Board[x, y - 1] == null)
                    {
                        isAllowed = true;
                    }
                }
                else if (direction == Direction.East)
                {
                    if (Board[x + 1,y - 1] == null || Board[x + 1, y - 1].GetComponent<Token>().isWhite)
                    {
                        isAllowed = true;
                    }
                }
                else if (direction == Direction.West)
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
                    }
                    Board[x, y + 1] = temp;
                }
                else if (direction == Direction.East)
                {
                    if (Board[x + 1, y + 1] != null)
                    {
                        capturedPiece = Board[x + 1, y + 1];
                    }
                    Board[x + 1, y + 1] = temp;
                }
                else if (direction == Direction.West)
                {
                    if (Board[x - 1, y + 1] != null)
                    {
                        capturedPiece = Board[x - 1, y + 1];
                    }
                    Board[x - 1, y + 1] = temp;
                }
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
                    }
                    Board[x, y - 1] = temp;
                }
                else if (direction == Direction.East)
                {
                    if (Board[x + 1, y - 1] != null)
                    {
                        capturedPiece = Board[x + 1, y - 1];
                    }
                    Board[x + 1, y - 1] = temp;
                }
                else if (direction == Direction.West)
                {
                    if (Board[x - 1, y - 1] != null)
                    {
                        capturedPiece = Board[x - 1, y - 1];
                    }
                    Board[x - 1, y - 1] = temp;
                }
            }
            else
            {
                throw new ArgumentException("You attempted an illegal move.");
            }

            return capturedPiece;
        }
    }
}

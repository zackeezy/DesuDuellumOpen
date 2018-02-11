using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakthrough
{
   public class Move : Token
    {
        private int fromX;
        private int fromY;
        private int toX;
        private int toY;

        public bool Exit { set; get; }
        public bool isWhiteTurn = true;
        public bool isWhite=true;

        public Move()
        {
            fromX = 0;
            fromY = 0;
            toX = 0;
            toY = 0;
            Exit = false;
        }

        public void GetMove()
        {
            GetInput();

            if (!Exit /*&& allowMove()*/)
            {
                RearrangeTokens();
            }
        }
        private void GetInput()
        {
           if(isWhiteTurn)
            {
                //get input and validate it
                Console.WriteLine("Enter White Token's from coordinate X");
                Exit = whiteValidateInput(int.TryParse(Console.ReadLine(), out fromX));

                if (!Exit)//if previous validation passed, move to the next coordinates
                {
                    Console.WriteLine("Enter White Token's from coordinate Y");
                    Exit = whiteValidateInput(int.TryParse(Console.ReadLine(), out fromY));
                }
                if (!Exit)//if previous validation passed, move to the next coordinates
                {
                    Console.WriteLine("Enter White Token's to coordinate X");
                    Exit = whiteValidateInput(int.TryParse(Console.ReadLine(), out toX));
                }
                if (!Exit)//if previous validation passed, move to the next coordinates
                {
                    Console.WriteLine("Enter White Token's to coordinate Y");
                    Exit = whiteValidateInput(int.TryParse(Console.ReadLine(), out toY));
                }
                isWhiteTurn = false;
            }
           else
            {
                //get input and validate it
                Console.WriteLine("Enter Black Token's from coordinate X");
                Exit = BlackValidateInput(int.TryParse(Console.ReadLine(), out fromX));

                if (!Exit)//if previous validation passed, move to the next coordinates
                {
                    Console.WriteLine("Enter Black Token's from coordinate Y");
                    Exit = BlackValidateInput(int.TryParse(Console.ReadLine(), out fromY));
                }
                if (!Exit)//if previous validation passed, move to the next coordinates
                {
                    Console.WriteLine("Enter Black Token's to coordinate X");
                    Exit = BlackValidateInput(int.TryParse(Console.ReadLine(), out toX));
                }
                if (!Exit)//if previous validation passed, move to the next coordinates
                {
                    Console.WriteLine("Enter Black Token's to coordinate Y");
                    Exit = BlackValidateInput(int.TryParse(Console.ReadLine(), out toY));
                }
                isWhiteTurn = true;
            }
            //if (isWhiteTurn && toY == 7)
            //{
            //    Console.WriteLine("White wins");
            //}
            //if (!isWhiteTurn && toY == 0)
            //{
            //    Console.WriteLine("Black Wins");
            //}
        }
        private bool BlackValidateInput(bool parsed)
        {
            bool error = false;

            if (!parsed)
            {
                error = true;
            }
            else if (fromX < 0 || fromY < 0 || toX < 0 || toY < 0)
            {
                error = true;
            }
            else if (fromX > BreakthroughBoard.boardSize - 1 || fromY > BreakthroughBoard.boardSize - 1
                || toX > BreakthroughBoard.boardSize - 1 || toY > BreakthroughBoard.boardSize - 1)
            {
                error = true;
            }
            else if (fromX == toX && fromY == toY)
            {
                error = true;
            }
            if (error)
            {
                Console.WriteLine("Invalid input. Exiting program now.");
            }

            return error;
        }
        private bool whiteValidateInput(bool parsed)
        {
            bool error = false;

            if (!parsed)
            {
                error = true;
            }
            else if (fromX < 0 || fromY < 0 || toX < 0 || toY < 0)
            {
                error = true;
            }
            else if (fromX > BreakthroughBoard.boardSize - 1 || fromY > BreakthroughBoard.boardSize - 1
                || toX > BreakthroughBoard.boardSize - 1 || toY > BreakthroughBoard.boardSize - 1)
            {
                error = true;
            }
            else if (fromX == toX && fromY == toY)
            {
                error = true;
            }
            if (error)
            {
                Console.WriteLine("Invalid input. Exiting program now.");
            }

            return error;
        }
        private bool allowMove()
        {
            bool isAllowed = false;
            if (isWhite)
            {
                //diagonal left
                if (fromX != 0 && fromY != 7)
                {
                    if (toX == fromX - 1 && toY == fromY + 1)
                    {
                        isAllowed = true;
                    }
                }
                //diagonal right
                if (fromX != 7 && fromY != 7)
                {
                    if (toX == fromX + 1 && toY == fromY + 1)
                    {
                        isAllowed = true;
                    }
                }
                //middle
                if (fromY != 7)
                {
                    if (toX == fromX && toY == fromY + 1)
                    {
                        isAllowed = true;
                    }
                }
                isWhite = false;
            }
            else
            {
                //diagonal left
                if (fromX != 0 && fromY != 0)
                {
                    if (toX == fromX - 1 && toY == fromY - 1)
                    {
                        isAllowed = true;
                    }
                }
                //diagonal right
                if(fromX!=7 && fromY != 0)
                {
                    if (toX == fromX + 1 && toY == fromY - 1)
                    {
                        isAllowed = true;
                    }
                }
                if(fromY!=0)
                {
                    if (toX == fromX && toY == fromY - 1)
                    {
                        isAllowed = true;
                    }
                }
                isWhite = true;
            }
            return isAllowed;
        }

        private void RearrangeTokens()
        {
               tokens[toX, toY] = tokens[fromX, fromY];
               tokens[fromX, fromY] = DASH;//once moved, place a dash in its place
         
        }
    }
}

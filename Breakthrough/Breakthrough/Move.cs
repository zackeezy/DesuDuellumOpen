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
        public bool isWhite;

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

            if (!Exit)
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
                Exit = validateInput(int.TryParse(Console.ReadLine(), out fromX));

                if (!Exit)//if previous validation passed, move to the next coordinates
                {
                    Console.WriteLine("Enter White Token's from coordinate Y");
                    Exit = validateInput(int.TryParse(Console.ReadLine(), out fromY));
                }
                if (!Exit)//if previous validation passed, move to the next coordinates
                {
                    Console.WriteLine("Enter White Token's to coordinate X");
                    Exit = validateInput(int.TryParse(Console.ReadLine(), out toX));
                }
                if (!Exit)//if previous validation passed, move to the next coordinates
                {
                    Console.WriteLine("Enter White Token's to coordinate Y");
                    Exit = validateInput(int.TryParse(Console.ReadLine(), out toY));
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
            ////toX == 0 && toY == 0 ||
            //else if ( toX != fromX - 1 && toY != fromY - 1)
            //{
            //    error = true;
            //}
            ////toX == 7 && toY == 0 || 
            //else if (toX != fromX + 1 && toY != fromY - 1)
            //{
            //    error = true;
            //}
            ////toY == 0 ||
            //else if ( toX != fromX && toY != fromY - 1)
            //{
            //    error = true;
            //}
            if (error)
            {
                Console.WriteLine("Invalid input. Exiting program now.");
            }

            return error;
        }
        private bool validateInput(bool parsed)
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
            //toX == 0 && toY == 7 ||
            //else if ( toX != fromX - 1 && toY != fromY + 1)
            //{
            //    error = true;
            //}
            ////toX == 7 && toY == 7 ||
            //else if ( toX != fromX + 1 && toY != fromY + 1)
            //{
            //    error = true;
            //}
            ////toY == 7 ||
            //else if ( toX != fromX && toY != fromY + 1)
            //{
            //    error = true;
            //}
            if (error)
            {
                Console.WriteLine("Invalid input. Exiting program now.");
            }

            return error;
        }

        private void RearrangeTokens()
        {
            tokens[toX, toY] = tokens[fromX, fromY];
            tokens[fromX, fromY] = DASH;//once moved, place a dash in its place
        }
    }
}

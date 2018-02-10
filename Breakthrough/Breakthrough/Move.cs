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
            //get input and validate it
            Console.WriteLine("Enter Token's from coordinate X");
            Exit = validateInput(int.TryParse(Console.ReadLine(),out fromX));

            if (!Exit)//if previous validation passed, move to the next coordinates
            {
                Console.WriteLine("Enter Token's from coordinate Y");
                Exit = validateInput(int.TryParse(Console.ReadLine(), out fromY));
            }
            if (!Exit)//if previous validation passed, move to the next coordinates
            {
                Console.WriteLine("Enter Token's to coordinate X");
                Exit = validateInput(int.TryParse(Console.ReadLine(), out toX));
            }
            if (!Exit)//if previous validation passed, move to the next coordinates
            {
                Console.WriteLine("Enter Token's to coordinate Y");
                Exit = validateInput(int.TryParse(Console.ReadLine(), out toY));
            }
        }

        private bool validateInput(bool parsed)
        {
            bool error = false;

            if (!parsed)
            {
                error = true;
            }
            else if (fromX<0||fromY<0||toX<0||toY<0)
            {
                error = true;
            }
            else if(fromX>BreakthroughBoard.boardSize-1|| fromY > BreakthroughBoard.boardSize - 1 
                || toX > BreakthroughBoard.boardSize - 1|| toY > BreakthroughBoard.boardSize - 1)
            {
                error = true;
            }

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

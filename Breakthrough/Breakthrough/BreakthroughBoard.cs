using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakthrough
{
   public class BreakthroughBoard
    {
        private string[,] gameBoard;
        public const int boardSize = 8;

        //private Token token;
        private Move move;

        //gameboard symbols
        public string GameBoardHorizontal { get; set; }
        public string GameBoardVertical { get; set; }

        public BreakthroughBoard()
        {
            move = new Move();
            gameBoard = new string[boardSize, boardSize];
            GameBoardHorizontal = "+---";
            GameBoardVertical = "| ";
        }

        public void displayBoard()
        {
            while (!move.Exit)
            {
                Console.Clear();
                Console.WriteLine("    A   B   C   D   E   F   G   H");

                for (int i = 0; i < boardSize;i++)
                {
                    Console.Write("  ");//left spacing two spaces
                    for(int j = 0; j < boardSize; j++)
                    {
                        Console.Write(GameBoardHorizontal);//horizontal part of board
                    }

                    Console.Write("+\n");
                    
                    for(int j = 0; j < boardSize; j++)
                    {
                        if (j == 0)
                        {
                            Console.Write(i + " ");
                        }
                        Console.Write(GameBoardVertical + Token.tokens[i, j] + " ");//display centered tokens
                     }
                Console.Write("|\n");
                 }
                Console.Write("  "); 
                for(int j = 0; j < boardSize; j++)
                 {
                    Console.Write(GameBoardHorizontal);
                 }
            Console.Write("+\n");
            Console.WriteLine("    A   B   C   D   E   F   G   H\n");
                move.GetMove();
            }
        }
    }
}

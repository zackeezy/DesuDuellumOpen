using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakthrough
{
    public class GameCoreToken
    {
        public const char WHITE_SYMBOL = 'W';
        public const char BLACK_SYMBOL = 'B';
        public const char SPACE = ' ';
        public const char DASH = '-';
        public static char[,] tokens;

        public GameCoreToken()
        {
            //initialize
            tokens = new char[BreakthroughBoard.boardSize, BreakthroughBoard.boardSize];
            initialToken();
        }
        private void initialToken()
        {
            for(int i = 0; i < BreakthroughBoard.boardSize; i++)
            {
                for(int j = 0; j < BreakthroughBoard.boardSize; j++)
                {
                    if (j == 0 || j == 1)
                    {
                        tokens[i, j] = WHITE_SYMBOL;
                    }
                    else if (j == 6 || j == 7)
                    {
                        tokens[i, j] = BLACK_SYMBOL;
                    }
                    else
                    {
                        tokens[i, j] = SPACE;
                    }
                }
            }
        }

        //public static implicit operator Token(char v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

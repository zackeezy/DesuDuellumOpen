using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakthrough
{
    public class Token
    {
        public const char WHITE_SYMBOL = 'W';
        public const char BLACK_SYMBOL = 'B';
        public const char SPACE = ' ';
        public const char DASH = '-';
        public static char[,] tokens;

        public Token()
        {
            //initialize
            tokens = new char[BreakthroughBoard.boardSize, BreakthroughBoard.boardSize];
            initialToken();
        }
        private void initialToken()
        {
            for(int i = 0; i < BreakthroughBoard.boardSize; i++)
            {
                for(int j =0; j < BreakthroughBoard.boardSize; j++)
                {
                    //place @ first two and last two rows;
                    if(i==0||i==1)
                    {
                        tokens[i, j] = WHITE_SYMBOL;
                    }
                    else if(
                    i == BreakthroughBoard.boardSize - 2 || i == BreakthroughBoard.boardSize - 1)
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

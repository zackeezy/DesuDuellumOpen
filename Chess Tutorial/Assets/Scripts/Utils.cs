using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakthrough_AI
{
    public class BitBoard
    {
        public BitBoard()
        {
            whitePieces = 0;
            blackPieces = 0;
        }

        public ulong CombinedBoard()
        {
            return whitePieces | blackPieces;
        }

        public ulong whitePieces;
        public ulong blackPieces;
    }

    public enum PlayerColor
    {
        White = 0,
        Black = 1,
    }

    public enum Direction
    {
        Forward = 0,
        Left = 1,
        Right = 2,
    }
}

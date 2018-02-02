using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakthrough_AI
{
    struct BitBoard
    {
        ulong whitePieces;
        ulong blackPieces;
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

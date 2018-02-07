using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//This File contains small functions and classes that don't really have a better home.
//If what you are looking for doesn't fit into the other classes, it probably lives here.
namespace Breakthrough_AI
{
    public class Utils
    {
        public static PlayerColor FlipColor(PlayerColor color)
        {
            if (color == PlayerColor.Black)
            {
                return PlayerColor.White;
            }

            return PlayerColor.Black;
        }
    }
   
    public class AlphaBetaNode
    {
        public BitBoard Child;
        public BitBoard Parent;
        public int Value;

        public AlphaBetaNode()
        {
            Child = new BitBoard();
            Parent = new BitBoard();
        }
    }

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
}

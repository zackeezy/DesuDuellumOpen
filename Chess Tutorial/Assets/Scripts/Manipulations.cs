using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakthrough_AI
{
    /// <summary>
    /// Contains methods that alter bitboards.
    /// </summary>
    public class Manipulations
    {
        public ulong MovePiece(ulong piece, Direction direction, PlayerColor color)
        {
            if (color == PlayerColor.White)
            {
                switch (direction)
                {
                    case Direction.Forward:
                        return MoveNorth(piece);
                    case Direction.Left:
                        return MoveNorth(MoveWest(piece));
                    case Direction.Right:
                        return MoveNorth(MoveEast(piece));
                }
            }
            else //color == PlayerColor.Black
            {
                switch (direction)
                {
                    case Direction.Forward:
                        return MoveSouth(piece);
                    case Direction.Left:
                        return MoveSouth(MoveEast(piece));
                    case Direction.Right:
                        return MoveSouth(MoveWest(piece));
                }
            }

            throw new Exception("Improper Direction or Color.");
        }

        private ulong MoveNorth(ulong piece)
        {
            if ((piece & Grid.Rows.Row8) != 0)
            {
                throw new Exception("Invalid move, cannot move north from the top row.");
            }
            return piece >> 8;
        }

        private ulong MoveSouth(ulong piece)
        {
            if ((piece & Grid.Rows.Row1) != 0)
            {
                throw new Exception("Invalid move, cannot move south from the bottom row.");
            }
            return piece << 8;
        }

        private ulong MoveEast(ulong piece)
        {
            if ((piece & Grid.Columns.ColH) != 0)
            {
                throw new Exception("Invalid move, cannot move east from the bottom row.");
            }
            return piece >> 1;
        }

        private ulong MoveWest(ulong piece)
        {
            return piece << 1;
        }

        public int[] index64 = new int[64]
        {
            0,  1, 48,  2, 57, 49, 28,  3,
           61, 58, 50, 42, 38, 29, 17,  4,
           62, 55, 59, 36, 53, 51, 43, 22,
           45, 39, 33, 30, 24, 18, 12,  5,
           63, 47, 56, 27, 60, 41, 37, 16,
           54, 35, 52, 21, 44, 32, 23, 11,
           46, 26, 40, 15, 34, 20, 31, 10,
           25, 14, 19,  9, 13,  8,  7,  6
        };

        public int BitScanForward(ulong board)
        {
            //Visit ChessProgramming.com for explanations of BitScan Magic.

            const long debruijn64 = 0x03f79d71b4cb0a89;

            if (board == 0)
            {
                return -1;
            }

            return index64[(int)(((ulong)((long)board & -(long)(board)) * debruijn64) >> 58)];
        }

        public int BitScanForwardWithReset(ref ulong board)
        {
            int index = BitScanForward(board);
            board &= board - 1;
            return index;
        }
    }
}

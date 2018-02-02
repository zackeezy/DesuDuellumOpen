using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakthrough_AI
{
    /// <summary>
    /// Contains methods that alter bitboards.
    /// </summary>
    static class Manipulations
    {
        public static ulong MovePiece(ulong piece, Direction direction, PlayerColor color)
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

        private static ulong MoveNorth(ulong piece)
        {
            if ((piece & Grid.Rows.Row8) != 0)
            {
                throw new Exception("Invalid move, cannot move north from the top row.");
            }
            return piece >> 8;
        }

        private static ulong MoveSouth(ulong piece)
        {
            if ((piece & Grid.Rows.Row1) != 0)
            {
                throw new Exception("Invalid move, cannot move south from the bottom row.");
            }
            return piece << 8;
        }

        private static ulong MoveEast(ulong piece)
        {
            if ((piece & Grid.Columns.ColH) != 0)
            {
                throw new Exception("Invalid move, cannot move east from the bottom row.");
            }
            return piece >> 1;
        }

        private static ulong MoveWest(ulong piece)
        {
            return piece << 1;
        }
    }
}

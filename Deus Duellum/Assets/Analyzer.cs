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

    /// <summary>
    /// Contains outward facing methods that are used in the rest of the project for 
    /// obtaining AI moves.
    /// </summary>
    public class Analyzer
    {
        public PlayerColor AiColor
        {
            get
            {
                return _aiColor;
            }
            private set
            {
                _aiColor = value;
            }
        }
        private PlayerColor _aiColor;
        

        public Analyzer(PlayerColor aiColor)
        {
            _aiColor = aiColor;
        }

        //Get list of tokens, return origin coordinates, new coordinates.
        public string GetMove()
        {
            throw new NotImplementedException("GetMove is not implemented.");
        }

        /// <summary>
        /// Performs the basic tree search to find the best possible move.
        /// Built from pseudocode taken from https://en.wikipedia.org/wiki/Alpha%E2%80%93beta_pruning
        /// </summary>
        /// <returns></returns>
        private int AlphaBetaLoop(BitBoard node, int remainingDepth, int alpha, int beta, bool maximizingPlayer)
        {            
            if (remainingDepth == 0 || IsGameOver(node))
            {
                return Evaluate(node);
            }

            //TODO: Generate All Child Nodes.
            List<BitBoard> children = new List<BitBoard>();

            if (maximizingPlayer)
            {
                int value = Int32.MinValue;

                foreach (BitBoard child in children)
                {
                    value = Math.Max(value, AlphaBetaLoop(child, remainingDepth - 1, alpha, beta, false));
                    alpha = Math.Max(alpha, value);

                    if (beta < alpha) break;
                }

                return value;
            }
            else
            {
                int value = Int32.MaxValue;

                foreach (BitBoard child in children)
                {
                    value = Math.Min(value, AlphaBetaLoop(child, remainingDepth - 1, alpha, beta, true));
                    beta = Math.Min(beta, value);

                    if (beta < alpha) break;
                }

                return value;
            }

            throw new NotImplementedException();
        }

        private bool IsGameOver(BitBoard bitBoard)
        {
            throw new NotImplementedException();
        }

        private int Evaluate(BitBoard origin)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Contains methods that alter bitboards.
    /// </summary>
    static class Manipulations 
    {
        public static ulong MovePiece(ulong piece, Direction direction, PlayerColor color)
        {
            if(color == PlayerColor.White)
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
            if ((piece & Grid.Columns.ColH)  != 0)
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

    /// <summary>
    /// Contains constants and methods that return predetermined spaces.
    /// </summary>
    static class Grid
    {
        public class Squares
        {
            public const ulong A1 = 0x8000000000000000;
            public const ulong B1 = 0x4000000000000000;
            public const ulong C1 = 0x2000000000000000;
            public const ulong D1 = 0x1000000000000000;
            public const ulong E1 = 0x0800000000000000;
            public const ulong F1 = 0x0400000000000000;
            public const ulong G1 = 0x0200000000000000;
            public const ulong H1 = 0x0100000000000000;

            public const ulong A2 = 0x0080000000000000;
            public const ulong B2 = 0x0040000000000000;
            public const ulong C2 = 0x0020000000000000;
            public const ulong D2 = 0x0010000000000000;
            public const ulong E2 = 0x0008000000000000;
            public const ulong F2 = 0x0004000000000000;
            public const ulong G2 = 0x0002000000000000;
            public const ulong H2 = 0x0001000000000000;

            public const ulong A3 = 0x0000800000000000;
            public const ulong B3 = 0x0000400000000000;
            public const ulong C3 = 0x0000200000000000;
            public const ulong D3 = 0x0000100000000000;
            public const ulong E3 = 0x0000080000000000;
            public const ulong F3 = 0x0000040000000000;
            public const ulong G3 = 0x0000020000000000;
            public const ulong H3 = 0x0000010000000000;

            public const ulong A4 = 0x0000008000000000;
            public const ulong B4 = 0x0000004000000000;
            public const ulong C4 = 0x0000002000000000;
            public const ulong D4 = 0x0000001000000000;
            public const ulong E4 = 0x0000000800000000;
            public const ulong F4 = 0x0000000400000000;
            public const ulong G4 = 0x0000000200000000;
            public const ulong H4 = 0x0000000100000000;

            public const ulong A5 = 0x0000000080000000;
            public const ulong B5 = 0x0000000040000000;
            public const ulong C5 = 0x0000000020000000;
            public const ulong D5 = 0x0000000010000000;
            public const ulong E5 = 0x0000000008000000;
            public const ulong F5 = 0x0000000004000000;
            public const ulong G5 = 0x0000000002000000;
            public const ulong H5 = 0x0000000001000000;

            public const ulong A6 = 0x0000000000800000;
            public const ulong B6 = 0x0000000000400000;
            public const ulong C6 = 0x0000000000200000;
            public const ulong D6 = 0x0000000000100000;
            public const ulong E6 = 0x0000000000080000;
            public const ulong F6 = 0x0000000000040000;
            public const ulong G6 = 0x0000000000020000;
            public const ulong H6 = 0x0000000000010000;

            public const ulong A7 = 0x0000000000008000;
            public const ulong B7 = 0x0000000000004000;
            public const ulong C7 = 0x0000000000002000;
            public const ulong D7 = 0x0000000000001000;
            public const ulong E7 = 0x0000000000000800;
            public const ulong F7 = 0x0000000000000400;
            public const ulong G7 = 0x0000000000000200;
            public const ulong H7 = 0x0000000000000100;

            public const ulong A8 = 0x0000000000000080;
            public const ulong B8 = 0x0000000000000040;
            public const ulong C8 = 0x0000000000000020;
            public const ulong D8 = 0x0000000000000010;
            public const ulong E8 = 0x0000000000000008;
            public const ulong F8 = 0x0000000000000004;
            public const ulong G8 = 0x0000000000000002;
            public const ulong H8 = 0x0000000000000001;
        }
        public class Rows
        {
            public const ulong Row1 = Squares.A1 | Squares.B1 | Squares.C1 | Squares.D1
                                    | Squares.E1 | Squares.F1 | Squares.G1 | Squares.H1;

            public const ulong Row2 = Squares.A2 | Squares.B2 | Squares.C2 | Squares.D2
                                    | Squares.E2 | Squares.F2 | Squares.G2 | Squares.H2;

            public const ulong Row3 = Squares.A3 | Squares.B3 | Squares.C3 | Squares.D3
                                    | Squares.E3 | Squares.F3 | Squares.G3 | Squares.H3;

            public const ulong Row4 = Squares.A4 | Squares.B4 | Squares.C4 | Squares.D4
                                    | Squares.E4 | Squares.F4 | Squares.G4 | Squares.H4;

            public const ulong Row5 = Squares.A5 | Squares.B5 | Squares.C5 | Squares.D5
                                    | Squares.E5 | Squares.F5 | Squares.G5 | Squares.H5;

            public const ulong Row6 = Squares.A6 | Squares.B6 | Squares.C6 | Squares.D6
                                    | Squares.E6 | Squares.F6 | Squares.G6 | Squares.H6;

            public const ulong Row7 = Squares.A7 | Squares.B7 | Squares.C7 | Squares.D7
                                    | Squares.E7 | Squares.F7 | Squares.G7 | Squares.H7;

            public const ulong Row8 = Squares.A8 | Squares.B8 | Squares.C8 | Squares.D8
                                    | Squares.E8 | Squares.F8 | Squares.G8 | Squares.H8;
        }
        public class Columns
        {
            public const ulong ColA = Squares.A1 | Squares.A2 | Squares.A3 | Squares.A4
                                    | Squares.A5 | Squares.A6 | Squares.A7 | Squares.A8;

            public const ulong ColB = Squares.B1 | Squares.B2 | Squares.B3 | Squares.B4
                                    | Squares.B5 | Squares.B6 | Squares.B7 | Squares.B8;

            public const ulong ColC = Squares.C1 | Squares.C2 | Squares.C3 | Squares.C4
                                    | Squares.C5 | Squares.C6 | Squares.C7 | Squares.C8;

            public const ulong ColD = Squares.D1 | Squares.D2 | Squares.D3 | Squares.D4
                                    | Squares.D5 | Squares.D6 | Squares.D7 | Squares.D8;

            public const ulong ColE = Squares.E1 | Squares.E2 | Squares.E3 | Squares.E4
                                    | Squares.E5 | Squares.E6 | Squares.E7 | Squares.E8;

            public const ulong ColF = Squares.F1 | Squares.F2 | Squares.F3 | Squares.F4
                                    | Squares.F5 | Squares.F6 | Squares.F7 | Squares.F8;

            public const ulong ColG = Squares.G1 | Squares.G2 | Squares.G3 | Squares.G4
                                    | Squares.G5 | Squares.G6 | Squares.G7 | Squares.G8;

            public const ulong ColH = Squares.H1 | Squares.H2 | Squares.H3 | Squares.H4
                                    | Squares.H5 | Squares.H6 | Squares.H7 | Squares.H8;
        }
        public class Patterns
        {
            public const ulong WhiteStart = Rows.Row1 | Rows.Row2;
            public const ulong BlackStart = Rows.Row7 | Rows.Row8;
        }
    }

    public enum PlayerColor
    {
        White = 0,
        Black = 1,
    }

    enum Direction
    {
        Forward = 0,
        Left = 1,
        Right = 2,
    }
}

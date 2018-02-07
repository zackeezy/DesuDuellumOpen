using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Breakthrough_AI
{
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

        private const int MAX_DEPTH = 5;

        private Random _random; 

        public Analyzer(PlayerColor aiColor)
        {
            _aiColor = aiColor;
            _random = new Random();
        }

        /// <summary>
        /// The outerward facing function that will return the move the AI wishes to take.
        /// </summary>
        public List<int> GetMove(List<Token> tokens)
        {
            BitBoard currentBoard = new BitBoard();
            foreach(Token token in tokens)
            {
                if (token.isWhite)
                {
                    currentBoard.whitePieces = currentBoard.whitePieces | Converters.Squares[token.currentX, token.currentY];
                }
                else
                {
                    currentBoard.blackPieces = currentBoard.blackPieces | Converters.Squares[token.currentX, token.currentY];
                }
            }

            AlphaBetaNode bestOption = AlphaBetaLoop(currentBoard, MAX_DEPTH, Int32.MinValue, Int32.MaxValue, true);

            ulong movedPiece = 0;
            List<int> coordinates = new List<int>();

            if (_aiColor == PlayerColor.White)
            {
                movedPiece = bestOption.Child.whitePieces ^ currentBoard.whitePieces;
                int destination = BitsMagic.BitScanForwardWithReset(ref movedPiece);
                int start = BitsMagic.BitScanForwardWithReset(ref movedPiece);
                coordinates.Add(Converters.XCoordinate(start));
                coordinates.Add(Converters.YCoordinate(start));
                coordinates.Add(Converters.XCoordinate(destination));
                coordinates.Add(Converters.YCoordinate(destination));

            }
            else
            {
                movedPiece = bestOption.Child.blackPieces ^ currentBoard.blackPieces;
                int start = BitsMagic.BitScanForwardWithReset(ref movedPiece);
                int destination = BitsMagic.BitScanForwardWithReset(ref movedPiece);
                coordinates.Add(Converters.XCoordinate(start));
                coordinates.Add(Converters.YCoordinate(start));
                coordinates.Add(Converters.XCoordinate(destination));
                coordinates.Add(Converters.YCoordinate(destination));
            }


            return coordinates;
        }

        /// <summary>
        /// Will return a list of all of the possible moves for the given board.
        /// </summary>
        private List<BitBoard> GetChildren(BitBoard board, PlayerColor color)
        {
            List<BitBoard> children = new List<BitBoard>();
            ulong myBoard = color == PlayerColor.White ? board.whitePieces : board.blackPieces;

            int piece = BitsMagic.BitScanForwardWithReset(ref myBoard);

            while (piece >= 0)
            {
                if (color == PlayerColor.White)
                {
                    ulong forward = Masks.WhiteMasks.Forward[piece];
                    ulong east = Masks.WhiteMasks.EastAttack[piece];
                    ulong west = Masks.WhiteMasks.WestAttack[piece];

                    if (forward != 0 && (forward & board.CombinedBoard()) == 0 )
                    {
                        BitBoard child = new BitBoard();
                        child.whitePieces = board.whitePieces & ~Masks.CurrentSquare[piece];
                        child.whitePieces = child.whitePieces | forward;
                        child.blackPieces = board.blackPieces;
                        children.Add(child);
                    }

                    if (east != 0 && (east & board.whitePieces) == 0)
                    {
                        BitBoard child = new BitBoard();
                        child.whitePieces = board.whitePieces & ~Masks.CurrentSquare[piece];
                        child.whitePieces = child.whitePieces | east;
                        child.blackPieces = board.blackPieces & ~east;
                        children.Add(child);
                    }

                    if (west != 0 && (west & board.whitePieces) == 0)
                    {
                        BitBoard child = new BitBoard();
                        child.whitePieces = board.whitePieces & ~Masks.CurrentSquare[piece];
                        child.whitePieces = child.whitePieces | west;
                        child.blackPieces = board.blackPieces & ~west;
                        children.Add(child);
                    }
                }
                else
                {
                    ulong forward = Masks.BlackMasks.Forward[piece];
                    ulong east = Masks.BlackMasks.EastAttack[piece];
                    ulong west = Masks.BlackMasks.WestAttack[piece];

                    if (forward != 0 && (forward & board.CombinedBoard()) == 0)
                    {
                        BitBoard child = new BitBoard();
                        child.blackPieces = board.blackPieces & ~Masks.CurrentSquare[piece];
                        child.blackPieces = child.blackPieces | forward;
                        child.whitePieces = board.whitePieces;
                        children.Add(child);
                    }

                    if (east != 0 && (east & board.blackPieces) == 0)
                    {
                        BitBoard child = new BitBoard();
                        child.blackPieces = board.blackPieces & ~Masks.CurrentSquare[piece];
                        child.blackPieces = child.blackPieces | east;
                        child.whitePieces = board.whitePieces & ~east;
                        children.Add(child);
                    }

                    if (west != 0 && (west & board.blackPieces) == 0)
                    {
                        BitBoard child = new BitBoard();
                        child.blackPieces = board.blackPieces & ~Masks.CurrentSquare[piece];
                        child.blackPieces = child.blackPieces | west;
                        child.whitePieces = board.whitePieces & ~west;
                        children.Add(child);
                    }
                }
                piece = BitsMagic.BitScanForwardWithReset(ref myBoard);
            }

            return children;
        }

        /// <summary>
        /// Performs the basic tree search to find the best possible move.
        /// Built from pseudocode taken from https://en.wikipedia.org/wiki/Alpha%E2%80%93beta_pruning
        /// </summary>
        private AlphaBetaNode AlphaBetaLoop(BitBoard node, int remainingDepth, int alpha, int beta, bool maximizingPlayer)
        {
            AlphaBetaNode returnValue = new AlphaBetaNode();
            returnValue.Parent = node;

            if (remainingDepth == 0 || IsGameOver(node))
            {
                returnValue.Child = node;
                returnValue.Value = Evaluate(node);
                return returnValue;
            }

            PlayerColor side = maximizingPlayer ? _aiColor : Utils.FlipColor(_aiColor);

            List<BitBoard> children = GetChildren(node, side);

            if (maximizingPlayer)
            {
                int value = Int32.MinValue;

                AlphaBetaNode workingNode = new AlphaBetaNode();

                foreach (BitBoard child in children)
                {
                    workingNode = AlphaBetaLoop(child, remainingDepth - 1, alpha, beta, false);

                    if (value < workingNode.Value)
                    {
                        returnValue.Child = child;
                        returnValue.Value = workingNode.Value;
                    }
                    value = Math.Max(value, workingNode.Value);
                    alpha = Math.Max(alpha, value);

                    if (beta < alpha) break;
                }

                return returnValue;
            }
            else
            {
                int value = Int32.MaxValue;
                AlphaBetaNode workingNode = new AlphaBetaNode();

                foreach (BitBoard child in children)
                {
                    workingNode = AlphaBetaLoop(child, remainingDepth - 1, alpha, beta, true);

                    if (value > workingNode.Value)
                    {
                        returnValue.Child = child;
                        returnValue.Value = workingNode.Value;
                    }

                    value = Math.Min(value, workingNode.Value);
                    beta = Math.Min(beta, value);

                    if (beta < alpha) break;
                }

                return returnValue;
            }
        }

        /// <summary>
        /// Tells if somebody has won the game.
        /// </summary>
        private bool IsGameOver(BitBoard bitBoard)
        {
            ulong myWhitePieces = bitBoard.whitePieces;
            ulong myBlackPieces = bitBoard.blackPieces;

            int piece = BitsMagic.BitScanForwardWithReset(ref myWhitePieces);
            while (piece >= 0)
            {
                if ((Masks.CurrentSquare[piece] & Grid.Row8) != 0)
                {
                    return true;
                }
                piece = BitsMagic.BitScanForwardWithReset(ref myWhitePieces);
            }

            piece = BitsMagic.BitScanForwardWithReset(ref myBlackPieces);
            while (piece >= 0)
            {
                if ((Masks.CurrentSquare[piece] & Grid.Row1) != 0)
                {
                    return true;
                }
                piece = BitsMagic.BitScanForwardWithReset(ref myBlackPieces);
            }

            return false;
        }

        /// <summary>
        /// Returns a heuristic score for the board.  
        /// </summary>
        private int Evaluate(BitBoard origin)
        {
            return _random.Next(-1000,1000);
        }
    }
}

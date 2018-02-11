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

        class Weights
        {
            public const int WIN = Int32.MaxValue;
            public const int LOSS = Int32.MinValue;
            public const int VERTICAL_CONNECTION = 2;
            public const int HORIZONTAL_CONNECTION = 2;
            public const int PROTECTED = 2;
            public const int ATTACKED = 1;
            public const int DANGER_LOW = 2;
            public const int DANGER_MED = 4;
            public const int DANGER_HIGH = 6;
            public const int FLAT_DANGER = 2;
            public const int MOVEMENT = 1;
            public const int COLUMN_HOLE_PENALTY = 1;
        }

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
                        child.whitePieces = board.whitePieces & ~Masks.OrientationMasks.CurrentSquare[piece];
                        child.whitePieces = child.whitePieces | forward;
                        child.blackPieces = board.blackPieces;
                        children.Add(child);
                    }

                    if (east != 0 && (east & board.whitePieces) == 0)
                    {
                        BitBoard child = new BitBoard();
                        child.whitePieces = board.whitePieces & ~Masks.OrientationMasks.CurrentSquare[piece];
                        child.whitePieces = child.whitePieces | east;
                        child.blackPieces = board.blackPieces & ~east;
                        children.Add(child);
                    }

                    if (west != 0 && (west & board.whitePieces) == 0)
                    {
                        BitBoard child = new BitBoard();
                        child.whitePieces = board.whitePieces & ~Masks.OrientationMasks.CurrentSquare[piece];
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
                        child.blackPieces = board.blackPieces & ~Masks.OrientationMasks.CurrentSquare[piece];
                        child.blackPieces = child.blackPieces | forward;
                        child.whitePieces = board.whitePieces;
                        children.Add(child);
                    }

                    if (east != 0 && (east & board.blackPieces) == 0)
                    {
                        BitBoard child = new BitBoard();
                        child.blackPieces = board.blackPieces & ~Masks.OrientationMasks.CurrentSquare[piece];
                        child.blackPieces = child.blackPieces | east;
                        child.whitePieces = board.whitePieces & ~east;
                        children.Add(child);
                    }

                    if (west != 0 && (west & board.blackPieces) == 0)
                    {
                        BitBoard child = new BitBoard();
                        child.blackPieces = board.blackPieces & ~Masks.OrientationMasks.CurrentSquare[piece];
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

            if (myWhitePieces == 0 || myBlackPieces == 0)
            {
                return true;
            }

            int piece = BitsMagic.BitScanForwardWithReset(ref myWhitePieces);
            while (piece >= 0)
            {
                if ((Masks.OrientationMasks.CurrentSquare[piece] & Grid.Row8) != 0)
                {
                    return true;
                }
                piece = BitsMagic.BitScanForwardWithReset(ref myWhitePieces);
            }

            piece = BitsMagic.BitScanForwardWithReset(ref myBlackPieces);
            while (piece >= 0)
            {
                if ((Masks.OrientationMasks.CurrentSquare[piece] & Grid.Row1) != 0)
                {
                    return true;
                }
                piece = BitsMagic.BitScanForwardWithReset(ref myBlackPieces);
            }

            return false;
        }

        /// <summary>
        /// Returns a heuristic score for the board. 
        /// Initially based on the article by Roman Atachiants, found at  https://www.codeproject.com/Articles/37024/Simple-AI-for-the-Game-of-Breakthrough
        /// </summary>
        private int Evaluate(BitBoard origin)
        {
            int score = 0;

            //Add up scores for the following features
            //Winning Postition - The score for if a side has won.
            //Piece Almost Win - Prediction of if a piece will win in a few moves
            //Piece Value - Puts a value on a piece
            //Piece Danger Value - Positional Value of a piece (row * danger_value)
            //Piece High Danger Value - Feature for the highest danger value of a piece
            //Piece Attack Value - Value that weights an attack on a pawn, cumulative.
            //Piece Protection Value - Value that weights the protection on a piece, culumative.
            //Connection Horizontal - Chracterizes a two pawn horizontal connection.
            //Connection Vertical - Characterizes a two pawns vertical connection.
            //Piece Mobility value - Valorize the number of valid moves for a piece.
            //Column Hole Value - Penalty on columns without a piece on them.
            //Home Ground Value - Valorizes Pieces on the Home Row.


            /*
             * General Outline for Evaluation:
             *  We will begin by generating two scores, one for black, and one for white.
             *  For each side,
             *      First, check if already won, and return if found.
             *      Next, iterate through all pieces, generating scores.
             *      Finally, calculate penalties for the total board.
             *  Next, combine total scores, with negative modifiers for opponent side, positive for player side.
             *  Return the final score.
             */

            if (IsGameOver(origin))
            {
                if ((Grid.Row1 & origin.blackPieces) != 0)
                {
                    if (_aiColor == PlayerColor.Black)
                    {
                        return Weights.WIN;
                    }
                    else
                    {
                        return Weights.LOSS;
                    }
                }
                else if ((Grid.Row8 & origin.whitePieces) != 0)
                {
                    if (_aiColor == PlayerColor.White)
                    {
                        return Weights.WIN;
                    }
                    else
                    {
                        return Weights.LOSS;
                    }
                }
            }

            int whiteScore = 0;
            ulong whitePieces = origin.whitePieces;
            int iterator = BitsMagic.BitScanForwardWithReset(ref whitePieces);
            while (iterator >= 0)
            {
                int pieceScore = 0;

                if ((origin.whitePieces & Masks.OrientationMasks.Above[iterator]) != 0)
                {
                    pieceScore += Weights.VERTICAL_CONNECTION;
                }

                if ((origin.whitePieces & Masks.OrientationMasks.RightOf[iterator]) != 0)
                {
                    pieceScore += Weights.HORIZONTAL_CONNECTION;
                }

                int pieceProtectedScore = 0;
                if ((origin.whitePieces & Masks.BlackMasks.EastAttack[iterator]) != 0)
                {
                    pieceProtectedScore += Weights.PROTECTED;
                }

                if ((origin.whitePieces & Masks.BlackMasks.WestAttack[iterator]) != 0)
                {
                    pieceProtectedScore += Weights.PROTECTED;
                }

                int pieceAttackedValue = 0;

                if ((origin.blackPieces & Masks.WhiteMasks.EastAttack[iterator]) != 0)
                {
                    pieceAttackedValue += Weights.ATTACKED;
                }

                if ((origin.blackPieces & Masks.WhiteMasks.WestAttack[iterator]) != 0)
                {
                    pieceAttackedValue += Weights.ATTACKED;
                }

                if (pieceAttackedValue > 0)
                {
                    pieceScore -= pieceAttackedValue;
                    if (pieceProtectedScore == 0)
                    {
                        pieceScore -= pieceAttackedValue;
                    }
                }
                else
                {
                    if (pieceProtectedScore != 0)
                    {
                        if ((Masks.OrientationMasks.CurrentSquare[iterator] & Grid.Row5) != 0)
                        {
                            pieceScore += Weights.DANGER_LOW;
                        }
                        else if ((Masks.OrientationMasks.CurrentSquare[iterator] & Grid.Row6) != 0)
                        {
                            pieceScore += Weights.DANGER_MED;
                        }
                        else if ((Masks.OrientationMasks.CurrentSquare[iterator] & Grid.Row7) != 0)
                        {
                            pieceScore += Weights.DANGER_HIGH;
                        }
                    }
                }

                pieceScore += Masks.OrientationMasks.CurrentRow[iterator] * Weights.FLAT_DANGER;

                if ((origin.CombinedBoard() & Masks.WhiteMasks.Forward[iterator]) == 0)
                {
                    pieceScore += Weights.MOVEMENT;
                }
                if ((origin.whitePieces & Masks.WhiteMasks.EastAttack[iterator]) == 0)
                {
                    pieceScore += Weights.MOVEMENT;
                }
                if ((origin.whitePieces & Masks.WhiteMasks.WestAttack[iterator]) == 0)
                {
                    pieceScore += Weights.MOVEMENT;
                }

                whiteScore += pieceScore;
                iterator = BitsMagic.BitScanForwardWithReset(ref whitePieces);
            }

            if ((origin.whitePieces & Grid.ColA) == 0)
            {
                whiteScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.whitePieces & Grid.ColB) == 0)
            {
                whiteScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.whitePieces & Grid.ColC) == 0)
            {
                whiteScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.whitePieces & Grid.ColD) == 0)
            {
                whiteScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.whitePieces & Grid.ColE) == 0)
            {
                whiteScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.whitePieces & Grid.ColF) == 0)
            {
                whiteScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.whitePieces & Grid.ColG) == 0)
            {
                whiteScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.whitePieces & Grid.ColH) == 0)
            {
                whiteScore -= Weights.COLUMN_HOLE_PENALTY;
            }

            int blackScore = 0;
            ulong blackPieces = origin.blackPieces;
            iterator = BitsMagic.BitScanForwardWithReset(ref blackPieces);
            while (iterator >= 0)
            {
                int pieceScore = 0;

                if ((origin.blackPieces & Masks.OrientationMasks.Above[iterator]) != 0)
                {
                    pieceScore += Weights.VERTICAL_CONNECTION;
                }

                if ((origin.blackPieces & Masks.OrientationMasks.RightOf[iterator]) != 0)
                {
                    pieceScore += Weights.HORIZONTAL_CONNECTION;
                }

                int pieceProtectedScore = 0;
                if ((origin.blackPieces & Masks.WhiteMasks.EastAttack[iterator]) != 0)
                {
                    pieceProtectedScore += Weights.PROTECTED;
                }

                if ((origin.blackPieces & Masks.WhiteMasks.WestAttack[iterator]) != 0)
                {
                    pieceProtectedScore += Weights.PROTECTED;
                }

                int pieceAttackedValue = 0;

                if ((origin.whitePieces & Masks.BlackMasks.EastAttack[iterator]) != 0)
                {
                    pieceAttackedValue += Weights.ATTACKED;
                }

                if ((origin.whitePieces & Masks.BlackMasks.WestAttack[iterator]) != 0)
                {
                    pieceAttackedValue += Weights.ATTACKED;
                }

                if (pieceAttackedValue > 0)
                {
                    pieceScore -= pieceAttackedValue;
                    if (pieceProtectedScore == 0)
                    {
                        pieceScore -= pieceAttackedValue;
                    }
                }
                else
                {
                    if (pieceProtectedScore != 0)
                    {
                        if ((Masks.OrientationMasks.CurrentSquare[iterator] & Grid.Row4) != 0)
                        {
                            pieceScore += Weights.DANGER_LOW;
                        }
                        else if ((Masks.OrientationMasks.CurrentSquare[iterator] & Grid.Row3) != 0)
                        {
                            pieceScore += Weights.DANGER_MED;
                        }
                        else if ((Masks.OrientationMasks.CurrentSquare[iterator] & Grid.Row2) != 0)
                        {
                            pieceScore += Weights.DANGER_HIGH;
                        }
                    }
                }

                pieceScore += (9 - Masks.OrientationMasks.CurrentRow[iterator]) * Weights.FLAT_DANGER;

                if ((origin.CombinedBoard() & Masks.BlackMasks.Forward[iterator]) == 0)
                {
                    pieceScore += Weights.MOVEMENT;
                }
                if ((origin.blackPieces & Masks.BlackMasks.EastAttack[iterator]) == 0)
                {
                    pieceScore += Weights.MOVEMENT;
                }
                if ((origin.blackPieces & Masks.BlackMasks.WestAttack[iterator]) == 0)
                {
                    pieceScore += Weights.MOVEMENT;
                }

                blackScore += pieceScore;
                iterator = BitsMagic.BitScanForwardWithReset(ref blackPieces);
            }

            if ((origin.blackPieces & Grid.ColA) == 0)
            {
                blackScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.blackPieces & Grid.ColB) == 0)
            {
                blackScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.blackPieces & Grid.ColC) == 0)
            {
                blackScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.blackPieces & Grid.ColD) == 0)
            {
                blackScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.blackPieces & Grid.ColE) == 0)
            {
                blackScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.blackPieces & Grid.ColF) == 0)
            {
                blackScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.blackPieces & Grid.ColG) == 0)
            {
                blackScore -= Weights.COLUMN_HOLE_PENALTY;
            }
            if ((origin.blackPieces & Grid.ColH) == 0)
            {
                blackScore -= Weights.COLUMN_HOLE_PENALTY;
            }

            if (_aiColor == PlayerColor.White)
            {
                score = whiteScore - blackScore;
            }
            else
            {
                score = blackScore = whiteScore;
            }

            return score;
        }
    }
}

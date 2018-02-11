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
            public const int VERTICAL_CONNECTION = 100;
            public const int HORIZONTAL_CONNECTION = 100;
            public const int PROTECTED = 250;
            public const int ATTACKED = 100;
            public const int DANGER_LOW = 500;
            public const int DANGER_HIGH = 10000;
            public const int FLAT_DANGER = 100;
            public const int IMMEDIATE_MOVEMENT = 150;
            public const int COLUMN_HOLE_PENALTY = 200;
            public const int BASE_MOBILITY = 10000;
            public const int MOBILITY_PENALTY = 625;
            public const int HOME_ROW_MOVED = 5000;
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
        private int Evaluate(BitBoard board)
        {
            int totalScore = 0;

            //Skips the evaluation if somebody has won the game.
            if (IsGameOver(board))
            {
                if ((Grid.Row1 & board.blackPieces) != 0)
                {
                    return _aiColor == PlayerColor.Black ?  Weights.WIN : Weights.LOSS;
                }
                else if ((Grid.Row8 & board.whitePieces) != 0)
                {
                    return _aiColor == PlayerColor.White ? Weights.WIN : Weights.LOSS;
                }
            }

            //Accumulate Score for each White Piece.
            int whiteScore = 0;
            ulong whitePieces = board.whitePieces;
            int iterator = BitsMagic.BitScanForwardWithReset(ref whitePieces);
            while (iterator >= 0)
            {
                whiteScore += GenerateBlockingPatternScore(board, iterator, PlayerColor.White);

                int pieceProtectionScore = GenerateProtectionScore(board, iterator, PlayerColor.White);
                whiteScore += pieceProtectionScore;
                int pieceAttackedPenalty = GenerateAttackedPenalty(board, iterator, PlayerColor.White);

                if(pieceAttackedPenalty > 0)
                {
                    whiteScore -= pieceAttackedPenalty;
                    if (pieceProtectionScore == 0)
                    {
                        whiteScore -= pieceAttackedPenalty;
                    }
                }
                else
                {
                    if(pieceProtectionScore != 0)
                    {
                        if(Masks.OrientationMasks.CurrentRow[iterator] == 6)
                        {
                            whiteScore += Weights.DANGER_LOW;
                        }
                        else if(Masks.OrientationMasks.CurrentRow[iterator] == 7)
                        {
                            whiteScore += Weights.DANGER_HIGH;
                        }
                    }
                }

                whiteScore += GenerateDangerScore(board, iterator, PlayerColor.White);

                whiteScore += GenerateMobilityScore(board, iterator, PlayerColor.White);

                iterator = BitsMagic.BitScanForwardWithReset(ref whitePieces);
            }

            whiteScore -= GenerateColumnHolePenalty(board, PlayerColor.White);
            whiteScore -= GenerateHomeRowProtectionPenalty(board, PlayerColor.White);


            //Acculmulate score for all black pieces.
            int blackScore = 0;
            ulong blackPieces = board.whitePieces;
            iterator = BitsMagic.BitScanForwardWithReset(ref blackPieces);
            while (iterator >= 0)
            {
                blackScore += GenerateBlockingPatternScore(board, iterator, PlayerColor.Black);

                int pieceProtectionScore = GenerateProtectionScore(board, iterator, PlayerColor.Black);
                blackScore += pieceProtectionScore;
                int pieceAttackedPenalty = GenerateAttackedPenalty(board, iterator, PlayerColor.Black);

                if (pieceAttackedPenalty > 0)
                {
                    blackScore -= pieceAttackedPenalty;
                    if (pieceProtectionScore == 0)
                    {
                        blackScore -= pieceAttackedPenalty;
                    }
                }
                else
                {
                    if (pieceProtectionScore != 0)
                    {
                        if (Masks.OrientationMasks.CurrentRow[iterator] == 3)
                        {
                            blackScore += Weights.DANGER_LOW;
                        }
                        else if (Masks.OrientationMasks.CurrentRow[iterator] == 2)
                        {
                            blackScore += Weights.DANGER_HIGH;
                        }
                    }
                }

                blackScore += GenerateDangerScore(board, iterator, PlayerColor.Black);
                blackScore += GenerateMobilityScore(board, iterator, PlayerColor.Black);

                iterator = BitsMagic.BitScanForwardWithReset(ref blackPieces);
            }

            blackScore -= GenerateColumnHolePenalty(board, PlayerColor.Black);
            blackScore -= GenerateHomeRowProtectionPenalty(board, PlayerColor.Black);


            //Finalize Score
            if (_aiColor == PlayerColor.White)
            {
                totalScore = whiteScore - blackScore;
            }
            else
            {
                totalScore = blackScore = whiteScore;
            }

            return totalScore;
        }

        private int GenerateBlockingPatternScore(BitBoard board, int index, PlayerColor color)
        {
            /*
             * Generate a score for a piece's blocking patterns.  Does it form vertical
             * and horizontal chains of pieces.  Eventually should check for if the chains 
             * exist in the mobility triangles of high danger enemy pieces.
             */
            int score = 0;

            if (color == PlayerColor.White)
            {
                if((board.whitePieces & Masks.OrientationMasks.Above[index]) != 0)
                {
                    score += Weights.VERTICAL_CONNECTION;
                }

                if ((board.whitePieces & Masks.OrientationMasks.RightOf[index]) != 0)
                {
                    score += Weights.HORIZONTAL_CONNECTION;
                }
            }
            else if (color == PlayerColor.Black)
            {
                if ((board.blackPieces & Masks.OrientationMasks.Above[index]) != 0)
                {
                    score += Weights.VERTICAL_CONNECTION;
                }

                if ((board.blackPieces & Masks.OrientationMasks.RightOf[index]) != 0)
                {
                    score += Weights.HORIZONTAL_CONNECTION;
                }
            }

            return score;
        }

        private int GenerateMobilityScore(BitBoard board, int index, PlayerColor color)
        {
            /*
             * Generate a score for a pieces mobility.  This takes into account immediate moves,
             * as well as the entire mobility triangle of the piece.  The less enemy pieces
             * in the triangle, the better the score.
             */
            int score = Weights.BASE_MOBILITY;

            if (color == PlayerColor.White)
            {
                ulong enemyPiecesInTriangle = board.blackPieces & Masks.WhiteMasks.MobilityTriangleList[index];
                int iterator = BitsMagic.BitScanForwardWithReset(ref enemyPiecesInTriangle);
                int pieceCount = 0;
                do
                {
                    if (iterator >= 0) pieceCount++;
                    iterator = BitsMagic.BitScanForwardWithReset(ref enemyPiecesInTriangle);
                } while (iterator >= 0);

                score -= pieceCount * Weights.MOBILITY_PENALTY;

                if ((board.CombinedBoard() & Masks.WhiteMasks.Forward[index]) == 0)
                {
                    score += Weights.IMMEDIATE_MOVEMENT;
                }
                if ((board.whitePieces & Masks.WhiteMasks.EastAttack[index]) == 0)
                {
                    score += Weights.IMMEDIATE_MOVEMENT;
                }
                if ((board.whitePieces & Masks.WhiteMasks.WestAttack[index]) == 0)
                {
                    score += Weights.IMMEDIATE_MOVEMENT;
                }
            }
            else if (color == PlayerColor.Black)
            {
                ulong enemyPiecesInTriangle = board.whitePieces & Masks.BlackMasks.MobilityTriangleList[index];
                int iterator = BitsMagic.BitScanForwardWithReset(ref enemyPiecesInTriangle);
                int pieceCount = 0;
                do
                {
                    if (iterator >= 0) pieceCount++;
                    iterator = BitsMagic.BitScanForwardWithReset(ref enemyPiecesInTriangle);
                } while (iterator >= 0);

                score -= pieceCount * Weights.MOBILITY_PENALTY;

                if ((board.CombinedBoard() & Masks.BlackMasks.Forward[index]) == 0)
                {
                    score += Weights.IMMEDIATE_MOVEMENT;
                }
                if ((board.blackPieces & Masks.BlackMasks.EastAttack[index]) == 0)
                {
                    score += Weights.IMMEDIATE_MOVEMENT;
                }
                if ((board.blackPieces & Masks.BlackMasks.WestAttack[index]) == 0)
                {
                    score += Weights.IMMEDIATE_MOVEMENT;
                }
            }

            return score;
        }

        private int GenerateColumnHolePenalty(BitBoard board, PlayerColor color)
        {
            /*
             * Generates a penalty for not having sufficient coverage of all columns.  
             * Pieces must sufficiently threaten every column on the board. (I'm not sure 
             * that I actually agree with this score.  I feel like simply prioritizing home
             * row protection should be sufficient.)
             */
            int penalty = 0;

            if (color == PlayerColor.White)
            {
                if ((board.whitePieces & Grid.ColA) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.whitePieces & Grid.ColB) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.whitePieces & Grid.ColC) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.whitePieces & Grid.ColD) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.whitePieces & Grid.ColE) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.whitePieces & Grid.ColF) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.whitePieces & Grid.ColG) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.whitePieces & Grid.ColH) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
            }
            else if(color == PlayerColor.Black)
            {
                if ((board.blackPieces & Grid.ColA) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.blackPieces & Grid.ColB) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.blackPieces & Grid.ColC) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.blackPieces & Grid.ColD) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.blackPieces & Grid.ColE) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.blackPieces & Grid.ColF) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.blackPieces & Grid.ColG) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
                if ((board.blackPieces & Grid.ColH) == 0)
                {
                    penalty += Weights.COLUMN_HOLE_PENALTY;
                }
            }

            return penalty;
        }

        private int GenerateHomeRowProtectionPenalty(BitBoard board, PlayerColor color)
        {
            /*
             * If the back four have moved, create a large penalty.  
             */
            int penalty = 0;

            if (color == PlayerColor.White)
            {
                if ((board.whitePieces & Grid.A2) == 0)
                {
                    penalty += Weights.HOME_ROW_MOVED;
                }
                if ((board.whitePieces & Grid.A3) == 0)
                {
                    penalty += Weights.HOME_ROW_MOVED;
                }
                if ((board.whitePieces & Grid.A6) == 0)
                {
                    penalty += Weights.HOME_ROW_MOVED;
                }
                if ((board.whitePieces & Grid.A7) == 0)
                {
                    penalty += Weights.HOME_ROW_MOVED;
                }
            }
            else if (color == PlayerColor.Black)
            {
                if ((board.blackPieces & Grid.H2) == 0)
                {
                    penalty += Weights.HOME_ROW_MOVED;
                }
                if ((board.blackPieces & Grid.H3) == 0)
                {
                    penalty += Weights.HOME_ROW_MOVED;
                }
                if ((board.blackPieces & Grid.H6) == 0)
                {
                    penalty += Weights.HOME_ROW_MOVED;
                }
                if ((board.blackPieces & Grid.H7) == 0)
                {
                    penalty += Weights.HOME_ROW_MOVED;
                }
            }

            return penalty;
        }

        private int GenerateAlmostWinScore(BitBoard origin, int index)
        {
            /*
             * Generate score that is a function of piece danger and mobility.  
             * Higher danger plus less potential blockers makes a high score.
             */
            int score = 0;

            return score;
        }

        private int GenerateDangerScore(BitBoard board, int index, PlayerColor color)
        {
            /*
             * Generates a score based simply on proximity to enemy home row.
             */
            int score = 0;

            if (color == PlayerColor.White)
            {
                score = Masks.OrientationMasks.CurrentRow[index] * Weights.FLAT_DANGER;
            }
            else if (color == PlayerColor.Black)
            {
                score = (9 - Masks.OrientationMasks.CurrentRow[index])* Weights.FLAT_DANGER;
            }

            return score;
        }

        private int GenerateAttackedPenalty(BitBoard board, int index, PlayerColor color)
        {
            /*
             * Generates a penalty based on number of pieces attacking this piece, doubled
             * if the piece is unprotected.
             */
            int penalty = 0;

            if (color == PlayerColor.White)
            {
                if ((board.blackPieces & Masks.WhiteMasks.EastAttack[index]) != 0)
                {
                    penalty += Weights.ATTACKED;
                }

                if ((board.blackPieces & Masks.WhiteMasks.WestAttack[index]) != 0)
                {
                    penalty += Weights.ATTACKED;
                }
            }
            else if (color == PlayerColor.Black)
            {
                if ((board.whitePieces & Masks.BlackMasks.EastAttack[index]) != 0)
                {
                    penalty += Weights.ATTACKED;
                }

                if ((board.whitePieces & Masks.BlackMasks.WestAttack[index]) != 0)
                {
                    penalty += Weights.ATTACKED;
                }
            }

            return penalty;
        }

        private int GenerateProtectionScore(BitBoard board, int index, PlayerColor color)
        {
            /*
             * Generates a score for having pieces protectecting the piece in question.
             * We use the opposite color's attack patterns to quickly check if we are 
             * protected.
             */
            int score = 0;

            if (color == PlayerColor.White)
            {
                if ((board.whitePieces & Masks.BlackMasks.EastAttack[index]) != 0)
                {
                    score += Weights.PROTECTED;
                }

                if ((board.whitePieces & Masks.BlackMasks.WestAttack[index]) != 0)
                {
                    score += Weights.PROTECTED;
                }
            }
            else if (color == PlayerColor.Black)
            {
                if ((board.blackPieces & Masks.WhiteMasks.EastAttack[index]) != 0)
                {
                    score += Weights.PROTECTED;
                }

                if ((board.blackPieces & Masks.WhiteMasks.WestAttack[index]) != 0)
                {
                    score += Weights.PROTECTED;
                }
            }

            return score;
        }
    }
}

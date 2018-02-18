#include "stdafx.h"
#include "Analyzer.h"
#include "BitsMagic.h" 
#include "Masks.h"

int Analyzer::WIN = INT_MIN;
int Analyzer::LOSS = INT_MAX;

int Analyzer::VERTICAL_CONNECTION = 500;
int Analyzer::HORIZONTAL_CONNECTION = 500;
int Analyzer::PROTECTED = 2500;
int Analyzer::ATTACKED = 10000;
int Analyzer::DANGER_LOW = 5000;
int Analyzer::DANGER_HIGH = 100000;
int Analyzer::FLAT_DANGER = 750;
int Analyzer::IMMEDIATE_MOVEMENT = 150;
int Analyzer::COLUMN_HOLE_PENALTY = 200;
int Analyzer::HOME_ROW_MOVED = 5000;

Analyzer::Analyzer()
{
}

PlayerColor Analyzer::AiColor = PlayerColor::White;
BitBoard Analyzer::_bestMove;


Analyzer::~Analyzer()
{
}


vector<BitBoard> Analyzer::GetChildren(BitBoard board, PlayerColor color)
{
    vector<BitBoard> children;
    unsigned long long myBoard = color == PlayerColor::White ? board.whitePieces : board.blackPieces;
    int iterator = 0;
    do
    {
        iterator = BitsMagic::BitScanForwardWithReset(myBoard);
        if (iterator != -1)
        {
            if (color == PlayerColor::White)
            {
                unsigned long long forward = Masks::WhiteMasks::Forward[iterator];
                unsigned long long east = Masks::WhiteMasks::EastAttack[iterator];
                unsigned long long west = Masks::WhiteMasks::WestAttack[iterator];

                if (forward != 0 && (forward & board.CombinedBoard()) == 0)
                {
                    BitBoard child;
                    child.whitePieces = board.whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.whitePieces = child.whitePieces | forward;
                    child.blackPieces = board.blackPieces;
                    children.push_back(child);
                }

                if (east != 0 && (east & board.whitePieces) == 0)
                {
                    BitBoard child;
                    child.whitePieces = board.whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.whitePieces = child.whitePieces | east;
                    child.blackPieces = board.blackPieces & ~east;
                    children.push_back(child);
                }

                if (west != 0 && (west & board.whitePieces) == 0)
                {
                    BitBoard child;
                    child.whitePieces = board.whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.whitePieces = child.whitePieces | west;
                    child.blackPieces = board.blackPieces & ~west;
                    children.push_back(child);
                }
            }
            else if (color == PlayerColor::Black)
            {
                unsigned long long forward = Masks::BlackMasks::Forward[iterator];
                unsigned long long east = Masks::BlackMasks::EastAttack[iterator];
                unsigned long long west = Masks::BlackMasks::WestAttack[iterator];

                if (forward != 0 && (forward & board.CombinedBoard()) == 0)
                {
                    BitBoard child;
                    child.blackPieces = board.blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.blackPieces = child.blackPieces | forward;
                    child.whitePieces = board.whitePieces;
                    children.push_back(child);
                }

                if (east != 0 && (east & board.blackPieces) == 0)
                {
                    BitBoard child;
                    child.blackPieces = board.blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.blackPieces = child.blackPieces | east;
                    child.whitePieces = board.whitePieces & ~east;
                    children.push_back(child);
                }

                if (west != 0 && (west & board.blackPieces) == 0)
                {
                    BitBoard child;
                    child.blackPieces = board.blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.blackPieces = child.blackPieces | west;
                    child.whitePieces = board.whitePieces & ~west;
                    children.push_back(child);
                }
            }
        }
    } while (iterator != -1);

    return children;
}

int Analyzer::AlphaBetaLoop(BitBoard node, int remainingDepth, int alpha, int beta, bool maximizingPlayer, int & bestScore) 
{
    if (remainingDepth == 0 || IsGameOver(node)) 
    {
        return Evaluate(node);
    }

    vector<BitBoard> children = GetChildren(node, maximizingPlayer ? AiColor : FlipColor(AiColor));

    if (maximizingPlayer) 
    {
        int value = INT_MIN;
        
        for (BitBoard child : children) 
        {
            int childScore = AlphaBetaLoop(child, remainingDepth - 1, alpha, beta, false, bestScore);

            if (remainingDepth == MAX_DEPTH)
            {
                if (childScore > bestScore)
                {
                    _bestMove = child;
                    bestScore = childScore;
                }
            }
            value = max(value, childScore);
            alpha = max(alpha, value);

            if (beta <= alpha) break;
        }

        return value;
    }
    else if(!maximizingPlayer)
    {
        int value = INT_MAX;
        

        for (BitBoard child : children) 
        {
            int childScore = AlphaBetaLoop(child, remainingDepth - 1, alpha, beta, true, bestScore);
            
            if (remainingDepth == MAX_DEPTH) 
            {
                if (childScore > bestScore)
                {
                    _bestMove = child;
                    bestScore = childScore;
                }
            }
            value = min(value, childScore);
            beta = min(beta, value);

            if (beta <= alpha) break;
        }
        return value;
    }
}

bool Analyzer::IsGameOver(BitBoard bitBoard) 
{
    unsigned long long  myWhitePieces = bitBoard.whitePieces;
    unsigned long long  myBlackPieces = bitBoard.blackPieces;

    if (myWhitePieces == 0 || myBlackPieces == 0)
    {
        return true;
    }

    int piece = BitsMagic::BitScanForwardWithReset(myWhitePieces);
    while (piece >= 0)
    {
        if ((Masks::OrientationMasks::CurrentSquare[piece] & Grid::Row8) != 0)
        {
            return true;
        }
        piece = BitsMagic::BitScanForwardWithReset(myWhitePieces);
    }

    piece = BitsMagic::BitScanForwardWithReset(myBlackPieces);
    while (piece >= 0)
    {
        if ((Masks::OrientationMasks::CurrentSquare[piece] & Grid::Row1) != 0)
        {
            return true;
        }
        piece = BitsMagic::BitScanForwardWithReset(myBlackPieces);
    }

    return false;
}

int Analyzer::Evaluate(BitBoard board) 
{
    int totalScore = 0;

    //Skips the evaluation if somebody has won the game.
    if (IsGameOver(board))
    {
        if ((Grid::Row1 & board.blackPieces) != 0)
        {
            return AiColor == PlayerColor::Black ? WIN : LOSS;
        }
        else if ((Grid::Row8 & board.whitePieces) != 0)
        {
            return AiColor == PlayerColor::White ? WIN : LOSS;
        }
    }

    //Accumulate Score for each White Piece.
    int whiteScore = 0;
    unsigned long long whitePieces = board.whitePieces;
    int iterator = BitsMagic::BitScanForwardWithReset(whitePieces);
    while (iterator >= 0)
    {
        whiteScore += GenerateBlockingPatternScore(board, iterator, PlayerColor::White);

        int pieceProtectionScore = GenerateProtectionScore(board, iterator, PlayerColor::White);
        whiteScore += pieceProtectionScore;
        int pieceAttackedPenalty = GenerateAttackedPenalty(board, iterator, PlayerColor::White);

        if (pieceAttackedPenalty > 0)
        {
            whiteScore -= pieceAttackedPenalty;
            if (pieceProtectionScore == 0)
            {
                whiteScore -= pieceAttackedPenalty;
            }
        }
        else
        {
            if (pieceProtectionScore != 0)
            {
                if (Masks::OrientationMasks::CurrentRow[iterator] == 6)
                {
                    whiteScore += DANGER_LOW;
                }
                else if (Masks::OrientationMasks::CurrentRow[iterator] == 7)
                {
                    whiteScore += DANGER_HIGH;
                }
            }
        }

        whiteScore += GenerateDangerScore(board, iterator, PlayerColor::White);

        whiteScore += GenerateMobilityScore(board, iterator, PlayerColor::White);

        iterator = BitsMagic::BitScanForwardWithReset(whitePieces);
    }

    whiteScore -= GenerateColumnHolePenalty(board, PlayerColor::White);
    whiteScore -= GenerateHomeRowProtectionPenalty(board, PlayerColor::White);


    //Acculmulate score for all black pieces.
    int blackScore = 0;
    unsigned long long blackPieces = board.whitePieces;
    iterator = BitsMagic::BitScanForwardWithReset(blackPieces);
    while (iterator >= 0)
    {
        blackScore += GenerateBlockingPatternScore(board, iterator, PlayerColor::Black);

        int pieceProtectionScore = GenerateProtectionScore(board, iterator, PlayerColor::Black);
        blackScore += pieceProtectionScore;
        int pieceAttackedPenalty = GenerateAttackedPenalty(board, iterator, PlayerColor::Black);

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
                if (Masks::OrientationMasks::CurrentRow[iterator] == 3)
                {
                    blackScore += DANGER_LOW;
                }
                else if (Masks::OrientationMasks::CurrentRow[iterator] == 2)
                {
                    blackScore += DANGER_HIGH;
                }
            }
        }

        blackScore += GenerateDangerScore(board, iterator, PlayerColor::Black);
        blackScore += GenerateMobilityScore(board, iterator, PlayerColor::Black);

        iterator = BitsMagic::BitScanForwardWithReset(blackPieces);
    }

    blackScore -= GenerateColumnHolePenalty(board, PlayerColor::Black);
    blackScore -= GenerateHomeRowProtectionPenalty(board, PlayerColor::Black);


    //Finalize Score
    if (AiColor == PlayerColor::White)
    {
        totalScore = whiteScore - blackScore;
    }
    else
    {
        totalScore = blackScore - whiteScore;
    }

    return totalScore;
}

int Analyzer::GenerateBlockingPatternScore(BitBoard board, int index, PlayerColor color)
{
    /*
    * Generate a score for a piece's blocking patterns.  Does it form vertical
    * and horizontal chains of pieces.  Eventually should check for if the chains
    * exist in the mobility triangles of high danger enemy pieces.
    */
    int score = 0;

    if (color == PlayerColor::White)
    {
        if ((board.whitePieces & Masks::OrientationMasks::Above[index]) != 0)
        {
            score += VERTICAL_CONNECTION;
        }

        if ((board.whitePieces & Masks::OrientationMasks::RightOf[index]) != 0)
        {
            score += HORIZONTAL_CONNECTION;
        }
    }
    else if (color == PlayerColor::Black)
    {
        if ((board.blackPieces & Masks::OrientationMasks::Above[index]) != 0)
        {
            score += VERTICAL_CONNECTION;
        }

        if ((board.blackPieces & Masks::OrientationMasks::RightOf[index]) != 0)
        {
            score += HORIZONTAL_CONNECTION;
        }
    }

    return score;
}

int Analyzer::GenerateMobilityScore(BitBoard board, int index, PlayerColor color)
{
    /*
    * Generate a score for a pieces mobility::  This takes into account immediate moves,
    * as well as the entire mobility triangle of the piece::  The less enemy pieces
    * in the triangle, the better the score::
    */
    int score = 0;

    if (color == PlayerColor::White)
    {
        if ((board.CombinedBoard() & Masks::WhiteMasks::Forward[index]) == 0)
        {
            score += IMMEDIATE_MOVEMENT;
        }
        if ((board.whitePieces & Masks::WhiteMasks::EastAttack[index]) == 0)
        {
            score += IMMEDIATE_MOVEMENT;
        }
        if ((board.whitePieces & Masks::WhiteMasks::WestAttack[index]) == 0)
        {
            score += IMMEDIATE_MOVEMENT;
        }
    }
    else if (color == PlayerColor::Black)
    {
        if ((board.CombinedBoard() & Masks::BlackMasks::Forward[index]) == 0)
        {
            score += IMMEDIATE_MOVEMENT;
        }
        if ((board.blackPieces & Masks::BlackMasks::EastAttack[index]) == 0)
        {
            score += IMMEDIATE_MOVEMENT;
        }
        if ((board.blackPieces & Masks::BlackMasks::WestAttack[index]) == 0)
        {
            score += IMMEDIATE_MOVEMENT;
        }
    }

    return score;
}

int Analyzer::GenerateColumnHolePenalty(BitBoard board, PlayerColor color)
{
    /*
    * Generates a penalty for not having sufficient coverage of all columns.
    * Pieces must sufficiently threaten every column on the board. (I'm not sure
    * that I actually agree with this score.  I feel like simply prioritizing home
    * row protection should be sufficient.)
    */
    int penalty = 0;

    if (color == PlayerColor::White)
    {
        if ((board.whitePieces & Grid::ColA) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.whitePieces & Grid::ColB) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.whitePieces & Grid::ColC) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.whitePieces & Grid::ColD) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.whitePieces & Grid::ColE) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.whitePieces & Grid::ColF) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.whitePieces & Grid::ColG) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.whitePieces & Grid::ColH) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
    }
    else if (color == PlayerColor::Black)
    {
        if ((board.blackPieces & Grid::ColA) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.blackPieces & Grid::ColB) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.blackPieces & Grid::ColC) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.blackPieces & Grid::ColD) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.blackPieces & Grid::ColE) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.blackPieces & Grid::ColF) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.blackPieces & Grid::ColG) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
        if ((board.blackPieces & Grid::ColH) == 0)
        {
            penalty += COLUMN_HOLE_PENALTY;
        }
    }

    return penalty;
}

int Analyzer::GenerateHomeRowProtectionPenalty(BitBoard board, PlayerColor color)
{
    /*
    * If the back four have moved, create a large penalty.
    */
    int penalty = 0;

    if (color == PlayerColor::White)
    {
        if ((board.whitePieces & Grid::B1) == 0)
        {
            penalty += HOME_ROW_MOVED;
        }
        if ((board.whitePieces & Grid::C1) == 0)
        {
            penalty += HOME_ROW_MOVED;
        }
        if ((board.whitePieces & Grid::F1) == 0)
        {
            penalty += HOME_ROW_MOVED;
        }
        if ((board.whitePieces & Grid::G1) == 0)
        {
            penalty += HOME_ROW_MOVED;
        }
    }
    else if (color == PlayerColor::Black)
    {
        if ((board.blackPieces & Grid::B8) == 0)
        {
            penalty += HOME_ROW_MOVED;
        }
        if ((board.blackPieces & Grid::C8) == 0)
        {
            penalty += HOME_ROW_MOVED;
        }
        if ((board.blackPieces & Grid::F8) == 0)
        {
            penalty += HOME_ROW_MOVED;
        }
        if ((board.blackPieces & Grid::G8) == 0)
        {
            penalty += HOME_ROW_MOVED;
        }
    }

    return penalty;
}

int Analyzer::GenerateDangerScore(BitBoard board, int index, PlayerColor color)
{
    /*
    * Generates a score based simply on proximity to enemy home row.
    */
    int score = 0;

    if (color == PlayerColor::White)
    {
        score = Masks::OrientationMasks::CurrentRow[index] * FLAT_DANGER;
    }
    else if (color == PlayerColor::Black)
    {
        score = (9 - Masks::OrientationMasks::CurrentRow[index])* FLAT_DANGER;
    }

    return score;
}

int Analyzer::GenerateAttackedPenalty(BitBoard board, int index, PlayerColor color)
{
    /*
    * Generates a penalty based on number of pieces attacking this piece, doubled
    * if the piece is unprotected.
    */
    int penalty = 0;

    if (color == PlayerColor::White)
    {
        if ((board.blackPieces & Masks::WhiteMasks::EastAttack[index]) != 0)
        {
            penalty += ATTACKED;
        }

        if ((board.blackPieces & Masks::WhiteMasks::WestAttack[index]) != 0)
        {
            penalty += ATTACKED;
        }
    }
    else if (color == PlayerColor::Black)
    {
        if ((board.whitePieces & Masks::BlackMasks::EastAttack[index]) != 0)
        {
            penalty += ATTACKED;
        }

        if ((board.whitePieces & Masks::BlackMasks::WestAttack[index]) != 0)
        {
            penalty += ATTACKED;
        }
    }

    return penalty;
}

int Analyzer::GenerateProtectionScore(BitBoard board, int index, PlayerColor color)
{
    /*
    * Generates a score for having pieces protectecting the piece in question.
    * We use the opposite color's attack patterns to quickly check if we are
    * protected.
    */
    int score = 0;

    if (color == PlayerColor::White)
    {
        if ((board.whitePieces & Masks::BlackMasks::EastAttack[index]) != 0)
        {
            score += PROTECTED;
        }

        if ((board.whitePieces & Masks::BlackMasks::WestAttack[index]) != 0)
        {
            score += PROTECTED;
        }
    }
    else if (color == PlayerColor::Black)
    {
        if ((board.blackPieces & Masks::WhiteMasks::EastAttack[index]) != 0)
        {
            score += PROTECTED;
        }

        if ((board.blackPieces & Masks::WhiteMasks::WestAttack[index]) != 0)
        {
            score += PROTECTED;
        }
    }

    return score;
}

BitBoard Analyzer::GetMove(BitBoard board, PlayerColor color) 
{
    AiColor = color;
    int bestScore = INT_MIN;
    AlphaBetaLoop(board, MAX_DEPTH, INT_MIN, INT_MAX, true, bestScore);
    return _bestMove;
}

void Analyzer::SetWeights(Weights weights)
{
    VERTICAL_CONNECTION = weights.VERTICAL_CONNECTION;
    HORIZONTAL_CONNECTION = weights.HORIZONTAL_CONNECTION;
    PROTECTED = weights.PROTECTED;
    ATTACKED = weights.ATTACKED;
    DANGER_LOW = weights.DANGER_LOW;
    DANGER_HIGH = weights.DANGER_HIGH;
    FLAT_DANGER = weights.FLAT_DANGER;
    IMMEDIATE_MOVEMENT = weights.IMMEDIATE_MOVEMENT;
    COLUMN_HOLE_PENALTY = weights.COLUMN_HOLE_PENALTY;
    HOME_ROW_MOVED = weights.HOME_ROW_MOVED;
}
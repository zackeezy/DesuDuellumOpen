#include "stdafx.h"
#include "Analyzer.h"
#include "BitsMagic.h" 
#include "Masks.h"
#include <iostream>
#include <chrono>
#include <thread>
#include <random>
using namespace std;
void PrintBoard(BitBoard board, int score, PlayerColor color)
{
    system("cls");

    vector<vector<char>> _board;

    for (int i = 0; i < 8; i++)
    {
        vector<char> temp;
        for (int j = 0; j < 8; j++)
        {
            temp.push_back(' ');
        }
        _board.push_back(temp);
    }

    unsigned long long iterator = 1;
    for (int row = 7; row >= 0; row--)
    {
        for (int column = 7; column >= 0; column--)
        {
            if ((board.whitePieces & iterator) != 0)
            {
                _board[row][column] = 'W';
            }
            else if ((board.blackPieces & iterator) != 0)
            {
                _board[row][column] = 'B';
            }
            else
            {
                _board[row][column] = ' ';
            }
            iterator <<= 1;
        }
    }

    for (int row = 7; row >= 0; row--)
    {
        for (int column = 0; column <= 7; column++)
        {
            cout << _board[row][column];
            if (column != 7)
            {
                cout << "|";
            }
        }
        cout << endl;
        if (row != 0)
        {
            cout << "---------------" << endl;
        }
    }

    if (color == PlayerColor::White)
    {
        cout << "White's Move";
    }
    else {
        cout << "Black's Move";
    }

    cout << endl;

    cout << "Score: " << score;

    std::this_thread::sleep_for(std::chrono::milliseconds(250));
}

Analyzer::Analyzer()
{
}

PlayerColor Analyzer::AiColor = PlayerColor::White;

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
                    child.currentTurn = FlipColor(color);
                    child.whitePieces = board.whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.whitePieces = child.whitePieces | forward;
                    child.blackPieces = board.blackPieces;
                    children.push_back(child);
                }

                if (east != 0 && (east & board.whitePieces) == 0)
                {
                    BitBoard child;
                    child.currentTurn = FlipColor(color);
                    child.whitePieces = board.whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.whitePieces = child.whitePieces | east;
                    child.blackPieces = board.blackPieces & ~east;
                    children.push_back(child);
                }

                if (west != 0 && (west & board.whitePieces) == 0)
                {
                    BitBoard child;
                    child.currentTurn = FlipColor(color);
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
                    child.currentTurn = FlipColor(color);
                    child.blackPieces = board.blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.blackPieces = child.blackPieces | forward;
                    child.whitePieces = board.whitePieces;
                    children.push_back(child);
                }

                if (east != 0 && (east & board.blackPieces) == 0)
                {
                    BitBoard child;
                    child.currentTurn = FlipColor(color);
                    child.blackPieces = board.blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.blackPieces = child.blackPieces | east;
                    child.whitePieces = board.whitePieces & ~east;
                    children.push_back(child);
                }

                if (west != 0 && (west & board.blackPieces) == 0)
                {
                    BitBoard child;
                    child.currentTurn = FlipColor(color);
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

int Analyzer::AlphaBetaLoop(BitBoard node, int remainingDepth, int alpha, int beta, bool maximizingPlayer, BitBoard & bestMove, int & bestScore) 
{
    if (remainingDepth == 0 || IsGameOver(node)) 
    {
        return Evaluate(node, remainingDepth);
    }

    vector<BitBoard> children = GetChildren(node, maximizingPlayer ? AiColor : FlipColor(AiColor));
    vector<int> scores;

    if (maximizingPlayer) 
    {
        int value = INT_MIN;
        
        for (BitBoard child : children) 
        {
            int childScore = AlphaBetaLoop(child, remainingDepth - 1, alpha, beta, false, bestMove ,bestScore);

            if (remainingDepth == MAX_DEPTH)
            {
                //PrintBoard(child, childScore, AiColor);
                scores.push_back(childScore);
            }

            value = max(value, childScore);
            alpha = max(alpha, value);

            if (beta <= alpha) break;
        }


        if (remainingDepth == MAX_DEPTH) 
        {
            int bestScore = INT_MIN;
            for (int i = 0; i < scores.size(); i++) 
            {
                if (scores[i] >= INT_MIN)
                {
                    bestScore = scores[i];
                }
            }
            vector<BitBoard> bestMoves;
            for (int i = 0; i < scores.size(); i++)
            {
                if (scores[i] == bestScore)
                {
                    bestMoves.push_back(children[i]);
                }
            }

            std::random_device rd;
            std::mt19937 mt(rd());
            std::uniform_int_distribution<int> dist(0, bestMoves.size() - 1);

            bestMove = bestMoves[dist(mt)];
        }
        return value;
    }
    else if (!maximizingPlayer)
    {
        int value = INT_MAX;


        for (BitBoard child : children)
        {
            int childScore = AlphaBetaLoop(child, remainingDepth - 1, alpha, beta, true, bestMove, bestScore);

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

int Analyzer::Evaluate(BitBoard board, int remainingDepth) 
{
    int totalScore = 0;

    //Skips the evaluation if somebody has won the game.
    if (IsGameOver(board))
    {
        if ((Grid::Row1 & board.blackPieces) != 0)
        {
            return AiColor == PlayerColor::Black ? AUTO_WIN * remainingDepth : LOSS;
        }
        else if ((Grid::Row8 & board.whitePieces) != 0)
        {
            return AiColor == PlayerColor::White ? AUTO_WIN * remainingDepth : LOSS;
        }
        else if (board.blackPieces == 0) 
        {
            return AiColor == PlayerColor::White ? AUTO_WIN * remainingDepth : LOSS;
        }
        else if (board.whitePieces == 0)
        {
            return AiColor == PlayerColor::Black ? AUTO_WIN * remainingDepth : LOSS;
        }
    }

     //Accumulate Score for each White Piece.
    int whiteScore = 0;
    unsigned long long whitePieces = board.whitePieces;
    int iterator = BitsMagic::BitScanForwardWithReset(whitePieces);
    int whiteCount = 0;
    while (iterator >= 0)
    {
        whiteCount++;
        whiteScore += GenerateBlockingPatternScore(board, iterator, PlayerColor::White);
        whiteScore += GenerateCombatScore(board, iterator, PlayerColor::White);
        whiteScore += GenerateDangerScore(board, iterator, PlayerColor::White);
        whiteScore += GenerateMobilityScore(board, iterator, PlayerColor::White);
        //whiteScore += GenerateTriangleScores(board, iterator, PlayerColor::White);
        iterator = BitsMagic::BitScanForwardWithReset(whitePieces);
    }

    whiteScore += GenerateColumnHoleScore(board, PlayerColor::White);
    whiteScore += GenerateHomeRowScore(board, PlayerColor::White);
    whiteScore += whiteCount * PIECE_TOTAL_SCALAR;


    //Acculmulate score for all black pieces.
    int blackScore = 0;
    unsigned long long blackPieces = board.blackPieces;
    iterator = BitsMagic::BitScanForwardWithReset(blackPieces);
    int blackCount = 0;
    while (iterator >= 0)
    {
        blackCount++;
        blackScore += GenerateBlockingPatternScore(board, iterator, PlayerColor::Black);
        blackScore += GenerateCombatScore(board, iterator, PlayerColor::Black);
        blackScore += GenerateDangerScore(board, iterator, PlayerColor::Black);
        blackScore += GenerateMobilityScore(board, iterator, PlayerColor::Black);
        //blackScore += GenerateTriangleScores(board, iterator, PlayerColor::Black);

        iterator = BitsMagic::BitScanForwardWithReset(blackPieces);
    }

    blackScore += GenerateColumnHoleScore(board, PlayerColor::Black);
    blackScore += GenerateHomeRowScore(board, PlayerColor::Black);
    blackScore += blackCount * PIECE_TOTAL_SCALAR;

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

int Analyzer::GenerateColumnHoleScore(BitBoard board, PlayerColor color)
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

int Analyzer::GenerateHomeRowScore(BitBoard board, PlayerColor color)
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
        score = Masks::OrientationMasks::CurrentRow[index] * DANGER_SCALAR;
    }
    else if (color == PlayerColor::Black)
    {
        score = (9 - Masks::OrientationMasks::CurrentRow[index])* DANGER_SCALAR;
    }

    return score;
}

BitBoard Analyzer::GetMove(BitBoard board, PlayerColor color) 
{
    Analyzer::AiColor = color;
    int bestScore = INT_MIN;
    BitBoard bestMove;
    bestMove.currentTurn = color;
    AlphaBetaLoop(board, MAX_DEPTH, INT_MIN, INT_MAX, true, bestMove, bestScore);
    return bestMove;
}

int Analyzer::GenerateCombatScore(BitBoard board, int index, PlayerColor color) 
{
    int score = 0;
    int attackCount = 0;
    int protectCount = 0;
    /*int lowA = 0;
    int lowB = 0;
    int high = 0;*/

    if (color == PlayerColor::White)
    {
        /*lowA = 5;
        lowB = 6;
        high = 7;*/
        if ((board.whitePieces & Masks::BlackMasks::EastAttack[index]) != 0)
        {
            protectCount++;
        }

        if ((board.whitePieces & Masks::BlackMasks::WestAttack[index]) != 0)
        {
            protectCount++;
        }

        if ((board.blackPieces & Masks::WhiteMasks::EastAttack[index]) != 0) 
        {
            attackCount++;
        }
        
        if ((board.blackPieces & Masks::WhiteMasks::WestAttack[index]) != 0)
        {
            attackCount++;
        }
    }
    else if (color == PlayerColor::Black)
    {
   /*     lowA = 4;
        lowB = 3;
        high = 2;*/
        if ((board.blackPieces & Masks::WhiteMasks::EastAttack[index]) != 0)
        {
            protectCount++;
        }

        if ((board.blackPieces & Masks::WhiteMasks::WestAttack[index]) != 0)
        {
            protectCount++;
        }

        if ((board.whitePieces & Masks::BlackMasks::EastAttack[index]) != 0)
        {
            attackCount++;
        }

        if ((board.whitePieces & Masks::BlackMasks::WestAttack[index]) != 0)
        {
            attackCount++;
        }
    }

    if (attackCount == 0) 
    {
        /*if (Masks::OrientationMasks::CurrentRow[index] == lowA || Masks::OrientationMasks::CurrentRow[index] == lowB)
        {
            score += DANGER_LOW;
        }
        else if (Masks::OrientationMasks::CurrentRow[index] == high)
        {
            score += DANGER_HIGH;
        }*/
    }

    for (int i = 0; i < protectCount; i++)
    {
        score += PROTECT_SCORE;
    }

    if (attackCount > protectCount)
    {
        score += UNPROTECTED_ATTACK;
    }
    else if (protectCount >= attackCount) 
    {
        for (int i = 0; i < attackCount; i++)
        {
            score += ATTACK_SCORE;
        }
    }
    else if (attackCount > protectCount) 
    {
        for (int i = protectCount; i < attackCount; i++)
        {
            score -= ATTACK_SCORE;
        }
    }

    return score;
}

int Analyzer::GenerateTriangleScores(BitBoard board, int index, PlayerColor color) 
{   
    int score = 0;

    if (color == PlayerColor::White) 
    {
        unsigned long long enemyPiecesInTriangle = Masks::WhiteMasks::MobilityTriangle[index] & board.blackPieces;
        int enemyCount = 0;
        int iterator = BitsMagic::BitScanForwardWithReset(enemyPiecesInTriangle);
        while (iterator >= 0)
        {
            enemyCount++;
            iterator = BitsMagic::BitScanForwardWithReset(enemyPiecesInTriangle);
        }

        //if enemy side
        //few pieces in triangle.
        //if our side
        //Many enemies in triangle.
        if (Masks::OrientationMasks::CurrentRow[index] >= 6) 
        {
            score -= (enemyCount * TRIANGLE_SCALAR);
        }
        else
        {
            score += (enemyCount * TRIANGLE_SCALAR);
        }
    }
    else
    {
        unsigned long long enemyPiecesInTriangle = Masks::BlackMasks::MobilityTriangle[index] & board.whitePieces;
        int enemyCount = 0;
        int iterator = BitsMagic::BitScanForwardWithReset(enemyPiecesInTriangle);
        while (iterator >= 0)
        {
            enemyCount++;
            iterator = BitsMagic::BitScanForwardWithReset(enemyPiecesInTriangle);
        }

        if (Masks::OrientationMasks::CurrentRow[index] <= 3)
        {
            score -= (enemyCount * TRIANGLE_SCALAR);
        }
        else
        {
            score += (enemyCount * TRIANGLE_SCALAR);
        }
    }

    return score;
}

bool Analyzer::IsStupidMove(BitBoard board, int index, PlayerColor color)
{
    int attackCount = 0;
    int protectCount = 0;

    if (color == PlayerColor::White)
    {
        /*lowA = 5;
        lowB = 6;
        high = 7;*/
        if ((board.whitePieces & Masks::BlackMasks::EastAttack[index]) != 0)
        {
            protectCount++;
        }

        if ((board.whitePieces & Masks::BlackMasks::WestAttack[index]) != 0)
        {
            protectCount++;
        }

        if ((board.blackPieces & Masks::WhiteMasks::EastAttack[index]) != 0)
        {
            attackCount++;
        }

        if ((board.blackPieces & Masks::WhiteMasks::WestAttack[index]) != 0)
        {
            attackCount++;
        }
    }
    else if (color == PlayerColor::Black)
    {
        /*     lowA = 4;
        lowB = 3;
        high = 2;*/
        if ((board.blackPieces & Masks::WhiteMasks::EastAttack[index]) != 0)
        {
            protectCount++;
        }

        if ((board.blackPieces & Masks::WhiteMasks::WestAttack[index]) != 0)
        {
            protectCount++;
        }

        if ((board.whitePieces & Masks::BlackMasks::EastAttack[index]) != 0)
        {
            attackCount++;
        }

        if ((board.whitePieces & Masks::BlackMasks::WestAttack[index]) != 0)
        {
            attackCount++;
        }
    }

    if (attackCount > protectCount) 
    {
        return true;
    }
    return false;
}

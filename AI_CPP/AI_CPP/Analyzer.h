#pragma once
#include "Utils.h"
#include <climits>


static class Analyzer
{
private:
    static const int MAX_DEPTH = 5;

    static const int WIN = INT_MIN;
    static const int LOSS = INT_MAX;
    static const int VERTICAL_CONNECTION = 100;
    static const int HORIZONTAL_CONNECTION = 100;
    static const int PROTECTED = 250;
    static const int ATTACKED = 100;
    static const int DANGER_LOW = 500;
    static const int DANGER_HIGH = 10000;
    static const int FLAT_DANGER = 10000;
    static const int IMMEDIATE_MOVEMENT = 150;
    static const int COLUMN_HOLE_PENALTY = 200;
    static const int BASE_MOBILITY = 10000;
    static const int MOBILITY_PENALTY = 625;
    static const int HOME_ROW_MOVED = 5000;

    static BitBoard * GetChildren(BitBoard board, PlayerColor color, int & childCount);
    static AlphaBetaNode AlphaBetaLoop(BitBoard node, int remainingDepth, int alpha, int beta, bool maximizingPlayer);
    static bool IsGameOver(BitBoard bitBoard);
    static int Evaluate(BitBoard board);
    static int GenerateBlockingPatternScore(BitBoard board, int index, PlayerColor color);
    static int GenerateMobilityScore(BitBoard board, int index, PlayerColor color);
    static int GenerateColumnHolePenalty(BitBoard board, PlayerColor color);
    static int GenerateHomeRowProtectionPenalty(BitBoard board, PlayerColor color);
    static int GenerateAlmostWinScore(BitBoard board, int index);
    static int GenerateDangerScore(BitBoard board, int index, PlayerColor color);
    static int GenerateAttackedPenalty(BitBoard board, int index, PlayerColor color);
    static int GenerateProtectionScore(BitBoard board, int index, PlayerColor color);

public:


    Analyzer();
    ~Analyzer();
    static PlayerColor AiColor;

    static int * GetMove(int whiteCoordinates[], int whiteCount, int blackCoordinates[], int blackCount);
};


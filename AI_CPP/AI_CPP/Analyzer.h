#pragma once
#include "Utils.h"
#include <vector>
#include <climits>
#include <algorithm>
#include "Grid.h"

using std::vector;
using std::max;
using std::min;


static class Analyzer
{
private:
    static const int MAX_DEPTH = 5;
    static BitBoard _bestMove;

    static const int WIN = INT_MIN;
    static const int LOSS = INT_MAX;
    static const int VERTICAL_CONNECTION = 500;
    static const int HORIZONTAL_CONNECTION = 500;
    static const int PROTECTED = 2500;
    static const int ATTACKED = 10000;
    static const int DANGER_LOW = 5000;
    static const int DANGER_HIGH = 100000;
    static const int FLAT_DANGER = 750;
    static const int IMMEDIATE_MOVEMENT = 150;
    static const int COLUMN_HOLE_PENALTY = 200;
    static const int HOME_ROW_MOVED = 5000;

    static vector<BitBoard> GetChildren(BitBoard board, PlayerColor color);
    static int AlphaBetaLoop(BitBoard node, int remainingDepth, int alpha, int beta, bool maximizingPlayer, int & bestScore);
    static bool IsGameOver(BitBoard bitBoard);
    static int Evaluate(BitBoard board);
    static int GenerateBlockingPatternScore(BitBoard board, int index, PlayerColor color);
    static int GenerateMobilityScore(BitBoard board, int index, PlayerColor color);
    static int GenerateColumnHolePenalty(BitBoard board, PlayerColor color);
    static int GenerateHomeRowProtectionPenalty(BitBoard board, PlayerColor color);
    static int GenerateDangerScore(BitBoard board, int index, PlayerColor color);
    static int GenerateAttackedPenalty(BitBoard board, int index, PlayerColor color);
    static int GenerateProtectionScore(BitBoard board, int index, PlayerColor color);

public:


    Analyzer();
    ~Analyzer();
    static PlayerColor AiColor;

    static BitBoard GetMove(BitBoard board, PlayerColor color);
};


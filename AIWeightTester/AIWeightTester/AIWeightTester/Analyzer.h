#pragma once
#include "Utils.h"
#include <vector>
#include <climits>
#include <algorithm>
#include "Grid.h"
#include "Weights.h"

using std::vector;
using std::max;
using std::min;

static class Analyzer
{
private:
    static const int MAX_DEPTH = 4;
    static BitBoard _bestMove;

    static vector<BitBoard> GetChildren(BitBoard board, PlayerColor color);
    static int AlphaBetaLoop(BitBoard node, int remainingDepth, int alpha, int beta, bool maximizingPlayer, int & bestScore);
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
    static bool IsGameOver(BitBoard bitBoard);

    static int WIN;
    static int LOSS;
    static int VERTICAL_CONNECTION;
    static int HORIZONTAL_CONNECTION;
    static int PROTECTED;
    static int ATTACKED;
    static int DANGER_LOW;
    static int DANGER_HIGH;
    static int FLAT_DANGER;
    static int IMMEDIATE_MOVEMENT;
    static int COLUMN_HOLE_PENALTY;
    static int HOME_ROW_MOVED;

    static void SetWeights(Weights weights);
};


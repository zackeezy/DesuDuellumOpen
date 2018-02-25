#pragma once
#include "Utils.h"
#include <vector>
#include <climits>
#include <algorithm>
#include "Grid.h"
#include <map>
#include <utility>


using std::map;
using std::vector;
using std::max;
using std::min;
using namespace std;


static class Analyzer
{
private:
    static int MAX_DEPTH;
    static map<unsigned long long, pair<int,int>> TranspositionTable;
    static unsigned long long ZobristTable[64][2];

    static const int WHITE = 0;
    static const int BLACK = 1;

    static const int MAX_TIME = 2;
    static const int AUTO_WIN = 100000000;
    static const int WIN = INT_MAX;
    static const int LOSS = INT_MIN;
    static const int VERTICAL_CONNECTION = 15;
    static const int HORIZONTAL_CONNECTION = 15;
    static const int DANGER_LOW = 25;
    static const int DANGER_HIGH = 50;
    static const int DANGER_SCALAR = 25;
    static const int IMMEDIATE_MOVEMENT = 10;
    static const int COLUMN_HOLE_PENALTY = -15;
    static const int HOME_ROW_MOVED = -10000;

    static const int PIECE_TOTAL_SCALAR = 50000;
    static const int UNPROTECTED_ATTACK = -20000;
    static const int UNPROTECTED_PIECE = -2000;
    
    static const int PROTECT_SCORE = 2000;
    static const int ATTACK_SCORE = 1000;

    static const int TRIANGLE_SCALAR = 25;
    
    static vector<BitBoard> GetChildren(BitBoard board, PlayerColor color);
    static int AlphaBetaLoop(BitBoard node, int remainingDepth, int alpha, int beta, bool maximizingPlayer, BitBoard & bestMove, int & bestScore);
    static int Evaluate(BitBoard board, int remainingDepth);
    static int GenerateBlockingPatternScore(BitBoard board, int index, PlayerColor color);
    static int GenerateMobilityScore(BitBoard board, int index, PlayerColor color);
    static int GenerateColumnHoleScore(BitBoard board, PlayerColor color);
    static int GenerateHomeRowScore(BitBoard board, PlayerColor color);
    static int GenerateDangerScore(BitBoard board, int index, PlayerColor color);
    static int GenerateCombatScore(BitBoard board, int index, PlayerColor color);
    static int GenerateTriangleScores(BitBoard board, int index, PlayerColor color);
    bool IsStupidMove(BitBoard board, int index, PlayerColor color);

    static BitBoard IterativeDeepeningAlphaBeta(BitBoard board);
    static void InitializeZobristTable();
    static unsigned long long HashBoard(BitBoard board);


public:
    static bool IsGameOver(BitBoard bitBoard);


    Analyzer();
    ~Analyzer();
    static PlayerColor AiColor;

    static BitBoard GetMove(BitBoard board, PlayerColor color);
};


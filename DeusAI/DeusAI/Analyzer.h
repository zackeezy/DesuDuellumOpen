#pragma once
#include "Utils.h"
#include "Node.h"
#include <random>
#include <vector>

using namespace std;
static class Analyzer
{
private:
    static int _totalPlayOuts;

    static const int MAX_TIME = 5900;
    static const int PIECE_SCALAR = 10;
    static const int WhiteScoreArray[64];
    static const int BlackScoreArray[64];

public:
    Analyzer();
    ~Analyzer();

    static Move GetMove(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color);
    static Move GetMoveImproved(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color);
    static int GetTotalPlayOuts();
    static void RunPlayOut(Node * root);
    static PlayerColor IsGameOver(Node * node);
    static PlayerColor ScoreGame(Node * node);
};


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
    static vector<int> _gamesAtDepth;
    
public:
    Analyzer();
    ~Analyzer();

    static Move GetMove_Heavy(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color);
    static Move GetMove_Light(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color);
    static int GetTotalPlayOuts();
    static void RunPlayOut_Heavy(Node * root);
    static void RunPlayOut_Light(Node * root);
    static PlayerColor IsGameOver(Node * node);
};


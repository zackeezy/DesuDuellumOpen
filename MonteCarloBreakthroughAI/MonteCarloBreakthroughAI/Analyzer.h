#pragma once
#include "Utils.h"
#include "Node.h"
#include <random>

using namespace std;
static class Analyzer
{
private:
    static int _totalPlayOuts;
    static const int MAX_TIME = 6;
    
public:
    Analyzer();
    ~Analyzer();

    static Move GetMove(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color);
    static int GetTotalPlayOuts();
    static void RunPlayOut(Node * root);
};


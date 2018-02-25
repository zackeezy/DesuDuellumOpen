#pragma once
#include <vector>
#include "Weights.h"
#include "Utils.h"
#include <time.h>
using namespace std;

class TestDriver
{
private:
    vector<vector<char>> _board;

public:
    TestDriver();
    ~TestDriver();

    int _playGame(Weights white, Weights black);
    void PrintBoard(BitBoard board, clock_t t);

    /*void RunTests();
    void SpawnNewGeneration();
    void SaveGeneration();
    void LoadGeneration();*/
};


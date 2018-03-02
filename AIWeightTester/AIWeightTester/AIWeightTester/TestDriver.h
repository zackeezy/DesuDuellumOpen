#pragma once
#include <vector>
#include "Weights.h"
#include "Utils.h"
using namespace std;

class TestDriver
{
private:
    vector<Weights> _weights;
    vector<int> _wins;

    vector<vector<char>> _board;

    int _generationSize;
    int _gamesPerSimulation;

    int _playGame(Weights white, Weights black);

public:
    TestDriver();
    ~TestDriver();

    int TempPlayGame(Weights white, Weights black);
    void PrintBoard(BitBoard board);

    /*void RunTests();
    void SpawnNewGeneration();
    void SaveGeneration();
    void LoadGeneration();*/
};


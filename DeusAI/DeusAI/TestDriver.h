#pragma once
#include <vector>
#include <time.h>
using namespace std;

class TestDriver
{
private:
    vector<vector<char>> _board;

public:
    TestDriver();
    ~TestDriver();

    void PlayGame();
    void PrintBoard(unsigned long long white, unsigned long long black, clock_t t);
};


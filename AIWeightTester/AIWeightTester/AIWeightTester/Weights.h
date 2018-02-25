#pragma once
#include <climits>
class Weights
{
public:
    int WIN = INT_MAX;
    int LOSS = INT_MIN;
    int VERTICAL_CONNECTION;
    int HORIZONTAL_CONNECTION;
    int PROTECTED;
    int ATTACKED;
    int DANGER_LOW;
    int DANGER_HIGH;
    int FLAT_DANGER;
    int IMMEDIATE_MOVEMENT;
    int COLUMN_HOLE_PENALTY;
    int HOME_ROW_MOVED;

    Weights();
    ~Weights();

    void RandomizeWeights();
};


#include "stdafx.h"
#include "Weights.h"
#include <random>
#include <chrono>


Weights::Weights()
{
}

Weights::~Weights()
{
}

void Weights::RandomizeWeights() 
{
    unsigned seed = std::chrono::system_clock::now().time_since_epoch().count();
    std::minstd_rand0 generator(seed);

    this->ATTACKED = generator();
    this->COLUMN_HOLE_PENALTY = generator();
    this->DANGER_HIGH = generator();
    this->DANGER_LOW = generator();
    this->FLAT_DANGER = generator();
    this->HOME_ROW_MOVED = generator();
    this->HORIZONTAL_CONNECTION = generator();
    this->IMMEDIATE_MOVEMENT = generator();
    this->PROTECTED = generator();
    this->VERTICAL_CONNECTION = generator();
}

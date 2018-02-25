#include "stdafx.h"
#include "Weights.h"
#include "TestDriver.h"
#include <iostream>
using namespace std;

int main()
{
    TestDriver driver;

    Weights white;
    Weights black;

    white.RandomizeWeights();
    black.RandomizeWeights();

    if (driver.TempPlayGame(white, black) == 0)
    {
        cout << endl << "White Won!";
    }
    else
    {
        cout << endl << "Black Won!";
    }

    return 0;
}


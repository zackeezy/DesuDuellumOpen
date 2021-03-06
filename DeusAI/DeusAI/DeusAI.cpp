// DeusAI.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"


#include "Node.h"
#include "Analyzer.h"
#include "Masks.h"
#include "TestDriver.h"
static unsigned long long WhitePieces;
static unsigned long long BlackPieces;
static bool IsHardMode;


extern "C" __declspec(dllexport) void __stdcall InitializeAI(bool isHardMode)
{
    IsHardMode = isHardMode;
}

extern "C" __declspec(dllexport) void __stdcall FillOrigin(int x, int y, int color)
{
    if (color == 0)
    {
        WhitePieces |= Masks::OrientationMasks::CoordinatesToUlong[x][y];
    }
    else
    {
        BlackPieces |= Masks::OrientationMasks::CoordinatesToUlong[x][y];
    }
}

extern "C" __declspec(dllexport) void __stdcall GenerateMove(int & fromX, int & fromY, int & direction, int color)
{
    if (IsHardMode)
    {
        Move move = Analyzer::GetMoveImproved(WhitePieces, BlackPieces, color == 0 ? PlayerColor::White : PlayerColor::Black);

        fromX = move.xCoordinate;
        fromY = move.yCoordinate;
        direction = move.direction;
        WhitePieces = 0;
        BlackPieces = 0;
    }
    else
    {
        Move move = Analyzer::GetMove(WhitePieces, BlackPieces, color == 0 ? PlayerColor::White : PlayerColor::Black);

        fromX = move.xCoordinate;
        fromY = move.yCoordinate;
        direction = move.direction;
        WhitePieces = 0;
        BlackPieces = 0;
    }
}

// AI_CPP.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "Analyzer.h"
extern "C" __declspec(dllexport) int * __stdcall GetMove(int whiteCoordinates[], int whiteCount, int blackCoordinates[], int blackCount, int color)
{
    Analyzer::AiColor = (PlayerColor)color;
    int * move = Analyzer::GetMove(whiteCoordinates, whiteCount, blackCoordinates, blackCount);
    return move;
}


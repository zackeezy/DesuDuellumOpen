// AI_CPP.cpp : Defines the exported functions for the DLL application.
//
#include "stdafx.h"
#include "Masks.h"
#include "Utils.h"
#include "Analyzer.h"
#include "BitsMagic.h"

static BitBoard Origin;

extern "C" __declspec(dllexport) void __stdcall FillOrigin(int x, int y, int color) 
{
    if (color == 0) 
    {
        Origin.whitePieces |= Masks::OrientationMasks::CoordinatesToUlong[x][y];
    }
    else
    {
        Origin.blackPieces |= Masks::OrientationMasks::CoordinatesToUlong[x][y];
    }
}

extern "C" __declspec(dllexport) void __stdcall GenerateMove(int & fromX, int & fromY, int & direction, int color)
{
    //call Analyzer::GetMove
    //convert answer to answer variables

    BitBoard move = Analyzer::GetMove(Origin, color == 0 ? PlayerColor::White : PlayerColor::Black);
    
    unsigned long long movedPiece = 0;
    if (color == 0)
    {
        movedPiece = move.whitePieces ^ Origin.whitePieces;
        int destination = BitsMagic::BitScanForwardWithReset(movedPiece);
        int start = BitsMagic::BitScanForwardWithReset(movedPiece);
        fromX = Masks::OrientationMasks::CurrentColumn[start] - 1;
        fromY = Masks::OrientationMasks::CurrentRow[start] - 1;
        
    }
    else if (color == 1) 
    {
        movedPiece = move.blackPieces ^ Origin.blackPieces;
        int start = BitsMagic::BitScanForwardWithReset(movedPiece);
        int destination = BitsMagic::BitScanForwardWithReset(movedPiece);
        fromX = Masks::OrientationMasks::CurrentColumn[start] - 1;
        fromY = Masks::OrientationMasks::CurrentRow[start] - 1;
        if ((Masks::BlackMasks::EastAttack[start] & Masks::OrientationMasks::CurrentSquare[destination]) != 0)
        {
            direction = 0;
        }
        else if ((Masks::BlackMasks::Forward[start] & Masks::OrientationMasks::CurrentSquare[destination]) != 0)
        {
            direction = 1;
        }
        else if ((Masks::BlackMasks::WestAttack[start] & Masks::OrientationMasks::CurrentSquare[destination]) != 0)
        {
            direction = 2;
        }
    }
    Origin.whitePieces = 0;
    Origin.blackPieces = 0;
}

extern "C" __declspec(dllexport) void __stdcall InitializeAI() 
{
    Masks::InitializeMobilityTriangles();
}
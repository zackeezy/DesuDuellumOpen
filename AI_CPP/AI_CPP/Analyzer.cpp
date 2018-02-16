#include "stdafx.h"
#include "Analyzer.h"
#include "BitsMagic.h" 
#include "Masks.h"

Analyzer::Analyzer()
{
}


Analyzer::~Analyzer()
{
}


BitBoard * Analyzer::GetChildren(BitBoard board, PlayerColor color, int & childCount) 
{
    BitBoard * children = new BitBoard[childCount];
    unsigned long long myBoard = color == PlayerColor::White ? board.whitePieces : board.blackPieces;
    int iterator = 0;
    childCount = 0;
    do 
    {
        iterator = BitsMagic::BitScanForwardWithReset(myBoard);
        if (iterator != -1)
        {
            if (color == PlayerColor::White) 
            {
                unsigned long long forward = Masks::WhiteMasks::Forward[iterator];
                unsigned long long east = Masks::WhiteMasks::EastAttack[iterator];
                unsigned long long west = Masks::WhiteMasks::WestAttack[iterator];

                if (forward != 0 && (forward & board.CombinedBoard()) == 0) 
                {
                    BitBoard child;
                    child.whitePieces = board.whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.whitePieces = child.whitePieces | forward;
                    child.blackPieces = board.blackPieces;
                    children[childCount] = child;
                    childCount++;
                }

                if (east != 0 && (east & board.whitePieces) == 0)
                {
                    BitBoard child;
                    child.whitePieces = board.whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.whitePieces = child.whitePieces | east;
                    child.blackPieces = board.blackPieces & ~east;
                    children[childCount] = child;
                    childCount++;
                }

                if (west != 0 && (west & board.whitePieces) == 0)
                {
                    BitBoard child;
                    child.whitePieces = board.whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.whitePieces = child.whitePieces | west;
                    child.blackPieces = board.blackPieces & ~west;
                    children[childCount] = child;
                    childCount++;
                }
            }
            else if (color == PlayerColor::Black)
            {
                unsigned long long forward = Masks::BlackMasks::Forward[iterator];
                unsigned long long east = Masks::BlackMasks::EastAttack[iterator];
                unsigned long long west = Masks::BlackMasks::WestAttack[iterator];

                if (forward != 0 && (forward & board.CombinedBoard()) == 0)
                {
                    BitBoard child;
                    child.blackPieces = board.blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.blackPieces = child.blackPieces | forward;
                    child.whitePieces = board.whitePieces;
                    children[childCount] = child;
                    childCount++;
                }

                if (east != 0 && (east & board.blackPieces) == 0)
                {
                    BitBoard child;
                    child.blackPieces = board.blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.blackPieces = child.blackPieces | east;
                    child.whitePieces = board.whitePieces & ~east;
                    children[childCount] = child;
                    childCount++;
                }

                if (west != 0 && (west & board.blackPieces) == 0)
                {
                    BitBoard child;
                    child.blackPieces = board.blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator];
                    child.blackPieces = child.blackPieces | west;
                    child.whitePieces = board.whitePieces & ~west;
                    children[childCount] = child;
                    childCount++;
                }
            }
        }
    } while (iterator != -1);

    return children;
}
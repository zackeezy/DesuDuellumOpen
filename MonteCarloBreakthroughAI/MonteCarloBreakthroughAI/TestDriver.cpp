#include "stdafx.h"
#include "TestDriver.h"
#include "Grid.h"
#include "Utils.h"
#include "Analyzer.h"
#include "Masks.h"
#include <iostream>
using namespace std;

TestDriver::TestDriver()
{
    for (int i = 0; i < 8; i++)
    {
        vector<char> temp;
        for (int j = 0; j < 8; j++)
        {
            temp.push_back(' ');
        }
        _board.push_back(temp);
    }
}

TestDriver::~TestDriver()
{
}

void TestDriver::PlayGame() 
{
    unsigned long long whitePieces = Grid::Row1 | Grid::Row2;
    unsigned long long blackPieces = Grid::Row7 | Grid::Row8;
    PlayerColor color = White;
    bool continuePlaying = true;
    /*while (continuePlaying = true)
    {*/
        Move move;
        clock_t timer;
        cout << "Generating Move..." << endl;
        timer = clock();

        move = Analyzer::GetMove(whitePieces, blackPieces, color);
        
        timer = clock() - timer;

        //Move Pieces
        if (color == White) 
        {
            int start = Masks::OrientationMasks::IndexOf[Masks::OrientationMasks::CoordinatesToUlong[move.xCoordinate - 1][move.yCoordinate - 1]];
            whitePieces &= ~Masks::OrientationMasks::CurrentSquare[start];
            if (move.direction == East) 
            {
                blackPieces &= ~Masks::WhiteMasks::EastAttack[start];
                whitePieces |= Masks::WhiteMasks::EastAttack[start];
            }
            else if (move.direction == West) 
            {
                blackPieces &= ~Masks::WhiteMasks::WestAttack[start];
                whitePieces |= Masks::WhiteMasks::WestAttack[start];
            }
            else
            {
                whitePieces |= Masks::WhiteMasks::Forward[start];
            }
        }
        else
        {
            int start = Masks::OrientationMasks::IndexOf[Masks::OrientationMasks::CoordinatesToUlong[move.xCoordinate - 1][move.yCoordinate - 1]];
            blackPieces &= ~Masks::OrientationMasks::CurrentSquare[start];
            if (move.direction == East)
            {
                whitePieces &= ~Masks::BlackMasks::EastAttack[start];
                blackPieces |= Masks::BlackMasks::EastAttack[start];
            }
            else if (move.direction == West)
            {
                whitePieces &= ~Masks::BlackMasks::WestAttack[start];
                blackPieces |= Masks::BlackMasks::WestAttack[start];
            }
            else
            {
                blackPieces |= Masks::BlackMasks::Forward[start];
            }
        }
        color = FlipColor(color);
        PrintBoard(whitePieces, blackPieces, timer);
    //}
}

void TestDriver::PrintBoard(unsigned long long whitePieces, unsigned long long blackPieces, clock_t t) 
{
    
    unsigned long long iterator = 1;
    for (int row = 7; row >= 0; row--)
    {
        for (int column = 7; column >= 0; column--)
        {
            if ((whitePieces & iterator) != 0)
            {
                _board[row][column] = 'W';
            }
            else if ((blackPieces & iterator) != 0)
            {
                _board[row][column] = 'B';
            }
            else
            {
                _board[row][column] = ' ';
            }
            iterator <<= 1;
        }
    }

    for (int row = 7; row >= 0; row--)
    {
        for (int column = 0; column <= 7; column++)
        {
            cout << _board[row][column];
            if (column != 7)
            {
                cout << "|";
            }
        }
        cout << endl;
        if (row != 0)
        {
            cout << "---------------" << endl;
        }
    }
    cout << "Move took " << ((float)t / CLOCKS_PER_SEC) << " seconds."<< endl;
    cout << "Ran " << Analyzer::GetTotalPlayOuts() << " simulations." << endl;
    //system("Pause");
}
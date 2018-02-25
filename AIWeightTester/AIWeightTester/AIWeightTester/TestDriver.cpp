#include "stdafx.h"
#include "TestDriver.h"
#include "Utils.h"
#include "Grid.h"
#include "Analyzer.h"
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

int TestDriver::_playGame(Weights whiteWeights, Weights blackWeights)
{
    BitBoard board;
    board.whitePieces = Grid::Row1 | Grid::Row2;
    board.blackPieces = Grid::Row7 | Grid::Row8;

    PlayerColor currentTurn = PlayerColor::White;
    int x = 0;

    while (!Analyzer::IsGameOver(board)) 
    {
        //Change weights.
        if (currentTurn == PlayerColor::White) 
        {
            Analyzer::SetWeights(whiteWeights);
        }
        else
        {
            Analyzer::SetWeights(blackWeights);
        }

        //Make Move.
        board = Analyzer::GetMove(board, currentTurn);

        //PrintBoard
        PrintBoard(board);

        //Change Turn.
        currentTurn = FlipColor(currentTurn);
    }

    return (currentTurn == PlayerColor::White) ? 1 : 0;
}

int TestDriver::TempPlayGame(Weights white, Weights black)
{
    return _playGame(white, black);
}

void TestDriver::PrintBoard(BitBoard board) 
{
    system("cls");

    unsigned long long iterator = 1;
    for (int row = 7; row >= 0; row--)
    {
        for (int column = 7; column >= 0; column--) 
        {
            if ((board.whitePieces & iterator) != 0) 
            {
                _board[row][column] = 'W';
            }
            else if ((board.blackPieces & iterator) != 0)
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
}

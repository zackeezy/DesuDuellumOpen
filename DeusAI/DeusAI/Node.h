#pragma once
#include "Utils.h"
#include<vector>
#include <iostream>
using namespace std;

class Node
{
private:
    static const int WIN = 100000000;
    static const int LOSS = -100000000;
    static const int PIECE_SCALAR = 10;
    static const int FURTHEST_ROW_BONUS = 2;

    unsigned long long _whitePieces;
    unsigned long long _blackPieces;

    Node * _parent;
    Node * _nextSibling;
    Node * _prevSibling;
    Node * _firstChild;

    int _wins;
    int _games;
    double _confidence;

    PlayerColor _nextToPlay;

    unsigned long long CombinedBoard();
   

public:
    Node(unsigned long long whitePieces, unsigned long long blackPieces, Node * parent);
    Node(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color);
    ~Node();

    void SetPrevSibling(Node * prevSibling);
    void GenerateConfidence();
    int GenerateChildren();
    PlayerColor GetNextToPlay();
    Node * GetFirstChild();
    Node * GetNextSibling();
    Node * GetParent();
    int GetConfidence();
    unsigned long long GetBlackPieces();
    unsigned long long GetWhitePieces();
    void AddWin();
    void AddLoss();
    int GetGames();
    int GetWins();
    void InitializeWins(int wins);
};


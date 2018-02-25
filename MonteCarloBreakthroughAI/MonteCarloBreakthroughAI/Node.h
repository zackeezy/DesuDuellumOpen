#pragma once
#include "Utils.h"

class Node
{
private:
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
    int GetConfidence();
    unsigned long long GetBlackPieces();
    unsigned long long GetWhitePieces();
};


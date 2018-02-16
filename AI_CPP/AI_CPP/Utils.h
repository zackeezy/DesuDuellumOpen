#pragma once
static enum PlayerColor 
{
    White = 0,
    Black = 1
};

class BitBoard
{
private: 

public:
    unsigned long long whitePieces;
    unsigned long long blackPieces;
    unsigned long long CombinedBoard();
    BitBoard();
    BitBoard(unsigned long long white, unsigned long long black);
    ~BitBoard();
};

class AlphaBetaNode 
{
private:

public: 
    BitBoard Child;
    BitBoard Parent;
    int value;
    AlphaBetaNode();
    AlphaBetaNode(BitBoard parent);
    ~AlphaBetaNode();
};

static PlayerColor FlipColor(PlayerColor color);


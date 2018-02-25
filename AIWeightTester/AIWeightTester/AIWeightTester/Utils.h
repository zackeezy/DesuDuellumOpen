#pragma once
enum PlayerColor
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
    PlayerColor currentTurn;
};

class AlphaBetaNode
{
private:

public:
    BitBoard Child;
    BitBoard Parent;
    int Value;
    AlphaBetaNode();
    AlphaBetaNode(BitBoard parent);
    ~AlphaBetaNode();
};

static PlayerColor FlipColor(PlayerColor color)
{
    if (color == PlayerColor::Black)
    {
        return PlayerColor::White;
    }

    return PlayerColor::Black;
}


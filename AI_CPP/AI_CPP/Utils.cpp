#include "stdafx.h"
#include "Utils.h"


BitBoard::BitBoard() 
{
    
}

BitBoard::BitBoard(unsigned long long white, unsigned long long black)
{
    whitePieces = white;
    blackPieces = black;
}

BitBoard::~BitBoard()
{

}

unsigned long long BitBoard::CombinedBoard()
{
    return whitePieces | blackPieces;
}

AlphaBetaNode::AlphaBetaNode() 
{

}

AlphaBetaNode::~AlphaBetaNode()
{

}

AlphaBetaNode::AlphaBetaNode(BitBoard parent)
{
    Parent = parent;
}

PlayerColor FlipColor(PlayerColor color) 
{
    if (color == PlayerColor::Black)
    {
        return PlayerColor::White;
    }

    return PlayerColor::Black;
}
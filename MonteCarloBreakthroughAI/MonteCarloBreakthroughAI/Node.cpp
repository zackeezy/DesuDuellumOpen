#include "stdafx.h"
#include "Node.h"
#include "Analyzer.h" 
#include "BitsMagic.h"
#include "Masks.h"
#include <cmath>

Node::Node(unsigned long long whitePieces, unsigned long long blackPieces, Node * parent)
{
    _whitePieces = whitePieces;
    _blackPieces = blackPieces;

    _parent = parent;

    _nextSibling = NULL;
    _prevSibling = NULL;
    _firstChild = NULL;

    _wins = 0;
    _games = 0;
    _confidence = 0;

    _nextToPlay = FlipColor(_parent->GetNextToPlay());
}

Node::Node(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color) 
{
    _whitePieces = whitePieces;
    _blackPieces = blackPieces;

    _parent = NULL;

    _nextSibling = NULL;
    _prevSibling = NULL;
    _firstChild = NULL;

    _wins = 0;
    _games = 0;
    _confidence = 0;

    _nextToPlay = color;
}


Node::~Node()
{
    if ( _firstChild != NULL)
    {
        _firstChild->~Node();
        if (_nextSibling != NULL) 
        {
            _nextSibling->~Node();
        }
    }
}

void Node::GenerateConfidence() 
{
    _confidence = 0;

    if (_games != 0) 
    {
        _confidence = _wins / _games + sqrt( 2 * log(_games) / _games);
    }
}

int Node::GenerateChildren() 
{
    if (_firstChild != NULL) 
    {
        return;
    }
    int childCount = 0;
    unsigned long long myBoard = _nextToPlay == PlayerColor::White ? _whitePieces : _blackPieces;
    int iterator = 0;
    do
    {
        iterator = BitsMagic::BitScanForwardWithReset(myBoard);
        if (iterator != -1)
        {
            if (_nextToPlay == PlayerColor::White)
            {
                unsigned long long forward = Masks::WhiteMasks::Forward[iterator];
                unsigned long long east = Masks::WhiteMasks::EastAttack[iterator];
                unsigned long long west = Masks::WhiteMasks::WestAttack[iterator];

                if (forward != 0 && (forward & CombinedBoard()) == 0)
                {   
                    childCount++;
                    if (_firstChild == NULL)
                    {
                        _firstChild = new Node((_whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | forward, 
                            _blackPieces, 
                            this);
                    }
                    else 
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL) 
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node((_whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | forward,
                            _blackPieces,
                            this);

                        temp->_nextSibling->SetPrevSibling(temp);
                    }
                }

                if (east != 0 && (east & _whitePieces) == 0)
                {
                    childCount++;
                    if (_firstChild == NULL)
                    {
                        _firstChild = new Node((_whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | east,
                            _blackPieces & ~east,
                            this);
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node((_whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | forward,
                            _blackPieces,
                            this);

                        temp->_nextSibling->SetPrevSibling(temp);
                    }
                }

                if (west != 0 && (west & _whitePieces) == 0)
                {
                    childCount++;
                    if (_firstChild == NULL)
                    {
                        _firstChild = new Node((_whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | west,
                            _blackPieces & ~west,
                            this);
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node((_whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | forward,
                            _blackPieces,
                            this);

                        temp->_nextSibling->SetPrevSibling(temp);
                    }
                }
            }
            else
            {
                unsigned long long forward = Masks::BlackMasks::Forward[iterator];
                unsigned long long east = Masks::BlackMasks::EastAttack[iterator];
                unsigned long long west = Masks::BlackMasks::WestAttack[iterator];

                if (forward != 0 && (forward & CombinedBoard()) == 0)
                {
                    childCount++;
                    if (_firstChild == NULL)
                    {
                        _firstChild = new Node((_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | forward,
                            _whitePieces,
                            this);
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node((_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | forward,
                            _whitePieces,
                            this);

                        temp->_nextSibling->SetPrevSibling(temp);
                    }
                }

                if (east != 0 && (east & _blackPieces) == 0)
                {
                    childCount++;
                    if (_firstChild == NULL)
                    {
                        _firstChild = new Node((_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | east,
                            _whitePieces & ~east,
                            this);
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node((_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | east,
                            _whitePieces & ~east,
                            this);

                        temp->_nextSibling->SetPrevSibling(temp);
                    }
                }

                if (west != 0 && (west & _blackPieces) == 0)
                {
                    childCount++;
                    if (_firstChild == NULL)
                    {
                        _firstChild = new Node((_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | west,
                            _whitePieces & ~west,
                            this);
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node((_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | west,
                            _whitePieces & ~west,
                            this);

                        temp->_nextSibling->SetPrevSibling(temp);
                    }
                }
            }
        }
    } while (iterator != -1);
    return childCount;
}

PlayerColor Node::GetNextToPlay()
{
    return _nextToPlay;
}

void Node::SetPrevSibling(Node * prevSibling) 
{
    if (_prevSibling == NULL) 
    {
        _prevSibling = prevSibling;
    }
}

unsigned long long Node::CombinedBoard() 
{
    return _whitePieces | _blackPieces;
}

Node * Node::GetFirstChild() 
{
    return _firstChild;
}

Node * Node::GetNextSibling() 
{
    return _nextSibling;
}

int Node::GetConfidence()
{
    return _confidence;
}

unsigned long long Node::GetBlackPieces() 
{
    return _blackPieces;
}

unsigned long long Node::GetWhitePieces() 
{
    return _whitePieces;
}
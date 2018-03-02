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
    _depth = parent->GetDepth() + 1;
    _confidence = 0;
   
    _nextToPlay = FlipColor(_parent->GetNextToPlay());
    GenerateScore();
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
    _depth = 0;
    _confidence = 0;

    _nextToPlay = color;
    GenerateScore();
}

Node::~Node()
{
    delete _firstChild;
    delete _nextSibling;
    /*if ( _firstChild != NULL)
    {
        _firstChild->~Node();
        if (_nextSibling != NULL) 
        {
            _nextSibling->~Node();
        }
    }*/
}

void Node::GenerateConfidence()
{
    // 1/sqrt(2) - 0.70710678118;

    _confidence = INT_MAX;

    if (_games != 0) 
    {
        _confidence = (_wins / _games) + (2  / sqrt(2) * sqrt( (2 * log(Analyzer::GetTotalPlayOuts())) / _games));
    }

   /* if (_firstChild != NULL) 
    {
        _firstChild->GenerateConfidence();
    }

    if (_nextSibling != NULL)
    {
        _nextSibling->GenerateConfidence();
    }*/
}

int Node::GenerateChildren() 
{
    if (_firstChild != NULL) 
    {
        return -1;
    }

    if (Analyzer::IsGameOver(this) != Neither) 
    {
        return 0;
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

                        if (Analyzer::IsGameOver(_firstChild) != Neither)
                        {
                            _firstChild->MarkAsNotSelectable();
                        }
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

                        if (Analyzer::IsGameOver(temp->_nextSibling) != Neither)
                        {
                            temp->_nextSibling->MarkAsNotSelectable();
                        }

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
                        if (Analyzer::IsGameOver(_firstChild) != Neither)
                        {
                            _firstChild->MarkAsNotSelectable();
                        }
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node((_whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | east,
                            _blackPieces,
                            this);
                        if (Analyzer::IsGameOver(temp->_nextSibling) != Neither)
                        {
                            temp->_nextSibling->MarkAsNotSelectable();
                        }

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
                        if (Analyzer::IsGameOver(_firstChild) != Neither)
                        {
                            _firstChild->MarkAsNotSelectable();
                        }
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node((_whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | west,
                            _blackPieces,
                            this);
                        if (Analyzer::IsGameOver(temp->_nextSibling) != Neither)
                        {
                            temp->_nextSibling->MarkAsNotSelectable();
                        }

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
                        _firstChild = new Node(_whitePieces,
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | forward,
                            this);
                        if (Analyzer::IsGameOver(_firstChild) != Neither)
                        {
                            _firstChild->MarkAsNotSelectable();
                        }
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node(_whitePieces, 
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | forward,
                            this);
                        if (Analyzer::IsGameOver(temp->_nextSibling) != Neither)
                        {
                            temp->_nextSibling->MarkAsNotSelectable();
                        }

                        temp->_nextSibling->SetPrevSibling(temp);
                    }
                }

                if (east != 0 && (east & _blackPieces) == 0)
                {
                    childCount++;
                    if (_firstChild == NULL)
                    {
                        _firstChild = new Node(_whitePieces & ~east,
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | east,
                            this);
                        if (Analyzer::IsGameOver(_firstChild) != Neither)
                        {
                            _firstChild->MarkAsNotSelectable();
                        }
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node(_whitePieces & ~east, 
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | east,
                            this);
                        if (Analyzer::IsGameOver(temp->_nextSibling) != Neither)
                        {
                            temp->_nextSibling->MarkAsNotSelectable();
                        }

                        temp->_nextSibling->SetPrevSibling(temp);
                    }
                }

                if (west != 0 && (west & _blackPieces) == 0)
                {
                    childCount++;
                    if (_firstChild == NULL)
                    {
                        _firstChild = new Node(_whitePieces & ~west, 
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | west,
                            this);
                        if (Analyzer::IsGameOver(_firstChild) != Neither)
                        {
                            _firstChild->MarkAsNotSelectable();
                        }
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node(_whitePieces & ~west,
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | west,
                            this);
                        if (Analyzer::IsGameOver(temp->_nextSibling) != Neither)
                        {
                            temp->_nextSibling->MarkAsNotSelectable();
                        }

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
    GenerateConfidence();
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

void Node::AddWin() 
{
    _wins++;
    _games++;
}

void Node::AddLoss() 
{
    _games++;
}

int Node::GetGames() 
{
    return _games;
}

Node * Node::GetParent() 
{
    return _parent;
}

int Node::GetWins() 
{
    return _wins;
}

bool Node::GetSelectable()
{
    return _selectable;
}

void Node::MarkAsNotSelectable()
{
    _selectable = false;
}

int Node::GetDepth()
{
    return _depth;
}

void Node::Print() 
{
    cout << "My depth was " << _depth << " and my winrate was " << _wins << "/" << _games << endl;
    if (_nextSibling != NULL) 
    {
        _nextSibling->Print();
    }
    if (_firstChild != NULL)
    {
        _firstChild->Print();
    }
}

Node * Node::GenerateChildren_Scored() 
{
    if (_firstChild != NULL)
    {
        return NULL;
    }

    if (Analyzer::IsGameOver(this) != Neither)
    {
        return NULL;
    }

    Node * bestChild;
    vector<Node *> bestChildren;
    int bestScore = INT_MIN;
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

                        if (_firstChild->GetScore() > bestScore) 
                        {
                            bestChildren.clear();
                            bestChildren.push_back(_firstChild);
                            bestScore = _firstChild->_score;
                        }
                        else if (_firstChild->GetScore() == bestScore)
                        {
                            bestChildren.push_back(_firstChild);
                        }
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

                        if (temp->_nextSibling->GetScore() > bestScore)
                        {
                            bestChildren.clear();
                            bestChildren.push_back(temp->_nextSibling);
                            bestScore = temp->_nextSibling->_score;
                        }
                        else if (temp->_nextSibling->GetScore() == bestScore)
                        {
                            bestChildren.push_back(temp->_nextSibling);
                        }
                        

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
                        if (_firstChild->GetScore() > bestScore)
                        {
                            bestChildren.clear();
                            bestChildren.push_back(_firstChild);
                            bestScore = _firstChild->_score;
                        }
                        else if (_firstChild->GetScore() == bestScore)
                        {
                            bestChildren.push_back(_firstChild);
                        }
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node((_whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | east,
                            _blackPieces,
                            this);
                        
                        if (temp->_nextSibling->GetScore() > bestScore)
                        {
                            bestChildren.clear();
                            bestChildren.push_back(temp->_nextSibling);
                            bestScore = temp->_nextSibling->_score;
                        }
                        else if (temp->_nextSibling->GetScore() == bestScore)
                        {
                            bestChildren.push_back(temp->_nextSibling);
                        }
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
                        if (_firstChild->GetScore() > bestScore)
                        {
                            bestChildren.clear();
                            bestChildren.push_back(_firstChild);
                            bestScore = _firstChild->_score;
                        }
                        else if (_firstChild->GetScore() == bestScore)
                        {
                            bestChildren.push_back(_firstChild);
                        }
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node((_whitePieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | west,
                            _blackPieces,
                            this);
                        
                        if (temp->_nextSibling->GetScore() > bestScore)
                        {
                            bestChildren.clear();
                            bestChildren.push_back(temp->_nextSibling);
                            bestScore = temp->_nextSibling->_score;
                        }
                        else if (temp->_nextSibling->GetScore() == bestScore)
                        {
                            bestChildren.push_back(temp->_nextSibling);
                        }
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
                        _firstChild = new Node(_whitePieces,
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | forward,
                            this);
                        if (_firstChild->GetScore() > bestScore)
                        {
                            bestChildren.clear();
                            bestChildren.push_back(_firstChild);
                            bestScore = _firstChild->_score;
                        }
                        else if (_firstChild->GetScore() == bestScore)
                        {
                            bestChildren.push_back(_firstChild);
                        }
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node(_whitePieces,
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | forward,
                            this);
                        
                        if (temp->_nextSibling->GetScore() > bestScore)
                        {
                            bestChildren.clear();
                            bestChildren.push_back(temp->_nextSibling);
                            bestScore = temp->_nextSibling->_score;
                        }
                        else if (temp->_nextSibling->GetScore() == bestScore)
                        {
                            bestChildren.push_back(temp->_nextSibling);
                        }
                        temp->_nextSibling->SetPrevSibling(temp);
                    }
                }

                if (east != 0 && (east & _blackPieces) == 0)
                {
                    childCount++;
                    if (_firstChild == NULL)
                    {
                        _firstChild = new Node(_whitePieces & ~east,
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | east,
                            this);
                        if (_firstChild->GetScore() > bestScore)
                        {
                            bestChildren.clear();
                            bestChildren.push_back(_firstChild);
                            bestScore = _firstChild->_score;
                        }
                        else if (_firstChild->GetScore() == bestScore)
                        {
                            bestChildren.push_back(_firstChild);
                        }
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node(_whitePieces & ~east,
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | east,
                            this);
                        
                        if (temp->_nextSibling->GetScore() > bestScore)
                        {
                            bestChildren.clear();
                            bestChildren.push_back(temp->_nextSibling);
                            bestScore = temp->_nextSibling->_score;
                        }
                        else if (temp->_nextSibling->GetScore() == bestScore)
                        {
                            bestChildren.push_back(temp->_nextSibling);
                        }
                        temp->_nextSibling->SetPrevSibling(temp);
                    }
                }

                if (west != 0 && (west & _blackPieces) == 0)
                {
                    childCount++;
                    if (_firstChild == NULL)
                    {
                        _firstChild = new Node(_whitePieces & ~west,
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | west,
                            this);
                        if (_firstChild->GetScore() > bestScore)
                        {
                            bestChildren.clear();
                            bestChildren.push_back(_firstChild);
                            bestScore = _firstChild->_score;
                        }
                        else if (_firstChild->GetScore() == bestScore)
                        {
                            bestChildren.push_back(_firstChild);
                        }
                    }
                    else
                    {
                        Node * temp = _firstChild;
                        while (temp->_nextSibling != NULL)
                        {
                            temp = temp->_nextSibling;
                        }

                        temp->_nextSibling = new Node(_whitePieces & ~west,
                            (_blackPieces & ~Masks::OrientationMasks::CurrentSquare[iterator]) | west,
                            this);
                        
                        if (temp->_nextSibling->GetScore() > bestScore)
                        {
                            bestChildren.clear();
                            bestChildren.push_back(temp->_nextSibling);
                            bestScore = temp->_nextSibling->_score;
                        }
                        else if (temp->_nextSibling->GetScore() == bestScore)
                        {
                            bestChildren.push_back(temp->_nextSibling);
                        }
                        temp->_nextSibling->SetPrevSibling(temp);
                    }
                }
            }
        }
    } while (iterator != -1);

    random_device random;
    mt19937 mt(random());
    uniform_int_distribution<int> distribution(0, bestChildren.size() - 1);
    bestChild = bestChildren[distribution(mt)];

    return bestChild;
}

void Node::GenerateScore() 
{
    PlayerColor winner = Analyzer::IsGameOver(this);

    if (winner != Neither) 
    {
        if (FlipColor(_nextToPlay) == winner)
        {
            _score = WIN;
        }
        else 
        {
            _score = LOSS;
        }
    }
    else
    { 
        unsigned long long whitePieces = _whitePieces;
        int iterator = BitsMagic::BitScanForwardWithReset(whitePieces);
        int whiteCount = 0;
        int whiteScore = 0;
        int whiteFurthestRow = 0;
        while (iterator >= 0) 
        {
            whiteCount++;
            /*if (Masks::OrientationMasks::CurrentRow[iterator] > whiteFurthestRow) 
            {
                whiteFurthestRow = Masks::OrientationMasks::CurrentRow[iterator];
            }*/
            whiteScore += FURTHEST_ROW_BONUS * Masks::OrientationMasks::CurrentRow[iterator];
            iterator = BitsMagic::BitScanForwardWithReset(whitePieces);
        }
        //efMS - Each piece receives a score of 10
        //Player with further row gets 2.5;
        whiteScore += (whiteCount * PIECE_SCALAR);
        
        unsigned long long blackPieces = _blackPieces;
        iterator = BitsMagic::BitScanForwardWithReset(blackPieces);
        int blackCount = 0;
        int blackScore = 0;
        int blackFurthestRow = 9;
        while (iterator >= 0)
        {
            blackCount++;
            /*if (Masks::OrientationMasks::CurrentRow[iterator] < blackFurthestRow)
            {
                blackFurthestRow = Masks::OrientationMasks::CurrentRow[iterator];
            }*/
            blackScore += FURTHEST_ROW_BONUS * (9 - Masks::OrientationMasks::CurrentRow[iterator]);
            iterator = BitsMagic::BitScanForwardWithReset(blackPieces);
        }
        blackScore += (blackCount * PIECE_SCALAR);
        blackFurthestRow = 9 - blackFurthestRow;

        /*if (whiteFurthestRow > blackFurthestRow)
        {
            whiteFurthestRow += FURTHEST_ROW_BONUS;
        }
        else if (whiteFurthestRow < blackFurthestRow)
        {
            blackFurthestRow += FURTHEST_ROW_BONUS;
        }*/

        if (FlipColor(_nextToPlay) == White) 
        {
            _score = whiteScore - blackScore;
        }
        else
        {
            _score = blackScore - whiteScore;
        }
    }
}

int Node::GetScore() 
{
    return _score;
}
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

void Node::InitializeWins(int wins) 
{
    _wins = wins;
    _games = 100;
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

                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[forward]];
                                switch (row) 
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            /*else if (//Captured a piece) 
                            {
                                initialWins = 60;
                            }*/
                            else  
                            {
                                initialWins = 30;
                            }
                        }

                        _firstChild->InitializeWins(initialWins);
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

                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[forward]];
                                switch (row)
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            /*else if (//Captured a piece)
                            {
                            initialWins = 60;
                            }*/
                            else
                            {
                                initialWins = 30;
                            }
                        }

                        temp->_nextSibling->InitializeWins(initialWins);

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

                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[east]];
                                switch (row)
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            else if ((east & _blackPieces) != 0)
                            {
                                initialWins = 60;
                            }
                            else
                            {
                                initialWins = 30;
                            }
                        }

                        _firstChild->InitializeWins(initialWins);
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

                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[east]];
                                switch (row)
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            else if ((east & _blackPieces) != 0)
                            {
                                initialWins = 60;
                            }
                            else
                            {
                                initialWins = 30;
                            }
                        }

                        temp->_nextSibling->InitializeWins(initialWins);

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

                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[west]];
                                switch (row)
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            else if ((west & _blackPieces) != 0)
                            {
                                initialWins = 60;
                            }
                            else
                            {
                                initialWins = 30;
                            }
                        }

                        _firstChild->InitializeWins(initialWins);
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

                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[west]];
                                switch (row)
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            else if ((west & _blackPieces) != 0)
                            {
                                initialWins = 60;
                            }
                            else
                            {
                                initialWins = 30;
                            }
                        }

                        temp->_nextSibling->InitializeWins(initialWins);

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

                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[forward]];
                                switch (row)
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            /*else if ((forward & _blackPieces) != 0)
                            {
                                initialWins = 60;
                            }*/
                            else
                            {
                                initialWins = 30;
                            }
                        }

                        _firstChild->InitializeWins(initialWins);
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

                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[forward]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[forward]];
                                switch (row)
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            /*else if ((forward & _blackPieces) != 0)
                            {
                            initialWins = 60;
                            }*/
                            else
                            {
                                initialWins = 30;
                            }
                        }

                        temp->_nextSibling->InitializeWins(initialWins);

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
                       
                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[east]];
                                switch (row)
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            else if ((east & _whitePieces) != 0)
                            {
                                initialWins = 60;
                            }
                            else
                            {
                                initialWins = 30;
                            }
                        }

                        _firstChild->InitializeWins(initialWins);
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
                        
                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[east]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[east]];
                                switch (row)
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            else if ((east & _whitePieces) != 0)
                            {
                                initialWins = 60;
                            }
                            else
                            {
                                initialWins = 30;
                            }
                        }

                        temp->_nextSibling->InitializeWins(initialWins);

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


                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((_firstChild->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[west]];
                                switch (row)
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            else if ((west & _whitePieces) != 0)
                            {
                                initialWins = 60;
                            }
                            else
                            {
                                initialWins = 30;
                            }
                        }

                        _firstChild->InitializeWins(initialWins);

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
                        
                        int initialWins = 0;
                        //Safety Bonus
                        {
                            int attackCount = 0;
                            int protectCount = 0;


                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::EastAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetBlackPieces() & Masks::WhiteMasks::WestAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::EastAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                attackCount++;
                            }

                            if ((temp->_nextSibling->GetWhitePieces() & Masks::BlackMasks::WestAttack[Masks::OrientationMasks::IndexOf[west]]) != 0)
                            {
                                attackCount++;
                            }

                            if (protectCount >= attackCount)
                            {
                                int row = Masks::OrientationMasks::CurrentRow[Masks::OrientationMasks::IndexOf[west]];
                                switch (row)
                                {
                                case 8:
                                    initialWins = 100;
                                    break;
                                case 7:
                                    initialWins = 95;
                                    break;
                                case 6:
                                    initialWins = 85;
                                    break;
                                case 5:
                                    initialWins = 75;
                                    break;
                                case 4:
                                    initialWins = 60;
                                    break;
                                default:
                                    initialWins = 30;
                                    break;
                                }
                            }
                            else if ((west & _whitePieces) != 0)
                            {
                                initialWins = 60;
                            }
                            else
                            {
                                initialWins = 30;
                            }
                        }

                        temp->_nextSibling->InitializeWins(initialWins);

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


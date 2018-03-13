#include "stdafx.h"
#include "Analyzer.h"
#include "Masks.h"
#include "BitsMagic.h"
#include "Grid.h"
#include "Utils.h"
#include <ctime>
#include <climits>
#include <iostream>
#include <omp.h>
using namespace std;

const int Analyzer::WhiteScoreArray[64] =
{
    36, 36, 36, 36, 36, 36, 36, 36, 
    20, 28, 28, 28, 28, 28, 28, 20,
    16, 21, 21, 21, 21, 21, 21, 16, 
    11, 15, 15, 15, 15, 15, 15, 11,
    7, 10, 10, 10, 10, 10, 10, 7,
    4, 6, 6, 6, 6, 6, 6, 4,
    2, 3, 3, 3, 3, 3, 3, 2,
    5, 15, 15, 5, 5, 15, 15, 5,
};

const int Analyzer::BlackScoreArray[64] =
{
    5, 15, 15, 5, 5, 15, 15, 5,
    2, 3, 3, 3, 3, 3, 3, 2,
    4, 6, 6, 6, 6, 6, 6, 4,
    7, 10, 10, 10, 10, 10, 10, 7,
    11, 15, 15, 15, 15, 15, 15, 11,
    16, 21, 21, 21, 21, 21, 21, 16,
    20, 28, 28, 28, 28, 28, 28, 20,
    36, 36, 36, 36, 36, 36, 36, 36,
};

Analyzer::Analyzer()
{
}

Analyzer::~Analyzer()
{
}

int Analyzer::_totalPlayOuts = 0;

int Analyzer::GetTotalPlayOuts() 
{
    return _totalPlayOuts;
}

Move TranslateChildToMove(Node * parent, Node * child) 
{
    if (parent->GetNextToPlay() == PlayerColor::White)
    {
        unsigned long long movedPiece = parent->GetWhitePieces() ^ child->GetWhitePieces();
        int destination = BitsMagic::BitScanForwardWithReset(movedPiece);
        int start = BitsMagic::BitScanForwardWithReset(movedPiece);
        Move move;
        move.xCoordinate = Masks::OrientationMasks::CurrentColumn[start];;
        move.yCoordinate = Masks::OrientationMasks::CurrentRow[start];
        if ((Masks::WhiteMasks::EastAttack[start] & Masks::OrientationMasks::CurrentSquare[destination]) != 0)
        {
            move.direction = East;
        }
        else if ((Masks::WhiteMasks::Forward[start] & Masks::OrientationMasks::CurrentSquare[destination]) != 0)
        {
            move.direction = Forward;
        }
        else if ((Masks::WhiteMasks::WestAttack[start] & Masks::OrientationMasks::CurrentSquare[destination]) != 0)
        {
            move.direction = West;
        }
        return move;
    }
    else
    {
        unsigned long long movedPiece = parent->GetBlackPieces() ^ child->GetBlackPieces();
        int start = BitsMagic::BitScanForwardWithReset(movedPiece);
        int destination = BitsMagic::BitScanForwardWithReset(movedPiece);
        Move move;
        move.xCoordinate = Masks::OrientationMasks::CurrentColumn[start];;
        move.yCoordinate = Masks::OrientationMasks::CurrentRow[start];
        if ((Masks::BlackMasks::EastAttack[start] & Masks::OrientationMasks::CurrentSquare[destination]) != 0)
        {
            move.direction = East;
        }
        else if ((Masks::BlackMasks::Forward[start] & Masks::OrientationMasks::CurrentSquare[destination]) != 0)
        {
            move.direction = Forward;
        }
        else if ((Masks::BlackMasks::WestAttack[start] & Masks::OrientationMasks::CurrentSquare[destination]) != 0)
        {
            move.direction = West;
        }
        return move;
    }
}

Move Analyzer::GetMove_Singleton(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color) 
{
    Move bestMove;
    Node * root = new Node(whitePieces, blackPieces, color);
    
    _totalPlayOuts = 0;

    clock_t startTime = clock();
    while ((((float)(clock() - startTime))) < MAX_TIME)
    {
        RunPlayOut_Singleton(root);
    }

    Node* children = root->GetFirstChild();
    Node* bestChild = children;

    while (children != NULL) 
    {
        if (children->GetGames() > bestChild->GetGames())
        {
            bestChild = children;
        }

        children = children->GetNextSibling();
    }
    Move move = TranslateChildToMove(root, bestChild);
    
    delete root;
    return move;
}

Move Analyzer::GetMove_LeafParallel(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color)
{
    Move bestMove;
    Node * root = new Node(whitePieces, blackPieces, color);

    _totalPlayOuts = 0;

    clock_t startTime = clock();
    while ((((float)(clock() - startTime))) < MAX_TIME)
    {
        //cout << "Start RunPlayOut" << endl;
        RunPlayOut_Leaf_Parallel(root);
        //cout << "End RunPlayOut" << endl;
    }

    Node* children = root->GetFirstChild();
    Node* bestChild = children;

    //cout << "Start best move selection." << endl;
    while (children != NULL)
    {
        //cout << "My depth was " << children->GetDepth() << " and my winrate was " << children->GetWins() << "/" << children->GetGames() << endl;
        if (children->GetGames() > bestChild->GetGames())
        {
            bestChild = children;
        }

        children = children->GetNextSibling();
    }
    //cout << "End best move selection." << endl;

    //cout << "Start TranslateChildToMove" << endl;
    Move move = TranslateChildToMove(root, bestChild);
    //cout << "End TranslateChildToMove" << endl;

    //root->Print();
    delete root;
    return move;
}

Node * GetSelectableChildWithBestConfidence(Node * currentNode) 
{
    if (currentNode->GetFirstChild() == NULL) 
    {
        return currentNode;
    }

    Node * children = currentNode->GetFirstChild();

    int bestConfidence = INT_MIN;
    Node * bestChild = NULL;

    vector<Node *> bestOptions;

    while (children != NULL) 
    {
        if (children->GetConfidence() > bestConfidence)
        {
            bestOptions.clear();
            bestOptions.push_back(children);
            bestConfidence = children->GetConfidence();
        }
        else if (children->GetConfidence() == bestConfidence) 
        {
            bestOptions.push_back(children);
            bestConfidence = children->GetConfidence();
        }
        children = children->GetNextSibling();
    }

    random_device random;
    mt19937 mt(random());

    uniform_int_distribution<int> distribution(0, bestOptions.size() - 1);

    return bestOptions[distribution(mt)];
}

Node * TraverseForNextMove(Node * root)
{
    /*if (!root->GetSelectable())
    {
        return NULL;
    }*/

    if (root->GetFirstChild() == NULL)
    {
        return root;
    }

    Node * bestCurrentChild = GetSelectableChildWithBestConfidence(root);

    if (bestCurrentChild == NULL)
    {
        root->MarkAsNotSelectable();
        return NULL;
    }

    else if (bestCurrentChild->GetFirstChild() == NULL) 
    {
        return bestCurrentChild;
    }

    bool continueLoop = true;
    while (continueLoop) 
    {
        Node * temp = GetSelectableChildWithBestConfidence(bestCurrentChild);

        if (temp->GetFirstChild() == NULL) 
        {
            bestCurrentChild = temp;
            continueLoop = false;
        }

        else 
        {
            bestCurrentChild = temp;
        }
    }

    return bestCurrentChild;
}

PlayerColor Analyzer::IsGameOver(Node * node)
{
   
    if (node->GetWhitePieces() == 0)
    {
        return PlayerColor::Black;
    }
    else if (node->GetBlackPieces() == 0)
    {
        return PlayerColor::White;
    }

    if ((node->GetWhitePieces() & Grid::Row8) != 0)
    {
        return PlayerColor::White;
    }
    else if ((node->GetBlackPieces() & Grid::Row1) != 0)
    {
        return PlayerColor::Black;
    }
    else 
    {
        return PlayerColor::Neither;
    }
}

PlayerColor FinishPlayOut_Light(Node * initialState) 
{
    Node * currentBoard = new Node(initialState->GetWhitePieces(), initialState->GetBlackPieces(), initialState->GetNextToPlay());
    Node * root = currentBoard;
    random_device random;
    mt19937 mt(random());

    PlayerColor gameOver = Analyzer::IsGameOver(currentBoard);
    while (gameOver == PlayerColor::Neither) 
    {
        int totalKids = currentBoard->GenerateChildren();
        uniform_int_distribution<int> distribution(0, totalKids - 1);
        int selectedChild = distribution(mt);
        
        currentBoard = currentBoard->GetFirstChild();
        for (int i = 0; i < selectedChild; i++) 
        {
            currentBoard = currentBoard->GetNextSibling();
        }

        gameOver = Analyzer::IsGameOver(currentBoard);
    }

    delete root;
    return FlipColor(gameOver);
}

PlayerColor Analyzer::ScoreGame(Node * node)
{
    PlayerColor winner = Neither;

    unsigned long long whitePieces = node->GetWhitePieces();
    int iterator = BitsMagic::BitScanForwardWithReset(whitePieces);
    int whiteScore = 0;
    int whiteCount = 0;
    while (iterator >= 0)
    {
        //Piece Existence Bonus
        whiteCount++;

        //Board Value Bonus
        whiteScore += WhiteScoreArray[iterator];

        //Safety Bonus
        {
            int attackCount = 0;
            int protectCount = 0;


            if ((node->GetWhitePieces() & Masks::BlackMasks::EastAttack[iterator]) != 0)
            {
                protectCount++;
            }

            if ((node->GetWhitePieces() & Masks::BlackMasks::WestAttack[iterator]) != 0)
            {
                protectCount++;
            }

            if ((node->GetBlackPieces() & Masks::WhiteMasks::EastAttack[iterator]) != 0)
            {
                attackCount++;
            }

            if ((node->GetBlackPieces() & Masks::WhiteMasks::WestAttack[iterator]) != 0)
            {
                attackCount++;
            }

            if (protectCount >= attackCount)
            {
                whiteScore += (WhiteScoreArray[iterator] / 2);
            }
        }

        //Breakthrough Bonus
        //I honestly don't know if I want to do this one...
        {
            //int breakthroughBonus = 6;
            //int forward = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::Forward[iterator]];
            //int forward2 = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::Forward[forward]];
            //int east = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::EastAttack[iterator]];
            //int east2 = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::Forward[east]];
            //int west = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::WestAttack[iterator]];
            //int west2 = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::Forward[west]];

            ////for each location, if the location has a black piece on it, subtract one from breakthrough bonus.
            //if ([]) 
            //{

            //}
        }

        iterator = BitsMagic::BitScanForwardWithReset(whitePieces);
    }

    unsigned long long blackPieces = node->GetBlackPieces();
    iterator = BitsMagic::BitScanForwardWithReset(blackPieces);
    int blackScore = 0;
    int blackCount = 0;
    while (iterator >= 0)
    {
        //Piece Existence Bonus
        blackCount++;

        //Board Value Bonus
        blackScore += BlackScoreArray[iterator];

        //Safety Bonus
        {
            int attackCount = 0;
            int protectCount = 0;

            if ((node->GetBlackPieces() & Masks::WhiteMasks::EastAttack[iterator]) != 0)
            {
                protectCount++;
            }

            if ((node->GetBlackPieces() & Masks::WhiteMasks::WestAttack[iterator]) != 0)
            {
                protectCount++;
            }

            if ((node->GetWhitePieces() & Masks::BlackMasks::EastAttack[iterator]) != 0)
            {
                attackCount++;
            }

            if ((node->GetWhitePieces() & Masks::BlackMasks::WestAttack[iterator]) != 0)
            {
                attackCount++;
            }

            if (protectCount >= attackCount)
            {
                blackScore += (BlackScoreArray[iterator] / 2);
            }
        }

        //Breakthrough Bonus
        //I honestly don't know if I want to do this one...
        {
            //int breakthroughBonus = 6;
            //int forward = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::Forward[iterator]];
            //int forward2 = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::Forward[forward]];
            //int east = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::EastAttack[iterator]];
            //int east2 = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::Forward[east]];
            //int west = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::WestAttack[iterator]];
            //int west2 = Masks::OrientationMasks::IndexOf[Masks::WhiteMasks::Forward[west]];

            ////for each location, if the location has a black piece on it, subtract one from breakthrough bonus.
            //if ([]) 
            //{

            //}
        }

        iterator = BitsMagic::BitScanForwardWithReset(blackPieces);
    }
    
    if (whiteCount > blackCount) 
    {
        whiteScore += (whiteCount - blackCount) * PIECE_SCALAR;
    }
    else if (blackCount > whiteCount)
    {
        blackScore += (blackCount - whiteCount) * PIECE_SCALAR;
    }

    if (FlipColor(node->GetNextToPlay()) == White)
    {
        if (whiteScore >= blackScore) 
        {
            winner = White;
        }
        else 
        {
            winner = Black;
        }
    }
    else
    {
        if (blackScore >= whiteScore)
        {
            winner = Black;
        }
        else
        {
            winner = White;
        }
    }
    
    return winner;
}

PlayerColor FinishPlayOut_Heavy(Node * initialState)
{
    Node * currentBoard = new Node(initialState->GetWhitePieces(), initialState->GetBlackPieces(), initialState->GetNextToPlay());
    Node * root = currentBoard;
    random_device random;
    mt19937 mt(random());

    PlayerColor gameOver = Analyzer::IsGameOver(currentBoard);
    int turnCounter = 0;
    while (gameOver == PlayerColor::Neither && turnCounter < 5)
    {
        int totalKids = currentBoard->GenerateChildren();
        uniform_int_distribution<int> distribution(0, totalKids - 1);
        int selectedChild = distribution(mt);
        
        currentBoard = currentBoard->GetFirstChild();
        for (int i = 0; i < selectedChild; i++)
        {
            currentBoard = currentBoard->GetNextSibling();
        }

        gameOver = Analyzer::IsGameOver(currentBoard);
        turnCounter++;
    }

    if (gameOver == Neither) 
    {
        gameOver = Analyzer::ScoreGame(currentBoard);
    }

    delete root;
    return FlipColor(gameOver);
}

//Now runs with leaf parallelization of factor 8.
void Analyzer::RunPlayOut_Singleton(Node * root) 
{

    Node * bestNode;
    bestNode = TraverseForNextMove(root);

    if (bestNode == NULL) 
    {
        return;
    }

    PlayerColor winner;

    int childrenCount = bestNode->GenerateChildren();
    Node * firstMove;
    if (childrenCount == 0) 
    {
        firstMove = bestNode;
        winner = IsGameOver(firstMove);

        _totalPlayOuts++;
        while (firstMove != NULL)
        {
            if (firstMove->GetNextToPlay() == winner)
            {
                firstMove->AddWin();
            }
            else
            {
                firstMove->AddLoss();
            }
            firstMove = firstMove->GetParent();
        }
    }
    else
    {
        firstMove = bestNode->GetFirstChild();

        PlayerColor winner = FinishPlayOut_Heavy(firstMove);
        

        _totalPlayOuts++;

        while (firstMove != NULL)
        {
            if (winner == firstMove->GetNextToPlay())
            {
                firstMove->AddWin();
            }
            else
            {
                firstMove->AddLoss();
            }
            firstMove = firstMove->GetParent();
        }
    }
}

//Now runs with leaf parallelization of factor 8.
//Now terminates simulations at 4 turns deep.
//Now scores games using 3/4 of Lorentz's algorithm.
void Analyzer::RunPlayOut_Leaf_Parallel(Node * root)
{
    Node * bestNode;
    bestNode = TraverseForNextMove(root);

    if (bestNode == NULL)
    {
        return;
    }

    PlayerColor winner;

    int childrenCount = bestNode->GenerateChildren();
    Node * firstMove;
    if (childrenCount == 0)
    {
        firstMove = bestNode;
        winner = IsGameOver(firstMove);

        _totalPlayOuts++;
        while (firstMove != NULL)
        {
            if (firstMove->GetNextToPlay() == winner)
            {
                firstMove->AddWin();
            }
            else
            {
                firstMove->AddLoss();
            }
            firstMove = firstMove->GetParent();
        }
    }
    else
    {
        firstMove = bestNode->GetFirstChild();

        int threadCount = 8;
        omp_set_dynamic(0);
        omp_set_num_threads(threadCount);
        PlayerColor colors[8];

    #pragma omp parallel for
        for (int i = 0; i < threadCount; i++)
        {
            colors[i] = FinishPlayOut_Heavy(firstMove);
        }

        _totalPlayOuts += threadCount;

        while (firstMove != NULL)
        {
            for (int i = 0; i < threadCount; i++)
            {
                if (colors[i] == firstMove->GetNextToPlay())
                {
                    firstMove->AddWin();
                }
                else
                {
                    firstMove->AddLoss();
                }
            }

            firstMove = firstMove->GetParent();
        }
    }
}


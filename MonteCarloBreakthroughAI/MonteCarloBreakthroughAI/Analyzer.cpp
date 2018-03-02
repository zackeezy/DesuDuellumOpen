#include "stdafx.h"
#include "Analyzer.h"
#include "Masks.h"
#include "BitsMagic.h"
#include "Grid.h"
#include "Utils.h"
#include <ctime>
#include <climits>
#include <iostream>
using namespace std;


Analyzer::Analyzer()
{
}

Analyzer::~Analyzer()
{
}

int Analyzer::_totalPlayOuts = 0;
vector<int> Analyzer::_gamesAtDepth;

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

Move Analyzer::GetMove_Light(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color) 
{
    _gamesAtDepth.clear();

    Move bestMove;
    Node * root = new Node(whitePieces, blackPieces, color);
    
    _totalPlayOuts = 0;

    clock_t startTime = clock();
    while ((((float)(clock() - startTime))) < MAX_TIME)
    {
        //cout << "Start RunPlayOut" << endl;
        RunPlayOut_Light(root);
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

Move Analyzer::GetMove_Heavy(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color)
{
    _gamesAtDepth.clear();

    Move bestMove;
    Node * root = new Node(whitePieces, blackPieces, color);

    _totalPlayOuts = 0;

    clock_t startTime = clock();
    while ((((float)(clock() - startTime))) < MAX_TIME)
    {
        //cout << "Start RunPlayOut" << endl;
        RunPlayOut_Heavy(root);
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

//Case 1: currentNode is unselectable
    //return NULL
//Case 2: currentNode is leaf
    //return currentNode
//Case 3: currentNode is not leaf
    //return immediate best confidence selectable child
Node * GetSelectableChildWithBestConfidence(Node * currentNode) 
{
    /*if (!currentNode->GetSelectable()) 
    {
        return NULL;
    }*/

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
        /*if (children->GetSelectable())
        {*/
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
        //}
        children = children->GetNextSibling();
    }

    random_device random;
    mt19937 mt(random());

    uniform_int_distribution<int> distribution(0, bestOptions.size() - 1);

    return bestOptions[distribution(mt)];
}

//Case 1: root is unSelectable
    //return NULL
//Case 2: root is leaf
    //return root
//Case 3: root is not leaf
    //loop for a leaf, return it.
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

        /*if (temp == NULL) 
        {
            bestCurrentChild->MarkAsNotSelectable();
            bestCurrentChild = root;
        }*/

        /*else*/ if (temp->GetFirstChild() == NULL) 
        {
            bestCurrentChild = temp;
            continueLoop = false;
        }

        /*else if (!root->GetSelectable()) 
        {
            return NULL;
        }*/

        else 
        {
            bestCurrentChild = temp;
        }
    }

    return bestCurrentChild;
}

PlayerColor Analyzer::IsGameOver(Node * node)
{
    unsigned long long  myWhitePieces = node->GetWhitePieces();
    unsigned long long  myBlackPieces = node->GetBlackPieces();

    if (myWhitePieces == 0)
    {
        return PlayerColor::Black;
    }
    else if (myBlackPieces == 0)
    {
        return PlayerColor::White;
    }

    int piece = BitsMagic::BitScanForwardWithReset(myWhitePieces);
    while (piece >= 0)
    {
        if ((Masks::OrientationMasks::CurrentSquare[piece] & Grid::Row8) != 0)
        {
            return PlayerColor::White;
        }
        piece = BitsMagic::BitScanForwardWithReset(myWhitePieces);
    }

    piece = BitsMagic::BitScanForwardWithReset(myBlackPieces);
    while (piece >= 0)
    {
        if ((Masks::OrientationMasks::CurrentSquare[piece] & Grid::Row1) != 0)
        {
            return PlayerColor::Black;
        }
        piece = BitsMagic::BitScanForwardWithReset(myBlackPieces);
    }

    return PlayerColor::Neither;
}

PlayerColor FinishPlayOut_Light(Node * initialState) 
{
   /* if (initialState == NULL) 
    {
        cout << "Initial State is NULL" << endl;
    }*/
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
        //cout << "Selected child " << selectedChild << " out of " << totalKids << endl;
        Node * root = currentBoard;
        currentBoard = currentBoard->GetFirstChild();
        for (int i = 0; i < selectedChild; i++) 
        {
            currentBoard = currentBoard->GetNextSibling();
        }

        //currentBoard = new Node(currentBoard->GetWhitePieces(), currentBoard->GetBlackPieces(), currentBoard->GetNextToPlay());

        gameOver = Analyzer::IsGameOver(currentBoard);
    }

    delete root;
    return FlipColor(gameOver);
}

PlayerColor FinishPlayOut_Heavy(Node * initialState)
{
    /* if (initialState == NULL)
    {
    cout << "Initial State is NULL" << endl;
    }*/
    Node * currentBoard = new Node(initialState->GetWhitePieces(), initialState->GetBlackPieces(), initialState->GetNextToPlay());
    Node * root = currentBoard;
    random_device random;
    mt19937 mt(random());

    PlayerColor gameOver = Analyzer::IsGameOver(currentBoard);
    while (gameOver == PlayerColor::Neither)
    {
        currentBoard = currentBoard->GenerateChildren_Scored();
        gameOver = Analyzer::IsGameOver(currentBoard);
    }

    delete root;
    return FlipColor(gameOver);
}

void Analyzer::RunPlayOut_Light(Node * root) 
{
    //Traverse to Best Confidence Selectable Leaf
    //Generate Chldren For that Leaf.
    //Finish Playout On BCSL->FirstChild()
    //BackPropagate

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
    }
    else
    {
        firstMove = bestNode->GetFirstChild();
        winner = FinishPlayOut_Light(firstMove);
    }
    
    //int depth = firstMove->GetDepth();
    
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

    //root->GenerateConfidence();
}

void Analyzer::RunPlayOut_Heavy(Node * root)
{
    //Traverse to Best Confidence Selectable Leaf
    //Generate Chldren For that Leaf.
    //Finish Playout On BCSL->FirstChild()
    //BackPropagate

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
    }
    else
    {
        firstMove = bestNode->GetFirstChild();
        winner = FinishPlayOut_Heavy(firstMove);
    }

    //int depth = firstMove->GetDepth();

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

    //root->GenerateConfidence();
}


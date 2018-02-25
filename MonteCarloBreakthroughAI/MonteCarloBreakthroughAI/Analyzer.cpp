#include "stdafx.h"
#include "Analyzer.h"
#include "Masks.h"
#include "BitsMagic.h"
#include "Grid.h"
#include <ctime>
#include <climits>


Analyzer::Analyzer()
{
}


Analyzer::~Analyzer()
{
}

int Analyzer::GetTotalPlayOuts() 
{
    return _totalPlayOuts;
}

Move Analyzer::GetMove(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color) 
{
    Move bestMove;
    Node * root = new Node(whitePieces, blackPieces, color);
    
    clock_t startTime = clock();
    while ((((float)(clock() - startTime)) / CLOCKS_PER_SEC) < MAX_TIME)
    {
        RunPlayOut(root);
    }

    //bestMove is the the move with the most simulations ran.
    return bestMove;
}

void TraverseForNextMove(Node * currentNode, Node * & bestNode)
{
    if (currentNode == NULL) 
    {
        return;
    }

    int bestConfidence = INT_MIN;

    Node * iterator = currentNode;
    while(iterator != NULL)
    {
        if (iterator->GetConfidence() > bestConfidence) 
        {
            bestNode = iterator;
            bestConfidence = iterator->GetConfidence();
        }
        iterator = iterator->GetNextSibling();
    }

    TraverseForNextMove(bestNode->GetFirstChild(), bestNode);
}

PlayerColor IsGameOver(Node * node)
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

PlayerColor FinishPlayOut(Node * initialState) 
{
    Node * currentBoard = new Node(initialState->GetWhitePieces(), initialState->GetBlackPieces(), initialState->GetNextToPlay());
    random_device random;
    mt19937 mt(random());

    PlayerColor gameOver = IsGameOver(currentBoard);
    while (gameOver == PlayerColor::Neither) 
    {
        int totalKids = currentBoard->GenerateChildren();
        uniform_int_distribution<int> distribution(0, totalKids);
        int selectedChild = distribution(random);

        Node * root = 
    }
    //GenerateChild
    //repeat until someone wins.
}

void Analyzer::RunPlayOut(Node * root) 
{
    //Iterate through all leaf nodes in the tree, determining the highest confidence interval.
    //Run a playout on that node.
    //BackPropagate the full tree.

    Node * bestNode;

    TraverseForNextMove(root, bestNode);

    bestNode->GenerateChildren();
    Node * initialState = bestNode->GetFirstChild();

    PlayerColor winner = FinishPlayOut(initialState);
}

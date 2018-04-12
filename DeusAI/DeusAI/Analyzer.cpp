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
    Move move;
    if (parent->GetNextToPlay() == PlayerColor::White)
    {
        unsigned long long movedPiece = parent->GetWhitePieces() ^ child->GetWhitePieces();
        int destination = BitsMagic::BitScanForwardWithReset(movedPiece);
        int start = BitsMagic::BitScanForwardWithReset(movedPiece);

        move.xCoordinate = Masks::OrientationMasks::CurrentColumn[start];
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

    }
    else
    {
        unsigned long long movedPiece = parent->GetBlackPieces() ^ child->GetBlackPieces();
        int start = BitsMagic::BitScanForwardWithReset(movedPiece);
        int destination = BitsMagic::BitScanForwardWithReset(movedPiece);

        move.xCoordinate = Masks::OrientationMasks::CurrentColumn[start];
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

    }
    return move;
}

Move Analyzer::GetMove(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color)
{
    Move bestMove;
    Node * root = new Node(whitePieces, blackPieces, color);

    _totalPlayOuts = 0;

    clock_t startTime = clock();
    while ((((float)(clock() - startTime))) < MAX_TIME)
    {
        RunPlayOut(root);
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

Move Analyzer::GetMoveImproved(unsigned long long whitePieces, unsigned long long blackPieces, PlayerColor color)
{
    clock_t startTime = clock();

    Move bestMove;
    Node * root = new Node(whitePieces, blackPieces, color);

    _totalPlayOuts = 0;

    root->GenerateChildren();
    Node * winningMoves = NULL;
    Node* savingMoves = NULL;
    Node* safeCaptures = NULL;
    Node* safeMoves = NULL;
    Node* unsafeMoves = NULL;

    Node* temp = root->GetFirstChild();

    //Sort children;
    while (temp != NULL)
    {
        Node * copy = new Node(temp->GetWhitePieces(), temp->GetBlackPieces(), temp->GetNextToPlay(), temp->GetWins(), temp->GetGames());

        if (root->GetNextToPlay() == White)
        {
            if (Analyzer::IsGameOver(temp) == White)
            {
                if (winningMoves == NULL)
                {
                    winningMoves = copy;
                }
                else
                {
                    Node* t = winningMoves;
                    while (t->GetNextSibling() != NULL)
                    {
                        t = t->GetNextSibling();
                    }
                    t->SetNextSibling(copy);
                    copy->SetPrevSibling(t);
                }
            }
            else if ((root->GetBlackPieces() & Grid::Row2) != 0
                && (root->GetBlackPieces() & Grid::Row2) == 0)
            {
                if (savingMoves == NULL)
                {
                    savingMoves = copy;
                }
                else
                {
                    Node* t = savingMoves;
                    while (t->GetNextSibling() != NULL)
                    {
                        t = t->GetNextSibling();
                    }
                    t->SetNextSibling(copy);
                    copy->SetPrevSibling(t);
                }
            }
            else
            {
                unsigned long long movedPiece = root->GetWhitePieces() ^ temp->GetWhitePieces();
                int destination = BitsMagic::BitScanForwardWithReset(movedPiece);

                int attackCount = 0;
                int protectCount = 0;


                if ((temp->GetWhitePieces() & Masks::BlackMasks::EastAttack[destination]) != 0)
                {
                    protectCount++;
                }

                if ((temp->GetWhitePieces() & Masks::BlackMasks::WestAttack[destination]) != 0)
                {
                    protectCount++;
                }

                if ((temp->GetBlackPieces() & Masks::WhiteMasks::EastAttack[destination]) != 0)
                {
                    attackCount++;
                }

                if ((temp->GetBlackPieces() & Masks::WhiteMasks::WestAttack[destination]) != 0)
                {
                    attackCount++;
                }


                if (protectCount >= attackCount && (root->GetBlackPieces() & Masks::OrientationMasks::CurrentSquare[destination]) != 0)
                {
                    if (safeCaptures == NULL)
                    {
                        safeCaptures = copy;
                    }
                    else
                    {
                        Node* t = safeCaptures;
                        while (t->GetNextSibling() != NULL)
                        {
                            t = t->GetNextSibling();
                        }
                        t->SetNextSibling(copy);
                        copy->SetPrevSibling(t);
                    }
                }
                //If destination is safe and NOT capture
                else if (protectCount >= attackCount)
                {
                    if (safeMoves == NULL)
                    {
                        safeMoves = copy;
                    }
                    else
                    {
                        Node* t = safeMoves;
                        while (t->GetNextSibling() != NULL)
                        {
                            t = t->GetNextSibling();
                        }
                        t->SetNextSibling(copy);
                        copy->SetPrevSibling(t);
                    }
                }
                //If destination is not safe
                else
                {
                    if (unsafeMoves == NULL)
                    {
                        unsafeMoves = copy;
                    }
                    else
                    {
                        Node* t = unsafeMoves;
                        while (t->GetNextSibling() != NULL)
                        {
                            t = t->GetNextSibling();
                        }
                        t->SetNextSibling(copy);
                        copy->SetPrevSibling(t);
                    }
                }
            }
        }
        else
        {

            if (Analyzer::IsGameOver(temp) == Black)
            {
                if (winningMoves == NULL)
                {
                    winningMoves = copy;
                }
                else
                {
                    Node* t = winningMoves;
                    while (t->GetNextSibling() != NULL)
                    {
                        t = t->GetNextSibling();
                    }
                    t->SetNextSibling(copy);
                    copy->SetPrevSibling(t);
                }
            }
            else if ((root->GetWhitePieces() & Grid::Row7) != 0
                && (temp->GetWhitePieces() & Grid::Row7) == 0)
            {
                if (savingMoves == NULL)
                {
                    savingMoves = copy;
                }
                else
                {
                    Node* t = savingMoves;
                    while (t->GetNextSibling() != NULL)
                    {
                        t = t->GetNextSibling();
                    }
                    t->SetNextSibling(copy);
                    copy->SetPrevSibling(t);
                }
            }
            else
            {
                unsigned long long movedPiece = root->GetBlackPieces() ^ temp->GetBlackPieces();
                int start = BitsMagic::BitScanForwardWithReset(movedPiece);
                int destination = BitsMagic::BitScanForwardWithReset(movedPiece);

                int attackCount = 0;
                int protectCount = 0;

                if ((temp->GetBlackPieces() & Masks::WhiteMasks::EastAttack[destination]) != 0)
                {
                    protectCount++;
                }

                if ((temp->GetBlackPieces() & Masks::WhiteMasks::WestAttack[destination]) != 0)
                {
                    protectCount++;
                }

                if ((temp->GetWhitePieces() & Masks::BlackMasks::EastAttack[destination]) != 0)
                {
                    attackCount++;
                }

                if ((temp->GetWhitePieces() & Masks::BlackMasks::WestAttack[destination]) != 0)
                {
                    attackCount++;
                }

                //If destination is safe and a capture
                if (protectCount >= attackCount && (root->GetWhitePieces() & Masks::OrientationMasks::CurrentSquare[destination]) != 0)
                {
                    if (safeCaptures == NULL)
                    {
                        safeCaptures = copy;
                    }
                    else
                    {
                        Node* t = safeCaptures;
                        while (t->GetNextSibling() != NULL)
                        {
                            t = t->GetNextSibling();
                        }
                        t->SetNextSibling(copy);
                        copy->SetPrevSibling(t);
                    }
                }
                //If destination is safe and NOT a capture
                else if (protectCount >= attackCount)
                {
                    if (safeMoves == NULL)
                    {
                        safeMoves = copy;
                    }
                    else
                    {
                        Node* t = safeMoves;
                        while (t->GetNextSibling() != NULL)
                        {
                            t = t->GetNextSibling();
                        }
                        t->SetNextSibling(copy);
                        copy->SetPrevSibling(t);
                    }
                }
                //If destination is not safe
                else
                {
                    if (unsafeMoves == NULL)
                    {
                        unsafeMoves = copy;
                    }
                    else
                    {
                        Node* t = unsafeMoves;
                        while (t->GetNextSibling() != NULL)
                        {
                            t = t->GetNextSibling();
                        }
                        t->SetNextSibling(copy);
                        copy->SetPrevSibling(t);
                    }
                }
            }
        }
        temp = temp->GetNextSibling();
    }

    //Process results, put correct children as the real children, delete all others;
    delete root->GetFirstChild();

    if (winningMoves != NULL)
    {
        winningMoves->SetParent(root);
        root->SetFirstChild(winningMoves);

        //delete winningMoves;
        delete savingMoves;
        delete safeCaptures;
        delete safeMoves;
        delete unsafeMoves;
    }
    else if (savingMoves != NULL)
    {
        savingMoves->SetParent(root);
        root->SetFirstChild(savingMoves);

        //delete winningMoves;
        //delete savingMoves;
        delete safeCaptures;
        delete safeMoves;
        delete unsafeMoves;
    }
    else if (safeCaptures != NULL)
    {
        safeCaptures->SetParent(root);
        root->SetFirstChild(safeCaptures);

        //delete winningMoves;
        //delete savingMoves;
        //delete safeCaptures;
        delete safeMoves;
        delete unsafeMoves;
    }
    else if (safeMoves != NULL)
    {
        safeMoves->SetParent(root);
        root->SetFirstChild(safeMoves);

        //delete winningMoves;
        //delete savingMoves;
        //delete safeCaptures;
        //delete safeMoves;
        delete unsafeMoves;
    }
    else
    {
        unsafeMoves->SetParent(root);
        root->SetFirstChild(unsafeMoves);

        //delete winningMoves;
        //delete savingMoves;
        //delete safeCaptures;
        //delete safeMoves;
        //delete unsafeMoves;
    }

    while ((((float)(clock() - startTime))) < MAX_TIME)
    {
        RunPlayOut(root);
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

Node * GetBestNode(Node * currentNode)
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
    if (root->GetFirstChild() == NULL)
    {
        return root;
    }

    Node * bestCurrentChild = GetBestNode(root);

    if (bestCurrentChild == NULL)
    {

        return NULL;
    }

    else if (bestCurrentChild->GetFirstChild() == NULL)
    {
        return bestCurrentChild;
    }

    bool continueLoop = true;
    while (continueLoop)
    {
        Node * temp = GetBestNode(bestCurrentChild);

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

//Now runs with leaf parallelization of factor 8.
//Now terminates simulations at 5 turns deep.
//Now scores games using 3/4 of Lorentz's algorithm.
void Analyzer::RunPlayOut(Node * root)
{
    Node * bestNode;

    bestNode = TraverseForNextMove(root);

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
            Node * currentBoard = new Node(bestNode->GetWhitePieces(), bestNode->GetBlackPieces(), bestNode->GetNextToPlay());
            Node * root = currentBoard;
            random_device random;
            mt19937 mt(random());

            PlayerColor gameOver = Analyzer::IsGameOver(currentBoard);
            int turnCounter = 0;
            while (gameOver == PlayerColor::Neither && turnCounter < 5)
            {
                int totalKids = currentBoard->GenerateChildren();
                vector<Node*> selectionPool;//Will have safe moves three times to make safe moves have preference in playout.
                Node* temp = currentBoard->GetFirstChild();

                Node* winningMove = NULL;
                Node* savingMove = NULL;
                while (temp != NULL)
                {
                    if (currentBoard->GetNextToPlay() == White)
                    {

                        if (Analyzer::IsGameOver(temp) == White)
                        {
                            winningMove = temp;
                        }
                        else if ((currentBoard->GetBlackPieces() & Grid::Row2) != 0
                            && (temp->GetBlackPieces() & Grid::Row2) == 0)
                        {
                            savingMove = temp;
                        }
                        else
                        {
                            unsigned long long movedPiece = currentBoard->GetWhitePieces() ^ temp->GetWhitePieces();
                            int destination = BitsMagic::BitScanForwardWithReset(movedPiece);

                            int attackCount = 0;
                            int protectCount = 0;


                            if ((temp->GetWhitePieces() & Masks::BlackMasks::EastAttack[destination]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->GetWhitePieces() & Masks::BlackMasks::WestAttack[destination]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->GetBlackPieces() & Masks::WhiteMasks::EastAttack[destination]) != 0)
                            {
                                attackCount++;
                            }

                            if ((temp->GetBlackPieces() & Masks::WhiteMasks::WestAttack[destination]) != 0)
                            {
                                attackCount++;
                            }

                            //If destination is safe, add three times.
                            if (protectCount >= attackCount)
                            {
                                selectionPool.push_back(temp);
                                selectionPool.push_back(temp);
                                selectionPool.push_back(temp);
                            }
                            //If destination is not safe, add once.
                            else
                            {
                                selectionPool.push_back(temp);
                            }
                        }
                    }
                    else
                    {

                        if (Analyzer::IsGameOver(temp) == Black)
                        {
                            winningMove = temp;
                        }
                        else if ((currentBoard->GetWhitePieces() & Grid::Row7) != 0
                            && (temp->GetWhitePieces() & Grid::Row7) == 0)
                        {
                            savingMove = temp;
                        }
                        else
                        {
                            unsigned long long movedPiece = currentBoard->GetBlackPieces() ^ temp->GetBlackPieces();
                            int start = BitsMagic::BitScanForwardWithReset(movedPiece);
                            int destination = BitsMagic::BitScanForwardWithReset(movedPiece);

                            int attackCount = 0;
                            int protectCount = 0;

                            if ((temp->GetBlackPieces() & Masks::WhiteMasks::EastAttack[destination]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->GetBlackPieces() & Masks::WhiteMasks::WestAttack[destination]) != 0)
                            {
                                protectCount++;
                            }

                            if ((temp->GetWhitePieces() & Masks::BlackMasks::EastAttack[destination]) != 0)
                            {
                                attackCount++;
                            }

                            if ((temp->GetWhitePieces() & Masks::BlackMasks::WestAttack[destination]) != 0)
                            {
                                attackCount++;
                            }

                            //If destination is safe, add three times.
                            if (protectCount >= attackCount)
                            {
                                selectionPool.push_back(temp);
                                selectionPool.push_back(temp);
                                selectionPool.push_back(temp);
                            }
                            //If destination is not safe, add once.
                            else
                            {
                                selectionPool.push_back(temp);
                            }
                        }
                    }
                    temp = temp->GetNextSibling();
                }

                if (winningMove != NULL)
                {
                    currentBoard = winningMove;
                }
                else if (savingMove != NULL)
                {
                    currentBoard = savingMove;
                }
                else
                {
                    uniform_int_distribution<int> distribution(0, totalKids - 1);
                    int selectedChild = distribution(mt);

                    currentBoard = selectionPool[selectedChild];
                }

                gameOver = Analyzer::IsGameOver(currentBoard);
                turnCounter++;
            }

            if (gameOver == Neither)
            {
                gameOver = Analyzer::ScoreGame(currentBoard);
            }

            delete root;
            colors[i] = FlipColor(gameOver);

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


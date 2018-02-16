#include "stdafx.h"
#include "BitsMagic.h"


BitsMagic::BitsMagic()
{

}


BitsMagic::~BitsMagic()
{

}

int BitsMagic::_index64[64] =
{
    0,  1, 48,  2, 57, 49, 28,  3,
    61, 58, 50, 42, 38, 29, 17,  4,
    62, 55, 59, 36, 53, 51, 43, 22,
    45, 39, 33, 30, 24, 18, 12,  5,
    63, 47, 56, 27, 60, 41, 37, 16,
    54, 35, 52, 21, 44, 32, 23, 11,
    46, 26, 40, 15, 34, 20, 31, 10,
    25, 14, 19,  9, 13,  8,  7,  6
};

int BitsMagic::MyBitScanForward(unsigned long long pieces)
{
    if (pieces == 0)
    {
        return -1;
    }

    return _index64[(int)(((unsigned long long)((long long)pieces & -(long long)(pieces)) * BitsMagic::debruijn64) >> 58)];
}

int BitsMagic::BitScanForwardWithReset(unsigned long long & pieces)
{
    int index = MyBitScanForward(pieces);
    pieces &= pieces - 1;
    return index;
}

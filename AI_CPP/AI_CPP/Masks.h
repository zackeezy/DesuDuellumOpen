#pragma once
#include <map>

static class Masks
{
private:
    Masks();
    ~Masks();
public:
    static class WhiteMasks
    {
    private:
        static unsigned long long * GenerateMobilityTriangleList();
        static unsigned long long GenerateMobilityTriangle(int index);

    public:
        static unsigned long long Forward[64];
        static unsigned long long EastAttack[64];
        static unsigned long long WestAttack[64];
        static unsigned long long * MobilityTriangleList;
    };

    static class BlackMasks
    {
    private:
        static unsigned long long * GenerateMobilityTriangleList();
        static unsigned long long GenerateMobilityTriangle(int index);

    public:
        static unsigned long long Forward[64];
        static unsigned long long EastAttack[64];
        static unsigned long long WestAttack[64];
        static unsigned long long * MobilityTriangleList;
    };

    static class OrientationMasks
    {
    private:
    public:
        static int CurrentColumn[64];
        static int CurrentRow[64];
        static unsigned long long CurrentSquare[64];
        static std::map<unsigned long long, int> IndexOf;
        static unsigned long long Above[64];
        static unsigned long long RightOf[64];
        static unsigned long long CoordinatesToUlong[8][8];
    };
};


#pragma once
static class BitsMagic
{
private:
    static int _index64[64];
    static const long long debruijn64 = 0x03f79d71b4cb0a89;
public:
    BitsMagic();
    ~BitsMagic();

    static int MyBitScanForward(unsigned long long pieces);
    static int BitScanForwardWithReset(unsigned long long & pieces);
};


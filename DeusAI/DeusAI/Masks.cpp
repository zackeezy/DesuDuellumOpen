#include "stdafx.h"
#include "Masks.h"
#include "Grid.h"
Masks::Masks()
{
}
Masks::~Masks()
{

}

void Masks::InitializeMobilityTriangles()
{

}

//WhiteMasks
unsigned long long Masks::WhiteMasks::MobilityTriangle[64] = {
    0, 0, 0, 0, 0, 0, 0, 0,
    0x3, 0x7, 0xe, 0x1c, 0x38, 0x70, 0xe0, 0xc0,
    0x307, 0x70f, 0xe1f, 0x1c3e, 0x387c, 0x70f8, 0xe0f0, 0xc0e0,
    0x3070f, 0x70f1f, 0xe1f3f, 0x1c3e7f, 0x387cfe, 0x70f8fc, 0xe0f0f8, 0xc0e0f0,
    0x3070f1f, 0x70f1f3f, 0xe1f3f7f, 0x1c3e7fff, 0x387cfeff, 0x70f8fcfe, 0xe0f0f8fc, 0xc0e0f0f8,
    0x3070f1f3f, 0x70f1f3f7f, 0xe1f3f7fff, 0x1c3e7fffff, 0x387cfeffff, 0x70f8fcfeff, 0xe0f0f8fcfe, 0xc0e0f0f8fc,
    0x3070f1f3f7f, 0x70f1f3f7fff, 0xe1f3f7fffff, 0x1c3e7fffffff, 0x387cfeffffff, 0x70f8fcfeffff, 0xe0f0f8fcfeff, 0xc0e0f0f8fcfe,
    0x3070f1f3f7fff, 0x70f1f3f7fffff, 0xe1f3f7fffffff, 0x1c3e7fffffffff, 0x387cfeffffffff, 0x70f8fcfeffffff, 0xe0f0f8fcfeffff, 0xc0e0f0f8fcfeff,
};

unsigned long long Masks::WhiteMasks::Forward[64] =
{
    0, 0, 0, 0, 0, 0, 0, 0,
    Grid::H8, Grid::G8, Grid::F8, Grid::E8, Grid::D8, Grid::C8, Grid::B8, Grid::A8,
    Grid::H7, Grid::G7, Grid::F7, Grid::E7, Grid::D7, Grid::C7, Grid::B7, Grid::A7,
    Grid::H6, Grid::G6, Grid::F6, Grid::E6, Grid::D6, Grid::C6, Grid::B6, Grid::A6,
    Grid::H5, Grid::G5, Grid::F5, Grid::E5, Grid::D5, Grid::C5, Grid::B5, Grid::A5,
    Grid::H4, Grid::G4, Grid::F4, Grid::E4, Grid::D4, Grid::C4, Grid::B4, Grid::A4,
    Grid::H3, Grid::G3, Grid::F3, Grid::E3, Grid::D3, Grid::C3, Grid::B3, Grid::A3,
    Grid::H2, Grid::G2, Grid::F2, Grid::E2, Grid::D2, Grid::C2, Grid::B2, Grid::A2,
};

unsigned long long Masks::WhiteMasks::EastAttack[64] =
{
    0, 0, 0, 0, 0, 0, 0, 0,
    0, Grid::H8, Grid::G8, Grid::F8, Grid::E8, Grid::D8, Grid::C8, Grid::B8,
    0, Grid::H7, Grid::G7, Grid::F7, Grid::E7, Grid::D7, Grid::C7, Grid::B7,
    0, Grid::H6, Grid::G6, Grid::F6, Grid::E6, Grid::D6, Grid::C6, Grid::B6,
    0, Grid::H5, Grid::G5, Grid::F5, Grid::E5, Grid::D5, Grid::C5, Grid::B5,
    0, Grid::H4, Grid::G4, Grid::F4, Grid::E4, Grid::D4, Grid::C4, Grid::B4,
    0, Grid::H3, Grid::G3, Grid::F3, Grid::E3, Grid::D3, Grid::C3, Grid::B3,
    0, Grid::H2, Grid::G2, Grid::F2, Grid::E2, Grid::D2, Grid::C2, Grid::B2,
};

unsigned long long Masks::WhiteMasks::WestAttack[64] =
{
    0, 0, 0, 0, 0, 0, 0, 0,
    Grid::G8, Grid::F8, Grid::E8, Grid::D8, Grid::C8, Grid::B8, Grid::A8, 0,
    Grid::G7, Grid::F7, Grid::E7, Grid::D7, Grid::C7, Grid::B7, Grid::A7, 0,
    Grid::G6, Grid::F6, Grid::E6, Grid::D6, Grid::C6, Grid::B6, Grid::A6, 0,
    Grid::G5, Grid::F5, Grid::E5, Grid::D5, Grid::C5, Grid::B5, Grid::A5, 0,
    Grid::G4, Grid::F4, Grid::E4, Grid::D4, Grid::C4, Grid::B4, Grid::A4, 0,
    Grid::G3, Grid::F3, Grid::E3, Grid::D3, Grid::C3, Grid::B3, Grid::A3, 0,
    Grid::G2, Grid::F2, Grid::E2, Grid::D2, Grid::C2, Grid::B2, Grid::A2, 0,
};




//BlackMasks
unsigned long long Masks::BlackMasks::MobilityTriangle[64] =
{
    0xff7f3f1f0f070000, 0xffff7f3f1f0f0700, 0xffffff7f3f1f0e00, 0xffffffff7f3e1c00, 0xfffffffffe7c3800, 0xfffffffefcf87000, 0xfffffefcf8f0e000, 0xfffefcf8f0e0c000,
    0x7f3f1f0f07030000, 0xff7f3f1f0f070000, 0xffff7f3f1f0e0000, 0xffffff7f3e1c0000, 0xfffffffe7c380000, 0xfffffefcf8700000, 0xfffefcf8f0e00000, 0xfefcf8f0e0c00000,
    0x3f1f0f0703000000, 0x7f3f1f0f07000000, 0xff7f3f1f0e000000, 0xffff7f3e1c000000, 0xfffffe7c38000000, 0xfffefcf870000000, 0xfefcf8f0e0000000, 0xfcf8f0e0c0000000,
    0x1f0f070300000000, 0x3f1f0f0700000000, 0x7f3f1f0e00000000, 0xff7f3e1c00000000, 0xfffe7c3800000000, 0xfefcf87000000000, 0xfcf8f0e000000000, 0xf8f0e0c000000000,
    0xf07030000000000, 0x1f0f070000000000, 0x3f1f0e0000000000, 0x7f3e1c0000000000, 0xfe7c380000000000, 0xfcf8700000000000, 0xf8f0e00000000000, 0xf0e0c00000000000,
    0x703000000000000, 0xf07000000000000, 0x1f0e000000000000, 0x3e1c000000000000, 0x7c38000000000000, 0xf870000000000000, 0xf0e0000000000000, 0xe0c0000000000000,
    0x300000000000000, 0x700000000000000, 0xe00000000000000, 0x1c00000000000000, 0x3800000000000000, 0x7000000000000000, 0xe000000000000000, 0xc000000000000000,
    0, 0, 0, 0, 0, 0, 0, 0,
};


unsigned long long Masks::BlackMasks::Forward[64] =
{
    Grid::H7, Grid::G7, Grid::F7, Grid::E7, Grid::D7, Grid::C7, Grid::B7, Grid::A7,
    Grid::H6, Grid::G6, Grid::F6, Grid::E6, Grid::D6, Grid::C6, Grid::B6, Grid::A6,
    Grid::H5, Grid::G5, Grid::F5, Grid::E5, Grid::D5, Grid::C5, Grid::B5, Grid::A5,
    Grid::H4, Grid::G4, Grid::F4, Grid::E4, Grid::D4, Grid::C4, Grid::B4, Grid::A4,
    Grid::H3, Grid::G3, Grid::F3, Grid::E3, Grid::D3, Grid::C3, Grid::B3, Grid::A3,
    Grid::H2, Grid::G2, Grid::F2, Grid::E2, Grid::D2, Grid::C2, Grid::B2, Grid::A2,
    Grid::H1, Grid::G1, Grid::F1, Grid::E1, Grid::D1, Grid::C1, Grid::B1, Grid::A1,
    0, 0, 0, 0, 0, 0, 0, 0,
};

unsigned long long Masks::BlackMasks::EastAttack[64] =
{
    0, Grid::H7, Grid::G7, Grid::F7, Grid::E7, Grid::D7, Grid::C7, Grid::B7,
    0, Grid::H6, Grid::G6, Grid::F6, Grid::E6, Grid::D6, Grid::C6, Grid::B6,
    0, Grid::H5, Grid::G5, Grid::F5, Grid::E5, Grid::D5, Grid::C5, Grid::B5,
    0, Grid::H4, Grid::G4, Grid::F4, Grid::E4, Grid::D4, Grid::C4, Grid::B4,
    0, Grid::H3, Grid::G3, Grid::F3, Grid::E3, Grid::D3, Grid::C3, Grid::B3,
    0, Grid::H2, Grid::G2, Grid::F2, Grid::E2, Grid::D2, Grid::C2, Grid::B2,
    0, Grid::H1, Grid::G1, Grid::F1, Grid::E1, Grid::D1, Grid::C1, Grid::B1,
    0, 0, 0, 0, 0, 0, 0, 0,
};

unsigned long long Masks::BlackMasks::WestAttack[64] =
{
    Grid::G7, Grid::F7, Grid::E7, Grid::D7, Grid::C7, Grid::B7, Grid::A7, 0,
    Grid::G6, Grid::F6, Grid::E6, Grid::D6, Grid::C6, Grid::B6, Grid::A6, 0,
    Grid::G5, Grid::F5, Grid::E5, Grid::D5, Grid::C5, Grid::B5, Grid::A5, 0,
    Grid::G4, Grid::F4, Grid::E4, Grid::D4, Grid::C4, Grid::B4, Grid::A4, 0,
    Grid::G3, Grid::F3, Grid::E3, Grid::D3, Grid::C3, Grid::B3, Grid::A3, 0,
    Grid::G2, Grid::F2, Grid::E2, Grid::D2, Grid::C2, Grid::B2, Grid::A2, 0,
    Grid::G1, Grid::F1, Grid::E1, Grid::D1, Grid::C1, Grid::B1, Grid::A1, 0,
    0, 0, 0, 0, 0, 0, 0, 0,
};

//OrientationMasks

int Masks::OrientationMasks::CurrentColumn[64] =
{
    8, 7, 6, 5, 4, 3, 2, 1,
    8, 7, 6, 5, 4, 3, 2, 1,
    8, 7, 6, 5, 4, 3, 2, 1,
    8, 7, 6, 5, 4, 3, 2, 1,
    8, 7, 6, 5, 4, 3, 2, 1,
    8, 7, 6, 5, 4, 3, 2, 1,
    8, 7, 6, 5, 4, 3, 2, 1,
    8, 7, 6, 5, 4, 3, 2, 1,
};

int Masks::OrientationMasks::CurrentRow[64] =
{
    8, 8, 8, 8, 8, 8, 8, 8,
    7, 7, 7, 7, 7, 7, 7, 7,
    6, 6, 6, 6, 6, 6, 6, 6,
    5, 5, 5, 5, 5, 5, 5, 5,
    4, 4, 4, 4, 4, 4, 4, 4,
    3, 3, 3, 3, 3, 3, 3, 3,
    2, 2, 2, 2, 2, 2, 2, 2,
    1, 1, 1, 1, 1, 1, 1, 1,
};

unsigned long long Masks::OrientationMasks::CurrentSquare[64] =
{
    Grid::H8, Grid::G8, Grid::F8, Grid::E8, Grid::D8, Grid::C8, Grid::B8, Grid::A8,
    Grid::H7, Grid::G7, Grid::F7, Grid::E7, Grid::D7, Grid::C7, Grid::B7, Grid::A7,
    Grid::H6, Grid::G6, Grid::F6, Grid::E6, Grid::D6, Grid::C6, Grid::B6, Grid::A6,
    Grid::H5, Grid::G5, Grid::F5, Grid::E5, Grid::D5, Grid::C5, Grid::B5, Grid::A5,
    Grid::H4, Grid::G4, Grid::F4, Grid::E4, Grid::D4, Grid::C4, Grid::B4, Grid::A4,
    Grid::H3, Grid::G3, Grid::F3, Grid::E3, Grid::D3, Grid::C3, Grid::B3, Grid::A3,
    Grid::H2, Grid::G2, Grid::F2, Grid::E2, Grid::D2, Grid::C2, Grid::B2, Grid::A2,
    Grid::H1, Grid::G1, Grid::F1, Grid::E1, Grid::D1, Grid::C1, Grid::B1, Grid::A1,
};

std::map<unsigned long long, int> Masks::OrientationMasks::IndexOf =
{
    { 0, 0 },

{ Grid::H8, 0 },
{ Grid::G8, 1 },
{ Grid::F8, 2 },
{ Grid::E8, 3 },
{ Grid::D8, 4 },
{ Grid::C8, 5 },
{ Grid::B8, 6 },
{ Grid::A8, 7 },

{ Grid::H7, 8 },
{ Grid::G7, 9 },
{ Grid::F7, 10 },
{ Grid::E7, 11 },
{ Grid::D7, 12 },
{ Grid::C7, 13 },
{ Grid::B7, 14 },
{ Grid::A7, 15 },

{ Grid::H6, 16 },
{ Grid::G6, 17 },
{ Grid::F6, 18 },
{ Grid::E6, 19 },
{ Grid::D6, 20 },
{ Grid::C6, 21 },
{ Grid::B6, 22 },
{ Grid::A6, 23 },

{ Grid::H5, 24 },
{ Grid::G5, 25 },
{ Grid::F5, 26 },
{ Grid::E5, 27 },
{ Grid::D5, 28 },
{ Grid::C5, 29 },
{ Grid::B5, 30 },
{ Grid::A5, 31 },

{ Grid::H4, 32 },
{ Grid::G4, 33 },
{ Grid::F4, 34 },
{ Grid::E4, 35 },
{ Grid::D4, 36 },
{ Grid::C4, 37 },
{ Grid::B4, 38 },
{ Grid::A4, 39 },

{ Grid::H3, 40 },
{ Grid::G3, 41 },
{ Grid::F3, 42 },
{ Grid::E3, 43 },
{ Grid::D3, 44 },
{ Grid::C3, 45 },
{ Grid::B3, 46 },
{ Grid::A3, 47 },

{ Grid::H2, 48 },
{ Grid::G2, 49 },
{ Grid::F2, 50 },
{ Grid::E2, 51 },
{ Grid::D2, 52 },
{ Grid::C2, 53 },
{ Grid::B2, 54 },
{ Grid::A2, 55 },

{ Grid::H1, 56 },
{ Grid::G1, 57 },
{ Grid::F1, 58 },
{ Grid::E1, 59 },
{ Grid::D1, 60 },
{ Grid::C1, 61 },
{ Grid::B1, 62 },
{ Grid::A1, 63 },
};

unsigned long long Masks::OrientationMasks::Above[64] =
{
    0, 0, 0, 0, 0, 0, 0, 0,
    Grid::H8, Grid::G8, Grid::F8, Grid::E8, Grid::D8, Grid::C8, Grid::B8, Grid::A8,
    Grid::H7, Grid::G7, Grid::F7, Grid::E7, Grid::D7, Grid::C7, Grid::B7, Grid::A7,
    Grid::H6, Grid::G6, Grid::F6, Grid::E6, Grid::D6, Grid::C6, Grid::B6, Grid::A6,
    Grid::H5, Grid::G5, Grid::F5, Grid::E5, Grid::D5, Grid::C5, Grid::B5, Grid::A5,
    Grid::H4, Grid::G4, Grid::F4, Grid::E4, Grid::D4, Grid::C4, Grid::B4, Grid::A4,
    Grid::H3, Grid::G3, Grid::F3, Grid::E3, Grid::D3, Grid::C3, Grid::B3, Grid::A3,
    Grid::H2, Grid::G2, Grid::F2, Grid::E2, Grid::D2, Grid::C2, Grid::B2, Grid::A2,
};

unsigned long long Masks::OrientationMasks::RightOf[64] =
{
    0, Grid::H8, Grid::G8, Grid::F8, Grid::E8, Grid::D8, Grid::C8, Grid::B8,
    0, Grid::H7, Grid::G7, Grid::F7, Grid::E7, Grid::D7, Grid::C7, Grid::B7,
    0, Grid::H6, Grid::G6, Grid::F6, Grid::E6, Grid::D6, Grid::C6, Grid::B6,
    0, Grid::H5, Grid::G5, Grid::F5, Grid::E5, Grid::D5, Grid::C5, Grid::B5,
    0, Grid::H4, Grid::G4, Grid::F4, Grid::E4, Grid::D4, Grid::C4, Grid::B4,
    0, Grid::H3, Grid::G3, Grid::F3, Grid::E3, Grid::D3, Grid::C3, Grid::B3,
    0, Grid::H2, Grid::G2, Grid::F2, Grid::E2, Grid::D2, Grid::C2, Grid::B2,
    0, Grid::H1, Grid::G1, Grid::F1, Grid::E1, Grid::D1, Grid::C1, Grid::B1,
};

unsigned long long Masks::OrientationMasks::CoordinatesToUlong[8][8] =
{
    { Grid::A1, Grid::A2, Grid::A3, Grid::A4, Grid::A5, Grid::A6, Grid::A7, Grid::A8, },
{ Grid::B1, Grid::B2, Grid::B3, Grid::B4, Grid::B5, Grid::B6, Grid::B7, Grid::B8, },
{ Grid::C1, Grid::C2, Grid::C3, Grid::C4, Grid::C5, Grid::C6, Grid::C7, Grid::C8, },
{ Grid::D1, Grid::D2, Grid::D3, Grid::D4, Grid::D5, Grid::D6, Grid::D7, Grid::D8, },
{ Grid::E1, Grid::E2, Grid::E3, Grid::E4, Grid::E5, Grid::E6, Grid::E7, Grid::E8, },
{ Grid::F1, Grid::F2, Grid::F3, Grid::F4, Grid::F5, Grid::F6, Grid::F7, Grid::F8, },
{ Grid::G1, Grid::G2, Grid::G3, Grid::G4, Grid::G5, Grid::G6, Grid::G7, Grid::G8, },
{ Grid::H1, Grid::H2, Grid::H3, Grid::H4, Grid::H5, Grid::H6, Grid::H7, Grid::H8, },
};
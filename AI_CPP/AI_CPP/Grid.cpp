#include "Grid.h"

Grid::Grid()
{
}

unsigned long long Grid::A1 = 0x8000000000000000;
unsigned long long Grid::B1 = 0x4000000000000000;
unsigned long long Grid::C1 = 0x2000000000000000;
unsigned long long Grid::D1 = 0x1000000000000000;
unsigned long long Grid::E1 = 0x0800000000000000;
unsigned long long Grid::F1 = 0x0400000000000000;
unsigned long long Grid::G1 = 0x0200000000000000;
unsigned long long Grid::H1 = 0x0100000000000000;

unsigned long long Grid::A2 = 0x0080000000000000;
unsigned long long Grid::B2 = 0x0040000000000000;
unsigned long long Grid::C2 = 0x0020000000000000;
unsigned long long Grid::D2 = 0x0010000000000000;
unsigned long long Grid::E2 = 0x0008000000000000;
unsigned long long Grid::F2 = 0x0004000000000000;
unsigned long long Grid::G2 = 0x0002000000000000;
unsigned long long Grid::H2 = 0x0001000000000000;

unsigned long long Grid::A3 = 0x0000800000000000;
unsigned long long Grid::B3 = 0x0000400000000000;
unsigned long long Grid::C3 = 0x0000200000000000;
unsigned long long Grid::D3 = 0x0000100000000000;
unsigned long long Grid::E3 = 0x0000080000000000;
unsigned long long Grid::F3 = 0x0000040000000000;
unsigned long long Grid::G3 = 0x0000020000000000;
unsigned long long Grid::H3 = 0x0000010000000000;

unsigned long long Grid::A4 = 0x0000008000000000;
unsigned long long Grid::B4 = 0x0000004000000000;
unsigned long long Grid::C4 = 0x0000002000000000;
unsigned long long Grid::D4 = 0x0000001000000000;
unsigned long long Grid::E4 = 0x0000000800000000;
unsigned long long Grid::F4 = 0x0000000400000000;
unsigned long long Grid::G4 = 0x0000000200000000;
unsigned long long Grid::H4 = 0x0000000100000000;

unsigned long long Grid::A5 = 0x0000000080000000;
unsigned long long Grid::B5 = 0x0000000040000000;
unsigned long long Grid::C5 = 0x0000000020000000;
unsigned long long Grid::D5 = 0x0000000010000000;
unsigned long long Grid::E5 = 0x0000000008000000;
unsigned long long Grid::F5 = 0x0000000004000000;
unsigned long long Grid::G5 = 0x0000000002000000;
unsigned long long Grid::H5 = 0x0000000001000000;

unsigned long long Grid::A6 = 0x0000000000800000;
unsigned long long Grid::B6 = 0x0000000000400000;
unsigned long long Grid::C6 = 0x0000000000200000;
unsigned long long Grid::D6 = 0x0000000000100000;
unsigned long long Grid::E6 = 0x0000000000080000;
unsigned long long Grid::F6 = 0x0000000000040000;
unsigned long long Grid::G6 = 0x0000000000020000;
unsigned long long Grid::H6 = 0x0000000000010000;

unsigned long long Grid::A7 = 0x0000000000008000;
unsigned long long Grid::B7 = 0x0000000000004000;
unsigned long long Grid::C7 = 0x0000000000002000;
unsigned long long Grid::D7 = 0x0000000000001000;
unsigned long long Grid::E7 = 0x0000000000000800;
unsigned long long Grid::F7 = 0x0000000000000400;
unsigned long long Grid::G7 = 0x0000000000000200;
unsigned long long Grid::H7 = 0x0000000000000100;

unsigned long long Grid::A8 = 0x0000000000000080;
unsigned long long Grid::B8 = 0x0000000000000040;
unsigned long long Grid::C8 = 0x0000000000000020;
unsigned long long Grid::D8 = 0x0000000000000010;
unsigned long long Grid::E8 = 0x0000000000000008;
unsigned long long Grid::F8 = 0x0000000000000004;
unsigned long long Grid::G8 = 0x0000000000000002;
unsigned long long Grid::H8 = 0x0000000000000001;

unsigned long long Grid::Row1 = A1 | B1 | C1 | D1 | E1 | F1 | G1 | H1;

unsigned long long Grid::Row2 = A2 | B2 | C2 | D2 | E2 | F2 | G2 | H2;

unsigned long long Grid::Row3 = A3 | B3 | C3 | D3 | E3 | F3 | G3 | H3;

unsigned long long Grid::Row4 = A4 | B4 | C4 | D4 | E4 | F4 | G4 | H4;

unsigned long long Grid::Row5 = A5 | B5 | C5 | D5 | E5 | F5 | G5 | H5;

unsigned long long Grid::Row6 = A6 | B6 | C6 | D6 | E6 | F6 | G6 | H6;

unsigned long long Grid::Row7 = A7 | B7 | C7 | D7 | E7 | F7 | G7 | H7;

unsigned long long Grid::Row8 = A8 | B8 | C8 | D8 | E8 | F8 | G8 | H8;

unsigned long long Grid::ColA = A1 | A2 | A3 | A4 | A5 | A6 | A7 | A8;

unsigned long long Grid::ColB = B1 | B2 | B3 | B4 | B5 | B6 | B7 | B8;

unsigned long long Grid::ColC = C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8;

unsigned long long Grid::ColD = D1 | D2 | D3 | D4 | D5 | D6 | D7 | D8;

unsigned long long Grid::ColE = E1 | E2 | E3 | E4 | E5 | E6 | E7 | E8;

unsigned long long Grid::ColF = F1 | F2 | F3 | F4 | F5 | F6 | F7 | F8;

unsigned long long Grid::ColG = G1 | G2 | G3 | G4 | G5 | G6 | G7 | G8;

unsigned long long Grid::ColH = H1 | H2 | H3 | H4 | H5 | H6 | H7 | H8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakthrough_AI
{
    /// <summary>
    /// Contains constants that return predetermined spaces for ease of use.
    /// </summary>
    public class Grid
    {
        public const ulong A1 = 0x8000000000000000;
        public const ulong B1 = 0x4000000000000000;
        public const ulong C1 = 0x2000000000000000;
        public const ulong D1 = 0x1000000000000000;
        public const ulong E1 = 0x0800000000000000;
        public const ulong F1 = 0x0400000000000000;
        public const ulong G1 = 0x0200000000000000;
        public const ulong H1 = 0x0100000000000000;

        public const ulong A2 = 0x0080000000000000;
        public const ulong B2 = 0x0040000000000000;
        public const ulong C2 = 0x0020000000000000;
        public const ulong D2 = 0x0010000000000000;
        public const ulong E2 = 0x0008000000000000;
        public const ulong F2 = 0x0004000000000000;
        public const ulong G2 = 0x0002000000000000;
        public const ulong H2 = 0x0001000000000000;

        public const ulong A3 = 0x0000800000000000;
        public const ulong B3 = 0x0000400000000000;
        public const ulong C3 = 0x0000200000000000;
        public const ulong D3 = 0x0000100000000000;
        public const ulong E3 = 0x0000080000000000;
        public const ulong F3 = 0x0000040000000000;
        public const ulong G3 = 0x0000020000000000;
        public const ulong H3 = 0x0000010000000000;

        public const ulong A4 = 0x0000008000000000;
        public const ulong B4 = 0x0000004000000000;
        public const ulong C4 = 0x0000002000000000;
        public const ulong D4 = 0x0000001000000000;
        public const ulong E4 = 0x0000000800000000;
        public const ulong F4 = 0x0000000400000000;
        public const ulong G4 = 0x0000000200000000;
        public const ulong H4 = 0x0000000100000000;

        public const ulong A5 = 0x0000000080000000;
        public const ulong B5 = 0x0000000040000000;
        public const ulong C5 = 0x0000000020000000;
        public const ulong D5 = 0x0000000010000000;
        public const ulong E5 = 0x0000000008000000;
        public const ulong F5 = 0x0000000004000000;
        public const ulong G5 = 0x0000000002000000;
        public const ulong H5 = 0x0000000001000000;

        public const ulong A6 = 0x0000000000800000;
        public const ulong B6 = 0x0000000000400000;
        public const ulong C6 = 0x0000000000200000;
        public const ulong D6 = 0x0000000000100000;
        public const ulong E6 = 0x0000000000080000;
        public const ulong F6 = 0x0000000000040000;
        public const ulong G6 = 0x0000000000020000;
        public const ulong H6 = 0x0000000000010000;

        public const ulong A7 = 0x0000000000008000;
        public const ulong B7 = 0x0000000000004000;
        public const ulong C7 = 0x0000000000002000;
        public const ulong D7 = 0x0000000000001000;
        public const ulong E7 = 0x0000000000000800;
        public const ulong F7 = 0x0000000000000400;
        public const ulong G7 = 0x0000000000000200;
        public const ulong H7 = 0x0000000000000100;

        public const ulong A8 = 0x0000000000000080;
        public const ulong B8 = 0x0000000000000040;
        public const ulong C8 = 0x0000000000000020;
        public const ulong D8 = 0x0000000000000010;
        public const ulong E8 = 0x0000000000000008;
        public const ulong F8 = 0x0000000000000004;
        public const ulong G8 = 0x0000000000000002;
        public const ulong H8 = 0x0000000000000001;
        
        public const ulong Row1 = A1 | B1 | C1 | D1
                                | E1 | F1 | G1 | H1;

        public const ulong Row2 = A2 | B2 | C2 | D2
                                | E2 | F2 | G2 | H2;

        public const ulong Row3 = A3 | B3 | C3 | D3
                                | E3 | F3 | G3 | H3;

        public const ulong Row4 = A4 | B4 | C4 | D4
                                | E4 | F4 | G4 | H4;

        public const ulong Row5 = A5 | B5 | C5 | D5
                                | E5 | F5 | G5 | H5;

        public const ulong Row6 = A6 | B6 | C6 | D6
                                | E6 | F6 | G6 | H6;

        public const ulong Row7 = A7 | B7 | C7 | D7
                                | E7 | F7 | G7 | H7;

        public const ulong Row8 = A8 | B8 | C8 | D8
                                | E8 | F8 | G8 | H8;
        
        public const ulong ColA = A1 | A2 | A3 | A4
                                | A5 | A6 | A7 | A8;

        public const ulong ColB = B1 | B2 | B3 | B4
                                | B5 | B6 | B7 | B8;

        public const ulong ColC = C1 | C2 | C3 | C4
                                | C5 | C6 | C7 | C8;

        public const ulong ColD = D1 | D2 | D3 | D4
                                | D5 | D6 | D7 | D8;

        public const ulong ColE = E1 | E2 | E3 | E4
                                | E5 | E6 | E7 | E8;

        public const ulong ColF = F1 | F2 | F3 | F4
                                | F5 | F6 | F7 | F8;

        public const ulong ColG = G1 | G2 | G3 | G4
                                | G5 | G6 | G7 | G8;

        public const ulong ColH = H1 | H2 | H3 | H4
                                | H5 | H6 | H7 | H8;
    }
}

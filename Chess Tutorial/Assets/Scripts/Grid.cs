using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakthrough_AI
{
    /// <summary>
    /// Contains constants and methods that return predetermined spaces.
    /// </summary>
    public class Grid
    {

        public class Squares
        {
            public static ulong[,] SquaresArray = new ulong[8,8]
            {
                { A1, B1, C1, D1, E1, F1, G1, H1, },
                { A2, B2, C2, D2, E2, F2, G2, H2, },
                { A3, B3, C3, D3, E3, F3, G3, H3, },
                { A4, B4, C4, D4, E4, F4, G4, H4, },
                { A5, B5, C5, D5, E5, F5, G5, H5, },
                { A6, B6, C6, D6, E6, F6, G6, H6, },
                { A7, B7, C7, D7, E7, F7, G7, H7, },
                { A8, B8, C8, D8, E8, F8, G8, H8, },
            };

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

            public static ulong[] CurrentSquare = new ulong[64]
            {
               Squares.H8, Squares.G8, Squares.F8, Squares.E8, Squares.D8, Squares.C8, Squares.B8, Squares.A8,
               Squares.H7, Squares.G7, Squares.F7, Squares.E7, Squares.D7, Squares.C7, Squares.B7, Squares.A7,
               Squares.H6, Squares.G6, Squares.F6, Squares.E6, Squares.D6, Squares.C6, Squares.B6, Squares.A6,
               Squares.H5, Squares.G5, Squares.F5, Squares.E5, Squares.D5, Squares.C5, Squares.B5, Squares.A5,
               Squares.H4, Squares.G4, Squares.F4, Squares.E4, Squares.D4, Squares.C4, Squares.B4, Squares.A4,
               Squares.H3, Squares.G3, Squares.F3, Squares.E3, Squares.D3, Squares.C3, Squares.B3, Squares.A3,
               Squares.H2, Squares.G2, Squares.F2, Squares.E2, Squares.D2, Squares.C2, Squares.B2, Squares.A2,
               Squares.H1, Squares.G1, Squares.F1, Squares.E1, Squares.D1, Squares.C1, Squares.B1, Squares.A1,
            };
        }

        public class Rows
        {
            public const ulong Row1 = Squares.A1 | Squares.B1 | Squares.C1 | Squares.D1
                                    | Squares.E1 | Squares.F1 | Squares.G1 | Squares.H1;

            public const ulong Row2 = Squares.A2 | Squares.B2 | Squares.C2 | Squares.D2
                                    | Squares.E2 | Squares.F2 | Squares.G2 | Squares.H2;

            public const ulong Row3 = Squares.A3 | Squares.B3 | Squares.C3 | Squares.D3
                                    | Squares.E3 | Squares.F3 | Squares.G3 | Squares.H3;

            public const ulong Row4 = Squares.A4 | Squares.B4 | Squares.C4 | Squares.D4
                                    | Squares.E4 | Squares.F4 | Squares.G4 | Squares.H4;

            public const ulong Row5 = Squares.A5 | Squares.B5 | Squares.C5 | Squares.D5
                                    | Squares.E5 | Squares.F5 | Squares.G5 | Squares.H5;

            public const ulong Row6 = Squares.A6 | Squares.B6 | Squares.C6 | Squares.D6
                                    | Squares.E6 | Squares.F6 | Squares.G6 | Squares.H6;

            public const ulong Row7 = Squares.A7 | Squares.B7 | Squares.C7 | Squares.D7
                                    | Squares.E7 | Squares.F7 | Squares.G7 | Squares.H7;

            public const ulong Row8 = Squares.A8 | Squares.B8 | Squares.C8 | Squares.D8
                                    | Squares.E8 | Squares.F8 | Squares.G8 | Squares.H8;
        }

        public class Columns
        {
            public const ulong ColA = Squares.A1 | Squares.A2 | Squares.A3 | Squares.A4
                                    | Squares.A5 | Squares.A6 | Squares.A7 | Squares.A8;

            public const ulong ColB = Squares.B1 | Squares.B2 | Squares.B3 | Squares.B4
                                    | Squares.B5 | Squares.B6 | Squares.B7 | Squares.B8;

            public const ulong ColC = Squares.C1 | Squares.C2 | Squares.C3 | Squares.C4
                                    | Squares.C5 | Squares.C6 | Squares.C7 | Squares.C8;

            public const ulong ColD = Squares.D1 | Squares.D2 | Squares.D3 | Squares.D4
                                    | Squares.D5 | Squares.D6 | Squares.D7 | Squares.D8;

            public const ulong ColE = Squares.E1 | Squares.E2 | Squares.E3 | Squares.E4
                                    | Squares.E5 | Squares.E6 | Squares.E7 | Squares.E8;

            public const ulong ColF = Squares.F1 | Squares.F2 | Squares.F3 | Squares.F4
                                    | Squares.F5 | Squares.F6 | Squares.F7 | Squares.F8;

            public const ulong ColG = Squares.G1 | Squares.G2 | Squares.G3 | Squares.G4
                                    | Squares.G5 | Squares.G6 | Squares.G7 | Squares.G8;

            public const ulong ColH = Squares.H1 | Squares.H2 | Squares.H3 | Squares.H4
                                    | Squares.H5 | Squares.H6 | Squares.H7 | Squares.H8;
        }

        public class Patterns
        {
            public const ulong WhiteStart = Rows.Row1 | Rows.Row2;
            public const ulong BlackStart = Rows.Row7 | Rows.Row8;
        }

        public class WhiteMasks
        {
            public static ulong[] Forward = new ulong[64]
            {
               0, 0, 0, 0, 0, 0, 0, 0,
               Squares.H8, Squares.G8, Squares.F8, Squares.E8, Squares.D8, Squares.C8, Squares.B8, Squares.A8,
               Squares.H7, Squares.G7, Squares.F7, Squares.E7, Squares.D7, Squares.C7, Squares.B7, Squares.A7,
               Squares.H6, Squares.G6, Squares.F6, Squares.E6, Squares.D6, Squares.C6, Squares.B6, Squares.A6,
               Squares.H5, Squares.G5, Squares.F5, Squares.E5, Squares.D5, Squares.C5, Squares.B5, Squares.A5,
               Squares.H4, Squares.G4, Squares.F4, Squares.E4, Squares.D4, Squares.C4, Squares.B4, Squares.A4,
               Squares.H3, Squares.G3, Squares.F3, Squares.E3, Squares.D3, Squares.C3, Squares.B3, Squares.A3,
               Squares.H2, Squares.G2, Squares.F2, Squares.E2, Squares.D2, Squares.C2, Squares.B2, Squares.A2,
            };

            public static ulong[] EastAttack = new ulong[64]
            {
               0, 0, 0, 0, 0, 0, 0, 0,
               0, Squares.H8, Squares.G8, Squares.F8, Squares.E8, Squares.D8, Squares.C8, Squares.B8,
               0, Squares.H7, Squares.G7, Squares.F7, Squares.E7, Squares.D7, Squares.C7, Squares.B7,
               0, Squares.H6, Squares.G6, Squares.F6, Squares.E6, Squares.D6, Squares.C6, Squares.B6,
               0, Squares.H5, Squares.G5, Squares.F5, Squares.E5, Squares.D5, Squares.C5, Squares.B5,
               0, Squares.H4, Squares.G4, Squares.F4, Squares.E4, Squares.D4, Squares.C4, Squares.B4,
               0, Squares.H3, Squares.G3, Squares.F3, Squares.E3, Squares.D3, Squares.C3, Squares.B3,
               0, Squares.H2, Squares.G2, Squares.F2, Squares.E2, Squares.D2, Squares.C2, Squares.B2,
            };

            public static ulong[] WestAttack = new ulong[64]
            {
               0, 0, 0, 0, 0, 0, 0, 0,
               Squares.G8, Squares.F8, Squares.E8, Squares.D8, Squares.C8, Squares.B8, Squares.A8, 0,
               Squares.G7, Squares.F7, Squares.E7, Squares.D7, Squares.C7, Squares.B7, Squares.A7, 0,
               Squares.G6, Squares.F6, Squares.E6, Squares.D6, Squares.C6, Squares.B6, Squares.A6, 0,
               Squares.G5, Squares.F5, Squares.E5, Squares.D5, Squares.C5, Squares.B5, Squares.A5, 0,
               Squares.G4, Squares.F4, Squares.E4, Squares.D4, Squares.C4, Squares.B4, Squares.A4, 0,
               Squares.G3, Squares.F3, Squares.E3, Squares.D3, Squares.C3, Squares.B3, Squares.A3, 0,
               Squares.G2, Squares.F2, Squares.E2, Squares.D2, Squares.C2, Squares.B2, Squares.A2, 0,
            };
        }

        public class BlackMasks
        {
            public static ulong[] Forward = new ulong[64]
             {
               Squares.H7, Squares.G7, Squares.F7, Squares.E7, Squares.D7, Squares.C7, Squares.B7, Squares.A7,
               Squares.H6, Squares.G6, Squares.F6, Squares.E6, Squares.D6, Squares.C6, Squares.B6, Squares.A6,
               Squares.H5, Squares.G5, Squares.F5, Squares.E5, Squares.D5, Squares.C5, Squares.B5, Squares.A5,
               Squares.H4, Squares.G4, Squares.F4, Squares.E4, Squares.D4, Squares.C4, Squares.B4, Squares.A4,
               Squares.H3, Squares.G3, Squares.F3, Squares.E3, Squares.D3, Squares.C3, Squares.B3, Squares.A3,
               Squares.H2, Squares.G2, Squares.F2, Squares.E2, Squares.D2, Squares.C2, Squares.B2, Squares.A2,
               Squares.H1, Squares.G1, Squares.F1, Squares.E1, Squares.D1, Squares.C1, Squares.B1, Squares.A1,
               0, 0, 0, 0, 0, 0, 0, 0,
             };

            public static ulong[] EastAttack = new ulong[64]
            {

                0, Squares.H7, Squares.G7, Squares.F7, Squares.E7, Squares.D7, Squares.C7, Squares.B7,
                0, Squares.H6, Squares.G6, Squares.F6, Squares.E6, Squares.D6, Squares.C6, Squares.B6,
                0, Squares.H5, Squares.G5, Squares.F5, Squares.E5, Squares.D5, Squares.C5, Squares.B5,
                0, Squares.H4, Squares.G4, Squares.F4, Squares.E4, Squares.D4, Squares.C4, Squares.B4,
                0, Squares.H3, Squares.G3, Squares.F3, Squares.E3, Squares.D3, Squares.C3, Squares.B3,
                0, Squares.H2, Squares.G2, Squares.F2, Squares.E2, Squares.D2, Squares.C2, Squares.B2,
                0, Squares.H1, Squares.G1, Squares.F1, Squares.E1, Squares.D1, Squares.C1, Squares.B1,
                0, 0, 0, 0, 0, 0, 0, 0,
            };

            public static ulong[] WestAttack = new ulong[64]
            {
               Squares.G7, Squares.F7, Squares.E7, Squares.D7, Squares.C7, Squares.B7, Squares.A7, 0,
               Squares.G6, Squares.F6, Squares.E6, Squares.D6, Squares.C6, Squares.B6, Squares.A6, 0,
               Squares.G5, Squares.F5, Squares.E5, Squares.D5, Squares.C5, Squares.B5, Squares.A5, 0,
               Squares.G4, Squares.F4, Squares.E4, Squares.D4, Squares.C4, Squares.B4, Squares.A4, 0,
               Squares.G3, Squares.F3, Squares.E3, Squares.D3, Squares.C3, Squares.B3, Squares.A3, 0,
               Squares.G2, Squares.F2, Squares.E2, Squares.D2, Squares.C2, Squares.B2, Squares.A2, 0,
               Squares.G1, Squares.F1, Squares.E1, Squares.D1, Squares.C1, Squares.B1, Squares.A1, 0,
               0, 0, 0, 0, 0, 0, 0, 0,
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakthrough_AI
{
    /// <summary>
    /// Contains Mask arrays for indexes generated from BitsMagic.  By plugging an index into 
    /// these arrays, you can quickly grab the movement options for any piece.
    /// </summary>
    public class Masks
    {
        public class WhiteMasks
        {
            public static ulong[] Forward = new ulong[64]
            {
               0, 0, 0, 0, 0, 0, 0, 0,
               Grid.H8, Grid.G8, Grid.F8, Grid.E8, Grid.D8, Grid.C8, Grid.B8, Grid.A8,
               Grid.H7, Grid.G7, Grid.F7, Grid.E7, Grid.D7, Grid.C7, Grid.B7, Grid.A7,
               Grid.H6, Grid.G6, Grid.F6, Grid.E6, Grid.D6, Grid.C6, Grid.B6, Grid.A6,
               Grid.H5, Grid.G5, Grid.F5, Grid.E5, Grid.D5, Grid.C5, Grid.B5, Grid.A5,
               Grid.H4, Grid.G4, Grid.F4, Grid.E4, Grid.D4, Grid.C4, Grid.B4, Grid.A4,
               Grid.H3, Grid.G3, Grid.F3, Grid.E3, Grid.D3, Grid.C3, Grid.B3, Grid.A3,
               Grid.H2, Grid.G2, Grid.F2, Grid.E2, Grid.D2, Grid.C2, Grid.B2, Grid.A2,
            };

            public static ulong[] EastAttack = new ulong[64]
            {
               0, 0, 0, 0, 0, 0, 0, 0,
               0, Grid.H8, Grid.G8, Grid.F8, Grid.E8, Grid.D8, Grid.C8, Grid.B8,
               0, Grid.H7, Grid.G7, Grid.F7, Grid.E7, Grid.D7, Grid.C7, Grid.B7,
               0, Grid.H6, Grid.G6, Grid.F6, Grid.E6, Grid.D6, Grid.C6, Grid.B6,
               0, Grid.H5, Grid.G5, Grid.F5, Grid.E5, Grid.D5, Grid.C5, Grid.B5,
               0, Grid.H4, Grid.G4, Grid.F4, Grid.E4, Grid.D4, Grid.C4, Grid.B4,
               0, Grid.H3, Grid.G3, Grid.F3, Grid.E3, Grid.D3, Grid.C3, Grid.B3,
               0, Grid.H2, Grid.G2, Grid.F2, Grid.E2, Grid.D2, Grid.C2, Grid.B2,
            };

            public static ulong[] WestAttack = new ulong[64]
            {
               0, 0, 0, 0, 0, 0, 0, 0,
               Grid.G8, Grid.F8, Grid.E8, Grid.D8, Grid.C8, Grid.B8, Grid.A8, 0,
               Grid.G7, Grid.F7, Grid.E7, Grid.D7, Grid.C7, Grid.B7, Grid.A7, 0,
               Grid.G6, Grid.F6, Grid.E6, Grid.D6, Grid.C6, Grid.B6, Grid.A6, 0,
               Grid.G5, Grid.F5, Grid.E5, Grid.D5, Grid.C5, Grid.B5, Grid.A5, 0,
               Grid.G4, Grid.F4, Grid.E4, Grid.D4, Grid.C4, Grid.B4, Grid.A4, 0,
               Grid.G3, Grid.F3, Grid.E3, Grid.D3, Grid.C3, Grid.B3, Grid.A3, 0,
               Grid.G2, Grid.F2, Grid.E2, Grid.D2, Grid.C2, Grid.B2, Grid.A2, 0,
            };
        }

        public class BlackMasks
        {
            public static ulong[] Forward = new ulong[64]
             {
               Grid.H7, Grid.G7, Grid.F7, Grid.E7, Grid.D7, Grid.C7, Grid.B7, Grid.A7,
               Grid.H6, Grid.G6, Grid.F6, Grid.E6, Grid.D6, Grid.C6, Grid.B6, Grid.A6,
               Grid.H5, Grid.G5, Grid.F5, Grid.E5, Grid.D5, Grid.C5, Grid.B5, Grid.A5,
               Grid.H4, Grid.G4, Grid.F4, Grid.E4, Grid.D4, Grid.C4, Grid.B4, Grid.A4,
               Grid.H3, Grid.G3, Grid.F3, Grid.E3, Grid.D3, Grid.C3, Grid.B3, Grid.A3,
               Grid.H2, Grid.G2, Grid.F2, Grid.E2, Grid.D2, Grid.C2, Grid.B2, Grid.A2,
               Grid.H1, Grid.G1, Grid.F1, Grid.E1, Grid.D1, Grid.C1, Grid.B1, Grid.A1,
               0, 0, 0, 0, 0, 0, 0, 0,
             };

            public static ulong[] EastAttack = new ulong[64]
            {

                0, Grid.H7, Grid.G7, Grid.F7, Grid.E7, Grid.D7, Grid.C7, Grid.B7,
                0, Grid.H6, Grid.G6, Grid.F6, Grid.E6, Grid.D6, Grid.C6, Grid.B6,
                0, Grid.H5, Grid.G5, Grid.F5, Grid.E5, Grid.D5, Grid.C5, Grid.B5,
                0, Grid.H4, Grid.G4, Grid.F4, Grid.E4, Grid.D4, Grid.C4, Grid.B4,
                0, Grid.H3, Grid.G3, Grid.F3, Grid.E3, Grid.D3, Grid.C3, Grid.B3,
                0, Grid.H2, Grid.G2, Grid.F2, Grid.E2, Grid.D2, Grid.C2, Grid.B2,
                0, Grid.H1, Grid.G1, Grid.F1, Grid.E1, Grid.D1, Grid.C1, Grid.B1,
                0, 0, 0, 0, 0, 0, 0, 0,
            };

            public static ulong[] WestAttack = new ulong[64]
            {
               Grid.G7, Grid.F7, Grid.E7, Grid.D7, Grid.C7, Grid.B7, Grid.A7, 0,
               Grid.G6, Grid.F6, Grid.E6, Grid.D6, Grid.C6, Grid.B6, Grid.A6, 0,
               Grid.G5, Grid.F5, Grid.E5, Grid.D5, Grid.C5, Grid.B5, Grid.A5, 0,
               Grid.G4, Grid.F4, Grid.E4, Grid.D4, Grid.C4, Grid.B4, Grid.A4, 0,
               Grid.G3, Grid.F3, Grid.E3, Grid.D3, Grid.C3, Grid.B3, Grid.A3, 0,
               Grid.G2, Grid.F2, Grid.E2, Grid.D2, Grid.C2, Grid.B2, Grid.A2, 0,
               Grid.G1, Grid.F1, Grid.E1, Grid.D1, Grid.C1, Grid.B1, Grid.A1, 0,
               0, 0, 0, 0, 0, 0, 0, 0,
            };
        }

        public class OrientationMasks
        {
            public static int[] CurrentColumn = new int[64]
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

            public static int[] CurrentRow = new int[64]
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

            public static ulong[] CurrentSquare = new ulong[64]
            {
                Grid.H8, Grid.G8, Grid.F8, Grid.E8, Grid.D8, Grid.C8, Grid.B8, Grid.A8,
                Grid.H7, Grid.G7, Grid.F7, Grid.E7, Grid.D7, Grid.C7, Grid.B7, Grid.A7,
                Grid.H6, Grid.G6, Grid.F6, Grid.E6, Grid.D6, Grid.C6, Grid.B6, Grid.A6,
                Grid.H5, Grid.G5, Grid.F5, Grid.E5, Grid.D5, Grid.C5, Grid.B5, Grid.A5,
                Grid.H4, Grid.G4, Grid.F4, Grid.E4, Grid.D4, Grid.C4, Grid.B4, Grid.A4,
                Grid.H3, Grid.G3, Grid.F3, Grid.E3, Grid.D3, Grid.C3, Grid.B3, Grid.A3,
                Grid.H2, Grid.G2, Grid.F2, Grid.E2, Grid.D2, Grid.C2, Grid.B2, Grid.A2,
                Grid.H1, Grid.G1, Grid.F1, Grid.E1, Grid.D1, Grid.C1, Grid.B1, Grid.A1,
            };

            public static ulong[] Above = new ulong[64]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 
                Grid.H8, Grid.G8, Grid.F8, Grid.E8, Grid.D8, Grid.C8, Grid.B8, Grid.A8,
                Grid.H7, Grid.G7, Grid.F7, Grid.E7, Grid.D7, Grid.C7, Grid.B7, Grid.A7,
                Grid.H6, Grid.G6, Grid.F6, Grid.E6, Grid.D6, Grid.C6, Grid.B6, Grid.A6,
                Grid.H5, Grid.G5, Grid.F5, Grid.E5, Grid.D5, Grid.C5, Grid.B5, Grid.A5,
                Grid.H4, Grid.G4, Grid.F4, Grid.E4, Grid.D4, Grid.C4, Grid.B4, Grid.A4,
                Grid.H3, Grid.G3, Grid.F3, Grid.E3, Grid.D3, Grid.C3, Grid.B3, Grid.A3,
                Grid.H2, Grid.G2, Grid.F2, Grid.E2, Grid.D2, Grid.C2, Grid.B2, Grid.A2,
            };

            public static ulong[] RightOf = new ulong[64]
            {
                0, Grid.H8, Grid.G8, Grid.F8, Grid.E8, Grid.D8, Grid.C8, Grid.B8,
                0, Grid.H7, Grid.G7, Grid.F7, Grid.E7, Grid.D7, Grid.C7, Grid.B7,
                0, Grid.H6, Grid.G6, Grid.F6, Grid.E6, Grid.D6, Grid.C6, Grid.B6,
                0, Grid.H5, Grid.G5, Grid.F5, Grid.E5, Grid.D5, Grid.C5, Grid.B5,
                0, Grid.H4, Grid.G4, Grid.F4, Grid.E4, Grid.D4, Grid.C4, Grid.B4,
                0, Grid.H3, Grid.G3, Grid.F3, Grid.E3, Grid.D3, Grid.C3, Grid.B3,
                0, Grid.H2, Grid.G2, Grid.F2, Grid.E2, Grid.D2, Grid.C2, Grid.B2,
                0, Grid.H1, Grid.G1, Grid.F1, Grid.E1, Grid.D1, Grid.C1, Grid.B1,
            };
        }
       
    }
}

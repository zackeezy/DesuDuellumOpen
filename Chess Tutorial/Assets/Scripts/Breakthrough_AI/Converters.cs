using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakthrough_AI
{
    /// <summary>
    /// Contains the methods used to transform token coordinates into squares, 
    /// and Bitsmagic indeces back into coordinates.
    /// </summary>
    public class Converters
    {
        /// <summary>
        /// Used for converting (x,y) coordinates into a hex space.
        /// </summary>
        public static ulong[,] Squares = new ulong[8, 8]
        {
            { Grid.A1, Grid.B1, Grid.C1, Grid.D1, Grid.E1, Grid.F1, Grid.G1, Grid.H1, },
            { Grid.A2, Grid.B2, Grid.C2, Grid.D2, Grid.E2, Grid.F2, Grid.G2, Grid.H2, },
            { Grid.A3, Grid.B3, Grid.C3, Grid.D3, Grid.E3, Grid.F3, Grid.G3, Grid.H3, },
            { Grid.A4, Grid.B4, Grid.C4, Grid.D4, Grid.E4, Grid.F4, Grid.G4, Grid.H4, },
            { Grid.A5, Grid.B5, Grid.C5, Grid.D5, Grid.E5, Grid.F5, Grid.G5, Grid.H5, },
            { Grid.A6, Grid.B6, Grid.C6, Grid.D6, Grid.E6, Grid.F6, Grid.G6, Grid.H6, },
            { Grid.A7, Grid.B7, Grid.C7, Grid.D7, Grid.E7, Grid.F7, Grid.G7, Grid.H7, },
            { Grid.A8, Grid.B8, Grid.C8, Grid.D8, Grid.E8, Grid.F8, Grid.G8, Grid.H8, },
        };

        /// <summary>
        /// Used for turning a BitsMagic index into an X coordinate.  Used in conjunction
        /// with YCoordinate.
        /// </summary>
        public static int XCoordinate(int location)
        {
            int xcoord = 0;

            if ((Masks.CurrentSquare[location] & Grid.ColA) != 0)
            {
                xcoord = 0;
            }
            if ((Masks.CurrentSquare[location] & Grid.ColB) != 0)
            {
                xcoord = 1;
            }
            if ((Masks.CurrentSquare[location] & Grid.ColC) != 0)
            {
                xcoord = 2;
            }
            if ((Masks.CurrentSquare[location] & Grid.ColD) != 0)
            {
                xcoord = 3;
            }
            if ((Masks.CurrentSquare[location] & Grid.ColE) != 0)
            {
                xcoord = 4;
            }
            if ((Masks.CurrentSquare[location] & Grid.ColF) != 0)
            {
                xcoord = 5;
            }
            if ((Masks.CurrentSquare[location] & Grid.ColG) != 0)
            {
                xcoord = 6;
            }
            if ((Masks.CurrentSquare[location] & Grid.ColH) != 0)
            {
                xcoord = 7;
            }

            return xcoord;
        }

        /// <summary>
        /// Used for turning a BitsMagic index into an Y coordinate.  Used in conjunction
        /// with XCoordinate.
        /// </summary>
        public static int YCoordinate(int location)
        {
            int ycoord = 0;

            if ((Masks.CurrentSquare[location] & Grid.Row1) != 0)
            {
                ycoord = 0;
            }
            if ((Masks.CurrentSquare[location] & Grid.Row2) != 0)
            {
                ycoord = 1;
            }
            if ((Masks.CurrentSquare[location] & Grid.Row3) != 0)
            {
                ycoord = 2;
            }
            if ((Masks.CurrentSquare[location] & Grid.Row4) != 0)
            {
                ycoord = 3;
            }
            if ((Masks.CurrentSquare[location] & Grid.Row5) != 0)
            {
                ycoord = 4;
            }
            if ((Masks.CurrentSquare[location] & Grid.Row6) != 0)
            {
                ycoord = 5;
            }
            if ((Masks.CurrentSquare[location] & Grid.Row7) != 0)
            {
                ycoord = 6;
            }
            if ((Masks.CurrentSquare[location] & Grid.Row8) != 0)
            {
                ycoord = 7;
            }

            return ycoord;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakthrough_AI
{
    /// <summary>
    /// Uses Bitscanning and fancy maths to quickly enumerate through pieces.
    /// </summary>
    public class BitsMagic
    {
        /// <summary>
        /// The converter array used to turn the unique value given by the Bitscan into 
        /// the proper board index.
        /// </summary>
        private static int[] _index64 = new int[64]
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

        /// <summary>
        /// This function returns the least significant bit (LSB) of the given board.
        /// The internet can explain the maths much better than I, if you want 
        /// to look for it.
        /// </summary>
        public static int BitScanForward(ulong board)
        {
            //Visit ChessProgramming.com for explanations of BitScan Magic.

            const long debruijn64 = 0x03f79d71b4cb0a89;

            if (board == 0)
            {
                return -1;
            }

            return _index64[(int)(((ulong)((long)board & -(long)(board)) * debruijn64) >> 58)];
        }

        /// <summary>
        /// This method contains more fancy maths that will turn off the LSB it found.  Useful
        /// for iterating through all pieces.
        /// </summary>
        public static int BitScanForwardWithReset(ref ulong board)
        {
            int index = BitScanForward(board);
            board &= board - 1;
            return index;
        }
    }
}

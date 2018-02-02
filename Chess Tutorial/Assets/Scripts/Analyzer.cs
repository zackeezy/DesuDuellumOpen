using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Breakthrough_AI
{
    /// <summary>
    /// Contains outward facing methods that are used in the rest of the project for 
    /// obtaining AI moves.
    /// </summary>
    public class Analyzer
    {
        public PlayerColor AiColor
        {
            get
            {
                return _aiColor;
            }
            private set
            {
                _aiColor = value;
            }
        }
        private PlayerColor _aiColor;
        

        public Analyzer(PlayerColor aiColor)
        {
            _aiColor = aiColor;
        }

        //Get list of tokens, return origin coordinates, new coordinates.
        public string GetMove()
        {
            throw new NotImplementedException("GetMove is not implemented.");
        }

        /// <summary>
        /// Performs the basic tree search to find the best possible move.
        /// Built from pseudocode taken from https://en.wikipedia.org/wiki/Alpha%E2%80%93beta_pruning
        /// </summary>
        /// <returns></returns>
        private int AlphaBetaLoop(BitBoard node, int remainingDepth, int alpha, int beta, bool maximizingPlayer)
        {            
            if (remainingDepth == 0 || IsGameOver(node))
            {
                return Evaluate(node);
            }

            //TODO: Generate All Child Nodes.
            List<BitBoard> children = new List<BitBoard>();

            if (maximizingPlayer)
            {
                int value = Int32.MinValue;

                foreach (BitBoard child in children)
                {
                    value = Math.Max(value, AlphaBetaLoop(child, remainingDepth - 1, alpha, beta, false));
                    alpha = Math.Max(alpha, value);

                    if (beta < alpha) break;
                }

                return value;
            }
            else
            {
                int value = Int32.MaxValue;

                foreach (BitBoard child in children)
                {
                    value = Math.Min(value, AlphaBetaLoop(child, remainingDepth - 1, alpha, beta, true));
                    beta = Math.Min(beta, value);

                    if (beta < alpha) break;
                }

                return value;
            }

            throw new NotImplementedException();
        }

        private bool IsGameOver(BitBoard bitBoard)
        {
            throw new NotImplementedException();
        }

        private int Evaluate(BitBoard origin)
        {
            throw new NotImplementedException();
        }
    }
}

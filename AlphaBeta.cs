using System.Collections.Generic;
using Libby_Mancala;
using System.Diagnostics;

namespace Libby_Mancala
{
    class AlphaBeta
    {
        public static int NrEntries = 0;
        public static double Value(Board board, int depth, double alfa, double beta, Player player)
        {
            Trace.WriteLine("Enter alphabeta d = " + depth + " a = " + alfa + " b = " + beta + " P = " + player + 5);
            ++NrEntries;
            double value = 0.0;
            if (depth == 0)
            {
                value = board.heuristicValue();
            }
            else if (board.IsDraw()) //isDraw is if both mancala's contain equal number of stones
            {
                value = 0.0;
            }
            else
            {
                Player opponent = player == Player.MAX ? Player.MIN : Player.MAX;
                List<Board> positions = board.GenerateAllPositions(player);
                if (player == Player.MAX)
                {
                    foreach (Board nextPos in positions)
                    {
                        double thisVal = Value(nextPos, depth - 1, alfa, beta, opponent);
                        if (thisVal > alfa)
                        {
                            alfa = thisVal;
                        }
                        if (beta <= alfa)
                        {
                            break;
                        }

                    }
                    value = alfa;
                }
                else  // player == Player.MIN
                {
                    foreach (Board nextPos in positions)
                    {
                        double thisVal = Value(nextPos, depth - 1, alfa, beta, opponent);
                        if (thisVal < beta)
                        {
                            beta = thisVal;
                        }
                        if (beta <= alfa)
                        {
                            break;
                        }

                    }
                    value = beta;
                }
            }
            Trace.WriteLine("Exit alfabeta value = " + value + " depth " + depth + 5);
            return value;
        }
    }
}
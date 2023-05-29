// See https://aka.ms/new-console-template for more information
using Libby_Mancala;
using System.Diagnostics;
using System.Configuration;



namespace Libby_Mancala
{
    public class Program
    {
        static int MAX_DEPTH = 4;  // default value reset inside ProcessConfiguration
        public static readonly double MIN_VALUE = -1.0;
        public static readonly double MAX_VALUR = 1.0;


        static void Main(string[] args)
        {
            Board gameBoard = new Board();
            bool gameOver = false;

            while (!gameOver)
            {
                Console.WriteLine("\nI am thinking about my move now");
                double highVal = -2.0;
                Board bestMove = null;
                double alfa = -1.0;
                double beta = 1.0;
                List<Board> positions = gameBoard.GenerateAllPositions(Player.MAX);
                foreach (Board nextPos in positions)
                {
                    double thisVal = AlphaBeta.Value(nextPos, MAX_DEPTH - 1, alfa, beta, Player.MIN);
                    if (thisVal > highVal)
                    {
                        bestMove = nextPos;
                        highVal = thisVal;
                    }

                }

                if (highVal == -1)
                {
                    //bestMove = DesperationMove(gameBoard);
                    bestMove = positions[0];
                }
                Console.WriteLine($"My move is {gameBoard.whichMove(bestMove)+1}    (subj. value {highVal})");
                gameBoard = new Board(bestMove);
                gameBoard.showBoard();

                if (gameBoard.isWin(Player.MAX))
                {
                    Console.WriteLine("\n I win");
                    gameOver = true;
                }
                else
                {
                    Console.WriteLine("\nYour move");
                    int theirMove = UserInput.getInteger("Select column 1 - 6", 1, 6) - 1;
                    if (!gameBoard.isWin(Player.MIN))
                    {
                        gameBoard = gameBoard.MakeMove(Player.MIN, theirMove);
                        Console.WriteLine("");
                        gameBoard.showBoard();
                    }
                    else
                    {
                        Console.WriteLine("no can do  :-P\n\n");
                    }
                    if (gameBoard.isWin(Player.MIN))
                    {
                        Console.WriteLine("\n You win :-(");
                        gameOver = true;
                    }
                }
            }
            Console.WriteLine("nr of calls to AlphaBeta: " + AlphaBeta.NrEntries);
            Console.ReadKey();
        }


    }
}

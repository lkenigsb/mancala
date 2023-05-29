using System;
using System.Numerics;
using System.Reflection.PortableExecutable;

namespace Libby_Mancala
{
    class Board
    {
        //public static readonly int NR_ROWS = 2;
        //public static readonly int NR_COLS = 8; // includes the 'home bases'

        public int[] computer = new int[6];
        public int[] player = new int[6];
        public int compMancala { get; set; }
        public int playerMancala { get; set; }


        #region  Constructors 
        public Board()
        {
            for (int index = 0; index < computer.Length; index++)
            {
                computer[index] = 4;
                player[index] = 4;
            }
            compMancala = 0;
            playerMancala = 0;
        }
        #endregion

        public Board(Board boardOther)
        {
            for (int index = 0; index < computer.Length; index++)
            {
                computer[index] = boardOther.computer[index];
                player[index] = boardOther.player[index];
            }
            compMancala = boardOther.compMancala;
            playerMancala = boardOther.playerMancala;
        }

        /*----------------------------------------
         * display the current status of the board
         ----------------------------------------*/
        public void showBoard()
        {
            //display computer mancala
            Console.Write(compMancala + "   |");
            //display computer side
            for (int computerRow = 0; computerRow < computer.Length; computerRow++)
            {
                Console.Write(computer[computerRow] + "|");
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("    |");
            //display player side
            for (int playerRow = 0; playerRow < player.Length; playerRow++)
            {
                Console.Write(player[playerRow] + "|");
            }
            //display player mancala
            Console.Write("   " + playerMancala);
        }


        /*-----------------------------------------------------------------
         * make the specified move
         *
         * INPUT:   Player who    - MAX or MIN
         *          int    column - into which column was the next piece dropped
         *          
         * OUTPU:  Board - new board configuration after the move was made
         ----------------------------------------------------------------*/
        public Board MakeMove(Player who, int column)
        {
            Board moveMade = new Board(this);
            /*
             * WITH DR. K IDEA:
             * while numStones > 0
                    * iterate through the array, 
                    * when reach index 0 (for cmputer)
                    * or index 6 (for user) 
                        * then reset index 
                        * 
             * created alias array to change based on index
             */
            if (who == Player.MAX)
            {
                int stones = computer[column];
                int[] tempArr = computer;

                tempArr[column] = 0;
                column--;

                while (stones > 0)
                {
                    if (column == 6)
                    {
                        column = 5;
                        tempArr = computer;
                    }
                    else if (column == -1)
                    {
                        compMancala++;
                        stones--;
                        column = 0;
                        tempArr = player;

                    }
                    else //in players board
                    {
                        tempArr[column]++;
                        stones--;
                        if (tempArr == computer)
                        {
                            column--;
                        }
                        else //tempArr = player
                        {
                            column++; ;
                        }
                    }
                }
                //at this point completed dropping of stones
                if (tempArr == computer)
                {
                    if (tempArr[column + 1] == 1)
                    {
                        compMancala += player[column + 1];
                        compMancala += tempArr[column + 1];
                        player[column + 1] = 0;
                        tempArr[column + 1] = 0;
                    }
                }
                moveMade = this;
            }
            
            else // (who == Player.MIN)
            {
                int stones = player[column];
                int[] tempArr = player;

                tempArr[column] = 0;
                column++;

                while (stones > 0)
                {
                    if (column == 6)
                    {
                        playerMancala++;
                        stones--;
                        column = 5;
                        tempArr = computer;
                    }
                    else if (column == -1)
                    {
                        column = 0;
                        tempArr = player;

                    }
                    else //in players board
                    {
                        tempArr[column]++;
                        stones--;
                        if (tempArr == player)
                        {
                            column++;
                        }
                        else //tempArr = computer
                        {
                            column--; 
                        }
                    }
                }
                //at this point completed dropping of stones
                if (tempArr == player)
                {
                    if (tempArr[column - 1] == 1)
                    {
                        playerMancala += computer[column - 1];
                        playerMancala += tempArr[column - 1];
                        tempArr[column - 1] = 0;
                        computer[column - 1] = 0;
                    }
                }
                moveMade = this;
            }
            return moveMade;
        }

        public List<Board> GenerateAllPositions(Player who)
        {
            List<Board> boards = new List<Board>();


            if (who == Player.MAX)
            {
                for (int i = 0; i < computer.Length; i++)
                {
                    Board tempBoard = new Board(this);
                    tempBoard.MakeMove(who, i);
                    boards.Add(tempBoard);
                }
            }
            else
            {
                for (int i = 0; i < player.Length; i++)
                {
                    Board tempBoard = new Board(this);
                    tempBoard.MakeMove(who, i);
                    boards.Add(tempBoard);
                }
            }
            

            return boards;
        }

        public bool IsDraw()
        {
            return compMancala == playerMancala;
        }

        #region "Check if the board has a winner" 
        /*----------------------------------------------------------
         * is the given player the winner
         *
         * INPUT:   Player player: MAX or MIN
         * OUTPUT"  bool   decision
         ---------------------------------------------------------*/
        public bool isWin(Player whichPlayer)
        {
            bool retVal = false;
            bool compIsEmpty = true;
            bool userIsEmpty = true;

            if (whichPlayer == Player.MAX) //looking at computer 
            {
                //check if max players are empty
                for (int index = 1; index < computer.Length; index++)
                {
                    if (computer[index] != 0)
                    {
                        compIsEmpty = false;
                        break;
                    }
                }

                //add other players stones to this players mancala
                if (compIsEmpty)
                {
                    int userStones = 0;
                    for (int user = 0; user < player.Length; user++)
                    {
                        userStones += player[user];
                    }
                    compMancala += userStones;
                    //Now check who wins
                    if (compMancala > playerMancala)
                    {
                        retVal = true;
                    }
                    else if (compMancala < playerMancala)
                    {
                        retVal = false;
                    }
                }
            }
            else  //looking at player / user
            {
                for (int index = 0; index < player.Length - 1; index++)
                {
                    if (player[index] != 0)
                    {
                        userIsEmpty = false;
                        break;
                    }
                }
                //if all user's are empty, add computer stones to user's mancala
                if (userIsEmpty)
                {
                    int compStones = 0;
                    for (int comp = 0; comp < player.Length; comp++)
                    {
                        compStones += player[comp];
                    }
                    playerMancala += compStones;

                    if (playerMancala > compMancala)
                    {
                        retVal = true;
                    }
                    else if (playerMancala < compMancala)
                    {
                        retVal = false;
                    }
                }

                
            }
            return retVal;

            /*
             * iterate through player's row
             * if its all empty, 
             * put other player's stones into this player's mancala
             * then compare that player's mancala to the opponets
             * return who the winner is
             */
        }
        #endregion

        #region heuristic
        /*----------------------------------------------------------------------
         * assign a value between -1 (min is winning) and 1 (MAX is winning)
         * to the given board positin
         ---------------------------------------------------------------------*/
        public double heuristicValue()
        {
            double retVal = 0.0;
            if (isWin(Player.MAX))
            {
                retVal = 1.0;
            }
            else if (isWin(Player.MIN))
            {
                retVal = -1.0;
            }
            else
            {
                //AS PER DR. K: add two mancala's together and divide by 48;
                retVal = ((Double)compMancala + (Double)playerMancala) / 48;
            }
            return retVal;
        }
        #endregion

        public int whichMove(Board newBoard)
        {
            int theMove = 0;

            for(int compIndex = 0; compIndex < computer.Length; compIndex++)
            {
                if (this.computer[compIndex] != 0)
                {
                    if (newBoard.computer[compIndex] == 0)
                    {
                        theMove = compIndex;
                        break;
                    }
                }
            }

            return theMove;
        }

    }
}

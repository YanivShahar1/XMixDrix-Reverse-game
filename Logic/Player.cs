using System.Collections.Generic;
namespace Logic
{
    public class Player
    {
        private eSymbol             m_Symbol;
        private readonly eEntity    r_Entity;
        private System.Random       m_Random = null;
        private Board               m_Board;

        public Player(eEntity i_Entity, eSymbol i_Symbol, Board io_Board)
        {
            m_Board = io_Board;
            r_Entity = i_Entity;
            m_Symbol = i_Symbol;
            if (i_Entity == eEntity.RandomComputer)
            {
                m_Random = new System.Random();
            }
        }
        public eEntity Entity
        {
            get
            {
                return r_Entity;
            }
        }
        public eSymbol Symbol
        {
            get
            {
                return m_Symbol;
            }
        }
        public void     Move(ref Cell io_Cell)
        {
            if (r_Entity == eEntity.Human)
            {
                m_Board[io_Cell.Row, io_Cell.Column] = m_Symbol;
            }
            else if (r_Entity == eEntity.AIComputer)
            {
                makeAIMove(ref io_Cell);
            }
            else if (r_Entity == eEntity.RandomComputer)
            {
                makeRandomMove(ref io_Cell);
            }
        }
        private int     determineMaxDepth(int i_NumOfEmptyCell)
        {
            int maxDepth;
            if (i_NumOfEmptyCell < ((int)eBoardComplexity.VeryLow))
            {
                maxDepth = ((int)eMaxSearchDepth.VeryHigh);
            }
            else if (i_NumOfEmptyCell <= ((int)eBoardComplexity.Low))
            {
                maxDepth = ((int)eMaxSearchDepth.High);
            }
            else if (i_NumOfEmptyCell <= ((int)eBoardComplexity.Moderate))
            {
                maxDepth = ((int)eMaxSearchDepth.Moderate);
            }
            else if (i_NumOfEmptyCell <= ((int)eBoardComplexity.High))
            {
                maxDepth = ((int)eMaxSearchDepth.Low);
            }
            else
            {
                maxDepth = ((int)eMaxSearchDepth.VeryLow);
            }
            return maxDepth;
        }
        private void    makeAIMove(ref Cell io_Cell)
        {
            List<Cell> emptyCellsList = new List<Cell>(m_Board.EmptyCellsList);
            int maxDepth = determineMaxDepth(emptyCellsList.Count);
            int bestScore = int.MinValue;
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            Cell? bestMove = null;

            foreach (Cell cell in emptyCellsList)
            {
                int score = int.MinValue;
                int terminalScore;
                Board boardForAI = m_Board.Clone();
                boardForAI[cell.Row, cell.Column] = eSymbol.Player2;
                if (checkTerminalConditions(cell, eSymbol.Player2, boardForAI, out terminalScore))
                {
                    score = terminalScore;
                }  
                else
                {
                    boardForAI.EmptyCellsList.Remove(cell);
                    score = alphaBeta(boardForAI, maxDepth, alpha, beta, eSymbol.Player1);
 
                }
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = cell;
                }
                // Update alpha value
                alpha = System.Math.Max(alpha, bestScore);
            }
            if (bestMove != null)
            {
                m_Board[bestMove.Value.Row, bestMove.Value.Column] = eSymbol.Player2;
                io_Cell.Row = bestMove.Value.Row;
                io_Cell.Column = bestMove.Value.Column;
            }
        }
        private bool    checkTerminalConditions(Cell i_Cell, eSymbol i_Player, Board i_Board, out int o_Score)
        {
            if (isGameOver(i_Cell, i_Player, i_Board))
            {
                if(i_Player == eSymbol.Player2)
                {
                    o_Score = (i_Board.EmptyCellsList.Count * -200) - 400;
                }
                else
                {
                    o_Score = (i_Board.EmptyCellsList.Count * 200) + 400;
                }
                
                return true;
            }
            else if (i_Board.isBoardFull())
            {
                o_Score = 0;
                return true;
            }
            else
            {
                o_Score = -1;
                return false;
            }
           
        }
        private void    makeRandomMove(ref Cell io_Cell)
        {
            int randomIndexFromList = m_Random.Next(0, m_Board.EmptyCellsList.Count);
            int count = 0;
            Cell nextRandomCell = m_Board.EmptyCellsList.First.Value;
            foreach (Cell cell in m_Board.EmptyCellsList)
            {
                if (count == randomIndexFromList)
                {
                    nextRandomCell = cell;
                    break;
                }
                count++;
            }
            m_Board[nextRandomCell.Row, nextRandomCell.Column] = eSymbol.Player2;
            io_Cell.Row = nextRandomCell.Row;
            io_Cell.Column = nextRandomCell.Column;
        }
        private bool    isGameOver(Cell i_Cell, eSymbol i_Symbol, Board i_AIBoard)
        {
            return (i_AIBoard.DiagonalSymbolStreak(i_Cell, i_Symbol) == i_AIBoard.Size
                || i_AIBoard.VerticalSymbolStreak(i_Cell, i_Symbol) == i_AIBoard.Size
                || i_AIBoard.HorizontalSymbolStreak(i_Cell, i_Symbol) == i_AIBoard.Size);
        }
        private int     alphaBeta(Board i_Board,  int i_Depth, int i_Alph, int i_Beta, eSymbol i_Turn)
        {
            bool isMaximizingPlayer = i_Turn == eSymbol.Player2;
            if (isMaximizingPlayer)
            {
                int bestScore = int.MinValue;
                foreach (Cell move in i_Board.EmptyCellsList)
                {
                    Board clonedBoard = i_Board.Clone();
                    int score = 0;
                    int terminalScore;
                    clonedBoard[move.Row, move.Column] = i_Turn;
                    if (checkTerminalConditions(move, eSymbol.Player2, clonedBoard, out terminalScore))
                    {
                        return terminalScore;
                    }
                    if (i_Depth == 1) //last branch for ai recursion tree
                    {
                        return evaluateNextMoveScore(clonedBoard);
                    }

                    clonedBoard.EmptyCellsList.Remove(move);
                    score = alphaBeta(clonedBoard, i_Depth - 1, i_Alph, i_Beta, eSymbol.Player1);
                    bestScore = System.Math.Max(bestScore, score);

                    // Update alpha value
                    i_Alph = System.Math.Max(i_Alph, bestScore);

                    // Prune the remaining branches if beta <= alpha
                    if (i_Beta <= i_Alph)
                    {
                        break;
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                foreach (Cell move in i_Board.EmptyCellsList)
                {
                    Board clonedBoard = i_Board.Clone();
                    int score = 0;
                    int terminalScore;
                    clonedBoard[move.Row, move.Column] = eSymbol.Player1;
                    if (checkTerminalConditions(move, eSymbol.Player1, clonedBoard, out terminalScore))
                    {
                        return terminalScore;
                    }
                    if (i_Depth == 1) //last branch for ai recursion tree
                    {
                        return evaluateNextMoveScore(clonedBoard);
                    }
                    clonedBoard.EmptyCellsList.Remove(move);
                    score = alphaBeta(clonedBoard, i_Depth - 1, i_Alph, i_Beta, eSymbol.Player2);
                    bestScore = System.Math.Min(score, bestScore);
                    // Update beta value
                    i_Beta = System.Math.Min(i_Beta, bestScore);

                    // Prune the remaining branches if beta <= alpha
                    if (i_Beta <= i_Alph)
                    {
                        break;
                    }

                }
                return bestScore;
            }
        }
        private int     evaluateNextMoveScore(Board i_Board)
        {
            int player1Score = 0;
            int player2Score = 0;
            Cell cell = new Cell(1, 1);
            // Evaluate rows
            for (int row = 0 ; row < m_Board.Size ; row++)
            {
                int rowPlayer1Count = i_Board.HorizontalSymbolStreak(cell, eSymbol.Player1);
                int rowPlayer2Count = i_Board.HorizontalSymbolStreak(cell, eSymbol.Player2);
                player1Score += getScoreForCounter(rowPlayer1Count);
                player2Score += getScoreForCounter(rowPlayer2Count);
                cell.Row++;
            }
            // Evaluate columns
            for (int col = 0 ; col < m_Board.Size ; col++)
            {
                int colPlayer1Count = i_Board.VerticalSymbolStreak(cell, eSymbol.Player1);
                int colPlayer2Count = i_Board.VerticalSymbolStreak(cell, eSymbol.Player2);
                player1Score += getScoreForCounter(colPlayer1Count);
                player2Score += getScoreForCounter(colPlayer2Count);
                cell.Column++;
            }
            cell.Column = cell.Row = 1;
            // Evaluate diagonals
            int diagPlayer1Count = i_Board.DiagonalSymbolStreak(cell, eSymbol.Player1);
            int diagPlayer2Count = i_Board.DiagonalSymbolStreak(cell, eSymbol.Player2);

            player1Score += getScoreForCounter(diagPlayer1Count * 2);
            player2Score += getScoreForCounter(diagPlayer2Count * 2);

            return (player1Score - player2Score);
        }
        private int     getScoreForCounter(int i_Count)
        {
            return (int)System.Math.Pow(10, i_Count - 1);
        }
    }
}
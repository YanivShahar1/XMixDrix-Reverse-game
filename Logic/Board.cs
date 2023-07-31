using System.Collections.Generic;
using System;
namespace Logic
{
    public class Board
    {
        private readonly int        r_GridSize;
        private eSymbol[,]          m_Grid; 
        private LinkedList<Cell>    m_EmptyCellsList;
        public event Action<Cell, eSymbol> Fill;
        public Board(int i_BoardSize)
        {
            r_GridSize = i_BoardSize;
            m_Grid = new eSymbol[r_GridSize + 1, r_GridSize + 1];    
            resetGrid();
            m_EmptyCellsList = new LinkedList<Cell>();
            InitializeEmptyCellsList();
        }
        public int Size
        {
            get
            {
                return r_GridSize;
            }
        }
        public eSymbol[,] Grid
        {
            get
            {
                return m_Grid;
            }
        }
        public eSymbol this[int i_Row, int i_Column]
        {
            get
            {
                return m_Grid[i_Row, i_Column];
            }
            set
            {
                Cell cell = new Cell(i_Row, i_Column);
                if (!IsCellInBounds(cell))
                {
                    throw new System.Exception("Cell is out of bounds !");
                }
                if (!IsCellEmpty(cell) && value != eSymbol.Empty)
                {
                    throw new System.Exception("Cell is already used !");
                }
                m_Grid[i_Row, i_Column] = value;
                Fill?.Invoke(cell, value);
                if(value == eSymbol.Empty)
                {
                    m_EmptyCellsList.AddLast(cell);
                }
                else
                {
                    m_EmptyCellsList.Remove(cell);
                }
            }
        }
        public int  NumOfEmptyCells
        {
            get
            {
                return m_EmptyCellsList.Count;
            }
        }
        public LinkedList<Cell> EmptyCellsList
        {
            get
            {
                return m_EmptyCellsList;
            }
        }
        public void     InitializeEmptyCellsList()
        {
            m_EmptyCellsList.Clear();
            for (int i = 1 ; i <= r_GridSize ; i++)
            {
                for (int j = 1 ; j <= r_GridSize ; j++)
                {
                    m_EmptyCellsList.AddLast(new Cell(i, j));
                }
            }
        }
        private void    resetGrid()
        {
            for (int i = 0 ; i <= r_GridSize ; i++)
            {
                for (int j = 0 ; j <= r_GridSize ; j++)
                {
                    m_Grid[i, j] = eSymbol.Empty;
                }
            }
        }
        public void     Reset()
        {
            resetGrid();
            InitializeEmptyCellsList();
        }
        public bool     IsCellEmpty(Cell i_Cell)
        {
            return m_Grid[i_Cell.Row, i_Cell.Column] == eSymbol.Empty;
        }
        public bool     IsCellInBounds(Cell i_Cell)
        {
            return i_Cell.Row >= 1 && i_Cell.Row <= r_GridSize
                && i_Cell.Column >= 1 && i_Cell.Column <= r_GridSize;
        }
        private int     TopLeftDiagonalSymbolStreakFromCell(Cell i_Cell, eSymbol i_Symbol)
        {
            int streak = 0;
            i_Cell.Row--;
            i_Cell.Column--;
            while (IsCellInBounds(i_Cell) && m_Grid[i_Cell.Row, i_Cell.Column] == i_Symbol)
            {
                streak++;
                i_Cell.Row--;
                i_Cell.Column--;
            }
            return streak;
        }
        private int     BottomRightDiagonalSymbolStreakFromCell(Cell i_Cell, eSymbol i_Symbol)
        {
            int streak = 0;
            i_Cell.Row++;
            i_Cell.Column++;
            while (IsCellInBounds(i_Cell) && m_Grid[i_Cell.Row, i_Cell.Column] == i_Symbol)
            {
                streak++;
                i_Cell.Row++;
                i_Cell.Column++;
            }
            return streak;
        }
        private int     TopRightDiagonalSymbolStreakFromCell(Cell i_Cell, eSymbol i_Symbol)
        {
            int streak = 0;
            i_Cell.Row--;
            i_Cell.Column++;
            while (IsCellInBounds(i_Cell) && m_Grid[i_Cell.Row, i_Cell.Column] == i_Symbol)
            {
                streak++;
                i_Cell.Row--;
                i_Cell.Column++;
            }
            return streak;
        }
        private int     BottomLeftDiagonalSymbolStreakFromCell(Cell i_Cell, eSymbol i_Symbol)
        {
            int streak = 0;
            i_Cell.Row++;
            i_Cell.Column--;
            while (IsCellInBounds(i_Cell) && m_Grid[i_Cell.Row, i_Cell.Column] == i_Symbol)
            {
                streak++;
                i_Cell.Row++;
                i_Cell.Column--;
            }
            return streak;
        }
        private int     LeftSymbolStreakFromCell(Cell i_Cell, eSymbol i_Symbol)
        {
            int streak = 0;
            i_Cell.Column--;
            while (IsCellInBounds(i_Cell) && m_Grid[i_Cell.Row, i_Cell.Column] == i_Symbol)
            {
                streak++;
                i_Cell.Column--;
            }
            return streak;
        }
        private int     RightSymbolStreakFromCell(Cell i_Cell, eSymbol i_Symbol)
        {
            int streak = 0;
            i_Cell.Column++;
            while (IsCellInBounds(i_Cell) && m_Grid[i_Cell.Row, i_Cell.Column] == i_Symbol)
            {
                streak++;
                i_Cell.Column++;
            }
            return streak;
        }
        private int     TopSymbolStreakFromCell(Cell i_Cell, eSymbol i_Symbol)
        {
            int streak = 0;
            i_Cell.Row--;
            while (IsCellInBounds(i_Cell) && m_Grid[i_Cell.Row, i_Cell.Column] == i_Symbol)
            {
                streak++;
                i_Cell.Row--;
            }
            return streak;
        }
        private int     BottomSymbolStreakFromCell(Cell i_Cell, eSymbol i_Symbol)
        {
            int streak = 0;
            i_Cell.Row++;
            while (IsCellInBounds(i_Cell) && m_Grid[i_Cell.Row, i_Cell.Column] == i_Symbol)
            {
                streak++;
                i_Cell.Row++;
            }
            return streak;
        }
        public int      DiagonalSymbolStreak(Cell i_Cell, eSymbol i_Symbol)
        {
            int topLeftToBottomRightDiagonalSymbolStreak = 
                TopLeftDiagonalSymbolStreakFromCell(i_Cell, i_Symbol) + 1
                + BottomRightDiagonalSymbolStreakFromCell(i_Cell, i_Symbol);
            int bottomLeftToTopRightDiagonalSymbolStreak =
                BottomLeftDiagonalSymbolStreakFromCell(i_Cell, i_Symbol) + 1
                + TopRightDiagonalSymbolStreakFromCell(i_Cell, i_Symbol);

            return System.Math.Max(
                topLeftToBottomRightDiagonalSymbolStreak,
                bottomLeftToTopRightDiagonalSymbolStreak
                );
        }
        public int      VerticalSymbolStreak(Cell i_Cell, eSymbol i_Symbol)
        {
            return TopSymbolStreakFromCell( i_Cell, i_Symbol) + 1 
                + BottomSymbolStreakFromCell(i_Cell, i_Symbol);
        }
        public int      HorizontalSymbolStreak(Cell i_Cell, eSymbol i_Symbol)
        {
            return RightSymbolStreakFromCell(i_Cell, i_Symbol) + 1
                + LeftSymbolStreakFromCell(i_Cell, i_Symbol);
        }
        public bool     isBoardFull()
        {
            return m_EmptyCellsList.Count == 0;
        }
        public Board    Clone()
        {
            Board clonedBoard = new Board(r_GridSize);

            // Copy the grid
            for (int i = 1 ; i <= r_GridSize ; i++)
            {
                for (int j = 1 ; j <= r_GridSize ; j++)
                {
                    clonedBoard.m_Grid[i, j] = m_Grid[i, j];
                }
            }

            // Copy the empty cells list
            clonedBoard.m_EmptyCellsList = new LinkedList<Cell>(m_EmptyCellsList);

            return clonedBoard;
        }
    }
}
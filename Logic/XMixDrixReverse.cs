namespace Logic
{
    public partial class XMixDrixReverse
    {
        private Board       m_Board;
        private Player      m_Player1 = null;
        private Player      m_Player2 = null;
        private eTurn       m_CurrentTurn = eTurn.Player1;
        private eWinner     m_Winner = eWinner.NotDetermined;

        public XMixDrixReverse(int i_BoardSize, eEntity i_Rival)
        {
            m_Board = new Board(i_BoardSize);
            m_Player1 = new Player(eEntity.Human, eSymbol.Player1, m_Board);
            m_Player2 = new Player(i_Rival, eSymbol.Player2, m_Board);
        }
        public Player CurrentPlayer
        {
            get
            {
                return (m_CurrentTurn == eTurn.Player1) ? m_Player1 : m_Player2;
            }
        }
        public eTurn Turn
        {
            get
            {
                return m_CurrentTurn;
            }
        }
        public eWinner Winner
        {
            get
            {
                return m_Winner;
            }
        }
        public eSymbol[,] Grid
        {
            get
            {
                return m_Board.Grid;
            }
        }
        public int GridSize
        {
            get
            {
                return m_Board.Size;
            }
        }
        public Board Board
        {
            get
            {
                return m_Board;
            }
        }
        private void    switchTurn()
        {
            m_CurrentTurn = (m_CurrentTurn == eTurn.Player1) ? eTurn.Player2 : eTurn.Player1;
        }
        public void     MakeMove(ref Cell i_Cell)
        {
            if (m_CurrentTurn == eTurn.Player1)
            {
                m_Player1.Move(ref i_Cell);
                m_Board.EmptyCellsList.Remove(i_Cell);
                IsCurrentMoveLost(i_Cell, eSymbol.Player1);
                if (m_Player2.Entity == eEntity.Human)
                {
                    switchTurn();
                }
                else
                {
                    if (!IsGameOver())
                    {
                        m_Player2.Move(ref i_Cell);
                        m_Board.EmptyCellsList.Remove(i_Cell);
                        IsCurrentMoveLost(i_Cell, eSymbol.Player2);
                    }
                }
            }
            else if (m_CurrentTurn == eTurn.Player2)
            {
                m_Player2.Move(ref i_Cell);
                m_Board.EmptyCellsList.Remove(i_Cell);
                IsCurrentMoveLost(i_Cell, eSymbol.Player2);
                switchTurn();
            }
        }
        public bool     IsCurrentMoveLost(Cell i_CurrentMove, eSymbol i_Symbol)
        {
            bool isCurrentMoveLost = false;
            if (m_Board.HorizontalSymbolStreak(i_CurrentMove, i_Symbol) == m_Board.Size
                || m_Board.VerticalSymbolStreak(i_CurrentMove, i_Symbol) == m_Board.Size
                || m_Board.DiagonalSymbolStreak(i_CurrentMove, i_Symbol) == m_Board.Size)
            {
                m_Winner = (i_Symbol == eSymbol.Player1) ? eWinner.Player2 : eWinner.Player1;
                isCurrentMoveLost = true;
            }
            else if (Board.NumOfEmptyCells == 0)
            {
                m_Winner = eWinner.Tie;
                isCurrentMoveLost = true;
            }

            return isCurrentMoveLost;
        }
        public void     ResetGame()
        {
            m_Board.Reset();
            m_CurrentTurn = eTurn.Player1;
            m_Winner = eWinner.NotDetermined;
            m_Board.InitializeEmptyCellsList();
        }
        public bool     IsGameOver()
        {
            return m_Winner != eWinner.NotDetermined;
        }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;
using Logic;
namespace XMixDrixReverseGUI
{
    public class BoardGUI : Form
    {
        private static readonly string  sr_Player1Symbol = "X";
        private static readonly string  sr_Player2Symbol = "O";
        private const int               k_Margin = 12;
        private GameSettingsPackage     m_GameSettings;
        private Button[,]               m_Cells;
        private XMixDrixReverse         m_Game;
        private int                     m_Player1Score = 0;
        private int                     m_Player2Score = 0;
        private Label                   m_Player1Label;
        private Label                   m_Player2Label;
        private readonly Color          r_Player1Color = Color.Green;
        private readonly Color          r_Player2Color = Color.Red;

        public BoardGUI(GameSettingsPackage i_GameSettings)
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            m_Game = new XMixDrixReverse(i_GameSettings.BoardSize, i_GameSettings.Rival);
            m_Game.Board.Fill += board_Fill;
            m_GameSettings = i_GameSettings;
            m_Player1Label = new Label();
            m_Player2Label = new Label();
            this.StartPosition = FormStartPosition.CenterScreen;
            initializeBoard();
            this.Resize += boardGUI_Resize;
        }
        private void boardGUI_Resize(object sender, EventArgs e)
        {
            // Resized horizontally
            if (this.Size.Width > MinimumSize.Width)
                this.Size = new Size(this.Width, (int)(this.Size.Width * MinimumSize.Height / MinimumSize.Width));
            // Resized vertically
            else if (this.Size.Height > MinimumSize.Height)
                this.Size = new Size((int)(this.Size.Height * MinimumSize.Width / MinimumSize.Height), this.Size.Height);
            
            int buttonSize = (Math.Min(this.ClientSize.Height, this.ClientSize.Width) - k_Margin * 2) / m_GameSettings.BoardSize;
            for (int i = 0 ; i < m_GameSettings.BoardSize ; i++)
            {
                for (int j = 0 ; j < m_GameSettings.BoardSize ; j++)
                {
                    Button button = m_Cells[i + 1, j + 1];
                    button.Location = new Point(j * buttonSize + k_Margin / 2, i * buttonSize + k_Margin / 2);
                    button.Size = new Size(buttonSize, buttonSize);
                }

            }
            m_Player1Label.Location = new Point(this.Width / 5, m_GameSettings.BoardSize * buttonSize + k_Margin);
            m_Player2Label.Location = new Point(this.Width / 2, m_GameSettings.BoardSize * buttonSize + k_Margin);
        }
        private void board_Fill(Cell i_Cell, eSymbol i_Symbol)
        {
            Button clickedButton = m_Cells[i_Cell.Row, i_Cell.Column];
            string Symbol = i_Symbol == eSymbol.Player1 ? sr_Player1Symbol : sr_Player2Symbol;
            Color color = i_Symbol == eSymbol.Player1 ? r_Player1Color : r_Player2Color;
            clickedButton.Text = Symbol;
            clickedButton.BackColor = color;
            clickedButton.Enabled = false;
        }
        private void initializeButtonsOnBoard(int i_InitCellSize)
        {
            m_Cells = new Button[m_GameSettings.BoardSize + 1, m_GameSettings.BoardSize + 1];
            for (int i = 0 ; i < m_GameSettings.BoardSize ; i++)
            {
                for (int j = 0 ; j < m_GameSettings.BoardSize ; j++)
                {
                    Button newButton = new Button();
                    newButton.Location = new Point(j * i_InitCellSize + k_Margin / 2, i * i_InitCellSize + k_Margin / 2);
                    newButton.Tag = new Cell(i + 1, j + 1);
                    newButton.Size = new Size(i_InitCellSize, i_InitCellSize);
                    newButton.Font = new Font(newButton.Font, FontStyle.Bold);
                    newButton.Click += CellClickHandler;
                    m_Cells[i + 1, j + 1] = newButton;
                    this.Controls.Add(newButton);
                }
            }
        }
        private void initializeScoreLabels(int i_InitCellSize)
        {
            // Score display for Player 1
            m_Player1Label.Location = new Point(this.Width / 5, m_GameSettings.BoardSize * i_InitCellSize + k_Margin);
            m_Player1Label.AutoSize = true;
            m_Player1Label.Size = new Size(this.Width / 2, i_InitCellSize);
            m_Player1Label.Text = string.Format("{0}: {1}", m_GameSettings.Player1Name, m_Player1Score);
            m_Player1Label.Font = new Font(m_Player1Label.Font, FontStyle.Bold);
            this.Controls.Add(m_Player1Label);
            // Score display for Player 2
            m_Player2Label.Location = new Point(this.Width / 2, m_GameSettings.BoardSize * i_InitCellSize + k_Margin);
            m_Player2Label.AutoSize = true;
            m_Player2Label.Size = new Size(this.Width / 2, i_InitCellSize);
            m_Player2Label.Text = string.Format("{0}: {1}", m_GameSettings.Player2Name, m_Player2Score);
            this.Controls.Add(m_Player2Label);
        }
        private void initializeBoard()
        {
            int cellSize = 60;
            this.Text = "X Mix Drix Reverse";
            this.ClientSize = new Size(cellSize * m_GameSettings.BoardSize + k_Margin,
                               cellSize * (m_GameSettings.BoardSize) + k_Margin * 3);
            initializeButtonsOnBoard(cellSize);
            initializeScoreLabels(cellSize);
        }
        private void resetBoard()
        {
            m_Game.ResetGame();
            for (int i = 0 ; i < m_GameSettings.BoardSize ; i++)
            {
                for (int j = 0 ; j < m_GameSettings.BoardSize ; j++)
                {
                    m_Cells[i + 1, j + 1].Text = "";
                    m_Cells[i + 1, j + 1].Enabled = true;
                    m_Cells[i + 1, j + 1].BackColor = Color.Transparent;
                }
            }
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BoardGUI
            // 
            this.ClientSize = new System.Drawing.Size(278, 244);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BoardGUI";
            this.ResumeLayout(false);

        }
        private void CellClickHandler(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                Cell move = (Cell)clickedButton.Tag;
                try
                {
                    Cell previousMove = new Cell(move.Row, move.Column);
                    m_Game.MakeMove(ref move);
                    XMixDrixReverse.eTurn currentTurn = m_Game.Turn;
                    if (currentTurn == XMixDrixReverse.eTurn.Player1)
                    {
                        m_Player1Label.Font = new Font(m_Player1Label.Font, FontStyle.Bold);
                        m_Player2Label.Font = new Font(m_Player2Label.Font, FontStyle.Regular);
                    }
                    else
                    {
                        m_Player1Label.Font = new Font(m_Player1Label.Font, FontStyle.Regular);
                        m_Player2Label.Font = new Font(m_Player2Label.Font, FontStyle.Bold);
                    }
                    if (m_Game.IsGameOver())
                    {
                        if (m_Game.Winner == XMixDrixReverse.eWinner.Tie)
                        {
                            DialogResult dialogResult = MessageBox.Show
                                (
                                    $"Tie! \nWould You Like To Play Another Round?",
                                    "A Tie !", MessageBoxButtons.YesNo
                                );
                            if (dialogResult == DialogResult.Yes)
                            {
                                resetBoard();
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                Close();
                            }
                        }
                        else
                        {
                            m_Player1Score += m_Game.Winner == XMixDrixReverse.eWinner.Player1 ? 1 : 0;
                            m_Player2Score += m_Game.Winner == XMixDrixReverse.eWinner.Player2 ? 1 : 0;
                            m_Player1Label.Text = string.Format("{0}: {1}", m_GameSettings.Player1Name, m_Player1Score);
                            m_Player2Label.Text = string.Format("{0}: {1}", m_GameSettings.Player2Name, m_Player2Score);
                            DialogResult dialogResult = MessageBox.Show
                                (
                                    $"The Winner is {(m_Game.Winner == XMixDrixReverse.eWinner.Player1 ? m_GameSettings.Player1Name : m_GameSettings.Player2Name)}! \nWould You Like To Play Another Round?",
                                    "A Win !", MessageBoxButtons.YesNo
                                );
                            if (dialogResult == DialogResult.Yes)
                            {
                                resetBoard();
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                Close();
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }
    }
}

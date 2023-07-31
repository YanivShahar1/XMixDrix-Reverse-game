using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XMixDrixReverseGUI
{
    public partial class formGameSettings : Form
    {
        private GameSettingsPackage m_GameSettings;
        public formGameSettings()
        {
            InitializeComponent();
        }
        private void checkBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPlayer2.Checked)
            {
                textBoxPlayer2Name.Clear();
                textBoxPlayer2Name.Enabled = true;
            }
            else
            {
                textBoxPlayer2Name.Text = "[Computer]";
                textBoxPlayer2Name.Enabled = false;
            }
        }

        private void numericUpDownRows_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownColums.Value = numericUpDownRows.Value;
        }

        private void numericUpDownColums_ValueChanged(object sender, EventArgs e)
        {
            numericUpDownRows.Value = numericUpDownColums.Value;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (textBoxPlayer1Name.Text.Length == 0)
            {
                MessageBox.Show("No Name Specified For Player 1 !");
            }
            else if (checkBoxPlayer2.Checked && textBoxPlayer2Name.Text.Length == 0)
            {
                MessageBox.Show("No Name Specified For Player 2 !");
            }
            else
            {
                m_GameSettings = new GameSettingsPackage(
                    textBoxPlayer1Name.Text, 
                    checkBoxPlayer2.Checked ? textBoxPlayer2Name.Text : "Computer",
                    checkBoxPlayer2.Checked ? Logic.eEntity.Human : Logic.eEntity.AIComputer,
                    (int)numericUpDownColums.Value);
                Close();
            }
        }
        public GameSettingsPackage GameSettings
        {
            get
            {
                return m_GameSettings;
            }
        }
    }
}

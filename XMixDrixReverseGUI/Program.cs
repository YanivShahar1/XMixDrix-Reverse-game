using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace XMixDrixReverseGUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            formGameSettings gameSettingsForm = new formGameSettings();
            Application.Run(gameSettingsForm);
            GameSettingsPackage gameSettingsPackage = gameSettingsForm.GameSettings;
            BoardGUI board = new BoardGUI(gameSettingsPackage);
            Application.Run(board);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic;

namespace XMixDrixReverseGUI
{
    public class GameSettingsPackage
    {
        private string  m_Player1Name;
        private string  m_Player2Name;
        private eEntity m_Rival;
        private int     m_BoardSize;

        public GameSettingsPackage(string i_Player1Name, string i_Player2Name,
                                    eEntity i_Rival, int i_BoardSize)
        {
            m_Player1Name = i_Player1Name;
            m_Player2Name = i_Player2Name;
            m_Rival = i_Rival;
            m_BoardSize = i_BoardSize;
        }

        public string Player1Name
        {
            get
            {
                return m_Player1Name;
            }
        }
        public string Player2Name
        {
            get
            {
                return m_Player2Name;
            }
        }
        public eEntity Rival
        {
            get
            {
                return m_Rival;
            }
        }
        public int BoardSize
        {
            get
            {
                return m_BoardSize;
            }
        }
    }
}

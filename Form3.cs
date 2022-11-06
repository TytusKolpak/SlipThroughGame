using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Slip_through
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            label1.Text = "To control the behavior of the game You can either use mouse and select buttons in the window or use keyboard.\r\n\r\nButtons related to movement are placed in the left bottom corner of the window.\r\nEach represents the number of tiles Your character would pass.\r\nTheir keyboard equivalents are keys 1,2,3,4,5,6.\r\n\r\nButtons related to accepting stat increase are placed in the character panel, just below each stat's numerical value.\r\nInitially they are not visible, since You are entitled to a stat increase only after defeating an enemy.\r\nTheir keyboard equivalents are keys 'a' for Attack, 'd' for Defense and 'e' for Effectiveness.\r\n\r\nCheckbox related to slipping is placed to the right of number buttons.\r\nIt can be checked only while waiting for the character to move. During combat it's disabled.\r\nIts keyboard equivalent is key 's' for Slip.\r\n\r\nButton related to acknowledging death in combat is located at the bottom of combat log panel.\r\nInitially it is not visible, since combat log appears only after the combat has taken place.\r\nIts keyboard equivalent is key 'o' for Ok.\r\n\r\nAfter combat application focuses on the button corresponding to the first choice a player can make, so +ATT, and OK.\r\nThis enables player to choose those buttons by clicking Enter as well as 'a' or 'o'.";
        }
    }
}

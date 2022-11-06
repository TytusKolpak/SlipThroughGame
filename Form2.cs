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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            label1.Text = "\r\n   _____ _ _         _   _                           _     \r\n  / ____| (_)       | | | |                         | |    \r\n | (___ | |_ _ __   | |_| |__  _ __ ___  _   _  __ _| |__  \r\n  \\___ \\| | | '_ \\  | __| '_ \\| '__/ _ \\| | | |/ _` | '_ \\ \r\n  ____) | | | |_) | | |_| | | | | | (_) | |_| | (_| | | | |\r\n |_____/|_|_| .__/   \\__|_| |_|_|  \\___/ \\__,_|\\__, |_| |_|\r\n            | |                                 __/ |      \r\n            |_|                                |___/       \r\n";
            label1.Text += "\n\n";
            label1.Text += "# SlipThroughGame\r\n\r\nA simple C# Windows Forms boadlike game\r\n\r\n## Game Instruction\r\n\r\n### Game overview\r\n\r\nThe game is supposed to be a digital version of a simple board game for 3 people.\r\nIt is played on a board with 30 tiles in a grid pattern - with 6 rows and 5 columns.\r\nPlayers move through the board, along single path, that can be changed if the player wants it.\r\nThe goal of the game is to get to the end, to the 30th tile before other players do so.\r\nAlong the path there are increasingly difficult to defeat enemies trying to kill you, in which case you are placed on the 1st tile of the board.\r\n\r\n### Movement rules\r\n\r\n- Each player starts from 1st tile and the first player to 30th tile wins.\r\n- Movement is decided by player, who has a choice to move by 1 - 6 tiles at once.\r\n- At the end of every player's turn it is decided weather or not he is attacked by an enemy.\r\n  - Chance for being attacked is calculated as follows: (number of tiles crossed -1 ) / 6.\r\n- In first two rows player can be attacked only by a wolf, 3rd and 4th a werewolf and for 5th and 6th is a cerberus.\r\n- At tile 10, 15, 20, 25 the player can choose to go back up (to tile 1,6,11,16) instead of of continuing by normal path (to tile 11,16,21,26). That action is the Slip Through.\r\n  - If the player does so then their character recieves a stat boost according to the character which they chose.\r\n- The order in which players move is set to warrior -> archer -> wizard, after last players turn end a new round begins.\r\n\r\n### Combat rules\r\n\r\n- Each of 3 players gets their own character with preset statistics to determine how the combat carries out: warrior, wizard, archer.\r\n- If you win combat you get a chance to choose a stat increase, any stat:\r\n  - +1 for wolf,\r\n  - +2 for werewolf and cerberus.\r\n- For performing a Slip Through player's character gets stat boost:\r\n  - Warrior gets +1 defence,\r\n  - Wizard gets +1 attack,\r\n  - Archer gets +1 effeciveness.\r\n- Combat is carried in turns: first the player attack their enemy, and then the enemy attack the player\r\n- Success of an attack is determined like so: if a dice roll is greater than effectiveness of the enemy lowered by effectiveness of the character, then an attack is succesful, otherwise it fails and nothing happens.\r\n  - If an attack is succesful then the attacking character deals damage equal to their attack lowered by defence ot their enemy.\r\n  - If the character dies, then it goes back to 1st tile with their healt set to it's previous value lowered by 1 (each death lowers it further).\r\n- If the character kills its enemy then it recieves a stat boost and stays on the tile the fight took place.\r\n\r\n### Statistics\r\n\r\nEach instance begins with the same amount of predefined statistics: attack, defence, effectiveness, health.\r\n\r\n- Warrior: 2, 1, 2, 10,\r\n- Archer: 1, 1, 3, 10,\r\n- Wizard: 3, 0, 2, 10,\r\n- Wolf: 3, 0, 5, 5,\r\n- Werewolf: 4, 1, 6, 7,\r\n- Cerberus: 5, 3, 8, 10\r\n\r\n### Board form as depicted by tile numbers\r\n\r\n|   1   |   2   |   3   |   4   |   5   |\r\n|  10   |   9   |   8   |   7   |   6   |\r\n|  11   |  12   |  13   |  14   |  15   |\r\n|  20   |  19   |  18   |  17   |  16   |\r\n|  21   |  22   |  23   |  24   |  25   |\r\n|  30   |  29   |  28   |  27   |  26   |\r\n";
            label1.Text += "\n\n\nTytus Kołpak 2022";
        }
    }
}

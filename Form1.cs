using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Slip_through
{
    public partial class Form1 : Form
    {
        int panelNumberInt = 0;
        int thisPlayerInTurn = 0;
        String panelName = "panel1";
        String panelNumberString = "0";
        Panel[] panelArray = new Panel[0];
        PictureBox[] pictureBoxArray = new PictureBox[6];
        CombatCard[] CombatCardArray = new CombatCard[0];

        CombatCard WarriorCard;
        CombatCard WizardCard;
        CombatCard ArcherCard;
        CombatCard WolfCard;
        CombatCard WerewolfCard;
        CombatCard CerberusCard;

        public Form1()
        {
            createPanelArray();
            createPlayerArray();
            InitializeComponent();
            createPictureBoxArray();

            WarriorCard = new CombatCard(2, 1, 2, 10, Properties.Resources.warrior, pictureBoxArray[0]);
            WizardCard = new CombatCard(3, 1, 1, 10, Properties.Resources.wizard, pictureBoxArray[1]);
            ArcherCard = new CombatCard(1, 1, 3, 10, Properties.Resources.archer, pictureBoxArray[2]);
            WolfCard = new CombatCard(3, 0, 2, 5, Properties.Resources.wolf, pictureBoxArray[3]);
            WerewolfCard = new CombatCard(4, 2, 3, 10, Properties.Resources.werewolf, pictureBoxArray[4]);
            CerberusCard = new CombatCard(5, 4, 4, 15, Properties.Resources.cerberus, pictureBoxArray[5]);
            CombatCard.currentPlayer = WarriorCard;
        }
        private void createPanelArray()
        {
            panelArray = new Panel[30]
            {
                panel1, panel2, panel3, panel4, panel5,
                panel6, panel7, panel8, panel9, panel10,
                panel11, panel12, panel13, panel14, panel15,
                panel16, panel17, panel18, panel19, panel20,
                panel21, panel22, panel23, panel24, panel25,
                panel26, panel27, panel28, panel29, panel30,
            };
        }
        private void createPictureBoxArray()
        {
            pictureBoxArray = new PictureBox[6]
            {
                pictureBoxWarrior,
                pictureBoxWizard,
                pictureBoxArcher,
                pictureBoxWolf,
                pictureBoxWerewolf,
                pictureBoxCerberus,
            };
        }
        private void createPlayerArray()
        {
            CombatCardArray = new CombatCard[3]{
                WarriorCard,
                WizardCard,
                ArcherCard,
            };
        }
        private void moveThisBy(int steps)
        {
            //change parent to a path to the parent
            panelName = CombatCard.currentPlayer.pictureBox.Parent.Name;
            panelNumberString = Regex.Match(panelName, @"\d+").Value; //single numeric value in string form
            panelNumberInt = Int32.Parse(panelNumberString);//single numeric value in int form
            if (panelNumberInt < 30) //will have a place to end on
            {
                for (int i = 0; i < steps; i++)
                {
                    Thread.Sleep(250);
                    if (SlipBox.Checked == true && panelNumberInt % 5 == 0 && panelNumberInt >= 10)
                    {
                        panelNumberInt -= 9;
                    }
                    else
                    {
                        panelNumberInt++; //mark next panel as destination
                    }

                    CombatCard.currentPlayer.pictureBox.Parent = panelArray[panelNumberInt - 1];
                    //position of picture box is set by assigning it to specific parent (setting it's parent property)
                    //it's like: place the picture box on destinated panel
                    //- 1 is because panel1 has index 0, so xth panel: panelx has index of x-1
                    CombatCard.currentPlayer.pictureBox.BringToFront();           //so that the panel does not cover the picturebox
                    CombatCard.currentPlayer.pictureBox.Update();                 //idk without it the picture is not visible
                }
            }
        }
        private void endOfMovement()
        {
            pictureBoxPlayer.Image = Properties.Resources.wizard;
            labelEnemyHitPoints.Text = "83";
            tableLayoutPanelEnemy.Visible = true;
        }
        private void nextPlayer()
        {
            if (thisPlayerInTurn<2)
            {
                thisPlayerInTurn++;
            }
            else
            {
                thisPlayerInTurn = 0;
            }

            CombatCard.currentPlayer = CombatCardArray[thisPlayerInTurn];
        }
        private void loadPlayerInfo(CombatCard currentPlayer)
        {
            CombatCard.currentPlayer = WarriorCard;
            labelPlayerAttack.Text = CombatCard.currentPlayer.attack.ToString();
            labelPlayerDefense.Text = CombatCard.currentPlayer.attack.ToString();
            labelPlayerEffectiveness.Text = CombatCard.currentPlayer.attack.ToString();
            labelPlayerHitPoints.Text = CombatCard.currentPlayer.attack.ToString();
            pictureBoxPlayer.Image = CombatCard.currentPlayer.picture;
        }
        private void carryOutSequence(int diceThrowValue)
        {
            loadPlayerInfo(CombatCard.currentPlayer);
            moveThisBy(diceThrowValue);//from which panel it starts and how far it moves
            endOfMovement();
            CombatCard.fight(WarriorCard, WolfCard, 2);//example of execute combat
            nextPlayer();//and when it;s ended
        }
        private void button1_Click(object sender, EventArgs e)
        {
            carryOutSequence(1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            carryOutSequence(2);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            carryOutSequence(3);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            carryOutSequence(4);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            carryOutSequence(5);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            carryOutSequence(6);
        }
    }
}
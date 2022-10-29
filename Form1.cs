using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Slip_through
{
    public partial class Form1 : Form
    {
        int panelNumberInt = 0;
        int pictureBoxNumber = 0;
        int turn = 1;
        String panelName = "panel1";
        String panelNumberString = "0";
        Panel[] panelArray = new Panel[0];
        PictureBox[] pictureBoxArray = new PictureBox[0];
        PictureBox currentPictureBox;
        CombatCard WizardCard = new CombatCard(3, 1, 1, 10);
        CombatCard WarriorCard = new CombatCard(2, 1, 2, 10);
        CombatCard ArcherCard = new CombatCard(1, 1, 3, 10);
        CombatCard WolfCard = new CombatCard(3, 0, 2, 5);
        CombatCard WerewolfCard = new CombatCard(4, 2, 3, 10);
        CombatCard CerberusCard = new CombatCard(5, 4, 4, 15);

        public Form1()
        {
            InitializeComponent();
            InitMySetup();
            currentPictureBox = Warrior;
        }
        private void InitMySetup()
        {
            createArrays();
            label2.Text = "Turn :";
        }
        private void createArrays()
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

            pictureBoxArray = new PictureBox[3]
            {
                Warrior, Archer, Wizard,
            };
            currentPictureBox = pictureBoxArray[0];
        }
        private void moveThisBy(Control parent, int steps)
        {
            panelName = parent.Name.ToString();
            panelNumberString = Regex.Match(panelName, @"\d+").Value; //single numeric value in string form
            panelNumberInt = Int32.Parse(panelNumberString);//single numeric value in int form
            if (panelNumberInt < 30) //will have a place to end on
            {
                for (int i = 0; i < steps; i++)
                {
                    Thread.Sleep(250);
                    if (SlipBox.Checked == true && panelNumberInt % 5 == 0 && panelNumberInt >= 10) // that is wrong
                    {
                        panelNumberInt -= 9; //works, but updates in wrong moment
                    }
                    else
                    {
                        panelNumberInt++;
                    }
                    currentPictureBox.Parent = panelArray[panelNumberInt - 1];
                    //-1 is because panel1 has index 0 : x on x-1
                    currentPictureBox.BringToFront();
                    currentPictureBox.Update();
                }
            }
            endOfMovement();
        }

        private void endOfMovement()
        {
            pictureBoxPlayer.Image = Properties.Resources.wizard;
            labelEnemyHitPoints.Text = "83";
            tableLayoutPanelEnemy.Visible = true;
            //execute combat

            fight(WarriorCard, WolfCard, 2); //example

            //and when it;s ended
            nextPlayer();
        }

        public void fight(CombatCard attacker, CombatCard defender, int diceThrow)
        {

                if (attacker.hitPoints > 0)
                {
                    if (attacker.effectivenes + diceThrow > defender.effectivenes)
                    {
                        defender.hitPoints -= attacker.attack - defender.defence;
                    }
                }

                if (defender.hitPoints > 0)
                {
                    if (defender.effectivenes - diceThrow > attacker.effectivenes)
                    {
                        defender.hitPoints -= attacker.attack - defender.defence;
                    }
                }
            
        }

        private void nextPlayer() //complete, works properly
        {

            if (pictureBoxNumber < 2)
            {
                pictureBoxNumber++;
            }
            else
            {
                pictureBoxNumber = 0;
                turn++;
            }

            TurnLabel.Text = turn.ToString();

            if (pictureBoxNumber == 0)
            {
                label1.Text = "Warrior";
            }
            else if (pictureBoxNumber == 1)
            {
                label1.Text = "Archer";
            }
            else
            {
                label1.Text = "Wizard";
            }

            currentPictureBox = pictureBoxArray[pictureBoxNumber];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            moveThisBy(currentPictureBox.Parent, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            moveThisBy(currentPictureBox.Parent, 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            moveThisBy(currentPictureBox.Parent, 3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            moveThisBy(currentPictureBox.Parent, 4);
            pictureBoxPlayer.Image = Properties.Resources.knight;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            moveThisBy(currentPictureBox.Parent, 5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBoxPlayer.Image = Properties.Resources.archer;
            moveThisBy(currentPictureBox.Parent, 6);
            tableLayoutPanelEnemy.Visible = false;
        }

        private void SlipBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        //check if panel has a child of enemy when stepping on it,
        //enable choice of a throw and a random throw
    }
}
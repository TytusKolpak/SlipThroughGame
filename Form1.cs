using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Slip_through
{
    public partial class Form1 : Form
    {
        String panelName = "panel1";
        String panelNumberString = "0";
        int panelNumberInt = 0;
        int pictureBoxNumber = 0;
        Panel[] panelArray = new Panel[0];
        PictureBox[] pictureBoxArray = new PictureBox[0];
        PictureBox currentPictureBox;
        int turn = 1;

        public Form1()
        {
            InitializeComponent();
            createArrays();
            label2.Text = "Turn :";
            currentPictureBox = Warrior;
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
                    if (SlipBox.Checked == true && panelNumberInt%5==0 && panelNumberInt>=10) // that is wrong
                    {
                        panelNumberInt -= 9; //works, but updates in wrong moment
                    }
                    else
                    {
                        panelNumberInt++;
                    }
                    currentPictureBox.Parent = panelArray[panelNumberInt-1];
                    //-1 is because panel1 has index 0 : x on x-1
                    currentPictureBox.BringToFront();
                    currentPictureBox.Update();
                }
            }
            nextPlayer();
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
        }

        private void button5_Click(object sender, EventArgs e)
        {
            moveThisBy(currentPictureBox.Parent, 5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            moveThisBy(currentPictureBox.Parent, 6);
        }

        private void SlipBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
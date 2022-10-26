using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Slip_through
{
    public partial class Form1 : Form
    {
        String panelName = "panel1";
        String panelNumberString = "0";
        String newPanelNumber = "0";
        int panelNumberInt = 0;
        Panel[] panelArray;

        public Form1()
        {
            InitializeComponent();
            createPanelArray();
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

        private void changePanel(Control parent, int steps)
        {
            panelName = parent.Name.ToString();
            panelNumberString = Regex.Match(panelName, @"\d+").Value; //single numeric value in string form
            panelNumberInt = Int32.Parse(panelNumberString);//single numeric value in int form
            panelNumberInt += steps - 1;
            if (panelNumberInt < 30)
            {
                pictureBox1.Parent = panelArray[panelNumberInt];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            changePanel(pictureBox1.Parent, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            changePanel(pictureBox1.Parent, 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            changePanel(pictureBox1.Parent, 3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            changePanel(pictureBox1.Parent, 4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            changePanel(pictureBox1.Parent, 5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            changePanel(pictureBox1.Parent, 6);
        }

        private void GoButton_Click(object sender, EventArgs e)
        {

        }

        private void SlipButton_Click(object sender, EventArgs e)
        {

        }
    }
}
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
        }

        private void changePanel(Control parent, int steps)
        {
            panelName = parent.Name.ToString();
            panelNumberString = Regex.Match(panelName, @"\d+").Value; //single numeric value in string form
            panelNumberInt = Int32.Parse(panelNumberString);//single numeric value in int form
            panelNumberInt += steps;
            pictureBox1.Parent = panelArray[panelNumberInt];
        }

        private void checkPanelNumber(Control parent)
        {
            panelName = parent.Name.ToString();
            panelNumberString = Regex.Match(panelName, @"\d+").Value; //single numeric value in string form
            panelNumberInt = Int32.Parse(panelNumberString);//single numeric value in int form
            label1.Text = panelNumberString;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Parent = panel1;
            pictureBox1.Location = new Point(0, 0);
            checkPanelNumber(pictureBox1.Parent);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Parent = panel2;
            pictureBox1.Location = new Point(0, 0);
            checkPanelNumber(pictureBox1.Parent);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Parent = panel3;
            pictureBox1.Location = new Point(0, 0);
            checkPanelNumber(pictureBox1.Parent);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            changePanel(pictureBox1.Parent, 4);
        }
        private void test123(Panel panel)
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
        private void button5_Click(object sender, EventArgs e)
        {
            test123(panel1);
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void GoButton_Click(object sender, EventArgs e)
        {

        }

        private void SlipButton_Click(object sender, EventArgs e)
        {

        }
    }
}
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Slip_through
{
    public partial class Form1 : Form
    {
        String panelName = "panel2";
        String panelNumberString = "0";
        int panelNumberInt = 0;

        public Form1()
        {
            InitializeComponent();
        }
        private void checkPanelNumber(Control parent)
        {
            pictureBox1.Parent = panel1;
            panelName = parent.Name.ToString();
            panelNumberString = Regex.Match(panelName, @"\d+").Value;
            panelNumberInt = Int32.Parse(panelNumberString);
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
        }

        private void button5_Click(object sender, EventArgs e)
        {
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
namespace Slip_through
{
    public partial class Form1 : Form
    {
        String nextPanel = "panel1";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            movePiece(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            movePiece(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            movePiece(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            movePiece(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            movePiece(5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            movePiece(6);
        }

        private void GoButton_Click(object sender, EventArgs e)
        {

        }

        private void SlipButton_Click(object sender, EventArgs e)
        {

        }

        private void movePiece(int steps)
        {
            switch (steps)
            {
                case 1:
                    pictureBox1.Parent = panel1;
                    break;
                case 2:
                    pictureBox1.Parent = panel2;
                    break;
                case 3:
                    pictureBox1.Parent = panel3;
                    break;
                case 4:
                    pictureBox1.Parent = panel4;
                    break;
                case 5:
                    pictureBox1.Parent = panel5;
                    break;
                case 6:
                    pictureBox1.Parent = panel6;
                    break;
                default:
                    Console.WriteLine("Something is wrong");
                    break;
            }
            pictureBox1.Location = new Point(0, 0);
        }
    }
}
namespace Slip_through
{
    public class MyInputs
    {
        public static MyInputs instance;

        public MyInputs()
        {
            instance = this;
        }
        public void button3_Click(object sender, EventArgs e)
        {
            if (Form1.instance.panelNumberInt <= 27)
                Form1.instance.mainSequence(3);
        }//move by 3
        //CAN IT BE MOVED TO HERE? 
    }
}

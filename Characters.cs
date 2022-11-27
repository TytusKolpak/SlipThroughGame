namespace Slip_through
{
    public partial class Characters : Form
    {
        //initialized here to be able to be sent over to Form1.cs beginning so that there is only 1 source of those values
        public static Characters instance;
        public bool warriorPlays = true, archerPlays = true, wizardPlays = true, druidPlays = true;
        public Characters()
        {
            //osdeqwe
            InitializeComponent();
            instance = this;
            //nUD stands for numericUpDOwn, following lines constitute preparing values in the Characters window for each card
            nUDWarriorATT.Value = Form1.instance.S[0, 0];
            nUDWarriorDEF.Value = Form1.instance.S[0, 2];
            nUDWarriorEFF.Value = Form1.instance.S[0, 4];
             nUDWarriorHP.Value = Form1.instance.S[0, 6];

            nUDArcherATT.Value = Form1.instance.S[1, 0];
            nUDArcherDEF.Value = Form1.instance.S[1, 2];
            nUDArcherEFF.Value = Form1.instance.S[1, 4];
             nUDArcherHP.Value = Form1.instance.S[1, 6];

            nUDWizardATT.Value = Form1.instance.S[2, 0];
            nUDWizardDEF.Value = Form1.instance.S[2, 2];
            nUDWizardEFF.Value = Form1.instance.S[2, 4];
             nUDWizardHP.Value = Form1.instance.S[2, 6];

            nUDDruidATT.Value = Form1.instance.S[3, 0];
            nUDDruidDEF.Value = Form1.instance.S[3, 2];
            nUDDruidEFF.Value = Form1.instance.S[3, 4];
             nUDDruidHP.Value = Form1.instance.S[3, 6];

                nUDWolfATT.Value = Form1.instance.E[0, 0];
                nUDWolfDEF.Value = Form1.instance.E[0, 1];
                nUDWolfEFF.Value = Form1.instance.E[0, 2];
                 nUDWolfHP.Value = Form1.instance.E[0, 3];

            nUDWerewolfATT.Value = Form1.instance.E[1, 0];
            nUDWerewolfDEF.Value = Form1.instance.E[1, 1];
            nUDWerewolfEFF.Value = Form1.instance.E[1, 2];
             nUDWerewolfHP.Value = Form1.instance.E[1, 3];

            nUDCerberusATT.Value = Form1.instance.E[2, 0];
            nUDCerberusDEF.Value = Form1.instance.E[2, 1];
            nUDCerberusEFF.Value = Form1.instance.E[2, 2];
             nUDCerberusHP.Value = Form1.instance.E[2, 3];
        }
        private void buttonSetCustom_Click(object sender, EventArgs e)
        {
            bool validPlayerNumber = warriorPlays || archerPlays || wizardPlays || druidPlays;

            if (!validPlayerNumber)
            {
                MessageBox.Show("You have removed all of the characters - " +
                    "you will have nobody to play as. You cannot continue. Leave at least one character as playing to continue.",
                    "Incorrect number of players",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else // reset all data to intial values
            {
                int WATT = (int)nUDWarriorATT.Value;    //changable in Characters window
                int WDEF = (int)nUDWarriorDEF.Value;    //-||-
                int WEFF = (int)nUDWarriorEFF.Value;    //-||-
                int WHP = (int)nUDWarriorHP.Value;      //-||-
                int mWATT = Form1.instance.S[0, 1];     //set for good in top of Form1 OR can be changed if I add new numericalUpDowns - would be pretty cool but not so useful xd
                int mWDEF = Form1.instance.S[0, 3];     //-||- do change after all else fucntional changes are aplied
                int mWEFF = Form1.instance.S[0, 5];     //-||-
                int minWHP = Form1.instance.S[0, 7];    //-||-

                int AATT = (int)nUDArcherATT.Value;
                int ADEF = (int)nUDArcherDEF.Value;
                int AEFF = (int)nUDArcherEFF.Value;
                int AHP = (int)nUDArcherHP.Value;
                int mAATT = Form1.instance.S[1, 1];
                int mADEF = Form1.instance.S[1, 3]; 
                int mAEFF = Form1.instance.S[1, 5]; 
                int minAHP = Form1.instance.S[1, 7];

                int WiATT = (int)nUDWizardATT.Value;
                int WiDEF = (int)nUDWizardDEF.Value;
                int WiEFF = (int)nUDWizardEFF.Value;
                int WiHP = (int)nUDWizardHP.Value;
                int mWiATT = Form1.instance.S[2, 1];
                int mWiDEF = Form1.instance.S[2, 3]; 
                int mWiEFF = Form1.instance.S[2, 5]; 
                int minWiHP = Form1.instance.S[2, 7];

                int DATT = (int)nUDDruidATT.Value;
                int DDEF = (int)nUDDruidDEF.Value;
                int DEFF = (int)nUDDruidEFF.Value;
                int DHP = (int)nUDDruidHP.Value;
                int mDATT = Form1.instance.S[3, 1];
                int mDDEF = Form1.instance.S[3, 3]; 
                int mDEFF = Form1.instance.S[3, 5]; 
                int minDHP = Form1.instance.S[3, 7];

                int WoATT = (int)nUDWolfATT.Value;
                int WoDEF = (int)nUDWolfDEF.Value;
                int WoEFF = (int)nUDWolfEFF.Value;
                int WoHP = (int)nUDWolfHP.Value;

                int WeATT = (int)nUDWerewolfATT.Value;
                int WeDEF = (int)nUDWerewolfDEF.Value;
                int WeEFF = (int)nUDWerewolfEFF.Value;
                int WeHP = (int)nUDWerewolfHP.Value;

                int CATT = (int)nUDCerberusATT.Value;
                int CDEF = (int)nUDCerberusDEF.Value;
                int CEFF = (int)nUDCerberusEFF.Value;
                int CHP = (int)nUDCerberusHP.Value;


                bool[] WarriorwST = new bool[4] { false, false, false, false };
                bool[] ArcherwST = new bool[4] { false, false, false, false };
                bool[] WizardwST = new bool[4] { false, false, false, false };
                bool[] DruidwST = new bool[4] { false, false, false, false };

                //OVERVIEW: CREATE NEW CHARACTER -   T E M P L A T E     BASING ON THE CHANGES MADE IN THE CHARACTER WINDOW
                //send WarriorCardTemplate in / create a WarriorCardTemplate for Form1 to use (is used when new game is called)
                //can be accessed in Form1 simply by "WarriorCardTemplate"
                //has to be here, because the editing of values is carried out in characters window
                //                                        name,       att, maxAtt, def, maxDef, eff, maxEff, hp, minHp, walls, picture
                //this value is leading - is overrithing the one from Form1 after a change was done.\
                Form1.instance.WarriorCardTemplate = new("Warrior", WATT, mWATT, WDEF, mWDEF, WEFF, mWEFF, WHP, minWHP, WarriorwST, Properties.Resources.warrior);
                Form1.instance.ArcherCardTemplate = new("Archer", AATT, mAATT, ADEF, mADEF, AEFF, mAEFF, AHP, minAHP, ArcherwST, Properties.Resources.archer);
                Form1.instance.WizardCardTemplate = new("Wizard", WiATT, mWiATT, WiDEF, mWiDEF, WiEFF, mWiEFF, WiHP, minWiHP, WizardwST, Properties.Resources.wizard);
                Form1.instance.DruidCardTemplate = new("Druid", DATT, mDATT, DDEF, mDDEF, DEFF, mDEFF, DHP, minDHP, DruidwST, Properties.Resources.druid);
                Form1.instance.WolfCardTemplate = new("Wolf", WoATT, WoDEF, WoEFF, WoHP, Properties.Resources.wolf);
                Form1.instance.WerewolfCardTemplate = new("Werewolf", WeATT, WeDEF, WeEFF, WeHP, Properties.Resources.werewolf);
                Form1.instance.CerberusCardTemplate = new("Cerberus", CATT, CDEF, CEFF, CHP, Properties.Resources.cerberus);

                MessageBox.Show("Values have been updated. To use newly created cards begin a new game.", "Customization", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void WarriorPlaysCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxWarrior.Visible = !pictureBoxWarrior.Visible;
            tableLayoutPanelWarriorStats.Visible = !tableLayoutPanelWarriorStats.Visible;
            warriorPlays = !warriorPlays;
            //used in Form1 to maintain the cycle of players
        }
        private void ArcherPlaysCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxArcher.Visible = !pictureBoxArcher.Visible;
            tableLayoutPanelArcherStats.Visible = !tableLayoutPanelArcherStats.Visible;
            archerPlays = !archerPlays;
        }
        private void WizardPlaysCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxWizard.Visible = !pictureBoxWizard.Visible;
            tableLayoutPanelWizardStats.Visible = !tableLayoutPanelWizardStats.Visible;
            wizardPlays = !wizardPlays;
        }
        private void DruidPlaysCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pictureBoxDruid.Visible = !pictureBoxDruid.Visible;
            tableLayoutPanelDruidStats.Visible = !tableLayoutPanelDruidStats.Visible;
            druidPlays = !druidPlays;
        }
    }
}

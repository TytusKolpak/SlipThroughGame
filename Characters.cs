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
    public partial class Characters : Form
    {
        //initialized here to be able to be sent over to Form1.cs beginning so that there is only 1 source of those values
        public static Characters instance;
        public bool warriorPlays = true, archerPlays = true, wizardPlays = true, druidPlays = true;
        public Characters()
        {
            //osd
            InitializeComponent();
            instance = this;
            //nUD stands for numericUpDOwn, following lines constitute preparing values in the Characters window for each card
            nUDWarriorATT.Value = Form1.instance.WATT;
            nUDWarriorDEF.Value = Form1.instance.WDEF;
            nUDWarriorEFF.Value = Form1.instance.WEFF;
            nUDWarriorHP.Value = Form1.instance.WHP;

            nUDArcherATT.Value = Form1.instance.AATT;
            nUDArcherDEF.Value = Form1.instance.ADEF;
            nUDArcherEFF.Value = Form1.instance.AEFF;
            nUDArcherHP.Value = Form1.instance.AHP;

            nUDWizardATT.Value = Form1.instance.WiATT;
            nUDWizardDEF.Value = Form1.instance.WiDEF;
            nUDWizardEFF.Value = Form1.instance.WiEFF;
            nUDWizardHP.Value = Form1.instance.WiHP;

            nUDDruidATT.Value = Form1.instance.DATT;
            nUDDruidDEF.Value = Form1.instance.DDEF;
            nUDDruidEFF.Value = Form1.instance.DEFF;
            nUDDruidHP.Value = Form1.instance.DHP;

            nUDWolfATT.Value = Form1.instance.WoATT;
            nUDWolfDEF.Value = Form1.instance.WoDEF;
            nUDWolfEFF.Value = Form1.instance.WoEFF;
            nUDWolfHP.Value = Form1.instance.WoHP;

            nUDWerewolfATT.Value = Form1.instance.WeATT;
            nUDWerewolfDEF.Value = Form1.instance.WeDEF;
            nUDWerewolfEFF.Value = Form1.instance.WeEFF;
            nUDWerewolfHP.Value = Form1.instance.WeHP;

            nUDCerberusATT.Value = Form1.instance.CATT;
            nUDCerberusDEF.Value = Form1.instance.CDEF;
            nUDCerberusEFF.Value = Form1.instance.CEFF;
            nUDCerberusHP.Value = Form1.instance.CHP;
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
                int mWATT = Form1.instance.mWATT;       //set for good in top of Form1
                int mWDEF = Form1.instance.mWDEF;       //-||-
                int mWEFF = Form1.instance.mWEFF;       //-||-
                int minWHP = Form1.instance.minWHP;     //-||-

                int AATT = (int)nUDArcherATT.Value;
                int ADEF = (int)nUDArcherDEF.Value;
                int AEFF = (int)nUDArcherEFF.Value;
                int AHP = (int)nUDArcherHP.Value;
                int mAATT = Form1.instance.mAATT;
                int mADEF = Form1.instance.mADEF;
                int mAEFF = Form1.instance.mAEFF;
                int minAHP = Form1.instance.minAHP;

                int WiATT = (int)nUDWizardATT.Value;
                int WiDEF = (int)nUDWizardDEF.Value;
                int WiEFF = (int)nUDWizardEFF.Value;
                int WiHP = (int)nUDWizardHP.Value;
                int mWiATT = Form1.instance.mWiATT;
                int mWiDEF = Form1.instance.mWiDEF;
                int mWiEFF = Form1.instance.mWiEFF;
                int minWiHP = Form1.instance.minWiHP;

                int DATT = (int)nUDDruidATT.Value;
                int DDEF = (int)nUDDruidDEF.Value;
                int DEFF = (int)nUDDruidEFF.Value;
                int DHP = (int)nUDDruidHP.Value;
                int mDATT = Form1.instance.mDATT;
                int mDDEF = Form1.instance.mDDEF;
                int mDEFF = Form1.instance.mDEFF;
                int minDHP = Form1.instance.minDHP;

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

                bool[] wSP = new bool[4];//walls Slipped Through
                                         //send WarriorCardTemplate in / create a WarriorCardTemplate for Form1 to use (is used when new game is called)
                                         //can be accessed in Form1 simply by "WarriorCardTemplate"
                                         //has to be here, because the editing of values is carried out in characters window
                                         //                                        name,       att, maxAtt, def, maxDef, eff, maxEff, hp, minHp, walls, picture
                                         //this value is leading - is overrithing the one from Form1 after a change was done.\
                Form1.instance.WarriorCardTemplate = new("Warrior", WATT, mWATT, WDEF, mWDEF, WEFF, mWEFF, WHP, minWHP, wSP, Properties.Resources.warrior);
                Form1.instance.ArcherCardTemplate = new("Archer", AATT, mAATT, ADEF, mADEF, AEFF, mAEFF, AHP, minAHP, wSP, Properties.Resources.archer);
                Form1.instance.WizardCardTemplate = new("Wizard", WiATT, mWiATT, WiDEF, mWiDEF, WiEFF, mWiEFF, WiHP, minWiHP, wSP, Properties.Resources.wizard);
                Form1.instance.DruidCardTemplate = new("Druid", DATT, mDATT, DDEF, mDDEF, DEFF, mDEFF, DHP, minDHP, wSP, Properties.Resources.druid);
                Form1.instance.WolfCardTemplate = new("Wolf", WoATT, 0, WoDEF, 0, WoEFF, 0, WoHP, 5, wSP, Properties.Resources.wolf);
                Form1.instance.WerewolfCardTemplate = new("Werewolf", WeATT, 0, WeDEF, 0, WeEFF, 0, WeHP, 0, wSP, Properties.Resources.werewolf);
                Form1.instance.CerberusCardTemplate = new("Cerberus", CATT, 0, CDEF, 0, CEFF, 0, CHP, 0, wSP, Properties.Resources.cerberus);

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

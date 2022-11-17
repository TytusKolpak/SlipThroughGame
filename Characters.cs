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
        public bool warriorPlays = true, archerPlays = true, wizardPlays;
        public Characters()
        {
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
            int WATT = (int)nUDWarriorATT.Value;
            int WDEF = (int)nUDWarriorDEF.Value;
            int WEFF = (int)nUDWarriorEFF.Value;
            int WHP = (int)nUDWarriorHP.Value;

            int AATT = (int)nUDArcherATT.Value;
            int ADEF = (int)nUDArcherDEF.Value;
            int AEFF = (int)nUDArcherEFF.Value;
            int AHP = (int)nUDArcherHP.Value;

            int WiATT = (int)nUDWizardATT.Value;
            int WiDEF = (int)nUDWizardDEF.Value;
            int WiEFF = (int)nUDWizardEFF.Value;
            int WiHP = (int)nUDWizardHP.Value;

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

            //send WarriorCardTemplate in / create a WarriorCardTemplate for Form1 to use (is used when new game is called)
            //can be accessed in Form1 simply by "WarriorCardTemplate"
            Form1.instance.WarriorCardTemplate = new("Warrior", WATT, WDEF, WEFF, WHP, Properties.Resources.warrior);
            Form1.instance.ArcherCardTemplate = new("Archer", AATT, ADEF, AEFF, AHP, Properties.Resources.archer);
            Form1.instance.WizardCardTemplate = new("Wizard", WiATT, WiDEF, WiEFF, WiHP, Properties.Resources.wizard);
            Form1.instance.WolfCardTemplate = new("Wolf", WoATT, WoDEF, WoEFF, WoHP, Properties.Resources.wolf);
            Form1.instance.WerewolfCardTemplate = new("Werewolf", WeATT, WeDEF, WeEFF, WeHP, Properties.Resources.werewolf);
            Form1.instance.CerberusCardTemplate = new("Cerberus", CATT, CDEF, CEFF, CHP, Properties.Resources.cerberus);

            MessageBox.Show("Values have been updated. To use newly created cards begin a new game.", "Customization", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void WarriorPlaysCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //send info to Form1 about whoo plays
            pictureBoxWarrior.Visible = !pictureBoxWarrior.Visible;
            tableLayoutPanelWarriorStats.Visible = !tableLayoutPanelWarriorStats.Visible;
            warriorPlays = !warriorPlays;
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
    }
}

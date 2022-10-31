using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Slip_through
{
    public partial class Form1 : Form
    {
        bool gameOver = false;
        int panelNumberInt = 0;
        int playerNr = 0;
        int turnCounter = 1;
        String panelName = "panel1";
        String panelNumberString = "0";

        Panel[] panelArray = System.Array.Empty<Panel>();                   //resolves CA1825 (and now allocates less memory)
        PictureBox[] pictureBoxArray = System.Array.Empty<PictureBox>();    //resolves CA1825
        PictureBox currentPictureBox;

        CombatCard WizardCard = new("Wizard", 3, 1, 1, 8, Properties.Resources.wizard);         //no need to create objects as: 
        CombatCard WarriorCard = new("Warrior", 2, 1, 2, 10, Properties.Resources.warrior);    //Cat cat = new Cat()
        CombatCard ArcherCard = new("Archer", 1, 1, 3, 9, Properties.Resources.archer);        //Cat cat = () is enough
        CombatCard WolfCard = new("wolf", 3, 0, 2, 5, Properties.Resources.wolf);
        CombatCard WerewolfCard = new("werewolf", 4, 2, 3, 10, Properties.Resources.werewolf);
        CombatCard CerberusCard = new("cerberus", 5, 4, 4, 15, Properties.Resources.cerberus);
        CombatCard player;
        CombatCard enemy;

        public Form1()//working and complete (so far)
        {
            //function executes once in the beginning of the game
            InitializeComponent();
            createArrays();
            tableLayoutPanelEnemy.Visible = false;
            pictureBoxWolf.Visible = false;
            pictureBoxWerewolf.Visible = false;
            pictureBoxCerberus.Visible = false;
            labelCombatLog.Visible = false;
            labelCombatLog.Visible = true;
            player = WarriorCard;
            currentPictureBox = pictureBoxWarrior;
            loadPlayerInfo();
        }
        private void createArrays()//working and complete
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
            pictureBoxArray = new PictureBox[6]
            {
                pictureBoxWarrior,
                pictureBoxArcher,
                pictureBoxWizard,
                pictureBoxWolf,
                pictureBoxWerewolf,
                pictureBoxCerberus,
            };
        }
        private void movementElement(int steps)//working and complete
        {
            panelName = currentPictureBox.Parent.Name.ToString();
            panelNumberString = Regex.Match(panelName, @"\d+").Value; //single numeric value in string form
            panelNumberInt = Int32.Parse(panelNumberString);//single numeric value in int form
            if (panelNumberInt + steps <= 30) //will have a place to end on
            {
                for (int i = 0; i < steps; i++)
                {
                    Thread.Sleep(100);
                    if (SlipBox.Checked == true && panelNumberInt % 5 == 0 && panelNumberInt >= 10) // that is wrong
                    {
                        panelNumberInt -= 9;
                    }
                    else
                    {
                        panelNumberInt++; //mark next panel as destination
                    }

                    //place the player on destination tile, on it, and update it
                    currentPictureBox.Parent = panelArray[panelNumberInt - 1]; //-1 is because panel1 has index 0 : x on x-1
                    currentPictureBox.BringToFront();
                    currentPictureBox.Update();

                    //check for the end of the game
                    if (panelNumberInt == 30)
                    {
                        gameOver = true;
                        label1.Text = player.name + " won in " +
                            turnCounter + " turns, with " +
                            player.deathCounter + " deaths.";
                    }
                }
            }
        }
        private void combatElement()
        {
            //chosing the enemy depending on how far the player is 
            if (panelNumberInt <= 10)
                enemy = WolfCard;
            else if (panelNumberInt >= 11 && panelNumberInt <= 20)
                enemy = WerewolfCard;
            else
                enemy = CerberusCard;

            //both lines are necessary to create a random integer between 1 and 6 including both
            Random random = new();
            int randomNumber = random.Next(1, 7);

            if (panelNumberInt == 5) // just a test, change later to 1-4 dice throw
            {
                loadEnemyInfo();
                setCombatText("A " + enemy.name + " attacks you. You have to fight it.");
                randomNumber = random.Next(1, 7);
                setCombatText("You attack. You roll= " + randomNumber + ". Your EFF + roll= " + (randomNumber + player.effectiveness));
            }

            //labelCombatLog.Text = ""; //clear this label




            //////////////////////////////////////////////
            //pictureBoxArray[2].Parent //wolf's tile

            

            //if (randomNumber <= 4)
            //{
            //    CombatCard enemy;



            //    //executing combat as long as one of them is alive
            //    if (player.hitPoints > 0 && enemy.hitPoints > 0)
            //    {
            //        int chosen_value = 3; // let player roll for a number or choose it, let's start with choosing for tests
            //        CombatCard.combatRound(player, enemy, chosen_value);
            //    }
            //    else if (enemy.hitPoints <= 0){  //if enemy dies then you chose a reward
            //        tableLayoutPanelEnemy.Visible = false;
            //        CombatCard.choseReward();
            //    }
            //    else                            //if the player dies he revives with full healt 10 tiles back
            //    {
            //        player.deathCounter++;
            //        panelNumberInt -= 10;

            //        //ensure the player stays on the board
            //        if (panelNumberInt < 0)
            //            panelNumberInt = 0;

            //        //move to the back that many tiles
            //        currentPictureBox.Parent = panelArray[panelNumberInt - 1];
            //        //get back all hit points
            //        if (player.name == "Wizard")
            //            player.hitPoints = 8;
            //        else if (player.name == "Warrior")
            //            player.hitPoints = 10;
            //        else if (player.name == "Archer")
            //            player.hitPoints = 9;
            //    }
            //}
            //otherwise do nothing, you avoided the danger
        }
        private void nextPlayer()//working and complete
        {
            //change player to the next one in a loop
            if (playerNr < 2)
                playerNr++;
            else
            {
                turnCounter++;
                playerNr = 0;
            }

            //assign the object of the card(player) whose turn it is now to the currentCard for easy - uniform access
            if (playerNr == 0)
                player = WarriorCard;
            else if (playerNr == 1)
                player = ArcherCard;
            else
                player = WizardCard;
        }
        private void loadPlayerInfo()//working and complete
        {
            //load that player's info to the window elements to be shown
            pictureBoxPlayer.Image = player.bitmapImage;                    //big main picture on the right
            labelPlayerAttack.Text = player.attack.ToString();              //following stats
            labelPlayerDefense.Text = player.defence.ToString();
            labelPlayerEffectiveness.Text = player.effectiveness.ToString();
            labelPlayerHitPoints.Text = player.hitPoints.ToString();
            label1.Text = player.name;                                      //name of the current player on the bottom of window

            currentPictureBox = pictureBoxArray[playerNr];                                  //this is complicated
        }
        private void loadEnemyInfo()
        {
            tableLayoutPanelEnemy.Visible = true;
            pictureBoxEnemy.Image = enemy.bitmapImage;
            labelEnemyAttack.Text = enemy.attack.ToString();
            labelEnemyDefense.Text = enemy.defence.ToString();
            labelEnemyEffectiveness.Text = enemy.effectiveness.ToString();
            labelEnemyHitPoints.Text = enemy.hitPoints.ToString();
            tableLayoutPanelEnemy.Update(); //absolutely neccessary
        }
        private void setCombatText(String message)
        {
            labelCombatLog.Text = message;
            labelCombatLog.Update();
            Thread.Sleep(3000);
        }
        private void mainSequence(int diceThrow)
        {
            movementElement(diceThrow);      //carry outh all actions relatet do just movement
            combatElement();

            nextPlayer();               //switch to the next player (should be at the end of sequence)

            if (!gameOver)
                loadPlayerInfo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!gameOver)
                mainSequence(1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!gameOver)
                mainSequence(2);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (!gameOver)
                mainSequence(3);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (!gameOver)
                mainSequence(4);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (!gameOver)
                mainSequence(5);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (!gameOver)
                mainSequence(6);
        }
    }
}
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Slip_through
{
    public partial class Form1 : Form
    {
        bool rewardEarned = false;
        bool gameOver = false;
        bool fought = false;

        int iterationMs = 100;
        int panelNumberInt = 0;
        int playerNr = 0;
        int turnCounter = 1;
        String panelName = "panel1";
        String panelNumberString = "0";

        Panel[] panelArray = System.Array.Empty<Panel>();                   //resolves CA1825 (and now allocates less memory)
        PictureBox[] pictureBoxArray = System.Array.Empty<PictureBox>();    //resolves CA1825
        PictureBox currentPictureBox;

        CombatCard WarriorCard = new("Warrior", 2, 1, 2, 10, Properties.Resources.warrior);    
        CombatCard ArcherCard = new("Archer", 1, 1, 3, 9, Properties.Resources.archer);        
        CombatCard WizardCard = new("Wizard", 3, 1, 1, 8, Properties.Resources.wizard);        
        CombatCard WolfCard = new("Wolf", 3, 0, 0, 5, Properties.Resources.wolf);
        CombatCard WerewolfCard = new("Werewolf", 4, 2, 2, 10, Properties.Resources.werewolf);
        CombatCard CerberusCard = new("Cerberus", 5, 4, 4, 15, Properties.Resources.cerberus);
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
            buttonAddATT.Visible = false;
            buttonAddDEF.Visible = false;
            buttonAddEFF.Visible = false;

            player = WarriorCard;
            currentPictureBox = pictureBoxWarrior;
            displayPlayerInfo();
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
                        labelCombatLog.Text = player.name + " won in " +
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

            fought = false;

            if (panelNumberInt == 5)                        // just a test means to fight always on 5th tile, change later to 1-4 dice throw and anywhere
            {
                fought = true;
                pictureBoxWolf.Visible = true;
                pictureBoxWolf.Update();
                displayEnemyInfo();
                setCombatText(enemy.name + " attacks you. You have to fight it.");

                while (player.hitPoints > 0 && enemy.hitPoints > 0)
                {
                    fightSequence();
                }

                if (enemy.hitPoints <= 0)                   //enemy died - player won
                {
                    WolfCard = new("Wolf", 3, 0, 0, 5, Properties.Resources.wolf);
                    pictureBoxWolf.Visible = false;
                    rewardEarned = true;
                    tableLayoutPanelEnemy.Visible = false;  //erase dead opponent
                }

                if (player.hitPoints <=0)                                      //player died
                {
                    setCombatText("You died.");
                    player.deathCounter++;
                    panelNumberInt -= 10;
                    panelNumberInt = panelNumberInt < 1 ? 1 : panelNumberInt;  //if tile number it's less than 1, make it 1 (index 0)

                    currentPictureBox.Parent = panelArray[panelNumberInt - 1]; //move to the back that many tiles

                    //get back all hit points - heal to full
                    if (player.name == "Wizard")
                        player.hitPoints = 8;
                    else if (player.name == "Warrior")
                        player.hitPoints = 10;
                    else if (player.name == "Archer")
                        player.hitPoints = 9;


                    nextPlayer();               //switch to the next player (should be at the end of sequence)
                    if (!gameOver)
                        displayPlayerInfo();
                }


                labelCombatLog.Text = " ";              //clear text
            }


            if (!fought)
            {
                nextPlayer();               //switch to the next player (should be at the end of sequence)
                if (!gameOver)
                    displayPlayerInfo();
            }
        }
        private void fightSequence()//working and enough for now
        {
            //both lines are necessary to create a random integer between 1 and 6 including both
            Random random = new();
            int diceRoll;
            int damage;

            //player attack sequence
            setCombatText("You attempt to attack your enemy. Your ATT + EFF = " + (player.attack + player.effectiveness));

            diceRoll = random.Next(1, 7);
            //if (player.effectiveness + diceRoll > enemy.effectiveness) //alternate
            if (player.effectiveness + player.attack > enemy.effectiveness + diceRoll) //player manages to attack
            {
                damage = player.attack - enemy.defence;
                damage = damage >= 0 ? damage : 0;      //it can only go down to 0, no lower
                enemy.hitPoints -= damage;              //enemy loses health
                setCombatText("Your ATT + EFF = " + (player.attack + player.effectiveness) + " > " + enemy.name+ "'s EFF + roll = " + (enemy.effectiveness + diceRoll) + ", so you succeed.");
                setCombatText("You deal your ATT - their DEF = " + (player.attack - enemy.defence) + " damage.");
                displayEnemyInfo();                     //show changed stats and refresh the view
            }
            else                                        //player fails to attack, nothing happens
                setCombatText("Your ATT + EFF = " + (player.attack + player.effectiveness) + " <= " + enemy.name + "'s EFF + roll = " + (enemy.effectiveness + diceRoll) + ", so you fail.");

            //enemy attack sequence
            diceRoll = random.Next(1, 7);
            setCombatText(enemy.name + " attacks you it rolls for = " + diceRoll + ". Its EFF + roll= " + (enemy.effectiveness + diceRoll));

            if (enemy.effectiveness + diceRoll > player.effectiveness) //It manages to attack the player
            {
                damage = enemy.attack - player.defence;
                damage = damage >= 0 ? damage : 0;
                player.hitPoints -= damage;
                setCombatText(enemy.name + "'s EFF + roll = " + (enemy.effectiveness + diceRoll) + " > your EFF = " + player.effectiveness + ", so it succeeds");
                setCombatText(enemy.name + " deals their ATT - your DEF = " + (enemy.attack - player.defence) + " damage.");
                displayPlayerInfo();                     //show changed stats and refresh the view
            }
            else
            {
                setCombatText(enemy.name + "'s EFF + roll = " + (enemy.effectiveness + diceRoll) + " <= your EFF = " + player.effectiveness + ", so it fails");
            }
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
        private void displayPlayerInfo()//working and complete
        {
            //load that player's info to the window elements to be shown
            pictureBoxPlayer.Image = player.bitmapImage;                    //big main picture on the right
            labelPlayerAttack.Text = player.attack.ToString();              //following stats
            labelPlayerDefense.Text = player.defence.ToString();
            labelPlayerEffectiveness.Text = player.effectiveness.ToString();
            labelPlayerHitPoints.Text = player.hitPoints.ToString();
            tableLayoutPanelPlayer.Update();

            currentPictureBox = pictureBoxArray[playerNr];                                  //this is complicated
        }
        private void displayEnemyInfo()
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
            Thread.Sleep(iterationMs);             //slider can be setting this value, same as the sleep before / you can check every 1000ms if there was a click or something
        }
        private void getReward()
        {
            if (rewardEarned)
            {
                buttonAddATT.Visible = true;
                buttonAddDEF.Visible = true;
                buttonAddEFF.Visible = true;
            }
        }
        private void mainSequence(int diceRoll)
        {
            movementElement(diceRoll);      //carry outh all actions relatet do just movement
            combatElement();
            getReward();                    //this works but scatters the code, might need reshaping


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

        private void buttonAddATT_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
            {
                player.attack += 1;
            }
            else if (panelNumberInt <= 20)
            {
                player.attack += 2;
            }
            else
            {
                player.attack += 3;
            }
            displayPlayerInfo();
            buttonAddATT.Visible = false;
            buttonAddDEF.Visible = false;
            buttonAddEFF.Visible = false;

            Thread.Sleep(iterationMs);

            nextPlayer();               //switch to the next player (should be at the end of sequence)
            if (!gameOver)
                displayPlayerInfo();
        }

        private void buttonAddDEF_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
            {
                player.defence += 1;
            }
            else if (panelNumberInt <= 20)
            {
                player.defence += 2;
            }
            else
            {
                player.defence += 3;
            }
            displayPlayerInfo();
            buttonAddATT.Visible = false;
            buttonAddDEF.Visible = false;
            buttonAddEFF.Visible = false;

            Thread.Sleep(iterationMs);

            nextPlayer();               //switch to the next player (should be at the end of sequence)
            if (!gameOver)
                displayPlayerInfo();
        }

        private void buttonAddEFF_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
            {
                player.effectiveness += 1;
            }
            else if (panelNumberInt <= 20)
            {
                player.effectiveness += 2;
            }
            else
            {
                player.effectiveness += 3;
            }
            displayPlayerInfo();
            buttonAddATT.Visible = false;
            buttonAddDEF.Visible = false;
            buttonAddEFF.Visible = false;

            Thread.Sleep(iterationMs);

            nextPlayer();               //switch to the next player (should be at the end of sequence)
            if (!gameOver)
                displayPlayerInfo();
        }
    }
}
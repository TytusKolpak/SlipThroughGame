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

        int iterationMs = 100;
        int panelNumberInt = 0;
        int playerNr = 0;
        int turnCounter = 1;
        String panelName = "panel1";
        String panelNumberString = "0";

        Panel[] panelArray = System.Array.Empty<Panel>();                   //resolves CA1825 (and now allocates less memory)
        PictureBox[] pictureBoxArray = System.Array.Empty<PictureBox>();    //resolves CA1825
        PictureBox currentPlayerPictureBox;
        PictureBox currentEnemyPictureBox;

        CombatCard WarriorCard = new("Warrior", 2, 1, 2, 10, Properties.Resources.warrior);
        CombatCard ArcherCard = new("Archer", 1, 1, 3, 9, Properties.Resources.archer);
        CombatCard WizardCard = new("Wizard", 3, 1, 1, 8, Properties.Resources.wizard);
        CombatCard WolfCard = new("Wolf", 3, 0, 0, 5, Properties.Resources.wolf);
        CombatCard WerewolfCard = new("Werewolf", 4, 2, 2, 10, Properties.Resources.werewolf);
        CombatCard CerberusCard = new("Cerberus", 5, 4, 4, 15, Properties.Resources.cerberus);
        CombatCard player;
        CombatCard enemy;

        Random random = new();

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
            setAddButtonsVisibility(false);

            player = WarriorCard;
            currentPlayerPictureBox = pictureBoxWarrior;
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
            panelName = currentPlayerPictureBox.Parent.Name.ToString();
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
                    currentPlayerPictureBox.Parent = panelArray[panelNumberInt - 1]; //-1 is because panel1 has index 0 : x on x-1
                    currentPlayerPictureBox.BringToFront();
                    currentPlayerPictureBox.Update();

                    //check for the end of the game
                    if (panelNumberInt == 30)
                    {
                        gameOver = true;
                        labelCombatLog.Text = player.name + " won. Turns: " + turnCounter + ". Deaths: " + player.deathCounter + ".";
                    }
                }
            }
        }
        private void combatElement(int steps)
        {
            //chosing the enemy depending on how far the player is 
            if (panelNumberInt <= 10)
            {
                currentEnemyPictureBox = pictureBoxArray[3];
                enemy = WolfCard;
            }
            else if (panelNumberInt <= 20)
            {
                currentEnemyPictureBox = pictureBoxArray[4];
                enemy = WerewolfCard;
            }
            else
            {
                currentEnemyPictureBox = pictureBoxArray[5];
                enemy = CerberusCard;
            }


            int diceRoll = random.Next(1, 7);

            if (diceRoll <= steps)//if player rolls more than what they moved then they are not found out (move less -> less chance of a fight)
            {
                currentEnemyPictureBox.Parent = panelArray[panelNumberInt - 1];
                currentEnemyPictureBox.Visible = true;
                currentEnemyPictureBox.Update();

                displayEnemyInfo();
                setCombatText(enemy.name + " attacks you. You have to fight it.");

                while (player.hitPoints > 0 && enemy.hitPoints > 0)             //carry out full sequence until someone dies
                    fightSequence();

                if (enemy.hitPoints <= 0)                                       //enemy died - player won
                {
                    setCombatText(player.name + " killed the " + enemy.name);
                    rewardEarned = true;                                        //enable adding stat points later on
                }

                if (player.hitPoints <= 0)                                      //player died
                {
                    setCombatText(enemy.name + " killed the " + player.name);
                    player.deathCounter++;                                      //keep track of how many deaths each player has
                    currentPlayerPictureBox.Parent = panelArray[0];             //move player to the first tile

                    if (player.name == "Wizard")                                //get back all hit points - heal to full
                        player.hitPoints = 8;
                    else if (player.name == "Warrior")
                        player.hitPoints = 10;
                    else if (player.name == "Archer")
                        player.hitPoints = 9;
                }

                enemy.hitPoints = enemy.maxHP;                              //Set health of the enemy back to max for next fight
                currentEnemyPictureBox.Visible = false;                     //opponents either flee or die after a battle
                tableLayoutPanelEnemy.Visible = false;                      //old enemies are never encountered again (idk if correct)
            }
        }
        private void fightSequence()//working and enough for now
        {
            int diceRoll;
            int damage;

            if (player.hitPoints > 0)                       //carry out player attack if they are alive
            {
                diceRoll = random.Next(1, 7);
                if (player.effectiveness + player.attack > enemy.effectiveness + diceRoll) //player manages to attack the enemy
                {
                    damage = player.attack - enemy.defence;
                    damage = damage >= 0 ? damage : 0;      //it can only go down to 0, no lower
                    enemy.hitPoints -= damage;              //enemy loses health
                    displayEnemyInfo();                     //show changed stats and refresh the view
                    setCombatText("You succesfully attacked");
                }
                else
                    setCombatText("You failed to attack");
            }

            if (enemy.hitPoints > 0)                        //carry out enemy attack if they are alive
            {
                diceRoll = random.Next(1, 7);
                if (enemy.effectiveness + diceRoll > player.effectiveness) //enemy manages to attack the player
                {
                    damage = enemy.attack - player.defence;
                    damage = damage >= 0 ? damage : 0;
                    player.hitPoints -= damage;
                    displayPlayerInfo();                     //show changed stats and refresh the view
                    setCombatText(enemy.name + " succesfully attacked");
                }
                else
                    setCombatText(enemy.name + " failed to attack");
            }
        }
        private void nextPlayer()//working and complete
        {
            //temporary change to 1 player for testing purposes

            //change player to the next one in a loop
            //if (playerNr < 2)
            //    playerNr++;
            //else
            //{
            //    turnCounter++;
            //    playerNr = 0;
            //}

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

            currentPlayerPictureBox = pictureBoxArray[playerNr];                                  //this is complicated
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
            setAddButtonsVisibility(true);          //show the buttons related to increasing stats
            setMovementButtonsVisibility(false);    //hide the buttons related to movement
        }
        private void setMovementButtonsVisibility(bool logicValue)
        {
            button1.Visible = logicValue;
            button2.Visible = logicValue;
            button3.Visible = logicValue;
            button4.Visible = logicValue;
            button5.Visible = logicValue;
            button6.Visible = logicValue;
        }
        private void setAddButtonsVisibility(bool logicValue)
        {
            buttonAddATT.Visible = logicValue;
            buttonAddDEF.Visible = logicValue;
            buttonAddEFF.Visible = logicValue;
        }
        private void endRewardCollection()
        {
            displayPlayerInfo();
            setAddButtonsVisibility(false);
            setMovementButtonsVisibility(true);
            Thread.Sleep(iterationMs);

            nextPlayer();
            displayPlayerInfo();    //of the next player
        }
        private void mainSequence(int distanceChoice)
        {
            movementElement(distanceChoice);            //carry out all actions related to movement

            combatElement(distanceChoice);              //carry out all actions related to combat

            if (gameOver)
            {
                setAddButtonsVisibility(false);
                setMovementButtonsVisibility(false);
                //set play again button isibility true?
            }
            else
            {
                //next player should load only after the due reward has been collected (it's due only after an enemy is defeated)
                if (rewardEarned)
                    getReward();                            //a chain of events leading to waiting for user's choice of reward
                else                                        //the reward is not due, no need to wait for it's collection, load next player
                {
                    nextPlayer();
                    displayPlayerInfo();
                }

                rewardEarned = false;                       //to reset ability to access reward
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainSequence(1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            mainSequence(2);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            mainSequence(3);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            mainSequence(4);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            mainSequence(5);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            mainSequence(6);
        }
        private void buttonAddATT_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
                player.attack += 1;
            else if (panelNumberInt <= 20)
                player.attack += 2;
            else
                player.attack += 3;

            endRewardCollection();
        }
        private void buttonAddDEF_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
                player.defence += 1;
            else if (panelNumberInt <= 20)
                player.defence += 2;
            else
                player.defence += 3;

            endRewardCollection();
        }
        private void buttonAddEFF_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
                player.effectiveness += 1;
            else if (panelNumberInt <= 20)
                player.effectiveness += 2;
            else
                player.effectiveness += 3;

            endRewardCollection();
        }
    }
}
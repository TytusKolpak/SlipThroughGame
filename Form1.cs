using System;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Slip_through
{
    public partial class Form1 : Form
    {
        //try let player choose a different amout of players like 1-8 (so that they will fill the board)
        //with 1-3 players make tile 2x2, and 4-8 make it 3x3

        //try to make the values of combat card cusomable for the player - both of them can be in menu bar or something

        bool nowMovement = true;
        // used to determine weather keyboard button presses should be registered as movement or ignored (when witing for player to collect reward)

        bool died = false;
        bool fought = false;
        bool gameOver = false;
        int iterationMs = 200;
        int panelNumberInt = 0;
        int playerNr = 0;
        int turnCounter = 1;
        string panelName = "panel1";
        string panelNumberString = "0";
        string combatText = "";
        Panel[] panelArray = System.Array.Empty<Panel>();
        PictureBox[] pictureBoxArray = System.Array.Empty<PictureBox>();
        PictureBox currentPlayerPictureBox;
        PictureBox currentEnemyPictureBox;
        CombatCard WarriorCard = new("Warrior", 2, 1, 2, 10, Properties.Resources.warrior);
        CombatCard ArcherCard = new("Archer", 1, 1, 3, 10, Properties.Resources.archer);
        CombatCard WizardCard = new("Wizard", 3, 0, 2, 10, Properties.Resources.wizard);
        CombatCard WolfCard = new("Wolf", 3, 0, 5, 5, Properties.Resources.wolf);
        CombatCard WerewolfCard = new("Werewolf", 4, 1, 6, 7, Properties.Resources.werewolf);
        CombatCard CerberusCard = new("Cerberus", 5, 3, 8, 10, Properties.Resources.cerberus);
        CombatCard player;
        CombatCard enemy;
        Random random = new();

        public Form1()//working and complete (so far)
        {
            InitializeComponent();
            createArrays();
            tableLayoutPanelEnemy.Visible = false;
            pictureBoxWolf.Visible = false;
            pictureBoxWerewolf.Visible = false;
            pictureBoxCerberus.Visible = false;
            labelResults.Visible = false;
            labelResults.Visible = true;
            flowLayoutLongLog.Visible = false;
            setAddButtonsVisibility(false);
            buttonOK.Visible = false;
            labelResults.Text = "";

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
            fought = false;
            panelName = currentPlayerPictureBox.Parent.Name.ToString();
            panelNumberString = Regex.Match(panelName, @"\d+").Value;   //single numeric value in string form
            panelNumberInt = Int32.Parse(panelNumberString);            //single numeric value in int form

            labelResults.Text = " ";

            if (panelNumberInt + steps <= 30)                           //will have a place to end on
            {
                for (int i = 0; i < steps; i++)
                {
                    Thread.Sleep(100);
                    if (SlipBox.Checked == true && panelNumberInt % 5 == 0 && panelNumberInt >= 10)//enable at 10,15,20,25 rank, at 5 there is no up to go and at 30 you win
                    {
                        panelNumberInt -= 9;                            // that is going up a file

                        if (player.name == "Wizard")                    //if player slips, then give class specific bonuses               
                            player.attack++;
                        else if (player.name == "Warrior")
                            player.defence++;
                        else if (player.name == "Archer")
                            player.effectiveness++;

                        labelResults.Text = player.name + " slipped and gains +1 point to their class' attribute";
                    }
                    else
                    {
                        panelNumberInt++;                               //just mark next panel as destination
                    }

                    //place the player on destination tile and update it
                    currentPlayerPictureBox.Parent = panelArray[panelNumberInt - 1]; //-1 is because panel1 has index 0 : x on x-1
                    currentPlayerPictureBox.Update();
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

            //if player rolls more than what they moved then they are not found out (move less -> less chance of a fight)
            if (diceRoll < steps)// <= enables fighting at moving by 1 inf there is < then moving 1 is 100% safe
            {
                //FIGHT
                flowLayoutLongLog.Visible = false;
                currentEnemyPictureBox.Parent = panelArray[panelNumberInt - 1];
                currentEnemyPictureBox.Visible = true;
                currentEnemyPictureBox.Update();

                combatText = "";
                combatText += enemy.name + " attacks " + player.name + ".\n";
                combatText += player.name + " (" + player.attack + ", " + player.defence + ", " + player.effectiveness + ", " + player.hitPoints + ").\n";
                combatText += enemy.name + " (" + enemy.attack + ", " + enemy.defence + ", " + enemy.effectiveness + ", " + enemy.hitPoints + ").\n\n";
                combatText += "Attacked|Roll|Cond|Success|HP\n";
                combatText += "--------|----|----|-------|--\n";

                labelCombatLog.Text = combatText; //override previous that battle log

                displayEnemyInfo();

                while (player.hitPoints > 0 && enemy.hitPoints > 0)             //carry out full sequence until someone dies
                    fightSequence();

                if (enemy.hitPoints <= 0)                                       //enemy died - player won
                {
                    nowMovement = false;                                        //to disable player form clicking 1 on their keybaord multiple times
                    fought = true;                                              //and also enable them to choose reward
                    died = false;

                    labelResults.Update();                                      //enable adding stat points later on
                    setCombatText(player.name + " killed the " + enemy.name + " and earned a stat boost.");
                }

                if (player.hitPoints <= 0)                                      //player died
                {
                    fought = true;
                    died = true;
                    nowMovement = false;                                        //same as if player wins, but for acknowledging death

                    setCombatText(enemy.name + " killed the " + player.name + ". " + player.name + "'s max health lowers by 1");
                    player.maxHP--;
                    labelResults.Update();
                    player.deathCounter++;                                      //keep track of how many deaths each player has
                    currentPlayerPictureBox.Parent = panelArray[0];             //move player to the first tile

                    if (player.name == "Wizard")                                //get back all hit points - heal to full
                        player.hitPoints = player.maxHP;
                    else if (player.name == "Warrior")
                        player.hitPoints = player.maxHP;
                    else if (player.name == "Archer")
                        player.hitPoints = player.maxHP;

                    setMovementButtonsVisibility(false);
                    buttonOK.Visible = true;                                //confirm death (give player time to read combat log)
                }

                flowLayoutLongLog.Visible = true;                           //It will contain all previouse combat history

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
                combatText = "";
                diceRoll = random.Next(1, 7);
                if (player.effectiveness + diceRoll > enemy.effectiveness) //player manages to attack the enemy
                {
                    combatText += "enemy   |" + diceRoll + "   |>" + (enemy.effectiveness - player.effectiveness) + "  |true   |";

                    damage = player.attack - enemy.defence;
                    damage = damage >= 1 ? damage : 1;      //it can only go down to 1, no lower (to avoid infinite loops)
                    enemy.hitPoints -= damage;              //enemy loses health

                    animatePlayerAttack();

                    combatText += enemy.hitPoints + "\n";
                    setCombatText(combatText);

                    displayEnemyInfo();                     //show changed stats and refresh the view
                }
                else
                {
                    combatText += "enemy   |" + diceRoll + "   |>" + (enemy.effectiveness - player.effectiveness) + "  |false  |";

                    combatText += enemy.hitPoints + "\n";
                    setCombatText(combatText);
                }
            }

            if (enemy.hitPoints > 0)                        //carry out enemy attack if they are alive
            {
                combatText = "";
                diceRoll = random.Next(1, 7);
                if (enemy.effectiveness - diceRoll >= player.effectiveness) //enemy manages to attack the player
                {
                    combatText += "player  |" + diceRoll + "   |<=" + (enemy.effectiveness - player.effectiveness) + " |true   |";

                    damage = enemy.attack - player.defence;
                    damage = damage >= 1 ? damage : 1;      //it can only go down to 1, no lower (to avoid infinite loops) same as for player
                    player.hitPoints -= damage;

                    animateEnemyAttack();

                    combatText += player.hitPoints + "\n";
                    setCombatText(combatText);

                    displayPlayerInfo();                     //show changed stats and refresh the view
                }
                else
                {
                    combatText += "player  |" + diceRoll + "   |<=" + (enemy.effectiveness - player.effectiveness) + " |false  |";


                    combatText += player.hitPoints + "\n";
                    setCombatText(combatText);
                }
            }
        }

        private void animatePlayerAttack()
        {   
            //animate a kind of attack
            tableLayoutPanelPlayer.BringToFront();
            tableLayoutPanelPlayer.Location = new Point(tableLayoutPanelPlayer.Location.X, tableLayoutPanelPlayer.Location.Y + 100);
            tableLayoutPanelPlayer.Update();
            Thread.Sleep(50);
            tableLayoutPanelPlayer.Location = new Point(tableLayoutPanelPlayer.Location.X, tableLayoutPanelPlayer.Location.Y + 50);
            tableLayoutPanelPlayer.Update();
            Thread.Sleep(100);
            tableLayoutPanelPlayer.Location = new Point(tableLayoutPanelPlayer.Location.X, tableLayoutPanelPlayer.Location.Y - 50);
            tableLayoutPanelPlayer.Update();
            tableLayoutPanelEnemy.Update();
            Thread.Sleep(25);
            tableLayoutPanelPlayer.Location = new Point(tableLayoutPanelPlayer.Location.X, tableLayoutPanelPlayer.Location.Y - 100);
            tableLayoutPanelPlayer.Update();
            tableLayoutPanelEnemy.Update();
        }
        private void animateEnemyAttack()
        {
            //animate a kind of smoothish attack
            tableLayoutPanelEnemy.BringToFront();
            tableLayoutPanelEnemy.Location = new Point(tableLayoutPanelEnemy.Location.X, tableLayoutPanelEnemy.Location.Y - 100);
            tableLayoutPanelEnemy.Update();
            Thread.Sleep(50);
            tableLayoutPanelEnemy.Location = new Point(tableLayoutPanelEnemy.Location.X, tableLayoutPanelEnemy.Location.Y - 50);
            tableLayoutPanelEnemy.Update();
            Thread.Sleep(100);
            tableLayoutPanelEnemy.Location = new Point(tableLayoutPanelEnemy.Location.X, tableLayoutPanelEnemy.Location.Y + 50);
            tableLayoutPanelEnemy.Update();
            tableLayoutPanelPlayer.Update();
            Thread.Sleep(25);
            tableLayoutPanelEnemy.Location = new Point(tableLayoutPanelEnemy.Location.X, tableLayoutPanelEnemy.Location.Y + 100);
            tableLayoutPanelEnemy.Update();
            tableLayoutPanelPlayer.Update();
        }
        private void nextPlayer()//working and complete
        {
            turnCounter++;
            //change player to the next one in a loop
            if (playerNr < 2)
                playerNr++;
            else
                playerNr = 0;

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

            currentPlayerPictureBox.BorderStyle = BorderStyle.None;  //back to normal
            currentPlayerPictureBox.Update();

            currentPlayerPictureBox = pictureBoxArray[playerNr];                                  //this is complicated

            currentPlayerPictureBox.BorderStyle = BorderStyle.Fixed3D;      //make it special
            currentPlayerPictureBox.Update();
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
            labelCombatLog.Text += message;        //this waiting is important for the feel of the fight to be somewhat natural
            Thread.Sleep(iterationMs);             //slider can be setting this value, same as the sleep before / you can check every 1000ms if there was a click or something
        }
        private void getReward()
        {
            setAddButtonsVisibility(true);          //show the buttons related to increasing stats
            buttonAddATT.Focus();                   //mark it as ready to be clicked, create a blue rectangle on the button
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
            buttonAddATT.BringToFront();
            buttonAddDEF.BringToFront();
            buttonAddEFF.BringToFront();
        }
        private void endRewardCollection()
        {
            nowMovement = true;
            setAddButtonsVisibility(false);
            displayPlayerInfo();
            setMovementButtonsVisibility(true);
            Thread.Sleep(iterationMs);

            nextPlayer();
            displayPlayerInfo();    //of the next player
            flowLayoutLongLog.Visible = false;
        }
        private void mainSequence(int distanceChoice)
        {
            movementElement(distanceChoice);            //carry out all actions related to movement

            combatElement(distanceChoice);              //carry out all actions related to combat


            //check for the end of the game, shoud be somewhere else
            if (panelNumberInt == 30)
            {
                gameOver = true;
                labelResults.Text = player.name + " won. Turns: " + turnCounter + ". Deaths: " + player.deathCounter + ".";
                labelResults.Update();
            }

            if (gameOver)
            {
                setAddButtonsVisibility(false);
                buttonOK.Visible = false;                   //confirm death
                setMovementButtonsVisibility(false);
                //create play again button? - not necessary.
            }
            else
            {
                //next player should load only after the due reward has been collected (it's due only after an enemy is defeated)
                if (fought)
                {
                    if (!died)          //fought and won
                    {
                        getReward();        //a chain of events leading to waiting for user's choice of reward
                                            //the reward is not due, no need to wait for it's collection, load next player
                    }
                    else                //fought and lost - died (don't give reward, but don't load next character until player acknowledges death
                    {
                        buttonOK.Focus();   //mark it for quick confiration, but wait for that confirmation - player button click or key down
                    }
                }
                else
                {
                    nextPlayer();
                    displayPlayerInfo();
                }

                fought = false;                       //to reset ability to access reward
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
            else
                player.attack += 2;

            endRewardCollection();
        }
        private void buttonAddDEF_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
                player.defence += 1;
            else
                player.defence += 2;

            endRewardCollection();
        }
        private void buttonAddEFF_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
                player.effectiveness += 1;
            else
                player.effectiveness += 2;

            endRewardCollection();
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            buttonOK.Visible = false;
            setMovementButtonsVisibility(true);
            nextPlayer();
            displayPlayerInfo();    //of the next player
            flowLayoutLongLog.Visible = false;
            nowMovement = true;
        }
        private void MyKeyDown(object sender, KeyEventArgs e)
        {
            //taking care of character movement with keyoard 1-6 buttons instad of clicking buttons on the form1 (for conviniency)
            //might substitute for the button1_Click and so on, but then the player might not be aware of possible movement restrictions

            //check if program is in right sequence element (for example not waiting to choose reward)
            if (nowMovement)
            {
                if (e.KeyValue >= 49 && e.KeyValue <= 54)  // keys between '1' and '6' 
                {
                    mainSequence(e.KeyValue - 48);
                }

                if (e.KeyValue == 83)                       // 'S' key, like Slip
                {
                    SlipBox.Checked = !SlipBox.Checked;
                }
            }
            else //now program awaits reward collection only buttons in form and 3 keys are accepted  
            {
                switch (e.KeyValue)
                {
                    case 65:                                //'A' key, like Attack
                        if (!died)
                        {
                            if (panelNumberInt <= 10)
                                player.attack += 1;
                            else
                                player.attack += 2;
                            endRewardCollection();
                        }
                        break;

                    case 68:                                //'D' key, like Defence
                        if (!died)
                        {
                            if (panelNumberInt <= 10)
                                player.defence += 1;
                            else
                                player.defence += 2;
                            endRewardCollection();
                        }
                        break;

                    case 69:                                //'E' key, like Effectiveness
                        if (!died)
                        {
                            if (panelNumberInt <= 10)
                                player.effectiveness += 1;
                            else
                                player.effectiveness += 2;
                            endRewardCollection();
                        }
                        break;


                    case 79:                                //'O' key, like Ok (confirmation of death)
                        if (died)
                        {
                            buttonOK.Visible = false;
                            setMovementButtonsVisibility(true);
                            nextPlayer();
                            displayPlayerInfo();    //of the next player
                            flowLayoutLongLog.Visible = false;
                            nowMovement = true;
                            died = false;
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
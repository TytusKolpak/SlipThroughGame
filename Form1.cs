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

        bool nowMovement = true; //should keyboard button presses be registered as movement or ignored (when witing for player to collect reward)
        bool died = false;
        bool fought = false;
        bool gameOver = false;
        bool warriorPlays = true;
        bool archerPlays = true;
        bool wizardPlays = true;
        int iterationMs = 200;
        int panelNumberInt = 0;
        int playerNr = 0;
        int turnCounter = 1;
        int numberOfPlayers = 3;
        string panelName = "panel1";
        string panelNumberString = "0";
        string combatText = "";
        Panel[] panelArray = System.Array.Empty<Panel>();
        PictureBox[] pictureBoxPlayerArray = System.Array.Empty<PictureBox>();
        CombatCard[] playerCombatCardArray = System.Array.Empty<CombatCard>();
        PictureBox currentPlayerPictureBox;

        //assign default values: (short names not to repeat the same long names everywhere)
        //last 3 letters (or 2 in HP) correspond to the stat that this value corresponds to
        //first one (or 2 if there would be conflicts W Warrior and W Wizard? W for first starting with W and W+i for next WIzard) are for name

        public int WATT = 2, WDEF = 1, WEFF = 0, WHP = 10;             //Warrior's stats        //minHP = 6 
        public int AATT = 1, ADEF = 1, AEFF = 1, AHP = 10;             //Archer's stats         //minHP = 5 
        public int WiATT = 3, WiDEF = 0, WiEFF = 0, WiHP = 10;         //Wizard's stats     pp    //minHP = 4 
        //public int SATT = 3, SDEF = 0, SEFF = 0, SHP = 10;             //Shaman's stats         //minHP = 5  (4th character 
        public int WoATT = 3, WoDEF = 0, WoEFF = 3, WoHP = 4;          //Wolf's stats
        public int WeATT = 4, WeDEF = 1, WeEFF = 5, WeHP = 7;          //Werewolf's stats
        public int CATT = 5, CDEF = 3, CEFF = 8, CHP = 11;             //Cerberus' stats
        public int GATT = 1, GDEF = 1, GEFF = 1, GHP = 1;              //Ghost's stats (they change later)

        //First create template objects for CombatCard templates
        //templates are used to create card objects used in matches/games,
        //but templates themselves can be customed in a special window, outside of a match/game
        public CombatCardTemplate WarriorCardTemplate;
        public CombatCardTemplate ArcherCardTemplate;
        public CombatCardTemplate WizardCardTemplate;
        public CombatCardTemplate WolfCardTemplate;
        public CombatCardTemplate WerewolfCardTemplate;
        public CombatCardTemplate CerberusCardTemplate;
        public CombatCardTemplate WarriorGhostCardTemplate;
        public CombatCardTemplate ArcherGhostCardTemplate;
        public CombatCardTemplate WizardGhostCardTemplate;

        CombatCard WarriorCard;
        CombatCard ArcherCard;
        CombatCard WizardCard;
        CombatCard WolfCard;
        CombatCard WerewolfCard;
        CombatCard CerberusCard;
        CombatCard WarriorGhostCard;
        CombatCard ArcherGhostCard;
        CombatCard WizardGhostCard;

        CombatCard player;
        CombatCard enemy;
        Random random = new();

        public static Form1 instance;   //for sending data between forms

        public Form1()//working and complete (so far)
        {
            InitializeComponent();
            instance = this;            //for sending data between forms
            createArrays();
            createCardsFromTemplates();
            tableLayoutPanelEnemy.Visible = false;
            flowLayoutLongLog.Visible = false;
            setAddButtonsVisibility(false);
            buttonOK.Visible = false;
            labelResults.Visible = false;
        }
        public void createCardsFromTemplates() //once at the beginning
        {
            WarriorCardTemplate = new("Warrior", WATT, WDEF, WEFF, WHP, Properties.Resources.warrior);
            ArcherCardTemplate = new("Archer", AATT, ADEF, AEFF, AHP, Properties.Resources.archer);
            WizardCardTemplate = new("Wizard", WiATT, WiDEF, WiEFF, WiHP, Properties.Resources.wizard);
            WolfCardTemplate = new("Wolf", WoATT, WoDEF, WoEFF, WoHP, Properties.Resources.wolf);
            WerewolfCardTemplate = new("Werewolf", WeATT, WeDEF, WeEFF, WeHP, Properties.Resources.werewolf);
            CerberusCardTemplate = new("Cerberus", CATT, CDEF, CEFF, CHP, Properties.Resources.cerberus);
            WarriorGhostCardTemplate = new("Warrior's Ghost", GATT, GDEF, GEFF, GHP, Properties.Resources.warrior_negate);
            ArcherGhostCardTemplate = new("Archer's Ghost", GATT, GDEF, GEFF, GHP, Properties.Resources.archer_negate);
            WizardGhostCardTemplate = new("Wizard's Ghost", GATT, GDEF, GEFF, GHP, Properties.Resources.wizard_negate);

            //one object created based on another object, but one is used in a game, and other can be set outside of a game (customed by player)
            WarriorCard = new(WarriorCardTemplate);
            ArcherCard = new(ArcherCardTemplate);
            WizardCard = new(WizardCardTemplate);
            WolfCard = new(WolfCardTemplate);
            WerewolfCard = new(WerewolfCardTemplate);
            CerberusCard = new(CerberusCardTemplate);
            CerberusCard = new(CerberusCardTemplate);
            WarriorGhostCard = new(WarriorGhostCardTemplate);
            ArcherGhostCard = new(ArcherGhostCardTemplate);
            WizardGhostCard = new(WizardGhostCardTemplate);

            playerCombatCardArray = new CombatCard[3]           //it has to ber here for some reason
            {
                WarriorCard,
                ArcherCard,
                WizardCard,
                //ShamanCard,
            };

            player = playerCombatCardArray[0];
            currentPlayerPictureBox = pictureBoxPlayerArray[0];
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

            pictureBoxPlayerArray = new PictureBox[3]
            {
                pictureBoxWarrior,
                pictureBoxArcher,
                pictureBoxWizard,
                //pictureBoxShaman,
            };

            //playerCombatCardArray = new CombatCard[3] // for some reason it doesn't work if it's here
            //{
            //    WarriorCard,
            //    ArcherCard,
            //    WizardCard,
            //};
        }
        private void movementElement(int steps)//working and complete
        {
            fought = false;
            panelName = currentPlayerPictureBox.Parent.Name.ToString();
            panelNumberString = Regex.Match(panelName, @"\d+").Value;   //single numeric value in string form
            panelNumberInt = Int32.Parse(panelNumberString);            //single numeric value in int form

            if (panelNumberInt + steps <= 30)                           //will have a place to end on
            {
                for (int i = 0; i < steps; i++)
                {
                    Thread.Sleep(100);

                    //slipped
                    if (SlipBox.Checked == true && panelNumberInt % 5 == 0 && panelNumberInt >= 10)//enable at 10,15,20,25 rank, at 5 there is no up to go and at 30 you win
                    {
                        panelNumberInt -= 9;                            // that is going up a file

                        //if player slips, then give class specific bonuses  
                        if (player.name == "Wizard")
                        {
                            player.attack++;
                            labelPlayerAttack.Text = player.attack.ToString();
                            player.attack = player.attack >= 12 ? 12 : player.attack;      //it can only go up to 12, no higher (to avoid excessive shenenighans)
                        }
                        else if (player.name == "Warrior")
                        {
                            player.defence++;
                            labelPlayerDefense.Text = player.defence.ToString();
                            player.defence = player.defence >= 12 ? 12 : player.defence;
                        }
                        else if (player.name == "Archer")
                        {
                            player.effectiveness++;
                            labelPlayerEffectiveness.Text = player.effectiveness.ToString();
                            player.effectiveness = player.effectiveness >= 12 ? 12 : player.effectiveness;
                        }

                        tableLayoutPanelPlayer.Update(); //Show the change right after th eplayer slipped
                    }
                    else
                        panelNumberInt++;                               //just mark next panel as destination

                    //place the player on destination tile and update it
                    currentPlayerPictureBox.Parent = panelArray[panelNumberInt - 1]; //-1 is because panel1 has index 0 : x on x-1
                    currentPlayerPictureBox.Update();
                }
            }
        }
        private void combatElement(int steps)
        {
            //chosing the enemy depending on how far the player is 
            if (panelNumberInt <= 10) //1-10
            {
                WolfCard.hitPoints = WolfCardTemplate.maxHP;            //something of revive (reallly its like making a new one but less unnecessary operations since only hp changes)
                enemy = WolfCard;
            }
            else if (panelNumberInt <= 20) //11-20
            {
                WerewolfCard.hitPoints = WerewolfCardTemplate.maxHP;    //something of revive
                enemy = WerewolfCard;
            }
            else if (panelNumberInt <= 29) // 21-29
            {
                CerberusCard.hitPoints = CerberusCardTemplate.maxHP;    //something of revive
                enemy = CerberusCard;
            }
            else //if it's the last panel - you HAVE TO FIGHT and you have to fight a BOSS
            {
                if (player.name == "Warrior")
                    enemy = WarriorGhostCard;
                else if (player.name == "Archer")
                    enemy = ArcherGhostCard;
                else if (player.name == "Wizard")
                    enemy = WizardGhostCard;

                //there are templates if i wanted to set custom stats for each ghost
                //#Boss stats
                enemy.attack = player.attack - 1;
                if (player.defence != 0)
                    enemy.defence = player.defence - 1;
                else
                    enemy.defence = 0;
                enemy.effectiveness = player.effectiveness - 3;
                enemy.hitPoints = 9;
            }


            int diceRoll = random.Next(1, 7);

            //if player rolls more than what they moved then they are not found out (move less -> less chance of a fight)
            //also if they step onto the last tile there is always a fight with a boss
            if (diceRoll < steps || panelNumberInt == 30)// <= enables fighting at moving by 1 inf there is < then moving 1 is 100% safe
            {
                //FIGHT
                flowLayoutLongLog.Visible = false;

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

                    setCombatText(player.name + " killed the " + enemy.name + " and earned a stat boost.");
                }

                if (player.hitPoints <= 0)                                      //player died
                {
                    panelNumberInt = 1;                                         //here for end of panel scope detection purposes
                    fought = true;
                    died = true;
                    nowMovement = false;                                        //same as if player wins, but for acknowledging death

                    setCombatText(enemy.name + " killed the " + player.name + ". " + player.name + "'s max health lowers by 1");
                    player.deathCounter++;                                      //keep track of how many deaths each player has
                    currentPlayerPictureBox.Parent = panelArray[0];             //move player to the first tile

                    //get back all hit points - heal to full
                    if (player.name == "Wizard")
                    {
                        WizardCardTemplate.maxHP--;
                        WizardCard.hitPoints = WizardCardTemplate.maxHP;
                    }
                    else if (player.name == "Warrior")
                    {
                        WarriorCardTemplate.maxHP--;
                        WarriorCard.hitPoints = WarriorCardTemplate.maxHP;
                    }
                    else if (player.name == "Archer")
                    {
                        ArcherCardTemplate.maxHP--;
                        ArcherCard.hitPoints = ArcherCardTemplate.maxHP;
                    }

                    setMovementButtonsVisibility(false);
                    buttonOK.Visible = true;                                //confirm death (give player time to read combat log)
                }

                flowLayoutLongLog.Visible = true;                           //It will contain all previouse combat history

                enemy.hitPoints = enemy.maxHP;                              //Set health of the enemy back to max for next fight
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

                int condition1;

                if (enemy.effectiveness - player.effectiveness < 0)
                    condition1 = 0;
                else
                    condition1 = enemy.effectiveness - player.effectiveness;

                if (player.effectiveness + diceRoll > enemy.effectiveness) //player manages to attack the enemy
                {
                    combatText += "enemy   |" + diceRoll + "   |>" + condition1 + "  |true   |";

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
                    combatText += "enemy   |" + diceRoll + "   |>" + condition1 + "  |false  |";

                    aniematePlayerAttackFail();

                    combatText += enemy.hitPoints + "\n";
                    setCombatText(combatText);
                }
            }

            if (enemy.hitPoints > 0)                        //carry out enemy attack if they are alive
            {
                combatText = "";
                diceRoll = random.Next(1, 7);

                int condition2 = 0;

                if (enemy.effectiveness - player.effectiveness < 0)
                    condition2 = 0;
                else
                    condition2 = enemy.effectiveness - player.effectiveness;

                if (enemy.effectiveness - diceRoll >= player.effectiveness) //enemy manages to attack the player
                {
                    combatText += "player  |" + diceRoll + "   |<=" + condition2 + " |true   |";

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
                    combatText += "player  |" + diceRoll + "   |<=" + condition2 + " |false  |";

                    animateEnemyAttackFail();

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
        private void aniematePlayerAttackFail()
        {
            //animate both of the panels to signify a miss 
            //imidiately move player out of the enemy's reach
            tableLayoutPanelEnemy.Location = new Point(tableLayoutPanelEnemy.Location.X, tableLayoutPanelEnemy.Location.Y + 25);
            tableLayoutPanelEnemy.Update();
            Thread.Sleep(50);

            //move 20px to the top and wait for a short while (wait lenght determines percieved speed of card's movement)
            tableLayoutPanelPlayer.Location = new Point(tableLayoutPanelPlayer.Location.X, tableLayoutPanelPlayer.Location.Y + 20);
            tableLayoutPanelPlayer.Update();
            Thread.Sleep(50);

            //move 5px up still, and wait more to look like the advance is slowing down
            tableLayoutPanelPlayer.Location = new Point(tableLayoutPanelPlayer.Location.X, tableLayoutPanelPlayer.Location.Y + 5);
            tableLayoutPanelPlayer.Update();
            Thread.Sleep(100);

            //return enemy to original position
            tableLayoutPanelPlayer.Location = new Point(tableLayoutPanelPlayer.Location.X, tableLayoutPanelPlayer.Location.Y - 25);
            tableLayoutPanelPlayer.Update();
            Thread.Sleep(25);

            //return player to original position a while after enemy has done so
            tableLayoutPanelEnemy.Location = new Point(tableLayoutPanelEnemy.Location.X, tableLayoutPanelEnemy.Location.Y - 25);
            tableLayoutPanelPlayer.Update();
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
        private void animateEnemyAttackFail()
        {
            //animate both of the panels to signify a miss 
            //imidiately move player out of the enemy's reach
            tableLayoutPanelPlayer.Location = new Point(tableLayoutPanelPlayer.Location.X, tableLayoutPanelPlayer.Location.Y - 25);
            tableLayoutPanelPlayer.Update();
            Thread.Sleep(50);

            //move 20px to the top and wait for a short while (wait lenght determines percieved speed of card's movement)
            tableLayoutPanelEnemy.Location = new Point(tableLayoutPanelEnemy.Location.X, tableLayoutPanelEnemy.Location.Y - 20);
            tableLayoutPanelEnemy.Update();
            Thread.Sleep(50);

            //move 5px up still, and wait more to look like the advance is slowing down
            tableLayoutPanelEnemy.Location = new Point(tableLayoutPanelEnemy.Location.X, tableLayoutPanelEnemy.Location.Y - 5);
            tableLayoutPanelEnemy.Update();
            Thread.Sleep(100);

            //return enemy to original position
            tableLayoutPanelEnemy.Location = new Point(tableLayoutPanelEnemy.Location.X, tableLayoutPanelEnemy.Location.Y + 25);
            tableLayoutPanelEnemy.Update();
            Thread.Sleep(25);

            //return player to original position a while after enemy has done so
            tableLayoutPanelPlayer.Location = new Point(tableLayoutPanelPlayer.Location.X, tableLayoutPanelPlayer.Location.Y + 25);
            menuStrip1.Update();            //otherwise there would be remains of player card left on it
            tableLayoutPanelPlayer.Update();
        }
        private void nextPlayer()//working and complete
        {
            if (playerNr < numberOfPlayers - 1)
                playerNr++;
            else
            {
                playerNr = 0;
                turnCounter++;
            }

            if (playerNr == 0)
                player = playerCombatCardArray[0]; //was WarriorCard
            else if (playerNr == 1)
                player = playerCombatCardArray[1];
            else
                player = playerCombatCardArray[2];
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

            currentPlayerPictureBox.BorderStyle = BorderStyle.None;         //back to normal
            currentPlayerPictureBox.Update();

            currentPlayerPictureBox = pictureBoxPlayerArray[playerNr];      //this is complicated

            currentPlayerPictureBox.BorderStyle = BorderStyle.Fixed3D;      //make it look special
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

        private void customizingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //load for each character their default values on entry too 
            Characters Characters = new Characters();
            Characters.Show();
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
        private void Form1_Resize(object sender, System.EventArgs e)
        {
            //

            int width = pictureBoxWarrior.Parent.Size.Width / 2;
            int height = pictureBoxWarrior.Parent.Size.Height / 2;
            pictureBoxWarrior.Size = new Size(width, height);

            //
        }
        private void mainSequence(int distanceChoice)
        {
            movementElement(distanceChoice);            //carry out all actions related to movement

            combatElement(distanceChoice);              //carry out all actions related to combat


            //check for the end of the game, shoud be somewhere else
            if (panelNumberInt == 30)
            {
                gameOver = true;
                labelResults.Visible = true;
                labelResults.Text = player.name + " won in " + turnCounter + " turns and " + player.deathCounter + " deaths.";
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
            if (panelNumberInt <= 28)
                mainSequence(2);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 27)
                mainSequence(3);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 26)
                mainSequence(4);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 25)
                mainSequence(5);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 24)
                mainSequence(6);
        }
        private void buttonAddATT_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
                player.attack += 1;
            else
                player.attack += 2;

            player.attack = player.attack >= 12 ? 12 : player.attack;      //it can only go up to 12, no higher (to avoid excessive shenenighans)

            endRewardCollection();
        }
        private void buttonAddDEF_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
                player.defence += 1;
            else
                player.defence += 2;

            player.defence = player.defence >= 12 ? 12 : player.defence;

            endRewardCollection();
        }
        private void buttonAddEFF_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
                player.effectiveness += 1;
            else
                player.effectiveness += 2;

            player.effectiveness = player.effectiveness >= 12 ? 12 : player.effectiveness;

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
            died = false;
        }
        private void MyKeyDown(object sender, KeyEventArgs e)
        {
            //taking care of character movement with keyoard 1-6 buttons instad of clicking buttons on the form1 (for conviniency)
            //might substitute for the button1_Click and so on, but then the player might not be aware of possible movement restrictions

            //check if program is in right sequence element (for example not waiting to choose reward)
            if (nowMovement)
            {
                if (e.KeyValue >= 49 && e.KeyValue <= 54)   // keys between '1' and '6' 
                {
                    if (e.KeyValue <= 49 + 30 - panelNumberInt)       // if there is still place to go  (not move by 4 tiles when you are on 29th)
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
        private void gameInstructionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 Instruction = new Form2();
            Instruction.Show();
        }
        private void gameControllsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 Controlls = new Form3();
            Controlls.Show();
        }
        private void playAgainToolStripMenuItem_Click(object sender, EventArgs e) //working
        {//reload everything to start a new game
            DialogResult result = MessageBox.Show("Do you really want to start again?", "Play again", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK) // reset all data to intial values
            {
                labelResults.Visible = false;
                gameOver = false;
                panelNumberInt = 0;
                playerNr = 0;
                turnCounter = 1;
                numberOfPlayers = 0;
                pictureBoxPlayerArray = new PictureBox[0];
                playerCombatCardArray = new CombatCard[0];

                SlipBox.Checked = false;    //uncheck if checked (in default it's unchecked)

                if (Characters.instance != null)
                {
                    warriorPlays = Characters.instance.warriorPlays;
                    archerPlays = Characters.instance.archerPlays;
                    wizardPlays = Characters.instance.wizardPlays;
                }

                //adapting to possible different number og players
                if (warriorPlays)
                {
                    numberOfPlayers++;
                    //rewind picture boxes to 1 tile - where they begin
                    pictureBoxWarrior.Parent = panel1;
                    pictureBoxWarrior.Update();

                    //reset stats to default (reverse stat increases and hp drops) OR SET STATS TO NEWLY SET CUSTOM VALUES IN Characters WINDOW
                    WarriorCard = new(WarriorCardTemplate);

                    //for picture boxes
                    Array.Resize(ref pictureBoxPlayerArray, pictureBoxPlayerArray.Length + 1);
                    pictureBoxPlayerArray[pictureBoxPlayerArray.GetUpperBound(0)] = pictureBoxWarrior;

                    //for cards
                    Array.Resize(ref playerCombatCardArray, playerCombatCardArray.Length + 1);
                    playerCombatCardArray[playerCombatCardArray.GetUpperBound(0)] = WarriorCard;
                    pictureBoxWarrior.Visible = true;
                }
                else
                    pictureBoxWarrior.Visible = false;

                if (archerPlays)
                {
                    numberOfPlayers++;
                    pictureBoxArcher.Parent = panel1;
                    pictureBoxArcher.Update();
                    ArcherCard = new(ArcherCardTemplate);
                    Array.Resize(ref pictureBoxPlayerArray, pictureBoxPlayerArray.Length + 1);
                    pictureBoxPlayerArray[pictureBoxPlayerArray.GetUpperBound(0)] = pictureBoxArcher;
                    Array.Resize(ref playerCombatCardArray, playerCombatCardArray.Length + 1);
                    playerCombatCardArray[playerCombatCardArray.GetUpperBound(0)] = ArcherCard;
                    pictureBoxArcher.Visible = true;
                }
                else
                    pictureBoxArcher.Visible = false;

                if (wizardPlays)
                {
                    numberOfPlayers++;
                    pictureBoxWizard.Parent = panel1;
                    pictureBoxWizard.Update();
                    WizardCard = new(WizardCardTemplate);
                    Array.Resize(ref pictureBoxPlayerArray, pictureBoxPlayerArray.Length + 1);
                    pictureBoxPlayerArray[pictureBoxPlayerArray.GetUpperBound(0)] = pictureBoxWizard;
                    Array.Resize(ref playerCombatCardArray, playerCombatCardArray.Length + 1);
                    playerCombatCardArray[playerCombatCardArray.GetUpperBound(0)] = WizardCard;
                    pictureBoxWizard.Visible = true;
                }
                else
                    pictureBoxWizard.Visible = false;

                WolfCard = new(WolfCardTemplate);
                WerewolfCard = new(WerewolfCardTemplate);
                CerberusCard = new(CerberusCardTemplate);

                //update player card in player's panel, so that it shows reseted stats and not the ones from previous game
                currentPlayerPictureBox.BorderStyle = BorderStyle.None;  //back to normal
                currentPlayerPictureBox.Update();

                if (warriorPlays)
                {
                    player = WarriorCard;
                    currentPlayerPictureBox = pictureBoxWarrior;
                }
                else if (archerPlays)
                {
                    player = ArcherCard;
                    currentPlayerPictureBox = pictureBoxArcher;
                }
                else if (wizardPlays)
                {
                    player = WizardCard;
                    currentPlayerPictureBox = pictureBoxWizard;
                }
                displayPlayerInfo();

                //remove possible remains of previous unended turn 
                flowLayoutLongLog.Visible = false;
                nowMovement = true;
                setAddButtonsVisibility(false);
                setMovementButtonsVisibility(true);
            }
        }
    }
}
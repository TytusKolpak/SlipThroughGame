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
        bool druidPlays = true;
        int iterationMs = 200;
        int panelNumberInt = 0;
        int playerNr = 0;
        int turnCounter = 1;
        int numberOfPlayers;
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


        public int WATT = 2, mWATT = 6, WDEF = 1, mWDEF = 6, WEFF = 0, mWEFF = 6, WHP = 12, minWHP = 7;//Warrior's 
        public int AATT = 1, mAATT = 6, ADEF = 1, mADEF = 3, AEFF = 1, mAEFF = 12, AHP = 9, minAHP = 5;//Archer's 
        public int WiATT = 3, mWiATT = 12, WiDEF = 0, mWiDEF = 3, WiEFF = 0, mWiEFF = 6, WiHP = 8, minWiHP = 4;//Wizard's stats     
        public int DATT = 1, mDATT = 6, DDEF = 2, mDDEF = 3, DEFF = 0, mDEFF = 6, DHP = 9, minDHP = 6;//Druid's 
        public int WoATT = 2, mWoATT = 0, WoDEF = 0, mWoDEF = 0, WoEFF = 3, mWoEFF = 0, WoHP = 5, minWoHP = 0;//Wolf's stats
        public int WeATT = 4, mWeATT = 0, WeDEF = 1, mWeDEF = 0, WeEFF = 5, mWeEFF = 0, WeHP = 7, minWeHP = 0;//Werewolf's stats
        public int CATT = 5, mCATT = 0, CDEF = 3, mCDEF = 0, CEFF = 8, mCEFF = 0, CHP = 11, minCHP = 0;//Cerberus' stats
        public int GATT = 0, mGATT = 0, GDEF = 0, mGDEF = 0, GEFF = 0, mGEFF = 0, GHP = 0, minGHP = 0;//Ghost's stats (they change later)
        public bool[] WarriorwST = new bool[4] { false, false, false, false };
        public bool[] ArcherwST = new bool[4] { false, false, false, false };
        public bool[] WizardwST = new bool[4] { false, false, false, false };
        public bool[] DruidwST = new bool[4] { false, false, false, false };
        public bool[] filler = new bool[4] { false, false, false, false };
        //never used in reality by some player

        //Everybody has the same pattern of walls Slipped Through - player can slpi only if they have not done it yet.
        //In the beginning none is slipped and when a player slips it changes to true

        //First create template objects for CombatCard templates
        //templates are used to create card objects used in matches/games,
        //but templates themselves can be customed in a special window, outside of a match/game
        public CombatCardTemplate WarriorCardTemplate;
        public CombatCardTemplate ArcherCardTemplate;
        public CombatCardTemplate WizardCardTemplate;
        public CombatCardTemplate DruidCardTemplate;
        public CombatCardTemplate WolfCardTemplate;
        public CombatCardTemplate WerewolfCardTemplate;
        public CombatCardTemplate CerberusCardTemplate;
        public CombatCardTemplate WarriorGhostCardTemplate;
        public CombatCardTemplate ArcherGhostCardTemplate;
        public CombatCardTemplate WizardGhostCardTemplate;
        public CombatCardTemplate DruidGhostCardTemplate;

        CombatCard WarriorCard;
        CombatCard ArcherCard;
        CombatCard WizardCard;
        CombatCard DruidCard;
        CombatCard WolfCard;
        CombatCard WerewolfCard;
        CombatCard CerberusCard;
        CombatCard WarriorGhostCard;
        CombatCard ArcherGhostCard;
        CombatCard WizardGhostCard;
        CombatCard DruidGhostCard;

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
        }
        public void createCardsFromTemplates() //once at the beginning
        {
            WarriorCardTemplate = new("Warrior", WATT, mWATT, WDEF, mWDEF, WEFF, mWEFF, WHP, minWHP, WarriorwST, Properties.Resources.warrior);
            ArcherCardTemplate = new("Archer", AATT, mAATT, ADEF, mADEF, AEFF, mAEFF, AHP, minAHP, ArcherwST, Properties.Resources.archer);
            WizardCardTemplate = new("Wizard", WiATT, mWiATT, WiDEF, mWiDEF, WiEFF, mWiEFF, WiHP, minWiHP, WizardwST, Properties.Resources.wizard);
            DruidCardTemplate = new("Druid", DATT, mDATT, DDEF, mDDEF, DEFF, mDEFF, DHP, minDHP, DruidwST, Properties.Resources.druid);
            WolfCardTemplate = new("Wolf", WoATT, mWoATT, WoDEF, mWoDEF, WoEFF, mWoEFF, WoHP, minWoHP, filler, Properties.Resources.wolf);
            WerewolfCardTemplate = new("Werewolf", WeATT, mWeATT, WeDEF, mWeDEF, WeEFF, mWeEFF, WeHP, minWeHP, filler, Properties.Resources.werewolf);
            CerberusCardTemplate = new("Cerberus", CATT, mCATT, CDEF, mCDEF, CEFF, mCEFF, CHP, minCHP, filler, Properties.Resources.cerberus);
            WarriorGhostCardTemplate = new("Warrior's Ghost", GATT, mGATT, GDEF, mGDEF, GEFF, mGEFF, GHP, minGHP, filler, Properties.Resources.warrior_negate);
            ArcherGhostCardTemplate = new("Archer's Ghost", GATT, mGATT, GDEF, mGDEF, GEFF, mGEFF, GHP, minGHP, filler, Properties.Resources.archer_negate);
            WizardGhostCardTemplate = new("Wizard's Ghost", GATT, mGATT, GDEF, mGDEF, GEFF, mGEFF, GHP, minGHP, filler, Properties.Resources.wizard_negate);
            DruidGhostCardTemplate = new("Druid's Ghost", GATT, mGATT, GDEF, mGDEF, GEFF, mGEFF, GHP, minGHP, filler, Properties.Resources.druid_negate);

            //one object created based on another object, but one is used in a game, and other can be set outside of a game (customed by player)
            WarriorCard = new(WarriorCardTemplate);
            ArcherCard = new(ArcherCardTemplate);
            WizardCard = new(WizardCardTemplate);
            DruidCard = new(DruidCardTemplate);
            WolfCard = new(WolfCardTemplate);
            WerewolfCard = new(WerewolfCardTemplate);
            CerberusCard = new(CerberusCardTemplate);
            CerberusCard = new(CerberusCardTemplate);
            WarriorGhostCard = new(WarriorGhostCardTemplate);
            ArcherGhostCard = new(ArcherGhostCardTemplate);
            WizardGhostCard = new(WizardGhostCardTemplate);
            DruidGhostCard = new(DruidGhostCardTemplate);

            playerCombatCardArray = new CombatCard[4]           //it has to ber here for some reason
            {
                WarriorCard,
                ArcherCard,
                WizardCard,
                DruidCard,
            };

            numberOfPlayers = playerCombatCardArray.Length;

            player = playerCombatCardArray[0];
            currentPlayerPictureBox = pictureBoxPlayerArray[0];
            displayPlayerInfo();

            currentPlayerPictureBox.BorderStyle = BorderStyle.Fixed3D;      //make it look special
            currentPlayerPictureBox.Update();
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

            pictureBoxPlayerArray = new PictureBox[4]
            {
                pictureBoxWarrior,
                pictureBoxArcher,
                pictureBoxWizard,
                pictureBoxDruid,
            };
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
                    Thread.Sleep(100); //make movement slower to indicate real displacement of the figurine by hand

                    bool slipThrough = false;

                    if (SlipBox.Checked == true)
                    {
                        if (panelNumberInt == 10 && !player.wallsSlippedThrough[0])// ()
                        {
                            slipThrough = true;
                            player.wallsSlippedThrough[0] = true;
                        }
                        else if (panelNumberInt == 15 && !player.wallsSlippedThrough[1])
                        {
                            slipThrough = true;
                            player.wallsSlippedThrough[1] = true;
                        }
                        else if (panelNumberInt == 20 && !player.wallsSlippedThrough[2])
                        {
                            slipThrough = true;
                            player.wallsSlippedThrough[2] = true;
                        }
                        else if (panelNumberInt == 25 && !player.wallsSlippedThrough[3])
                        {
                            slipThrough = true;
                            player.wallsSlippedThrough[3] = true;
                        }
                    }

                    //managed to slip through
                    //enable at 10,15,20,25 rank, at 5 there is no up to go and at 30 you win
                    if (slipThrough)
                    {
                        panelNumberInt -= 9;                            // that is going back 9 tiles, which is going up 1 file

                        //might change later for player.main stat or something as a pointer to e.g. player.attack and .maxAttack
                        //if player slips, then give class specific bonuses  
                        if (player.name == "Wizard")
                        {
                            player.attack++;
                            //it can only go up to WizardCard.maxAttack (12), no higher (to avoid excessive shenenighans)
                            player.attack = player.attack >= WizardCard.maxAttack ? WizardCard.maxAttack : player.attack;
                            labelPlayerAttack.Text = player.attack.ToString();
                        }
                        else if (player.name == "Warrior")
                        {
                            player.defence++;
                            player.defence = player.defence >= WarriorCard.maxDefence ? WarriorCard.maxDefence : player.defence;
                            labelPlayerDefense.Text = player.defence.ToString();
                        }
                        else if (player.name == "Archer")
                        {
                            player.effectiveness++;
                            player.effectiveness = player.effectiveness >= ArcherCard.maxEffectiveness ? ArcherCard.maxEffectiveness : player.effectiveness;
                            labelPlayerEffectiveness.Text = player.effectiveness.ToString();
                        }
                        else if (player.name == "Druid")
                        {//add something for totems mby for now it's eff
                            player.effectiveness++;
                            player.effectiveness = player.effectiveness >= DruidCard.maxEffectiveness ? DruidCard.maxEffectiveness : player.effectiveness;
                            labelPlayerEffectiveness.Text = player.effectiveness.ToString();
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
                WolfCard.hitPoints = WolfCardTemplate.maxHP;            
                //something of revive (reallly its like making a new one but less unnecessary operations since only hp changes)
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
                else if (player.name == "Druid")
                    enemy = DruidGhostCard;

                //there are templates if i wanted to set custom stats for each ghost
                //#Boss stats (over all - jsut a bit weaker than you, with 50% chance for att and def and high hp)
                enemy.attack = player.attack - 1;
                if (player.defence != 0)
                    enemy.defence = player.defence - 1;
                else
                    enemy.defence = 0;
                enemy.effectiveness = player.effectiveness + 3;
                enemy.hitPoints = 10;
            }


            int diceRoll = random.Next(1, 7);

            //if player rolls more than what they moved then they are not found out (move less -> less chance of a fight)
            //also if they step onto the last tile there is always a fight with a boss
            if (diceRoll < steps || panelNumberInt == 30)// <= moving  by 1 is 100% safe, by 6 is 17% safe, entering last tile is 0% safe no matter the movement length
            {
                //FIGHT
                flowLayoutLongLog.Visible = false;

                combatText = "COMBAT LOG \n";
                combatText += $"{enemy.name} attacks {player.name}\n";
                combatText += $"{player.name} ({player.attack},{player.defence},{player.effectiveness},{player.hitPoints})\n";
                combatText += $"{enemy.name} ({enemy.attack},{enemy.defence},{enemy.effectiveness},{enemy.hitPoints})\n\n";
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

                    setCombatText($"\n{player.name} killed the {enemy.name}!");
                }

                if (player.hitPoints <= 0)                                      //player died
                {
                    for (int i = 0; i < player.wallsSlippedThrough.Length; i++) //reset all the walls to allow this player through
                        player.wallsSlippedThrough[i] = false;

                    panelNumberInt = 1;                                         //here for end of panel scope detection purposes
                    fought = true;
                    died = true;
                    nowMovement = false;                                        //same as if player wins, but for acknowledging death

                    setCombatText($"{enemy.name} killed the {player.name} .{player.name}'s max health lowers by 1");
                    player.deathCounter++;                                      //keep track of how many deaths each player has
                    currentPlayerPictureBox.Parent = panelArray[0];             //move player to the first tile

                    //dont go lower than minHp
                    player.maxHP--;
                    player.hitPoints = player.maxHP < player.minHP ? player.minHP : player.maxHP;

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
                    combatText += $"enemy   |{diceRoll}   |>{condition1}  |true   |";

                    damage = player.attack - enemy.defence;
                    damage = damage >= 1 ? damage : 1;      //it can only go down to 1, no lower (to avoid infinite loops)
                    enemy.hitPoints -= damage;              //enemy loses health

                    animatePlayerAttack();

                    combatText += $"{enemy.hitPoints}\n";
                    setCombatText(combatText);

                    displayEnemyInfo();                     //show changed stats and refresh the view
                }
                else
                {
                    combatText += $"enemy   |{diceRoll}   |>{condition1}  |false  |";

                    aniematePlayerAttackFail();

                    combatText += $"{enemy.hitPoints}\n";
                    setCombatText(combatText);
                }
            }

            if (enemy.hitPoints > 0)                        //carry out enemy attack if they are alive
            {
                combatText = "";
                diceRoll = random.Next(1, 7);

                int condition2;

                if (enemy.effectiveness - player.effectiveness < 0)
                    condition2 = 0;
                else
                    condition2 = enemy.effectiveness - player.effectiveness;

                if (enemy.effectiveness - diceRoll >= player.effectiveness) //enemy manages to attack the player
                {
                    combatText += $"player  |{diceRoll}   |<={condition2} |true   |";

                    damage = enemy.attack - player.defence;
                    damage = damage >= 1 ? damage : 1;      //it can only go down to 1, no lower (to avoid infinite loops) same as for player
                    player.hitPoints -= damage;

                    animateEnemyAttack();

                    combatText += $"{player.hitPoints}\n";
                    setCombatText(combatText);

                    displayPlayerInfo();                     //show changed stats and refresh the view
                }
                else
                {
                    combatText += $"player  |{diceRoll}   |<={condition2} |false  |";

                    animateEnemyAttackFail();

                    combatText += $"{player.hitPoints}\n";
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
        private void nextPlayer()//working and complete, adaptive to any number of players below 3
        {
            currentPlayerPictureBox.BorderStyle = BorderStyle.None;         //back to normal
            currentPlayerPictureBox.Update();

            if (playerNr < numberOfPlayers - 1)
                playerNr++;
            else
            {
                playerNr = 0;
                turnCounter++;
                labelResults.Text = $"Turn: {turnCounter}";
            }

            if (playerNr == 0)
                player = playerCombatCardArray[0]; //was WarriorCard
            else if (playerNr == 1)
                player = playerCombatCardArray[1];
            else if (playerNr == 2)
                player = playerCombatCardArray[2];
            else if (playerNr == 3)
                player = playerCombatCardArray[3];

            currentPlayerPictureBox = pictureBoxPlayerArray[playerNr];      //this is complicated

            currentPlayerPictureBox.BorderStyle = BorderStyle.Fixed3D;      //make it look special
            currentPlayerPictureBox.Update();
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
            button7.Visible = logicValue;
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
            //make them approximately half of the tile - always, and start form the 0,0 or 0,y/2 or x/2,0 point
            //where x,y are the width and height of the tile they are on
            int width = pictureBoxWarrior.Parent.Size.Width / 2;
            int height = pictureBoxWarrior.Parent.Size.Height / 2;
            pictureBoxWarrior.Size = new Size(width, height);
            pictureBoxWarrior.Location = new Point(0, 0);
            pictureBoxArcher.Size = new Size(width, height);
            pictureBoxArcher.Location = new Point(0, height + 2);
            pictureBoxWizard.Size = new Size(width, height);
            pictureBoxWizard.Location = new Point(width + 2, 0);
            pictureBoxDruid.Size = new Size(width, height);
            pictureBoxDruid.Location = new Point(width + 2, height + 2);
        }
        private void mainSequence(int distanceChoice)
        {
            movementElement(distanceChoice);            //carry out all actions related to movement

            combatElement(distanceChoice);              //carry out all actions related to combat


            //check for the end of the game, shoud be somewhere else
            if (panelNumberInt == 30) //#Changing now if(wonTheGame)
            {
                SlipBox.Visible = false;
                labelResults.Visible = true;
                gameOver = true;
                labelResults.Text = $"{player.name} won in {turnCounter} turns and with {player.deathCounter} deaths.";
                labelResults.Update();
            }

            if (gameOver)
            {
                setAddButtonsVisibility(false);
                buttonOK.Visible = false;
                setMovementButtonsVisibility(false);
                //create play again button? - not necessary.
            }
            else
            {
                //next player should load only after the due reward has been collected (it's due only after an enemy is defeated)
                if (fought)
                {
                    if (!died)          //fought and won
                        getReward();        //a chain of events leading to waiting for user's choice of reward
                                            //the reward is not due, no need to wait for it's collection, load next player
                    else                //fought and lost - died (don't give reward, but don't load next character until player acknowledges death
                        buttonOK.Focus();   //mark it for quick confiration, but wait for that confirmation - player button click or key down
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
        }//move by 1
        private void button2_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 28)
                mainSequence(2);
        }//move by 2
        private void button3_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 27)
                mainSequence(3);
        }//move by 3
        private void button4_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 26)
                mainSequence(4);
        }//move by 4
        private void button5_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 25)
                mainSequence(5);
        }//move by 5
        private void button6_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 24)
                mainSequence(6);
        }//move by 6
        private void button7_Click(object sender, EventArgs e)
        {
            player.hitPoints += 4;
            player.hitPoints = player.hitPoints > player.maxHP ? player.maxHP : player.hitPoints;
            //it can only go up to maxHP (no need to show how it changes in the same player turn)

            nextPlayer();
            displayPlayerInfo();
        }//rest
        private void buttonAddATT_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
                player.attack += 1;
            else
                player.attack += 2;

            //it can only go up to 12, no higher (to avoid excessive shenenighans)
            //check who is it is not neccessary since player.maxAttack is adaptive
            player.attack = player.attack >= player.maxAttack ? player.maxAttack : player.attack;

            endRewardCollection();
        }
        private void buttonAddDEF_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
                player.defence += 1;
            else
                player.defence += 2;

            player.defence = player.defence >= player.maxDefence ? player.maxDefence : player.defence;

            endRewardCollection();
        }
        private void buttonAddEFF_Click(object sender, EventArgs e)
        {
            if (panelNumberInt <= 10)
                player.effectiveness += 1;
            else
                player.effectiveness += 2;

            player.effectiveness = player.effectiveness >= player.maxEffectiveness ? player.maxEffectiveness : player.effectiveness;

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

                            //it can only go up to 12, no higher (to avoid excessive shenenighans)
                            //check who is it is not neccessary since player.maxAttack is adaptive
                            player.attack = player.attack >= player.maxAttack ? player.maxAttack : player.attack;

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

                            player.defence = player.defence >= player.maxDefence ? player.maxDefence : player.defence;

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

                            player.effectiveness = player.effectiveness >= player.maxEffectiveness ? player.maxEffectiveness : player.effectiveness;

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

                    case 82:                                //'R' key, like rest
                        if (died)
                        {
                            player.hitPoints += 4;
                            player.hitPoints = player.hitPoints > player.maxHP ? player.maxHP : player.hitPoints;
                            //it can only go up to maxHP (no need to show how it changes in the same player turn)

                            nextPlayer();
                            displayPlayerInfo();
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
                turnCounter = 1;
                labelResults.Text = $"Turn: {turnCounter}";
                gameOver = false;
                panelNumberInt = 0;
                playerNr = 0;
                numberOfPlayers = 0;
                pictureBoxPlayerArray = new PictureBox[0];
                playerCombatCardArray = new CombatCard[0];

                SlipBox.Checked = false;    //uncheck if checked (in default it's unchecked)

                if (Characters.instance != null)
                {
                    warriorPlays = Characters.instance.warriorPlays;
                    archerPlays = Characters.instance.archerPlays;
                    wizardPlays = Characters.instance.wizardPlays;
                    druidPlays = Characters.instance.druidPlays;
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

                if (druidPlays)
                {
                    numberOfPlayers++;
                    pictureBoxDruid.Parent = panel1;
                    pictureBoxDruid.Update();
                    DruidCard = new(DruidCardTemplate);
                    Array.Resize(ref pictureBoxPlayerArray, pictureBoxPlayerArray.Length + 1);
                    pictureBoxPlayerArray[pictureBoxPlayerArray.GetUpperBound(0)] = pictureBoxDruid;
                    Array.Resize(ref playerCombatCardArray, playerCombatCardArray.Length + 1);
                    playerCombatCardArray[playerCombatCardArray.GetUpperBound(0)] = DruidCard;
                    pictureBoxDruid.Visible = true;
                }
                else
                    pictureBoxDruid.Visible = false;

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
                else if (druidPlays)
                {
                    player = DruidCard;
                    currentPlayerPictureBox = pictureBoxDruid;
                }

                displayPlayerInfo();

                //remove possible remains of previous unended turn 
                flowLayoutLongLog.Visible = false;
                nowMovement = true;
                setAddButtonsVisibility(false);
                setMovementButtonsVisibility(true);

                currentPlayerPictureBox.BorderStyle = BorderStyle.Fixed3D;      //make it look special
                currentPlayerPictureBox.Update();
            }
        }
    }
}
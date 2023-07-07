using System.Text.RegularExpressions;

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
        bool archerSkipsTurn = false;
        bool wizardCreatedImage = false;
        bool druidSummonedBoar = false;
        int iterationMs = 200;
        //int panelNumberInt = 0;
        public int panelNumberInt = 0;
        int playerNr = 0;
        int turnCounter = 1;
        int numberOfPlayers;
        string panelName = "panel1";
        string panelNumberString = "0";
        string combatText = "";
        Panel[] panelArray = Array.Empty<Panel>();
        PictureBox[] pictureBoxPlayerArray = Array.Empty<PictureBox>();
        CombatCard[] playerCombatCardArray = Array.Empty<CombatCard>();
        PictureBox currentPlayerPictureBox;

        //assign default values:
        public int[,] S = new int[,] //2D ARRAY for Stats (character stats)
        //pattern: stat, max value of that stat (created not to grind for eternity to ensure 100% succes
        {  //ATT MAX  DEF MAX EFF MAX HP MinHP
            {2  ,4   ,1  ,6  ,0  ,6  ,12 ,7},    //Warrior's STATS
            {1  ,6   ,1  ,3  ,1  ,12 ,9  ,5},    //Archer's
            {3  ,12  ,0  ,3  ,0  ,6  ,8  ,4},    //Wizard's
            {1  ,6   ,2  ,4  ,0  ,6  ,9  ,6},    //Druid's
        };

        public int[,] E = new int[,] //2D for Enemy stats (they have less)
        {
            {2,0,3,5},      //Wolf's
            {4,1,5,7},      //Werewolf'S
            {5,3,8,11},     //Cerberus'
            {0,0,0,0},      //Ghost's
        };

        public bool[] WarriorwST = new bool[4] { false, false, false, false };
        public bool[] ArcherwST = new bool[4] { false, false, false, false };
        public bool[] WizardwST = new bool[4] { false, false, false, false };
        public bool[] DruidwST = new bool[4] { false, false, false, false };

        //Everybody has the same pattern of walls Slipped Through - player can slip only if they have not done it yet.
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
        public CombatCardTemplate BoarCardTemplate;

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
        CombatCard BoarCard; //druid's fighting companion, summoned for 2 hp


        CombatCard player;
        CombatCard enemy;
        Random random = new();

        public static Form1 instance;   //for sending data between forms

        public Form1()
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
        public void createCardsFromTemplates()
        {                                                     // att     max att  def      max def  eff      max eff  max hp   min hp   slip enabler picture
            WarriorCardTemplate = new("Warrior",                S[0, 0], S[0, 1], S[0, 2], S[0, 3], S[0, 4], S[0, 5], S[0, 6], S[0, 7], WarriorwST,  Properties.Resources.warrior);
            ArcherCardTemplate = new("Archer",                  S[1, 0], S[1, 1], S[1, 2], S[1, 3], S[1, 4], S[1, 5], S[1, 6], S[1, 7], ArcherwST,   Properties.Resources.archer);
            WizardCardTemplate = new("Wizard",                  S[2, 0], S[2, 1], S[2, 2], S[2, 3], S[2, 4], S[2, 5], S[2, 6], S[2, 7], WizardwST,   Properties.Resources.wizard);
            DruidCardTemplate = new("Druid",                    S[3, 0], S[3, 1], S[3, 2], S[3, 3], S[3, 4], S[3, 5], S[3, 6], S[3, 7], DruidwST,    Properties.Resources.druid);
            WolfCardTemplate = new("Wolf",                      E[0, 0],          E[0, 1],          E[0, 2],          E[0, 3],                       Properties.Resources.wolf);
            WerewolfCardTemplate = new("Werewolf",              E[1, 0],          E[1, 1],          E[1, 2],          E[1, 3],                       Properties.Resources.werewolf);
            CerberusCardTemplate = new("Cerberus",              E[2, 0],          E[2, 1],          E[2, 2],          E[2, 3],                       Properties.Resources.cerberus);
            WarriorGhostCardTemplate = new("Warrior's Ghost",   E[3, 0],          E[3, 1],          E[3, 2],          E[3, 3],                       Properties.Resources.warrior_negate);
            ArcherGhostCardTemplate = new("Archer's Ghost",     E[3, 0],          E[3, 1],          E[3, 2],          E[3, 3],                       Properties.Resources.archer_negate);
            WizardGhostCardTemplate = new("Wizard's Ghost",     E[3, 0],          E[3, 1],          E[3, 2],          E[3, 3],                       Properties.Resources.wizard_negate);
            DruidGhostCardTemplate = new("Druid's Ghost",       E[3, 0],          E[3, 1],          E[3, 2],          E[3, 3],                       Properties.Resources.druid_negate);

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
        private void createArrays()
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
        private void movementElement(int steps)
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
                    currentPlayerPictureBox.Parent = panelArray[panelNumberInt - 1]; //-1 is because panel1 has index 0 and panel 'x' hasss index of 'x-1'
                    currentPlayerPictureBox.Update();

                    //if it's the last iteration of movement - if the character is on his last tile
                    if (i==steps-1)
                    {
                        //if there is someone on the last tile of movement and it's not me XD
                        if (pictureBoxWarrior.Parent == panelArray[panelNumberInt - 1] && currentPlayerPictureBox != pictureBoxWarrior)
                        { 
                            //-2 is because panel1 has index 0 and panel 'x' hasss index of 'x-1' and additionaly -1 for displacement to the back
                            pictureBoxWarrior.Parent = panelArray[panelNumberInt - 2];
                            pictureBoxWarrior.Update();
                        }
                        if (pictureBoxArcher.Parent == panelArray[panelNumberInt - 1] && currentPlayerPictureBox != pictureBoxArcher)
                        {
                            pictureBoxArcher.Parent = panelArray[panelNumberInt - 2];
                            pictureBoxArcher.Update();
                        }
                        if (pictureBoxWizard.Parent == panelArray[panelNumberInt - 1] && currentPlayerPictureBox != pictureBoxWizard)
                        {
                            pictureBoxWizard.Parent = panelArray[panelNumberInt - 2];
                            pictureBoxWizard.Update();
                        }
                        if (pictureBoxDruid.Parent == panelArray[panelNumberInt - 1] && currentPlayerPictureBox != pictureBoxDruid)
                        {
                            pictureBoxDruid.Parent = panelArray[panelNumberInt - 2];
                            pictureBoxDruid.Update();
                        }
                    }
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

                //there are templates if i wanted to set custom S for each ghost
                //#Boss S (over all - jsut a bit weaker than you, with 50% chance for att and def and high hp)
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
                displayEnemyInfo();

                //ask a player if he wants to use a card / special ability
                // if we want to give player any time to o it then we can split the main sequence in 2 ?
                string warriorAbility = "Warrior can go into a rage, " +
                    "which would reduce his max health by 1 and rise his max attack by 1.\n";//relocate
                string archerbility = "Archer can apply poison to his arrows," +
                    " which would rise his damage by 1 in the next fight, but make him skip his first attack.\n";//relocate
                string wizardAbility = "Wizard can create his magic image causing the enemy to attack it instad of him," +
                    " which would cause their first attack to deal no damage and him to deal 1 less damage that fight.\n";//relocate
                string druidAbility = "Druid can summon a boar to fight for him, " +
                    " which would cost him 2 hp, beast would fight before him and have the stats of ((his att)/0/(his eff)/(his def)).\n";//relocate

                string messageBoxContent = $"Do You want {player.name} to use his special ability?\n";

                if (player.name == "Warrior")
                    messageBoxContent += warriorAbility;
                else if (player.name == "Archer")
                    messageBoxContent += archerbility;
                else if (player.name == "Wizard")
                    messageBoxContent += wizardAbility;
                else if (player.name == "Druid")
                    messageBoxContent += druidAbility;

                DialogResult result = MessageBox.Show(messageBoxContent, "Special ability", MessageBoxButtons.YesNo);
                //program will wait until YES or NO is clicked 

                if (result == DialogResult.Yes)
                {
                    if (player.name == "Warrior")
                    {
                        player.maxAttack++;
                        player.maxHP--;
                        player.hitPoints--;
                        displayPlayerInfo();
                    }
                    else if (player.name == "Archer")
                    {
                        player.attack++;
                        archerSkipsTurn = true;
                        displayPlayerInfo();
                    }
                    else if (player.name == "Wizard")
                    {
                        wizardCreatedImage = true;
                        player.attack--;
                        displayPlayerInfo();
                    }
                    else if (player.name == "Druid")
                    {
                        player.hitPoints -= 2;
                        druidSummonedBoar = true; //        att      def         eff               health
                        BoarCardTemplate = new("Boar", player.attack, 1, player.effectiveness, player.defence, Properties.Resources.boar);
                        BoarCard = new (BoarCardTemplate);

                        //that's just the easier way of replacing druid wth boar for the beginning of the fight
                        player = BoarCard;
                        displayPlayerInfo();
                    }
                }
                //else do nothing

                //Fight begins
                combatText = $"{enemy.name} attacks {player.name}\n";//override previous that battle log
                combatText += $"{player.name} ({player.attack},{player.defence},{player.effectiveness},{player.hitPoints})\n";
                combatText += $"{enemy.name} ({enemy.attack},{enemy.defence},{enemy.effectiveness},{enemy.hitPoints})\n\n";
                combatText += "Attacked|Roll|Cond|Success|HP\n";
                combatText += "--------|----|----|-------|--\n";
                labelCombatLog.Text = combatText; 

                while (player.hitPoints > 0 && enemy.hitPoints > 0)             //carry out full sequence until someone dies                    
                    fightSequence();

                if (druidSummonedBoar) //can connect those 2 conditions now
                {
                    if (enemy.hitPoints > 0)   //if enemy killed the boar
                    {
                        combatText = $"{enemy.name} kills {player.name}\n";
                        druidSummonedBoar = false;                  //reset 
                        player = DruidCard;                         //change player form boar card to druid card
                        displayPlayerInfo();

                        //write down that the druid is now fighting
                        combatText += $"{enemy.name} attacks {player.name}\n";
                        combatText += $"{player.name} ({player.attack},{player.defence},{player.effectiveness},{player.hitPoints})\n";
                        labelCombatLog.Text += combatText;

                        while (player.hitPoints > 0 && enemy.hitPoints > 0)     //carry out full sequence until druid or the enemy                    
                            fightSequence();
                    }
                }

                if (enemy.hitPoints <= 0)               //enemy died - player won
                {
                    nowMovement = false;                //to disable player form clicking 1 on their keybaord multiple times
                    fought = true;                      //and also enable them to choose reward
                    died = false;

                    setCombatText($"\n{player.name} killed the {enemy.name}!");

                    if (druidSummonedBoar)
                    {
                        druidSummonedBoar = false;      //reset 
                        player = DruidCard;             //change player form boar card to druid card
                        displayPlayerInfo();
                    }
                }

                if (player.hitPoints <= 0)                                      //player died
                {
                    for (int i = 0; i < player.wallsSlippedThrough.Length; i++) //reset all the walls to allow this player through
                        player.wallsSlippedThrough[i] = false;

                    panelNumberInt = 1;                                         //here for end of panel scope detection purposes
                    fought = true;
                    died = true;
                    nowMovement = false;                                        //same as if player wins, but for acknowledging death

                    setCombatText($"{enemy.name} killed the {player.name}. {player.name}'s max health lowers by 1");
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

                if (result == DialogResult.Yes)
                {
                    //fight ended - reverse some temporary changes
                    if (player.name == "Archer")
                    {
                        player.attack--;
                        displayPlayerInfo();
                    }
                    else if (player.name == "Wizard")
                    {
                        player.attack++;
                        displayPlayerInfo();
                    }
                }
            }
        }
        private void fightSequence()
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

                if (!archerSkipsTurn)
                {
                    if (player.effectiveness + diceRoll > enemy.effectiveness) //player manages to attack the enemy
                    {
                        combatText += $"enemy   |{diceRoll}   |>{condition1}  |true   |";

                        damage = player.attack - enemy.defence;
                        damage = damage >= 1 ? damage : 1;      //it can only go down to 1, no lower (to avoid infinite loops)
                        enemy.hitPoints -= damage;              //enemy loses health

                        animatePlayerAttack();

                        combatText += $"{enemy.hitPoints}\n";
                        setCombatText(combatText);

                        displayEnemyInfo();                     //show changed S and refresh the view

                    }
                    else
                    {
                        combatText += $"enemy   |{diceRoll}   |>{condition1}  |false  |";

                        animatePlayerAttackFail();

                        combatText += $"{enemy.hitPoints}\n";
                        setCombatText(combatText);
                    }
                }
                else
                    archerSkipsTurn = false; //skip 1 so check if true and then revert to default false
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

                if (!wizardCreatedImage)
                {
                    if (enemy.effectiveness - diceRoll >= player.effectiveness) //enemy manages to attack the player
                    {
                        combatText += $"player  |{diceRoll}   |<={condition2} |true   |";

                        damage = enemy.attack - player.defence;
                        damage = damage >= 1 ? damage : 1;      //it can only go down to 1, no lower (to avoid infinite loops) same as for player
                        player.hitPoints -= damage;

                        animateEnemyAttack();

                        combatText += $"{player.hitPoints}\n";
                        setCombatText(combatText);

                        displayPlayerInfo();                     //show changed S and refresh the view
                    }
                    else
                    {
                        combatText += $"player  |{diceRoll}   |<={condition2} |false  |";

                        animateEnemyAttackFail();

                        combatText += $"{player.hitPoints}\n";
                        setCombatText(combatText);
                    }
                }
                else
                    wizardCreatedImage = false;
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
        private void animatePlayerAttackFail()
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
            ////animate a kind of smoothish attack
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
            ////animate both of the panels to signify a miss 
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
        private void nextPlayer()
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
        private void displayPlayerInfo()
        {
            //load that player's info to the window elements to be shown
            pictureBoxPlayer.Image = player.bitmapImage;                    //big main picture on the right
            labelPlayerAttack.Text = player.attack.ToString();              //following S
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
        private void setCombatText(string message)
        {
            labelCombatLog.Text += message;        //this waiting is important for the feel of the fight to be somewhat natural
            Thread.Sleep(iterationMs);             //slider can be setting this value, same as the sleep before / you can check every 1000ms if there was a click or something
        }
        private void getReward()
        {
            setAddButtonsVisibility(true);          //show the buttons related to increasing S
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
        private void Form1_Resize(object sender, EventArgs e)
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
        public void mainSequence(int distanceChoice)
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
        private void playAgainToolStripMenuItem_Click(object sender, EventArgs e)
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

                    //reset S to default (reverse stat increases and hp drops) OR SET S TO NEWLY SET CUSTOM VALUES IN Characters WINDOW
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

                //update player card in player's panel, so that it shows reseted S and not the ones from previous game
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
# SlipThroughGame

A simple C# Windows Forms game

## Table of contents

- [Installing the game on your PC](#installing-the-game-on-your-pc)
- [Game Instruction](#game-instruction)
    - [Game overview](#game-overview)
	- [Movement rules](#movement-rules)
	- [Combat rules](#combat-rules)
	- [Statistics](#statistics)
	- [Board form as depicted by tile numbers](#board-form-as-depicted-by-tile-numbers)

## Installing the game on your PC

It is really easy. These are the steps:

1. Download a .zip file from this site. To do that click the green button `<> Code` then at the bottom of Local tab is a `Download ZIP` section. Click on it to download the file.
2. Unpack the .zip file.
3. Go into the unpacked contents, firstly the `bin` folder and the `publish`, there should be a `setup.exe` file.
4. Launch the `setup.exe` file. It will take you through the process of installation step by step.
	1. If there is no .NET environment on Your device yet, then after your permission it will be installed.
	2. There might be some security alerts, ignore them. Don't worry. This is just a game.
5. After the installation process has ended, You can run the `Slip through.exe` file to play the game.

I additionally encourage you, to create a shortcut of this file and put it on the desktop or other more convenient location.

## Game Instruction

### Game overview

The game is supposed to be a digital version of a simple board game for 1-4 people. It is played on a board with 30 tiles in a grid pattern - with 6 rows and 5 columns. Players move through the board, along single path, that can be changed if the player wants it. The goal of the game is to get to the end, to the 30th tile before other players do so. Along the path there are enemies trying to kill You. It gets increasingly more difficult to defeat enemies. If they manage to kill you, then you are placed on the 1st tile of the board and You begin the whole journey anew.

### Movement rules

- Each player starts from 1st tile and heads down to 30th tile where they fight the boss.
- At the 30th tile is a boss - a ghost, you to win the game you have to defeat it.
- Movement is decided by player, who has a choice to move by 1 - 6 tiles at once.
- At the end of every player's turn it is decided weather or not he is attacked by an enemy.
	- Chance for being attacked is calculated as follows: (number of tiles crossed -1 ) / 6.
- In first two rows player can be attacked only by a wolf, 3rd and 4th a werewolf and for 5th and 6th is a Cerberus.
- At tile 10, 15, 20, 25 the player can choose to go back up (to tile 1,6,11,16) instead of of continuing by normal path (to tile 11,16,21,26). That action is the Slip Through.
	- If the player does so then their character receives a stat boost according to the character which they chose.
	- Such action can be performed only once per wall.
- The order in which players move is set to warrior -> archer -> wizard -> s, after last players turn end a new round begins.
- Instead of movement, the player can choose to rest. If they do, then they stay on the tile they started and heal by 4HP (but limited by their maxHP).
- When a player finishes a move on a field with one or more other players alredy on it, they are moved back by 1 tile.

### Combat rules

- Each player gets their own character with preset statistics to determine how the combat carries out: warrior, wizard, archer, druid.
- If you win combat you get a chance to choose a stat increase, any stat:
	- +1 for wolf,
	- +2 for werewolf and Cerberus.
- For performing a Slip Through player's character gets stat boost:
	- Warrior gets +1 defence,
	- Wizard gets +1 attack,
	- Archer and Druid get +1 effectiveness.
- Additionally each character has their own special ability, which they can use once at the start of each combat:
	- Warrior can go into a rage, which would reduce his max health by 1 and rise his max attack by 1,
	- Archer can apply poison to his arrows, which would rise his damage by 1 in the next fight, but make him skip his first attack.
	- Wizard can create his magic image causing the enemy to attack it instead of him, which would cause enemy to deal no damage in first turn (whether it succeed or not).
	- Druid can summon a boar to fight for him, which would cost him 2 hp, boar would fight before him and have the stats of:
		- Boar's attack = druid's attack,
		- Boar's defence = 1,
		- Boar's effectiveness = druid's effectiveness,
		- Boar's health = druid's defence.
- Combat is carried in turns: first the player attack their enemy, and then the enemy attack the player
	  - Success of an attack is determined like so: if a dice roll is greater than effectiveness of the enemy lowered by effectiveness of the character, then an attack is successful, otherwise it fails and nothing happens.
	  - If an attack is successful then the attacking character deals damage equal to their attack lowered by defence of their enemy.
- If the character dies, then it goes back to 1st tile with their health set to it's previous value lowered by 1 (each death lowers it further).
- If the character kills its enemy then it receives a stat boost and stays on the tile the fight took place.

### Statistics

Each instance begins with the same amount of predefined statistics: attack, defence, effectiveness, health. Each value can go up only to it's max value and down to it's min value.

|Name      |ATT  |maxATT|DEF  |maxDEF|EFF  |maxEFF|HP   |minHp|
|---------:|:---:|:----:|:---:|:----:|:---:|:----:|:---:|:---:|
|Warrior   |2    |4     |1    |6     |0    |6     |12   |7    |
|Archer    |1    |6     |1    |3     |1    |12    |9    |5    |
|Wizard    |3    |12    |0    |3     |0    |6     |8    |4    |
|Druid     |1    |6     |2    |4     |0    |6     |9    |6    |
|Wolf      |3    |-     |0    |-     |3    |-     |5    |-    |
|Werewolf  |4    |-     |1    |-     |5    |-     |7    |-    |
|Cerberus  |5    |-     |3    |-     |8    |-     |10   |-    |
|Ghost     |p-1  |-     |p-1  |-     |p+3  |-     |10   |-    |

Ghost is the game's boss. Ghost's statistics are defined by the character who attacks it. It's attack is always equal to that of the character and decreased by 1. The same applies to defence. Effectiveness is increased by 3.

### Board form as depicted by tile numbers

| 1   | 2   | 3   | 4   | 5   |
|:---:|:---:|:---:|:---:|:---:|
| 10  | 9   | 8   | 7   | 6   |
| 11  | 12  | 13  | 14  | 15  |
| 20  | 19  | 18  | 17  | 16  |
| 21  | 22  | 23  | 24  | 25  |
| 30  | 29  | 28  | 27  | 26  |

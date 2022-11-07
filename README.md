# SlipThroughGame

A simple C# Windows Forms boadlike game (latest exe files are in `bin/Release v2`)

## Game Instruction

### Game overview

The game is supposed to be a digital version of a simple board game for 3 people.
It is played on a board with 30 tiles in a grid pattern - with 6 rows and 5 columns.
Players move through the board, along single path, that can be changed if the player wants it.
The goal of the game is to get to the end, to the 30th tile before other players do so.
Along the path there are increasingly difficult to defeat enemies trying to kill you, in which case you are placed on the 1st tile of the board.

### Movement rules

 - Each player starts from 1st tile and the first player to 30th tile wins.
 - Movement is decided by player, who has a choice to move by 1 - 6 tiles at once.
 - At the end of every player's turn it is decided weather or not he is attacked by an enemy.
    - Chance for being attacked is calculated as follows: (number of tiles crossed -1 ) / 6.
 - In first two rows player can be attacked only by a wolf, 3rd and 4th a werewolf and for 5th and 6th is a cerberus.
 - At tile 10, 15, 20, 25 the player can choose to go back up (to tile 1,6,11,16) instead of of continuing by normal path (to tile 11,16,21,26). That action is the Slip Through.
    - If the player does so then their character recieves a stat boost according to the character which they chose.
 - The order in which players move is set to warrior -> archer -> wizard, after last players turn end a new round begins.
 
### Combat rules
 
 - Each of 3 players gets their own character with preset statistics to determine how the combat carries out: warrior, wizard, archer.
 - If you win combat you get a chance to choose a stat increase, any stat:
    - +1 for wolf,
    - +2 for werewolf and cerberus.
 - For performing a Slip Through player's character gets stat boost:
    - Warrior gets +1 defence, 
    - Wizard gets +1 attack,
    - Archer gets +1 effeciveness.
 - Combat is carried in turns: first the player attack their enemy, and then the enemy attack the player
    - Success of an attack is determined like so: if a dice roll is greater than effectiveness of the enemy lowered by effectiveness of the character, then an attack is succesful, otherwise it fails and nothing happens.
    - If an attack is succesful then the attacking character deals damage equal to their attack lowered by defence ot their enemy.
 - If the character dies, then it goes back to 1st tile with their healt set to it's previous value lowered by 1 (each death lowers it further).
 - If the character kills its enemy then it recieves a stat boost and stays on the tile the fight took place.
 
### Statistics

Each instance begins with the same amount of predefined statistics: attack, defence, effectiveness, health.

 - Warrior: 2, 1, 2, 10, 
 - Archer: 1, 1, 3, 10,
 - Wizard: 3, 0, 2, 10,
 - Wolf: 3, 0, 5, 5,
 - Werewolf: 4, 1, 6, 7,
 - Cerberus: 5, 3, 8, 10
 
 ### Board form as depicted by tile numbers
 
| 1   | 2   | 3   | 4   | 5   | 
|:---:|:---:|:---:|:---:|:---:|
| 10  | 9   | 8   | 7   | 6   |
|  11 | 12  | 13  | 14  | 15  |
|  20 |  19 |  18 | 17  |  16 |
|  21 | 22  |  23 | 24  | 25  |
|  30 | 29  |  28 | 27  | 26  |

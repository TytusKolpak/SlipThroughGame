using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slip_through
{
    public class CombatCard
    {
        public string name;
        public int attack, defence, effectiveness, hitPoints, deathCounter;
        public Bitmap bitmapImage;

        public static CombatCard currentCard = new ("Warrior", 2, 1, 2, 10, Properties.Resources.warrior);

        public CombatCard(String name, int attack, int defence, int effectiveness, int hitPoints, Bitmap bitmapImage)
        {
            this.name = name;
            this.attack = attack;
            this.defence = defence;
            this.effectiveness = effectiveness;
            this.hitPoints = hitPoints;
            this.bitmapImage = bitmapImage;
            this.deathCounter = 0;
        }

        public static void combatRound(CombatCard attacker, CombatCard defender, int diceThrow)
        {
            //Assumed that the player attacks the enemy, who defends angainst them (changable depending on variant)
            if (attacker.hitPoints > 0)
            {
                if (attacker.effectiveness + diceThrow > defender.effectiveness)
                    defender.hitPoints -= attacker.attack - defender.defence;
            }

            if (defender.hitPoints > 0)
            {
                if (defender.effectiveness - diceThrow > attacker.effectiveness)
                    defender.hitPoints -= attacker.attack - defender.defence;
            }
        }

        public static void choseReward()
        {

        }
    }
}

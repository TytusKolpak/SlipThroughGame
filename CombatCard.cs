using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slip_through
{
    public class CombatCard
    {
        public int attack, defence, effectiveness, hitPoints;
        public Bitmap picture;
        public PictureBox pictureBox;


        public static CombatCard currentPlayer;
        //question mark enables field to begin with null
        //static meaning that this variable belongs to the whole class instead of a single object

        public CombatCard(int attack, int defence, int effectiveness, int hitPoints, Bitmap picture, PictureBox pictureBox)
        {
            this.attack = attack;
            this.defence = defence;
            this.effectiveness = effectiveness;
            this.hitPoints = hitPoints;
            this.picture = picture;
            this.pictureBox = pictureBox;
        }

        public static void fight(CombatCard attacker, CombatCard defender, int diceThrow)
        {

            if (attacker.hitPoints > 0)
            {
                if (attacker.effectiveness + diceThrow > defender.effectiveness)
                {
                    defender.hitPoints -= attacker.attack - defender.defence;
                }
            }

            if (defender.hitPoints > 0)
            {
                if (defender.effectiveness - diceThrow > attacker.effectiveness)
                {
                    defender.hitPoints -= attacker.attack - defender.defence;
                }
            }

        }

    }
}

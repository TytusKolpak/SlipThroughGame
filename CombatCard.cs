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
        public int attack, defence, effectiveness, hitPoints, maxHP, deathCounter;
        public Bitmap bitmapImage;
        public int maxAttack, maxDefence, maxEffectiveness, minHP;
        public bool[] wallsSlippedThrough;

        public CombatCard(CombatCardTemplate template)
        {
            // can "this." be removed? is it unnecessary?
            this.name = template.name;
            this.attack = template.attack;
            this.maxAttack = template.maxAttack;
            this.defence = template.defence;
            this.maxDefence = template.maxDefence;
            this.effectiveness = template.effectiveness;
            this.maxEffectiveness = template.maxEffectiveness;
            this.maxHP = template.maxHP;
            this.hitPoints = template.maxHP;
            this.minHP = template.minHP;
            this.wallsSlippedThrough = template.wallsSlippedThrough;
            this.bitmapImage = template.bitmapImage;
        }
    }
}

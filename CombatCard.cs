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

        public CombatCard(CombatCardTemplate template)
        {
            this.name = template.name;
            this.attack = template.attack;
            this.defence = template.defence;
            this.effectiveness = template.effectiveness;
            this.hitPoints = template.maxHP;
            this.bitmapImage = template.bitmapImage;
        }
    }
}

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
    }
}

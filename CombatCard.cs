using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slip_through
{
    public class CombatCard
    {
        public int attack, defence, effectivenes, hitPoints;

        public CombatCard(int attack, int defence, int effectivenes, int hitPoints)
        {
            this.attack = attack;
            this.defence = defence;
            this.effectivenes = effectivenes;
            this.hitPoints = hitPoints;
        }

    }
}

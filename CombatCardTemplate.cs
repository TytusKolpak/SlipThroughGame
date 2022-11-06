using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slip_through
{
    //prototype for CombatCards, a CombatCardTemplate, ready to be changed in the custom character window 
    public class CombatCardTemplate
    {
        public string name;
        public int attack, defence, effectiveness, maxHP;
        public Bitmap bitmapImage;

        //this is a constructor for object that is a template for a different object
        //from CombatCardTemplate the class you make WarriorCardTemplate the object
        //then you can use the WarriorCardTemplate to affect new WarriorCard objects
        //which is usefull if the player wants to customize his warrior and others
        public CombatCardTemplate(String name, int attack, int defence, int effectiveness, int maxHP, Bitmap bitmapImage)
        {
            this.name = name;
            this.attack = attack;
            this.defence = defence;
            this.effectiveness = effectiveness;
            this.maxHP = maxHP;
            this.bitmapImage = bitmapImage;
        }
    }
}

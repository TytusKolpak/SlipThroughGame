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
        {//for some reason there is no need for this.
            name = template.name;
            attack = template.attack;
            maxAttack = template.maxAttack;
            defence = template.defence;
            maxDefence = template.maxDefence;
            effectiveness = template.effectiveness;
            maxEffectiveness = template.maxEffectiveness;
            maxHP = template.maxHP;
            hitPoints = template.maxHP;
            minHP = template.minHP;
            wallsSlippedThrough = template.wallsSlippedThrough;
            bitmapImage = template.bitmapImage;
        }
    }
}

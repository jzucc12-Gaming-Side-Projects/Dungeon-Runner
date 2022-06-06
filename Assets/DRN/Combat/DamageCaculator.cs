using DRN.COMBAT.COMBATANT;
using DRN.STATS;
using UnityEngine;

namespace DRN.COMBAT
{
    public static class DamageCalculator
    {
        public static int RollDamage(WeaponStats attackStat, CombatAttacker attacker, CombatBody target, bool crit, float critBonus = 0)
        {
            var strength = attacker.GetStat(attackStat);
            var dmgRoll = Random.Range(10 + strength + attacker.GetStat(BodyStats.fortune), 100 + strength + attacker.GetStat(BodyStats.fortune));
            BodyStats defenseStat = (attackStat == WeaponStats.techAffinity ? BodyStats.techRes : BodyStats.physicalRes);
            var def = target.GetStat(defenseStat);

            float damage = strength + dmgRoll - def;
            //Take elemental affinity into account
            if(crit) damage *= GetCritMultiplier(attacker, critBonus); 
            if(target.IsBlocking()) damage *= 0.75f;
            return Mathf.RoundToInt(damage);
        }

        public static (bool, bool) RollHit(WeaponStats attackStat, CombatAttacker attacker, CombatBody target, int hitBonus = 0, int critBonus = 0)
        {
            int attackRoll = Random.Range(0,101);

            var crit = attacker.GetStat(WeaponStats.crit) / 20 + attacker.GetStat(BodyStats.fortune) / 50 + hitBonus;
            bool didCrit = attackRoll >= 95 - crit - critBonus;
            if(didCrit) return (true, true);

            int aim = attacker.GetStat(WeaponStats.aim) + attacker.GetStat(attackStat)/4; // + level
            int dodge = target.GetStat(BodyStats.dodge) + (target.IsBlocking() ? 30 : 0) + target.GetStat(BodyStats.fortune) * (4/20);
            
            var hitChance = 80 + aim - dodge;
            var hitDC = 100 - hitChance;
            
            bool didHit = attackRoll >= hitDC;

            return (didHit, false);
        }

        public static float GetCritMultiplier(CombatAttacker attacker, float critBonus)
        {
            return 1.5f + attacker.GetStat(WeaponStats.crit) * 0.05f/10f + critBonus;
        }
    }
}

//TODO
//MAKE HIT AND DAMAGE MORE SOPHISTICATED!!! THIS IS MERELY PLACE HOLDER AND A BASE OF THE PROCESS
//Update damage calculator to include character levels
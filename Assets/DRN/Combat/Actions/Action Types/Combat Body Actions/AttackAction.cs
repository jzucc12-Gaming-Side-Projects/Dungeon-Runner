using System.Collections;
using DRN.COMBAT.COMBATANT;
using DRN.COMBAT.TARGETING;
using DRN.STATS;
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    public abstract class AttackAction : IAction
    {
        protected WeaponStats stat = WeaponStats.strength;
        protected int hitBonus = 0;
        protected int flatDamage = 0;
        protected int critChanceBonus = 0;
        protected float critDamageBonus = 0;
        protected int immuneCode = -2;
        protected int missCode = -1;


        public IEnumerator Perform(TargetData data)
        {
            yield return data.attacker.AttackAnimation(AnimKeys.startAttack);
            foreach(var target in data.GetLivingTargets())
            {
                int damage = Attack(data.attacker, target);
                if(damage == immuneCode) target.SendCombatText("Immune");
                else if(damage == missCode) target.SendCombatText("Miss");
                else target.SendDamageText(damage);
            }
            yield return data.attacker.AttackAnimation(AnimKeys.stopAttack);
        }

        protected abstract int Attack(CombatAttacker attacker, Combatant target);
    }

    public class StatDamageAttack : AttackAction
    {
        public StatDamageAttack(WeaponStats stat)
        {
            this.stat = stat;
        }

        public StatDamageAttack(WeaponStats stat, int hitBonus, int damageBonus, int critChanceBonus, float critDamageBonus)
        {
            this.stat = stat;
            this.hitBonus = hitBonus;
            this.flatDamage = damageBonus;
            this.critChanceBonus = critChanceBonus;
            this.critDamageBonus = critDamageBonus;
        }

        protected override int Attack(CombatAttacker attacker, Combatant target)
        {
            if(target.body == null) return immuneCode;
            (bool hit, bool crit) = DamageCalculator.RollHit(stat, attacker, target.body, hitBonus);
            if(!hit) return missCode;

            int damage = DamageCalculator.RollDamage(stat, attacker, target.body, crit, critDamageBonus) + flatDamage;
            target.body.TakeDamage(damage);
            return damage;
        }
    }

    public class DamageRangeAttack : AttackAction
    {
        private int minDamage = 0;
        private int maxDamage = 0;

        public DamageRangeAttack(WeaponStats stat)
        {
            this.stat = stat;
        }

        public DamageRangeAttack(WeaponStats stat, int hitBonus, int minDamage, int maxDamage, int critChanceBonus, float critDamageBonus)
        {
            this.stat = stat;
            this.hitBonus = hitBonus;
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.critChanceBonus = critChanceBonus;
            this.critDamageBonus = critDamageBonus;
        }

        protected override int Attack(CombatAttacker attacker, Combatant target)
        {

            if(target.body == null) return immuneCode;
            (bool hit, bool crit) = DamageCalculator.RollHit(stat, attacker, target.body, hitBonus);
            if(!hit) return missCode;

            float damage = Random.Range(minDamage, maxDamage);
            if(crit) damage *= DamageCalculator.GetCritMultiplier(attacker, critDamageBonus);
            target.body.TakeDamage(Mathf.RoundToInt(damage));
            return (int)damage;
        }
    }

    public class FixedDamageAttack : AttackAction
    {
        public FixedDamageAttack(WeaponStats stat)
        {
            this.stat = stat;
        }

        public FixedDamageAttack(WeaponStats stat, int hitBonus, int flatDamage, int critChanceBonus, int critDamageBonus)
        {
            this.stat = stat;
            this.hitBonus = hitBonus;
            this.flatDamage = flatDamage;
            this.critChanceBonus = critChanceBonus;
            this.critDamageBonus = critDamageBonus;
        }

        protected override int Attack(CombatAttacker attacker, Combatant target)
        {
            if(target.body == null) return immuneCode;
            (bool hit, bool crit) = DamageCalculator.RollHit(stat, attacker, target.body, hitBonus);
            if(!hit) return missCode;

            float damage = flatDamage;
            if(crit) damage *= DamageCalculator.GetCritMultiplier(attacker, critDamageBonus);
            target.body.TakeDamage(Mathf.RoundToInt(damage));
            return (int)damage;
        }
    }
}
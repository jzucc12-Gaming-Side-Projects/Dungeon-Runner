using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    [CreateAssetMenu(fileName = "Attack Factory", menuName = "DRN/Action Factory/Attack Factory", order = 0)]
    public class AttackFactory : ActionFactory
    {
        private enum AttackDamageType
        {
            StatDamage = 0,
            DamageRange = 1,
            FixedDamage = 2
        }
        [SerializeField] private AttackDamageType type = AttackDamageType.StatDamage;

        public override IAction GetAction()
        {
            switch(type)
            {
                default:
                case AttackDamageType.StatDamage:
                return new StatDamageAttack(STATS.WeaponStats.strength);

                case AttackDamageType.DamageRange:
                return new DamageRangeAttack(STATS.WeaponStats.strength);

                case AttackDamageType.FixedDamage:
                return new FixedDamageAttack(STATS.WeaponStats.strength);
            }
        }
    }
}

//TODO
//Once equipment is added, have the weapon's primary stat be used in the formation instead
using DRN.CHARACTER;
using DRN.STATS;
using UnityEngine;


namespace DRN.COMBAT.COMBATANT
{
    public abstract class CombatComponent : MonoBehaviour, ICharacterDataReceiver
    {
        protected CharacterData characterData = null;

        #region //Monobehaviour
        protected virtual void Awake() { }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void Start() { }
        #endregion

        #region //Getters
        public string GetName() { return characterData.characterName; }
        public bool IsAlive() { return characterData.IsAlive(); }
        public int GetMaxHP() { return characterData.GetTotalStat(BodyStats.maxHP); }
        public int GetHP() { return characterData.currentHP; }
        public float GetHPPercentage() { return (float)GetHP() / (float)GetMaxHP(); }
        public int GetTP() { return characterData.currentTP; }
        public int GetMaxTP() { return characterData.GetTotalStat(BodyStats.maxTP); }
        public float GetTPPercentage() { return (float)GetTP() / (float)GetMaxTP(); }
        public abstract int GetStat(BodyStats stat);
        public abstract int GetStat(WeaponStats stat);
        #endregion

        #region //Initialization
        public virtual void ReceiveData(CharacterData data)
        {
            characterData = data;
        }
        #endregion
    }
}
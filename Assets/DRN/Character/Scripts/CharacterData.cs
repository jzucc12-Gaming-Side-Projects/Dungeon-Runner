using System;
using System.IO;
using DRN.STATS;
using Newtonsoft.Json;
using UnityEngine;

namespace DRN.CHARACTER
{
    /// <summary>
    /// Information relevant to a given player or enemy character
    /// </summary>
    [Serializable]
    public class CharacterData
    {
        #region //Identification info
        public string characterName = "";
        public string nickName = "";
        #endregion
        
        #region //Stats
        public int currentHP = 100;
        public int currentTP = 100;
        public StatList baseStats = new StatList();
        public StatList armorStats = new StatList();
        public CharacterWeapon[] weapons = { new CharacterWeapon() };
        #endregion

        #region //Level
        public int exp = 0;
        public int level = 1;
        #endregion


        #region //Constructors
        public CharacterData() 
        { 
            this.currentHP = GetTotalStat(BodyStats.maxHP);
            this.currentTP = GetTotalStat(BodyStats.maxTP);
        }
        public CharacterData(string characterName) : this()
        { 
            this.characterName = characterName;
        }
        #endregion

        #region //Loading data
        public static CharacterData ObtainCharacterData(string filePath, string characterName)
        {
            if (File.Exists(filePath))
            {
                string data = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<CharacterData>(data);
            }
            else
            {
                return CreateCharacterData(filePath, characterName);
            }
        }

        public static CharacterData CreateCharacterData(string filePath, string characterName)
        {
            var characterData = new CharacterData(characterName);
            CreateCharacterData(filePath, characterData);
            return characterData;
        }

        public static void CreateCharacterData(string filePath, CharacterData characterData)
        {
            string data = JsonConvert.SerializeObject(characterData, Formatting.Indented);
            File.WriteAllText(filePath, data);
        }
        #endregion

        #region //Getters
        public int GetTotalStat(WeaponStats stat)
        {
            int value = baseStats.GetStat(stat);
            value += armorStats.GetStat(stat);
            foreach(var weapon in weapons)
                value += weapon.GetStat(stat);
            
            return value;
        }

        public int GetTotalStat(BodyStats stat)
        {
            int value = baseStats.GetStat(stat);
            value += armorStats.GetStat(stat);
            foreach(var weapon in weapons)
                value += weapon.GetStat(stat);
            
            return value;
        }

        public int GetWeaponStat(WeaponStats stat, int weaponNo)
        {
            if(weaponNo > weapons.Length - 1)
            {
                Debug.LogError("Weapon index request out of range. Returning 0");
                return 0;
            }
            int value = baseStats.GetStat(stat);
            value += armorStats.GetStat(stat);
            value += weapons[weaponNo].GetStat(stat);
            return value;
        }
        #endregion

        #region //Character state
        public bool IsAlive() { return currentHP > 0; }
        #endregion
    }

    [Serializable]
    public class CharacterWeapon
    {
        public string weaponName = "weapon";
        public StatList stats = new StatList();
        public int GetStat(WeaponStats stat)
        {
            return stats.GetStat(stat);
        }

        public int GetStat(BodyStats stat)
        {
            return stats.GetStat(stat);
        }
    }
}
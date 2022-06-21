using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace DRN.STATS
{
    /// <summary>
    /// Enum containing all stats required for weapon attacks
    /// </summary>
    public enum WeaponStats
    {
        strength = 0, //Melee damage
        dexterity = 1, //Ranged damage
        techAffinity = 2, //Tech damage
        speed = 3, //BB speed and escape capability
        aim = 4, //Accuracy
        crit = 5 //Crit rate and damage
    }

    /// <summary>
    /// Enum containing all character body stats
    /// </summary>
    public enum BodyStats
    {
        maxHP = 0, //Max health points
        maxTP = 1, //Max tech points
        physicalRes = 2, //Phyiscal damage resistance
        techRes = 3, //Tech damage resistance
        dodge = 4, //Avoiding attacks
        fortune = 5 //item drop boosts, crit rates, attack missing
    }

    public static class StatDesc
    {
        private static DescItem statNames = new DescItem();
        private static DescItem statDescriptions = new DescItem();
        private static bool setUp = false;


        [UnityEngine.RuntimeInitializeOnLoadMethodAttribute]
        private static void SetUp()
        {
            string nameData = File.ReadAllText($"{Application.dataPath}/DRN/Stats/Stat Names.txt");
            statNames = JsonConvert.DeserializeObject<DescItem>(nameData);

            string descData = File.ReadAllText($"{Application.dataPath}/DRN/Stats/Stat Descriptions.txt");
            statDescriptions = JsonConvert.DeserializeObject<DescItem>(descData);

            setUp = true;
        }

        public static string GetStatName(WeaponStats stat)
        {
            #if UNITY_EDITOR
            if(!setUp) SetUp();
            #endif

            return statNames.weaponStatInfo[stat];
        }

        public static string GetStatName(BodyStats stat)
        {
            #if UNITY_EDITOR
            if(!setUp) SetUp();
            #endif

            return statNames.bodyStatInfo[stat];
        }

        public static string GetStatDesc(WeaponStats stat)
        {
            #if UNITY_EDITOR
            if(!setUp) SetUp();
            #endif

            return statDescriptions.weaponStatInfo[stat];
        }

        public static string GetStatDesc(BodyStats stat)
        {
            #if UNITY_EDITOR
            if(!setUp) SetUp();
            #endif

            return statDescriptions.bodyStatInfo[stat];
        }

        private struct DescItem
        {
            public Dictionary<BodyStats, string> bodyStatInfo;
            public Dictionary<WeaponStats, string> weaponStatInfo;
        }
    }
}
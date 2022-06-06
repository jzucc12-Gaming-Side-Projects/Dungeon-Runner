using System.Collections.Generic;
using UnityEngine;

namespace DRN.CHARACTER
{
    public class PlayerDataContainer : MonoBehaviour
    {
        [SerializeField] private string characterName = "";
        private static Dictionary<string, CharacterData> players = new Dictionary<string, CharacterData>();

        private void Awake()
        {
            string filePath = $"{Application.dataPath}/Resources/{characterName}";
            var data = CharacterData.ObtainCharacterData(filePath, characterName);
            players.Add(characterName, data);
        }

        public static CharacterData GetPlayerData(string characterName)
        {
            if(!players.ContainsKey(characterName))
            {
                Debug.LogError("player not found. Returning null.");
                return null;
            }

            return players[characterName];
        }
    }
}
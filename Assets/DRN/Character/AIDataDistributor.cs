using UnityEngine;

namespace DRN.CHARACTER
{
    public class AIDataDistributor : CharacterDataDistributor
    {
        protected override CharacterData GetData()
        {
            string filePath = $"{Application.dataPath}/Resources/{characterName}";
            return CharacterData.ObtainCharacterData(filePath, characterName);
        }
    }
}
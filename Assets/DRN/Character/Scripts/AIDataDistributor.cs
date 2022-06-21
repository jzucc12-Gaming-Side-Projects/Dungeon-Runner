namespace DRN.CHARACTER
{
    public class AIDataDistributor : CharacterDataDistributor
    {
        protected override CharacterData GetData()
        {
            return CharacterData.ObtainCharacterData(DRNGlobals.GetAIDataPath(characterName, $"{characterName}.txt"), characterName);
        }

        public void SetUp(string newName) => characterName = newName;
    }
}
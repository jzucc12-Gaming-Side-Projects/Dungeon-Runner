namespace DRN.CHARACTER
{
    public class PlayerDataDistributor : CharacterDataDistributor
    {
        protected override CharacterData GetData()
        {
            return PlayerDataContainer.GetPlayerData(characterName);
        }
    }
}
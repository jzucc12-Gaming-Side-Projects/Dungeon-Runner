using UnityEngine;

namespace DRN.CHARACTER
{
    public abstract class CharacterDataDistributor : MonoBehaviour
    {
        [SerializeField] protected string characterName = "";

        private void Start()
        {
            var data = GetData();
            foreach(var receiver in GetComponentsInChildren<ICharacterDataReceiver>())
                receiver.ReceiveData(data);
        }

        protected abstract CharacterData GetData();
    }
}
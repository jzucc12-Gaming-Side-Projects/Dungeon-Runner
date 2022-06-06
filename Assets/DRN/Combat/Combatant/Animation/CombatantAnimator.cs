using System.Collections.Generic;
using UnityEngine;

namespace DRN.COMBAT.COMBATANT
{
    public class CombatantAnimator : MonoBehaviour
    {
        private Animator animator = null;
        private SpriteRenderer[] srs = new SpriteRenderer[0];
        [SerializeField] private CombatMaterials[] materials = new CombatMaterials[0];
        private Dictionary<string, Material> materialDict = new Dictionary<string, Material>();


        private void Awake()
        {
            animator = GetComponent<Animator>();
            srs = GetComponentsInChildren<SpriteRenderer>();
            foreach(var material in materials)
                materialDict.Add(material.materialName, material.material);
        }

        private void Start()
        {
            foreach(var attacker in GetComponentsInChildren<CombatAttacker>())
                attacker.SetAnimator(animator);
        }

        public void SetMaterial(string materialName)
        {
            if(!materialDict.ContainsKey(materialName))
            {
                Debug.LogWarning($"Material key {materialName} does not exist");
                return;
            }
            
            var material = materialDict[materialName];
            foreach(var sr in srs)
                sr.material = material;
        }

        [System.Serializable]
        private class CombatMaterials
        {
            public string materialName;
            public Material material;
        }
    }
}
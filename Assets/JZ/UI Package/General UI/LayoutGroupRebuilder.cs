using UnityEngine;
using UnityEngine.UI;

namespace JZ.UI
{
    /// <summary>
    /// <para>Ensures content size fitters are the proper size</para>
    /// </summary>
    public class LayoutGroupRebuilder : MonoBehaviour
    {
        private void Start()
        {
            RebuildLayoutGroups();   
        }

        public void RebuildLayoutGroups()
        {
            foreach(var layout in GetComponentsInChildren<LayoutGroup>())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());
            }
        }
    }
}
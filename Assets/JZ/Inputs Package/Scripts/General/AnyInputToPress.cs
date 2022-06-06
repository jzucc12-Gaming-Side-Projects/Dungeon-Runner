using UnityEngine;
using UnityEngine.UI;

namespace JZ.INPUT
{
    /// <summary>
    /// <para>Pressing any input presses the attached button</para>
    /// </summary>
    public class AnyInputToPress : MonoBehaviour
    {
        [SerializeField] private Button button = null;
        void Update()
        {
            if(JZ.INPUT.Utils.AnyKeyOrButton())
            {
                button.onClick?.Invoke();
                enabled = false;
            }
        }
    }
}

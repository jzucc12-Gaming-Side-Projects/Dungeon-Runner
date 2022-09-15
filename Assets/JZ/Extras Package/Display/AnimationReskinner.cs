using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace JZ.DISPLAY
{
    /// <summary>
    /// <para>Allows for an existing animation to be used with different sprites</para>
    /// <para>New spritesheet must have the same number of frames as the old one</para>
    /// <para>New spritesheet must follow the naming scheme of NAME_Frame#</para>
    /// </summary>
    public class AnimationReskinner : MonoBehaviour
    {
        #region //Variables
        [SerializeField] private string resourcePath = "";
        [SerializeField] private string fileName = "";
        private Sprite[] subSprites = new Sprite[0];
        private SpriteRenderer sr = null;
        private Image image = null;
        private int places = 0;
        #endregion
        

        #region //Monobehaviour
        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            image = GetComponent<Image>();
            subSprites = Resources.LoadAll<Sprite>(resourcePath + fileName);
            places = subSprites.Length.ToString().Count();
        }

        private void LateUpdate()
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(resourcePath)) return;

            if (image != null)
                Replace(subSprites, image);
            else
                Replace(subSprites, sr);
        }
        #endregion

        #region //Replacement
        private void Replace(Sprite[] subSprites, Image image)
        {
            int id = GetID(image.sprite.name);
            image.sprite = subSprites[id];
        }

        private void Replace(Sprite[] subSprites, SpriteRenderer renderer)
        {
            int id = GetID(renderer.sprite.name);
            renderer.sprite = subSprites[id];
        }

        private int GetID(string spriteName)
        {
            string chars = spriteName.Substring(spriteName.Length - places);
            string[] sections = chars.Split('_');
            return int.Parse(sections.Last());
        }
        #endregion
    }
}
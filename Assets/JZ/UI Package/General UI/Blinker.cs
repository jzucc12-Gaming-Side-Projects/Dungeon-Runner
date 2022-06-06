using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JZ.UI
{
    /// <summary>
    /// <para>Continuously fades a graphic in and out</para>
    /// </summary>
    public class Blinker : MonoBehaviour
    {
        #region //Variables
        [SerializeField] private float blinkDelay = 1f;
        [SerializeField] private float blinkDuration = 0.1f;
        private Graphic target = null;
        private bool isBlinking = false;
        private float delayTimer = 0;
        #endregion

        #region //Monobehaviour
        private void Awake() 
        {
            target = GetComponent<Graphic>();
        }

        private void FixedUpdate()
        {
            if(delayTimer >= blinkDelay)
            {
                delayTimer = 0;
                StartCoroutine(Fade());
            }

            if(isBlinking) return;
            delayTimer += Time.deltaTime;
        }
        #endregion

        #region //Fading
        private IEnumerator Fade()
        {
            //Get starting values
            isBlinking = true;
            Color currColor = target.color;
            float startAlpha = currColor.a;
            float targetAlpha = startAlpha == 0 ? 1 : 0;
            float currAlpha = currColor.a;

            //Blink
            float blinkTimer = 0;
            while(blinkTimer < blinkDuration)
            {
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.Min(blinkTimer/blinkDuration,1));
                currColor.a = newAlpha;
                target.color = currColor;
                blinkTimer += Time.deltaTime;
                yield return null;
            }

            //Set ending values
            currColor.a = targetAlpha;
            target.color = currColor;
            isBlinking = false;
        }
        #endregion
    }
}
using UnityEngine;

namespace DRN.COMBAT.ACTION
{
    public class ActionVFX : MonoBehaviour
    {
        private void Start()
        {
            if(transform.position.x > 0)
            {
                var scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
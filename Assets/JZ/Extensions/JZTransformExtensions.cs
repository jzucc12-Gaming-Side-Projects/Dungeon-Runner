using UnityEngine;

namespace JZExtensions.TRANSFORM
{
    public static class JZTransformExtensions
    {
        public static Transform FindDeepChild(this Transform refTransform, string childName)
        {
            Transform result = refTransform.Find(childName);
            if(result != null) return result;

            foreach(Transform child in refTransform)
            {
                result = child.FindDeepChild(childName);
                if(result != null) return result;
            }

            return null;
        }
    }
}
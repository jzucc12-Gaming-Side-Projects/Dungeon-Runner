using System;
using UnityEngine;

namespace JZ.SCENE
{
    /// <summary>
    /// <para>Tells the SceneTransitioner class how to behave during a transition</para>
    /// </summary>
    [Serializable]
    public struct SceneTransitionData
    {
        [Tooltip("Set to none for a non-Async transition")] public AnimType animationType;
        public bool additiveLoad;
        public bool unloadMyScene;
        [Tooltip("Only relevant if additive loading.")] public string[] scenesToUnload;
        [HideInInspector] public int targetSceneIndex;
        [HideInInspector] public int mySceneIndex;
        [HideInInspector] public string mySceneName;
    }
}
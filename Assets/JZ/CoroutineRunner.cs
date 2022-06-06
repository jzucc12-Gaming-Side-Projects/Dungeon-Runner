using System.Collections;
using UnityEngine;


//GIVEN TO ME BY theChief ON GMTK DISCORD
public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance; 

    /// <summary>
    /// When the game loads, create an instance of this class, and make sure it isn't destroyed between scenes
    /// </summary>
    private static void CreateInstance()
    {
        _instance = new GameObject ("Runner").AddComponent<CoroutineRunner>();
    }

    /// <summary>
    /// Run a coroutine
    /// </summary>
    public static void Run(IEnumerator coroutine)
    {
        if (_instance == null)
            CreateInstance();
        _instance.StartCoroutine(coroutine);
    }
}

public static class EnumeratorExtensions
{
    /// <summary>
    ///     Shorthand to run a coroutine: IEnumeratorFunction().Run(); 
    /// </summary>
    public static void Run(this IEnumerator toRun)
    {
        CoroutineRunner.Run(toRun);
    }
}
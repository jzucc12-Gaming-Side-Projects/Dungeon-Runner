using UnityEngine;

public static class DRNGlobals
{
    public static string aiAssetPath => "DRN/AI Characters";
    public static string aiCharacterPath => $"{aiAssetPath}/Characters";
    public static string GetAIDataPath(string aiName, string fileName) => $"{Application.dataPath}/{aiCharacterPath}/{aiName}/{fileName}";
}

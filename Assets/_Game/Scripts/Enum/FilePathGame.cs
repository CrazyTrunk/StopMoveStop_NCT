using System.IO;
using UnityEngine;

public static class FilePathGame
{
    public static string CHARACTER_PATH => Path.Combine(Application.persistentDataPath, "playerData.txt");
}
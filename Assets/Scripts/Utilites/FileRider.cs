using System.IO;
using UnityEngine;

public static class FileRider
{
    public static string[] ReadFile(string path)
    {
        try
        {
            string[] lines = File.ReadAllLines(path);
            return lines;
        }

        catch (IOException e)
        {
            Debug.LogError("Ошибка чтения файла: " + e.Message);
            return null;
        }
    }
}
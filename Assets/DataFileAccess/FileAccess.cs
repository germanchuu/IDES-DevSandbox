using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class FileAccess
{
   private static string GetPath(string dataFileName)
   {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        return Path.Combine(tempPath, dataFileName + ".txt");
   }

    public static void SaveData<T>(T dataToSave, string dataFileName)
    {
        string tempPath = GetPath(dataFileName);

        string jsonData = JsonConvert.SerializeObject(dataToSave);
        byte[] jsonByte = Encoding.UTF8.GetBytes(jsonData);

        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
            using (File.Create(tempPath)) { }
            Debug.LogAssertion($"Archivo {dataFileName}.txt creado en: {tempPath}");
        }

        try
        {
            File.WriteAllBytes(tempPath, jsonByte);
            Debug.Log($"Datos guardados en: {tempPath.Replace("/", "\\")}");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Error al guardar: {e.Message}");
        }
    }

    public static T GetData<T>(string dataFileName)
    {
        string tempPath = GetPath(dataFileName);

        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
            return default(T);
        if (!File.Exists(tempPath))
            return default(T);

        byte[] jsonByte = null;
        try
        {
            jsonByte = File.ReadAllBytes(tempPath);
            Debug.Log($"Datos cargados de: {tempPath.Replace("/", "\\")}");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Error al cargar: {e.Message}");
        }

        string jsonData = Encoding.UTF8.GetString(jsonByte);   
        object resultValue = JsonConvert.DeserializeObject<T>(jsonData);

        return (T)Convert.ChangeType(resultValue, typeof(T));
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBoxDB.LocalServer;
using Newtonsoft.Json;
using System.IO;
using System;

public class NGramaDB
{
    private DB db;
    private AutoBox auto;

    private readonly string _monoGramasTable = "MonoGramas";
    private readonly string _BiGramasTable = "BiGramas";
    private readonly string _TriGramasTable = "TriGramas";

    public NGramaDB()
    {
        CopyFileToPersistentDataPath("db1.box");
        CopyFileToPersistentDataPath("db1.box.swp");
        
        DB.Root(Path.Combine(Application.persistentDataPath, "ngramaData"));
        db = new();
        db.GetConfig().EnsureTable<NGramaTransitionSerialized>(_monoGramasTable, "Key");
        db.GetConfig().EnsureTable<NGramaTransitionSerialized>(_BiGramasTable, "Key");
        db.GetConfig().EnsureTable<NGramaTransitionSerialized>(_TriGramasTable, "Key");                  
    }

    private void CopyFileToPersistentDataPath(string fileName)
    {                
        string directoryPath = Path.Combine(Application.persistentDataPath, "ngramaData");
        string destinationPath = Path.Combine(directoryPath, fileName);

        try
        {
            Directory.CreateDirectory(directoryPath);

            if (!File.Exists(destinationPath))
            {
                byte[] fileBytes = BetterStreamingAssets.ReadAllBytes($"/{fileName}");
                File.WriteAllBytes(destinationPath, fileBytes);
                Debug.Log("Archivo copiado correctamente.");
            }
            else
            {
                Debug.Log("El archivo ya existe en persistentDataPath.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al copiar el archivo: {ex}");          
        }
    }

    public void SaveNGrama(NGramaTransition nGramas)
    {
        try
        {
            auto = db.Open();
            string transitionsSerialized = JsonConvert.SerializeObject(nGramas.Transitions);
            NGramaTransitionSerialized serialized = new(nGramas.Key, transitionsSerialized);

            auto.Insert(GetNGramasTable(serialized.Key), serialized);            
            Debug.Log($"Se guardó correctamente: {serialized.Key}");
        }
        catch (System.Exception ex)
        {
            Debug.Log("No se guardó");
            Debug.LogWarning(ex);
        }                
        finally
        {            
            db.Dispose();
        }
    }

    public bool ExitsNGrama(NGramaTransition nGrama)
    {
        auto = db.Open();
        var res = auto.Get<NGramaTransitionSerialized>(GetNGramasTable(nGrama.Key), nGrama.Key);
        db.Dispose();
        return true && res != null;
    }

    public bool ExitsNGrama(string key)
    {
        auto = db.Open();
        var res = auto.Get<NGramaTransitionSerialized>(GetNGramasTable(key), key);
        db.Dispose();
        return true && res != null;
    }

    public NGramaTransition GetNGramaByKey(string key)
    {
        try
        {
            auto = db.Open();
            var res = auto.Get<NGramaTransitionSerialized>(GetNGramasTable(key), key);
            List<Transition> transitions = JsonConvert.DeserializeObject<List<Transition>>(res.SerializedTransitions);
            return new(res.Key, transitions);
        }
        catch (System.Exception ex)
        {
            Debug.Log($"No se pudo obtener las transiciones para: {key}");
            Debug.LogWarning(ex);
            return null;
        }
        finally
        {
            db.Dispose();
        }
    }

    public void UpdateNGrama(NGramaTransition nGramas)
    {
        try
        {
            auto = db.Open();
            string transitionsSerialized = JsonConvert.SerializeObject(nGramas.Transitions);
            NGramaTransitionSerialized serialized = new(nGramas.Key, transitionsSerialized);

            auto.Update(GetNGramasTable(serialized.Key), serialized);
            Debug.Log($"Se actualizó correctamente: {serialized.Key}");
        }
        catch (System.Exception ex)
        {
            Debug.Log("No se actualizó");
            Debug.LogWarning(ex);
        }
        finally
        {
            db.Dispose();
        }
    }

    private string GetNGramasTable(string key)
    {
        int nGramasCount = key.Split(' ').Length;        
        return nGramasCount switch
        {
            1 => _monoGramasTable,
            2 => _BiGramasTable,
            3 => _TriGramasTable,
            _ => _monoGramasTable,
        };
    }
}

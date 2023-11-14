using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickAccessManager : IDataHandler<string, string[], object, string[]>
{
    private readonly string dataFileName = "quickAccess";    

    public void DeleteData(object data)
    {
        throw new System.NotImplementedException();
    }

    public string[] GetData()
    {
        return FileAccess.GetData<string[]>(dataFileName);
    }

    public void InsertData(string data)
    {
        string[] sentences = GetData();
        sentences ??= new string[3];

        for (int i = 0; i < sentences.Length; i++)
        {
            if (string.IsNullOrEmpty(sentences[i]))
            {
                sentences[i] = data;
                break;
            }
        }        

        FileAccess.SaveData(sentences, dataFileName);
    }

    public void UpdateData(string[] data)
    {
        FileAccess.SaveData(data, dataFileName);
    }
}

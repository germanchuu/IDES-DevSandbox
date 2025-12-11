using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultSentencesManager : IDataHandler<string, DefaultSentences, int, string>
{
    private string dataFileName = "defaultSentences";
    public DefaultSentences defaultSentences;

    public void InizialiteDefaultSentences()
    {
        defaultSentences = GetData();

        if (defaultSentences == null || defaultSentences.initialSentences.Count == 0 || defaultSentences.sentences.Count == 0)
        {
            defaultSentences = new DefaultSentences();            
            FileAccess.SaveData(defaultSentences, dataFileName);
        }
    }

    public void DeleteData(int indexData)
    {
        defaultSentences = GetData();
        if (defaultSentences.initialSentences.Remove(indexData))
            FileAccess.SaveData(defaultSentences, dataFileName);
        
        if (defaultSentences.sentences.Remove(indexData))
            FileAccess.SaveData(defaultSentences, dataFileName);

        defaultSentences = null;
    }

    public DefaultSentences GetData()
    {
        return FileAccess.GetData<DefaultSentences>(dataFileName);
    }

    public void InsertData(string data)
    {
        defaultSentences = GetData();
        defaultSentences ??= new DefaultSentences();

        int nextIndex = GetLastIndex() + 1;
        defaultSentences.sentences.Add(nextIndex, data);
        FileAccess.SaveData(defaultSentences, dataFileName);

        defaultSentences = null;
    }

    public void UpdateData(string data)
    {
        defaultSentences = GetData();
        defaultSentences ??= new DefaultSentences();

        defaultSentences.GetSentences();
        FileAccess.SaveData(defaultSentences, dataFileName);

        defaultSentences = null;
    }

    private int GetLastIndex()
    {
        if (defaultSentences.sentences.Any())
            return defaultSentences.sentences.Keys.Max();
        else
            return 0;
    }
}

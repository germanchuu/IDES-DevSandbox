using System;
using System.Collections.Generic;
using System.Linq;


public class SavedSentencesManager : IDataHandler<string, Dictionary<int, string>>
{
    private readonly string dataFileName = "savedSentences";
    public Dictionary<int, string> sentences;

    public Dictionary<int, string> GetData()
    {
        return FileAccess.GetData<Dictionary<int, string>>(dataFileName);
    }

    public void InsertData(string data)
    {
        sentences = GetData();
        sentences ??= new Dictionary<int, string>();

        int nextIndex = GetLastIndex() + 1;
        sentences.Add(nextIndex, data);
        FileAccess.SaveData(sentences, dataFileName);
        
        sentences = null;
    }

    private int GetLastIndex()
    {
        if (sentences.Any())
            return sentences.Keys.Max();
        else
            return 0;
    }

}



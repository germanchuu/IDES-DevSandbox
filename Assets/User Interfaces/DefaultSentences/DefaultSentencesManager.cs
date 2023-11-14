using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSentencesManager : IDataHandler<object, List<string>, object, List<string>>
{
    private string dataFileName = "defaultSentences";

    public void DeleteData(object data)
    {
        throw new System.NotImplementedException();
    }

    public List<string> GetData()
    {
        return FileAccess.GetData<List<string>>(dataFileName);
    }

    public void InsertData(object data)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateData(List<string> data)
    {
        FileAccess.SaveData(data, dataFileName);
    }
}

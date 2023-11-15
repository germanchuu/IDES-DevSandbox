using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationManager : IDataHandler<Configuration, Configuration, object, Configuration>
{
    private string dataFileName = "configuration";

    public void DeleteData(object data)
    {
        throw new System.NotImplementedException();
    }

    public Configuration GetData()
    {
        return FileAccess.GetData<Configuration>(dataFileName);   
    }

    public void InsertData(Configuration data)
    {
        FileAccess.SaveData(data, dataFileName);
    }

    public void UpdateData(Configuration data)
    {
        FileAccess.SaveData(data, dataFileName);
    }
}

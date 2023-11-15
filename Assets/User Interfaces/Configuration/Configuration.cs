using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configuration
{
    public string name;
    public string lastName;
    public int age;
    public string phoneNumber;
    public string emergencyContact;
    public int ttsVoice;

    public static Configuration GetGlobalConfiguration()
    {
        ConfigurationManager manager = new();
        return manager.GetData();
    }
}

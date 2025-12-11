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
    public float pictogramSize;

    public static Configuration GetGlobalConfiguration()
    {
        ConfigurationManager manager = new();
        return manager.GetData();
    }

    public static List<string> isDataValid(Configuration config)
    {
        List<string> validations = new();

        if (config.name.Length < 2)
            validations.Add("Nombre no válido.");

        if (config.lastName.Length < 2)
            validations.Add("Apellido no válido.");

        if (config.age < 1)
            validations.Add("Edad no válida.");

        if (config.phoneNumber.Length < 7)
            validations.Add("Número de celular no válido.");

        if (config.emergencyContact.Length < 7)
            validations.Add("Número de emergencia no válido.");

        return validations;
    }
}

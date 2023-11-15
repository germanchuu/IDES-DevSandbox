using LeastSquares.Overtone;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConfigurationUIManager : MonoBehaviour, IObserver
{
    public TTSPlayer tSPlayer;

    ConfigurationManager manager;
    Configuration config;

    UIDocument configuration;
    TextField txtName;
    TextField txtLastName;
    IntegerField txtAge;
    IntegerField txtPhoneNumber;
    IntegerField txtEmergencyContact;
    DropdownField cmbVoice;

    void Start()
    {
        InicializateInputs();
    }        

    private void InicializateInputs()
    {
        manager = new ConfigurationManager();
        configuration = GetComponent<UIDocument>();
        VisualElement root = configuration.rootVisualElement;
        txtName = root.Q<TextField>("txtName");
        txtLastName = root.Q<TextField>("txtLastName");
        txtAge = root.Q<IntegerField>("txtAge");
        txtPhoneNumber = root.Q<IntegerField>("txtPhoneNumber");
        txtEmergencyContact = root.Q<IntegerField>("txtEmergencyContact");
        cmbVoice = root.Q<DropdownField>("cmbVoice");

        cmbVoice.choices.Clear();
        cmbVoice.choices.Add("Varón");
        cmbVoice.choices.Add("Mujer");
        cmbVoice.RegisterValueChangedCallback(ChangeTTSVoice);
    }

    private void LoadInputs()
    {
        config = manager.GetData();

        if (config == null)
        {
            InsertInitialData();
            config = manager.GetData();
        }

        txtName.value = config.name;
        txtLastName.value = config.lastName;
        txtAge.value = config.age;
        txtPhoneNumber.value = int.Parse(config.phoneNumber);
        txtEmergencyContact.value = int.Parse(config.emergencyContact);
        cmbVoice.index = config.ttsVoice;
    }

    private void InsertInitialData()
    {
        config = new Configuration();

        config.name = "Nombre...";
        config.lastName = "Apellidos...";
        config.age = 20;
        config.phoneNumber = "70708080";
        config.emergencyContact = "70709090";
        config.ttsVoice = 0;

        manager.InsertData(config);
    }

    private void ChangeTTSVoice(ChangeEvent<string> evt)
    {
        if (cmbVoice.index == 0)
            tSPlayer.Voice.speakerId = 0;
        else if (cmbVoice.index == 1)
            tSPlayer.Voice.speakerId = 1;
        else
            tSPlayer.Voice.speakerId = 0;

        config.ttsVoice = cmbVoice.index;
        manager.UpdateData(config);
        ButtonManager.TextToSpeech("Esta es mi voz. Buenas tardes.", tSPlayer);
    }

    public void Notify()
    {
        LoadInputs();
    }
}
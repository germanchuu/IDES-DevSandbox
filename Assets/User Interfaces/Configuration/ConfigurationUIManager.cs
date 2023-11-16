using LeastSquares.Overtone;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConfigurationUIManager : MonoBehaviour, IObserver
{
    [SerializeField] TTSPlayer tSPlayer;
    [SerializeField] PopUpHandler popUp;

    ConfigurationManager manager;
    Configuration config;

    UIDocument configuration;
    TextField txtName;
    TextField txtLastName;
    IntegerField txtAge;
    IntegerField txtPhoneNumber;
    IntegerField txtEmergencyContact;
    DropdownField cmbVoice;
    Label lblTittle;
    Button btnPlayVoice;

    void Awake()
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
        lblTittle = root.Q<Label>("lblTittle");
        btnPlayVoice = root.Q<Button>("btnPlayVoice");
        
        cmbVoice.UnregisterValueChangedCallback(ChangeTTSVoice);
        cmbVoice.choices.Clear();
        cmbVoice.choices.Add("Varón");
        cmbVoice.choices.Add("Mujer");
        cmbVoice.RegisterValueChangedCallback(ChangeTTSVoice);

        btnPlayVoice.UnregisterCallback<ClickEvent>(ListenVoice);
        btnPlayVoice.RegisterCallback<ClickEvent>(ListenVoice);
    }

    private void LoadInputs()
    {        
        config = manager.GetData();

        if (config == null)
        {
            lblTittle.text = "REGISTRO INICIAL";
            InsertInitialData();
            config = manager.GetData();
        }
        else
            lblTittle.text = "CONFIGURACION";

        txtName.value = config.name;
        txtLastName.value = config.lastName;
        txtAge.value = config.age;
        txtPhoneNumber.value = int.Parse(config.phoneNumber);
        txtEmergencyContact.value = int.Parse(config.emergencyContact);
        cmbVoice.index = config.ttsVoice;
    }

    private void InsertInitialData()
    {
        config = new Configuration()
        {
            name = "...",
            lastName = "...",
            age = 0,
            phoneNumber = "0000",
            emergencyContact = "0000",
            ttsVoice = 0
        };        

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
    }

    private void ListenVoice(ClickEvent evt)
    {
        ButtonManager.TextToSpeech("Esta es mi voz. Buenas tardes.", tSPlayer);
    }

    public void UpdateConfigData(Action method)
    {
        config = new Configuration()
        {
            name = txtName.value,
            lastName = txtLastName.value,
            age = txtAge.value,
            phoneNumber = txtPhoneNumber.value.ToString(),
            emergencyContact = txtEmergencyContact.value.ToString(),
            ttsVoice = cmbVoice.index
        };

        ButtonManager.UpdateConfiguration(config, method, popUp);
    }

    public void Notify()
    {
        InicializateInputs();
        LoadInputs();
    }
}
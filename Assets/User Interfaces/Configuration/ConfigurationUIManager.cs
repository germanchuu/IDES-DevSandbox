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
    [SerializeField] SwitchScreens switchScreens;         

    ConfigurationManager manager;
    Configuration config;

    float sizeSelected;

    UIDocument configuration;
    VisualElement pictogramContainer;
    TextField txtName;
    TextField txtLastName;
    IntegerField txtAge;
    IntegerField txtPhoneNumber;
    IntegerField txtEmergencyContact;
    DropdownField cmbVoice;
    Label lblTittle;
    Button btnPlayVoice;
    
    Button btnBig;
    Button btnMiddle;
    Button btnSmall;

    public Button btnSaveConfiguration;

    void Awake()
    {
        InicializateInputs();
    }        

    private void InicializateInputs()
    {
        manager = new ConfigurationManager();
        configuration = GetComponent<UIDocument>();
        VisualElement root = configuration.rootVisualElement;
        pictogramContainer = root.Q<VisualElement>("pictogramContainer");
        txtName = root.Q<TextField>("txtName");
        txtLastName = root.Q<TextField>("txtLastName");
        txtAge = root.Q<IntegerField>("txtAge");
        txtPhoneNumber = root.Q<IntegerField>("txtPhoneNumber");
        txtEmergencyContact = root.Q<IntegerField>("txtEmergencyContact");
        cmbVoice = root.Q<DropdownField>("cmbVoice");
        lblTittle = root.Q<Label>("lblTittle");
        btnPlayVoice = root.Q<Button>("btnPlayVoice");
        btnSaveConfiguration = root.Q<Button>("btnConfirm");
        
        btnSaveConfiguration.RegisterCallback<ClickEvent>(ev =>
        {
            UpdateConfigData();
        });

        btnBig = root.Q<Button>("btnBig");
        btnMiddle = root.Q<Button>("btnMiddle");
        btnSmall = root.Q<Button>("btnSmall");

        btnBig.RegisterCallback<ClickEvent>(ev =>
        {
            sizeSelected = 350;
            UpdatePictogramContainer();

            btnBig.AddToClassList("buttonSizeSelected");
            btnMiddle.RemoveFromClassList("buttonSizeSelected");
            btnSmall.RemoveFromClassList("buttonSizeSelected");
        });

        btnMiddle.RegisterCallback<ClickEvent>(ev =>
        {
            sizeSelected = 300;
            UpdatePictogramContainer();

            btnBig.RemoveFromClassList("buttonSizeSelected");
            btnMiddle.AddToClassList("buttonSizeSelected");
            btnSmall.RemoveFromClassList("buttonSizeSelected");
        });

        btnSmall.RegisterCallback<ClickEvent>(ev =>
        {
            sizeSelected = 250;
            UpdatePictogramContainer();

            btnBig.RemoveFromClassList("buttonSizeSelected");
            btnMiddle.RemoveFromClassList("buttonSizeSelected");
            btnSmall.AddToClassList("buttonSizeSelected");
        });



        cmbVoice.UnregisterValueChangedCallback(ChangeTTSVoice);
        cmbVoice.choices.Clear();
        cmbVoice.choices.Add("Varón");
        cmbVoice.choices.Add("Mujer");
        cmbVoice.RegisterValueChangedCallback(ChangeTTSVoice);

        btnPlayVoice.UnregisterCallback<ClickEvent>(ListenVoice);
        btnPlayVoice.RegisterCallback<ClickEvent>(ListenVoice);
    }

    void UpdatePictogramContainer()
    {
        pictogramContainer.style.width = sizeSelected;
        pictogramContainer.style.height = sizeSelected;
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
        pictogramContainer.style.width = config.pictogramSize;
        pictogramContainer.style.height = config.pictogramSize;

        switch (config.pictogramSize)
        {
            case (> 300):
                btnBig.AddToClassList("buttonSizeSelected");
                break;
            case (300):
                btnMiddle.AddToClassList("buttonSizeSelected");
                break;
            case (< 300):
                btnSmall.AddToClassList("buttonSizeSelected");
                break;
            default:
                btnMiddle.AddToClassList("buttonSizeSelected");
                break;
        }
    }

    private void InsertInitialData()
    {
        config = new Configuration()
        {
            name = "",
            lastName = "",
            age = 0,
            phoneNumber = "",
            emergencyContact = "",
            ttsVoice = 0,
            pictogramSize = 300,
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

    public void UpdateConfigData()
    {                
        config = new Configuration()
        {
            name = txtName.value,
            lastName = txtLastName.value,
            age = txtAge.value,
            phoneNumber = txtPhoneNumber.value.ToString(),
            emergencyContact = txtEmergencyContact.value.ToString(),
            ttsVoice = cmbVoice.index,
            pictogramSize = sizeSelected
        };

        List<string> erros = Configuration.isDataValid(config);
        if (erros.Count > 0)
        {
            ButtonManager.ShowErrorsValidations(erros, popUp);
            return;
        }

        ButtonManager.UpdateConfiguration(config, switchScreens.ShowMainMenu, popUp);        
    }

    public void Notify()
    {
        InicializateInputs();
        LoadInputs();
    }
}
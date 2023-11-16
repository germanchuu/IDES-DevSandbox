using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SwitchScreens : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjects;
    [SerializeField] PopUpHandler popUp;
    [SerializeField] PopUpQuickAccessHandler popUpQuickAcces;

    List<UIDocument> documents;    
    UIDocument menu;

    // Botones menú principal
    Button btnMakeSentence;
    Button btnSaveSentence;
    Button btnDefaultSentence;
    Button btnMap;
    Button btnConfig;

    void Awake()
    {
        documents = GetUIDocuments();        

        menu = documents[0];
        VisualElement root = menu.rootVisualElement;

        btnMakeSentence = root.Q<Button>("btnMakeSentence");
        btnSaveSentence = root.Q<Button>("btnSaveSentence");
        btnDefaultSentence = root.Q<Button>("btnDefaultSentence");
        btnMap = root.Q<Button>("btnMap");
        btnConfig = root.Q<Button>("btnConfig");

        btnMakeSentence.RegisterCallback<ClickEvent, UIDocument>(SwitchScreen, documents[1]);
        btnSaveSentence.RegisterCallback<ClickEvent, UIDocument>(SwitchScreen, documents[2]);
        btnDefaultSentence.RegisterCallback<ClickEvent, UIDocument>(SwitchScreen, documents[3]);
        //btnMap.RegisterCallback<ClickEvent, UIDocument>(SwitchScreen, documents[4]);
        btnConfig.RegisterCallback<ClickEvent, UIDocument>(SwitchScreen, documents[5]);
    }

    private List<UIDocument> GetUIDocuments()
    {
        List<UIDocument> documents = new();

        foreach (GameObject obj in gameObjects)
        {
            if (obj.TryGetComponent<UIDocument>(out var document))
                documents.Add(document);
        }

        return documents;
    }

    void SwitchScreen(ClickEvent ev, UIDocument uIDocument)
    {
        DisplayScreens(uIDocument);

        int documentIndex = documents.IndexOf(uIDocument);
        if (documentIndex > 0 && documentIndex < gameObjects.Count)
        {
            GameObject gameObject = gameObjects[documentIndex];
            IObserver observer = gameObject.GetComponent<IObserver>();
            observer?.Notify();
        }
    }

    public void SwitchScreen(UIDocument uIDocument)
    {
        DisplayScreens(uIDocument);

        int documentIndex = documents.IndexOf(uIDocument);
        if (documentIndex > 0 && documentIndex < gameObjects.Count)
        {
            GameObject gameObject = gameObjects[documentIndex];
            IObserver observer = gameObject.GetComponent<IObserver>();
            observer?.Notify();
        }
    }

    void DisplayScreens(UIDocument uIDocument)
    {
        foreach (UIDocument document in documents)
        {
            if (document != uIDocument)
                document.rootVisualElement.style.display = DisplayStyle.None;
            else
                document.rootVisualElement.style.display = DisplayStyle.Flex;                                           
        }        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (documents[5].rootVisualElement.style.display == DisplayStyle.Flex)
            {
                GameObject gameObject = gameObjects[5];
                ConfigurationUIManager configurationUI = gameObject.GetComponent<ConfigurationUIManager>();
                configurationUI?.UpdateConfigData(ShowMainMenu);                
                return;
            }

            ShowMainMenu();
        }
    }

    public void ShowMainMenu()
    {
        DisplayScreens(documents[0]);

        GameObject gameObject = gameObjects[0];
        IObserver observer = gameObject.GetComponent<IObserver>();
        observer?.Notify();
        popUp.HidePopUp();
        popUpQuickAcces.HidePopUp();
    }
}

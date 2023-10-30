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
    
    List<UIDocument> documents;
    Subject subject;
    UIDocument menu;

    // Botones menú principal
    Button btnMakeSentence;
    Button btnSaveSentence;
    Button btnDefaultSentence;
    Button btnMap;

    void OnEnable()
    {
        documents = GetUIDocuments();
        subject = RegisterObservers();

        menu = documents[0];
        VisualElement root = menu.rootVisualElement;

        btnMakeSentence = root.Q<Button>("btnMakeSentence");
        btnSaveSentence = root.Q<Button>("btnSaveSentence");
        btnDefaultSentence = root.Q<Button>("btnDefaultSentence");
        btnMap = root.Q<Button>("btnMap");

        btnMakeSentence.RegisterCallback<ClickEvent, UIDocument>(SwitchScreen, documents[1]);
        btnSaveSentence.RegisterCallback<ClickEvent, UIDocument>(SwitchScreen, documents[2]);
        //btnDefaultSentence.RegisterCallback<ClickEvent, UIDocument>(SwitchScreen, documents[3]);
        //btnMap.RegisterCallback<ClickEvent, UIDocument>(SwitchScreen, documents[4]);
    }

    private Subject RegisterObservers()
    {
        Subject subject = new();

        foreach (GameObject obj in gameObjects)
        {
            IObserver observer = obj.GetComponent<IObserver>();
            if (observer != null)
                subject.RegisterObserver(observer);
        }

        return subject;
    }

    private List<UIDocument> GetUIDocuments()
    {
        List<UIDocument> documents = new List<UIDocument>();

        foreach (GameObject obj in gameObjects)
        {
            UIDocument document = obj.GetComponent<UIDocument>();
            if (document != null)
                documents.Add(document);
        }

        return documents;
    }

    void SwitchScreen(ClickEvent ev, UIDocument uIDocument)
    {
        Thread thread = new(subject.NotifyObserver);
        thread.Start();

        DisplayScreens(uIDocument);            
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
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                DisplayScreens(documents[0]);
            }

        }
    }
}

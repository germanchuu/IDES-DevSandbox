using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SwitchScreens : MonoBehaviour
{
    [SerializeField] List<UIDocument> documents;
    UIDocument menu;

    // Botones menú principal
    Button btnMakeSentence;
    Button btnSaveSentence;
    Button btnDefaultSentence;
    Button btnMap;

    void OnEnable()
    {
        menu = documents[0];
        VisualElement root = menu.rootVisualElement;

        btnMakeSentence = root.Q<Button>("btnMakeSentence");
        btnSaveSentence = root.Q<Button>("btnSaveSentence");
        btnDefaultSentence = root.Q<Button>("btnDefaultSentence");
        btnMap = root.Q<Button>("btnMap");

        btnMakeSentence.RegisterCallback<ClickEvent, UIDocument>(SwitchScreen, documents[1]);
    }

    void SwitchScreen(ClickEvent ev, UIDocument uIDocument)
    {
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
        Debug.Log($"UIDocument habilitado: {uIDocument.name}");
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

using LeastSquares.Overtone;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUIManager : MonoBehaviour, IObserver
{
    public TTSPlayer tSPlayer;

    private UIDocument mainMenuDocument;
    VisualElement root;

    private Button[] quickAccessbuttons = new Button[3];
    void Start()
    {
        mainMenuDocument = GetComponent<UIDocument>();
        root = mainMenuDocument.rootVisualElement;

        InitializateQuickAccess();
    }

    private void InitializateQuickAccess()
    {
        QuickAccessManager quickAccessManager = new();
        string[] quickAccesSentences = quickAccessManager.GetData();
        quickAccessbuttons = new Button[3];

        if (quickAccesSentences == null)
            InitializateQuickAccess();

        for (int i = 0; i < quickAccessbuttons.Length; i++)
        {
            quickAccessbuttons[i] = root.Q<Button>("btnQuickAccess" + (i + 1));            
            int index = i;

            quickAccessbuttons[i].UnregisterCallback<ClickEvent, string>(QuickAccessButtonClicked);
            quickAccessbuttons[i].text = quickAccesSentences[i];
            quickAccessbuttons[i].RegisterCallback<ClickEvent, string>(QuickAccessButtonClicked, quickAccesSentences[index]);
        }
    }

    private void QuickAccessButtonClicked(ClickEvent evt, string sentence)
    {
        ButtonManager.TextToSpeech(sentence, tSPlayer);
    }

    public void Notify()
    {
        InitializateQuickAccess();
    }
}

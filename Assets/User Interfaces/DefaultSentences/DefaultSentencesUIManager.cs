using LeastSquares.Overtone;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DefaultSentencesUIManager : MonoBehaviour, IObserver
{
    public VisualTreeAsset sentenceAsset;    
    public PopUpQuickAccessHandler quickAccessPopUp;
    public TTSPlayer tSPlayer;

    DefaultSentencesManager manager;
    string sentenceSelected;

    UIDocument defaultSentences;
    ScrollView sentencesScroll;
    Label sentenceLabel;
    
    Button btnListen;
    Button btnQuickAccess;

    private void Awake()
    {
        InitializedUIElements();

        manager = new();
        LoadSentences();
    }

    void InitializedUIElements()
    {
        defaultSentences = GetComponent<UIDocument>();
        VisualElement root = defaultSentences.rootVisualElement;
        sentencesScroll = root.Q<ScrollView>("scrollSentences");
        sentenceLabel = root.Q<Label>("lblSentence");        
        btnListen = root.Q<Button>("btnListen");
        btnQuickAccess = root.Q<Button>("btnQuick");

        btnListen.RegisterCallback<ClickEvent>(ev =>
        {
            if (!string.IsNullOrEmpty(sentenceLabel.text))
                ButtonManager.TextToSpeech(sentenceSelected, tSPlayer);
        });

        btnQuickAccess.RegisterCallback<ClickEvent>(ev =>
        {
            if (!string.IsNullOrEmpty(sentenceLabel.text))
                ButtonManager.UpdateQuickAcces(sentenceSelected.Trim(), quickAccessPopUp);
        });
    }

    void LoadSentences()
    {
        List<string> sentences = manager.GetData();
        sentencesScroll.contentContainer.Clear();

        if (sentences == null)
            return;

        foreach (var sentence in sentences)
        {
            TemplateContainer item = SetSentenceTemplate(sentenceAsset.Instantiate(), sentence);
            sentencesScroll.contentContainer.Add(item);

            item.RegisterCallback<ClickEvent>(e =>
            {
                sentenceSelected = sentence;
                sentenceLabel.text = sentenceSelected;
            });
        }
    }

    TemplateContainer SetSentenceTemplate(TemplateContainer item, string sentence)
    {
        item.style.height = 150;
        item.style.marginLeft = 15;
        item.style.marginRight = 15;
        item.Q<Label>("lblSentence").text = sentence;
        item.Q<Label>("lblSentence").style.width = new StyleLength(new Length(100f, LengthUnit.Percent));
        return item;
    }

    public void Notify()
    {
        LoadSentences();
        sentenceLabel.text = "";
    }
}

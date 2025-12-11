using LeastSquares.Overtone;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DefaultSentencesUIManager : MonoBehaviour, IObserver
{
    public VisualTreeAsset sentenceAsset;    
    public PopUpQuickAccessHandler popUpQuickAccess;
    public PopUpHandler popUpHandler;
    public PopUpNewDefaultSentenceHandler popUpNewDefaultSentence;
    public TTSPlayer tSPlayer;

    DefaultSentencesManager manager;
    KeyValuePair<int, string> sentenceSelected;

    UIDocument defaultSentences;
    ScrollView sentencesScroll;
    Label sentenceLabel;
    
    Button btnListen;
    Button btnQuickAccess;
    Button btnNewSentence;
    Button btnDelete;

    private void Awake()
    {
        InitializedUIElements();

        manager = new();
        manager.InizialiteDefaultSentences();
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
        btnNewSentence = root.Q<Button>("btnNewSentence");
        btnDelete = root.Q<Button>("btnDelete");

        btnListen.RegisterCallback<ClickEvent>(ev =>
        {
            if (!string.IsNullOrEmpty(sentenceLabel.text))
                ButtonManager.TextToSpeech(sentenceSelected.Value, tSPlayer);
        });

        btnQuickAccess.RegisterCallback<ClickEvent>(ev =>
        {
            if (!string.IsNullOrEmpty(sentenceLabel.text))
                ButtonManager.UpdateQuickAccess(sentenceSelected.Value.Trim(), popUpQuickAccess);
        });

        btnNewSentence.RegisterCallback<ClickEvent>(ev =>
        {
            sentenceLabel.text = "";
            popUpNewDefaultSentence.ShowPopUp(Notify);
        });

        btnDelete.RegisterCallback<ClickEvent>(ev =>
        {
            if (!string.IsNullOrEmpty(sentenceLabel.text))
                ButtonManager.DeleteDefaultSentence(sentenceSelected, popUpHandler, this);
        });
    }

    void LoadSentences()
    {
        DefaultSentences defaultSentences = manager.GetData();
        sentencesScroll.contentContainer.Clear();

        if (defaultSentences == null)
            return;

        foreach (var sentence in defaultSentences.initialSentences)
        {
            TemplateContainer item = SetSentenceTemplate(sentenceAsset.Instantiate(), sentence.Value);
            sentencesScroll.contentContainer.Add(item);

            item.RegisterCallback<ClickEvent>(e =>
            {
                sentenceSelected = sentence;
                sentenceLabel.text = sentenceSelected.Value;
            });
        }

        foreach (var sentence in defaultSentences.sentences)
        {
            TemplateContainer item = SetSentenceTemplate(sentenceAsset.Instantiate(), sentence.Value);
            sentencesScroll.contentContainer.Add(item);

            item.RegisterCallback<ClickEvent>(e =>
            {
                sentenceSelected = sentence;
                sentenceLabel.text = sentenceSelected.Value;
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

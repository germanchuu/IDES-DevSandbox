using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SavedSentencesUIManager : MonoBehaviour, IObserver
{
    public VisualTreeAsset sentenceAsset;
    
    SavedSentencesManager manager;

    UIDocument savedSentences;
    ScrollView sentencesScroll;
    Label sentenceLabel;    

    private void Awake()
    {
        InitializedUIElements();        

        manager = new();
        LoadSentences();
    }

    void InitializedUIElements()
    {
        savedSentences = GetComponent<UIDocument>();
        VisualElement root = savedSentences.rootVisualElement;
        sentencesScroll = root.Q<ScrollView>("scrollSentences");
        sentenceLabel = root.Q<Label>("lblSentence");
    }

    void LoadSentences()
    {
        Dictionary<int, string> sentences = manager.GetData();
        sentencesScroll.contentContainer.Clear();

        foreach (var sentence in sentences)
        {
            TemplateContainer item = SetSentenceTemplate(sentenceAsset.Instantiate(), sentence.Value);
            sentencesScroll.contentContainer.Add(item);

            item.RegisterCallback<ClickEvent>(e =>
            {
                sentenceLabel.text = sentence.Value;
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

    public void UpdateUI()
    {
        LoadSentences();
    }
}

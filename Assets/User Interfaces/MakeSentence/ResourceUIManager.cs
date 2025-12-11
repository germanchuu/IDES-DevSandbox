using LeastSquares.Overtone;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourceUIManager : MonoBehaviour, IObserver
{
    ResourceLoader loader;

    public VisualTreeAsset themeAsset;
    public VisualTreeAsset pictogramAsset;
    public VisualTreeAsset suggestedWordAsset;
    public PopUpHandler popUpHandler;
    public TTSPlayer tSPlayer;

    private UIDocument makeSentence;
    private ScrollView pictogramsScroll;
    private ScrollView categoriesScroll;
    private VisualElement suggestedWordsContainer;
    private ScrollView sentenceScroll;
    private Label lblSentence;
    private Button btnDelete;
    private Button btnSaveSentence;
    private Button btnListenSentence;

    List<Theme> themeList;

    void Start()
    {
        loader = GetComponent<ResourceLoader>();
        themeList = loader.GetThemes();

        InitializedUIElements();

        LoadThemes(themeList);
        SetSuggestedWords(new List<string>()
        {
            "Hola",
            "Gracias",
            "Quiero",
            "Ayuda",
            "Estoy"
        });
    }

    void InitializedUIElements()
    {
        makeSentence = GetComponent<UIDocument>();
        VisualElement root = makeSentence.rootVisualElement;
        pictogramsScroll = root.Q<ScrollView>("pictosScroll");
        categoriesScroll = root.Q<ScrollView>("categoriesScroll");
        suggestedWordsContainer = root.Q<VisualElement>("suggestedWordsContainer");
        sentenceScroll = root.Q<ScrollView>("sentenceScroll");
        lblSentence = root.Q<Label>("lblSentence");

        btnDelete = root.Q<Button>("btnDelete");
        btnDelete.RegisterCallback<ClickEvent>(e =>
        {            
            lblSentence.text = ButtonManager.DeleteLastWord(lblSentence.text.Trim());
            SetSuggestedWords(NGramaModel.GetNGramaTransitions(lblSentence.text));
        });

        btnSaveSentence = root.Q<Button>("btnSave");
        btnSaveSentence.RegisterCallback<ClickEvent>(e =>
        {            
            if (!string.IsNullOrEmpty(lblSentence.text))
                ButtonManager.SaveSentence(lblSentence.text.Trim(), popUpHandler);
        });

        btnListenSentence = root.Q<Button>("btnListen");
        btnListenSentence.RegisterCallback<ClickEvent>(e =>
        {
            if (!string.IsNullOrEmpty(lblSentence.text))
                ButtonManager.TextToSpeech(lblSentence.text.Trim(), tSPlayer);
        });
    }

    #region ThemesManager
    void LoadThemes(List<Theme> themes)
    {
        categoriesScroll.contentContainer.Clear();

        foreach (var theme in themes)
        {
            TemplateContainer item = SetThemeTemplate(themeAsset.Instantiate(), theme);
            categoriesScroll.contentContainer.Add(item);

            item.RegisterCallback<ClickEvent>(e =>
            {                
                LoadPictograms(theme);
            });                        
        }

        LoadPictograms(themes[0]);
    }

    TemplateContainer SetThemeTemplate(TemplateContainer item, Theme theme)
    {
        Configuration config = Configuration.GetGlobalConfiguration();

        item.style.width = config.pictogramSize;
        item.style.height = config.pictogramSize;
        item.style.marginLeft = 15;
        item.style.marginRight = 15;
        item.Q<Label>("lblTheme").text = theme.name;
        item.Q<VisualElement>("themeContainer").style.backgroundImage = new StyleBackground(theme.image);
        return item;
    }

    #endregion

    #region PictogramsManager
    void LoadPictograms(Theme theme)
    {        
        theme.pictograms ??= loader.GetPictograms(theme);                
        pictogramsScroll.contentContainer.Clear();
        pictogramsScroll.scrollOffset = new Vector2(0, 0);

        foreach (var pictogram in theme.pictograms)
        {
            TemplateContainer item = SetPictogramTemplate(pictogramAsset.Instantiate(), pictogram);
            pictogramsScroll.contentContainer.Add(item);

            item.RegisterCallback<ClickEvent>(e =>
            {
                if (lblSentence.text == "")
                {
                    lblSentence.text += char.ToUpper(pictogram.name[0]) + pictogram.name[1..] + ' ';
                }
                else
                    lblSentence.text += pictogram.name.ToLower() + ' ';
                sentenceScroll.scrollOffset = lblSentence.layout.max - sentenceScroll.contentViewport.layout.size;
                ButtonManager.TextToSpeech(pictogram.name, tSPlayer);                
                SetSuggestedWords(NGramaModel.GetNGramaTransitions(lblSentence.text));
            });
        }        
    }    

    public void SetSuggestedWords(List<string> words)
    {
        suggestedWordsContainer.contentContainer.Clear();
        if (words.Count == 0)        
            return;        

        foreach (string word in words)
        {
            TemplateContainer item = SetSuggestedWordTemplate(suggestedWordAsset.Instantiate(), word);
            suggestedWordsContainer.contentContainer.Add(item);

            item.RegisterCallback<ClickEvent>(e =>
            {
                if (lblSentence.text == "")
                {
                    lblSentence.text += char.ToUpper(word[0]) + word[1..] + ' ';
                } 
                else
                    lblSentence.text += word.ToLower() + ' ';                
                ButtonManager.TextToSpeech(word, tSPlayer);
                SetSuggestedWords(NGramaModel.GetNGramaTransitions(lblSentence.text));
            });
        }
    }

    TemplateContainer SetPictogramTemplate(TemplateContainer item, Pictogram pictogram)
    {
        Configuration config = Configuration.GetGlobalConfiguration();

        item.style.width = config.pictogramSize;
        item.style.height = config.pictogramSize;
        item.style.marginLeft = 15;
        item.style.marginRight = 15;
        item.Q<Label>("lblPictogram").text = pictogram.name;
        item.Q<VisualElement>("pictogramContainer").style.backgroundImage = new StyleBackground(pictogram.image);
        return item;
    }

    TemplateContainer SetSuggestedWordTemplate(TemplateContainer item, string word)
    {
        item.style.width = Length.Auto();
        item.style.height = Length.Percent(20);
        item.style.marginLeft = 10;
        item.style.marginRight = 10;
        item.Q<Label>("lblWord").text = word;
        return item;
    }

    #endregion

    public void Notify()
    {
        lblSentence.text = "";
        LoadThemes(themeList);
        SetSuggestedWords(new List<string>()
        {
            "Hola",
            "Gracias",
            "Quiero",
            "Ayuda",
            "Estoy"
        });
    }

}

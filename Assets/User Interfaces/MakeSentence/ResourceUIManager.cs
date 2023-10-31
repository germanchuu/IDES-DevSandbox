using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourceUIManager : MonoBehaviour, IObserver
{
    ResourceLoader loader;

    public VisualTreeAsset themeAsset;
    public VisualTreeAsset pictogramAsset;
    public PopUpHandler popUpHandler;
    
    private UIDocument makeSentence;
    private ScrollView pictogramsScroll;
    private ScrollView categoriesScroll;
    private ScrollView sentenceScroll;
    private Label lblSentence;
    private Button btnDelete;
    private Button btnSaveSentence;
    private Button btnListenSentence;

    void Start()
    {
        loader = GetComponent<ResourceLoader>();
        List<Theme> themeList = loader.GetThemes();

        InitializedUIElements();

        LoadThemes(themeList);
    }

    void InitializedUIElements()
    {
        makeSentence = GetComponent<UIDocument>();
        VisualElement root = makeSentence.rootVisualElement;
        pictogramsScroll = root.Q<ScrollView>("pictosScroll");
        categoriesScroll = root.Q<ScrollView>("categoriesScroll");
        sentenceScroll = root.Q<ScrollView>("sentenceScroll");
        lblSentence = root.Q<Label>("lblSentence");

        btnDelete = root.Q<Button>("btnDelete");
        btnDelete.RegisterCallback<ClickEvent>(e =>
        {            
            lblSentence.text = SentenceButtonManager.DeleteLastWord(lblSentence.text);
        });

        btnSaveSentence = root.Q<Button>("btnSave");
        btnSaveSentence.RegisterCallback<ClickEvent>(e =>
        {            
            SentenceButtonManager.SaveSentence(lblSentence.text, popUpHandler);            
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
        item.style.width = 300;
        item.style.height = 300;
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
                lblSentence.text += ' ' + pictogram.name;                                          
                sentenceScroll.scrollOffset = lblSentence.layout.max - sentenceScroll.contentViewport.layout.size;                
            });
        }        
    }    

    TemplateContainer SetPictogramTemplate(TemplateContainer item, Pictogram pictogram)
    {
        item.style.width = 300;
        item.style.height = 300;
        item.style.marginLeft = 15;
        item.style.marginRight = 15;
        item.Q<Label>("lblPictogram").text = pictogram.name;
        item.Q<VisualElement>("pictogramContainer").style.backgroundImage = new StyleBackground(pictogram.image);
        return item;
    }

    #endregion

    public void Notify()
    {
        lblSentence.text = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourceUIManager : MonoBehaviour
{
    public VisualTreeAsset themeAsset;
    public VisualTreeAsset pictogramAsset;
    
    private UIDocument makeSentence;
    private ScrollView pictogramsScroll;
    private ScrollView categoriesScroll;
    public Label lblSentence;

    void Start()
    {
        ResourceLoader loader = GetComponent<ResourceLoader>();
        List<Theme> themeList = loader.GetThemes();

        makeSentence = GetComponent<UIDocument>();
        VisualElement root = makeSentence.rootVisualElement;
        pictogramsScroll = root.Q<ScrollView>("pictosScroll");
        categoriesScroll = root.Q<ScrollView>("categoriesScroll");
        lblSentence = root.Q<Label>("lblSentence");
        lblSentence.text = loader.STREAMING_ASSETS_PATH;

        LoadThemes(themeList);
    }

    #region ThemesManager
    void LoadThemes(List<Theme> themes)
    {
        foreach (var theme in themes)
        {
            TemplateContainer item = SetThemeTemplate(themeAsset.Instantiate(), theme);
            categoriesScroll.contentContainer.Add(item);

            item.RegisterCallback<MouseUpEvent>(e =>
            {
                LoadPictograms(theme.name);
            });
        }
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

    void LoadPictograms(string name)
    {
        Debug.Log("CallBack registrado para: " + name);
    }
    #endregion

    #region PictogramsManager

    #endregion
}

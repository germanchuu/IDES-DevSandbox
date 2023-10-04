using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadPictogramsManager : MonoBehaviour
{
    const string RUTA = "Assets/Resources/";
    const string AssetsRoute = Path.Combine("jar:file://" + Application.dataPath + "!assets/", "Resources/");

    private List<Theme> themes = new List<Theme>();

    public VisualTreeAsset themeAsset;
    public VisualTreeAsset pictogramAsset;
    public UIDocument makeSentence;

    private ScrollView pictogramsScroll;
    private ScrollView categoriesScroll;

    void Start()
    {
        GetThemesFromDirectories();

        VisualElement root = makeSentence.rootVisualElement;
        pictogramsScroll = root.Q<ScrollView>("pictosScroll");
        categoriesScroll = root.Q<ScrollView>("categoriesScroll");

        LoadThemes();        
    }

    void GetThemesFromDirectories()
    {
        var themesDirectories = Directory.GetDirectories(RUTA);

        foreach (var themeRoute in themesDirectories)
        {                        
            Theme theme = new Theme();
            theme.route = themeRoute;
            theme.name = themeRoute.Substring(themeRoute.LastIndexOf('/') + 1);            
            theme.image = Resources.Load<Sprite>(theme.name + "/ThemeImage");

            themes.Add(theme);
        }
    }

    void LoadThemes()
    {
        foreach (var theme in themes)
        {
            TemplateContainer item = SetThemeTemplate(themeAsset.Instantiate(), theme);
            categoriesScroll.contentContainer.Add(item);
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

    TemplateContainer SetPictogramTemplate(TemplateContainer item, Pictogram pictogram)
    {
        item.style.width = 300;
        item.style.height = 300;
        item.style.marginLeft = 15;
        item.style.marginRight = 15;
        item.Q<Label>("lblPictogramText").text = pictogram.name;
        item.Q<VisualElement>("pictogram").style.backgroundImage = new StyleBackground(pictogram.image);
        return item;
    }
}

[System.Serializable]
public class Pictogram
{
    public string name;
    public Sprite image;
}

[System.Serializable]
public class Theme
{
    public string name;
    public Sprite image;
    public List<Pictogram> pictograms = new List<Pictogram>();
    public string route;
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceLoader : MonoBehaviour
{
    const string RUTA = "Assets/Resources/";
    //const string AssetsRoute = Path.Combine("jar:file://" + Application.dataPath + "!assets/", "Resources/");

    public List<Theme> GetThemes()
    {
        List<Theme> themes = new List<Theme>();
        var themesDirectories = Directory.GetDirectories(RUTA);

        foreach (var themeRoute in themesDirectories)
        {
            Theme theme = new Theme();
            theme.route = themeRoute;
            theme.name = themeRoute.Substring(themeRoute.LastIndexOf('/') + 1);
            theme.image = Resources.Load<Sprite>(theme.name + "/ThemeImage");

            themes.Add(theme);
        }

        return themes;
    }

    public List<Pictogram> GetPictograms()
    {
        return null;
    }
}

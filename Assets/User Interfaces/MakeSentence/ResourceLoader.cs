using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ResourceLoader : MonoBehaviour
{        
    public string STREAMING_ASSETS_PATH;

    private void Awake()
    {        
        STREAMING_ASSETS_PATH = Application.streamingAssetsPath;
    }

    public List<Theme> GetThemes()
    {
        List<Theme> themes = new List<Theme>();
        var themesDirectories = Directory.GetDirectories(STREAMING_ASSETS_PATH);

        foreach (var themeRoute in themesDirectories)
        {
            string imagePath = Path.Combine(themeRoute, "ThemeImage.png");

            // Carga la imagen de manera asincrónica usando UnityWebRequest
            StartCoroutine(LoadImageAsync(imagePath, (sprite) =>
            {
                Theme theme = new Theme
                {
                    route = themeRoute,
                    name = themeRoute.Substring(themeRoute.LastIndexOf('/') + 1),
                    image = sprite
                };

                themes.Add(theme);
            }));
        }

        return themes;
    }

    private IEnumerator LoadImageAsync(string imagePath, System.Action<Sprite> callback)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + imagePath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            callback(sprite);
        }
        else
        {
            Debug.LogError("No se pudo cargar la imagen: " + www.error);
            callback(null);
        }
    }

    //public List<Theme> GetThemes()
    //{
    //    List<Theme> themes = new List<Theme>();
    //    var themesDirectories = Directory.GetDirectories(STREAMING_ASSETS_PATH);

    //    foreach (var themeRoute in themesDirectories)
    //    {
    //        string imagePath = Path.Combine(themeRoute, "ThemeImage.png");

    //        Theme theme = new Theme
    //        {
    //            route = themeRoute,
    //            name = themeRoute.Substring(themeRoute.LastIndexOf('\\') + 1),
    //            image = LoadImageFromFile(imagePath)
    //        };

    //        themes.Add(theme);
    //    }

    //    return themes;
    //}

    //private Sprite LoadImageFromFile(string imagePath)
    //{
    //    if (File.Exists(imagePath))
    //    {
    //        byte[] fileData = File.ReadAllBytes(imagePath);
    //        Texture2D texture = new Texture2D(3, 3); // Ajusta las dimensiones según tu imagen
    //        if (texture.LoadImage(fileData))
    //            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);            
    //        else            
    //            Debug.LogError("No se pudo cargar la imagen: " + imagePath);            
    //    }
    //    else        
    //        Debug.LogError("La imagen no existe: " + imagePath);        

    //    return null;
    //}

    public List<Pictogram> GetPictograms()
    {
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ResourceLoader : MonoBehaviour
{        
    private void Awake()
    {        
        BetterStreamingAssets.Initialize();
    }

    public List<Theme> GetThemes()
    {
        List<Theme> themes = new();
        var filesThemePath = LoadThemeFolders();

        foreach (var fileThemePath in filesThemePath)
        {            
            Theme theme = new()
            {
                name = Path.GetFileName(Path.GetDirectoryName(fileThemePath)),
                image = LoadImageFromFile(fileThemePath),
                pictograms = null
            };

            theme.priority = theme.GetPriority(theme.name);
            themes.Add(theme);
        }

        themes = themes.OrderBy(theme => theme.priority).ToList();        
        return themes;
    }

    public List<Pictogram> GetPictograms(Theme theme)
    {
        List<Pictogram> pictograms = new();
        var filesPictogramPath = LoadPictogramsFolders(theme.name);

        foreach (var filePictogramPath in filesPictogramPath)
        {
            string pictogramName = Path.GetFileNameWithoutExtension(Path.GetFileName(filePictogramPath));            
            Pictogram pictogram = new()
            {
                name = pictogramName,
                image = LoadImageFromFile(filePictogramPath)
            };
            
            pictograms.Add(pictogram);
        }
        
        return pictograms;
    }

    private string[] LoadThemeFolders()
    {
        return BetterStreamingAssets.GetFiles("Pictograms", "ThemeImage.png", SearchOption.AllDirectories);
    }

    private string[] LoadPictogramsFolders(string themeName)
    {
        string[] pngFolderFiles = BetterStreamingAssets.GetFiles($"Pictograms/{themeName}", "*.png", SearchOption.AllDirectories);
        string[] jpgfolderFiles = BetterStreamingAssets.GetFiles($"Pictograms/{themeName}", "*.jpg", SearchOption.AllDirectories);
        string[] folderFiles = pngFolderFiles.Concat(jpgfolderFiles).ToArray();

        string ignoreName = "ThemeImage.png";
        string[] filteredFiles = folderFiles.Where(file => Path.GetFileName(file) != ignoreName).ToArray();

        return filteredFiles;
    }

    private Sprite LoadImageFromFile(string imagePath)
    {
        if (BetterStreamingAssets.FileExists(imagePath))
        {
            byte[] fileData = BetterStreamingAssets.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(3, 3);
            if (texture.LoadImage(fileData))
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            else
                Debug.LogError("No se pudo cargar la imagen: " + imagePath);
        }
        else
            Debug.LogError("No existe la imagen: " + imagePath);

        return null;
    }
}

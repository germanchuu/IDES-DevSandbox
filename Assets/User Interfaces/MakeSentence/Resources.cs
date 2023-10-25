using System.Collections.Generic;
using UnityEngine;

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
    public int priority;
    public List<Pictogram> pictograms = new List<Pictogram>();    

    public int GetPriority(string name)
    {
        name = name.ToLower();

        Dictionary<string, int> keyPriorities = new()
        {
            {"pronombres", 1 },
            {"adverbios", 2 },
            {"expresiones", 3 },
            {"verbos", 4 },
            {"adjetivos", 5 },
            {"tiempo", 6 },
            {"ánimos", 7 },
            {"comida", 8 },
            {"fruta", 9 },
            {"calendario", 10 },
            {"animales", 11 },
            {"colores", 12 },
            {"ropa", 13 },
            {"cuerpo", 14 },
            {"transporte", 15 },
            {"continentes", 16 },
            {"juegos", 17 }
        };

        if (keyPriorities.TryGetValue(name, out int prio))
            return prio;

        return 0;
    }
}

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
            {"verbos", 2 },
            {"adjetivos", 3 },
            {"sustantivos", 4 },
            {"preposiciones", 5 },
            {"adverbios", 6 },
            {"expresiones", 7 },
            {"ánimos", 8 },
            {"objetos", 9 },
            {"tiempo", 10 },
            {"familia",11 },
            {"comida", 12 },
            {"fruta", 13 },            
            {"cuerpo", 14 },
            {"lugares", 15 },
            {"ropa", 16 },
            {"transporte", 17 },
            {"calendario", 18 },
            {"animales", 19 },
            {"colores", 20 },
            {"juegos", 21 },
            {"música", 22 },
            {"profesiones", 23 },
            {"formas", 24 },
            {"continentes", 25 },
        };

        if (keyPriorities.TryGetValue(name, out int prio))
            return prio;

        return 0;
    }
}

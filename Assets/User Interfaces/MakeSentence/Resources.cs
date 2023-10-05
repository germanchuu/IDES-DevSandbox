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
    public List<Pictogram> pictograms = new List<Pictogram>();
    public string route;
}

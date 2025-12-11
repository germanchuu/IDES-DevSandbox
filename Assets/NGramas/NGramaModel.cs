using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class NGramaModel
{
    public static List<string> GetNGramaTransitions(string sentence)
    {
        if (string.IsNullOrEmpty(sentence))                    
            return new List<string>();       

        var model = NGramaManager.Instance;
        var transitionKeys = model.GetTransitions(sentence)?.Select(t => t.Key);

        return transitionKeys?.ToList() ?? new List<string>();
    }

    public static void AddNGramaTransitions(string sentences)
    {
        if (string.IsNullOrEmpty(sentences))
            return;

        var model = NGramaManager.Instance;
        model.AddTransition(sentences);
    }
}

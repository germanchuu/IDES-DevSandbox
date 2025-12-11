using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NGramaManager
{
    private static NGramaManager instance;

    private NGramaTransition keyTransition;
    private readonly char[] delimiters;
    private readonly NGramaDB db;

    private const int NGRAMAS_MAX = 3;

    public NGramaManager()
    {        
        delimiters = new char[] { ' ', '.', ',', '!', '?', '¡', '¿', '-', '_', '\n', '\r' };
        db = new NGramaDB();
    }

    public static NGramaManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NGramaManager();
            }
            return instance;
        }
    }

    private string[] Tokenization(string sentence)
    {
        string[] tokens = sentence.ToLower().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        return tokens;
    }

    public void AddTransition(string sentence)
    {        
        string[] tokens = Tokenization(sentence);

        for (int n = 1; n <= NGRAMAS_MAX; n++)
        {
            for (int i = 0; i < tokens.Length - n; i++)
            {
                string currentNGrama = string.Join(" ", tokens.Skip(i).Take(n));
                string nextWord = tokens[i + n];

                if (!db.ExitsNGrama(currentNGrama))
                {
                    keyTransition = new NGramaTransition(currentNGrama);
                    keyTransition.Transitions.Add(new Transition(nextWord, 1));
                    db.SaveNGrama(keyTransition);

                    continue;
                }

                keyTransition = db.GetNGramaByKey(currentNGrama);
                Transition currentTransition = keyTransition.Transitions.Find(t => t.Key == nextWord);

                if (currentTransition == null)
                    keyTransition.Transitions.Add(new Transition(nextWord, 1));
                else
                    currentTransition.Concurrency++;

                db.UpdateNGrama(keyTransition);
            }
        }
    }

    public List<Transition> GetTransitions(string sentence)
    {        
        if (string.IsNullOrEmpty(sentence))
            return null;

        int transitionsCount = 5;

        string[] tokens = Tokenization(sentence);
        string nGrama = string.Join(" ", tokens.TakeLast(Math.Min(NGRAMAS_MAX, tokens.Length)));
        NGramaTransition nGramaTransition = db.GetNGramaByKey(nGrama);

        if (nGramaTransition == null)
            return null;

        var transitions = nGramaTransition.Transitions;
        int maxTransitions = Math.Min(transitionsCount, transitions?.Count ?? 0);
        return transitions.OrderByDescending(t => t.Concurrency).Take(maxTransitions).ToList();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceButtonManager
{
    public static IObserver ObserverSavedSentences;    

    public static string DeleteLastWord(string sentence)
    {
        string[] words = sentence.Split(' ');

        if (words.Length >= 2)
        {
            Array.Resize(ref words, words.Length - 1);
            return string.Join(" ", words);
        }
        else
            return string.Empty;
    }

    public static void SaveSentence(string sentence, PopUpHandler popUp)
    {        
        sentence.Trim();

        popUp.ShowPopUp("Confirmar", $"¿Estás seguro de guardar la oración?\n\n\"{sentence}\"", () =>
        {
            SavedSentencesManager manager = new();
            manager.InsertData(sentence);

            if (ObserverSavedSentences != null)
            {
                ISubject subject = new Subject();
                subject.RegisterObserver(ObserverSavedSentences);
                subject.NotifyObserver();                
            }            
        });        
    }    
}

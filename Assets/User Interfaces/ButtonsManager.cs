using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceButtonManager
{
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
        popUp.ShowPopUp("Guardar oración", $"¿Estás seguro de guardar la oración?\n\n\"{sentence}\"", () =>
        {
            SavedSentencesManager manager = new();
            manager.InsertData(sentence);              
        });        
    } 
    
    public static void DeleteSavedSentence(KeyValuePair<int, string> sentenceSelected, PopUpHandler popUp)
    {
        popUp.ShowPopUp("Eliminar oración", $"¿Estás seguro de eliminar la oración?\n\n\"{sentenceSelected.Value}\"", () =>
        {
            SavedSentencesManager manager = new();
            manager.DeleteData(sentenceSelected.Key);
        });
    }
}

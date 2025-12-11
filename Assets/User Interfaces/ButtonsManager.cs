using LeastSquares.Overtone;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ButtonManager
{
    public async static void TextToSpeech(string text, TTSPlayer tSPlayer)
    {
        await tSPlayer.Speak(text);
    }

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
            manager.InsertData(char.ToUpper(sentence[0]) + sentence[1..]);
            NGramaModel.AddNGramaTransitions(sentence);
        });        
    } 
    
    public static void DeleteSavedSentence(KeyValuePair<int, string> sentenceSelected, PopUpHandler popUp, IObserver observer)
    {
        popUp.ShowPopUp("Eliminar oración", $"¿Estás seguro de eliminar la oración?\n\n\"{sentenceSelected.Value}\"", () =>
        {
            SavedSentencesManager manager = new();
            manager.DeleteData(sentenceSelected.Key);
            observer.Notify();
        });
    }

    public static void AddNewDefaultSentence(string sentence, PopUpHandler popUp, IObserver observer)
    {
        popUp.ShowPopUp("Añadir nueva oración", $"¿Estás seguro de añadir la oración?\n\n\"{sentence}\"", () =>
        {
            SavedSentencesManager manager = new();
            manager.InsertData(sentence);
            observer.Notify();
        });
    }

    public static void DeleteDefaultSentence(KeyValuePair<int, string> sentenceSelected, PopUpHandler popUp, IObserver observer)
    {
        popUp.ShowPopUp("Eliminar oración", $"¿Estás seguro de eliminar la oración?\n\n\"{sentenceSelected.Value}\"", () =>
        {
            DefaultSentencesManager manager = new();
            manager.DeleteData(sentenceSelected.Key);
            observer.Notify();
        });
    }

    public static void UpdateQuickAccess(string sentence, PopUpQuickAccessHandler popUp)
    {        
        popUp.ShowPopUp($"Cambiar \"{sentence}\" por:", sentence);
    }

    public static void UpdateConfiguration(Configuration config, Action method, PopUpHandler popUp)
    {
        popUp.ShowPopUp("¿Confirmar cambios?", "Asegurese de que sus cambios son correctos.", () =>
        {
            ConfigurationManager manager = new();
            manager.UpdateData(config);
            method?.Invoke();

            DefaultSentencesManager defaultManager = new();
            defaultManager.UpdateData("");                        
        });
    }

    public static void ShowErrorsValidations(List<string> errors, PopUpHandler popUp)
    {
        StringBuilder errorsInLine = new StringBuilder();

        for (int i = 0; i < errors.Count; i++)
        {
            errorsInLine.Append(errors[i]);
            if (i < errors.Count - 1) { 
                errorsInLine.Append("\n");}
        }

        popUp.ShowPopUp("Campos inválidos", errorsInLine.ToString(), () => { });
    }
}

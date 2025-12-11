using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSentences
{
    public Dictionary<int, string> initialSentences;
    public Dictionary<int, string> sentences;

    public DefaultSentences()
    {
        if (initialSentences == null)
            GetSentences();

        sentences = new Dictionary<int, string>();
    }

    public void GetSentences()
    {
        Configuration config = Configuration.GetGlobalConfiguration();

        initialSentences = new Dictionary<int, string>()
            {
                { 1, "Hola, buen día."},
                { 2, "Hola, buenas tardes." },
                { 3, "Hola, buenas noches."},
                { 4, $"Mi nombre es {config.name}."},
                { 5, $"Me llamo {config.name} {config.lastName}."},
                { 6, $"Mi edad es {config.age} años."},
                { 7, $"Mi número de teléfono es {config.phoneNumber}."},
                { 8, $"Mi contacto de emergencia es {config.emergencyContact}."},
                { 9, $"Muchas gracias."},
                { 10, $"No gracias."},
                { 11, $"Si por favor."},
                { 12, $"Tengo que ir al baño"},
            };
    }
}

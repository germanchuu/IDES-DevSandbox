using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceButtonManager 
{
    public static string DeleteWord(string sentence)
    {
        int lastSpaceIndex = sentence.Trim().LastIndexOf(' ');
        
        if (lastSpaceIndex != -1)        
            sentence = sentence.Substring(0, lastSpaceIndex);        
        else
            sentence = "";        

        return sentence;
    }
}

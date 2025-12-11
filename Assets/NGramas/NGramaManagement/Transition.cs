using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition
{
    private string key;
    private int concurrency;

    public int Concurrency
    {
        get { return concurrency; }
        set { concurrency = value; }
    }

    public string Key
    {
        get { return key; }
        set { key = value; }
    }

    public Transition(string key, int concurrency)
    {
        Concurrency = concurrency;
        Key = key;
    }
}

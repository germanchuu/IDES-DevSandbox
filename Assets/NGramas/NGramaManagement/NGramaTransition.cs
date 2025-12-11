using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGramaTransition
{
    private string key;
    private List<Transition> transitions;

    public string Key
    {
        get { return key; }
        set { key = value; }
    }

    public List<Transition> Transitions
    {
        get { return transitions; }
        set { transitions = value; }
    }

    public NGramaTransition()
    {
        
    }

    public NGramaTransition(string key)
    {
        Key = key;
        Transitions = new List<Transition>();
    }

    public NGramaTransition(string key, List<Transition> transitions)
    {
        Key = key;
        Transitions = transitions;
    }
}

public class NGramaTransitionSerialized
{
    private string key;
    private string serializedTransitions;

    public string Key
    {
        get { return key; }
        set { key = value; }
    }

    public string SerializedTransitions
    {
        get { return serializedTransitions; }
        set { serializedTransitions = value; }
    }

    public NGramaTransitionSerialized()
    {

    }

    public NGramaTransitionSerialized(string key, string serializedTransitions)
    {
        Key = key;
        SerializedTransitions = serializedTransitions;
    }
}

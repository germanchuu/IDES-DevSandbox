using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject : ISubject
{
    List<IObserver> observers = new List<IObserver>();

    public void NotifyObserver()
    {
        foreach (var observer in observers)
        {
            observer.UpdateUI();
        }
    }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }
}

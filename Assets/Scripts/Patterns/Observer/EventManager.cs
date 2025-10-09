using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    private static Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();
    private static Dictionary<string, Action<object>> eventDictionaryWithData = new Dictionary<string, Action<object>>();

    public static void Subscribe(string eventName, Action listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = null;
        }
        eventDictionary[eventName] += listener;
    }

    public static void Subscribe(string eventName, Action<object> listener)
    {
        if (!eventDictionaryWithData.ContainsKey(eventName))
        {
            eventDictionaryWithData[eventName] = null;
        }
        eventDictionaryWithData[eventName] += listener;
    }

    public static void Unsubscribe(string eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
        }
    }

    public static void Unsubscribe(string eventName, Action<object> listener)
    {
        if (eventDictionaryWithData.ContainsKey(eventName))
        {
            eventDictionaryWithData[eventName] -= listener;
        }
    }

    public static void TriggerEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName) && eventDictionary[eventName] != null)
        {
            eventDictionary[eventName].Invoke();
        }
    }

    public static void TriggerEvent(string eventName, object data)
    {
        if (eventDictionaryWithData.ContainsKey(eventName) && eventDictionaryWithData[eventName] != null)
        {
            eventDictionaryWithData[eventName].Invoke(data);
        }
    }

    public static void ClearAllEvents()
    {
        eventDictionary.Clear();
        eventDictionaryWithData.Clear();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EventManager", menuName = "Managers/EventManager")]
public class EventManager : BaseInjectable
{
    private static Dictionary<Type, object> _eventDictionary = new Dictionary<Type, object>();

    public void Add<T>(Action<T> listener) where T : BaseEvent
    {
        if (!_eventDictionary.ContainsKey(typeof(T)))
        {
            var eventList = new List<Action<T>>();
            eventList.Add(listener);
            _eventDictionary.Add(typeof(T), eventList);
        }
        else
        {
            _eventDictionary.TryGetValue(typeof(T), out var eventList);
            ((List<Action<T>>) eventList)?.Add(listener);
        }
    }

    public void Remove<T>(Action<T> listener) where T : BaseEvent
    {
        if (_eventDictionary.ContainsKey(typeof(T)))
        {
            _eventDictionary.TryGetValue(typeof(T), out var eventList);
            ((List<Action<T>>) eventList)?.Remove(listener);
        }
    }

    public void TriggerEvent<T>() where T : BaseEvent
    {
        if (_eventDictionary.ContainsKey(typeof(T)))
        {
            _eventDictionary.TryGetValue(typeof(T), out var eventList);

            foreach (var action in (List<Action<T>>) eventList)
            {
                action?.Invoke(default);
            }
        }
    }

    public void TriggerEvent<T>(T eventArgs) where T : BaseEvent
    {
        if (_eventDictionary.ContainsKey(typeof(T)))
        {
            _eventDictionary.TryGetValue(typeof(T), out var eventList);

            foreach (var action in (List<Action<T>>) eventList)
            {
                action?.Invoke(eventArgs);
            }
        }
    }
}
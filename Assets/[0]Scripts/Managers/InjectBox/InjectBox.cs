using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;

public class InjectBox : Singleton<InjectBox>
{
    private Dictionary<Type, object> _injectables = new Dictionary<Type, object>();

    public static void Add(object obj)
    {
        Instance._injectables.Add(obj.GetType(), obj);
    }

    public static void InitializeStartInjectables()
    {
        var injectablesList = Instance._injectables.Values.ToList();

        foreach (var injectable in injectablesList)
        {
            if (injectable is IAwake)
            {
                ((IAwake)injectable).OnAwake();
            }
        }
        
        foreach (var injectable in injectablesList)
        {
            if (injectable is IStart)
            {
                ((IStart)injectable).OnStart();
            }
        }
        
        foreach (var injectable in injectablesList)
        {
            if (injectable is ILateStart)
            {
                ((ILateStart)injectable).OnLateStart();
            }
        }

    }

    public static T Get<T>()
    {
        Instance._injectables.TryGetValue(typeof(T), out var manager);
        return (T) manager;
    }

    public static void DisableAll()
    {
        var injectablesList = Instance._injectables.Values.ToList();
        
        foreach (var injectable in injectablesList)
        {
            if (injectable is IDisable)
            {
                ((IDisable)injectable).LocalDisable();
            }
        }
    }
}
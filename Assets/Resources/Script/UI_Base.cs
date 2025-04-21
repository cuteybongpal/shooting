using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Base : MonoBehaviour
{
    Dictionary<string, Object> dict = new Dictionary<string, Object>();
    void Start()
    {
        Init();
    }
    protected virtual void Init()
    {

    }
    protected T Bind<T>(string name) where T : Component
    {
        Object obj;
        if (dict.TryGetValue(name, out obj))
            return obj as T;
        T[] components = GetComponentsInChildren<T>();
        if (components.Length == 1)
        {
            dict.Add(name, components[0]);
            return components[0];
        }
        foreach (T component in components)
        {
            if (component.gameObject.name == name)
            {
                dict.Add(name, component);
                return component;
            }
        }
        return null;
    }
}

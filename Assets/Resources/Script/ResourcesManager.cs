using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Resources;

public class ResourcesManager
{
    public Dictionary<string, GameObject> Prefab = new Dictionary<string, GameObject>();

    public void Init()
    {
        GameObject[] gos = Resources.LoadAll<GameObject>("Prefab");
        foreach (GameObject go in gos)
        {
            Prefab.Add(go.name, go);
        }
    }
}

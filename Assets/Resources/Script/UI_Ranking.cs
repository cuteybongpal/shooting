using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ranking : UI_Base
{
    void Start()
    {
        for (int i = 0; i < GameManager.Data.Score.Count; i++)
        {
            if (i >= 5)
                break;
            Bind<Text>($"Score{i + 1}").text = $"{GameManager.Data.Get(i)} Á¡";
        }
        Bind<Button>("Close").onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }
}

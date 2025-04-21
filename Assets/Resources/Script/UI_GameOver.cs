using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UI_GameOver : UI_Base
{
    protected override void Init()
    {
        Bind<Button>("GameOverButton").onClick.AddListener(() => { SceneManager.LoadScene(0); });
    }
}

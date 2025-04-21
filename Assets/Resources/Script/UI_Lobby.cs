using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Lobby : UI_Base
{
    public GameObject UI_Ranking;
    void Start()
    {
        Bind<Button>("Start").onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        Bind<Button>("Exit").onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Bind<Button>("Ranking").onClick.AddListener(() =>
        {
            Instantiate(UI_Ranking);
        });
    }

}

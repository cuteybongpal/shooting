using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : UI_Base
{
    stage _stage;
    Text timeText;
    Text scoreText;
    Slider hpSlider;
    PlayerController _player;
    void Start()
    {
        _stage = GameObject.Find("stage").GetComponent<stage>();
        _player = _stage.player;
        timeText = Bind<Text>("Text_Time");
        scoreText = Bind<Text>("Text_Score");
        hpSlider = Bind<Slider>("Slider_HP");

        _stage.ScoreChangeAction -= RefreshScore;
        _stage.ScoreChangeAction += RefreshScore;
        _stage.TimeChangeAction -= RefreshTime;
        _stage.TimeChangeAction += RefreshTime;
        _player.OnDamaged -= RefreshHp;
        _player.OnDamaged += RefreshHp;

        hpSlider.maxValue = _player.MaxHP;
        hpSlider.value = hpSlider.maxValue;
    }
    void RefreshScore(int score)
    {
        scoreText.text = $"Á¡¼ö : {score}";
    }
    void RefreshTime(float time)
    {
        int minute = (int)time / 60;
        float seconds = time - minute * 60;
        timeText.text = $"{minute} : {seconds}";
    }
    void RefreshHp(int currentHp)
    {
        hpSlider.value = currentHp;
    }
}

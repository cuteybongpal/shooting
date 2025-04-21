using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class stage : MonoBehaviour
{
    public int GameTime;
    float time;
    public PlayerController player;
    public BossController boss;
    int score;
    int pain;
    public event Action<float> TimeChangeAction;
    public event Action<int> ScoreChangeAction;
    public event Action<int> PainChangeAction;
    public float Time
    {
        get { return time; }
        set {
            time = value;
            TimeChangeAction?.Invoke(time);
        }
    }
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            ScoreChangeAction?.Invoke(score);
        }
    }
    public int Pain
    {
        get { return pain; }
        set {
            pain = value;
            if (pain < 0)
                pain = 0;
            PainChangeAction?.Invoke(pain);
        }
    }

    void Start()
    {
        GameManager.Instance.State = GameManager.GameState.Normal;
        Time = 0;
        Score = 0;
        player = Spawn("Player", new Vector2(0, -2.76f), Quaternion.identity).GetComponent<PlayerController>();
        Spawn("UI_InGame", Vector2.zero, Quaternion.identity);
        StartCoroutine(Gamestart());
        player.PlayerDie += GameOver;
    }
    IEnumerator Gamestart()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Time += 1f;
            if (Time >= GameTime)
                break;
            int rand = UnityEngine.Random.Range(0, 3);
            Vector2 pos = Vector2.zero;
            Quaternion q = Quaternion.identity;
            string key = "";
            switch (rand)
            {
                case 0:
                    key = "Enemy A";
                    int Rand = UnityEngine.Random.Range(0, 2);
                    if (Rand == 0)
                    {
                        pos = new Vector2(4, UnityEngine.Random.Range(0, 2.81f));
                        q = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-100f,-120f));
                    }
                    else
                    {
                        pos = new Vector2(-4, UnityEngine.Random.Range(0, 2.81f));
                        q = Quaternion.Euler(0, 0, UnityEngine.Random.Range(100f, 120f));
                    }
                        
                    break;
                case 1:
                    key = "Enemy B";
                    int R = UnityEngine.Random.Range(0, 2);
                    if (R == 0)
                    {
                        pos = new Vector2(4, UnityEngine.Random.Range(-1f, -2.81f));
                        q = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-160f, -165f));
                    }
                    else
                    {
                        pos = new Vector2(-4, UnityEngine.Random.Range(-1f, -2.81f));
                        q = Quaternion.Euler(0, 0, UnityEngine.Random.Range(160f, 165f));
                    }
                    break;
                case 2:
                    key = "Enemy C";
                    pos = new Vector2(UnityEngine.Random.Range(-2f, 2f), 6);
                    q = Quaternion.Euler(0, 0, 0);
                    break;
            }
            EnemyController ec =Spawn(key, pos, q).GetComponent<EnemyController>();
            ec.EnemyScreenOut += PainUp;
            ec.Death += MonsterDie;
        }
        BossPage();
    }

    void BossPage()
    {
        boss = Spawn("Boss1",new Vector2(0, 3.25f), Quaternion.Euler(0,0,180f)).GetComponent<BossController>();
        Spawn("UI_BossHP", Vector2.zero, Quaternion.identity).GetComponent<UI_HPbar>().Set(boss);
        boss.die += StageClear;
    }

    GameObject Spawn(string key, Vector2 pos, Quaternion q)
    {
        GameObject go = Instantiate(GameManager.Resource.Prefab[key]);
        go.transform.position = pos;
        go.transform.rotation = q;
        return go;
    }

    
    public void GameOver()
    {
        GameManager.Instance.State = GameManager.GameState.GameOver;
    }
    public void StageClear()
    {
        GameManager.Data.Add(Score);
        GameManager.Data.StageisClear[GameManager.Data.CurrentStage] = true;
        GameManager.Instance.State = GameManager.GameState.Clear;
    }
    void PainUp()
    {
        Pain += 1;
    }
    void MonsterDie(int score, Vector2 pos)
    {
        Score += score;
        int rand =  UnityEngine.Random.Range(0,10);
        string key = "";
        if (rand == 0)
        {
            int rnd = UnityEngine.Random.Range(0, 3);
            switch(rnd)
            {
                case 0:
                    key = "Item_DamageUp";
                    break;
                case 1:
                    key = "Item_heal";
                    break;
                case 2:
                    key = "Item_PainDown";
                    break;
            }
            Spawn(key, pos, Quaternion.identity).GetComponent<Item>()._stage = this;
        }
    }
    void BossDie()
    {

    }
}

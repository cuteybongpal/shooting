using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    static DataManager dataManager;
    static ResourcesManager _resource = new ResourcesManager();
    static BulletManager _bullet = new BulletManager();
    public static GameManager Instance { get { return instance; } }
    public static DataManager Data { get { return dataManager; } }
    public static ResourcesManager Resource { get { return _resource; } }
    public static BulletManager Bullet { get { return _bullet; } }
    public enum GameState
    {
        Normal,
        Pause,
        GameOver,
        Clear,
    }
    GameState _state = GameState.Normal;
    public GameState State {
        get { return _state; }
        set
        {
            _state = value;
            switch(_state)
            {
                case GameState.Normal:
                    Time.timeScale = 1f;
                    break;
                case GameState.Pause:
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    Instantiate(Resource.Prefab["UI_GameOver"]);
                    Time.timeScale = 0f;
                    break;
                case GameState.Clear:
                    Instantiate(Resource.Prefab["UI_GameWin"]);
                    Time.timeScale = 0f;
                    break;
            }
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            Screen.SetResolution(1080, 1960, false); // false는 창모드, true는 전체화면
            instance = this;
            DontDestroyOnLoad(gameObject);
            Resource.Init();
            dataManager = new DataManager();
        }
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public float Speed;
    public int MaxHP;
    int currenthp;
    public event Action<int> OnDamaged;
    public int CurrentHP
    {
        get { return currenthp; }
        set
        {
            currenthp = value;
            OnDamaged?.Invoke(currenthp);
        }
    }

    public enum CreatureState
    {
        Idle,
        MoveLeft,
        MoveRight,
        Skill,
        Die,
    }
    void Start()
    {
        Init();
    }
    protected virtual void Init()
    {
        StartCoroutine(Udt());
    }
    IEnumerator Udt()
    {
        while (true)
        {
            Upt();
            yield return null;
        }
    }
    protected virtual void Upt()
    {

    }
    protected virtual void Die()
    {
        StopCoroutine(Udt());
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : CreatureController, Atk, Damaged
{
    Transform[] shooters;
    PlayerController _player;
    Coroutine[] coroutines;

    public int Atk { get; set; } = 10;
    public float Attack1Cooldown;
    public float Attack2Cooldown;
    public float Attack3Cooldown;
    public event Action Damaged;
    public event Action die;
    protected override void Init()
    {
        CurrentHP = MaxHP;
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        shooters = new Transform[transform.childCount];
        for (int i = 0; i < shooters.Length; i++)
        {
            shooters[i] = transform.GetChild(i);
        }
        coroutines = new Coroutine[3];
        
        coroutines[0] = StartCoroutine(ExecuteAttack1());
        coroutines[1] = StartCoroutine(ExecuteAttack2());
        coroutines[2] = StartCoroutine(ExecuteAttack3());
    }

    protected override void Die()
    {
        die?.Invoke();
        StopAllCoroutines();
    }
    void Attack1()
    {
        for (int i =0; i < shooters.Length; i++)
        {
            Vector2 dir = (_player.transform.position - shooters[i].position).normalized;
            float angle = Mathf.Asin(dir.x) * Mathf.Rad2Deg;
            GameManager.Bullet.GenerateBullet(dir, Atk,Quaternion.Euler(0,0,angle + 180), shooters[i].position, this);
        }
    }
    void Attack2()
    {
        GameManager.Bullet.GenerateBullet(Vector2.down, Atk * 3, Quaternion.Euler(0,0,-90), transform.position, this, 1);
    }
    void Attack3()
    {
        GameManager.Bullet.GenerateBullet(Vector2.down, Atk * 2, Quaternion.Euler(0, 0, -90), (Vector2)transform.position + Vector2.right, this, 2);
        GameManager.Bullet.GenerateBullet(Vector2.down, Atk * 2, Quaternion.Euler(0, 0, -90), (Vector2)transform.position + Vector2.left, this, 2);
    }
    IEnumerator ExecuteAttack1()
    {
        while (true)
        {
            yield return new WaitForSeconds(Attack1Cooldown);
            Attack1();
        }
    }
    IEnumerator ExecuteAttack2()
    {
        while (true)
        {
            yield return new WaitForSeconds(Attack2Cooldown);
            Attack2();
        }
    }
    IEnumerator ExecuteAttack3()
    {
        while (true)
        {
            yield return new WaitForSeconds(Attack3Cooldown);
            Attack3();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag("Bullet"))
            return;
        if (gameObject.CompareTag(collision.GetComponent<Bullet>().Owner.gameObject.tag))
            return;
        damaged(collision.GetComponent<Bullet>().Atk);
    }

    public void damaged(int atk)
    {
        if (CurrentHP <=0)
            return;
        Damaged?.Invoke();
        CurrentHP -= atk;
        if (CurrentHP <= 0)
        {
            Die();
        }
    }
}

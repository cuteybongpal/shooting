using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class EnemyController : CreatureController, Damaged, Atk
{
    public int Atk { get; set; } = 10;
    public float CoolDown;
    public int gainScore;
    Transform[] shooter;
    Transform Player;
    public event Action EnemyScreenOut;
    public event Action<int, Vector2> Death;
    public enum EnemyType
    {
        pri,
        triple,
        dbl,
    }
    public EnemyType Type;
    public void damaged(int atk)
    {
        if (CurrentHP <= 0)
            return;
        CurrentHP -= atk;
        StartCoroutine(inv());
        if (CurrentHP <= 0)
            die();
    }
    IEnumerator inv()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().enabled = true;
    }

    protected override void Init()
    {
        CurrentHP = MaxHP;
        shooter = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            shooter[i] = transform.GetChild(i);
        }
        StartCoroutine(ShootBullet());
        if (GameObject.FindWithTag("Player") != null)
            Player = GameObject.FindWithTag("Player").transform;
        base.Init();
    }
    protected override void Upt()
    {
        Move();
        if (transform.position.y <= -6f)
        {
            EnemyScreenOut?.Invoke();
            gameObject.SetActive(false);
        }   
        if (transform.position.y >= 6.5f)
        {
            EnemyScreenOut?.Invoke();
            gameObject.SetActive(false);
        }   
        if (transform.position.x <= -4.5f)
        {
            EnemyScreenOut?.Invoke();
            gameObject.SetActive(false);
        }
        if (transform.position.x >= 4.5f)
        {
            EnemyScreenOut?.Invoke();
            gameObject.SetActive(false);
        }
            
    }
    void Move()
    {
        transform.Translate(0, -Speed * Time.deltaTime, 0);
    }

    void die()
    {
        Death?.Invoke(gainScore, transform.position);
        gameObject.SetActive(false);

        base.Die();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag("Bullet"))
            return;
        if (gameObject.CompareTag(collision.GetComponent<Bullet>().Owner.gameObject.tag))
            return;
        damaged(collision.GetComponent<Atk>().Atk);
    }
    protected virtual IEnumerator ShootBullet()
    {
        while (true)
        {
            switch (Type)
            {
                case EnemyType.pri:
                    yield return new WaitForSeconds(CoolDown);
                    if (Player != null)
                    {
                        for (int i =0; i < shooter.Length; i++)
                        {
                            Vector2 dir = (Player.position - transform.position).normalized;
                            float angle = Mathf.Asin(dir.y) * Mathf.Rad2Deg - 90;
                            if (dir.x < 0)
                                angle *= -1;
                            GameManager.Bullet.GenerateBullet((Player.position - transform.position).normalized, Atk, Quaternion.Euler(0, 0, angle), shooter[i].position, this);
                        }
                    }
                    break;
                case EnemyType.triple:
                    yield return new WaitForSeconds(CoolDown);
                    if (Player != null)
                    {
                        for (int i =0;i < shooter.Length; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                Vector2 dir = new Vector2(Player.position.x - transform.position.x + (j - 1) * 2, Player.position.y - transform.position.y).normalized;
                                float angle = Mathf.Asin(dir.y) * Mathf.Rad2Deg - 90;
                                if (dir.x < 0)
                                    angle *= -1;
                                GameManager.Bullet.GenerateBullet(dir, Atk, Quaternion.Euler(0, 0, angle), shooter[i].position, this);
                            }
                        }
                    }
                    break;
                case EnemyType.dbl:
                    yield return new WaitForSeconds(CoolDown);
                    if (Player != null)
                    {
                        for (int j = 0; j < shooter.Length; j++)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                Vector2 dir = (Player.position - transform.position).normalized;
                                float angle = Mathf.Asin(dir.y) * Mathf.Rad2Deg - 90;
                                if (dir.x < 0)
                                    angle *= -1;
                                GameManager.Bullet.GenerateBullet((Player.position - transform.position).normalized, Atk, Quaternion.Euler(0, 0, angle), shooter[i].position, this);
                                yield return new WaitForSeconds(0.1f);
                            }
                        }
                    }
                    break;
            }
            
        }
    }
}

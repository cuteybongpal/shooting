using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CreatureController, Damaged, Atk
{
    Animator anim;
    CreatureState _state = CreatureState.Idle;
    public float AtkCoolDown;
    bool CanAttack = true;
    AudioSource _audio;
    Coroutine[] coroutines = new Coroutine[3];
    Transform[] shooter;
    public GameObject[] Skills;
    public float[] SkillCoolDown;

    bool[] CanUseSkill;
    public event Action PlayerDie;

    CreatureState State
    {
        get { return _state; }
        set
        {
            _state = value;

            switch(_state)
            {
                case CreatureState.Idle:
                    Idle();
                    break;
                case CreatureState.MoveLeft:
                    MoveLeft();
                    break;
                case CreatureState.MoveRight:
                    MoveRight();
                    break;
                case CreatureState.Die:
                    StartCoroutine(die());
                    break;
                default:
                    break;
            }
        }
    }

    public int Atk { get; set; } = 10;

    protected override void Init()
    {
        CurrentHP = MaxHP;
        anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        shooter = new Transform[transform.childCount];
        CanUseSkill = new bool[Skills.Length];
        for (int i =  0; i < CanUseSkill.Length; i++)
        {
            CanUseSkill[i] = true;
        }
        for (int i = 0; i < shooter.Length; i++)
        {
            shooter[i] = transform.GetChild(i);
        }

        base.Init();

    }
    protected override void Upt()
    {
        if (Speed <= 0)
            return;

        if (State == CreatureState.Die)
            return;
        if (Input.GetKeyDown(KeyCode.E) && CanUseSkill[0])
        {
            GameObject go = Instantiate(Skills[0]);
            go.GetComponent<Bullet>().SetVelocity(Vector2.up, Quaternion.Euler(0, 0, 90));
            go.GetComponent<Bullet>().Owner = this;
            go.GetComponent<Bullet>().Atk = 100;
            go.transform.position = transform.position;
            StartCoroutine(UseSkill(0));
        }
        if (Input.GetKeyDown(KeyCode.Q) && CanUseSkill[1])
        {
            Skills[1].GetComponent<ParticleSystem>().Play();
            StartCoroutine(UseSkill(1));
        }
        if (Input.GetKeyDown(KeyCode.R) && CanUseSkill[2])
        {
            Skills[2].GetComponent<ParticleSystem>().Play();
            StartCoroutine(UseSkill(2));
        }
        if (Input.GetKey(KeyCode.Space) && CanAttack)
        {
            CanAttack = false;
            for (int i =0; i < shooter.Length; i++)
            {
                GameManager.Bullet.GenerateBullet(Vector2.up, Atk, Quaternion.identity, shooter[i].position, this);
            }
            _audio.Play();
            coroutines[0] = StartCoroutine(WaitCoolDown());
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0,Speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -Speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            State = CreatureState.MoveLeft;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            State = CreatureState.MoveRight;
        }
        else
        {
            State = CreatureState.Idle;
        }
    }

    IEnumerator WaitCoolDown()
    {    
        yield return new WaitForSeconds(AtkCoolDown);
        CanAttack = true;
    }
    #region StateMethod
    void Idle()
    {
        anim.Play("PlayerIdle");
    }
    void MoveLeft()
    {
        anim.Play("PlayerMoveLeft");
        transform.Translate( -Speed * Time.deltaTime,0, 0);
    }
    void MoveRight()
    {
        anim.Play("PlayerMoveRight");
        transform.Translate(Speed * Time.deltaTime,0, 0);
    }
    IEnumerator die()
    {
        
        anim.Play("PlayerDie");
        RuntimeAnimatorController animator = anim.runtimeAnimatorController;
        AnimationClip[] animationClips = animator.animationClips;
        AnimationClip clip;
        for (int i = 0; i < animationClips.Length; i++)
        {
            if (animationClips[i].name == "PlayerDie")
            {
                clip = animationClips[i];
                yield return new WaitForSeconds(clip.length);
                break;
            }
        }
        PlayerDie?.Invoke();
        base.Die();
        gameObject.SetActive(false);
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (gameObject.CompareTag(collision.GetComponent<Bullet>().Owner.gameObject.tag))
                return;
            damaged(collision.GetComponent<Atk>().Atk);
        }
        else if (collision.CompareTag("Item"))
        {
            Item i = collision.GetComponent<Item>();
            switch (i.type)
            {
                case Item.ItemType.DamageUp:
                    StartCoroutine(Buff(10, 0, 5f));
                    break;
                case Item.ItemType.PainDown:
                    StartCoroutine(invincible(3));
                    break;
                case Item.ItemType.Heal:
                    StartCoroutine(Buff(10, 1));
                    break;

            }
            Destroy(collision.gameObject);
        }
    }

    public void damaged(int atk)
    {
        if (CurrentHP <= 0)
            return;
        CurrentHP -= atk;
        if (CurrentHP <= 0)
            State = CreatureState.Die;
        else
            coroutines[1] = StartCoroutine(invincible(0.5f));
    }
    IEnumerator invincible(float duration)
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0.5f;
        GetComponent<SpriteRenderer>().color = color;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(duration);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        GetComponent<BoxCollider2D>().enabled = true;
    }
    IEnumerator Buff(int BuffAmount,int BuffType, float Duration = 0)
    {
        if (BuffType == 0)
        {
            Atk += BuffAmount;
            GetComponent<SpriteRenderer>().color = new Color(1, 0.7133f, 0.7133f);
            yield return new WaitForSeconds(Duration);
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            Atk -= BuffAmount;
        }
        else
        {
            CurrentHP += BuffAmount;
        }
    }
    IEnumerator UseSkill(int index)
    {
        CanUseSkill[index] = false;
        yield return new WaitForSeconds(SkillCoolDown[index]);
        CanUseSkill[index] = true;
    }
}

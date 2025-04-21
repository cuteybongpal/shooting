using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, Atk
{
    public int Atk { get; set; }
    public CreatureController Owner { get; set; }
    public float BulletSpeed;
    public float DestroyTime;
    public bool IsSkill = false;


    public void SetVelocity(Vector2 dir, Quaternion quaternion)
    {
        transform.GetComponent<Rigidbody2D>().velocity = dir * BulletSpeed;
        transform.rotation = quaternion;
    }
    private void Start()
    {
        Destroy(gameObject, DestroyTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
            return;
        if (collision.CompareTag(Owner.tag))
            return;
        if (collision.CompareTag("Bullet"))
            return;
        if (IsSkill)
            return;
        transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(Boom());
    }
    IEnumerator Boom()
    {
        Animator anim = GetComponentInChildren<Animator>();
        anim.Play("Hit");

        yield return new WaitForSeconds(0.583f);
        Destroy(gameObject);
    }
}
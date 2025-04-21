using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    public GameObject GenerateBullet(Vector2 dir, int Atk, Quaternion q, Vector2 pos, CreatureController Owner, int BulletType = 0)
    {
        GameObject go;
        if (BulletType == 0)
            go = GameObject.Instantiate(GameManager.Resource.Prefab["Bullet"]);
        else if (BulletType == 1)
            go = GameObject.Instantiate(GameManager.Resource.Prefab["Bullet_Ziant"]);
        else if (BulletType == 2)
            go = GameObject.Instantiate(GameManager.Resource.Prefab["Bullet_Wave"]);
        else
            return null;
        go.transform.position = pos;
        Bullet b = go.GetComponent<Bullet>();
        b.Owner = Owner;
        b.Atk = Atk;
        b.SetVelocity(dir, q);
        return go;
    }
}

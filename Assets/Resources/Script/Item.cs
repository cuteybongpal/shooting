using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        DamageUp,
        PainDown,
        Heal
    }
    public ItemType type;
    public stage _stage;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.down;
    }
}

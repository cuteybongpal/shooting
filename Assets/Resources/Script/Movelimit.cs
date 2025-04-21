using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movelimit : MonoBehaviour
{
    
    void Update()
    {
        if (transform.position.x >= 2.3f)
        {
            transform.position = new Vector2(2.29f, transform.position.y);
        }
        else if (transform.position.x <= -2.3f)
        {
            transform.position = new Vector2(-2.29f, transform.position.y);
        }
        if (transform.position.y <= -4.5f)
        {
            transform.position = new Vector2(transform.position.x, -4.49f);
        }
    }
}

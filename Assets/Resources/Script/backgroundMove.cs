using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMove : MonoBehaviour
{
    
    void FixedUpdate()
    {
        transform.Translate(0, -1 * Time.deltaTime, 0);
        if (transform.position.y <= -10)
            gameObject.transform.position = new Vector3(0, 10, 0);
    }
}

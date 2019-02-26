using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    private float lifeTime = 20.0f;
    void Start()
    {
        if(tag == "Explosion")
        {
            Destroy(gameObject, 2.0f);
        }
        else
        {
            Destroy(gameObject, lifeTime);
        }
    }
}

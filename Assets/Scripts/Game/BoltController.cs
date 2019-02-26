using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour
{
    public float Speed = 4.0f;
    private float EndTime;
    void Start()
    {
        EndTime = Time.time + 5.0f;
        GetComponent<Rigidbody>().velocity = transform.forward * Speed;
    }
    void Update()
    {
        if(Time.time > EndTime)
        {
            Destroy(gameObject);
        }
    }
}

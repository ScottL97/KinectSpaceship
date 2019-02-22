using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotate : MonoBehaviour
{
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.transform.Rotate(new Vector3(0.0f, 2.0f, 0.0f));
        }
    }
}

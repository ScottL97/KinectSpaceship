using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float tumble = 0.1f;
    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = new Vector3(-0.05f, 0.0f, 0.0f) * tumble;
    }

}

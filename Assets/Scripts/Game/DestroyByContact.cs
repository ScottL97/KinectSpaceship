using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject PlayerExplosion;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerController>().ReduceHealth(10);
            Instantiate(Explosion, transform.position, transform.rotation);
            Instantiate(PlayerExplosion, transform.position, transform.rotation);
        }
        else if(other.tag == "Bolt")
        {
            Destroy(other.gameObject);
            Instantiate(Explosion, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}

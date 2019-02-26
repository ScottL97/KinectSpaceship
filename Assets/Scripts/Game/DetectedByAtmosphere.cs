using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectedByAtmosphere : MonoBehaviour
{

    public GameObject Explosion;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().SetCenterPoint(transform.position);
            other.gameObject.GetComponent<PlayerController>().SetCurrentPosition(tag);
        }
        else if(other.tag == "Asteroid")
        {
            Destroy(other.gameObject);
            Instantiate(Explosion, other.transform.position, other.transform.rotation);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().SetCurrentPosition("Space");
        }
    }
}

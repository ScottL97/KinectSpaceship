using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectedByAtmosphere : MonoBehaviour
{
    

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log(transform.position);
            other.gameObject.GetComponent<PlayerController>().SetCenterPoint(transform.position);
            other.gameObject.GetComponent<PlayerController>().SetCurrentPosition(tag);
        }
        else if(other.tag == "aerolite")
        {
            Destroy(other.gameObject);
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

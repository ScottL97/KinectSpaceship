using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game;

public class DetectedByAtmosphere : MonoBehaviour
{

    public GameObject Explosion;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().SetCenterPoint(transform.position);
            other.gameObject.GetComponent<PlayerController>().SetCurrentPosition(tag);
            other.gameObject.GetComponent<PlayerController>().AddPlanet(tag);
            //获取行星公转/自转周期
            other.gameObject.GetComponent<PlayerController>().ExplorePlanet(tag, PlanetsParameters.Revolution_period);
            other.gameObject.GetComponent<PlayerController>().ExplorePlanet(tag, PlanetsParameters.Rotation_period);
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
            other.gameObject.GetComponent<PlayerController>().ExplorePlanet(tag, PlanetsParameters.Escape_velocity);
            other.gameObject.GetComponent<PlayerController>().SetCurrentPosition("Space");
        }
    }
}

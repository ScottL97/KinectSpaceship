using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByPlanet : MonoBehaviour
{
    public GameObject playerExplosion;
    public GameObject GameController;
    private GameController _GameController;
    public GameObject MainCamera;
    public GameObject EndCamera;
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _GameController = GameController.GetComponent<GameController>();
            _GameController.GameOver();
            EndCamera.transform.position = MainCamera.transform.position;
            EndCamera.transform.rotation = MainCamera.transform.rotation;
            EndCamera.SetActive(true);
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
        }
        if(other.tag == "Bolt")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
        }
        Destroy(other.gameObject);
    }
}

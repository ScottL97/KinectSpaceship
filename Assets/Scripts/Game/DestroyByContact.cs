using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject PlayerExplosion;
    private GameObject VariablesRoom;
    private VariablesRoom _VariablesRoom;

    private void Start()
    {
        VariablesRoom = GameObject.Find("VariablesRoom");
        _VariablesRoom = VariablesRoom.GetComponent<VariablesRoom>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //飞船碰到陨石生命-10
            other.GetComponent<PlayerController>().ReduceHealth(10);
            Instantiate(Explosion, transform.position, transform.rotation);
            Instantiate(PlayerExplosion, transform.position, transform.rotation);
        }
        else if(other.tag == "Bolt")
        {
            //子弹碰到陨石摧毁数+1
            Destroy(other.gameObject);
            Instantiate(Explosion, transform.position, transform.rotation);
            _VariablesRoom.destroy += 1;
        }
        Destroy(gameObject);
    }
}

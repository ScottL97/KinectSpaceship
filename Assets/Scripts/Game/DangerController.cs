using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerController : MonoBehaviour
{
    private float spawnWait = 3.0f;
    private float waveWait = 5.0f;
    private int hazardCount = 5;
    private float times; //陨石随机放大倍数
    private Vector3 spawnPosition;
    public Transform PlayerShip;
    public GameObject Asteroid1;
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }
    IEnumerator SpawnWaves()
    {
        while(true)
        {
            for(int i = 0; i < hazardCount; i++)
            {
                try
                {
                    spawnPosition = new Vector3(PlayerShip.position.x + Random.Range(-10.0f, 10.0f), 
                        PlayerShip.position.y + Random.Range(-10.0f, 10.0f), PlayerShip.position.z + Random.Range(-10.0f, 10.0f));
                }
                catch(MissingReferenceException) //飞船对象不存在
                {
                    break;
                }
                times = Random.Range(1.0f, 3.0f);
                Asteroid1.GetComponent<Transform>().localScale = new Vector3(times, times, times);
                Instantiate(Asteroid1, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnWait); //等待3s生成下一个陨石
            }
            yield return new WaitForSeconds(waveWait); //波次之间等待5s，每波5个陨石
        }
    }
}

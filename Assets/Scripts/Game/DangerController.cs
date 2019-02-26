using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerController : MonoBehaviour
{
    private float spawnWait = 3.0f;
    private float waveWait = 5.0f;
    private int hazardCount = 5;
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
            Debug.Log("生成波次");
            for(int i = 0; i < hazardCount; i++)
            {
                Debug.Log("生成陨石");
                spawnPosition = new Vector3(PlayerShip.position.x + Random.Range(10.0f, 20.0f), PlayerShip.position.y + Random.Range(10.0f, 20.0f), PlayerShip.position.z);
                Instantiate(Asteroid1, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//存储需要在场景之间传递的变量
public class VariablesRoom : MonoBehaviour
{
    public int ShipId;
    
    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }
}

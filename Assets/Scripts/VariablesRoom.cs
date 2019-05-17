using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game;

//存储需要在场景之间传递的变量
public class VariablesRoom : MonoBehaviour
{
    public int ShipId;
    public int flag; //Start or Continue

    public float x;
    public float y;
    public float z;
    public int planet_num; //探索行星数
    public float period; //探索时间
    public int destroy; //摧毁数
    public Dictionary<string, Planet> Planets = new Dictionary<string, Planet>(); //探索过的行星

    public bool IfDebug = true;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        IfDebug = true;
    }

}

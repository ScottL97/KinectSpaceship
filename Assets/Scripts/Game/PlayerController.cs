using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject MouseController;
    public GameObject GameController;
    public Text ShipStatusText;
    public Text EnvironmentText;

    private MouseController _MouseController; //MouseController类
    private GameController _GameController;
    private Rigidbody rb;
    private int ShipMode;

    public Transform LeftSide;
    public Transform RightSide;
    public Transform FrontSide;
    public Transform BackSide;
    private Vector3 CenterPoint;
    private string CurrentPosition;
    private string Info;

    private float Speed;
    private float RotateSpeed;
    private float CurrentAngel; //当前飞船旋转角度
    private float LeanValue;
    private float Vx, Vy, Vz;
    private float TargetAx, TargetAy, TargetAz; //目标旋转角度
    private bool IfShipReady = false;

    public GameObject Bolt;
    private float FireRate = 1.0f;
    private float NextFire = 0.0f;

    private float EnergyValue;
    private float EnergySpeed; //能量补充/消耗速度
    private float Health;
    void Start()
    {
        _MouseController = MouseController.GetComponent<MouseController>();
        _GameController = GameController.GetComponent<GameController>();
        rb = GetComponent<Rigidbody>();
        Speed = 0;
        RotateSpeed = 1.0f;
        EnergyValue = 10000.0f;
        Health = 100;
        EnergySpeed = 0;
        CurrentPosition = "Space";
        Info = "";
        LeanValue = 0;
        Vx = 0;
        Vy = 0;
        Vz = 1;
        ShipMode = 2;
    }
    void FixedUpdate()
    {
        if(EnergyValue <= 0 || Health <= 0)
        {
            _GameController.GameOver();
        }
        if(ShipMode == 1)
        {
            if(IfShipReady)
            {
                //环绕星球飞行
                transform.RotateAround(CenterPoint, new Vector3(LeftSide.position.x - RightSide.position.x,
                    LeftSide.position.y - RightSide.position.y,
                    LeftSide.position.z - RightSide.position.z), 0.5f);
                EnergySpeed = 0.1f;
                Info = "绕行星飞行，补充能量中...";
                EnvironmentText.text = "Position: " + CurrentPosition + "\nInfo: " + Info + "\nMode: Auto Operation (Use left hand to switch manual mode)";
            }
            else
            {
                rb.velocity = Vector3.zero;
                Speed = 0;
                if (Mathf.Ceil(Vector3.Dot(RightSide.position - LeftSide.position, transform.position - CenterPoint)) == 0)
                {
                    if (Mathf.Ceil(Vector3.Dot(FrontSide.position - BackSide.position, transform.position - CenterPoint)) == 0)
                    {
                        IfShipReady = true;
                    }
                    else
                    {
                        transform.Rotate(0.5f, 0, 0);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, 0.5f);
                }
                Info = "即将绕行星飞行，飞船正在准备中...";
                EnvironmentText.text = "Position: " + CurrentPosition + "\nInfo: " + Info + "\nMode: Changing Mode To Auto...";
            }
        }
        else if(ShipMode == 2)
        {
            IfShipReady = false;
            if(CurrentPosition != "Space")
            {
                Info = "Be careful to avoid collision!";
                EnvironmentText.text = "Position: " + CurrentPosition + "\nInfo: " + Info + "\nMode: Manual Operation (Use left hand to switch auto mode)";
            }
            else
            {
                Info = "";
                EnvironmentText.text = "Position: " + CurrentPosition + "\nInfo: " + Info + "\nMode: Manual Operation";
            }
            
            if (_MouseController.IfReady)
            {
                LeanValue = _MouseController._Bodies[_MouseController.CurrentId].Lean.X;
                Speed = Mathf.Abs(_MouseController._Bodies[_MouseController.CurrentId].Lean.Y * 10.0f);
                EnergySpeed = Speed * -0.1f;
                if (LeanValue < -0.2)
                {
                    transform.Rotate(0.0f, -RotateSpeed, 0.0f); //参数为角度
                }
                else if (LeanValue > 0.2)
                {
                    transform.Rotate(0.0f, RotateSpeed, 0.0f);
                }
                CurrentAngel = transform.localEulerAngles.y;
                Vx = Mathf.Sin(Mathf.Deg2Rad * CurrentAngel) * Speed;
                Vy = _MouseController.RightY > 0 ? Speed : -Speed;
                Vz = Mathf.Cos(Mathf.Deg2Rad * CurrentAngel) * Speed;
                rb.velocity = new Vector3(Vx, Vy, Vz);
            }
        }
        else //test
        {
            //transform.Rotate(0.0f, Mathf.PI * 0.5f, 0.0f);
            //rb.velocity = new Vector3(Vx, Vy, Vz);
            //Debug.Log(Mathf.Atan(0));
            //transform.rotation = Quaternion.Slerp(transform.rotation, TargetAngel, RotateSpeed * Time.deltaTime);
            //LatestSpeed = rb.velocity;
        }
        EnergyValue += EnergySpeed;
        
        ShipStatusText.text = "Speed：" + Speed + "\nHealth: " + Health + " %\nEnergy: " + EnergyValue + "\nDanger: No\n" +
            "Vx: " + rb.velocity.x + "\nVy: " + rb.velocity.y + "\nVz: " + rb.velocity.z;
    }
    public void Launch()
    {
        if(Time.time > NextFire)
        {
            NextFire = Time.time + FireRate;
            Instantiate(Bolt, transform.position, transform.rotation);
        }
    }
    public void ReduceHealth(float value)
    {
        Health -= value;
    }
    public void SetMode(int mode)
    {
        ShipMode = mode;
    }
    public int GetMode()
    {
        return ShipMode;
    }
    public void SetCenterPoint(Vector3 point)
    {
        CenterPoint = point;
    }
    public void SetCurrentPosition(string pos)
    {
        CurrentPosition = pos;
    }
    public string GetCurrentPosition()
    {
        return CurrentPosition;
    }
}

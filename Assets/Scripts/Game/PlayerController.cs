using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject MouseController;
    public Text ShipStatusText;
    public Text EnvironmentText;

    private MouseController _MouseController; //MouseController类
    private Rigidbody rb;
    private int ShipMode;

    private Vector3 CenterPoint;
    private string CurrentPosition;
    private string Info;

    private float Speed;
    private float RotateSpeed;
    private float CurrentAngel; //当前飞船旋转角度
    private float LeanValue;
    private float Vx, Vy, Vz;

    public GameObject Bolt;
    private float FireRate = 1.0f;
    private float NextFire = 0.0f;

    private float EnergyValue;
    private float EnergySpeed; //能量消耗速度
    void Start()
    {
        _MouseController = MouseController.GetComponent<MouseController>();
        rb = GetComponent<Rigidbody>();
        Speed = 0;
        RotateSpeed = 1.0f;
        EnergyValue = 10000.0f;
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
        if(ShipMode == 1)
        {
            //环绕星球飞行
            transform.RotateAround(CenterPoint, new Vector3(1.0f, 0.0f, 0.0f), 0.5f);
            EnergySpeed = 0.1f;
            Info = "绕行星飞行，补充能量中...";
            EnvironmentText.text = "Position: " + CurrentPosition + "\nInfo: " + Info + "\nMode: Auto Operation (Use left hand to switch manual mode)";
        }
        else if(ShipMode == 2)
        {
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
                Speed = Mathf.Abs(_MouseController._Bodies[_MouseController.CurrentId].Lean.Y * 3.0f);
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
        
        ShipStatusText.text = "Speed：" + Speed + "\nHealth: 100 %\nEnergy: " + EnergyValue + "\nDanger: No\n" +
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

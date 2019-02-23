using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject MouseController;
    public Text ShipStatusText;

    private MouseController _MouseController; //MouseController类
    private Rigidbody rb;
    private int ShipMode;
    private float Speed;
    private float LeanValue;
    private float Vy;
    private Vector3 CenterPoint;
    void Start()
    {
        _MouseController = MouseController.GetComponent<MouseController>();
        rb = GetComponent<Rigidbody>();
        Speed = 0;
        ShipMode = 3;
    }
    void FixedUpdate()
    {
        if(ShipMode == 1)
        {
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            rb.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            //环绕星球飞行
            transform.RotateAround(new Vector3(0.0f, -15.0f, 10.0f), new Vector3(1.0f, 0.0f, 0.0f), 0.5f);
        }
        else if(ShipMode == 2)
        {
            LeanValue = _MouseController._Bodies[_MouseController.CurrentId].Lean.X;
            Speed = _MouseController._Bodies[_MouseController.CurrentId].Lean.Y * 3.0f;
            if(LeanValue < -0.2)
            {
                if(Mathf.Abs(transform.rotation.y) < 90)
                {
                    CenterPoint = new Vector3(transform.position.x - 50.0f, transform.position.y, transform.position.z - 50.0f);
                }
                else
                {
                    CenterPoint = new Vector3(transform.position.x + 50.0f, transform.position.y, transform.position.z - 50.0f);
                }
                transform.RotateAround(CenterPoint, new Vector3(0.0f, 1.0f, 0.0f), 0.1f);
            }
            else if(LeanValue > 0.2)
            {
                if(Mathf.Abs(transform.rotation.y) < 90)
                {
                    CenterPoint = new Vector3(transform.position.x + 20.0f, transform.position.y, transform.position.z + 20.0f);
                }
                else
                {
                    CenterPoint = new Vector3(transform.position.x - 20.0f, transform.position.y, transform.position.z + 20.0f);
                }
                transform.RotateAround(CenterPoint, new Vector3(0.0f, 1.0f, 0.0f), 0.5f);
            }
            else
            {
                Vy = _MouseController.Y > 0 ? Speed : -Speed;
                //if(rb.rotation)
                rb.velocity = new Vector3(Speed * Mathf.Sin(transform.rotation.y), Vy, Speed * Mathf.Cos(transform.rotation.y));
            }
        }
        else //test
        {
            transform.RotateAround(new Vector3(-10.0f, 0.0f, -10.0f), new Vector3(0.0f, 1.0f, 0.0f), 0.1f);
        }
        ShipStatusText.text = "Position：Planet1\nSpeed：" + Speed + "\nStatus: Orbiting round the Planet1\nHealth: 100 %\nEnergy: 100 %\nDanger: No\n" +
            "Vx: " + rb.velocity.x + "\nVy: " + rb.velocity.y + "\nVz: " + rb.velocity.z;
    }
}

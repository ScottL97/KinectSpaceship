using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject MouseController;
    public GameObject VariablesRoom; //VariablesRoom游戏对象，存储需要在场景切换时传递的数据
    public GameObject PlayerShip;
    public Image Mouse;

    private MouseController _MouseController; //MouseController类
    private VariablesRoom _VariablesRoom; //VariablesRoom类

    private Rigidbody rb;
    void Start()
    {
        _VariablesRoom = VariablesRoom.GetComponent<VariablesRoom>();
        _MouseController = MouseController.GetComponent<MouseController>();
        rb = PlayerShip.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (_MouseController.IfReady)
        {
            Mouse.rectTransform.localPosition = _MouseController.GetMousePosition();
            //按钮点击事件
            if (_MouseController.ClickState == 1)
            {
                if (_MouseController.Y > 465 && _MouseController.Y < 530)
                {
                    if (_MouseController.X > -950 && _MouseController.X < -770)
                    {
                        //返回主菜单按钮点击事件
                        Debug.Log("Menu");
                        SceneManager.LoadScene("Menu");
                    }
                }
            }
            //飞船Z轴左右倾斜
            
        }   
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject Ship1;
    public GameObject Ship2;
    public GameObject MouseController;
    public GameObject VariablesRoom; //VariablesRoom游戏对象，存储需要在场景切换时传递的数据

    public Text TrackedCountText;
    public Image Mouse;
    public Button StartButton;
    public Button ContinueButton;
    public Button ExitButton;

    private MouseController _MouseController; //MouseController类
    private VariablesRoom _VariablesRoom; //VariablesRoom类

    private float EndTime;
    void Start()
    {
        _VariablesRoom = VariablesRoom.GetComponent<VariablesRoom>();
        _VariablesRoom.ShipId = 1;
        _MouseController = MouseController.GetComponent<MouseController>();
    }
    void FixedUpdate()
    {
        if(_MouseController.IfReady)
        {
            Mouse.rectTransform.localPosition = _MouseController.GetMousePosition();
            TrackedCountText.text = "玩家个数：" + _MouseController.GetPlayerNumbers();
            //按钮点击事件
            //如果飞船在运动中，不重复进行飞船切换动画，且暂时不能进入游戏，故进行时间限制
            if (Time.time > EndTime)
            {
                //超过时间后停止飞船切换动画
                Ship1.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                Ship2.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                if (_MouseController.ClickState == 1)
                {
                    //切换飞船按钮点击事件
                    if (_MouseController.Y > -45 && _MouseController.Y < 45)
                    {
                        if ((_MouseController.X > -760 && _MouseController.X < -660)
                        || (_MouseController.X > 660 && _MouseController.X < 760))
                        {
                            int targetId = _VariablesRoom.ShipId - 1 == 1 ? 1 : 2;
                            ChangeShip(_VariablesRoom.ShipId, targetId);
                            _VariablesRoom.ShipId = targetId;
                        }
                    }
                    else if (_MouseController.Y > -330 && _MouseController.Y < -270)
                    {
                        if (_MouseController.X > -500 && _MouseController.X < -200)
                        {
                            //开始按钮点击事件
                            Debug.Log("Start");
                            SceneManager.LoadScene("Loading");
                        }
                        else if (_MouseController.X > -150 && _MouseController.X < 150)
                        {
                            //继续游戏按钮点击事件
                            Debug.Log("Continue");
                            SceneManager.LoadScene("Loading");
                        }
                        else if (_MouseController.X > 200 && _MouseController.X < 500)
                        {
                            //退出游戏按钮点击事件
                            Debug.Log("Exit");
                            Application.Quit();
                        }
                    }
                }
            }
        }
    }
    void ChangeShip(int currentId, int targetId)
    {
        Debug.Log(currentId + " to " + targetId);
        EndTime = Time.time + 3.0f;
        switch(currentId)
        {
            case 1:
                {
                    Ship1.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -2.0f, 0.0f);
                    switch (targetId)
                    {
                        case 2: Ship2.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 2.0f, 0.0f);break;
                        default:break;
                    }
                } break;
            case 2:
                {
                    Ship2.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -2.0f, 0.0f);
                    switch (targetId)
                    {
                        case 1: Ship1.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 2.0f, 0.0f); break;
                        default: break;
                    }
                }
                break;
            default:break;
        }
    }
}

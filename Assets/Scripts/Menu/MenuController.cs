using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Xml;
using System.IO;

public class MenuController : MonoBehaviour
{
    public GameObject Ship1;
    public GameObject Ship2;
    public GameObject MouseController;
    public GameObject VariablesRoom; //VariablesRoom游戏对象，存储需要在场景切换时传递的数据
    public GameObject RecordCanvas;

    public Text TrackedCountText;
    public Text RecordText;
    public Image Mouse;

    private MouseController _MouseController; //MouseController类
    private VariablesRoom _VariablesRoom; //VariablesRoom类

    private bool IfCanContinue; //是否有存档
    private bool IfShowRecord; //是否显示记录

    private float EndTime;
    void Start()
    {
        VariablesRoom = GameObject.Find("VariablesRoom");
        _VariablesRoom = VariablesRoom.GetComponent<VariablesRoom>();
        _VariablesRoom.ShipId = 1;
        MouseController = GameObject.Find("MouseController");
        _MouseController = MouseController.GetComponent<MouseController>();
        if (_VariablesRoom.IfDebug)
        {
            Debug.Log("Debug");
        }
        //检查是否存在存档文件
        string path = Application.streamingAssetsPath + "/Record.xml";
        if (!File.Exists(path))
        {
            IfCanContinue = false;
        }
        else
        {
            IfCanContinue = true;
        }
        RecordCanvas.SetActive(false);
        IfShowRecord = false;
        EndTime = 0.0f; //飞船切换动画结束时间变量初始化
    }
    void FixedUpdate()
    {
        //Debug过程用键盘移动光标
        if(_VariablesRoom.IfDebug)
        {
            if (Input.GetKey(KeyCode.A))
            {
                Mouse.rectTransform.localPosition += new Vector3(-10, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Mouse.rectTransform.localPosition += new Vector3(10, 0, 0);
            }
            if (Input.GetKey(KeyCode.W))
            {
                Mouse.rectTransform.localPosition += new Vector3(0, 10, 0);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Mouse.rectTransform.localPosition += new Vector3(0, -10, 0);
            }
            if (_MouseController.GetHandState("RightHand") == 1)
            {
                switch (_MouseController.ButtonNum)
                {
                    //开始按钮点击事件
                    case 1:
                        {
                            _VariablesRoom.flag = 1;
                            SceneManager.LoadScene("Loading");
                        }
                        break;
                    //继续按钮点击事件
                    case 2:
                        {
                            if (IfCanContinue)
                            {
                                _VariablesRoom.flag = 2;
                                SceneManager.LoadScene("Loading");
                            }
                        }
                        break;
                    //设置按钮点击事件
                    case 3:
                        {

                        }
                        break;
                    //记录按钮点击事件
                    case 4:
                        {
                            if(!IfShowRecord)
                            {
                                IfShowRecord = true;
                                //读取XML存档
                                GetRecord();
                                RecordCanvas.SetActive(true);
                            }
                        }
                        break;
                    //记录关闭按钮点击事件
                    case 5:
                        {
                            if(IfShowRecord)
                            {
                                IfShowRecord = false;
                                RecordCanvas.SetActive(false);
                            }
                        }
                        break;
                    //退出按钮点击事件
                    case 6:
                        {
                            Debug.Log("Exit");
                            Application.Quit();
                        }
                        break;
                    default: break;
                }
            }
        }
        if (_MouseController.IfReady)
        {
            Mouse.rectTransform.localPosition = _MouseController.GetMousePosition("RightHand");
            TrackedCountText.text = "玩家个数：" + _MouseController.GetPlayerNumbers();
            //按钮点击事件
            //如果飞船在运动中，不重复进行飞船切换动画，且暂时不能进入游戏，故进行时间限制
            if (Time.time > EndTime)
            {
                //超过时间后停止飞船切换动画
                //Ship1.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                //Ship2.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                if (_MouseController.GetHandState("RightHand") == 1)
                {
                    switch(_MouseController.ButtonNum)
                    {
                        //开始按钮点击事件
                        case 1:
                            {
                                _VariablesRoom.flag = 1;
                                SceneManager.LoadScene("Loading");
                            }
                            break;
                        //继续按钮点击事件
                        case 2:
                            {
                                if (IfCanContinue)
                                {
                                    _VariablesRoom.flag = 2;
                                    SceneManager.LoadScene("Loading");
                                }
                            }
                            break;
                        //设置按钮点击事件
                        case 3:
                            {

                            }
                            break;
                        //记录按钮点击事件
                        case 4:
                            {
                                if (!IfShowRecord)
                                {
                                    IfShowRecord = true;
                                    //读取XML存档
                                    GetRecord();
                                    RecordCanvas.SetActive(true);
                                }
                            }
                            break;
                        //记录关闭按钮点击事件
                        case 5:
                            {
                                if (IfShowRecord)
                                {
                                    IfShowRecord = false;
                                    RecordCanvas.SetActive(false);
                                }
                            }
                            break;
                        //退出按钮点击事件
                        case 6:
                            {
                                Debug.Log("Exit");
                                Application.Quit();
                            }
                            break;
                        default: break;
                    }
                }
            }
        }
    }
    /*void ChangeShip(int currentId, int targetId)
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
    }*/
    void GetRecord()
    {
        //创建xml文档对象
        XmlDocument xml = new XmlDocument();
        xml.Load(Application.streamingAssetsPath + "/Record.xml");
        //得到Ships节点下的所有子节点
        XmlNodeList xmlNodeList = xml.SelectSingleNode("Ships").ChildNodes;
        RecordText.text = "存档数量：" + xmlNodeList.Count + "\n----------------------------------------------------------------";
        //遍历所有子节点，读取数据
        foreach (XmlElement ship in xmlNodeList)
        {
            RecordText.text += "\n飞船id：" + ship.GetAttribute("id");
            XmlElement scoreElement = (XmlElement)ship.SelectSingleNode("Score");
            float Score = int.Parse(scoreElement.SelectSingleNode("Planet_num").InnerText) * 100 +
                float.Parse(scoreElement.SelectSingleNode("Period").InnerText) + int.Parse(scoreElement.SelectSingleNode("Destroy").InnerText) * 10;
            RecordText.text += "\n探索行星数：" + scoreElement.SelectSingleNode("Planet_num").InnerText + "\n探索周期：" + 
                scoreElement.SelectSingleNode("Period").InnerText + "\n摧毁目标数：" + scoreElement.SelectSingleNode("Destroy").InnerText
                + "\n总分：" + Score + "\n**************************************************************************";
            XmlElement planetsList = (XmlElement)ship.SelectSingleNode("Planets");
            foreach (XmlElement planetElement in planetsList)
            {
                RecordText.text += "\n行星名称：" + planetElement.GetAttribute("name") + "\n赤道半径：" + planetElement.SelectSingleNode("Radius").InnerText
                    + "\n逃逸速度：" + planetElement.SelectSingleNode("Escape_velocity").InnerText + "\n质量：" + planetElement.SelectSingleNode("Mass").InnerText
                    + "\n密度：" + planetElement.SelectSingleNode("Density").InnerText + "\n公转周期：" + planetElement.SelectSingleNode("Revolution_period").InnerText 
                    + "\n自转周期：" + planetElement.SelectSingleNode("Rotation_period").InnerText + "\n**************************************************************************";
            }
        }
    }
}

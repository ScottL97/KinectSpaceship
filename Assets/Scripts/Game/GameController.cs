using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Xml;
using Assets.Scripts.Game;

public class GameController : MonoBehaviour
{
    private GameObject MouseController;
    public GameObject PlayerShip;
    private GameObject VariablesRoom; //VariablesRoom游戏对象，存储需要在场景切换时传递的数据
    public GameObject EndCamera; //游戏失败第三人称视角的摄像机
    public GameObject MiniMapCamera; //小地图摄像机
    public GameObject StarField;
    public GameObject RecordCanvas;
    public Image Mouse;
    public Text GameOverText;
    public Text RecordText;

    private MouseController _MouseController; //MouseController类
    private PlayerController _PlayerController;
    private VariablesRoom _VariablesRoom; //VariablesRoom类

    private bool IfShowRecord; //是否显示记录
    private int HasExplored; //已探索行星数

    private float LatestSpawnStarsTime = 0.0f;
    private bool IfGameOver = false;

    void Start()
    {
        EndCamera.SetActive(false);
        VariablesRoom = GameObject.Find("VariablesRoom");
        MouseController = GameObject.Find("MouseController");
        _VariablesRoom = VariablesRoom.GetComponent<VariablesRoom>();
        _MouseController = MouseController.GetComponent<MouseController>();
        _PlayerController = PlayerShip.GetComponent<PlayerController>();
        RecordCanvas.SetActive(false);
        IfShowRecord = false;
    }
    void FixedUpdate()
    {
        //Debug过程用键盘控制飞船
        if (_VariablesRoom.IfDebug)
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
            if (Input.GetKey(KeyCode.J))
            {
                _PlayerController.Launch();
            }
            if (_MouseController.GetHandState("RightHand") == 1)
            {
                switch(_MouseController.ButtonNum)
                {
                    //记录按钮点击事件
                    case 4:
                        {
                            if (!IfShowRecord)
                            {
                                //获取记录数据
                                foreach (KeyValuePair<string, Planet> planet in _VariablesRoom.Planets)
                                {
                                    RecordText.text = planet.Value.GetName() + "\n赤道半径：" + planet.Value.GetRadius() + "\n逃逸速度：" + planet.Value.GetEscapeVelocity() +
                                        "\n质量：" + planet.Value.GetMass() + "\n密度：" + planet.Value.GetDensity() + "\n公转周期：" + planet.Value.GetRevolutionPeriod() +
                                        "\n自转周期：" + planet.Value.GetRotationPeriod() + "\n------------------------------------------------------------------------------";
                                }
                                IfShowRecord = true;
                                RecordCanvas.SetActive(true);
                            }
                        } break;
                    //记录关闭按钮点击事件
                    case 5:
                        {
                            if (IfShowRecord)
                            {
                                IfShowRecord = false;
                                RecordCanvas.SetActive(false);
                            }
                        } break;
                    //返回主菜单按钮点击事件
                    case 7:
                        {
                            UpdateXML(_VariablesRoom.ShipId); //保存分数
                            SceneManager.LoadScene("Menu");
                        } break;
                    default: break;
                }  
            }
        }
        if (_MouseController.IfReady)
        {
            Mouse.rectTransform.localPosition = _MouseController.GetMousePosition("RightHand");
            //按钮点击事件
            if (_MouseController.GetHandState("RightHand") == 1)
            {
                switch (_MouseController.ButtonNum)
                {
                    //记录按钮点击事件
                    case 4:
                        {
                            if (!IfShowRecord)
                            {
                                //获取记录数据
                                foreach (KeyValuePair<string, Planet> planet in _VariablesRoom.Planets)
                                {
                                    RecordText.text = planet.Value.GetName() + "\n赤道半径：" + planet.Value.GetRadius() + "\n逃逸速度：" + planet.Value.GetEscapeVelocity() +
                                        "\n质量：" + planet.Value.GetMass() + "\n密度：" + planet.Value.GetDensity() + "\n公转周期：" + planet.Value.GetRevolutionPeriod() +
                                        "\n自转周期：" + planet.Value.GetRotationPeriod() + "\n------------------------------------------------------------------------------";
                                }
                                IfShowRecord = true;
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
                    //返回主菜单按钮点击事件
                    case 7:
                        {
                            UpdateXML(_VariablesRoom.ShipId); //保存分数
                            SceneManager.LoadScene("Menu");
                        }
                        break;
                    default: break;
                }
            }
            if (PlayerShip != null)
            {
                MiniMapCamera.transform.position = new Vector3(PlayerShip.transform.position.x, PlayerShip.transform.position.y + 5, PlayerShip.transform.position.z);
            }
            if(IfGameOver)
            {
                if (_MouseController.ModeSwitch)
                {
                    IfGameOver = false;
                    GameOverText.text = "";
                    SceneManager.LoadScene("Loading");
                }
            }
            else
            {
                //生成粒子特效
                if (Time.time > LatestSpawnStarsTime + 20.0f)
                {
                    LatestSpawnStarsTime = Time.time;
                    Instantiate(StarField, new Vector3(PlayerShip.transform.position.x, PlayerShip.transform.position.y + 1, PlayerShip.transform.position.z + 16),
                        Quaternion.identity);
                }
                //手部握拳点击事件
                if (_MouseController.GetHandState("RightHand") == 1)
                {
                    //发射导弹
                    _PlayerController.Launch();
                }
                if (_PlayerController.GetCurrentPosition() != "Space")
                {
                    if (_MouseController.ModeSwitch)
                    {
                        _PlayerController.SetMode(1);
                    }
                    else
                    {
                        _PlayerController.SetMode(2);
                    }
                }
            }
        }   
    }
    public void GameOver()
    {
        IfGameOver = true;
        _MouseController.ModeSwitch = false;
        GameOverText.text = "Game Over! 左手握拳重新开始游戏！";
        _PlayerController.SetMode(0); //飞船停止修改数据（如分数不再增加）
        UpdateXML(_VariablesRoom.ShipId); //保存分数
    }
    public void UpdateXML(int id)
    {
        Debug.Log("修改分数：" + _PlayerController.Score);
        string path = Application.streamingAssetsPath + "/Record.xml";
        //创建xml文档对象
        XmlDocument xml = new XmlDocument();
        xml.Load(path);
        //得到Ships节点下的所有子节点
        XmlNodeList xmlNodeList = xml.SelectSingleNode("Ships").ChildNodes;
        //遍历所有子节点，找到id相同的一个，清空分数与探索记录
        foreach (XmlElement ship in xmlNodeList)
        {
            if (int.Parse(ship.GetAttribute("id")) == id)
            {
                XmlElement positionElement = (XmlElement)ship.SelectSingleNode("Position");
                XmlNodeList positionList = positionElement.ChildNodes;
                positionElement.SelectSingleNode("X").InnerText = _PlayerController.transform.position.x.ToString();
                positionElement.SelectSingleNode("Y").InnerText = _PlayerController.transform.position.y.ToString();
                positionElement.SelectSingleNode("Z").InnerText = _PlayerController.transform.position.z.ToString();
                XmlElement scoreElement = (XmlElement)ship.SelectSingleNode("Score");
                XmlNodeList scoreList = scoreElement.ChildNodes;
                scoreElement.SelectSingleNode("Planet_num").InnerText = _VariablesRoom.planet_num.ToString();
                scoreElement.SelectSingleNode("Period").InnerText = _VariablesRoom.period.ToString();
                scoreElement.SelectSingleNode("Destroy").InnerText = _VariablesRoom.destroy.ToString();
                //将Planets字典中的数据存档
                XmlElement planetsList = (XmlElement)ship.SelectSingleNode("Planets");
                planetsList.InnerText = "";
                foreach (KeyValuePair<string, Planet> planet in _VariablesRoom.Planets)
                {
                    XmlElement planetElement = xml.CreateElement("Planet");
                    XmlElement radiusElement = xml.CreateElement("Radius");
                    radiusElement.InnerText = planet.Value.GetRadius().ToString();
                    XmlElement escapeElement = xml.CreateElement("Escape_velocity");
                    escapeElement.InnerText = planet.Value.GetEscapeVelocity().ToString();
                    XmlElement massElement = xml.CreateElement("Mass");
                    massElement.InnerText = planet.Value.GetMass().ToString();
                    XmlElement densityElement = xml.CreateElement("Density");
                    densityElement.InnerText = planet.Value.GetDensity().ToString();
                    XmlElement revolutionElement = xml.CreateElement("Revolution_period");
                    revolutionElement.InnerText = planet.Value.GetRevolutionPeriod().ToString();
                    XmlElement rotationElement = xml.CreateElement("Rotation_period");
                    rotationElement.InnerText = planet.Value.GetRotationPeriod().ToString();

                    planetElement.SetAttribute("name", planet.Value.GetName());
                    planetElement.AppendChild(radiusElement);
                    planetElement.AppendChild(escapeElement);
                    planetElement.AppendChild(massElement);
                    planetElement.AppendChild(densityElement);
                    planetElement.AppendChild(revolutionElement);
                    planetElement.AppendChild(rotationElement);

                    planetsList.AppendChild(planetElement);
                }
                xml.Save(path);
            }
        }
    }
}

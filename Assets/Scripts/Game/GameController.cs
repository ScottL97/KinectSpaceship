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
    public GameObject StatusCanvas;

    public Image Mouse;
    public Text GameOverText;
    public Text RecordText;
    public RawImage Weapon;
    public Text WeaponName;

    private MouseController _MouseController; //MouseController类
    private PlayerController _PlayerController;
    private VariablesRoom _VariablesRoom; //VariablesRoom类

    private bool IfShowRecord; //是否显示记录
    private bool IfShowStatus; //是否显示状态
    private int HasExplored; //已探索行星数
    private int CurrentWeapon; //当前武器

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
        StatusCanvas.SetActive(false);
        IfShowStatus = false;
        IfShowRecord = false;
        CurrentWeapon = 1;
    }
    void FixedUpdate()
    {
        //Debug过程用键盘控制
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
                switch(CurrentWeapon)
                {
                    //导弹发射
                    case 1:
                        {
                            _PlayerController.Launch();
                        } break;
                    //超声波发射
                    case 2:
                        {
                            AudioSource sound = gameObject.AddComponent<AudioSource>();
                            sound.clip = (AudioClip)Resources.Load("Sounds/explosion_enemy");
                            sound.Play();
                            _PlayerController.ExplorePlanet(_PlayerController.GetCurrentPosition(), PlanetsParameters.Radius);
                        } break;
                }
                
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
                    //状态开/关按钮点击事件
                    case 8:
                        {
                            if (IfShowStatus)
                            {
                                IfShowStatus = false;
                                StatusCanvas.SetActive(false);
                            }
                            else
                            {
                                IfShowStatus = true;
                                StatusCanvas.SetActive(true);
                            }
                        } break;
                    case 9:
                        {
                            CurrentWeapon++;
                            //循环显示所有武器和工具
                            if(CurrentWeapon == 3)
                            {
                                CurrentWeapon = 1;
                            }
                            Debug.Log("武器：" + CurrentWeapon);
                            switch (CurrentWeapon)
                            {
                                //导弹
                                case 1:
                                    {
                                        Weapon.texture = (Texture)Resources.Load("Textures/weapon");
                                        WeaponName.text = "导弹（右手握拳发射）";
                                    } break;
                                //超声波
                                case 2:
                                    {
                                        Weapon.texture = (Texture)Resources.Load("Textures/radar");
                                        WeaponName.text = "超声波（右手握拳发射）";
                                    } break;
                                default: break;
                            }
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
                    //状态开/关按钮点击事件
                    case 8:
                        {
                            if(IfShowStatus)
                            {
                                IfShowStatus = false;
                                StatusCanvas.SetActive(false);
                            }
                            else
                            {
                                IfShowStatus = true;
                                StatusCanvas.SetActive(true);
                            }
                        } break;
                    //切换武器
                    case 9:
                        {
                            CurrentWeapon++;
                            //循环显示所有武器和工具
                            if (CurrentWeapon == 3)
                            {
                                CurrentWeapon = 1;
                            }
                            switch (CurrentWeapon)
                            {
                                //导弹
                                case 1:
                                    {
                                        Weapon.texture = (Texture)Resources.Load("Textures/weapon");
                                        WeaponName.text = "导弹（右手握拳发射）";
                                    }
                                    break;
                                //超声波
                                case 2:
                                    {
                                        Weapon.texture = (Texture)Resources.Load("Textures/radar");
                                        WeaponName.text = "超声波（右手握拳发射）";
                                    }
                                    break;
                                default: break;
                            }
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
                    SceneManager.LoadScene("Menu");
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
                if (_MouseController.GetHandState("RightHand") == 1 && _MouseController.ButtonNum == 0)
                {
                    switch (CurrentWeapon)
                    {
                        //导弹发射
                        case 1:
                            {
                                _PlayerController.Launch();
                            }
                            break;
                        //超声波发射
                        case 2:
                            {
                                _PlayerController.ExplorePlanet(_PlayerController.GetCurrentPosition(), PlanetsParameters.Radius);
                            }
                            break;
                    }
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
        GameOverText.text = "Game Over! \n左手握拳返回主菜单，可以从上次存档处继续游戏";
        _PlayerController.SetMode(0); //飞船停止修改数据（如分数不再增加）
        //UpdateXML(_VariablesRoom.ShipId); //保存分数
        //不保存分数，玩家可以从上次存档继续
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
                if(_PlayerController != null)
                {
                    positionElement.SelectSingleNode("X").InnerText = _PlayerController.transform.position.x.ToString();
                    positionElement.SelectSingleNode("Y").InnerText = _PlayerController.transform.position.y.ToString();
                    positionElement.SelectSingleNode("Z").InnerText = _PlayerController.transform.position.z.ToString();
                }
                else
                {
                    positionElement.SelectSingleNode("X").InnerText = EndCamera.transform.position.x.ToString();
                    positionElement.SelectSingleNode("Y").InnerText = EndCamera.transform.position.y.ToString();
                    positionElement.SelectSingleNode("Z").InnerText = EndCamera.transform.position.z.ToString();
                }
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

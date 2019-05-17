using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using Assets.Scripts.Game;

public class PlayerController : MonoBehaviour
{
    private GameObject MouseController;
    public GameObject GameController;
    private GameObject VariablesRoom;
    public Text ShipStatusText;
    public Text EnvironmentText;
    public Slider DirSlider;

    private MouseController _MouseController; //MouseController类
    private GameController _GameController;
    private VariablesRoom _VariablesRoom;
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
    public GameObject BoltPosition;
    private float FireRate = 1.0f;
    private float NextFire = 0.0f;

    private float EnergyValue;
    private float EnergySpeed; //能量补充/消耗速度
    private float Health;
    public float Score; //当前总分
    void Start()
    {
        VariablesRoom = GameObject.Find("VariablesRoom");
        MouseController = GameObject.Find("MouseController");
        _MouseController = MouseController.GetComponent<MouseController>();
        _GameController = GameController.GetComponent<GameController>();
        _VariablesRoom = VariablesRoom.GetComponent<VariablesRoom>();
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
        Score = _VariablesRoom.planet_num * 100 + _VariablesRoom.period + _VariablesRoom.destroy * 10;
        transform.position = new Vector3(_VariablesRoom.x, _VariablesRoom.y, _VariablesRoom.z);
        DirSlider.value = 0.5f;
        ShipStatusText.text = "Speed：" + Speed + "\nHealth: " + Health + " %\nEnergy: " + EnergyValue + "\nDanger: No\n" +
            "Vx: " + rb.velocity.x + "\nVy: " + rb.velocity.y + "\nVz: " + rb.velocity.z + "\nScore: " + (int)Score;
        //test
        //CurrentPosition = "Planet1";
        //AddPlanet("Planet1");
        //ExplorePlanet("Planet1", PlanetsParameters.Radius);
    }
    void FixedUpdate()
    {
        if(EnergyValue <= 0 || Health <= 0)
        {
            _GameController.GameOver();
        }
        //环绕行星飞行模式
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
                EnergyValue += EnergySpeed;
                //使用超声波工具检测行星赤道半径
                ExplorePlanet("Planet1", PlanetsParameters.Radius);
                //使用测速仪根据公式计算行星质量、密度

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
        //自由飞行模式
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
            //Kinect加载完成
            if (_MouseController.IfReady)
            {
                LeanValue = _MouseController._Bodies[_MouseController.CurrentId].Lean.X;
                DirSlider.value = (LeanValue + 1) / 2;
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
                //_VariablesRoom.period += 0.01f;
            }
            _VariablesRoom.period += 0.01f;
            EnergyValue += EnergySpeed;
        }
        //实时计算分数
        Score = _VariablesRoom.planet_num * 100 + _VariablesRoom.period + _VariablesRoom.destroy * 10;
        ShipStatusText.text = "Speed：" + Speed + "\nHealth: " + Health + " %\nEnergy: " + EnergyValue + "\nDanger: No\n" +
            "Vx: " + rb.velocity.x + "\nVy: " + rb.velocity.y + "\nVz: " + rb.velocity.z + "\nScore: " + (int)Score;
    }
    public void Launch()
    {
        if(Time.time > NextFire)
        {
            NextFire = Time.time + FireRate;
            Instantiate(Bolt, BoltPosition.transform.position, BoltPosition.transform.rotation);
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
    //添加探索到的行星
    public void AddPlanet(string tag)
    {
        if(!_VariablesRoom.Planets.ContainsKey(tag))
        {
            _VariablesRoom.Planets[tag] = new Planet(tag);
            _VariablesRoom.planet_num++;
        }
    }
    //检测行星参数，第一个参数为当前行星的名字，第二个参数为赤道半径、逃逸速度、质量、密度、公转/自转周期
    public void ExplorePlanet(string cur, PlanetsParameters para)
    {
        if(cur != "Space")
        {
            if(_VariablesRoom.Planets.ContainsKey(cur))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Application.streamingAssetsPath + "/Planets.xml");
                //得到Planets节点下的所有子节点
                XmlNodeList xmlNodeList = xml.SelectSingleNode("Planets").ChildNodes;
                foreach (XmlElement e in xmlNodeList)
                {
                    if(e.GetAttribute("name") == cur)
                    {
                        switch (para)
                        {
                            case PlanetsParameters.Radius:
                                {
                                    _VariablesRoom.Planets[cur].SetRadius(double.Parse(e.SelectSingleNode("Radius").InnerText));
                                } break;
                            case PlanetsParameters.Escape_velocity:
                                {
                                    _VariablesRoom.Planets[cur].SetEscapeVelocity(double.Parse(e.SelectSingleNode("Escape_velocity").InnerText));
                                } break;
                            case PlanetsParameters.Mass:
                                {
                                    _VariablesRoom.Planets[cur].SetMass(double.Parse(e.SelectSingleNode("Mass").InnerText));
                                } break;
                            case PlanetsParameters.Density:
                                {
                                    _VariablesRoom.Planets[cur].SetDensity(double.Parse(e.SelectSingleNode("Density").InnerText));
                                } break;
                            case PlanetsParameters.Revolution_period:
                                {
                                    _VariablesRoom.Planets[cur].SetRevolutionPeriod(double.Parse(e.SelectSingleNode("Revolution_period").InnerText));
                                } break;
                            case PlanetsParameters.Rotation_period:
                                {
                                    _VariablesRoom.Planets[cur].SetRotationPeriod(double.Parse(e.SelectSingleNode("Rotation_period").InnerText));
                                } break;
                            default: break;
                        }
                    }
                }
            }
        }
    }
}

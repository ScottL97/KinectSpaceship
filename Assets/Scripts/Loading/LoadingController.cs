using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using Assets.Scripts.Game;

public class LoadingController : MonoBehaviour
{
    private GameObject VariablesRoom;
    private VariablesRoom _VariablesRoom; //VariablesRoom类

    public Text LoadingText;
    public Slider Process;
    private AsyncOperation asyn;
    void Start()
    {
        VariablesRoom = GameObject.Find("VariablesRoom");
        _VariablesRoom = VariablesRoom.GetComponent<VariablesRoom>();
        StartCoroutine(BeginLoading("Game"));
    }
    IEnumerator BeginLoading(string SceneName)
    {
        switch(_VariablesRoom.flag)
        {
            case 1:
                {
                    StartGame(_VariablesRoom.ShipId);
                } break;
            case 2:
                {
                    ContinueGame(_VariablesRoom.ShipId);
                } break;
            default: break;
        }
        asyn = SceneManager.LoadSceneAsync(SceneName);
        yield return asyn;
    }
    void Update()
    {
        Process.value = asyn.progress;
        LoadingText.text = "加载进度：" + (Process.value * 100) + "%";
    }
    //创建或重置存档文件
    void StartGame(int id)
    {
        string path = Application.streamingAssetsPath + "/Record.xml";
        //创建xml文档对象
        XmlDocument xml = new XmlDocument();
        if (!File.Exists(path))
        {
            Debug.Log("生成存档文件");
            //创建最上一层的Ships节点。
            XmlElement root = xml.CreateElement("Ships");
            //创建Ship子节点
            XmlElement shipElement = xml.CreateElement("Ship");
            //创建飞船位置Position节点
            XmlElement positionElement = xml.CreateElement("Position");
            //创建分数Score节点
            XmlElement scoreElement = xml.CreateElement("Score");
            //创建已探索行星Planets节点
            XmlElement planetsElement = xml.CreateElement("Planets");
            //创建具体分数节点
            XmlElement planetNumElement = xml.CreateElement("Planet_num");
            planetNumElement.InnerText = 0.ToString();
            XmlElement periodElement = xml.CreateElement("Period");
            periodElement.InnerText = 0.ToString();
            XmlElement destroyElement = xml.CreateElement("Destroy");
            destroyElement.InnerText = 0.ToString();
            //创建具体位置节点
            XmlElement xElement = xml.CreateElement("X");
            xElement.InnerText = 0.ToString();
            XmlElement yElement = xml.CreateElement("Y");
            yElement.InnerText = 0.ToString();
            XmlElement zElement = xml.CreateElement("Z");
            zElement.InnerText = 0.ToString();
            //设置节点的属性
            shipElement.SetAttribute("id", id.ToString());
            scoreElement.SetAttribute("sum", 0.ToString());
            //把节点一层一层的添加至xml中，注意先后顺序，这是生成XML文件的顺序
            positionElement.AppendChild(xElement);
            positionElement.AppendChild(yElement);
            positionElement.AppendChild(zElement);
            scoreElement.AppendChild(planetNumElement);
            scoreElement.AppendChild(periodElement);
            scoreElement.AppendChild(destroyElement);
            shipElement.AppendChild(positionElement);
            shipElement.AppendChild(scoreElement);
            shipElement.AppendChild(planetsElement);
            root.AppendChild(shipElement);
            xml.AppendChild(root);
            //最后保存文件
            xml.Save(path);
        }
        else
        //重置存档
        {
            Debug.Log("修改分数：" + 0);
            xml.Load(path);
            //得到Ships节点下的所有子节点
            XmlNodeList xmlNodeList = xml.SelectSingleNode("Ships").ChildNodes;
            Debug.Log("存档数量：" + xmlNodeList.Count);
            //遍历所有子节点，找到id相同的一个，清空分数与探索记录
            foreach (XmlElement ship in xmlNodeList)
            {
                if (int.Parse(ship.GetAttribute("id")) == id)
                {
                    XmlElement positionElement = (XmlElement)ship.SelectSingleNode("Position");
                    XmlNodeList positionList = positionElement.ChildNodes;
                    foreach (XmlElement position in positionList)
                    {
                        position.InnerText = 0.ToString();
                    }
                    XmlElement scoreElement = (XmlElement)ship.SelectSingleNode("Score");
                    scoreElement.SetAttribute("sum", 0.ToString());
                    XmlNodeList scoreList = scoreElement.ChildNodes;
                    foreach (XmlElement score in scoreList)
                    {
                        score.InnerText = 0.ToString();
                    }
                    XmlElement planetsList = (XmlElement)ship.SelectSingleNode("Planets");
                    planetsList.InnerText = "";
                    xml.Save(path);
                }
            }
        }
        
    }
    //读取存档文件，保存到VariablesRoom
    void ContinueGame(int id)
    {
        //创建xml文档对象
        XmlDocument xml = new XmlDocument();
        xml.Load(Application.streamingAssetsPath + "/Record.xml");
        //得到Ships节点下的所有子节点
        XmlNodeList xmlNodeList = xml.SelectSingleNode("Ships").ChildNodes;
        Debug.Log("存档数量：" + xmlNodeList.Count);
        //遍历所有子节点，找到id相同的一个，读取数据
        foreach (XmlElement ship in xmlNodeList)
        {
            if (int.Parse(ship.GetAttribute("id")) == id)
            {
                XmlElement positionElement = (XmlElement)ship.SelectSingleNode("Position");
                _VariablesRoom.x = float.Parse(positionElement.SelectSingleNode("X").InnerText);
                _VariablesRoom.y = float.Parse(positionElement.SelectSingleNode("Y").InnerText);
                _VariablesRoom.z = float.Parse(positionElement.SelectSingleNode("Z").InnerText);
                XmlElement scoreElement = (XmlElement)ship.SelectSingleNode("Score");
                _VariablesRoom.planet_num = int.Parse(scoreElement.SelectSingleNode("Planet_num").InnerText);
                _VariablesRoom.period = float.Parse(scoreElement.SelectSingleNode("Period").InnerText);
                _VariablesRoom.destroy = int.Parse(scoreElement.SelectSingleNode("Destroy").InnerText);
                XmlElement planetsList = (XmlElement)ship.SelectSingleNode("Planets");
                //_VariablesRoom.Planets = null; //清空内存
                //_VariablesRoom.Planets = new Dictionary<string, Planet>();
                foreach (XmlElement planetElement in planetsList)
                {
                    Planet planet = new Planet(planetElement.GetAttribute("name"));
                    planet.SetRadius(double.Parse(planetElement.SelectSingleNode("Radius").InnerText));
                    planet.SetEscapeVelocity(double.Parse(planetElement.SelectSingleNode("Escape_velocity").InnerText));
                    planet.SetMass(double.Parse(planetElement.SelectSingleNode("Mass").InnerText));
                    planet.SetDensity(double.Parse(planetElement.SelectSingleNode("Density").InnerText));
                    planet.SetRevolutionPeriod(double.Parse(planetElement.SelectSingleNode("Revolution_period").InnerText));
                    planet.SetRotationPeriod(double.Parse(planetElement.SelectSingleNode("Rotation_period").InnerText));
                    //_VariablesRoom.Planets.Add(planet.GetName(), planet);
                    _VariablesRoom.Planets[planet.GetName()] = planet;
                }
            }
        }
    }
}

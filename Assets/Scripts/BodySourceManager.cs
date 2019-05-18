using UnityEngine;
using UnityEngine.UI;
using Windows.Kinect;

public class BodySourceManager : MonoBehaviour
{
    private KinectSensor _Sensor; //Kinect对象
    private BodyFrameReader _Reader; //Kinect BodyFrame读取对象
    private Body[] _Bodies = null; //存储BodyFrame读取的Body对象
    public Text KinectStateText; //显示Kinect连接状态的Text组件

    public Body[] GetBodies()
    {
        return _Bodies;
    }
    void Start()
    {
        _Sensor = KinectSensor.GetDefault();
        if(_Sensor != null)
        {
            _Reader = _Sensor.BodyFrameSource.OpenReader();
            if(!_Sensor.IsOpen)
            {
                _Sensor.Open(); //启用 Kinect 传感器
            }
        }
    }
    void Update()
    {
        if(_Sensor == null)
        {
            KinectStateText.text = "Kinect状态：未创建Kinect对象";
            _Sensor = KinectSensor.GetDefault();
        }
        else
        {
            if (!_Sensor.IsOpen)
            {
                KinectStateText.text = "Kinect状态：未开启Kinect传感器";
                _Sensor.Open(); //启用 Kinect 传感器
            }
        }
        if(_Reader == null)
        {
            KinectStateText.text = "Kinect状态：未创建数据流读取器";
            _Reader = _Sensor.BodyFrameSource.OpenReader();
        }
        else
        {
            var frame = _Reader.AcquireLatestFrame(); //获取最近一帧的骨骼数据流
            if (frame != null)
            {
                KinectStateText.text = "Kinect状态：Ready";
                if (_Bodies == null)
                {
                    //为 Body 类型数组开辟空间
                    _Bodies = new Body[_Sensor.BodyFrameSource.BodyCount];
                }
                frame.GetAndRefreshBodyData(_Bodies);
                frame.Dispose(); //清除数据，防止垃圾占用内存
                frame = null;
            }
            else
            {
                KinectStateText.text = "Kinect状态：未成功读取数据流";
            }
        }
    }

    //清除占用内存资源
    void OnApplicationQuit()
    {
        if(_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }
        if(_Sensor != null)
        {
            if(_Sensor.IsOpen)
            {
                _Sensor.Close();
            }
            _Sensor = null;
        }
    }
}

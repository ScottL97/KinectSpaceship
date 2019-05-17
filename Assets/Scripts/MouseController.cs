using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class MouseController : MonoBehaviour
{
    private GameObject BodySourceManager; //BodySourceManager游戏对象
    private BodySourceManager _BodySourceManager; //BodySourceManager类
    public Dictionary<ulong, Body> _Bodies = new Dictionary<ulong, Body>(); //存储被检测到的Body对象
    private List<ulong> trackedIds; //由_Bodies字典的键获得，表示上一帧被检测到的Body对象
    public int MouseSpeed; //光标移动速度
    private int? RightHandState; //点击状态，为null时表示右手状态不确定，为0时表示可点击，为1时表示正处于点击状态，防止重复点击
    private int? LeftHandState; //点击状态，为null时表示左手状态不确定，为0时表示可点击，为1时表示正处于点击状态，防止重复点击
    public ulong CurrentId; //当前光标控制者Body的id
    public float RightX, RightY; //光标的X轴和Y轴坐标
    public float LeftX, LeftY;
    public bool ModeSwitch = false; //左手控制的开关
    public bool IfReady = false;
    public int ScreenWidth;
    public int ScreenHeight;
    public int ButtonNum; //当前指向的按钮

    private GameObject VariablesRoom; //VariablesRoom游戏对象，存储需要在场景切换时传递的数据
    private VariablesRoom _VariablesRoom; //VariablesRoom类

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        BodySourceManager = GameObject.Find("BodySourceManager");
        _BodySourceManager = BodySourceManager.GetComponent<BodySourceManager>();
        VariablesRoom = GameObject.Find("VariablesRoom");
        _VariablesRoom = VariablesRoom.GetComponent<VariablesRoom>();
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
    }
    void FixedUpdate()
    {
        UpdateBodies();
        //Debug过程用enter键控制点击
        if (_VariablesRoom.IfDebug)
        {
            RightHandState = 0;
            if(Input.GetKey(KeyCode.KeypadEnter))
            {
                RightHandState = 1;
            }
        }
        if (IfReady)
        {
            UpdateMouseState();
        }
    }
    public void UpdateBodies()
    {
        //检查BodySourceManager游戏对象是否为空
        if (BodySourceManager == null)
        {
            return;
        }
        //检查BodySourceManager游戏对象是否已挂载脚本
        if (_BodySourceManager == null)
        {
            return;
        }
        //获取Body对象数组并检查是否为空
        Body[] bodies = _BodySourceManager.GetBodies();
        if (bodies == null)
        {
            return;
        }
        //将本次检测到的id保存到列表中
        List<ulong> trackingIds = new List<ulong>();
        //将上一帧已检测到的id保存到列表中
        trackedIds = new List<ulong>(_Bodies.Keys);
        //将该帧检测到、上一帧没有检测到的Body对象添加到_Bodies字典中
        foreach (var body in bodies)
        {
            //Debug.Log("id: " + body.TrackingId);
            if (body.IsTracked)
            {
                IfReady = true;
                trackingIds.Add(body.TrackingId);
                if (!trackedIds.Contains(body.TrackingId))
                {
                    trackedIds.Add(body.TrackingId);
                    _Bodies[body.TrackingId] = body;
                }
            }
        }
        //将上一帧检测到，但不包含在该帧检测到的列表中的Body对象从_Bodies字典中删除
        foreach (var trackedId in trackedIds)
        {
            if (!trackingIds.Contains(trackedId))
            {
                _Bodies[trackedId] = null;
                trackedIds.Remove(trackedId);
                _Bodies.Remove(trackedId);
            }
        }
        //默认光标控制者为检测到的第一个人
        if(IfReady)
        {
            foreach (var trackedId in trackedIds)
            {
                if(trackedId != 0)
                {
                    CurrentId = trackedId;
                }
            }
        }
    }
    public Vector3 GetMousePosition(string hand)
    {
        if(hand == "RightHand")
        {
            //右手坐标，范围[-1, 1]
            CameraSpacePoint RightHandPoint = _Bodies[CurrentId].Joints[JointType.HandRight].Position;
            RightX = Mathf.Clamp(RightHandPoint.X * MouseSpeed * RightHandPoint.Z, -ScreenWidth / 2, ScreenWidth / 2); //通过Z坐标对光标的移动速度进行调节
            RightY = Mathf.Clamp(RightHandPoint.Y * MouseSpeed * RightHandPoint.Z, -ScreenHeight / 2, ScreenHeight / 2);
            return new Vector3(RightX, RightY, 0);
        }
        else
        {
            //左手坐标
            CameraSpacePoint LeftHandPoint = _Bodies[CurrentId].Joints[JointType.HandLeft].Position;
            LeftX = Mathf.Clamp(LeftHandPoint.X * MouseSpeed * LeftHandPoint.Z, -ScreenWidth / 2, ScreenWidth / 2);
            LeftY = Mathf.Clamp(LeftHandPoint.Y * MouseSpeed * LeftHandPoint.Z, -ScreenHeight / 2, ScreenHeight / 2);
            return new Vector3(LeftX, LeftY, 0);
        }
    }
    public int? GetHandState(string hand)
    {
        if (hand == "RightHand") return RightHandState;
        else return LeftHandState;
    }
    public void UpdateMouseState()
    {
        if (_Bodies[CurrentId].HandRightConfidence == TrackingConfidence.High)
        {
            if (_Bodies[CurrentId].HandRightState == HandState.Closed && RightHandState == 0)
            {
                Debug.Log("click");
                RightHandState = 1;
            }
            else if (_Bodies[CurrentId].HandRightState != HandState.Closed)
            {
                RightHandState = 0;
            }
        }
        if (_Bodies[CurrentId].HandLeftConfidence == TrackingConfidence.High)
        {
            if (_Bodies[CurrentId].HandLeftState == HandState.Closed && LeftHandState == 0)
            {
                ModeSwitch = !ModeSwitch;
                Debug.Log(ModeSwitch);
                LeftHandState = 1;
            }
            else if (_Bodies[CurrentId].HandLeftState != HandState.Closed)
            {
                LeftHandState = 0;
            }
        }
    }
    public int GetPlayerNumbers()
    {
        int count = 0;
        foreach(var trackedId in trackedIds)
        {
            count++;
        }
        return count;
    }
}

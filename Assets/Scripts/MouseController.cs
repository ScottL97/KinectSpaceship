using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class MouseController : MonoBehaviour
{
    public GameObject BodySourceManager; //BodySourceManager游戏对象
    private BodySourceManager _BodySourceManager; //BodySourceManager类
    private Dictionary<ulong, Body> _Bodies = new Dictionary<ulong, Body>(); //存储被检测到的Body对象
    private List<ulong> trackedIds; //由_Bodies字典的键获得，表示上一帧被检测到的Body对象
    public int MouseSpeed; //光标移动速度
    public int? ClickState; //点击状态，为null时表示右手状态不确定，为0时表示可点击，为1时表示正处于点击状态，防止重复点击
    private ulong CurrentId; //当前光标控制者Body的id
    public float X, Y; //光标的X轴和Z轴坐标
    public bool IfReady = false;
    void FixedUpdate()
    {
        UpdateBodies();
        if(IfReady)
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
        _BodySourceManager = BodySourceManager.GetComponent<BodySourceManager>();
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
    public Vector3 GetMousePosition()
    {
        //右手坐标，范围[-1, 1]
        CameraSpacePoint RightHandPoint = _Bodies[CurrentId].Joints[JointType.HandRight].Position;
        X = Mathf.Clamp(RightHandPoint.X * MouseSpeed * RightHandPoint.Z, -950, 950);
        Y = Mathf.Clamp(RightHandPoint.Y * MouseSpeed * RightHandPoint.Z, -530, 530);
        return new Vector3(X, Y, 0);
    }
    public void UpdateMouseState()
    {
        if (_Bodies[CurrentId].HandRightConfidence == TrackingConfidence.High)
        {
            if (_Bodies[CurrentId].HandRightState == HandState.Closed && ClickState == 0)
            {
                Debug.Log("click");
                ClickState = 1;
            }
            else if (_Bodies[CurrentId].HandRightState != HandState.Closed)
            {
                ClickState = 0;
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

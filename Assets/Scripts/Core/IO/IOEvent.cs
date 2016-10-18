/**
* Copyright (c) 2012,广州纷享游艺设备有限公司
* All rights reserved.
* 
* 文件名称：IOEvent.cs
* 简    述：每条协议包含的信息
* 创建标识：meij  2015/11/2
* 修改标识：meij  2015/11/06
* 修改描述：代码优化。
*/
using UnityEngine;
using System.Collections;

public class IOEvent
{
    private  byte _ID ;
    private  bool _isSet;
    private  bool _isWarning ;
    private  bool _isOutTicket;
    private  bool _isCoin ;
    private  bool _isShake;
    private  bool _isStart;
    private  bool _isPush1;
    private  bool _isPush2;
    private Vector2 _screenPos;
    private Vector3 _rockerBar;

    //角色id
    public  byte ID                              
    {
        get { return _ID; }
        set { _ID = value; }
    }

    //屏幕坐标
    public Vector2 ScreenPos
    {
        get { return _screenPos; }
        set { _screenPos = value; }
    }
    //摇杆传入坐标
    public Vector2 RockerBar
    {
        get { return _rockerBar; }
        set { _rockerBar = value; }
    }
    //1：设置按钮按下 0：设置按钮没按下
    public  bool IsSet                            
    {
        get { return _isSet; }
        set { _isSet = value; }
    }

    //1：警报 0:解除警报
    public  bool IsWarning                        
    {
        get { return _isWarning; }
        set { _isWarning = value; }
    }

    //1: 出票 0：否
    public  bool IsOutTicket                      
    {
        get { return _isOutTicket; }
        set { _isOutTicket = value; }
    }

    //1：投票  0：否
    public  bool IsCoin                           
    {
        get { return _isCoin; }
        set { _isCoin = value; }
    }

    //1： 开始按钮按下  0：否
    public  bool IsStart                          
    {
        get { return _isStart; }
        set { _isStart = value; }
    }
    
   //1: 摇杆摇动 0:否
    public bool IsShake
    {
        get { return _isShake; }
        set { _isShake = value; }
    }

    //
    public bool IsPush1
    {
        get { return _isPush1; }
        set { _isPush1 = value; }
    }
    //
    public bool IsPush2
    {
        get { return _isPush2; }
        set { _isPush2 = value; }
    }
    
}

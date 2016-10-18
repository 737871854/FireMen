/**
* Copyright (c) 2012,广州纷享游艺设备有限公司
* All rights reserved.
* 
* 文件名称：IOManager.cs
* 简    述：获取模拟器操作信息
* 创建标识：meij  2015/10/28
* 修改标识：meij  2015/11/10
* 修改描述：采用1个COM端口进行协议收发。
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Need.Mx;

public class IOManager : MonoBehaviour
{
    private bool[] isSetGameEnd;
    private int ioCount;
    private SerialIOHost serialIoHost;
    private IOEvent ie;
    private IOEvent[] ioEvent;                       //协议信息数组   每帧最多包含MAX_PLAYER_NUMBER个协议数,
    private bool[] playerGameBegine;
    private bool[] playerGameEnd;
    private bool bossCome;
    private bool[] shootingCalibration;
    private int[] ticketNumber;
    private bool isfindport;
    private bool cansearch = true;
    private float checkTime;
    private float ticktime = 1.0f;
    private float searchcount;
    private bool[] outtick;
    private bool checkPass;
    private bool hasCheck;
    private bool connectDown;
    private float time;
    private string idString;
    private bool isWaterLow;
    private bool isWaterHight;

    private int connectCount = 0;
    public int GetConnectCount()
    {
        return connectCount;   
    }
    public void SetConnnectCount()
    {
        ++connectCount;
    }

    // 端口读取是否正常
    private float connectTime = 2;
    private bool isConnect = true;
    public bool IsConnect
    {
        get { return isConnect; }
        set { isConnect = value; }
    }

    private bool receive5A = false;
    public bool Receice5A
    {
        get { return receive5A; }
        set { receive5A = value; }
    }
    private string findPort = "空";
    public void SetFindPort(string port)
    {
        findPort = findPort + port;
    }
    public string GetFindProt()
    {
        return findPort;
    }
   

    private bool beginFind = false;
    public bool BeginFind
    {
        get { return beginFind; }
        set { beginFind = value; }
    }


    private byte[] byteHost = new byte[14];
    // 每帧中每条协议对应一个角色信息)
    #region ----------------------获取当前帧中，对应Player的协议信息-----------------------------------
    public byte[] ByteHost
    {
        get { return byteHost; }
        set { byteHost = value; }
    }

    private byte[] byteHostAA = new byte[14];
    public byte[] ByteHostAA
    {
        get { return byteHostAA; }
        set { byteHostAA = value; }
    }

    public float CheckTime
    {
        get { return checkTime; }
        set { checkTime = value; }
    }

    public string IDString
    {
        get { return idString; }
        set { idString = value; }
    }

    public  byte GetPlayerID(int i)
    {
        return ioEvent[i].ID;
    }

    public Vector2 GetRockerBar(int i)
    {
        return ioEvent[i].RockerBar;
    }

    public bool GetIsSet(int i)
    {
        return ioEvent[i].IsSet;
    }

    public bool GetIsStart(int i)
    {
        return ioEvent[i].IsStart;
    }

    public bool GetIsWarning(int i)
    {
        return ioEvent[i].IsWarning;
    }

    public bool GetIsOutTicket(int i)
    {
        return ioEvent[i].IsOutTicket;
    }

    public bool GetIsCoin(int i)
    {
        return ioEvent[i].IsCoin;
    }

    public Vector3 GetScreenPos(int i)
    {
        return ioEvent[i].ScreenPos;
    }

    public bool GetIsShake(int i)
    {
        return ioEvent[i].IsShake;
    }

    public bool GetIsPush1(int i)
    {
        return ioEvent[i].IsPush1;
    }

    public bool GetIsPush2(int i)
    {
        return ioEvent[i].IsPush2;
    }

    public bool CheckPass
    {
        set { checkPass = value; }
        get { return checkPass; }
    }

    public bool HasCheck
    {
        set { hasCheck = value; }
        get { return hasCheck; }
    }

    public bool IsWaterLow
    {
        set { isWaterLow = value; }
        get { return isWaterLow; }
    }

    public bool IsWaterHight
    {
        set { isWaterHight = value; }
        get { return isWaterHight; }
    }

    //重置指定角色操作数据
    public void ResetEvent(int i)
    {
        ioEvent[i].IsSet = false;
        ioEvent[i].IsCoin = false;
        ioEvent[i].IsWarning = false;
        ioEvent[i].IsOutTicket = false;
        ioEvent[i].IsShake = false;
        ioEvent[i].IsStart = false;
        ioEvent[i].IsPush1 = false;
        ioEvent[i].IsPush2 = false;
    }
    #endregion

    #region ----------------设置下发协议信息-----------------------------
    //设置对应玩家是否游戏开始（硬件打开对应的水阀）
    public void SetPlayerGameBegine(int index, bool value)
    {
        playerGameBegine[index] = value;
    }
    //指定玩家游戏结束（关闭该玩家水阀）
    public void SetPlayerGameEnd(int index, bool value)
    {
        playerGameEnd[index] = value;
    }
    //Boss来了
    public void SetBossCome()
    {
        bossCome = true;
    }
    //射击校验
    public void ShootCalibration(int index, bool value)
    {
        shootingCalibration[index] = value;
    }
    /// <summary>
    /// 设置指定玩家出票数
    /// </summary>
    /// <param name="index"></param> playerID
    /// <param name="value"></param> 出票数
    public void ComputeTicket(int index, int value)
    {
        int scorepreticket = Main.SettingManager.TicketModel;
        ticketNumber[index] = Mathf.FloorToInt(value / scorepreticket);
        if(ticketNumber[index] > 0)
        {
            outtick[index] = true;
        }
    }
    //整个游戏结束（关闭所有玩家水阀）
    public void SetGameEnd()
    {
        for (int i = 0; i < ioCount;i++ )
        {
            playerGameEnd[i] = true;
        }
    }
    //控制条件（为了只触发一次结束条件）
    public bool GetIsSetGameEnd(int index)
    {
        return isSetGameEnd[index];
    }
    public void SetIsSetGameEnd(int index, bool value)
    {
        isSetGameEnd[index] = value;
    }
    #endregion ---------------------------------------------
    public SerialIOHost GetSerialIoHost(int i)
    {
        return serialIoHost;
    }

    /// <summary>
    /// 应用程序退出操作
    /// </summary>
    public void Close()
    {
        if (serialIoHost != null)
        {
            for (int i = 0; i < GameConfig.GAME_CONFIG_PLAYER_COUNT; i++)
            {
                Main.IOManager.SetPlayerGameBegine(i, false);
             }
            serialIoHost.Close();
            }    
    }

    public void ResetIO()
    {
        if (null != serialIoHost)
        {
            serialIoHost.Close();
            cansearch = true;
            isfindport = false;
            searchcount = 0;
            serialIoHost = null;
            serialIoHost = new SerialIOHost();
        }
    }

    //初始化操作，打开端口，初始化ioEvent数组
    public void Init(int portCount)
    {
        ioCount          = portCount;
        ioEvent          = new IOEvent[portCount];
        playerGameBegine = new bool[portCount];
        playerGameEnd    = new bool[portCount];
        ticketNumber     = new int[portCount];
        isSetGameEnd     = new bool[portCount];
        outtick          = new bool[portCount];
        shootingCalibration = new bool[portCount];
        checkPass        = true;
        idString         = null;
        hasCheck         = true;
        connectDown      = false;

        serialIoHost = new SerialIOHost();
        for (byte i = 0; i < ioCount; i++)
        {
            ioEvent[i]    = new IOEvent();
        }
    }

    /// <summary>
    /// 每帧分别读取一次协议队列中的协议，并存入协议信息类对象数组ioEvent
    /// </summary>
    public void UpdateIOEvent()
    {      
        if (null != idString)
        {
            Main.SettingManager.CheckID = idString;
            Main.SettingManager.Save();
            idString = null;
        }

        if (isfindport)
        {
            // 丢失连接
            if (!isConnect)
            {
                if (connectTime > 0)
                {
                    connectTime -= Time.deltaTime;
                }
                else
                {
                    isfindport = false;
                    connectTime = 2.0f;
                    Main.IOManager.SetFindPort("空");
                    Main.IOManager.ResetIO();
                }
            }
            else
            {
                connectTime = 2.0f;
            }
        }
      

        //// 测试
        checkTime += Main.NonStopTime.deltaTime;

        // 先屏蔽检测

        //if (!checkPass)
        //{
        //    connectDown = true;
        //}

        //if (connectDown)
        //{
        //    return;
        //}

        //if (hasCheck)
        //{
        //    checkTime = GameConfig.GAME_CONFIG_HAS_NO_CHECK_TIME;
        //    hasCheck = false;
        //}

        //checkTime -= Main.NonStopTime.deltaTime;
        //if (checkTime <= 0)
        //{
        //    checkTime = 0;
        //    checkPass = false;
        //}


        if (cansearch && !isfindport && searchcount < 50)
        {
            beginFind = true;
            isfindport = serialIoHost.Setup(14, 8, 10);
            cansearch = false;
            ++searchcount;
        }

        if(!isfindport)
        {
            time += Main.NonStopTime.deltaTime;
        }

        if (time >= ticktime)
        {
            cansearch = true;
            time = 0.0f;
        }
        if(serialIoHost != null && serialIoHost.HadDevice())
        {
            serialIoHost.Update();
            for (byte i = 0; i < ioCount; i++)
            {
                if(serialIoHost.quequeInfo[i].Count>0)
                {
                    //接受到的下位机数据
                    {
                        this.ie = serialIoHost.quequeInfo[i].Dequeue();
                        ioEvent[i].ID = this.ie.ID;
                        ioEvent[i].RockerBar = this.ie.RockerBar;
                        ioEvent[i].IsSet = this.ie.IsSet;
                        ioEvent[i].IsWarning = this.ie.IsWarning;
                        ioEvent[i].IsOutTicket = this.ie.IsOutTicket;
                        ioEvent[i].IsCoin = this.ie.IsCoin;
                        ioEvent[i].IsStart = this.ie.IsStart;
                        ioEvent[i].ScreenPos = this.ie.ScreenPos;
                        ioEvent[i].IsShake = this.ie.IsShake;
                        ioEvent[i].IsPush1 = this.ie.IsPush1;
                        ioEvent[i].IsPush2 = this.ie.IsPush2;
                        //if (i == 0)
                        //{
                        //    ioEvent[i].IsPush1 = this.ie.IsPush1;
                        //    ioEvent[i].IsPush2 = this.ie.IsPush2;
                        //}else if(i == 1)
                        //{
                        //    IsWaterLow = this.ie.IsPush1;
                        //    IsWaterHight = this.ie.IsPush2;
                        //}
                       if (i == 1)
                       {
                           IsWaterLow = this.ie.IsPush2;
                           IsWaterHight = this.ie.IsPush1;
                       }
                    }                            
                }              
            }
       
            //向下位机发协议
            int num = 0;
            for (byte i = 0; i < ioCount;i++ )
            {
                serialIoHost.Write(num, i);
                serialIoHost.Write(num + 1, 7, playerGameBegine[i]);
                serialIoHost.Write(num + 1, 6, playerGameEnd[i]);
                if(ioEvent[i].IsOutTicket)
                {
                    Main.SettingManager.LogTicket(1);
                    --ticketNumber[i];
                    if (ticketNumber[i] > 0)
                    {
                        outtick[i] = true;
                    }
                    else
                    {
                        Main.SettingManager.Save();
                        outtick[i] = false;
                    }
                }
                serialIoHost.Write(num + 1, 5, outtick[i]);
                serialIoHost.Write(num + 1, 4, bossCome);
                serialIoHost.Write(num + 1, 3, shootingCalibration[i]);
              
                if(bossCome)
                {
                    bossCome = false;
                }
               
                num += 2;
            }
          
        }            
     } 
}


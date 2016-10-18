/*
 * Copyright (c) 
 * 
 * 文件名称：   SettingManager.cs
 * 
 * 简    介:    SettingManager数据管理
 * 
 * 创建标识：  Mike 2016/7/28 17:54:50
 * 
 * 修改描述：    2016.9.3 PlayerPrefs转Json
 * 
 */

using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Need.Mx;
using JsonFx.Json;

public class SettingManager : MonoBehaviour
{
    private SettingConfigData po;

    private string _checkID         = string.Empty;
    private int _gameRate           = 3;                                // 游戏币率
    private int _gameVolume         = 10;                               // 游戏音量
    private int _gameLevel          = 1;                                // 游戏难度
    private int _gameLanguage       = 0;                                // 游戏语言
    private int _ticketModel        = 2000;                             // 出票模式
    private int _watershow          = 1;
    private int _ticketScore        = 0;                                // 没票分数

    private int[] _hasCoin;                                             // 玩家剩余币数

    private int[] _hasTicket;                                           // 玩家剩余未出票数

    private List<float[]> _monthList = new List<float[]>();             // 月份信息

    private float[] _totalRecord;                                       // 总记录

    private float[] _screenInfo;                                        // 屏幕宽高
    private List<float[]>  _screenInfoList = new List<float[]>();       // 玩家校验屏幕宽高

    private List<float[]> _pointXList = new List<float[]>();            // 玩家校验点
    private List<float[]> _pointYList = new List<float[]>();            // 玩家校验点

    private int _ticket;    // 玩家当前出票数
    private int _gameTime;  // 游戏时间
    private int _upTime;    // 开机时间
    private int _gameCount; // 游戏次数
    private int _coin;      // 耗币

    public void Init()
    {
        // 取出后台存储所有数据
        this.po = HelperTool.LoadJson(ResUpdateManager.Instance.GetFilePath(GameConfig.SETTING_COINFIG));
        if (null == this.po)
        {
            this.po = new SettingConfigData();
        }
        
        // CheckID
        this._checkID = po.CheckId;
    
        // 游戏币率
        this._gameRate = po.GameRate;

        // 游戏语言版本 0中文 1英文
        this._gameLanguage = po.GameLanguage;

        // 游戏难度
        this._gameLevel = po.GameDiffculty;

        // 出票模式
        this._ticketModel = po.TicketModel;

        // 出票分数
        this._ticketScore = SettingConfig.scorePreTicket[this._ticketModel];

        // 游戏音量
        this._gameVolume = po.GameVolume;

        // 当前剩余币数
        this._hasCoin = po.Coin;

        // 当前剩余票数
        //this._hasTicket = po.Ticket;

        // 是否显示水标 0显示 1不显示
        this._watershow = po.ShowWater;

        // 月份信息
        this._monthList = po.MonthList;

        // 总记录
        this._totalRecord = po.TotalRecord;

        // 获取玩家剩余币数
        for (int i = 0; i < GameConfig.GAME_CONFIG_PLAYER_COUNT; i++)
        {
            Main.PlayerManager.getPlayer(i).ChangeCoin(this._hasCoin[i]);
        }
 
        // 屏幕宽高
        _screenInfo = po.ScreenInfo;

        // 玩家校验屏幕宽高
        _screenInfoList = po.ScreenInfoList;

        // 玩家校验点X
        this._pointXList = po.PointX;
        this._pointYList = po.PointY;

        CheckIsNewMonth();
    }
      
   
    #region  -----------------后台提供借口-----------------------begin------------------
    // 校验ID
    public string CheckID
    {
        get { return _checkID; }
        set { _checkID = value; }
    }

    //获取游戏币率
    public int GameRate
    {
        get { return _gameRate; }
        set { _gameRate = value;}
    }
    //获取游戏音量
    public int GameVolume
    {
        get { return _gameVolume; }
        set { _gameVolume = value; }
    }
    //获取游戏难度
    public int GameLevel
    {
        get { return _gameLevel; }
        set { _gameLevel = value; }
    }
    //获取游戏语言
    public int GameLanguage
    {
        get { return _gameLanguage; }
        set { _gameLanguage = value; }
    }

    // 是否显示水标
    public int WaterShow
    {
        get { return _watershow; }
        set { _watershow = value; }
    }

    //获取1票需要多少分或出票模式
    public int TicketModel
    {
        get { return _ticketModel; }
        set { _ticketModel = value; }
    }
    
    // 总记录
    public float[] TotalRecord()
    {
        return this._totalRecord;
    }

    // 没票分数
    public int TicketScore
    {
        get { return this._ticketScore; }
        set { this._ticketScore = value; }
    }

    //获取射击校验信息
    public float[] GetPlayerScreenInfo(int playerID)
    {
        if (playerID >= GameConfig.GAME_CONFIG_PLAYER_COUNT)
       {
           return null;
       }

        return this._screenInfoList[playerID];      
    }

    // 获取指定玩家校验点
    public List<float[]> GetPoint(int playerID)
    {
        List<float[]> tempList = new List<float[]>();
        tempList.Add(this._pointXList[playerID]);
        tempList.Add(this._pointYList[playerID]);
        return tempList;

    }

    public float[] GetPointX(int playerID)
    {
        if (playerID >= GameConfig.GAME_CONFIG_PLAYER_COUNT)
        {
            return null;
        }
        return this._pointXList[playerID];
    }

    public float[] GetPointY(int playerID)
    {
        if (playerID >= GameConfig.GAME_CONFIG_PLAYER_COUNT)
        {
            return null;
        }
        return this._pointYList[playerID];
    }

    // 获取屏幕宽高信息
    public float[] GetScreenInfo()
    {
        for (int i = 0; i < this._screenInfo.Length; i++ )
        {
            this._screenInfo[i] *= 0.5f;
        }
        return this._screenInfo;
    }

    #endregion         -----------------后台提供借口-----------------end------------------------

    // 修改设计校验点X值
    public void SetPointX(int playerId, int index, float value)
    {
        if (playerId >= GameConfig.GAME_CONFIG_PLAYER_COUNT)
        {
            return;
        }

        this._pointXList[playerId][index] = value;
    }

    // 修改设计校验点Y值
    public void SetPointY(int playerId, int index, float value)
    {
        if (playerId >= GameConfig.GAME_CONFIG_PLAYER_COUNT)
        {
            return;
        }

        this._pointYList[playerId][index] = value;
    }

    // 修改po的Coin值
    public void ClearCoin(int playerID)
    {
        this._hasCoin[playerID] = 0;
        Main.PlayerManager.getPlayer(playerID).ClearCoin();
    }

    // 清除月份信息
    public void ClearMonthInfo()
    {
        for (int i = 0; i < this._monthList.Count; i++ )
        {
            for (int j = 1; j < this._monthList[i].Length; j++ )
            {
                this._monthList[i][j] = 0;
            }
        }
    }

    // 清除总记录
    public void ClearTotalRecord()
    {
        for (int i = 0; i < this._totalRecord.Length; i++ )
        {
            this._totalRecord[i] = 0;
        }
    }

    // 设置屏幕宽高信息
    public void SetScreenInfo(float[] info)
    {
        this._screenInfo = info;
    }

    //设置射击校验信息
    public void SetScreenInfo(int index, Vector2 value)
    {
        float[] temp = new float[2];
        temp[0] = value.x;
        temp[1] = value.y;

        if (index >= GameConfig.GAME_CONFIG_PLAYER_COUNT)
        {
            return;
        }

        this._screenInfoList[index] = temp;
    }

    public List<float[]> GetMonthData()
    {
        return this._monthList;
    }

    // 获取指定月份信息
    public float[] GetMonthData(int index)
    {
       if (index < 3)
       {
           return this._monthList[index];
       }

       return null;
    }

    // 玩家当前出票数
    public void LogTicket(int value)
    {
        this._ticket += value;
    }
    
    // 游戏时间
    public void LogGameTimes(int value)
    {
        this._gameTime += value;
    }

    // 开机时间
    public void LogUpTime(int value)
    {
        this._upTime += value;
    }

    // 耗币
    public void LogCoins(int value)
    {
        this._coin += value;
    }

    // 增加游戏次数
    public void LogNumberOfGame(int value)
    {
        this._gameCount += value;
    }

   //----------------------------------------------

    // 保存后台信息
    public void Save()
    {
        CopyToPo();
        HelperTool.SaveJson(this.po, ResUpdateManager.Instance.GetFilePath(GameConfig.SETTING_COINFIG));
    }

    public void CopyToPo()
    {
        // 玩家剩余币数
        {
            for (int i = 0; i < GameConfig.GAME_CONFIG_PLAYER_COUNT; i++)
            {
                this._hasCoin[i] = Main.PlayerManager.getPlayer(i).Coin;
            }
        }

        // 修改当期那月份信息
        {
            this._monthList[0][1] += this._coin;
            this._monthList[0][2] += this._ticket;
            this._monthList[0][3] += this._gameTime;
            this._monthList[0][4] += this._upTime;
        }

        // 修改总记录
        {
            this._totalRecord[0] += this._gameCount;
            this._totalRecord[1] += this._gameTime;
            this._totalRecord[2] += this._upTime;
            this._totalRecord[4] += this._coin;
            this._totalRecord[5] += this._ticket;          
        }

        // 临时数据清除
        {
            this._gameCount = 0;
            this._gameTime = 0;
            this._upTime = 0;
            this._coin = 0;
            this._ticket = 0;
        }

        // 自改po的值
        {
            this.po.CheckId = this._checkID;
            this.po.GameRate = this._gameRate;
            this.po.GameLanguage = this._gameLanguage;
            this.po.GameDiffculty = this._gameLevel;
            this.po.TicketModel = this._ticketModel;
            this.po.GameVolume = this._gameVolume;
            this.po.ShowWater = this._watershow;
            this.po.MonthList = this._monthList;
            this.po.Coin = this._hasCoin;

            this.po.TotalRecord = this._totalRecord;
            for (int i = 0; i < GameConfig.GAME_CONFIG_PLAYER_COUNT; i++)
            {
                this.po.Coin[i] = Main.PlayerManager.getPlayer(i).Coin;
            }
            this.po.ScreenInfo = this._screenInfo;
            this.po.ScreenInfoList = this._screenInfoList;
            this.po.PointX = this._pointXList;
            this.po.PointY = this._pointYList;
        }
    }

    private void CheckIsNewMonth()
    {
        DateTime now = DateTime.Now;
        int month = now.Month;
        bool isNewMonth = true;
        for (int i = 0; i < this._monthList.Count; i++ )
        {
            if (month == this._monthList[i][0])
            {
                isNewMonth = false;
                break;
            }
        }

        if (!isNewMonth)
        {
            return;
        }

        this._monthList[2] = this._monthList[1];
        this._monthList[1] = this._monthList[0];
        for (int i = 0; i < this._monthList[0].Length; i++ )
        {
            this._monthList[0][i] = 0;
        }
        this._monthList[0][0] = month;
    }
  
}

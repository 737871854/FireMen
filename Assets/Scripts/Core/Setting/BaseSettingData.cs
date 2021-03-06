﻿/*
 * Copyright (c) 
 * 
 * 文件名称：   BaseSettingData.cs
 * 
 * 简    介:    BaseSettingData后台存储数据信息
 * 
 * 创建标识：  
 * 
 * 修改描述：    Mike 2016/7/28 14:11:21
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SettingConfigData
{
    // 校验ID
    public string CheckId { get; set; }

    // 0,1,2,3,4,5,6币率
    public int GameRate { get; set; }

    // 0代表中文，1代表英文。
    public int GameLanguage { get; set; }

    // 0 代表简单，1代表中等，2代表困难
    public int GameDiffculty { get; set; }

    // 0 代表模式1,1 代表模式2
    public int TicketModel { get; set; }

    // 当前音量，分为10个等级
    public int GameVolume { get; set; }

    // 是否显示水标
    public int ShowWater { get; set; }

    // 月份信息
    public List<float[]> MonthList = new List<float[]>();

    // 总记录 游戏次数，游戏时间，开机时间，总币数，总出票数
    public float[] TotalRecord { get; set; }

    // 玩家剩余币数
    public int[] Coin { get; set; }

    // 玩家剩余票数
    //public int[] Ticket { get; set; }

    // 屏幕宽高
    public float[] ScreenInfo { get; set; }

    // 玩家校验屏幕宽高
    public List<float[]> ScreenInfoList = new List<float[]>();

    // 玩家校验点X
    public List<float[]> PointX = new List<float[]>();

    // 玩家校验点Y
    public List<float[]> PointY = new List<float[]>();


    public SettingConfigData()
    {
        this.CheckId = string.Empty;
        this.GameRate = 3;
        this.GameLanguage = 0;
        this.GameDiffculty = 1;
        this.TicketModel = 1;
        this.GameVolume = 5;
        this.ShowWater = 0;
        for (int i = 0; i < 3; i++ )
        {
            this.MonthList.Add(new float[5]);
        }

        this.TotalRecord = new float[6];
        this.Coin = new int[3];
        //this.Ticket = new int[3];
        this.ScreenInfo = new float[] { 128, 128 };
        for (int i = 0; i < 3; i++ )
        {
            this.ScreenInfoList.Add(new float[] { 128, 128 });
        }

        for (int i = 0; i < 3; i++ )
        {
            this.PointX.Add(new float[15]);
            this.PointY.Add(new float[15]);
        }

    }

   
}

/**
* 	Copyright (c) 2014 Need co.,Ltd
*	All rights reserved
*		
*	文件名称:	Main.cs	
*	简    介:	整个游戏的进入接口，继承于MonoBehaviour,挂接的GameObject不能销毁。
*	创建标识:	Terry 2015/08/11
*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using CodeStage.AdvanecedFPSCounter;
using Need.Mx;
using System.IO;


public delegate void delegateUpdate(float time, float deltaTime);
/// <summary>
/// 控制游戏程序的进入和退出
/// </summary>
public class Main : MonoBehaviour
{
    public static GameObject Container;
    public static DebugConsole Panel;
    public static GameObject MainCamera;
    public static GameController Controller;
    public static PlayerManager PlayerManager;
    public static GameMode GameMode;
    public static SoundManager SoundManager;
    public static SoundController SoundController;
    public static NonStopTime NonStopTime;
    public static GameTime GameTime;
    public static RankManager RankManager;
    public static IOManager IOManager;
    public static SettingManager SettingManager;
    private static float curTime;
    private static int   playerCount;
    private static float updateFix;
    private static float updateLog;
    private static float updateSave;

    void Awake()
    {
        InitUnity();
        InitGame();
    }

    void Start()
    {
        InitOption();
        InitUI();
    }

    /// <summary>
    /// 初始化UnitEngine.Application的一些属性
    /// </summary>
    void InitUnity()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Application.targetFrameRate = 60;
        //保证即使被切换到苹果后台也不会断线;即使这样设置也不能保证 苹果有后台
        Application.runInBackground = true;
        //注册log回调函数，主要为了错误时可以关闭Socket，不造成Untiy崩溃
        //Application.RegisterLogCallback(ProcessExceptionReport);
        Application.backgroundLoadingPriority = ThreadPriority.High;
    }

    /// <summary>
    /// 核心内容，初始化网络NetWork,游戏配置数据初始化
    /// </summary>
    void InitGame()
    {
        Container = gameObject;
        DontDestroyOnLoad(Container);
        Controller = Container.GetComponent<GameController>();
        PlayerManager = Container.GetComponent<PlayerManager>();
        GameMode = Container.GetComponent<GameMode>();
        SoundManager = Container.GetComponent<SoundManager>();
        SoundController = Container.GetComponent<SoundController>();
        NonStopTime = Container.GetComponent<NonStopTime>();
        GameTime = Container.GetComponent<GameTime>();
        RankManager = Container.GetComponent<RankManager>();
        IOManager = Container.GetComponent<IOManager>();
        SettingManager = Container.GetComponent<SettingManager>();

        //添加调试输出面板
        //Panel = Container.AddComponent<DebugConsole>();
        //Panel.Init();

        //主摄像机
        MainCamera = GameObject.Find("Main Camera");

        // 初始化随机种子
        UnityEngine.Random.seed = System.Environment.TickCount;
        UnityEngine.Random.seed = System.DateTime.Today.Millisecond;

        // 初始化动画插件(需要注意，这是全局的动画系统，全局设置时不会自动消耗
        // 所有需要主要在游戏切换的时候记得统一销毁
        DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(1000,10);

        // 初始化输入设备
        IOManager.Init(GameConfig.GAME_CONFIG_PLAYER_COUNT);
		Controller.Init(GameController.InputType.External);

        PlayerManager.Init();
    }

    void InitUI()
    {

    }

    /// <summary>
    /// 初始化配置值;
    /// </summary>
    void InitOption()
    {
        // 初始化声音
        SoundController.Init();
        // XML JSON加载
        LoadingManager.Instance.AddJsonFiles(GameConfig.DIFFICULTY_LEVEL_JSON, LevelData.LoadHandler);
        LoadingManager.Instance.AddJsonFiles(GameConfig.MONSTER_REFRESH_JSON, MonsterRefreshData.LoadHandler);
        LoadingManager.Instance.AddJsonFiles(GameConfig.SKILL_EFFECT_JSON, SkillEffectData.LoadHandler);
        LoadingManager.Instance.StartLoad(InitMode);
    }


     /// <summary>
    /// 初始化当前模式状态;在这里注册全部的网络监听消息;
    /// </summary>
   void InitMode()
    {
        updateLog = updateSave = updateFix = 0.0f;
        playerCount = GameConfig.GAME_CONFIG_PLAYER_COUNT;
        GameMode.ReturnStart();
    }

    void OnGUI()
    {

        //if (GUI.Button(new Rect(115, 130, 80, 50), "Load"))
        //{
        //    SoundController.FireEvent("Music_StandBy_Start");
        //}

        //if (GUI.Button(new Rect(115, 200, 80, 50), "1"))
        //{
        //    Time.timeScale -= 0.1f;
        //    Log.Hsz(Time.timeScale);
        //}
    }

    void Update()
    {
        try
        {
            curTime = Time.time;
            float tick = Main.NonStopTime.deltaTime;//Time.fixedDeltaTime;// 
            UpdateFrame();
            UpdateSave();
        }
        catch (Exception e)
        {
            Log.Print("Update Exception StackTrace : " + e.StackTrace);
            Debug.LogError("Update Exception StackTrace : " + e.StackTrace);
        }
    }

    void UpdateFrame()
    {
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_PER_FRAME_UPDATE);
        updateFix += Main.NonStopTime.deltaTime;
        if (updateFix >= 0.3f)
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_FIX_FRAME_UPDATE);
            updateFix = 0.0f;
        }
    }

    void UpdateSave()
    {
        updateLog += Main.NonStopTime.deltaTime;
        // 2秒保修改一次数据（游戏时间和开机时间）
        if (updateLog >= 2.0f)
        {
            if (GameMode.RunState() != RunMode.Start && GameMode.RunState() != RunMode.Setting)
            {
                SettingManager.LogGameTimes(2);
            }
            SettingManager.LogUpTime(2);
            updateLog = 0.0f;
        }

        //// 10秒自动保存一次（主要针对游戏时间和开机时间数据保存）
        //updateSave += Main.NonStopTime.deltaTime;
        //if (updateSave >= 10.0f)
        //{
        //    if (!GameMode.IsPlay())
        //    {
        //        SettingManager.Save();
        //        //RankManager.SaveTopN(-1, true);
        //    }
        //    updateSave = 0;
        //}
    }

    void OnApplicationQuit()
    {
        IOManager.Close();
    }

    /// <summary>
    /// 处理抛出的错误
    /// </summary>
    private void ProcessExceptionReport(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            Log.Print("ExceptionReport:" + condition);
        }
    }

    //修改为私有的，在注册函数里面管理一下;TODO:以下代码考虑之后删除，不用这种Register的方式来注册和移除;
    protected static List<delegateUpdate> updateList = new List<delegateUpdate>();
    //注册更新函数;
    public static void RegisterUpdateCallback(delegateUpdate callback)
    {
        if (updateList.Contains(callback))
        {
            Debug.LogWarning("Main.cs, RegisterUpdateCallback: Sorry, Main's updatelist has contains this item:" + callback.ToString());
            return;
        }
        updateList.Add(callback);
    }

    //反注册更新函数;	
    public static void UnRegisterUpdateCallback(delegateUpdate callback)
    {
        if (!updateList.Contains(callback))
        {
            Debug.LogWarning("Main.cs, UnRegisterUpdateCallback: Sorry, Main's updatelist has not contains this item:" + callback.ToString());
            return;
        }
        updateList.Remove(callback);
    }

    public static int PlayerCount()
    {
        return playerCount;
    }

    public static float FixDeltaTime()
    {
        return updateFix;
    }

   
}
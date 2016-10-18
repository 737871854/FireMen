using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public partial class GameMode : MonoBehaviour
{
    private RunMode mode;
    private string sceneName;
    private int sceneId;
    private float continueTime;
    private float idleTime;
    private bool resetGame;

    void Start()
    {
        sceneId             = 0;
        sceneName           = "";
        mode                = RunMode.Start;
        continueTime        = 0;
        idleTime            = 0;
        resetGame           = false;
        addEvent();
    }

    void OnDestroy()
    {
        removeEvent();
    }

    public RunMode RunState()
    {
        return mode;
    }

    public void RunState(RunMode type)
    {
        mode = type;
    }

    public string SceneName()
    {
        return sceneName;
    }

    public float ContinueTime()
    {
        return continueTime;
    }

    public bool IsPlay()
    {
        if (mode == RunMode.Play || mode == RunMode.Wait)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsWatering()
    {
        return mode == RunMode.Watering;
    }

    public bool IsEndingEdit()
    {
        return mode == RunMode.EndingEdit;
    }

    public bool IsCanJoinGame()
    {
        if (mode == RunMode.EndingEdit || mode == RunMode.EndingTop)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void PauseGame(bool value)
    {
        if (value)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void TimeScale(float value)
    {
        Time.timeScale = value;
    }

    public void UpdatePerFrame()
    {
        gameTime += Time.deltaTime;
        for (int index = 0; index < Main.PlayerCount(); ++index)
        {
            Player player = Main.PlayerManager.getPlayer(index);
            OnPlayerInput(index, ref player);
            OnPlayerLogic(index, ref player);
        }

        // 判断游戏结果
        OnPerGameJudge();
		// 判断游戏结果
		OnFixGameJudge();
        // 进入Idle 
        if (mode == RunMode.Start && idleTime >= GameConfig.GAME_CONFIG_IDLE_TIME)
        {
            GoToIdle();
        }
        idleTime += Time.deltaTime;
    }

    public void UpdateFixFrame()
    {
       
    }

    void OnGUI()
    {
        if (mode == RunMode.Play)
        {
            //GUI.TextArea(new Rect(Screen.width / 2, Screen.width / 2, 120, 20), "Monster Count:" + SceneManager.instance.getMonsterCount());
        }
    }

    /// <summary>
    /// 添加逻辑监听
    /// </summary>
    protected virtual void addEvent()
    {
        EventDispatcher.AddEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE, UpdatePerFrame);
        EventDispatcher.AddEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE, UpdateFixFrame);
        EventDispatcher.AddEventListener<int>(GameEventDef.EVNET_INPUT_COIN, OnEventInputCoin);
        EventDispatcher.AddEventListener<int>(GameEventDef.EVNET_OUTPUT_COIN, OnEventOutputCoin);
        EventDispatcher.AddEventListener<string>(GameEventDef.EVNET_ASYNC_LOADING_COMPLETE, OnEventAsyncLoadingComplete);
        EventDispatcher.AddEventListener(GameEventDef.EVNET_PLAY_MOVIE_ON_COMPLETE, OnEventPlayMovieComplete);
        EventDispatcher.AddEventListener<int>(GameEventDef.EVNET_DAMAGE_LIFE_TIME, OnEventDamageLifeTime);
        EventDispatcher.AddEventListener<int, int>(GameEventDef.EVNET_MONSTER_DEATH, OnEventMonsterDeath);
        EventDispatcher.AddEventListener<string, string, string>(GameEventDef.EVNET_RANK_EDIT_INPUT_COMPLETE, OnEventRankEditInputComplete);
        
    }

    /// <summary>
    /// 移除逻辑事件监听
    /// </summary>
    protected virtual void removeEvent()
    {
        EventDispatcher.RemoveEventListener<int>(GameEventDef.EVNET_INPUT_COIN, OnEventInputCoin);
        EventDispatcher.RemoveEventListener<int>(GameEventDef.EVNET_OUTPUT_COIN, OnEventOutputCoin);
        EventDispatcher.RemoveEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE, UpdatePerFrame);
        EventDispatcher.RemoveEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE, UpdateFixFrame);
        EventDispatcher.RemoveEventListener<string>(GameEventDef.EVNET_ASYNC_LOADING_COMPLETE, OnEventAsyncLoadingComplete);
        EventDispatcher.RemoveEventListener(GameEventDef.EVNET_PLAY_MOVIE_ON_COMPLETE, OnEventPlayMovieComplete);
        EventDispatcher.RemoveEventListener<int>(GameEventDef.EVNET_DAMAGE_LIFE_TIME, OnEventDamageLifeTime);
        EventDispatcher.RemoveEventListener<int, int>(GameEventDef.EVNET_MONSTER_DEATH, OnEventMonsterDeath);
        EventDispatcher.RemoveEventListener<string, string, string>(GameEventDef.EVNET_RANK_EDIT_INPUT_COMPLETE, OnEventRankEditInputComplete);
        
    }

}


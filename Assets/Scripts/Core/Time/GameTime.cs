using System;
using System.Collections.Generic;
using UnityEngine;
using Need.Mx;

public class GameTime : MonoBehaviour
{
    protected bool paused = false;
    protected float gameDeltaTime = 0;
    protected float gameTimeScale = 1;
    protected bool sendPauseEvents = true;
    protected float timeScaleBeforePause = 1;
    

    public bool isPaused
    {
        get
        {
            return paused;
        }

        set
        {
            Pause(value);
        }
    }


    public float deltaTime
    {
        get
        {
            if (!paused)
            {
                return gameDeltaTime * timeScale;
            }
            else
            {
                return 0;
            }
        }
    }

    public float timeScale
    {
        get
        {
            return gameTimeScale;
        }

        set
        {
            if (isPaused)
            {
                timeScaleBeforePause = value;
            }
            else
            {
                gameTimeScale = value;
            }
        }
    }

    void Pause(bool value)
    {
        if (paused == value)
            return;

        if (value)
        {
            timeScaleBeforePause = gameTimeScale;
            gameTimeScale = 0;
        }
        else
        {
            gameTimeScale = timeScaleBeforePause;
        }

        bool pauseValueChanged = (paused != value);

        paused = value;

        if (sendPauseEvents && pauseValueChanged)
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_GAME_TIME_PAUSE);
        }
    }

    void Update()
    {
        gameDeltaTime = Time.deltaTime;// * _timeScale;
    }
}

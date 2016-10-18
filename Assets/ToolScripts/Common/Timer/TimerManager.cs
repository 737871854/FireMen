/**
* Copyright (c) 2015,Need Corp. ltd
* All rights reserved.
* 
* 文件名称：TimerManager.cs
* 简    述：定时器管理类
* 创建标识：Terry  2015/03/31
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Need.Mx
{
    public class TimerManager
    {
        #region Singleton
        protected static TimerManager instance;

        public static TimerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimerManager();
                }
                return instance;
            }
        }

        protected TimerManager()
        {
            TimerList = new List<TimerObject>();
            removeList = new List<TimerObject>();
        }
        #endregion

        private int GuidIndex = 0;
        protected List<TimerObject> TimerList;

        protected List<TimerObject> removeList;

        #region 外部接口;
        /// <summary>
        /// 注册一个定时器;
        /// </summary>
        public int RegisterTimerEx(int start, int end, int trigger, TimerTriggerCallback callback, bool startTrigger = true)
        {
            int guid = GetGuid();
            TimerObject timerObj = new TimerObject(guid, start * 1000, end * 1000, trigger * 1000, callback);
            TimerList.Add(timerObj);
            if (startTrigger)
            {
                //第一次触发一下;
                callback(timerObj);
            }            
            return guid;
        }

        public int RegisterTimer(float start, float end, float trigger, TimerTriggerCallback callback, bool startTrigger = true)
        {
            return RegisterTimerEx((int)start, (int)end, (int)trigger, callback, startTrigger);
        }

        /// <summary>
        /// 注册一个定时器;
        /// </summary>
        public void RegisterTimer(TimerObject timerObj)
        {
            if (!TimerList.Contains(timerObj))
            {
                TimerList.Add(timerObj);
            }
        }

        /// <summary>
        /// 删除一个定时器;
        /// </summary>
        public TimerObject RemoverTimer(int guid)
        {
            TimerObject tobeRemObj = null;
            for (int i = 0; i < TimerList.Count; ++i)
            {
                TimerObject to = TimerList[i];
                if (to.TimerGuid == guid)
                {
                    tobeRemObj = to;
                    break;
                }
            }

            if (tobeRemObj != null)
            {
                TimerList.Remove(tobeRemObj);                
            }

            return tobeRemObj;
        }

        /// <summary>
        /// 删除一个定时器;
        /// </summary>
        public void RemoverTimer(TimerObject timerObj)
        {
            if (TimerList.Contains(timerObj))
            {
                TimerList.Remove(timerObj);
            }
        }

        /// <summary>
        /// 获取一个定时器;
        /// </summary>
        public TimerObject GetTimerObject(int guid)
        {
            TimerObject tobeRemObj = null;
            for (int i = 0; i < TimerList.Count; ++i)
            {
                TimerObject to = TimerList[i];
                if (to.TimerGuid == guid)
                {
                    tobeRemObj = to;
                    break;
                }
            }

            return tobeRemObj;
        }

        /// <summary>
        /// 定时器更新;
        /// </summary>
        public void UpdateAllTimers(float tick)
        {
            int iTick = (int)(tick * 1000);
            for (int i = 0; i < TimerList.Count; ++i)
            {
                TimerObject to = TimerList[i];
                if (to.UpdateTick(iTick))
                {
                    removeList.Add(to);
                }
            }

            for (int i = 0; i < removeList.Count; ++i)
            {
                TimerObject toRem = removeList[i];
                TimerList.Remove(toRem);
            }

            removeList.Clear();
        }
        #endregion

        #region 内部逻辑;

        private int GetGuid()
        {
            return GuidIndex++;
        }
        #endregion
    }
}

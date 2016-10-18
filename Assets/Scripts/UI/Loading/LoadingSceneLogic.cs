using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Need.Mx
{

    public class LoadingSceneLogic : MonoBehaviour
    {
        private LoadingSceneView view;
        private string name;
        void Start()
        {
            view = gameObject.GetComponent<LoadingSceneView>();
            Init();
            addEvent();
            addUIEventListener();
        }

        void Init()
        {
            if (Main.SettingManager.GameLanguage == 0)
            {
                view.image_PercentText_Chinese.gameObject.SetActive(true);
                view.image_PercentText_English.gameObject.SetActive(false);
            }
            else
            {
                view.image_PercentText_Chinese.gameObject.SetActive(false);
                view.image_PercentText_English.gameObject.SetActive(true);
            }
        }

        void OnDestroy()
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_ASYNC_LOADING_COMPLETE, name);
            removeEvent();
        }

        private IEnumerator StartLoading(string name)
        {
            int displayProgress = 0;
            int toProgress = 0;
            AsyncOperation op = Application.LoadLevelAsync(name);
            op.allowSceneActivation = false;
            while (op.progress < 0.9f)
            {
                toProgress = (int)op.progress * 100;
                while (displayProgress < toProgress)
                {
                    ++displayProgress;
                    SetLoadingPercentage(displayProgress);
                    yield return null;
                }
            }

            toProgress = 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                SetLoadingPercentage(displayProgress);
                yield return null;
            }
           
            op.allowSceneActivation = true;
        }

        void SetLoadingPercentage(int displayProgress)
        {
            view.slider_Percent.value = (float)displayProgress / 100.0f;
            if (displayProgress == 100)
            {
                displayProgress = 100;
            }
            int[] value = new int[3];
            int level = 100;
            int count = 0;
            for (int index = 0; index < 3; ++index)
            {
                int number = (displayProgress / level) % 10;
                value[count++] = number;
                level /= 10;
            }

            view.image_NumberSlot1.gameObject.SetActive(true);
            view.image_NumberSlot2.gameObject.SetActive(true);
            view.image_NumberSlot3.gameObject.SetActive(true);

            if (count == 1)
            {
                view.image_NumberSlot3.sprite = view.image_Numbers[value[0]].sprite;
            }
            else if (count == 2)
            {
                view.image_NumberSlot2.sprite = view.image_Numbers[value[0]].sprite;
                view.image_NumberSlot3.sprite = view.image_Numbers[value[1]].sprite;
            }
            else 
            {
                view.image_NumberSlot1.sprite = view.image_Numbers[value[0]].sprite;
                view.image_NumberSlot2.sprite = view.image_Numbers[value[1]].sprite;
                view.image_NumberSlot3.sprite = view.image_Numbers[value[2]].sprite;
            }
            
            view.image_NumberSlot1.SetNativeSize();
            view.image_NumberSlot2.SetNativeSize();
            view.image_NumberSlot3.SetNativeSize();
        }


        void OnEventAsyncLoading(string name)
        {
            this.name = name;
            switch (name)
            {
                case SceneType.Scene1:
                case SceneType.Scene1Start:
                    view.image_Ground1.gameObject.SetActive(true);
                    break;
                case SceneType.Scene2Start:
                case SceneType.Scene2:
                    view.image_Ground2.gameObject.SetActive(true);
                    break;
                case SceneType.Scene3:
                case SceneType.Scene3Start:
                case SceneType.Scene3Over:
                    view.image_Ground3.gameObject.SetActive(true);
                    break;
            }

            //在这里开启一个异步任务,进入StartLoading方法
            StartCoroutine(StartLoading(name));
        }

        /// <summary>
        /// 添加逻辑监听
        /// </summary>
        protected virtual void addEvent()
        {
            EventDispatcher.AddEventListener<string>(GameEventDef.EVNET_ASYNC_LOADING, OnEventAsyncLoading);
        }

        /// <summary>
        /// 添加UI监听
        /// </summary>
        protected virtual void addUIEventListener()
        {

        }

        /// <summary>
        /// 移除逻辑事件监听
        /// </summary>
        protected virtual void removeEvent()
        {
            EventDispatcher.RemoveEventListener<string>(GameEventDef.EVNET_ASYNC_LOADING, OnEventAsyncLoading);
        }

        /// <summary>
        /// 移除UI监听
        /// </summary>
        protected virtual void removeUIEventListener()
        {

        }

    }
}
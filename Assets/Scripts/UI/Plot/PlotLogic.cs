using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Need.Mx
{

    public class PlotLogic : MonoBehaviour
    {

        public MovieTexture movTexture;
        protected PlotView view;
        protected bool isPlaying;

        // Use this for initialization
        void Start()
        {
            view = gameObject.GetComponent<PlotView>();
            view.Init();
            Init();
            //view.image_Movie.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            //view.image_BackGround.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            isPlaying = false;
            movTexture.loop = false;
            addEvent();
            addUIEventListener();
        }

        void Init()
        {
            if (Main.SettingManager.GameLanguage == 0)
            {
                view.plot_skip0.SetActive(true);
                view.plot_skip1.SetActive(false);
            }
            else
            {
                view.plot_skip0.SetActive(false);
                view.plot_skip1.SetActive(true);
            }
        }

        // Update is called once per frame
        public void UpdatePerFrame()
        {

        }

        public void UpdateFixFrame()
        {
            if (isPlaying && !movTexture.isPlaying)
            {
                isPlaying = false;
                movTexture.Stop();
                AudioSource audio = GetComponent<AudioSource>();
                audio.Stop();
                view.image_Movie.gameObject.SetActive(true);
                view.image_BackGround.gameObject.SetActive(true);
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_PLAY_MOVIE_ON_COMPLETE);
            }
        }

        void OnDestroy()
        {
            removeEvent();
        }

        public void OnEventPlayMovie()
        {
            isPlaying = true;
            movTexture.Play();
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = movTexture.audioClip;
            audio.Play();
            audio.volume = (float)Main.SettingManager.GameVolume * 0.1f;
            view.image_Movie.texture = movTexture;
            view.image_Movie.gameObject.SetActive(true);
            view.image_BackGround.gameObject.SetActive(true);
        }

        /// <summary>
        /// 添加逻辑监听
        /// </summary>
        protected virtual void addEvent()
        {
            EventDispatcher.AddEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE,   UpdatePerFrame);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE,   UpdateFixFrame);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_PLAY_MOVIE,         OnEventPlayMovie);
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
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE,    UpdatePerFrame);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE,    UpdateFixFrame);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_PLAY_MOVIE,          OnEventPlayMovie);
        }

        /// <summary>
        /// 移除UI监听
        /// </summary>
        protected virtual void removeUIEventListener()
        {

        }
    }
}
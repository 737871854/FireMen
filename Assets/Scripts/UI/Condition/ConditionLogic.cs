using UnityEngine;
using System.Collections;

namespace Need.Mx
{
    public class ConditionLogic : MonoBehaviour
    {

        protected ConditionView view;

        // Use this for initialization
        void Start()
        {
            view = gameObject.GetComponent<ConditionView>();
            addEvent();
            addUIEventListener();
        }

        void OnDestroy()
        {
            removeEvent();
        }


        void OnEventShow(string typeName)
        {
            view.image_Ground0.gameObject.SetActive(true);
            if (typeName == "Score")
            {
                if (Main.SettingManager.GameLanguage == 0)
                {
                    view.image_Ground1.gameObject.SetActive(true);
                    view.image_Ground1_En.gameObject.SetActive(false);
                }
                else
                {
                    view.image_Ground1.gameObject.SetActive(false);
                    view.image_Ground1_En.gameObject.SetActive(true);
                }
                view.image_Scores.gameObject.SetActive(true);   
            }
            else if (typeName == "Boss")
            {
                if (Main.SettingManager.GameLanguage == 0)
                {
                    view.image_Ground2.gameObject.SetActive(true);
                    view.image_Ground2_En.gameObject.SetActive(false);
                }
                else
                {
                    view.image_Ground2.gameObject.SetActive(false);
                    view.image_Ground2_En.gameObject.SetActive(true);
                }
            }
        }

        void OnEventClose()
        {
            view.image_Ground0.gameObject.SetActive(false);
            view.image_Ground1.gameObject.SetActive(false);
            view.image_Ground2.gameObject.SetActive(false);
            view.image_Ground2_En.gameObject.SetActive(false);
            view.image_Ground1_En.gameObject.SetActive(false);
            view.image_Scores.SetActive(false);
        }

        /// <summary>
        /// 添加逻辑监听
        /// </summary>
        protected virtual void addEvent()
        {
            EventDispatcher.AddEventListener<string>(GameEventDef.EVNET_CONDITION_SHOW, OnEventShow);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_CONDITION_CLOSE, OnEventClose);
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
            EventDispatcher.RemoveEventListener<string>(GameEventDef.EVNET_CONDITION_SHOW, OnEventShow);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_CONDITION_CLOSE, OnEventClose);
        }

        /// <summary>
        /// 移除UI监听
        /// </summary>
        protected virtual void removeUIEventListener()
        {

        }

    }

}
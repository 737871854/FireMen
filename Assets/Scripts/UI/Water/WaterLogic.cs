using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;
using DG.Tweening.Core;
using Need.Mx;

namespace Need.Mx
{

    public class WaterLogic : MonoBehaviour
    {
        // <summary>
        /// View类变量
        /// </summary>
        protected WaterView view;
        protected float minSplashHeight;
        protected float maxSplashHeight;
        protected Tweener twCautionRed;
        protected int pushCount;
        protected int pullCount;
        protected float pullValue;
        protected float pullMaxTime;

        protected List<Image> goShowList;
        protected List<Image> goHideList;

        const int MIN_SPLASH_HEIGHT = -375;
        const int MAX_SPLASH_HEIGHT =  500;
        const int MIN_PULL_HEIGHT   = -250;
        const int MIN_HIDE_HEIGHT   = -600;
        const int MIN_LEFT_X        =  250;
        const int MAX_LEFT_X        =  434;
        const int MIN_RIGHT_X       =  434;
        const int MAX_RIGHT_X       =  618;
        const int PER_STEP_Y        =  75;

		protected float waitTime    = 30;
        // Use this for initialization
        void Start()
        {
            pullValue       = MIN_PULL_HEIGHT;
            minSplashHeight = MIN_SPLASH_HEIGHT;
            maxSplashHeight = MAX_SPLASH_HEIGHT;
            view = gameObject.GetComponent<WaterView>();
            view.Init();
            addEvent();
            addUIEventListener();
            Init();
            view.panel_Tutorial.gameObject.SetActive(false);
            view.panel_Handle.gameObject.SetActive(false);
            view.image_Reward.gameObject.SetActive(false);
            goShowList = new List<Image>();
            goHideList = new List<Image>();
        }

        //void OnGUI()
        //{
        //    if (GUI.Button(new Rect(115, 130, 80, 50), "Load"))
        //    {
        //        OnEventWaterPushing();
        //    }
        //}

        //void Update()
        //{
        //    UpdatePerFrame();
        //}
       
        //}

        //}
        //static float count = 0.0f;
        void Init()
        {
            if (Main.SettingManager.GameLanguage == 0)
            {
                view.panel_Tutorial.sprite = view.image_water_backround_chinese.sprite;
            }
            else
            {
                view.panel_Tutorial.sprite = view.image_water_backround_english.sprite;
            }
        }

        // Update is called once per frame
        public void UpdatePerFrame()
        {
            OnUpdateGround();

			waitTime -= Main.NonStopTime.noStopTime;

			if (waitTime < 0) 
			{
				return;
			}

			int valu10 = (int)((waitTime / 10) % 10);
			int value1 = (int)((waitTime % 10));
			view.image_number0.sprite = view.numList [valu10].sprite;
			view.image_number1.sprite = view.numList [value1].sprite;

			if (waitTime <= 0)
			{
				view.slider_Progress.value = 1;
			}
        }

        public void UpdateFixFrame()
        {
            OnUpdateWater();
        }

        void OnCheckPullTextHideEnd()
        {
            if (goHideList.Count > 0)
            {
                GameObject.Destroy(goHideList[0].gameObject);
                goHideList.RemoveAt(0);
            }
        }

        public void OnUpdateGround()
        {
            AnimatorStateInfo man = view.animator_WaterMan.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo column = view.animator_WaterColumn.GetCurrentAnimatorStateInfo(0);
            if (man.IsName("Play") && man.normalizedTime >= 1.0f && column.IsName("Play") && column.normalizedTime >= 1.0f)
            {
                if (pushCount <= 0)
                {
                    view.animator_WaterColumn.SetInteger("Flag", 0);
                    view.animator_WaterMan.SetInteger("Flag", 0);
                }
                else
                {
                    --pushCount;
                }
            }
            else if ((man.IsName("Wait") || man.IsName("Idle")) && column.IsName("Idle") && pushCount > 0)
            {
                view.animator_WaterColumn.SetInteger("Flag", 1);
                view.animator_WaterMan.SetInteger("Flag", 1);
                --pushCount;
            }
        }

        public void OnUpdateWater()
        {
            if (Main.GameMode.RunState() != RunMode.Watering)
            {
                return;
            }

            float percent = Main.GameMode.TotalPushWaterPercent();
            //float percent = count / 60.0f;
            view.slider_Progress.value = percent;
            float posY = maxSplashHeight * percent;
            Vector3 pos = view.image_Splash.rectTransform.anchoredPosition;
            pos.y = minSplashHeight + posY;
            view.image_Splash.rectTransform.anchoredPosition = pos;		
        }

        public void OnEventWaterEmptyCaution()
        {
			 waitTime  = 30;
             pushCount = 0;
             pullCount = 0;
             pullValue = MIN_PULL_HEIGHT;
             view.panel_Tutorial.gameObject.SetActive(true);
             view.panel_Handle.gameObject.SetActive(true);
             if (Main.SettingManager.GameLanguage == 0)
             {
                 view.image_pullHint.gameObject.SetActive(true);
                 view.image_pullHint_En.gameObject.SetActive(false);
             }
             else
             {
                 view.image_pullHint.gameObject.SetActive(false);
                 view.image_pullHint_En.gameObject.SetActive(true);
             }
             view.slider_Progress.value = 0.0f;
             Vector3 pos = view.image_Splash.rectTransform.anchoredPosition;
             pos.y = minSplashHeight;
             view.image_Splash.rectTransform.anchoredPosition = pos;
             Color color = view.panel_Red.color;
             color.a = 1.0f;
             twCautionRed = view.panel_Red.DOColor(color, 0.5f);
             twCautionRed.SetLoops(-1, LoopType.Yoyo);
             twCautionRed.Play();
        }

        public void OnEventWatering()
        {
            view.panel_Tutorial.gameObject.SetActive(false);
        }

        public void OnEventWaterFull()
        {
            DOTween.Kill(twCautionRed);
            view.panel_Handle.gameObject.SetActive(false);
        }

        public void OnEventWaterPushing(bool reward, bool pull)
        {
            ++pushCount;
            if (pull)
            {
                Image create = GameObject.Instantiate(view.image_PullText) as Image;
                int remain = goShowList.Count % 2;
                float scale = 0.5f + ((goShowList.Count / 10.0f) * 0.5f);
                float pullPosX = 0;
                float pullPosY = 0;
                if (remain == 0)
                {
                    pullValue += 100.0f;
                    pullPosX = Random.Range(MIN_LEFT_X, MAX_LEFT_X);
                    pullPosY = Random.Range(pullValue, pullValue + PER_STEP_Y);
                }
                else
                {
                    pullPosX = Random.Range(MIN_RIGHT_X, MAX_RIGHT_X);
                    pullPosY = Random.Range(pullValue, pullValue + PER_STEP_Y);
                }
                float angle = Random.Range(-30.0f, 30.0f);
                create.rectTransform.Rotate(Vector3.forward, angle);
                create.gameObject.SetActive(true);
                create.rectTransform.SetParent(view.panel_Handle.rectTransform);
                create.rectTransform.anchoredPosition = new Vector2(pullPosX, pullPosY);
                create.rectTransform.localScale = new Vector3(scale, scale, scale);
                goShowList.Add(create);
            }

            if (reward)
            {
                // 显示奖励
                for (int index = 0; index < goShowList.Count; ++index)
                {
                    GameObject.Destroy(goShowList[index].gameObject);
                    pullValue = MIN_PULL_HEIGHT;
                }
                goShowList.Clear();

                StartCoroutine(OnShowTimeReward());
                
            }
        }
        
        public void OnEventWaterUrging()
        {
            view.animator_WaterMan.SetInteger("Flag", 2);
            //goHideList.AddRange(goShowList);
            //for (int index = 0; index < goShowList.Count; ++index)
            //{
            //    Sequence sequence = DOTween.Sequence();
            //    Vector2 pos = goShowList[index].rectTransform.anchoredPosition;
            //    pos.y = MIN_HIDE_HEIGHT;
            //    sequence.Append(goShowList[index].rectTransform.DOAnchorPos(pos, 0.5f));
            //    sequence.Join(goShowList[index].DOColor(Color.gray, 0.25f));
            //    sequence.Play();
            //    sequence.OnComplete(OnCheckPullTextHideEnd);

            //}
            pullMaxTime = 0;
            //pullValue = MIN_PULL_HEIGHT;
            //goShowList.Clear();
        }


        IEnumerator OnShowTimeReward()
        {
            float showTime = 1.0f;
            while(showTime > 0)
            {
                if (Main.SettingManager.GameLanguage == 0)
                {
                    view.image_Reward.gameObject.SetActive(true);
                }
                else
                {
                    view.image_Reward_En.gameObject.SetActive(true);
                }
                showTime -= Main.NonStopTime.deltaTime;
                yield return null;
            }

            view.image_Reward.gameObject.SetActive(false); 
            view.image_Reward_En.gameObject.SetActive(false);
        }

        /// <summary>
        /// 添加逻辑监听
        /// </summary>
        protected virtual void addEvent()
        {
            EventDispatcher.AddEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE,           UpdatePerFrame);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE,           UpdateFixFrame);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_WATER_EMPTY_CAUTION,        OnEventWaterEmptyCaution);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_WATER_WATERING,             OnEventWatering);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_WATER_FULL,                 OnEventWaterFull);
            EventDispatcher.AddEventListener<bool,bool>(GameEventDef.EVNET_WATER_PUSHING,   OnEventWaterPushing);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_WATER_URGING,               OnEventWaterUrging);
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
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE,            UpdatePerFrame);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE,            UpdateFixFrame);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_WATER_EMPTY_CAUTION,         OnEventWaterEmptyCaution);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_WATER_WATERING,              OnEventWatering);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_WATER_FULL,                  OnEventWaterFull);
            EventDispatcher.RemoveEventListener<bool, bool>(GameEventDef.EVNET_WATER_PUSHING,   OnEventWaterPushing);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_WATER_URGING,                OnEventWaterUrging);
        }

        /// <summary>
        /// 移除UI监听
        /// </summary>
        protected virtual void removeUIEventListener()
        {

        }
    }
}
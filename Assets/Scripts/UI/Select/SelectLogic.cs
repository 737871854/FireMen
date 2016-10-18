using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Need.Mx
{

    public class SelectLogic : MonoBehaviour
    {

        // <summary>
        /// View类变量
        /// </summary>
        protected SelectView view;

        /// <summary>
        /// 倒计时变量
        /// </summary>
        protected float remainTime;
        protected bool start;

        // Use this for initialization
        void Start()
        {
            remainTime = GameConfig.GAME_CONFIG_SELECT_WAIT_TIME;
            start = false;
            view = gameObject.GetComponent<SelectView>();
            view.Init();
            Init();
            addEvent();
            addUIEventListener();
            Main.SoundController.PlayCountDownSound();
            Main.SoundController.PlayGetReadySound();
            Main.SoundController.PlayWelcomeSound();
        }

        void OnDestroy()
        {
            removeEvent();
            removeUIEventListener();
        }

        void Init()
        {
            if (Main.SettingManager.GameLanguage == 0)
            {
                view.image_coin_chinese0.gameObject.SetActive(true);
                view.image_coin_chinese1.gameObject.SetActive(true);
                view.image_coin_chinese2.gameObject.SetActive(true);
                view.image_P1RoleGrey.gameObject.SetActive(true);
                view.image_P2RoleGrey.gameObject.SetActive(true);
                view.image_P3RoleGrey.gameObject.SetActive(true);
                view.image_coin_english0.gameObject.SetActive(false);
                view.image_coin_english1.gameObject.SetActive(false);
                view.image_coin_english2.gameObject.SetActive(false);
                view.image_wait_english0.gameObject.SetActive(false);
                view.image_wait_english1.gameObject.SetActive(false);
                view.image_wait_english2.gameObject.SetActive(false);
            }
            else
            {
                view.image_coin_chinese0.gameObject.SetActive(false);
                view.image_coin_chinese1.gameObject.SetActive(false);
                view.image_coin_chinese2.gameObject.SetActive(false);
                view.image_P1RoleGrey.gameObject.SetActive(false);
                view.image_P2RoleGrey.gameObject.SetActive(false);
                view.image_P3RoleGrey.gameObject.SetActive(false);
                view.image_coin_english0.gameObject.SetActive(true);
                view.image_coin_english1.gameObject.SetActive(true);
                view.image_coin_english2.gameObject.SetActive(true);
                view.image_wait_english0.gameObject.SetActive(true);
                view.image_wait_english1.gameObject.SetActive(true);
                view.image_wait_english2.gameObject.SetActive(true);
            }
        }

        protected virtual void addEvent()
        {
            EventDispatcher.AddEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE, UpdatePerFrame);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE, UpdateFixFrame);
            EventDispatcher.AddEventListener<bool>(GameEventDef.EVENT_WATER_HIGHT, OnHightWater);
            EventDispatcher.AddEventListener<bool>(GameEventDef.EVENT_WATER_LOW, OnLowWater);
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
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE, UpdatePerFrame);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE, UpdateFixFrame);
            EventDispatcher.RemoveEventListener<bool>(GameEventDef.EVENT_WATER_HIGHT, OnHightWater);
            EventDispatcher.RemoveEventListener<bool>(GameEventDef.EVENT_WATER_LOW, OnLowWater);
        }

        /// <summary>
        /// 移除UI监听
        /// </summary>
        protected virtual void removeUIEventListener()
        {

        }

        public void UpdatePerFrame()
        {
            if (start == false)
            {
                remainTime -= Main.NonStopTime.deltaTime;
            }
        }

        private void OnHightWater(bool flag)
        {
            view.hight_water.SetActive(flag);
        }

        private void OnLowWater(bool flag)
        {
            view.low_water.SetActive(flag);
        }

        public void UpdateFixFrame()
        {
            {
                if (start)
                {
                    Main.SoundController.PlayGetStartSound();
                    StartCoroutine(OnWaitGetStartSound());
                    return;
                }

                if (remainTime <= 0 && !start)
                {
                    remainTime = 0;
                    start = true;
                }

                int value = (int)remainTime;
                int number1 = (value / 10) % 10;
                int number2 = (value / 1) % 10;
                view.image_Time1.sprite = view.image_Times[number1].sprite;
                view.image_Time2.sprite = view.image_Times[number2].sprite;
                Vector2 size = new Vector2(119, 167);
                view.image_Time1.rectTransform.sizeDelta = size;
                view.image_Time2.rectTransform.sizeDelta = size;
            }

            {
                Player player = Main.PlayerManager.getPlayer(0);
                int value = player.Coin;
                int number1 = (value / 10) % 10;
                int number2 = (value / 1) % 10;
                view.image_P1CoinNumber1.sprite = view.image_Numbers[number1].sprite;
                view.image_P1CoinNumber2.sprite = view.image_Numbers[number2].sprite;
                view.image_P1CoinNumber3.sprite = view.image_Numbers[GameConfig.GAME_CONFIG_PER_USE_COIN].sprite;
                if (value > 9)
                {
                    view.image_P1CoinNumber1.gameObject.SetActive(true);
                }
                else
                {
                    view.image_P1CoinNumber1.gameObject.SetActive(false);
                }

                if (player.State == Player.StateType.Idle)
                {
                    if (Main.SettingManager.GameLanguage == 0)
                    {
                        view.image_P1RoleGrey.gameObject.SetActive(true);
                        view.image_wait_english0.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.image_P1RoleGrey.gameObject.SetActive(false);
                        view.image_wait_english0.gameObject.SetActive(true);
                    }
                  
                    view.image_P1Role.gameObject.SetActive(false);                  
                }
                else if (player.State == Player.StateType.Play)
                {
                    if (Main.SettingManager.GameLanguage == 0)
                    {
                        view.image_P1RoleGrey.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.image_wait_english0.gameObject.SetActive(false);
                    }
                    view.image_P1Role.gameObject.SetActive(true);
                    view.image_P1Role.rectTransform.position = view.image_P1RoleGrey.rectTransform.position;
                }
            }

            {
                Player player = Main.PlayerManager.getPlayer(1);
                int value = player.Coin;
                int number1 = (value / 10) % 10;
                int number2 = (value / 1) % 10;
                view.image_P2CoinNumber1.sprite = view.image_Numbers[number1].sprite;
                view.image_P2CoinNumber2.sprite = view.image_Numbers[number2].sprite;
                view.image_P2CoinNumber3.sprite = view.image_Numbers[GameConfig.GAME_CONFIG_PER_USE_COIN].sprite;
                if (value > 9)
                {
                    view.image_P2CoinNumber1.gameObject.SetActive(true);
                }
                else
                {
                    view.image_P2CoinNumber1.gameObject.SetActive(false);
                }

                if (player.State == Player.StateType.Idle)
                {
                    if (Main.SettingManager.GameLanguage == 0)
                    {
                        view.image_P2RoleGrey.gameObject.SetActive(true);
                        view.image_wait_english1.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.image_P2RoleGrey.gameObject.SetActive(false);
                        view.image_wait_english1.gameObject.SetActive(true);
                    }
                    view.image_P2Role.gameObject.SetActive(false);
                }
                else if (player.State == Player.StateType.Play)
                {
                    if (Main.SettingManager.GameLanguage == 0)
                    {
                        view.image_P2RoleGrey.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.image_wait_english1.gameObject.SetActive(false);
                    }
                    view.image_P2Role.gameObject.SetActive(true);
                    view.image_P2Role.rectTransform.position = view.image_P2RoleGrey.rectTransform.position;
                }
            }

            {
                Player player = Main.PlayerManager.getPlayer(2);
                int value = player.Coin;
                int number1 = (value / 10) % 10;
                int number2 = (value / 1) % 10;
                view.image_P3CoinNumber1.sprite = view.image_Numbers[number1].sprite;
                view.image_P3CoinNumber2.sprite = view.image_Numbers[number2].sprite;
                view.image_P3CoinNumber3.sprite = view.image_Numbers[GameConfig.GAME_CONFIG_PER_USE_COIN].sprite;
                if (value > 9)
                {
                    view.image_P3CoinNumber1.gameObject.SetActive(true);
                }
                else
                {
                    view.image_P3CoinNumber1.gameObject.SetActive(false);
                }
                if (player.State == Player.StateType.Idle)
                {
                    if (Main.SettingManager.GameLanguage == 0)
                    {
                        view.image_P3RoleGrey.gameObject.SetActive(true);
                        view.image_wait_english2.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.image_P3RoleGrey.gameObject.SetActive(false);
                        view.image_wait_english2.gameObject.SetActive(true);
                    }
                    view.image_P3Role.gameObject.SetActive(false);
                }
                else if (player.State == Player.StateType.Play)
                {
                    if (Main.SettingManager.GameLanguage == 0)
                    {
                        view.image_P3RoleGrey.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.image_wait_english2.gameObject.SetActive(false);
                    }
                    view.image_P3Role.gameObject.SetActive(true);
                    view.image_P3Role.rectTransform.position = view.image_P3RoleGrey.rectTransform.position;
                }
            }
        }

        IEnumerator OnWaitGetStartSound()
        {
            yield return new WaitForSeconds(2);
            Main.GameMode.NextLevel();
        }
    }
}
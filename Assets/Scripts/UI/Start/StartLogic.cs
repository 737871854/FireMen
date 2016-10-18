using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Need.Mx
{
    public class StartLogic : MonoBehaviour
    {

        // <summary>
        /// View类变量
        /// </summary>
        protected StartView view;

        private float time = 1;

        // Use this for initialization
        void Start()
        {
            view = gameObject.GetComponent<StartView>();
            view.Init();
            Init();
            addEvent();
            addUIEventListener();
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
                view.image_panel0_coin_chinese.gameObject.SetActive(true);
                view.image_panel1_coin_chinese.gameObject.SetActive(true);
                view.image_panel2_coin_chinese.gameObject.SetActive(true);
                view.image_logo_chinese.gameObject.SetActive(true);
                view.image_panel0_coin_english.gameObject.SetActive(false);
                view.image_panel1_coin_english.gameObject.SetActive(false);
                view.image_panel2_coin_english.gameObject.SetActive(false);
                view.image_logo_english.gameObject.SetActive(false);
            }
            else
            {
                view.image_panel0_coin_chinese.gameObject.SetActive(false);
                view.image_panel1_coin_chinese.gameObject.SetActive(false);
                view.image_panel2_coin_chinese.gameObject.SetActive(false);
                view.image_logo_chinese.gameObject.SetActive(false);
                view.image_panel0_coin_english.gameObject.SetActive(true);
                view.image_panel1_coin_english.gameObject.SetActive(true);
                view.image_panel2_coin_english.gameObject.SetActive(true);
                view.image_logo_english.gameObject.SetActive(true);
            }
        }

        protected virtual void addEvent()
        {
            EventDispatcher.AddEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE, UpdatePreFrame);
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
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE, UpdatePreFrame);
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

        private void OnHightWater(bool flag)
        {
            view.hight_water.SetActive(flag);
        }

        private void OnLowWater(bool flag)
        {
            view.low_water.SetActive(flag);
        }

        public void UpdatePreFrame()
        {
            //Text text0 = transform.parent.transform.Find("Text0").GetComponent<Text>();
            //text0.text = "语言版本:   " + Main.SettingManager.GameLanguage;
            
            ////测试用
            //if (time <= 0)
            //{
            //    time = 2;
            //    GameConfig.GAME_CONFIG_ID = PlayerPrefs.GetString(GameConfig.GAME_CONFIG_CHECK_ID);
            //    Text text0 = transform.parent.transform.Find("Text0").GetComponent<Text>();
            //    text0.text = "保存的CheckID:   " + GameConfig.GAME_CONFIG_ID;

            //    Text text1 = transform.parent.transform.Find("Text1").GetComponent<Text>();
            //    text1.text = "收到5A: " + Main.IOManager.Receice5A.ToString();
         

            //    Text text3 = transform.parent.transform.Find("Text3").GetComponent<Text>();
            //    text3.text = "CheckPass:   " + Main.IOManager.CheckPass.ToString();

            //    Text text4 = transform.parent.transform.Find("Text4").GetComponent<Text>();
            //    text4.text = "找到端口:   " + Main.IOManager.GetFindProt();

            //    Text text5 = transform.parent.transform.Find("Text5").GetComponent<Text>();
            //    text5.text = "找端口次数:   " + Main.IOManager.GetConnectCount().ToString();
            //}
            //else
            //{
            //    time -= Time.deltaTime;
            //}

            //Text text6 = transform.parent.transform.Find("Text6").GetComponent<Text>();
            //text6.text = "Check倒计时:     " + Main.IOManager.CheckTime.ToString();

            //string f5 = string.Empty;
            //string aa = string.Empty;
            //for (int i = 0; i < Main.IOManager.ByteHost.Length; i++ )
            //{
            //    f5 = f5 + Main.IOManager.ByteHost[i].ToString() + "/";
            //}

            //Text text7 = transform.parent.transform.Find("Text7").GetComponent<Text>();
            //text7.text = "5A协议数据 共" + Main.IOManager.ByteHost.Length.ToString() + "位"  + ":     " + f5;

            //for (int i = 0; i < Main.IOManager.ByteHostAA.Length; i++ )
            //{
            //    aa = aa + Main.IOManager.ByteHostAA[i].ToString() + "/";
            //}

            //Text text2 = transform.parent.transform.Find("Text2").GetComponent<Text>();
            //text2.text = "AA协议数据 共" + Main.IOManager.ByteHostAA.Length.ToString() + "位" + ":     " + aa;
        }

        public void UpdateFixFrame()
        {
           

            {
                Player player = Main.PlayerManager.getPlayer(0);
                int value   = player.Coin;
                int number1 = (value / 10) % 10;
                int number2 = (value /  1) % 10;
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
            }
        }
    }
}

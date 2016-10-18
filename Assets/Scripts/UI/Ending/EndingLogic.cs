using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

namespace Need.Mx
{

    public class EndingLogic : MonoBehaviour
    {


        protected EndingView view;

        // Use this for initialization
        void Start()
        {
            view = gameObject.GetComponent<EndingView>();
            addEvent();
            addUIEventListener();            
        }

        void OnDestroy()
        {
            removeEvent();
        }

        //public void UpdatePerFrame()
        //{
           
        //}

        //public void UpdateFixFrame()
        //{

        //}

        //void OnGUI()
        //{

        //    if (GUI.Button(new Rect(115, 130, 80, 50), "Load"))
        //    {
        //        OnEventShow();
        //    }
        //}

        public void OnEventShow()
        {
            if (Main.GameMode.RunState() == RunMode.EndingLoss)
            {
                if (Main.SettingManager.GameLanguage == 0)
                {
                    view.image_GroundLoss.gameObject.SetActive(true);
                    view.animator_GroundLoss.SetBool("Flag", false);
                }
                else
                {
                    view.image_GroundLoss_En.gameObject.SetActive(true);
                    view.animator_GroundLoss_En.SetBool("Flag", false);
                }
               
            }
            else
            {
                if (Main.SettingManager.GameLanguage == 0)
                {
                    view.image_GroundSuccess.gameObject.SetActive(true);
                    view.animator_GroundSuccess.SetBool("Flag", true);
                }
                else
                {
                    view.image_GroundLoss_En.gameObject.SetActive(true);
                    view.animator_GroundSuccess_En.SetBool("Flag", true);
                }
                view.animator_GroundSuccessEffect.SetBool("Flag", true);
                view.image_GroundSuccessEffect.gameObject.SetActive(true);
            }
            StartCoroutine(OnShowScore());           
        }

        IEnumerator OnShowScore()
        {
            yield return new WaitForSeconds(0.5f);
            List<Player> players = new List<Player>();
            for (int index = 0; index < Main.PlayerCount(); ++index)
            {
                players.Add(Main.PlayerManager.getPlayer(index));
            }
            players.Sort(delegate(Player a, Player b) { return a.Score.CompareTo(b.Score); });
            players.Reverse();
            Main.SoundController.PlayEndingAccountSound();
            Vector2 size = new Vector2(37, 52);
            while (true)
            {
                bool  counting    = false;
                float countTime   = 0.0f;
                int   randomScore = Random.Range(1,1000);
                if (countTime > 2.5f)
                {
                    randomScore = 99999;
                }
                for (int index = 0; index < Main.PlayerCount(); ++index)
                {
                    EndingView.RankPanel rank = view.image_Ranks[index];
                    Player player = players[index];
                    rank.image_Head.sprite = view.image_Heads[player.Index].sprite;

                    int remaindScore = player.Score - player.LastScore;
                    if (remaindScore > 0 )
                    {
                        if(remaindScore < randomScore)
                        {
                            player.LastScore = player.LastScore + remaindScore;
                        }
                        else
                        {
                            player.LastScore = player.LastScore + randomScore;
                            counting = true;
                        }
                    }

                    int value = (int)player.LastScore;
                    int number0 = (value / 10000) % 10;
                    int number1 = (value / 1000) % 10;
                    int number2 = (value / 100) % 10;
                    int number3 = (value / 10) % 10;
                    int number4 = (value / 1) % 10;

                    rank.image_NumberSlot1.sprite = view.image_Numbers[number4].sprite;
                    rank.image_NumberSlot2.sprite = view.image_Numbers[number3].sprite;
                    rank.image_NumberSlot3.sprite = view.image_Numbers[number2].sprite;
                    rank.image_NumberSlot4.sprite = view.image_Numbers[number1].sprite;
                    rank.image_NumberSlot5.sprite = view.image_Numbers[number0].sprite;
                    rank.image_NumberSlot1.rectTransform.sizeDelta = size;
                    rank.image_NumberSlot2.rectTransform.sizeDelta = size;
                    rank.image_NumberSlot3.rectTransform.sizeDelta = size;
                    rank.image_NumberSlot4.rectTransform.sizeDelta = size;
                    rank.image_NumberSlot5.rectTransform.sizeDelta = size;
                    rank.go.SetActive(true);
                }
                if (counting)
                {
                    yield return new WaitForEndOfFrame();
                    countTime += Main.NonStopTime.deltaTime;
                }
                else
                {
                    break;
                }
            }
            Main.SoundController.StopEndingAccountSound();
        }

        /// <summary>
        /// 添加逻辑监听
        /// </summary>
        protected virtual void addEvent()
        {
            //EventDispatcher.AddEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE, UpdatePerFrame);
            //EventDispatcher.AddEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE, UpdateFixFrame);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_ENDING_SHOW, OnEventShow);
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
            //EventDispatcher.RemoveEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE, UpdatePerFrame);
            //EventDispatcher.RemoveEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE, UpdateFixFrame);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_ENDING_SHOW, OnEventShow);
        }

        /// <summary>
        /// 移除UI监听
        /// </summary>
        protected virtual void removeUIEventListener()
        {

        }
    }
}

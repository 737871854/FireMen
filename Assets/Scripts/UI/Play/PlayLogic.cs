using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

namespace Need.Mx
{

    public class PlayLogic : MonoBehaviour
    {
        public class PlayerUIParam
        {
            public int curScore = 0;
            public int addScore = 0;
            public float addTime = 0.0f;
            public bool playinngScale = false;
            public Tween twGround = null;
        }

        // <summary>
        /// View类变量
        /// </summary>
        protected PlayView view;
        protected List<PlayerUIParam> uiParamList;
        protected List<int> uiNumberScaled;
        
        // Use this for initialization
        void Start()
        {
            uiParamList = new List<PlayerUIParam>(GameConfig.GAME_CONFIG_PLAYER_COUNT);
            uiNumberScaled = new List<int>();
            for (int index = 0; index < Main.PlayerCount(); ++index)
            {
                uiParamList.Add(new PlayerUIParam());
                Player player = Main.PlayerManager.getPlayer(index);
                if (player == null)
                {
                    continue;
                }
                uiParamList[index].curScore = player.Score;
            }

            view = gameObject.GetComponent<PlayView>();
            view.Init();
            Init();
            addEvent();
            addUIEventListener();    
        }

        void Init()
        {
            if (Main.SettingManager.GameLanguage == 0)
            {
                view.image_ContinueText.gameObject.SetActive(true);
                view.image_text_english.gameObject.SetActive(false);
            }
            else
            {
                view.image_ContinueText.gameObject.SetActive(false);
                view.image_text_english.gameObject.SetActive(true);
            }
        }

        void OnDestroy()
        {
            for (int i = 0; i < GameConfig.GAME_CONFIG_PLAYER_COUNT;i++ )
            {
                Main.IOManager.SetPlayerGameBegine(i,false);
            }
            removeEvent();
        }
        // Update is called once per frame
        public void UpdatePerFrame()
        {
            for (int index = 0; index < uiParamList.Count; ++index)
            {
                Player player = Main.PlayerManager.getPlayer(index);
                if (player == null)
                {
                    return;
                }
                PlayerUpdateTarget(index, player);
                PlayerUpdateScore(index, player);
            }

            if (!(Main.GameMode.SceneName() == SceneType.Scene3))
            {
                UpdatePassPrpgress();
                view.image_Pass.gameObject.SetActive(true);
            }
            else
            {
                view.image_Pass.gameObject.SetActive(false);
            }
        }

        public void UpdateFixFrame()
        {
            for (int index = 0; index < uiParamList.Count; ++index)
            {
                Player player = Main.PlayerManager.getPlayer(index);
                if (player == null)
                {
                    return;
                }

                PlayerUpdateTimer(index, player);
                PlayerUpdateContinue(index, player);
                PlayerUpdateCoin(index, player);
                PlayerUpdateWarning(index, player);
            }

            UpdateBigContinue();
            //UpdateWater();
        }

        // AOE变化
        //void OnEventUpdateAoe()
        //{
        //    view.image_AoeBar.value = Main.GameMode.TotalAoePercent();
        //    AnimatorStateInfo info = view.animator_AOEFlash.GetCurrentAnimatorStateInfo(0);
        //    if (info.IsName("Idle"))
        //    {
        //        view.animator_AOEFlash.SetInteger("Flag", 1);
        //        view.Image_AoeSlotFlash.gameObject.SetActive(true);
        //    }
        //}

        void OnEventAppendScores(int index, int value)
        {
            PlayerUIParam uiParam = (PlayerUIParam)uiParamList[index];
            uiParam.addScore += value;
        }

        void OnEventUpdateAoe()
        {
            if (Main.GameMode.SceneName() == SceneType.Scene3)
            {
                return;
            }
            float rate = 0.0f;
            if (Main.GameMode.CheckPoint == 1)
            {
                rate = (float)Main.GameMode.TotalScore / (float)Main.GameMode.getConditionScore(1);
                rate *= 0.26f;
            }
            else if (Main.GameMode.CheckPoint == 2)
            {
                int value = (Main.GameMode.TotalScore - Main.GameMode.getConditionScore(1));
                if (value < 0)
                    value = 0;
                rate = (float)value / ((float)Main.GameMode.getConditionScore(2) - (float)Main.GameMode.getConditionScore(1));
                rate *= (0.62f - 0.26f);
                rate += 0.26f;
            }
            else if (Main.GameMode.CheckPoint >= 3)
            {
                int value = Main.GameMode.TotalScore - Main.GameMode.getConditionScore(2);
                if (value < 0)
                    value = 0;
                rate  = (float)value / ((float)Main.GameMode.getConditionScore(3) - (float)Main.GameMode.getConditionScore(2));
                rate *= (1.0f - 0.62f);
                rate += 0.62f;
            }

            view.image_PassBar.value = rate;
            AnimatorStateInfo info = view.animator_PassFlash.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Idle"))
            {
                //view.Image_PassSlot.gameObject.SetActive(true);
                view.animator_PassFlash.SetInteger("Flag", 1);
            }
        }

        void UpdateBigContinue()
        {
            if (Main.GameMode.RunState() == RunMode.Wait)
            {

                if (!view.image_ContinueNumber.gameObject.activeSelf)
                {
                    Main.SoundController.PlayTimeOverSound();
                }
                view.image_ContinueGround.gameObject.SetActive(true);
                if (Main.SettingManager.GameLanguage == 0)
                {
                    view.image_ContinueText.gameObject.SetActive(true);
                }
                else
                {
                    view.image_text_english.gameObject.SetActive(true);
                }
                view.image_ContinueNumber.gameObject.SetActive(true);
                int value = (int)Main.GameMode.ContinueTime();
                view.image_ContinueNumber.sprite = view.image_Continues[value].sprite;
                view.image_ContinueNumber.rectTransform.sizeDelta = new Vector2(110, 162);
                view.image_ContinueNumber.color = new Color(1.0f, 1.0f, 0.53f);
            }
            else
            {
                view.image_ContinueGround.gameObject.SetActive(false);
                view.image_text_english.gameObject.SetActive(false);
                view.image_ContinueText.gameObject.SetActive(false);
                view.image_ContinueNumber.gameObject.SetActive(false);
            }
        }

        //// 水量变化
        //void UpdateWater()
        //{
        //    view.image_WaterBar.value = Main.GameMode.TotalWaterPercent();
        //}

        // // 水量变化
        //void UpdateAOEFlash()
        //{
        //    AnimatorStateInfo info = view.animator_AOEFlash.GetCurrentAnimatorStateInfo(0);
        //    if (info.IsName("Play") && info.normalizedTime >= 1.0f)
        //    {
        //        view.animator_AOEFlash.SetInteger("Flag", 0);
        //        view.Image_AoeSlotFlash.gameObject.SetActive(false);
        //    }
        //}

        void UpdatePassPrpgress()
        {
            AnimatorStateInfo info = view.animator_PassFlash.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Play") && info.normalizedTime >= 1.0f)
            {
                view.animator_PassFlash.SetInteger("Flag", 0);
                //view.Image_PassSlot.gameObject.SetActive(false);
            }

            if (Main.GameMode.CheckPoint > 1)
            {
                view.image_Star1Gray.gameObject.SetActive(false);
                view.image_Star1.gameObject.SetActive(true);
            }

            if (Main.GameMode.CheckPoint > 2)
            {
                view.image_Star2Gray.gameObject.SetActive(false);
                view.image_Star2.gameObject.SetActive(true);
            }

            if (Main.GameMode.CheckPoint > 3 && view.image_PassBar.value >= 1.0f)
            {
                view.image_Star3Gray.gameObject.SetActive(false);
                view.image_Star3.gameObject.SetActive(true);
            }
        }
    
        // 准星
        void PlayerUpdateTarget(int playerIndex, Player player)
        {
            PlayView.PlayerPanel playerPanel = (PlayView.PlayerPanel)view.panelList[playerIndex];
            if (player.State == Player.StateType.Play)
            {
                // 根据屏幕的鼠标位置和UI的位置，通过特殊工具把屏幕坐标转变为UI坐标
                Vector2 pos;
                Camera cam = playerPanel.image_Target.canvas.worldCamera;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(playerPanel.image_Target.canvas.transform as RectTransform, Main.Controller.JoystickPosition(playerIndex), null, out pos))
                {
                    playerPanel.image_Target.rectTransform.anchoredPosition = pos;
                }
                if (!playerPanel.image_Target.gameObject.activeSelf)
                {
                    if(GameConfig.GAME_CONFIG_WATER_SHOW == 0)
                    {
                        playerPanel.image_Target.gameObject.SetActive(true);
                    }
                    else
                    {
                        playerPanel.image_Target.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (playerPanel.image_Target.gameObject.activeSelf)
                {
                    playerPanel.image_Target.gameObject.SetActive(false);
                }
            }
        }

        // 加水等待
        //void PlayerUpdateAddWaterAnimation(int playerIndex, Player player)
        //{
        //    // 改函数在固定刷新函数中
        //    PlayView.PlayerPanel playerPanel = (PlayView.PlayerPanel)view.panelList[playerIndex];
        //    if (player.AddWaterTime <= 0)
        //    {
        //        playerPanel.image_WaitAddWater.gameObject.SetActive(false);
        //        return;
        //    }

        //    PlayerUIParam uiParam = (PlayerUIParam)uiParamList[playerIndex];
        //    uiParam.curFrame++;
        //    if (uiParam.curFrame >= 8)
        //    {
        //        uiParam.curFrame = 0;
        //    }
        //    // 替换公共加水等待图片
        //    playerPanel.image_WaitAddWater.sprite = view.image_AddWaters[uiParam.curFrame].sprite;
        //    playerPanel.image_WaitAddWater.SetNativeSize();
        //    playerPanel.image_WaitAddWater.gameObject.SetActive(true);
        //    uiParamList[playerIndex] = uiParam;
        //}

        // 缺水警告
        void PlayerUpdateWarning(int playerIndex, Player player)
        {
            PlayView.PlayerPanel playerPanel = (PlayView.PlayerPanel)view.panelList[playerIndex];
            PlayerUIParam uiParam = (PlayerUIParam)uiParamList[playerIndex];
            if (player.State == Player.StateType.Play)
            {
                if(player.LifeTime <= 10)
                {
                    playerPanel.image_TimerWarning.gameObject.SetActive(true);
                    if (uiParam.twGround == null)
                    {
                        Color color = playerPanel.image_TimerWarning.color;
                        color.a = 0.0f;
                        uiParam.twGround = playerPanel.image_TimerWarning.DOColor(color, 0.2f);
                        uiParam.twGround.SetLoops(-1, LoopType.Yoyo);
                        uiParam.twGround.Play();
                    }
                    else 
                    {
                        if (!uiParam.twGround.IsPlaying())
                        {
                            uiParam.twGround.Play();
                        }
                    }
                }
            }
            else
            {
                if (uiParam.twGround != null && uiParam.twGround.IsPlaying())
                {
                    playerPanel.image_TimerWarning.gameObject.SetActive(false);
                    uiParam.twGround.Pause();
                }
            }
        }

        // 时间变化
        void PlayerUpdateTimer(int playerIndex, Player player)
        {
            PlayView.PlayerPanel playerPanel = (PlayView.PlayerPanel)view.panelList[playerIndex];
            if (player.State == Player.StateType.Play)
            {
                playerPanel.image_TimerBar.value = player.LifeTime / (float)GameConfig.GAME_CONFIG_MAX_LIFE_TIME;
                if (!playerPanel.image_HeadGround.gameObject.activeSelf)
                {
                    playerPanel.image_HeadGround.gameObject.SetActive(true);
                    playerPanel.image_TimerBar.gameObject.SetActive(true);
                }
            }
            else
            {
                if (playerPanel.image_HeadGround.gameObject.activeSelf)
                {
                    playerPanel.image_TimerBar.gameObject.SetActive(false);
                    playerPanel.image_HeadGround.gameObject.SetActive(false);
                }
            }
        }

        // 积分更新
        void PlayerUpdateScore(int playerIndex, Player player)
        {
            PlayView.PlayerPanel playerPanel = (PlayView.PlayerPanel)view.panelList[playerIndex];
            PlayerUIParam uiParam = (PlayerUIParam)uiParamList[playerIndex];
            if (player.State == Player.StateType.Play)
            {
                int score = player.Score;

                //if (uiParam.curScore + uiParam.addScore < player.Score)
                //{
                //    uiParam.addScore += (player.Score - (uiParam.curScore + uiParam.addScore));
                //}

                int randomScore = Random.Range(1, 20);
                bool playScale = false;
                if (uiParam.addScore > 0)
                {
                    if (uiParam.addScore >= randomScore)
                    {
                        uiParam.curScore += randomScore;
                        uiParam.addScore -= randomScore;
                    }
                    else
                    {
                        uiParam.curScore += uiParam.addScore;
                        uiParam.addScore  = 0;
                    }

                    playScale = true;
                }
                else 
                {
                    if (playerPanel.image_NumberSlot1.gameObject.activeSelf && uiParam.curScore == player.Score)
                        return;
                }

                int value1 = uiParam.curScore % 10;
                int value2 = (uiParam.curScore / 10) % 10;
                int value3 = (uiParam.curScore / 100) % 10;
                int value4 = (uiParam.curScore / 1000) % 10;
                int value5 = (uiParam.curScore / 10000) % 10;
                playerPanel.image_NumberSlot1.sprite = view.image_Numbers[value1].sprite;
                playerPanel.image_NumberSlot2.sprite = view.image_Numbers[value2].sprite;
                playerPanel.image_NumberSlot3.sprite = view.image_Numbers[value3].sprite;
                playerPanel.image_NumberSlot4.sprite = view.image_Numbers[value4].sprite;
                playerPanel.image_NumberSlot5.sprite = view.image_Numbers[value5].sprite;
                Vector2 size = new Vector2(21,29);
                playerPanel.image_NumberSlot1.rectTransform.sizeDelta = size;
                playerPanel.image_NumberSlot2.rectTransform.sizeDelta = size;
                playerPanel.image_NumberSlot3.rectTransform.sizeDelta = size;
                playerPanel.image_NumberSlot4.rectTransform.sizeDelta = size;
                playerPanel.image_NumberSlot5.rectTransform.sizeDelta = size;
                playerPanel.image_NumberSlot1.gameObject.SetActive(true);
                playerPanel.image_NumberSlot2.gameObject.SetActive(true);
                playerPanel.image_NumberSlot3.gameObject.SetActive(true);
                playerPanel.image_NumberSlot4.gameObject.SetActive(true);
                playerPanel.image_NumberSlot5.gameObject.SetActive(true);
                playerPanel.image_Head1.gameObject.SetActive(true);
                //playerPanel.image_Head2.gameObject.SetActive(true);
                playerPanel.image_HeadGround4.gameObject.SetActive(true);
                playerPanel.image_Head1Grey.gameObject.SetActive(false);
                playerPanel.image_image_HeadGroundGrey.gameObject.SetActive(false);
                playerPanel.image_HeadGround4Grey.gameObject.SetActive(false);

                if (!uiParam.playinngScale && playScale)
                {
                    Vector3 scaleEnd = new Vector3(1.5f,1.5f,1.5f);
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(playerPanel.image_NumberSlot1.rectTransform.DOScale(scaleEnd, 0.1f).SetLoops(2, LoopType.Yoyo));
                    sequence.Join(playerPanel.image_NumberSlot2.rectTransform.DOScale(scaleEnd, 0.1f).SetLoops(2, LoopType.Yoyo));
                    sequence.Join(playerPanel.image_NumberSlot3.rectTransform.DOScale(scaleEnd, 0.1f).SetLoops(2, LoopType.Yoyo));
                    sequence.Join(playerPanel.image_NumberSlot4.rectTransform.DOScale(scaleEnd, 0.1f).SetLoops(2, LoopType.Yoyo));
                    sequence.Join(playerPanel.image_NumberSlot5.rectTransform.DOScale(scaleEnd, 0.1f).SetLoops(2, LoopType.Yoyo));
                    sequence.SetLoops(1, LoopType.Yoyo);
                    uiNumberScaled.Add(playerIndex);
                    sequence.OnComplete(OnCheckScorePlayScaleEnd);
                    sequence.Play();
                    uiParam.playinngScale = true;
                }
                
                Main.IOManager.SetPlayerGameBegine(playerIndex, true);
            }
            else
            {
                if (player.State == Player.StateType.Idle)
                {

                    if (!Main.IOManager.GetIsSetGameEnd(playerIndex))
                    {
                        Main.IOManager.ComputeTicket(playerIndex, player.Score);
                        Main.IOManager.SetIsSetGameEnd(playerIndex, true);
                    }
                    
                    uiParam.addScore = 0;
                    uiParam.curScore = 0;
                    uiParam.addTime = 0.0f;
                    uiParam.playinngScale = false;                  
                }
                else if (player.State == Player.StateType.Wait)
                {
                     if (uiParam.addScore > 0)
                     {
                         uiParam.curScore += uiParam.addScore;
                         uiParam.addScore  = 0;
                     }
                }
                playerPanel.image_NumberSlot1.gameObject.SetActive(false);
                playerPanel.image_NumberSlot2.gameObject.SetActive(false);
                playerPanel.image_NumberSlot3.gameObject.SetActive(false);
                playerPanel.image_NumberSlot4.gameObject.SetActive(false);
                playerPanel.image_NumberSlot5.gameObject.SetActive(false);
                playerPanel.image_Head1.gameObject.SetActive(false);
                //playerPanel.image_Head2.gameObject.SetActive(false);
                playerPanel.image_HeadGround4.gameObject.SetActive(false);
                playerPanel.image_Head1Grey.gameObject.SetActive(true);
                playerPanel.image_image_HeadGroundGrey.gameObject.SetActive(true);
                playerPanel.image_HeadGround4Grey.gameObject.SetActive(true);

                //设置playerIndex为游戏结束状态 
                Main.IOManager.SetPlayerGameEnd(playerIndex,true);
                Main.IOManager.SetPlayerGameBegine(playerIndex, false);
               

                //if (!Main.IOManager.GetIsSetGameEnd(playerIndex))
                //{
                //    Main.IOManager.SetPlayerGameEnd(playerIndex);
                //    Main.IOManager.ComputeTicket(playerIndex, player.Score);
                //    //让结束状态值发送一次
                //    Main.IOManager.SetIsSetGameEnd(playerIndex, true);
                //}
            }
        }

        //void OnGUI()
        //{
          
        //}

        protected void OnCheckScorePlayScaleEnd()
        {
            if (uiNumberScaled.Count > 0)
            {
                int playerIndex = uiNumberScaled[0];
                PlayerUIParam uiParam = (PlayerUIParam)uiParamList[playerIndex];
                uiParam.playinngScale = false;
                uiNumberScaled.RemoveAt(0);
            }
        }

        // 时间
        //void PlayerUpdateTimer(int playerIndex, Player player)
        //{
        //    PlayView.PlayerPanel playerPanel = (PlayView.PlayerPanel)view.panelList[playerIndex];

        //    if (player.State == Player.StateType.Play)
        //    {
        //        float time = player.LifeTime;
        //        Vector2 size = new Vector2(21, 29);
        //        {
        //            float minute = (time / 60.0f);
        //            int value  = (int)minute;
        //            int value1 = value % 10;
        //            int value2 = (value / 10) % 10;
        //            playerPanel.image_TimerSlot4.sprite = view.image_Numbers[value1].sprite;
        //            playerPanel.image_TimerSlot5.sprite = view.image_Numbers[value2].sprite;
        //            playerPanel.image_TimerSlot4.rectTransform.sizeDelta = size;
        //            playerPanel.image_TimerSlot5.rectTransform.sizeDelta = size;
        //            playerPanel.image_TimerSlot4.color = Color.green;
        //            playerPanel.image_TimerSlot5.color = Color.green;
        //        }

        //        {

        //            int minute  = (int)(time / 60.0f);
        //            int value   = (int)(time - (float)minute * 60.0f);
        //            int value1 = value % 10;
        //            int value2 = (value / 10) % 10;
        //            playerPanel.image_TimerSlot1.sprite = view.image_Numbers[value1].sprite;
        //            playerPanel.image_TimerSlot2.sprite = view.image_Numbers[value2].sprite;
        //            playerPanel.image_TimerSlot1.rectTransform.sizeDelta = size;
        //            playerPanel.image_TimerSlot2.rectTransform.sizeDelta = size;
        //            playerPanel.image_TimerSlot1.color = Color.green;
        //            playerPanel.image_TimerSlot2.color = Color.green;
        //        }


        //        playerPanel.image_TimerSlot1.gameObject.SetActive(true);
        //        playerPanel.image_TimerSlot2.gameObject.SetActive(true);
        //        playerPanel.image_TimerSlot3.gameObject.SetActive(true);
        //        playerPanel.image_TimerSlot4.gameObject.SetActive(true);
        //        playerPanel.image_TimerSlot5.gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        playerPanel.image_TimerSlot1.gameObject.SetActive(false);
        //        playerPanel.image_TimerSlot2.gameObject.SetActive(false);
        //        playerPanel.image_TimerSlot3.gameObject.SetActive(false);
        //        playerPanel.image_TimerSlot4.gameObject.SetActive(false);
        //        playerPanel.image_TimerSlot5.gameObject.SetActive(false);
        //    }

        //}
        
        // 继续游戏倒计时
        void PlayerUpdateContinue(int playerIndex, Player player)
        {
            PlayView.PlayerPanel playerPanel = (PlayView.PlayerPanel)view.panelList[playerIndex];

            if (player.State == Player.StateType.Wait && !(Main.GameMode.RunState() == RunMode.Wait))
            {
                if (Main.SettingManager.GameLanguage == 0)
                {
                    playerPanel.image_ContinueText.gameObject.SetActive(true);
                    playerPanel.image_ContinueText_En.gameObject.SetActive(false);
                }
                else
                {
                    playerPanel.image_ContinueText.gameObject.SetActive(false);
                    playerPanel.image_ContinueText_En.gameObject.SetActive(true);
                }
                playerPanel.image_ContinueNumber.gameObject.SetActive(true);
                int value = (int)player.ContinueTime;
                playerPanel.image_ContinueNumber.sprite = view.image_Continues[value].sprite;
                playerPanel.image_ContinueNumber.rectTransform.sizeDelta = new Vector2(21,29);
                playerPanel.image_ContinueNumber.color = new Color(0.0f, 0.53f, 0.0f);
            }
            else
            {
                playerPanel.image_ContinueText.gameObject.SetActive(false);
                playerPanel.image_ContinueNumber.gameObject.SetActive(false);
                playerPanel.image_ContinueText_En.gameObject.SetActive(false);
            }         
        }

        // 游戏币
        void PlayerUpdateCoin(int playerIndex, Player player)
        {
            PlayView.PlayerPanel playerPanel = (PlayView.PlayerPanel)view.panelList[playerIndex];

            if (player.State == Player.StateType.Play)
            {
                playerPanel.image_CoinText1_En.gameObject.SetActive(false);
                playerPanel.image_CoinText1.gameObject.SetActive(false);
                playerPanel.image_CoinText2.gameObject.SetActive(false);
                playerPanel.image_CoinNumber1.gameObject.SetActive(false);
                playerPanel.image_CoinNumber2.gameObject.SetActive(false);
                playerPanel.image_CoinNumber3.gameObject.SetActive(false);
            }
            else
            {
                if (Main.SettingManager.GameLanguage == 0)
                {
                    playerPanel.image_CoinText1.gameObject.SetActive(true);
                }
                else
                {
                    playerPanel.image_CoinText1_En.gameObject.SetActive(true);
                }
                playerPanel.image_CoinText2.gameObject.SetActive(true);
                playerPanel.image_CoinNumber2.gameObject.SetActive(true);
                playerPanel.image_CoinNumber3.gameObject.SetActive(true);

                int value = player.Coin;
                int number1 = (value / 10) % 10;
                int number2 = (value / 1) % 10;
                playerPanel.image_CoinNumber1.sprite = view.image_Coins[number1].sprite;
                playerPanel.image_CoinNumber2.sprite = view.image_Coins[number2].sprite;
                playerPanel.image_CoinNumber3.sprite = view.image_Coins[GameConfig.GAME_CONFIG_PER_USE_COIN].sprite;
                if (value > 9)
                {
                    playerPanel.image_CoinNumber1.gameObject.SetActive(true);
                }
                else
                {
                    playerPanel.image_CoinNumber1.gameObject.SetActive(false);
                }
                
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

        /// <summary>
        /// 添加逻辑监听
        /// </summary>
        protected virtual void addEvent()
        {
            EventDispatcher.AddEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE, UpdatePerFrame);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE, UpdateFixFrame);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_UPDATE_AOE_POWER, OnEventUpdateAoe);
            EventDispatcher.AddEventListener<int,int>(GameEventDef.EVNET_APPEND_SCORES,    OnEventAppendScores);
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
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_UPDATE_AOE_POWER, OnEventUpdateAoe);
            EventDispatcher.RemoveEventListener<int, int>(GameEventDef.EVNET_APPEND_SCORES, OnEventAppendScores);
            EventDispatcher.RemoveEventListener<bool>(GameEventDef.EVENT_WATER_HIGHT, OnHightWater);
            EventDispatcher.RemoveEventListener<bool>(GameEventDef.EVENT_WATER_LOW, OnLowWater);
           
        }

        /// <summary>
        /// 移除UI监听
        /// </summary>
        protected virtual void removeUIEventListener()
        {

        }

    }
}
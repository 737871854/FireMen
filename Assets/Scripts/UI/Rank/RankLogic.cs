using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

namespace Need.Mx
{

    public class RankLogic : MonoBehaviour
    {
        public class PlayerUIParam
        {
            public string name;
            public int index;
            public bool flag;

            public PlayerUIParam()
            {
                name = "_";
                index = 0;
                flag = false;
            }
        }


        protected RankView view;
        protected float remainTime;
        protected bool start;
        protected List<PlayerUIParam> uiParamList;

        // Use this for initialization
        void Start()
        {
            uiParamList = new List<PlayerUIParam>(GameConfig.GAME_CONFIG_PLAYER_COUNT);
            for (int index = 0; index < Main.PlayerCount(); ++index)
            {
                uiParamList.Add(new PlayerUIParam());
            }

            remainTime = GameConfig.GAME_CONFIG_RANK_WAIT_TIME;
            start = false;
            view = gameObject.GetComponent<RankView>();
            view.Init();
            Init();
            addEvent();
            addUIEventListener();
        }

        void Init()
        {
            if (Main.SettingManager.GameLanguage == 0)
            {
                view.image_Ground1.gameObject.SetActive(true);
                view.image_TextTips1.gameObject.SetActive(true);
                view.iamge_EnSure.gameObject.SetActive(true);
                view.image_TopN_Ground1.gameObject.SetActive(true);
                view.image_EnSure_En.gameObject.SetActive(false);
                view.image_Ground1_En.gameObject.SetActive(false);
                view.image_TextTips1_En.gameObject.SetActive(false);
                view.image_TopN_Ground1_En.gameObject.SetActive(false);
            }
            else
            {
                view.image_Ground1.gameObject.SetActive(false);
                view.image_TextTips1.gameObject.SetActive(false);
                view.iamge_EnSure.gameObject.SetActive(false);
                view.image_TopN_Ground1.gameObject.SetActive(false);
                view.image_EnSure_En.gameObject.SetActive(true);
                view.image_Ground1_En.gameObject.SetActive(true);
                view.image_TextTips1_En.gameObject.SetActive(true);
                view.image_TopN_Ground1_En.gameObject.SetActive(true);
            }
        }

        void OnDestroy()
        {
            removeEvent();
            Main.SoundController.StopRankSound();
        }

        int GetLetterImageIndexBy(char value)
        {
            if (value == '_')
                return 26;
            int ascii = (int)value;
            if (ascii < 65 || ascii > 90)
                return 26;
            int index = ascii - 65;
            return index;
        }

        Image GetKeyImageByPosition(Vector2 pos)
        {
            for(int index = 0; index < view.image_Keys.Count; ++index)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(view.image_Keys[index].rectTransform, pos, null))
                {
                    return view.image_Keys[index];
                }
            }
            return null;
        }

        public void UpdatePerFrame()
        {
            if (Main.GameMode.RunState() == RunMode.EndingEdit)
            {
                for (int index = 0; index < view.image_Edits.Count; ++index)
                {
                    Player player = Main.PlayerManager.getPlayer(index);
                    if (player == null)
                    {
                        return;
                    }
                    PlayerUpdateTarget(index, player);
                }
            }

            if (start == false)
            {
                remainTime -= Main.NonStopTime.deltaTime;
                if (Main.GameMode.RunState() == RunMode.EndingEdit)
                {
                    int value = (int)remainTime;
                    int number1 = (value / 10) % 10;
                    int number2 = (value / 1) % 10;
                    Vector2 size1 = new Vector2(37, 52);
                    Vector2 size2 = new Vector2(82, 58);
                    view.image_Time1.sprite = view.image_Numbers[number1].sprite;
                    view.image_Time2.sprite = view.image_Numbers[number2].sprite;
                    view.image_Time1.rectTransform.sizeDelta = size1;
                    view.image_Time2.rectTransform.sizeDelta = size1;

                    if (remainTime <= 0 && !start)
                    {
                        remainTime = 0;
                        start = true;
                        List<string> names = new List<string>();
                        for (int index = 0; index < Main.PlayerCount(); ++index)
                        {
                            Player player = Main.PlayerManager.getPlayer(index);
                            RankView.EditPanel ui = view.image_Edits[index];
                            PlayerUIParam uiParam = uiParamList[index];
                            // 自动输入
                            for (int count = uiParam.index; count < GameConfig.GAME_CONFIG_NAME_LEN; ++count)
                            {
                                PushName(index, 'A');
                            }
                            ConfirmName(index);
                            names.Add(uiParam.name);
                        }

                        // 结算
                        EventDispatcher.TriggerEvent(GameEventDef.EVNET_RANK_EDIT_INPUT_COMPLETE, names[0], names[1], names[2]);
                    }

                    int playerCount = 0;
                    int fullNameCount = 0;
                    for (int index = 0; index < Main.PlayerCount(); ++index)
                    {
                        Player player = Main.PlayerManager.getPlayer(index);
                        RankView.EditPanel ui = view.image_Edits[index];
                        PlayerUIParam uiParam = uiParamList[index];

                        if (player.State == Player.StateType.Play)
                        {
                            ++playerCount;
                            // 判断文字
                            Image image_key = GetKeyImageByPosition(Main.Controller.JoystickPosition(index));
                            if (image_key != null)
                            {
                                if (ui.image_Key != null && image_key.gameObject.name == ui.image_Key.name  && !uiParam.flag)
                                {
                                    Animator animator = ui.image_KeyPressed.GetComponent<Animator>();
                                    AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
                                    if (info.IsName("Play") && info.normalizedTime >= 1.0f)
                                    {
                                        string keyName = ui.image_Key.name.Substring(9); 
                                        // 添加
                                        if (keyName.Length == 1)
                                        {
                                            PushName(index, keyName[0]);
                                        }
                                        // 删除
                                        else if (keyName.Equals("Del"))
                                        {
                                            DeleteName(index);
                                        }
                                        // 确定
                                        else if (keyName.Equals("Enter"))
                                        {
                                            ConfirmName(index);
                                            Main.IOManager.SetPlayerGameBegine(index, false);
                                        }
                                        GameObject.Destroy(ui.image_KeyPressed.gameObject);
                                        ui.image_KeyPressed = null;
                                        ui.image_Key = null; 
                                    }
                                }
                                else
                                {
                                    if (ui.image_KeyPressed != null)
                                    {
                                        GameObject.Destroy(ui.image_KeyPressed);
                                        ui.image_KeyPressed = null;
                                    }

                                    ui.image_Key = image_key;
                                    string imageName = image_key.gameObject.name;
                                    string keyName = imageName.Substring(9);
                                    if (keyName.Length > 1)
                                    {
                                        ui.image_KeyPressed = GameObject.Instantiate(view.image_KeyPressed2) as Image;
                                    }
                                    else
                                    {
                                        ui.image_KeyPressed = GameObject.Instantiate(view.image_KeyPressed1) as Image;
                                    }
                                    ui.image_KeyPressed.transform.parent = view.panel_KeyParent.transform;
                                    ui.image_KeyPressed.rectTransform.anchoredPosition = image_key.rectTransform.anchoredPosition;
                                    ui.image_KeyPressed.rectTransform.localScale = Vector3.one;
                                    ui.image_KeyPressed.SetNativeSize();
                                    ui.image_KeyPressed.gameObject.SetActive(true);
                                    
                                }
                            }
                            else
                            {
                                if (ui.image_KeyPressed != null)
                                {
                                    GameObject.Destroy(ui.image_KeyPressed.gameObject);
                                    ui.image_KeyPressed = null;
                                    ui.image_Key = null;
                                }
                            }

                            // 文字显示
                            ui.image_Head.gameObject.SetActive(true);
                            for (int count = 0; count < GameConfig.GAME_CONFIG_NAME_LEN; ++count)
                            {
                                if (uiParam.index >= count)
                                {
                                    // 显示文字
                                    int imageIndex = GetLetterImageIndexBy(uiParam.name[count]);
                                    ui.image_CharSlotList[count].sprite = view.image_EditChars[imageIndex].sprite;
                                    if (imageIndex == 26)
                                    {
                                        ui.image_CharSlotList[count].SetNativeSize();
                                    }
                                    else
                                    {
                                        ui.image_CharSlotList[count].rectTransform.sizeDelta = size2;
                                    }
                                    ui.image_CharSlotList[count].gameObject.SetActive(true);
                                }
                                else
                                {
                                    // 不显示
                                    ui.image_CharSlotList[count].gameObject.SetActive(false);
                                }
                            }

                            if (uiParam.flag)
                            {
                                ++fullNameCount;
                            }

                        }
                        else
                        {
                            ui.image_Head.gameObject.SetActive(false);
                        }
                        view.image_Edits[index] = ui;
                        uiParamList[index] = uiParam;
                    }

                    // 判断是否输入完毕
                    if (fullNameCount == playerCount)
                    {
                        remainTime = 0;
                    }
                }

            }

        }

        public void UpdateFixFrame()
        {
            
        }

        // 准星
        void PlayerUpdateTarget(int playerIndex, Player player)
        {
            RankView.EditPanel playerPanel = (RankView.EditPanel)view.image_Edits[playerIndex];
            if (player.State == Player.StateType.Play || player.Pass)
            {
                if (!Main.IOManager.GetIsSetGameEnd(playerIndex))
                {
                    Main.IOManager.ComputeTicket(playerIndex, player.Score);
                    Main.IOManager.SetIsSetGameEnd(playerIndex, true);
                }
                // 根据屏幕的鼠标位置和UI的位置，通过特殊工具把屏幕坐标转变为UI坐标
                Vector2 pos;
                Main.IOManager.SetPlayerGameBegine(playerIndex, true);
                Camera cam = playerPanel.image_Target.canvas.worldCamera;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(playerPanel.image_Target.canvas.transform as RectTransform, Main.Controller.JoystickPosition(playerIndex), null, out pos))
                {
                    playerPanel.image_Target.rectTransform.anchoredPosition = pos;
                }
                if (!playerPanel.image_Target.gameObject.activeSelf)
                {
                    playerPanel.image_Target.gameObject.SetActive(true);
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

        public void OnEventRankEditInput()
        {
            remainTime = GameConfig.GAME_CONFIG_RANK_WAIT_TIME;
            start = false;
            view.image_Edit.gameObject.SetActive(true);
            view.image_Time1.sprite = view.image_Numbers[3].sprite;
            view.image_Time2.sprite = view.image_Numbers[0].sprite;
            Vector2 size = new Vector2(37, 52);
            view.image_Time1.rectTransform.sizeDelta = size;
            view.image_Time2.rectTransform.sizeDelta = size;
            Main.SoundController.PlayEditNameSound();
            Main.SoundController.PlayRankSound();
        }

        public void OnEventRankTopN()
        {
            view.image_Edit.gameObject.SetActive(false);
            view.image_TopN.gameObject.SetActive(true);
            for (int count = 0; count < GameConfig.GAME_CONFIG_MAX_RANK_ITEM; ++count)
            {
                RankManager.RankItem item = Main.RankManager.GetItem(count);
                RankView.TopPanel panel = view.image_Tops[count];
                if(item == null)
                {
                    continue;
                }
                Vector2 size1 = new Vector2(82, 58);
                for (int nameIndex = 0; nameIndex < GameConfig.GAME_CONFIG_NAME_LEN; ++nameIndex)
                {
                    int imageIndex = GetLetterImageIndexBy(item.name[nameIndex]);
                    panel.image_CharSlotList[nameIndex].sprite = view.image_EditChars[imageIndex].sprite;
                    panel.image_CharSlotList[nameIndex].rectTransform.sizeDelta = size1;
                }
                {
                    Vector2 size2 = new Vector2(37,52);
                    int score = item.value;
                    int value1 = score % 10;
                    int value2 = (score / 10) % 10;
                    int value3 = (score / 100) % 10;
                    int value4 = (score / 1000) % 10;
                    int value5 = (score / 10000) % 10;
                    panel.image_NumberSlotList[0].sprite = view.image_Numbers[value5].sprite;
                    panel.image_NumberSlotList[1].sprite = view.image_Numbers[value4].sprite;
                    panel.image_NumberSlotList[2].sprite = view.image_Numbers[value3].sprite;
                    panel.image_NumberSlotList[3].sprite = view.image_Numbers[value2].sprite;
                    panel.image_NumberSlotList[4].sprite = view.image_Numbers[value1].sprite;
                    panel.image_NumberSlotList[0].rectTransform.sizeDelta = size2;
                    panel.image_NumberSlotList[1].rectTransform.sizeDelta = size2;
                    panel.image_NumberSlotList[2].rectTransform.sizeDelta = size2;
                    panel.image_NumberSlotList[3].rectTransform.sizeDelta = size2;
                    panel.image_NumberSlotList[4].rectTransform.sizeDelta = size2;
                }
                panel.image_Head.sprite = view.image_Head[item.id].sprite;
            }
        }

        public void PushName(int playerIndex,char value)
        {
            PlayerUIParam uiParam = uiParamList[playerIndex];
            if (uiParam.index >= GameConfig.GAME_CONFIG_NAME_LEN)
            {
                return;
            }
            if (uiParam.flag)
            {
                return;
            }
            char[] nameArray = uiParam.name.ToCharArray();
            nameArray[uiParam.index] = value;
            uiParam.name = new string(nameArray) + '_';
            ++uiParam.index;
        }

        public void ConfirmName(int playerIndex)
        {
            PlayerUIParam uiParam = uiParamList[playerIndex];
            if (uiParam.index < GameConfig.GAME_CONFIG_NAME_LEN - 1)
            {
                return;
            }
            uiParam.flag = true;
        }

        public void DeleteName(int playerIndex)
        {
            PlayerUIParam uiParam = uiParamList[playerIndex];
            if (uiParam.index == 0)
            {
                return;
            }
            if (uiParam.flag)
            {
                return;
            }
            --uiParam.index;
            uiParam.name = uiParam.name.Substring(0, uiParam.index) + '_';
            
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
            EventDispatcher.AddEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE,   UpdatePerFrame);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE,   UpdateFixFrame);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_RANK_EDIT_INPUT,    OnEventRankEditInput);
            EventDispatcher.AddEventListener(GameEventDef.EVNET_RANK_TOPN,          OnEventRankTopN);
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
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE,    UpdatePerFrame);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE,    UpdateFixFrame);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_RANK_EDIT_INPUT,     OnEventRankEditInput);
            EventDispatcher.RemoveEventListener(GameEventDef.EVNET_RANK_TOPN,           OnEventRankTopN);
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

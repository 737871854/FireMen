using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public partial class GameMode : MonoBehaviour
{
    void OnEventInputCoin(int index)
    {
        Player player = Main.PlayerManager.getPlayer(index);
        if (player == null)
        {
            return;
        }
        player.IncreaseCoin(1);
        Main.SoundController.PlayInsertCoinSound();
        Main.SettingManager.Save();

        idleTime = 0;
        if (mode == RunMode.Idle)
        {
            ReturnStart(false);
        }
    }

    void OnEventOutputCoin(int index)
    {

    }

    void OnEventDamageLifeTime(int value)
    {
        for (int index = 0; index < Main.PlayerCount(); ++index)
        {
            Player player = Main.PlayerManager.getPlayer(index);
            if (player.IsPlaying())
            {
                player.DecreaseLife(value);
            }
        }
    }

    void OnPlayerInput(int playerIndex, ref Player player)
    {
        if (IsPlay())
        {
            if (player.IsPlaying())
            {
                if (player.FireTime >= GameConfig.GAME_CONFIG_WATER_DAMAGE_INTERVAL)
                {
                    player.FireTime = 0.0f;
                    {
                        Monster monster = SceneManager.instance.PickMonster(Main.Controller.JoystickPosition(playerIndex));
                        if (monster)
                        {
                            monster.OnSprayWater(GameConfig.GAME_CONFIG_WATER_DAMAGE_1, player);
                            int value = monster.ObtainScore();
                            if (value > 0)
                            {
                                player.IncreaseScore(value);
                                PushScore(value);
                                Main.SoundController.PlayGetPointSound();
                            }
                        }
                    }

                    {
                        Monster monster = null;
                        GameObject goBind = null;
                        if (SceneManager.instance.PickObject(Main.Controller.JoystickPosition(playerIndex), out monster, out goBind))
                        {
                            if (monster != null && goBind != null)
                            {
                                bool breakHitting = monster.Invincible;
                                monster.OnSprayWaterHitting(goBind,GameConfig.GAME_CONFIG_WATER_DAMAGE_1);
                                int value = monster.ObtainScore();
                                if (value > 0)
                                {
                                    player.IncreaseScore(value);
                                    Main.SoundController.PlayGetPointSound();
                                }
                                if (breakHitting && !monster.Invincible)
                                {
                                    Main.SoundController.PlaySkillBreakSound();
                                }
                            }
                        }

                    }
                }
                else
                {
                    player.FireTime += Main.NonStopTime.deltaTime;
                }

                //if (Main.Controller.IsSupplyButtonPressed(playerIndex))
                //{
                //    if (player.Water < GameConfig.GAME_CONFIG_VALUE_10004)
                //    {
                //        player.SupplyWater();
                //    }
                //}

                if (Main.Controller.IsCallCar1ButtonPressed(playerIndex))
                {
                    CarManager.instance.Rescue(1);
                }

                if (Main.Controller.IsCallCar2ButtonPressed(playerIndex))
                {
                    CarManager.instance.Rescue(2);
                }

                if (Main.Controller.IsCallCar3ButtonPressed(playerIndex))
                {
                    CarManager.instance.Rescue(3);
                }
            }
        }

        if (IsWatering())
        {
            if (player.IsPlaying())
            {
                if (Main.Controller.IsCallCar1ButtonPressed(playerIndex))
                {
                    PushWater(GameConfig.GAME_CONFIG_PER_CONSUME_WATER);
                }

                if (Main.Controller.IsCallCar2ButtonPressed(playerIndex))
                {
                    PushWater(GameConfig.GAME_CONFIG_PER_CONSUME_WATER);
                }

                if (Main.Controller.IsCallCar3ButtonPressed(playerIndex))
                {
                    PushWater(GameConfig.GAME_CONFIG_PER_CONSUME_WATER);
                }
            }
        }

        if (IsEndingEdit())
        {
            //if (player.IsPlaying())
            //{
            //    if (Main.Controller.IsCallCar1ButtonPressed(playerIndex))
            //    {
            //        player.PushName();
            //    }
            //    if (Main.Controller.IsStartButtonPressed(playerIndex))
            //    {
            //        player.ConfirmName();
            //    }
            //}
        }

        if (IsCanJoinGame() && mode != RunMode.Idle && mode != RunMode.Setting)
        {
            if (Main.Controller.IsStartButtonPressed(playerIndex) && player.IsCanPlay())
            {
                Main.SettingManager.LogCoins(GameConfig.GAME_CONFIG_PER_USE_COIN);
                Main.SettingManager.Save();
                player.ChangePlay();
                Main.IOManager.SetPlayerGameEnd(playerIndex, false);
                Main.IOManager.SetIsSetGameEnd(playerIndex, false);
                Main.SoundController.PlayConfirmSound();
                // 只要有一个用户有足够的币开始就进入选择界面等待其他人加入
                if (RunState() == RunMode.Start)
                {
                    NextLevel();
                }
            }
        }

        if (mode == RunMode.Setting)
        {
            if (Main.Controller.IsFlag1ButtonPressed(playerIndex))
            {
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_SETTING_CONFIRM);
            }

            if (Main.Controller.IsFlag2ButtonPressed(playerIndex))
            {
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_SETTING_SELECT);
            }
        }
        else if (mode == RunMode.Start)
        {
            if (Main.Controller.IsFlag1ButtonPressed(playerIndex))
            {
                GoToSetting();
            }
        }
        else if (mode == RunMode.Idle)
        {
            if (Main.Controller.IsStartButtonPressed(playerIndex))
            {
                ReturnStart(false);
            }
        }

        if (mode == RunMode.MoviePlay)
        {
            if (Main.Controller.IsStartButtonPressed(playerIndex))
            {
                NextLevel();
            }
        }

        if (Main.Controller.IsWaterLow())
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVENT_WATER_LOW, true);
        }
        else
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVENT_WATER_LOW, false);
        }

        if (Main.Controller.IsWaterHight())
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVENT_WATER_HIGHT,true);
        }
        else
        {
             EventDispatcher.TriggerEvent(GameEventDef.EVENT_WATER_HIGHT,false);
        }


        Main.Controller.ClearState(playerIndex);
    }

    void OnPlayerLogic(int playerIndex, ref Player player)
    {
        if (IsPlay())
        {
            if (player.IsPlaying())
            {
                player.UpdateLife();
                player.UpdateWater();
                Monster target = CarManager.instance.Pickup(playerIndex+1);
                if (target != null)
                {
                    target.ChangeRescue(player);
                    int value = target.ObtainScore();
                    if (value > 0)
                    {
                        PushScore(value);
                        player.IncreaseScore(value);
                        Main.SoundController.PlayGetPointSound();
                    }
                }
            }
            else if (player.IsContinuing())
            {
                player.UpdateContinue();
            }
        }
    }

    void RewardLifeTime(float value)
    {
        for (int index = 0; index < Main.PlayerCount(); ++index)
        {
            Player player = Main.PlayerManager.getPlayer(index);
            if (player.IsPlaying())
            {
                player.LifeTime = value;
            }
        }
    }
}

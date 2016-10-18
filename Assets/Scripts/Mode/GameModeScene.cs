using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public partial class GameMode : MonoBehaviour
{

    public void LoadLevel(string name)
    {
        switch (name)
        {
            case SceneType.Idle:
                {
                    Application.LoadLevel(name);
                    mode = RunMode.Idle;
                    break;
                }
            case SceneType.Setting:
                {
                    Application.LoadLevel(name);
                    mode = RunMode.Setting;
                    break;
                }
            case SceneType.SceneSelect:
                {
                    Application.LoadLevel(name);
                    mode = RunMode.Select;
                    break;
                }
            case SceneType.SceneStart:
                {
                    Application.LoadLevel(name);
                    mode = RunMode.Start;
                    break;
                }
            case SceneType.Rank:
                {
                    Application.LoadLevel(name);
                    mode = RunMode.EndingEdit;
                    StartCoroutine(OnCloseEndingRank());
                    break;
                }
            case SceneType.Scene1:
            case SceneType.Scene2:
            case SceneType.Scene3:
            case SceneType.Scene1Start:
            case SceneType.Scene1Over:
            case SceneType.Scene2Start:
            case SceneType.Scene2Over:
            case SceneType.Scene3Start:
            case SceneType.Scene3Over:
                {
                    sceneName = name;
                    mode = RunMode.Load;
                    StartCoroutine(OnCheckSceneLoaded());
                    Application.LoadLevel("Loading");
                    break;
                }
        }
    }

    public void NextLevel()
    {
        string currName = Application.loadedLevelName;
        switch (currName)
        {
            case SceneType.SceneStart:
                {
                    Main.SoundController.StopRandomMenuMusic();
                    LoadLevel(SceneType.SceneSelect);
                    break;
                }
            case SceneType.SceneSelect:
                {
                    LoadLevel(SceneType.Scene1Start);
                    break;
                }
            case SceneType.Loading:
                {
                    LoadLevel(sceneName);
                    break;
                }
            case SceneType.Scene1Start:
                {
                    LoadLevel(SceneType.Scene1);
                    break;
                }
            case SceneType.Scene1:
                {
                    LoadLevel(SceneType.Scene2Start);
                    break;
                }
            case SceneType.Scene2Start:
                {
                    LoadLevel(SceneType.Scene2);
                    break;
                }
            case SceneType.Scene2:
                {
                    LoadLevel(SceneType.Scene3Start);
                    break;
                }
            case SceneType.Scene3Start:
                {
                    LoadLevel(SceneType.Scene3);
                    break;
                }
            case SceneType.Scene3:
                { 
                    LoadLevel(SceneType.Scene3Over);
                    break;
                }
            case SceneType.Scene3Over:
                {
                    LoadLevel(SceneType.Rank);
                    break;
                }
        }
    }

    public void ReturnStart(bool reset = true)
    {
        resetGame = reset;
		Main.SettingManager.Init();
        // XML JSON加载
        if (Main.SettingManager.GameLanguage == 0)
        {
            LoadingManager.Instance.AddJsonFiles(GameConfig.SKILL_JSON, SkillData.LoadHandler);
            LoadingManager.Instance.AddJsonFiles(GameConfig.MONSTER_JSON, MonsterData.LoadHandler);
        }
        else
        {
            LoadingManager.Instance.AddJsonFiles(GameConfig.SKILL_EN_JSON, SkillData.LoadHandler);
            LoadingManager.Instance.AddJsonFiles(GameConfig.MONSTER_EN_JSON, MonsterData.LoadHandler);
        }
        LoadingManager.Instance.StartLoad(GameBegin);
    }

    void GameBegin()
    {
        LoadLevel(SceneType.SceneStart);
        if (resetGame)
        {
            Main.PlayerManager.Reset();
            Main.IOManager.SetGameEnd();
        }
        GameConfig.ParsingGameConfig();
        Main.SoundController.ResetVolumeScale();
        Main.SoundController.StartRandomMenuMusic();
        idleTime = 0;
    }

    public void GoToSetting()
    {
        Main.SoundController.StopRandomMenuMusic();
        LoadLevel(SceneType.Setting);
    }

    public void GoToIdle()
    {
        Main.SoundController.StopRandomMenuMusic();
        LoadLevel(SceneType.Idle);
    }

    void OnEventAsyncLoadingComplete(string name)
    {
        if (name == SceneType.Scene1Start ||
            name == SceneType.Scene1Over  ||
            name == SceneType.Scene2Start ||
            name == SceneType.Scene2Over  ||
            name == SceneType.Scene3Start ||
            name == SceneType.Scene3Over)
        {
            mode = RunMode.MovieStop;
            StartCoroutine(OnCheckMovieReady());
        }
        else
        {
            if (name == SceneType.Scene1)
            {
                sceneId = (int)SceneManager.SceneLevelID.Level_1;             
            }
            else if (name == SceneType.Scene2)
            {
                sceneId = (int)SceneManager.SceneLevelID.Level_2;
            }
            else if (name == SceneType.Scene3)
            {
                sceneId = (int)SceneManager.SceneLevelID.Level_3;
                Main.IOManager.SetBossCome();
            }
            LoadCondition(sceneId, GameConfig.GAME_CONFIG_DIFFICULTY);
            mode = RunMode.Play;
            Main.SoundController.StartSceneMusic(sceneId);
            StartCoroutine(OnWaitPassConditionClose());
        }
    }

    void OnEventPlayMovieComplete()
    {
        NextLevel();
    }

    void OnEventRankEditInputComplete(string name1, string name2, string name3)
    {
        List<string> names = new List<string>();
        names.Add(name1);
        names.Add(name2);
        names.Add(name3);
        for (int playerIndex = 0; playerIndex < Main.PlayerCount(); ++playerIndex)
        {
            Player playerObject = Main.PlayerManager.getPlayer(playerIndex);
            if (playerObject.IsPlaying())
            {
                playerObject.Name = names[playerIndex];
                Main.RankManager.ChangeScore(playerIndex, playerObject.Name, playerObject.Score);
            }
        }
        for (int i = 0; i < GameConfig.GAME_CONFIG_PLAYER_COUNT; i++)
        {
            Main.IOManager.SetPlayerGameBegine(i, false);
        }
        mode = RunMode.EndingTop;
    }

    IEnumerator OnWaitPassConditionClose()
    {
        while (Application.loadedLevelName == "Loading")
        {
            yield return new WaitForEndOfFrame();
        }
        Main.SoundController.PlayPassConditionSound(sceneId);
        PauseGame(true);
        if (sceneId == (int)SceneManager.SceneLevelID.Level_3)
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_CONDITION_SHOW, "Boss");
        }
        else 
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_CONDITION_SHOW, "Score");
        }

        float waitTime = 0;
        while (waitTime < 3.0f)
        {
            yield return new WaitForEndOfFrame();
            waitTime += Main.NonStopTime.deltaTime;
        }

        EventDispatcher.TriggerEvent(GameEventDef.EVNET_CONDITION_CLOSE);
        PauseGame(false);
    }

    IEnumerator OnCheckSceneLoaded()
    {
        while (!(mode == RunMode.Load && Application.loadedLevelName == "Loading"))
        {
            yield return new WaitForEndOfFrame();
        }

        mode = RunMode.Loading;
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_ASYNC_LOADING, sceneName);
    }

    IEnumerator OnCheckMovieReady()
    {
        while (mode == RunMode.MovieStop)
        {
            if (Application.loadedLevelName != SceneType.Scene1Start)
            {
                yield return new WaitForEndOfFrame();
            }
            break;
        }

        mode = RunMode.MoviePlay;
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_PLAY_MOVIE);
    }

    IEnumerator OnCloseEndingLoss()
    {
        Main.GameMode.TimeScale(1.0f);
        mode = RunMode.EndingLoss;
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_ENDING_SHOW);
        Main.SoundController.PlayLoseSound();
        Main.SoundController.StopSceneMusic();
        SceneManager.instance.DestroyAllMonster();
        yield return new WaitForSeconds(5);
        Main.SettingManager.LogNumberOfGame(1);
        Main.SettingManager.Save();
        ReturnStart();
    }

    IEnumerator OnCloseEndingSuccess()
    {
        Main.GameMode.TimeScale(1.0f);
        mode = RunMode.EndingSuccess;
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_ENDING_SHOW);
        Main.SoundController.PlayWinSound();
        Main.SoundController.StopSceneMusic();
        SceneManager.instance.DestroyAllMonster();
        if (sceneName == SceneType.Scene3)
        {
            Main.SoundController.PlayGameSuccessSound();
        }
        else
        {
            Main.SoundController.PlayMissionPassSound();
        }
        yield return new WaitForSeconds(5);
        NextLevel();
    }

    IEnumerator OnCloseEndingRank()
    {
        while (!(mode == RunMode.EndingEdit && Application.loadedLevelName == "Rank"))
        {
            yield return new WaitForSeconds(0.1f);
        }

        EventDispatcher.TriggerEvent(GameEventDef.EVNET_RANK_EDIT_INPUT);
        while (mode != RunMode.EndingTop)
        {
            yield return new WaitForSeconds(1.0f);
        }

        yield return new WaitForSeconds(2.0f);
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_RANK_TOPN);
        yield return new WaitForSeconds(10.0f);
        Main.SettingManager.LogNumberOfGame(1);
        Main.SettingManager.Save();
        ReturnStart();

        //int countDown = (int)GameConfig.GAME_CONFIG_RANK_WAIT_TIME;
        //while(countDown > 0)
        //{
        //    int playerCount = 0;
        //    int fullNameCount = 0;
        //    for (int index = 0; index < Main.PlayerCount(); ++index)
        //    {
        //        Player player = Main.PlayerManager.getPlayer(index);
        //        if (player.IsPlaying())
        //        {
        //            ++playerCount;
        //            if(player.NameEditIndex >= GameConfig.GAME_CONFIG_NAME_LEN)
        //            {
        //                ++fullNameCount; 
        //            }
        //        }
        //    }
        //    if (playerCount == fullNameCount)
        //    {
        //        break;
        //    }

              
        //    --countDown;
        //}

        //for (int playerIndex = 0; playerIndex < Main.PlayerCount(); ++playerIndex)
        //{
        //    Player playerObject = Main.PlayerManager.getPlayer(playerIndex);
        //    if (playerObject.IsPlaying())
        //    {
        //        for (int count = playerObject.NameEditIndex; count < GameConfig.GAME_CONFIG_NAME_LEN; ++count)
        //        {
        //            playerObject.PushName();
        //            playerObject.ConfirmName();
        //        }
        //        Main.RankManager.ChangeScore(playerIndex, playerObject.Name, playerObject.Score);
        //    }
        //}
       
    }
}

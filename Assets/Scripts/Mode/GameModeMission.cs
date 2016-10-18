using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public partial class GameMode : MonoBehaviour
{
    public class KillCondition
    {
        public KillCondition(int typeValue, int countValue)
        {
            count = countValue;
            type  = typeValue;
        }
        public int count = 0;
        public int type  = 0;
    };

    private int weaponId;
    private int checkPoint;
    private Dictionary<string, KillCondition>   killTypeCondition;
    private Dictionary<int, KillCondition>      killIdCondition;
    private List<int> killIdList;
    private List<int> checkPointScores;
    private float gameTime; 
    private float curShakeCount;
    private float maxShakeCount;
    private int maxTriggerCount;
    private int curTriggerCount;
    private int maxAoeScore;
    private int curAoeScore;
    private int curPlayerCount;
    private int curTotalScore;
    private int pullWaterCount;
    private int pullWaterCount1;
    private float pullWaterTime;
    private float pullWaterCountdown;

    public int   CheckPoint { get { return checkPoint; } }
    public float GameTime   { get { return gameTime; } }
    public int TotalScore   { get { return curTotalScore; } } 

    bool LoadCondition(int sceneId, int difficultyId)
    {
        curShakeCount   = 0;
        maxShakeCount   = 0;
        maxTriggerCount = 0;
        curTriggerCount = 0;
        maxAoeScore     = 0;
        curAoeScore     = 0;
        curPlayerCount  = 0;
        checkPoint      = 1; // 为了防止刚开始的时候跳过一些刷怪点
        curTotalScore   = 0;
        gameTime        = 0;
        pullWaterCount  = 0;
        pullWaterCount1 = 0;
        pullWaterTime   = 0;
        pullWaterCountdown = 0;
        weaponId        = 0;
        killTypeCondition = new Dictionary<string, KillCondition>();
        killIdCondition   = new Dictionary<int, KillCondition>();
        killIdList        = new List<int>();
        checkPointScores  = new List<int>();
        int id = sceneId * 1000 + difficultyId;
        LevelPO dataPO = LevelData.Instance.GetLevelPO(id);
        if (dataPO == null)
        {
            return false;
        }

        // 暂时先去掉怪物数量的条件判断
        //if (dataPO.MustKillMonster.Length % 2 == 0)
        //{
        //    for (int index = 0; index < dataPO.MustKillMonster.Length; )
        //    {
        //        string name  = dataPO.MustKillMonster[index++];
        //        int    count = dataPO.MustKillMonster[index++].ToInt();
        //        killTypeCondition.Add(name, count);
        //    }
        //}

        if (dataPO.MustKillID.Length % 3 == 0)
        {
            for (int index = 0; index < dataPO.MustKillID.Length; )
            {
                int stage = dataPO.MustKillID[index++].ToInt();
                int key   = dataPO.MustKillID[index++].ToInt();
                int count = dataPO.MustKillID[index++].ToInt();
                killIdCondition.Add(key, new KillCondition(stage,count));
                killIdList.Add(key);
            }
        }

        for (int index = 0; index < dataPO.CheckPointScores.Length; ++index )
        {
            checkPointScores.Add(dataPO.CheckPointScores[index]);
        }

        maxShakeCount   = dataPO.ShakeTimes;
        maxTriggerCount = dataPO.WaterTime;
        maxAoeScore     = dataPO.AOEScores;
        return true;
    }

    public int getConditionScore()
    {
        int value = 0;
        if(checkPointScores.Count > 0)
        {
            value = checkPointScores[checkPointScores.Count-1];
        }
        return value;
    }

    public int getConditionScore(int index)
    {
        return checkPointScores[index - 1];
    }

    public void PushScore(int value,bool aoe = true)
    {
        // 统一添加积分接口
        // 对于AOE 水量的控制 总分阶段控制
        if (aoe)
        {
            curAoeScore += value;
            if (curAoeScore >= maxAoeScore)
            {
                curAoeScore = maxAoeScore;
            }
        }

        // 每局游戏只加一次水
        if (curShakeCount == 0)
        {
            curTriggerCount += value;
        }
        curTotalScore += value;
    }

    public float TotalScorePercent()
    {
        int value = 0;
        if (checkPointScores.Count > 0)
        {
            value = checkPointScores[checkPointScores.Count - 1];
        }
        if (value == 0)
            return 0.0f;

        return (float)curTotalScore / (float)value;
    }
    
    public float TotalWaterPercent()
    {
        if (maxTriggerCount == 0)
            return 0;
        return 1.0f - ((float)curTriggerCount / (float)maxTriggerCount);
    }

    public float TotalPushWaterPercent()
    {
        if (maxShakeCount == 0)
            return 0;
        return curShakeCount / maxShakeCount;
    }

    public void PushWater(int value)
    {
        curShakeCount += value;
        if (curShakeCount > maxShakeCount)
        {
            curShakeCount = maxShakeCount;
            return;
        }

        ++pullWaterCount;
        ++pullWaterCount1;
        bool reward = false;
        if (pullWaterCount == GameConfig.GAME_CONFIG_MAX_PULL_COUNT)
        {
            reward = true;
            pullWaterCount = 0;
            RewardLifeTime(GameConfig.GAME_CONFIG_PULL_REWARD_TIME);
        }
        bool pull = false;
        if (pullWaterCount1 == 5)
        {
            pullWaterCount1 = 0;
            pull = true;
            Main.SoundController.PlayWaterPullHydrantSound();
        }
        pullWaterTime = 0;
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_WATER_PUSHING, reward, pull);

        // 随机播放呼吸声音
        {
            int seed = Random.Range(1, 10000);
            if (seed > 1000)
            {
                return;
            }
            Main.SoundController.PlayWaterPullTiredSound();
        }

    }

    public float TotalAoePercent()
    {
        if (maxAoeScore == 0)
            return 0;
        return (float)curAoeScore / (float)maxAoeScore;
    }
    void OnPerGameJudge()
    {
        if (IsWatering())
        {
            // 判断每次加水是否间隔超时
            OnJudgeAddWaterRewardTimeOut();
        }
    }

    void OnFixGameJudge()
    {
        if (GameConfig.GAME_CONFIG_JUDEG_CONDITION == false)
        {
            return;
        }

        if (IsPlay())
        {
            // 判断游戏人数
            OnJudgeLifeTimeOut();

            // 判断自动触发AOE效果
            OnJudgeCreateWeapon();

            // 判断通关结果
            OnJudgePassCondition();
           
        }

        if(IsWatering() || IsPlay())
        {
            // 判断游戏加水
            OnJudgeAddWaterCondition();
        }

    }

    void OnJudgeLifeTimeOut()
    {
        // 判断游戏人数
        int idleCount = 0;
        int playCount = 0;
        int waitCount = 0;
        float maxContinueTime = 0.0f;
        curPlayerCount = 0;
        for (int index = 0; index < Main.PlayerCount(); ++index)
        {
            Player player = Main.PlayerManager.getPlayer(index);
            if (player.State == Player.StateType.Idle)
            {
                ++idleCount;
            }
            if (player.State == Player.StateType.Play)
            {
                ++playCount;
            }
            if (player.State == Player.StateType.Wait)
            {
                ++waitCount;
                if (maxContinueTime < player.ContinueTime)
                {
                    maxContinueTime = player.ContinueTime;
                }
            }
        }

        curPlayerCount = playCount;

        if (waitCount > 0 && playCount == 0)
        {
            RunState(RunMode.Wait);
            continueTime = maxContinueTime;
        }
        else
        {
            RunState(RunMode.Play);
        }

        if (idleCount == GameConfig.GAME_CONFIG_PLAYER_COUNT)
        {
            StartCoroutine(OnCloseEndingLoss());
        }
    }

    void OnJudgePassCondition()
    {
        // 判断是否通关

        // Check Point判断
        bool pass  = true;
        for (int index = checkPoint; index <= checkPointScores.Count; ++index)
        {
            if (curTotalScore > checkPointScores[index-1])
            {
                checkPoint += 1;
                gameTime    = 0;
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_ENVIRONMENT_MOVE_SPEED_SCALE);
            }
        }
        if (checkPoint <= checkPointScores.Count)
        {
            pass = false;
        }
       
        // 击杀数量
        //foreach (KeyValuePair<string, int> element in killTypeCondition)
        //{
        //    if(element.Value > 0)
        //    {
        //        pass = false;
        //        break;
        //    }
        //}
        // 击杀条件
        for (int index = 0; index < killIdList.Count; ++index)
        {
            if (killIdCondition.ContainsKey(killIdList[index]))
            {
                KillCondition element = killIdCondition[killIdList[index]];
                if (element.count > 0)
                {
                    pass = false;
                    break;
                }
            }
        }

        if (!pass)
        {
            return;
        }

        StartCoroutine(OnCloseEndingSuccess());
    }

    void OnEventMonsterDeath(int MonsterType, int MonsterId)
    {
        //string typeName = "";
        //if (MonsterType == (int)Monster.MonsterType.Normal || 
        //    MonsterType == (int)Monster.MonsterType.Senior)
        //{
        //    typeName = "Monster";
        //}
        //else if (MonsterType == (int)Monster.MonsterType.Boss)
        //{
        //    typeName = "Boss";
        //}
        //else if (MonsterType == (int)Monster.MonsterType.NPC)
        //{
        //    typeName = "NPC";
        //}
        //if (killTypeCondition.ContainsKey(typeName))
        //{
        //    int killCount = killTypeCondition[typeName];
        //    killCount -= 1;
        //    if (killCount < 0)
        //        killCount = 0;
        //    killTypeCondition[typeName] = killCount;
        //}

        if (killIdCondition.ContainsKey(MonsterId))
        {
            KillCondition killCount = killIdCondition[MonsterId];
            if (killCount.type != checkPoint)
                return;
            killCount.count -= 1;
            if (killCount.count < 0)
                killCount.count = 0;
            killIdCondition[MonsterId] = killCount;
        }
    }

    void OnJudgeAddWaterRewardTimeOut()
    {
        if (pullWaterTime >= GameConfig.GAME_CONFIG_WAIT_PULL_TIME)
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_WATER_URGING);
            //pullWaterCount = 0;
        }
        pullWaterTime += Main.NonStopTime.deltaTime;
    }

    void OnJudgeAddWaterCondition()
    {
        if (RunState() == RunMode.Play && maxShakeCount > 0 && maxTriggerCount > 0 && curShakeCount == 0)
        {
            if (curTriggerCount >= maxTriggerCount)
            {
                RunState(RunMode.Water);
                // 打开没水警告
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_WATER_EMPTY_CAUTION);
                // 启动警告等待
                StartCoroutine(OnCheckWaterEmptyCaution());
                // 播放警告声音
                Main.SoundController.PlayWaterWarningSound();
            }
            return;
        }

        if (RunState() == RunMode.Watering && maxShakeCount <= curShakeCount)
        {
            StartCoroutine(OnCheckWaterFull());
            return;
		}else if (RunState() == RunMode.Watering && pullWaterCountdown <= 0.0f)
        {
			RewardLifeTime(GameConfig.GAME_CONFIG_PULL_REWARD_TIME);
            StartCoroutine(OnCheckWaterFull());
            return;
        }
        else
        {
			pullWaterCountdown -= Main.NonStopTime.noStopTime;
        }
    }

    IEnumerator OnCheckWaterFull()
    {
        float waitTime = 0;
        while (waitTime < 1.5f)
        {
            yield return null;
            waitTime += Main.NonStopTime.deltaTime;
        }

        EventDispatcher.TriggerEvent(GameEventDef.EVNET_WATER_FULL);
        PauseGame(false);
        RunState(RunMode.Play);
        curTriggerCount = 0;
        Main.SoundController.StopWaterWarningSound();
    }

    IEnumerator OnCheckWaterEmptyCaution()
    {
        yield return new WaitForSeconds(2);
        PauseGame(true);
        RunState(RunMode.Watering);
        pullWaterTime = 0;
        pullWaterCountdown = 28.0f;
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_WATER_WATERING);
    }

    void OnJudgeCreateWeapon()
    {
        // 可以优化的代码
        // 让怪物自行处理一个被命中AOE的处理
        if (maxAoeScore > curAoeScore)
        {
            return;
        }
        curAoeScore = 0;
        
        // 创建AOE物品对象
        //场景一：51073  -1.09,0.825,0
        //场景二：52051  -0.978,0.935,5.857
        //场景三：53012  -1.065,0.794,3.024
        Vector3 pos = Vector3.zero;
        int id = 0;
        if (sceneName == SceneType.Scene1)
        {
            pos = new Vector3(-1.09f, 0.825f, 0.0f);
            id  = 51073;
        }
        else if (sceneName == SceneType.Scene2)
        {
            pos = new Vector3(-0.978f, 0.935f, 5.857f);
            id = 52051;
        }
        else if (sceneName == SceneType.Scene3)
        {
            pos = new Vector3(1.065f, 0.794f, 3.024f);
            id = 53012;
        }
        SceneManager.instance.CreateMonster(id, pos, null, 0.0f);
    }

    public void ActivateWeaponSkill(Player player)
    {
        int totalScore = 0;
        List<string> targetNameList = new List<string>();
        SceneManager.instance.getAllLifeMonsterName(out targetNameList);
        for (int index = 0; index < targetNameList.Count; ++index)
        {
            Monster go = SceneManager.instance.PickMonster(targetNameList[index]);
            if (go == null)
            {
                continue;
            }

            if (go.Type == Monster.MonsterType.NPC || go.Type == Monster.MonsterType.Weapon)
            {
                continue;
            }

            go.OnSprayWater(GameConfig.GAME_CONFIG_WATER_DAMAGE_3, player);
            go.OnSprayWaterHitting(null, GameConfig.GAME_CONFIG_WATER_DAMAGE_3);
            int value = go.ObtainScore();
            if (value > 0)
            {
                totalScore += value;
                //Main.SoundController.PlayGetPointSound();
            }
        }

        //for (int index = 0; index < Main.PlayerCount(); ++index)
        //{
        //    Player player = Main.PlayerManager.getPlayer(index);
        //    if (player.State != Player.StateType.Play || !player.AoeFlag)
        //    {
        //        continue;
        //    }
        //    player.IncreaseScore(totalScore);
        //    PushScore(totalScore, false);
        //    break;
        //}

        player.IncreaseScore(totalScore);
        PushScore(totalScore, false);
        
        GameObject goEffect = GameObject.Find("Point/CameraEffect");
        if (goEffect != null)
        {
            GameObject effect = SkillEffectManager.instance.CastEffect(EffectType.AOE, goEffect.transform.position, 5.0f);
            effect.transform.parent = goEffect.transform;
        }

        Main.SoundController.PlaySkillAoeSound();
    }

}

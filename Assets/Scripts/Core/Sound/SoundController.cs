using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;


public class SoundController : MonoBehaviour
{

    [SerializeField]
    public SoundLibrary library;

    public SoundLibrary Library
    {
        get { return library; }
        set { library = value; }
    }

    static int soundIDCounter = 0;

    static int GetSoundID()
    {
        soundIDCounter++;
        return soundIDCounter;
    }

    private Dictionary<string, SoundEvent> eventsDict = new Dictionary<string, SoundEvent>();
    private int standByMusicId;
    private int standByMusicIndex;
    private int sceneMusicId;
    private int sceneEffectId;
    private int sceneTempId;
    private int waterWarningId;
    private int endingAccountId;

    // Use this for initialization
    public void Init()
    {
        sceneMusicId        = -1;
        sceneTempId         = 0;
        standByMusicId      = -1;
        standByMusicIndex   = 0;
        waterWarningId      = -1;
        endingAccountId     = -1;
        library.Init();
        InitData();
    }

    void InitData()
    {
        AddSoundEvent("StartMusic", "name=Music_StandBy_Start_01", "sound=SFX_Standby_01", "startEndVolume=0,1", "fadeTime=1", "loop=false");
        AddSoundEvent("StopMusic", "name=Music_StandBy_Stop_01", "sound=SFX_Standby_01", "fadeTime=0");
        AddSoundEvent("StartMusic", "name=Music_StandBy_Start_02", "sound=SFX_Standby_02", "startEndVolume=0,1", "fadeTime=1", "loop=false");
        AddSoundEvent("StopMusic", "name=Music_StandBy_Stop_02", "sound=SFX_Standby_02", "fadeTime=0");
        AddSoundEvent("StartMusic", "name=Music_StandBy_Start_03", "sound=SFX_Standby_03", "startEndVolume=0,1", "fadeTime=1", "loop=false");
        AddSoundEvent("StopMusic", "name=Music_StandBy_Stop_03", "sound=SFX_Standby_03", "fadeTime=0");
        AddSoundEvent("StartMusic", "name=Music_StandBy_Start_04", "sound=SFX_Standby_04", "startEndVolume=0,1", "fadeTime=1", "loop=false");
        AddSoundEvent("StopMusic", "name=Music_StandBy_Stop_04", "sound=SFX_Standby_04", "fadeTime=0");
        AddSoundEvent("StartMusic", "name=Music_Scene_Start_01", "sound=SFX_Scene1", "startEndVolume=0,1", "fadeTime=1", "loop=true");
        AddSoundEvent("StopMusic", "name=Music_Scene_Stop_01", "sound=SFX_Scene1", "fadeTime=0");
        AddSoundEvent("StartMusic", "name=Music_Scene_Start_02", "sound=SFX_Scene2", "startEndVolume=0,1", "fadeTime=1", "loop=true");
        AddSoundEvent("StopMusic", "name=Music_Scene_Stop_02", "sound=SFX_Scene2", "fadeTime=0");
        AddSoundEvent("StartMusic", "name=Music_Scene_Start_03", "sound=SFX_Scene3", "startEndVolume=0,1", "fadeTime=1", "loop=true");
        AddSoundEvent("StopMusic", "name=Music_Scene_Stop_03", "sound=SFX_Scene3", "fadeTime=0");

        AddSoundEvent("StartMusic", "name=Music_Fire_Start_01", "sound=SFX_Fire_01", "startEndVolume=0,1", "fadeTime=1", "loop=true");
        AddSoundEvent("StopMusic", "name=Music_Fire_Stop_01", "sound=SFX_Fire_01", "fadeTime=0");
        AddSoundEvent("StartMusic", "name=Music_Fire_Start_02", "sound=SFX_Fire_02", "startEndVolume=0,1", "fadeTime=1", "loop=true");
        AddSoundEvent("StopMusic", "name=Music_Fire_Stop_02", "sound=SFX_Fire_02", "fadeTime=0");
        AddSoundEvent("StartMusic", "name=Music_Fire_Start_03", "sound=SFX_Fire_03", "startEndVolume=0,1", "fadeTime=1", "loop=true");
        AddSoundEvent("StopMusic", "name=Music_Fire_Stop_03", "sound=SFX_Fire_03", "fadeTime=0");

        AddSoundEvent("StartSoundLoop", "name=Sound_UI_WaterWarning_Start", "sound=SFX_UI_WaterWarning");
        AddSoundEvent("StopSoundLoop", "name=Sound_UI_WaterWarning_Stop");

        AddSoundEvent("StartSoundLoop", "name=Sound_UI_EndingAccount_Start", "sound=SFX_UI_Account");
        AddSoundEvent("StopSoundLoop", "name=Sound_UI_EndingAccount_Stop");

        AddSoundEvent("StartSoundLoop", "name=Sound_UI_Rank_Start", "sound=SFX_Rank");
        AddSoundEvent("StopSoundLoop", "name=Sound_UI_Rank_Stop");

        AddSoundEvent("PlaySound", "name=SFX_UI_Lose", "sound=SFX_UI_Lose");
        AddSoundEvent("PlaySound", "name=SFX_UI_Win", "sound=SFX_UI_Win");
        AddSoundEvent("PlaySound", "name=SFX_UI_InsertCoin", "sound=SFX_UI_InsertCoin");
        AddSoundEvent("PlaySound", "name=SFX_UI_CountDown", "sound=SFX_UI_CountDown");
        AddSoundEvent("PlaySound", "name=SFX_UI_GetReady", "sound=SFX_UI_GetReady");
        AddSoundEvent("PlaySound", "name=SFX_UI_Start", "sound=SFX_UI_Start");
        AddSoundEvent("PlaySound", "name=SFX_UI_Key", "sound=SFX_UI_Key");
        AddSoundEvent("PlaySound", "name=SFX_UI_CrashScreen", "sound=SFX_UI_CrashScreen");
        AddSoundEvent("PlaySound", "name=SFX_UI_GetPoint", "sound=SFX_UI_GetPoint");

        AddSoundEvent("PlaySound", "name=SFX_Voice_Welcome", "sound=SFX_Voice_Welcome");
        AddSoundEvent("PlaySound", "name=SFX_Voice_Water", "sound=SFX_Voice_Water");
        AddSoundEvent("PlaySound", "name=SFX_Voice_MissionOver", "sound=SFX_Voice_MissionOver");
        AddSoundEvent("PlaySound", "name=SFX_Voice_GameOver", "sound=SFX_Voice_GameOver");
        AddSoundEvent("PlaySound", "name=SFX_Voice_Success", "sound=SFX_Voice_Success");
        AddSoundEvent("PlaySound", "name=SFX_Voice_TimeUp", "sound=SFX_Voice_TimeUp");
        AddSoundEvent("PlaySound", "name=SFX_Voice_Name", "sound=SFX_Voice_Name");
        AddSoundEvent("PlaySound", "name=SFX_Voice_AOE", "sound=SFX_Voice_AOE");
        AddSoundEvent("PlaySound", "name=SFX_Voice_WellDone", "sound=SFX_Voice_WellDone");
        AddSoundEvent("PlaySound", "name=SFX_Voice_Circle", "sound=SFX_Voice_Circle");
        AddSoundEvent("PlaySound", "name=SFX_Voice_FireBall", "sound=SFX_Voice_FireBall");
        AddSoundEvent("PlaySound", "name=SFX_Voice_Attack", "sound=SFX_Voice_Attack");
        AddSoundEvent("PlaySound", "name=SFX_Voice_Animal", "sound=SFX_Voice_Animal");
        AddSoundEvent("PlaySound", "name=SFX_Voice_BeAttacked", "sound=SFX_Voice_BeAttacked"); 
        AddSoundEvent("PlaySound", "name=SFX_Voice_Mission1Start", "sound=SFX_Voice_Mission1Start"); 
        AddSoundEvent("PlaySound", "name=SFX_Voice_Mission2Start", "sound=SFX_Voice_Mission2Start"); 
        AddSoundEvent("PlaySound", "name=SFX_Voice_Boss", "sound=SFX_Voice_Boss");
        AddSoundEvent("PlaySound", "name=SFX_Voice_Tired", "sound=SFX_Voice_Tired");
        AddSoundEvent("PlaySound", "name=SFX_UI_AOE", "sound=SFX_UI_AOE");
        AddSoundEvent("PlaySound", "name=SFX_UI_HitCircle", "sound=SFX_UI_HitCircle");

        AddSoundEvent("PlaySound", "name=SFX_Voice_BabyCitizen", "sound=SFX_Voice_BabyCitizen");
        AddSoundEvent("PlaySound", "name=SFX_Voice_BoyCitizen", "sound=SFX_Voice_BoyCitizen");
        AddSoundEvent("PlaySound", "name=SFX_Voice_GirlCitizen", "sound=SFX_Voice_GirlCitizen");
		AddSoundEvent("PlaySound", "name=SFX_Monster_Cat", "sound=SFX_Monster_Cat");
		AddSoundEvent("PlaySound", "name=SFX_Monster_Dog", "sound=SFX_Monster_Dog");

        AddSoundEvent("PlaySound", "name=SFX_Voice_FireMonster01", "sound=SFX_Voice_FireMonster01");
        AddSoundEvent("PlaySound", "name=SFX_Voice_FireMonster02", "sound=SFX_Voice_FireMonster02");
		AddSoundEvent("PlaySound", "name=SFX_Voice_FireMonster03", "sound=SFX_Voice_FireMonster03");
        AddSoundEvent("PlaySound", "name=SFX_Voice_SmokeMonster01", "sound=SFX_Voice_SmokeMonster01");
        AddSoundEvent("PlaySound", "name=SFX_Voice_SmokeMonster02", "sound=SFX_Voice_SmokeMonster02");
		AddSoundEvent("PlaySound", "name=SFX_Voice_SmokeMonster03", "sound=SFX_Voice_SmokeMonster03");
        AddSoundEvent("PlaySound", "name=SFX_Voice_BFireMonster01", "sound=SFX_Voice_BFireMonster01");
        AddSoundEvent("PlaySound", "name=SFX_Voice_BFireMonster02", "sound=SFX_Voice_BFireMonster02");
		AddSoundEvent("PlaySound", "name=SFX_Voice_BFireMonster03", "sound=SFX_Voice_BFireMonster03");
		AddSoundEvent("PlaySound", "name=SFX_Voice_BossDead", "sound=SFX_Voice_BossDead");
        AddSoundEvent("PlaySound", "name=SFX_UI_FireBall", "sound=SFX_UI_FireBall");
        AddSoundEvent("PlaySound", "name=SFX_UI_Attack", "sound=SFX_UI_Attack");
        AddSoundEvent("PlaySound", "name=SFX_UI_PreSummon", "sound=SFX_UI_PreSummon");
        AddSoundEvent("PlaySound", "name=SFX_UI_Summon","sound=SFX_UI_Summon");
        AddSoundEvent("PlaySound", "name=SFX_UI_Water_Hydrant", "sound=SFX_UI_Water_Hydrant");    

        // 英文版本
        AddSoundEvent("PlaySound", "name=SFX_Voice_Animal_En", "sound=SFX_Voice_Animal_En"); //--
        AddSoundEvent("PlaySound", "name=SFX_Voice_AOE_En", "sound=SFX_Voice_AOE_En");//---
        AddSoundEvent("PlaySound", "name=SFX_Voice_BabyCitizen_En", "sound=SFX_Voice_BabyCitizen_En"); //--
        AddSoundEvent("PlaySound", "name=SFX_Voice_BeAttacked_En", "sound=SFX_Voice_BeAttacked_En");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_BFireMonster_En01", "sound=SFX_Voice_BFireMonster_En01"); //--
        AddSoundEvent("PlaySound", "name=SFX_Voice_BFireMonster_En02", "sound=SFX_Voice_BFireMonster_En02");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_BFireMonster_En03", "sound=SFX_Voice_BFireMonster_En03");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_Boss_En", "sound=SFX_Voice_Boss_En");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_BossDead_En", "sound=SFX_Voice_BossDead_En");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_BoyCitizen_En", "sound=SFX_Voice_BoyCitizen_En");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_Circle_En", "sound=SFX_Voice_Circle_En"); // ---
        AddSoundEvent("PlaySound", "name=SFX_Voice_FireBall_En", "sound=SFX_Voice_FireBall_En");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_FireMonster_En01", "sound=SFX_Voice_FireMonster_En01");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_FireMonster_En02", "sound=SFX_Voice_FireMonster_En02");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_FireMonster_En03", "sound=SFX_Voice_FireMonster_En03");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_GameOver_En", "sound=SFX_Voice_GameOver_En");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_GirlCitizen_En", "sound=SFX_Voice_GirlCitizen_En"); //--
        AddSoundEvent("PlaySound", "name=SFX_Voice_Mission1Start_En", "sound=SFX_Voice_Mission1Start_En"); // ---
        AddSoundEvent("PlaySound", "name=SFX_Voice_Mission2Start_En", "sound=SFX_Voice_Mission2Start_En"); // ---
        AddSoundEvent("PlaySound", "name=SFX_Voice_MissionOver_En", "sound=SFX_Voice_MissionOver_En"); //----------------
        AddSoundEvent("PlaySound", "name=SFX_Voice_Name_En", "sound=SFX_Voice_Name_En");//----
        AddSoundEvent("PlaySound", "name=SFX_Voice_SmokeMonster_En01", "sound=SFX_Voice_SmokeMonster_En01");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_SmokeMonster_En02", "sound=SFX_Voice_SmokeMonster_En02");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_SmokeMonster_En03", "sound=SFX_Voice_SmokeMonster_En03");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_Success_En", "sound=SFX_Voice_Success_En"); // --
        AddSoundEvent("PlaySound", "name=SFX_Voice_TimeUp_En", "sound=SFX_Voice_TimeUp_En");//----
        AddSoundEvent("PlaySound", "name=SFX_Voice_Tired_En", "sound=SFX_Voice_Tired_En");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_Water_En", "sound=SFX_Voice_Water_En");//---
        AddSoundEvent("PlaySound", "name=SFX_Voice_Welcome_En", "sound=SFX_Voice_Welcome_En");//--
        AddSoundEvent("PlaySound", "name=SFX_Voice_WellDone_En", "sound=SFX_Voice_WellDone_En");// --
    }

    public void ResetVolumeScale()
    {
        SoundEvent.volumeScale = (float)Main.SettingManager.GameVolume / 10.0f;
    }

    void Update()
    {
        SoundEvent.CleanUpChannels();
        OnRandomMenuMusic();
    }

    public AudioChannel GetAudioChannel(int id)
    {
        return SoundEvent.GetAudioChannel(id);
    }

    public int FireEvent(string eventName, int id = -1)
    {
        if (eventsDict.ContainsKey(eventName))
        {
            SoundEvent e = eventsDict[eventName];

            id = e.FireEvent(id);
        }
        else
        {
            Log.Hsz("Warning: Firing sound event that doesn't exist - " + eventName);
        }

        return id;
    }

    public void AddSoundEvent(string eventType, params string[] paramlist)
    {
        SoundEvent e = null;

        switch (eventType)
        {
            case "PlaySound":
                e = new PlaySFXEvent();
                break;
            case "StartSoundLoop":
                e = new StartSFXLoopEvent();
                break;
            case "StopSoundLoop":
                e = new StopSFXLoopEvent();
                break;
            case "StartMusic":
                e = new StartMusicEvent();
                break;
            case "StopMusic":
                e = new StopMusicEvent();
                break;
            case "FadeMusic":
                e = new FadeMusicEvent();
                break;
        }

        if (e != null)
        {
            e.Deserialize(paramlist);

            if (eventsDict.ContainsKey(e.name))
            {
                eventsDict[e.name].clip = library.GetSoundFromLibrary(e.soundName);
                eventsDict[e.name].library = library;
            }
            else
            {
                e.clip = library.GetSoundFromLibrary(e.soundName);
                e.library = library;
                eventsDict.Add(e.name, e);
            }
        }
    }

    public void StartRandomMenuMusic()
    {
        standByMusicIndex = Random.Range(1, 5);
        string name = "Music_StandBy_Start_0" + standByMusicIndex.ToString();
        standByMusicId = FireEvent(name);
    }

    public void StopRandomMenuMusic()
    {
        if (standByMusicId == -1)
        {
            return;
        }
        string name = "Music_StandBy_Stop_0" + standByMusicIndex.ToString();
        FireEvent(name, standByMusicId);
        standByMusicId = -1;
    }

    void OnRandomMenuMusic()
    {
        if (standByMusicId == -1)
        {
            return;
        }

        AudioChannel channel = GetAudioChannel(standByMusicId);
        if (!channel.source.isPlaying)
        {
            StopRandomMenuMusic();
            StartRandomMenuMusic();
        }
    }

    public void StartSceneMusic(int sceneId)
    {
        sceneTempId = sceneId;
        string name = "Music_Scene_Start_0" + sceneTempId.ToString();
        sceneMusicId = FireEvent(name);
        name = "Music_Fire_Start_0" + sceneTempId.ToString();
        sceneEffectId = FireEvent(name);
    }


    public void StopSceneMusic()
    {
        if (sceneMusicId == -1)
        {
            return;
        }

        string name = "Music_Scene_Stop_0" + sceneTempId.ToString();
        FireEvent(name, sceneMusicId);
        name = "Music_Fire_Stop_0" + sceneTempId.ToString();
        FireEvent(name, sceneEffectId);
        sceneMusicId  = -1;
        sceneEffectId = -1;
        sceneTempId   = 0;
    }

    public void PlayWinSound()
    {
        FireEvent("SFX_UI_Win");
    }

    public void PlayMissionPassSound()
    {
        if (Main.SettingManager.GameLanguage == 0)
        {
            FireEvent("SFX_Voice_MissionOver");
        }
        else
        {
            FireEvent("SFX_Voice_MissionOver_En");
        }
    }

    public void PlayLoseSound()
    {
        FireEvent("SFX_UI_Lose");
        if (Main.SettingManager.GameLanguage == 0)
        {
            FireEvent("SFX_Voice_GameOver");
        }
        else
        {
            FireEvent("SFX_Voice_GameOver_En");
        }
    }

    public void PlayGameSuccessSound()
    {
        if (Main.SettingManager.GameLanguage == 0)
        {
            FireEvent("SFX_Voice_Success");
        }
        else
        {
            FireEvent("SFX_Voice_Success_En");
        }
    }

    public void PlayTimeOverSound()
    {
        if (Main.SettingManager.GameLanguage == 0)
        {
            FireEvent("SFX_Voice_TimeUp");
        }
        else
        {
            FireEvent("SFX_Voice_TimeUp_En");
        }
    }

    public void PlayInsertCoinSound()
    {
        FireEvent("SFX_UI_InsertCoin");
    }

    public void PlayCountDownSound()
    {
        FireEvent("SFX_UI_CountDown");
    }

    public void PlayGetReadySound()
    {
        FireEvent("SFX_UI_GetReady");
    }

    public void PlayGetStartSound()
    {
        FireEvent("SFX_UI_Start");
    }

    public void PlayConfirmSound()
    {
        FireEvent("SFX_UI_Key");
    }

    public void PlayCrashScreenSound()
    {
        FireEvent("SFX_UI_CrashScreen");
    }

    public void PlayGetPointSound()
    {
        FireEvent("SFX_UI_GetPoint");
    }

    public void PlayWaterWarningSound()
    {
        if (Main.SettingManager.GameLanguage == 0)
        {
            FireEvent("SFX_Voice_Water");
        }
        else
        {
            FireEvent("SFX_Voice_Water_En");
        }
        waterWarningId = FireEvent("Sound_UI_WaterWarning_Start");
    }

    public void StopWaterWarningSound()
    {
        if (waterWarningId != -1)
        {
            FireEvent("Sound_UI_WaterWarning_Stop", waterWarningId);
            waterWarningId = -1;
        }
    }

    public void PlayEndingAccountSound()
    {
        endingAccountId = FireEvent("Sound_UI_EndingAccount_Start");
    }

    public void StopEndingAccountSound()
    {
        FireEvent("Sound_UI_EndingAccount_Stop", endingAccountId);
        endingAccountId = -1;
    }

    public void PlayRankSound()
    {
        endingAccountId = FireEvent("Sound_UI_Rank_Start");
    }

    public void StopRankSound()
    {
        FireEvent("Sound_UI_Rank_Stop", endingAccountId);
        endingAccountId = -1;
    }
    
    public void PlayWelcomeSound()
    {
        if (Main.SettingManager.GameLanguage == 0)
        {
            FireEvent("SFX_Voice_Welcome");
        }
        else
        {
            FireEvent("SFX_Voice_Welcome_En");
        }
    }

    public void PlayEditNameSound()
    {
        if (Main.SettingManager.GameLanguage == 0)
        {
            FireEvent("SFX_Voice_Name");
        }
        else
        {
            FireEvent("SFX_Voice_Name_En");
        }
    }
        
    public void PlaySkillAoeSound()
    {
        if (Main.SettingManager.GameLanguage == 0)
        {
            FireEvent("SFX_Voice_AOE");
        }
        else
        {
            FireEvent("SFX_Voice_AOE_En");
        }
        FireEvent("SFX_UI_AOE");
    }

    public void PlaySkillBreakSound()
    {
        if (Main.SettingManager.GameLanguage == 0)
        {
            FireEvent("SFX_Voice_WellDone");
        }
        else
        {
            FireEvent("SFX_Voice_WellDone_En");
        }
    }

    public void PlayAttackBossBodySound()
    {
        FireEvent("SFX_Voice_Attack");
    }

    public void PlayPassConditionSound(int sceneId)
    {
        if (sceneId == (int)SceneManager.SceneLevelID.Level_1)
        {
            if (Main.SettingManager.GameLanguage == 0)
            {
                FireEvent("SFX_Voice_Mission1Start");
            }
            else
            {
                FireEvent("SFX_Voice_Mission1Start_En");
            }
        }
        else if(sceneId == (int)SceneManager.SceneLevelID.Level_2)
        {
            if (Main.SettingManager.GameLanguage == 0)
            {
                FireEvent("SFX_Voice_Mission2Start");
            }
            else
            {
                FireEvent("SFX_Voice_Mission2Start_En");
            }
        }
        else if(sceneId == (int)SceneManager.SceneLevelID.Level_3)
        {
            if (Main.SettingManager.GameLanguage == 0)
            {
                FireEvent("SFX_Voice_Boss");
            }
            else
            {
                FireEvent("SFX_Voice_Boss_En");
            }
        }
    }

    public void PlayWaterPullTiredSound()
    {
        if (Main.SettingManager.GameLanguage == 0)
        {
            FireEvent("SFX_Voice_Tired");
        }
        else
        {
            FireEvent("SFX_Voice_Tired_En");
        }
    }

    public void PlayWaterPullHydrantSound()
    {
        FireEvent("SFX_UI_Water_Hydrant");
    }

    public void PlayHitCircleSound()
    {
        FireEvent("SFX_UI_HitCircle");
    }    
}
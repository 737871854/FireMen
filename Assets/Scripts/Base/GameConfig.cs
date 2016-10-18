using System;
using UnityEngine;
using System.Collections.Generic;

namespace Need.Mx
{
    public class GameConfig
    {

        #region---------------------------配置文件路径常量------------------------------
        public const string DIFFICULTY_LEVEL_JSON   = "Json/Level.json";
        public const string MONSTER_JSON            = "Json/Monster.json";
        public const string MONSTER_REFRESH_JSON    = "Json/MonsterRefresh.json";
        public const string SKILL_JSON              = "Json/Skill.json";
        public const string SKILL_EFFECT_JSON       = "Json/SkillEffect.json";
        public const string MONSTER_EN_JSON         = "Json/Monster_En.json";
        public const string SKILL_EN_JSON           = "Json/Skill_En.json";
        public const string SETTING_COINFIG         = "Json/SettingConfig.json";

        #endregion

        #region---------------------------内部定义游戏常量------------------------------
        public static int   GAME_CONFIG_LANGUAGE                = 0;
        public static int   GAME_CONFIG_DIFFICULTY              = 1;
        public static int   GAME_CONFIG_PLAYER_COUNT            = 3;
        public static int   GAME_CONFIG_PLAYER_1                = 0;
        public static int   GAME_CONFIG_PLAYER_2                = 1;
        public static int   GAME_CONFIG_PLAYER_3                = 2;
        public static float GAME_CONFIG_VOLUME                  = 1.0f;
        public static float GAME_CONFIG_WATER_SHOW              = 1;
        public static int   GAME_CONFIG_NAME_LEN                = 3;
        public static int   GAME_CONFIG_PER_CONSUME_WATER       = 1;
        public static int   GAME_CONFIG_MAX_SCORE               = 99999;
        public static int   GAME_CONFIG_MAX_CAR                 = 3;          // 消防车数量
        public static int   GAME_CONFIG_MAX_LIFE_TIME           = 90;         // 游戏时间
        public static int   GAME_CONFIG_MAX_WAIT_TIME           = 10;         // 续币时间
        public static int   GAME_CONFIG_MAX_COIN                = 99;         // 最大币数
        public static int   GAME_CONFIG_PER_USE_COIN            = 3;          // 每次币数
        public static float GAME_CONFIG_SELECT_WAIT_TIME        = 9.0f;       // 等待时间
        public static float GAME_CONFIG_RANK_WAIT_TIME          = 30.0f;      // 等待时间
        public static int   GAME_CONFIG_WATER_DAMAGE_1          = 1;          // 玩家喷水攻击力，对怪物造成伤害
        public static int   GAME_CONFIG_WATER_DAMAGE_2          = 5;          // 玩家喷水攻击力，对怪物造成伤害
        public static int   GAME_CONFIG_WATER_DAMAGE_3          = 10;         // 玩家AOE攻击力 ，对怪物造成伤害
        public static float GAME_CONFIG_WATER_DAMAGE_INTERVAL   = 0.3f;       // 玩家攻击造成伤害的间隔（毫秒）
        public static float GAME_CONFIG_ADD_WATER_TIEM          = 3.0f;       // 水枪加水时间（毫秒）
        public static int   GAME_CONFIG_FULL_WATER              = 15;         // 水枪满水能够攻击的次数
        public static float GAME_CONFIG_SHOW_HEAD_UI_TIME_1     = 3.0f;       // 怪物X毫秒没受到攻击血条暂时消失
        public static float GAME_CONFIG_SHOW_HEAD_UI_TIME_2     = 3.0f;       // 友方怪物X毫秒没被喷水，救援进度条暂时消失
        public static float GAME_CONFIG_CLOSE_DOOR_TIME         = 0.5f;       // 门窗关闭时间（毫秒）
        public static float GAME_CONFIG_DAMAGE_LIFE_TIME        = 1.0f;       // 遭遇伤害时减少生命时间
        public static int   GAME_CONFIG_MAX_RANK_ITEM           = 5;          // 排行榜个数
        public static int   GAME_CONFIG_MAX_PULL_COUNT          = 50;
        public static float GAME_CONFIG_WAIT_PULL_TIME          = 3.0f;
        public static float GAME_CONFIG_PULL_REWARD_TIME        = 90.0f;
        public static int   GAME_CONFIG_WEAPON_HP               = 3;
        public static float GAME_CONFIG_WEAPON_MAX_X            = 1100;
        public static float GAME_CONFIG_WEAPON_MIN_X            = -1100; 
        public static bool  GAME_CONFIG_JUDEG_CONDITION         = true;       // 是否进入条件判断(调试使用)
        public static float GAME_CONFIG_IDLE_TIME               = 60.0f;      // 是否进入条件判断(调试使用)
        public static float GAME_CONFIG_HALF_WIDTH_PLAYER_0     = 128;        // 玩家1机械横向运动半径
        public static float GAME_CONFIG_HALF_WIDTH_PLAYER_1     = 128;        // 玩家2机械横向运动半径
        public static float GAME_CONFIG_HALF_WIDTH_PLAYER_2     = 128;        // 玩家3机械横向运动半径
        public static float GAME_CONFIG_HALF_HEIGHT_PLAYER_0    = 128;        // 玩家1机械纵向运动半径
        public static float GAME_CONFIG_HALF_HEIGHT_PLAYER_1    = 128;        // 玩家2机械纵向运动半径
        public static float GAME_CONFIG_HALF_HEIGHT_PLAYER_2    = 128;        // 玩家3机械纵向运动半径
        public static float GAME_CONFIG_HAS_NO_CHECK_TIME       = 7800;       // 校验信号允许最大时间间隔 

        public static List<float[]> GAME_CONFIG_POINTS_POSES_0 = new List<float[]>();
        public static List<float[]> GAME_CONFIG_POINTS_POSES_1 = new List<float[]>();
        public static List<float[]> GAME_CONFIG_POINTS_POSES_2 = new List<float[]>();
        public static float[] GAME_CONFIG_SCREEN_WIDTH_HEIGHT;

        public static string GAME_CONFIG_CHECK_ID               = "GAME_CONFIG_CHECK_ID";    // 协议校验ID
        public static string GAME_CONFIG_ID                     = null;

        #endregion 

        /// <summary>
        /// 初始化函数
        /// </summary>
        public static void ParsingGameConfig()
        {
            GAME_CONFIG_LANGUAGE                = Main.SettingManager.GameLanguage;
            GAME_CONFIG_DIFFICULTY              = Main.SettingManager.GameLevel;
            GAME_CONFIG_PER_USE_COIN            = Main.SettingManager.GameRate;
            GAME_CONFIG_VOLUME                  = Main.SettingManager.GameVolume;
            GAME_CONFIG_WATER_SHOW              = Main.SettingManager.WaterShow;
            GAME_CONFIG_HALF_WIDTH_PLAYER_0     = Main.SettingManager.GetPlayerScreenInfo(0)[0];
            GAME_CONFIG_HALF_WIDTH_PLAYER_1     = Main.SettingManager.GetPlayerScreenInfo(1)[0];
            GAME_CONFIG_HALF_WIDTH_PLAYER_2     = Main.SettingManager.GetPlayerScreenInfo(2)[0];
            GAME_CONFIG_HALF_HEIGHT_PLAYER_0    = Main.SettingManager.GetPlayerScreenInfo(0)[1];
            GAME_CONFIG_HALF_HEIGHT_PLAYER_1    = Main.SettingManager.GetPlayerScreenInfo(1)[1];
            GAME_CONFIG_HALF_HEIGHT_PLAYER_2    = Main.SettingManager.GetPlayerScreenInfo(2)[1];
            GAME_CONFIG_POINTS_POSES_0          = Main.SettingManager.GetPoint(0);
            GAME_CONFIG_POINTS_POSES_1          = Main.SettingManager.GetPoint(1);
            GAME_CONFIG_POINTS_POSES_2          = Main.SettingManager.GetPoint(2);
            GAME_CONFIG_SCREEN_WIDTH_HEIGHT     = Main.SettingManager.GetScreenInfo();
            GAME_CONFIG_ID                      = Main.SettingManager.CheckID;
        }
    }
}

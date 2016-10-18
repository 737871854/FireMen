/**
* Copyright (c) 2015,Need Corp. ltd;
* All rights reserved.
* 
* 文件名称：GlabalDef.cs
* 简    述：不需要和服务器保持一致的常量和枚举定义存放处。
* 创建标识：Terry  2015/08/10
* 修改标识：
* 修改描述：
*/

using System;
using UnityEngine;
using System.Collections.Generic;

namespace Need.Mx
{
    /// <summary>
    /// Global def.和全局常数定义都放在这个位置上。
    /// </summary>
    public class GlobalDef
    {
       
    }

    /// <summary>
    /// 当前程序运行的模式
    /// </summary>
    public enum RunMode
    {
        Start,
        Select,
        Play,
        Wait,
        Water,
        Watering,
        Load,
        Loading,
        MovieStop,
        MoviePlay,
        EndingLoss,
        EndingSuccess,
        EndingEdit,
        EndingTop,
        Setting,
        Idle,
        UnKnown
    }

    public class SceneType
    {
        public const string SceneStart  = "Start";
        public const string SceneSelect = "Select";
        public const string Scene1      = "Play1";
        public const string Scene2      = "Play2";
        public const string Scene3      = "Play3";
        public const string Scene1Start = "Play1Start";
        public const string Scene1Over  = "Play1Over";
        public const string Scene2Start = "Play2Start";
        public const string Scene2Over  = "Play2Over";
        public const string Scene3Start = "Play3Start";
        public const string Scene3Over  = "Play3Over";
        public const string Loading     = "Loading";
        public const string Rank        = "Rank";
        public const string Setting     = "Setting";
        public const string Idle        = "Idle";
    }

   
    /// <summary>
    /// Game tag.游戏中物体的标记;
    /// </summary>
    public class GameTag
    {
        public const string PlayerTag = "Player";
        public const string MonsterTag = "Monster";
        public const string DestroyTag = "Destroy";
        public const string ParticleTag = "Particle";
        public const string TerrainTag = "Terrain";
        public const string HeroTag = "Hero";
        public const string NpcTag = "Npc";
        public const string ChatEditBox = "ChatEditBox1";
        public const string TreasureTag = "TreasureTag";
    }

    /// <summary>
    /// 层名的定义，可以通过层名直接获取层;
    /// </summary>
    public class SceneLayerMask
    {
        //默认;
        public const string Default = "Default";
        //透明;
        public const string TransparentFx = "TransparentFx";
        //用于忽略Raycast射线碰撞检测 Terry
        public const string IgnoreRaycast = "IgnoreRaycast";
        //水;
        public const string Water = "Water";
        //物体透明;
        //public const string ModelTranspant = "ModelTranspant";
        //UI
        public const string UI = "UI";
        //地图;
        public const string Map = "Environment";
        //角色层 包括NPC和Monster;
        public const string Actor = "Object";
        //目标层 用于特殊射击功能
        public const string Target = "Target";
        /// <summary>
        /// 掉落物品;
        /// </summary>
        public const string Drop = "Drop";
    }



    public class ModelType
    {
        public const string Fireman1 = "Fireman1";
        public const string Fireman2 = "Fireman2";
        public const string Firewoman1 = "Firewoman1";
        public const string FireMonster1 = "FireMonster1";
        public const string BFireMonster1 = "BFireMonster1";
        public const string Boss = "Boss";
        public const string SmokeMonster1 = "SmokeMonster1";
        public const string BabyCitizen = "BabyCitizen";
        public const string GirlCitizen = "GirlCitizen";
        public const string BoyCitizen = "BoyCitizen";
        public const string Dog1 = "Dog1";
        public const string Cat1 = "Cat1";
        public const string CloudFireCar = "CloudFireCar";
        public const string Bird = "Bird";
		public const string Balloon_Scene1 = "Balloon_Scene1";
		public const string Balloon_Scene2 = "Balloon_Scene2";
		public const string Balloon_Scene3 = "Balloon_Scene3";
    }

    public class EffectType
    {
        public const string HuoYan_Cat = "HuoYan_Cat";
        public const string YanWu_Dog_1 = "YanWu_Dog_1";
		public const string ZiRan_01 = "ZiRan_01";
		public const string ZiRan_02 = "ZiRan_02";
		public const string ZiRan_03 = "ZiRan_03";
        public const string Hit_SmokePuff = "Hit_SmokePuff";
		public const string ZiRan_04 = "ZiRan_04";
		public const string BaoZha_01 = "BaoZha_01";
		public const string HuoTuan = "HuoTuan";
		public const string YanTuan = "YanTuan";
		public const string YanLei_01 = "YanLei_01";
		public const string YanLei_02 = "YanLei_02";
		public const string HuoYan_Boss_Head = "HuoYan_Boss_Head";
		public const string HuoYan_Boss_Left = "HuoYan_Boss_Left";
		public const string HuoYan_Boss_Right = "HuoYan_Boss_Right";
		public const string HuoQiu_Boss = "HuoQiu_Boss";
		public const string ZhuaJi_Boss = "ZhuaJi_Boss";
		public const string ZhaoHuan_Boss = "ZhaoHuan_Boss";
		public const string HuoZhen_Boss = "HuoZhen_Boss";
		public const string AOE = "AOE";
    }

    /// <summary>
    /// 属性类型
    /// </summary>
    public enum AttrType
    {
        UnKnown = 0,
        ATTR_HP = 1,
        ATTR_HP_PERCENT = 2,
        ATTR_ATTACK_SPEED = 3,
        ATTR_ATTACK_COOL  = 4,
        ATTR_POSITION = 5,
    }
    

    /// <summary>
    /// 条件比较类型
    /// </summary>
    public enum ConditionType
    {
        CONDITION_UNKNOWN = 0,
        CONDITION_EQUALS = 1,
        CONDITION_NOTEQUAL = 2,
        CONDITION_LESS = 3,
        CONDITION_GREATER = 4,
    }
}


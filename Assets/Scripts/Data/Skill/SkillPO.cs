/**
*    Copyright (c) 2015 Need co.,Ltd
*    All rights reserved

*    文件名称:    SkillPO.cs
*    创建标识:    
*    简    介:    技能ID（=技能等级+技能系列*100）
*/
using System;
using System.Collections.Generic; 
using System.Text;
using LitJson; 
namespace Need.Mx
{

    public partial class SkillPO 
    {
        protected int m_Id;
        protected int m_Index;
        protected string m_SkillName;
        protected string m_SkillDescription;
        protected int m_SkillType;
        protected int m_TriggerTime;
        protected float[] m_SkillTrigger;
        protected int m_SkillProbability;
        protected int m_CanTriggerBuff;
        protected int[] m_MethodList;
        protected int m_CoolTick;
        protected int m_CoolGroupID;
        protected int m_CoolGroupTick;
        protected int m_ActionTick;
        protected string[] m_UseShake;
        protected int m_ImpactBomb;
        protected int m_IsHarm;
        protected int m_FixedDamage;
        protected int m_PercentDamage;
        protected int m_IsSummon;
        protected string[] m_SummonMonsterList;
        protected string m_Animation;
        protected string[] m_Effect;
        protected string[] m_TriggerSound;
        protected string[] m_BulletTimeSound;
        protected string[] m_BulletTime;
        protected int m_DamageLifeTime;

        public SkillPO(JsonData jsonNode)
        {
            m_Id = (int)jsonNode["Id"];
            m_Index = (int)jsonNode["Index"];
            m_SkillName = jsonNode["SkillName"].ToString() == "NULL" ? "" : jsonNode["SkillName"].ToString();
            m_SkillDescription = jsonNode["SkillDescription"].ToString() == "NULL" ? "" : jsonNode["SkillDescription"].ToString();
            m_SkillType = (int)jsonNode["SkillType"];
            m_TriggerTime = (int)jsonNode["TriggerTime"];
            {
                JsonData array = jsonNode["SkillTrigger"];
                m_SkillTrigger = new float[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_SkillTrigger[index] = (float)(double)array[index];
                }
            }
            m_SkillProbability = (int)jsonNode["SkillProbability"];
            m_CanTriggerBuff = (int)jsonNode["CanTriggerBuff"];
            {
                JsonData array = jsonNode["MethodList"];
                m_MethodList = new int[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_MethodList[index] = (int)array[index];
                }
            }
            m_CoolTick = (int)jsonNode["CoolTick"];
            m_CoolGroupID = (int)jsonNode["CoolGroupID"];
            m_CoolGroupTick = (int)jsonNode["CoolGroupTick"];
            m_ActionTick = (int)jsonNode["ActionTick"];
            {
                JsonData array = jsonNode["UseShake"];
                m_UseShake = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_UseShake[index] = array[index].ToString();
                }
            }
            m_ImpactBomb = (int)jsonNode["ImpactBomb"];
            m_IsHarm = (int)jsonNode["IsHarm"];
            m_FixedDamage = (int)jsonNode["FixedDamage"];
            m_PercentDamage = (int)jsonNode["PercentDamage"];
            m_IsSummon = (int)jsonNode["IsSummon"];
            {
                JsonData array = jsonNode["SummonMonsterList"];
                m_SummonMonsterList = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_SummonMonsterList[index] = array[index].ToString();
                }
            }
            m_Animation = jsonNode["Animation"].ToString() == "NULL" ? "" : jsonNode["Animation"].ToString();
            {
                JsonData array = jsonNode["Effect"];
                m_Effect = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_Effect[index] = array[index].ToString();
                }
            }
            {
                JsonData array = jsonNode["TriggerSound"];
                m_TriggerSound = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_TriggerSound[index] = array[index].ToString();
                }
            }
            {
                JsonData array = jsonNode["BulletTimeSound"];
                m_BulletTimeSound = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_BulletTimeSound[index] = array[index].ToString();
                }
            }
            {
                JsonData array = jsonNode["BulletTime"];
                m_BulletTime = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_BulletTime[index] = array[index].ToString();
                }
            }
            m_DamageLifeTime = (int)jsonNode["DamageLifeTime"];
        }

        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        public int Index
        {
            get
            {
                return m_Index;
            }
        }

        public string SkillName
        {
            get
            {
                return m_SkillName;
            }
        }

        public string SkillDescription
        {
            get
            {
                return m_SkillDescription;
            }
        }

        public int SkillType
        {
            get
            {
                return m_SkillType;
            }
        }

        public int TriggerTime
        {
            get
            {
                return m_TriggerTime;
            }
        }

        public float[] SkillTrigger
        {
            get
            {
                return m_SkillTrigger;
            }
        }

        public int SkillProbability
        {
            get
            {
                return m_SkillProbability;
            }
        }

        public int CanTriggerBuff
        {
            get
            {
                return m_CanTriggerBuff;
            }
        }

        public int[] MethodList
        {
            get
            {
                return m_MethodList;
            }
        }

        public int CoolTick
        {
            get
            {
                return m_CoolTick;
            }
        }

        public int CoolGroupID
        {
            get
            {
                return m_CoolGroupID;
            }
        }

        public int CoolGroupTick
        {
            get
            {
                return m_CoolGroupTick;
            }
        }

        public int ActionTick
        {
            get
            {
                return m_ActionTick;
            }
        }

        public string[] UseShake
        {
            get
            {
                return m_UseShake;
            }
        }

        public int ImpactBomb
        {
            get
            {
                return m_ImpactBomb;
            }
        }

        public int IsHarm
        {
            get
            {
                return m_IsHarm;
            }
        }

        public int FixedDamage
        {
            get
            {
                return m_FixedDamage;
            }
        }

        public int PercentDamage
        {
            get
            {
                return m_PercentDamage;
            }
        }

        public int IsSummon
        {
            get
            {
                return m_IsSummon;
            }
        }

        public string[] SummonMonsterList
        {
            get
            {
                return m_SummonMonsterList;
            }
        }

        public string Animation
        {
            get
            {
                return m_Animation;
            }
        }

        public string[] Effect
        {
            get
            {
                return m_Effect;
            }
        }

        public string[] TriggerSound
        {
            get
            {
                return m_TriggerSound;
            }
        }

        public string[] BulletTimeSound
        {
            get
            {
                return m_BulletTimeSound;
            }
        }

        public string[] BulletTime
        {
            get
            {
                return m_BulletTime;
            }
        }

        public int DamageLifeTime
        {
            get
            {
                return m_DamageLifeTime;
            }
        }

    }


}


/**
*    Copyright (c) 2015 Need co.,Ltd
*    All rights reserved

*    文件名称:    MonsterPO.cs
*    创建标识:    
*    简    介:    怪物ID（=怪物编号+场景ID*1000+怪物类型*10000）插入怪物时请根据格式在每一个场景ID的最后添加怪物
*/
using System;
using System.Collections.Generic; 
using System.Text;
using LitJson; 
namespace Need.Mx
{

    public partial class MonsterPO 
    {
        protected int m_Id;
        protected int m_Index;
        protected string m_ShapeName;
        protected string m_Desc;
        protected int m_MonsterType;
        protected int m_FriendType;
        protected string m_SceneName;
        protected int m_SceneId;
        protected int m_Hp;
        protected int m_Speed;
        protected int m_AttackSpeed;
        protected int m_AttackCool;
        protected int m_DisplayBlood;
        protected int m_StandAction;
        protected int m_RandomMove;
        protected int m_MoveCool;
        protected string m_MoveArea;
        protected float[] m_MoveDistance;
        protected int m_MoveTargetType;
        protected float[] m_FixPoint;
        protected int m_DisappearTime;
        protected int m_CorpseRemainTime;
        protected int m_MonsterValue;
        protected int m_DisplayBar;
        protected int m_SaveDegree;
        protected int m_MoveType;
        protected int m_FireCarId;
        protected int m_AttackMode;
        protected int m_SkillID1;
        protected int m_SkillID2;
        protected int m_SkillID3;
        protected float[] m_HitColor;
        protected string[] m_HitEffect;
        protected string[] m_Effect;
        protected string[] m_DieEffect;
        protected string[] m_BirthSound;
        protected string[] m_DieSound;

        public MonsterPO(JsonData jsonNode)
        {
            m_Id = (int)jsonNode["Id"];
            m_Index = (int)jsonNode["Index"];
            m_ShapeName = jsonNode["ShapeName"].ToString() == "NULL" ? "" : jsonNode["ShapeName"].ToString();
            m_Desc = jsonNode["Desc"].ToString() == "NULL" ? "" : jsonNode["Desc"].ToString();
            m_MonsterType = (int)jsonNode["MonsterType"];
            m_FriendType = (int)jsonNode["FriendType"];
            m_SceneName = jsonNode["SceneName"].ToString() == "NULL" ? "" : jsonNode["SceneName"].ToString();
            m_SceneId = (int)jsonNode["SceneId"];
            m_Hp = (int)jsonNode["Hp"];
            m_Speed = (int)jsonNode["Speed"];
            m_AttackSpeed = (int)jsonNode["AttackSpeed"];
            m_AttackCool = (int)jsonNode["AttackCool"];
            m_DisplayBlood = (int)jsonNode["DisplayBlood"];
            m_StandAction = (int)jsonNode["StandAction"];
            m_RandomMove = (int)jsonNode["RandomMove"];
            m_MoveCool = (int)jsonNode["MoveCool"];
            m_MoveArea = jsonNode["MoveArea"].ToString() == "NULL" ? "" : jsonNode["MoveArea"].ToString();
            {
                JsonData array = jsonNode["MoveDistance"];
                m_MoveDistance = new float[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_MoveDistance[index] = (float)(double)array[index];
                }
            }
            m_MoveTargetType = (int)jsonNode["MoveTargetType"];
            {
                JsonData array = jsonNode["FixPoint"];
                m_FixPoint = new float[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_FixPoint[index] = (float)(double)array[index];
                }
            }
            m_DisappearTime = (int)jsonNode["DisappearTime"];
            m_CorpseRemainTime = (int)jsonNode["CorpseRemainTime"];
            m_MonsterValue = (int)jsonNode["MonsterValue"];
            m_DisplayBar = (int)jsonNode["DisplayBar"];
            m_SaveDegree = (int)jsonNode["SaveDegree"];
            m_MoveType = (int)jsonNode["MoveType"];
            m_FireCarId = (int)jsonNode["FireCarId"];
            m_AttackMode = (int)jsonNode["AttackMode"];
            m_SkillID1 = (int)jsonNode["SkillID1"];
            m_SkillID2 = (int)jsonNode["SkillID2"];
            m_SkillID3 = (int)jsonNode["SkillID3"];
            {
                JsonData array = jsonNode["HitColor"];
                m_HitColor = new float[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_HitColor[index] = (float)(double)array[index];
                }
            }
            {
                JsonData array = jsonNode["HitEffect"];
                m_HitEffect = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_HitEffect[index] = array[index].ToString();
                }
            }
            {
                JsonData array = jsonNode["Effect"];
                m_Effect = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_Effect[index] = array[index].ToString();
                }
            }
            {
                JsonData array = jsonNode["DieEffect"];
                m_DieEffect = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_DieEffect[index] = array[index].ToString();
                }
            }
            {
                JsonData array = jsonNode["BirthSound"];
                m_BirthSound = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_BirthSound[index] = array[index].ToString();
                }
            }
            {
                JsonData array = jsonNode["DieSound"];
                m_DieSound = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_DieSound[index] = array[index].ToString();
                }
            }
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

        public string ShapeName
        {
            get
            {
                return m_ShapeName;
            }
        }

        public string Desc
        {
            get
            {
                return m_Desc;
            }
        }

        public int MonsterType
        {
            get
            {
                return m_MonsterType;
            }
        }

        public int FriendType
        {
            get
            {
                return m_FriendType;
            }
        }

        public string SceneName
        {
            get
            {
                return m_SceneName;
            }
        }

        public int SceneId
        {
            get
            {
                return m_SceneId;
            }
        }

        public int Hp
        {
            get
            {
                return m_Hp;
            }
        }

        public int Speed
        {
            get
            {
                return m_Speed;
            }
        }

        public int AttackSpeed
        {
            get
            {
                return m_AttackSpeed;
            }
        }

        public int AttackCool
        {
            get
            {
                return m_AttackCool;
            }
        }

        public int DisplayBlood
        {
            get
            {
                return m_DisplayBlood;
            }
        }

        public int StandAction
        {
            get
            {
                return m_StandAction;
            }
        }

        public int RandomMove
        {
            get
            {
                return m_RandomMove;
            }
        }

        public int MoveCool
        {
            get
            {
                return m_MoveCool;
            }
        }

        public string MoveArea
        {
            get
            {
                return m_MoveArea;
            }
        }

        public float[] MoveDistance
        {
            get
            {
                return m_MoveDistance;
            }
        }

        public int MoveTargetType
        {
            get
            {
                return m_MoveTargetType;
            }
        }

        public float[] FixPoint
        {
            get
            {
                return m_FixPoint;
            }
        }

        public int DisappearTime
        {
            get
            {
                return m_DisappearTime;
            }
        }

        public int CorpseRemainTime
        {
            get
            {
                return m_CorpseRemainTime;
            }
        }

        public int MonsterValue
        {
            get
            {
                return m_MonsterValue;
            }
        }

        public int DisplayBar
        {
            get
            {
                return m_DisplayBar;
            }
        }

        public int SaveDegree
        {
            get
            {
                return m_SaveDegree;
            }
        }

        public int MoveType
        {
            get
            {
                return m_MoveType;
            }
        }

        public int FireCarId
        {
            get
            {
                return m_FireCarId;
            }
        }

        public int AttackMode
        {
            get
            {
                return m_AttackMode;
            }
        }

        public int SkillID1
        {
            get
            {
                return m_SkillID1;
            }
        }

        public int SkillID2
        {
            get
            {
                return m_SkillID2;
            }
        }

        public int SkillID3
        {
            get
            {
                return m_SkillID3;
            }
        }

        public float[] HitColor
        {
            get
            {
                return m_HitColor;
            }
        }

        public string[] HitEffect
        {
            get
            {
                return m_HitEffect;
            }
        }

        public string[] Effect
        {
            get
            {
                return m_Effect;
            }
        }

        public string[] DieEffect
        {
            get
            {
                return m_DieEffect;
            }
        }

        public string[] BirthSound
        {
            get
            {
                return m_BirthSound;
            }
        }

        public string[] DieSound
        {
            get
            {
                return m_DieSound;
            }
        }

    }


}


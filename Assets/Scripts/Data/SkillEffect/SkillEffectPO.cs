/**
*    Copyright (c) 2015 Need co.,Ltd
*    All rights reserved

*    文件名称:    SkillEffectPO.cs
*    创建标识:    
*    简    介:    效果ID（=效果等级+效果系列*100）
*/
using System;
using System.Collections.Generic; 
using System.Text;
using LitJson; 
namespace Need.Mx
{

    public partial class SkillEffectPO 
    {
        protected int m_Id;
        protected int m_Index;
        protected string m_SkillName;
        protected string m_SkillDescription;
        protected int m_BuffProbability;
        protected int m_BuffUsefulType;
        protected int m_DurationTick;
        protected int m_IntervalTick;
        protected int[] m_AffectList;
        protected string m_EffectShape;
        protected string m_PlayerDiscoloration;

        public SkillEffectPO(JsonData jsonNode)
        {
            m_Id = (int)jsonNode["Id"];
            m_Index = (int)jsonNode["Index"];
            m_SkillName = jsonNode["SkillName"].ToString() == "NULL" ? "" : jsonNode["SkillName"].ToString();
            m_SkillDescription = jsonNode["SkillDescription"].ToString() == "NULL" ? "" : jsonNode["SkillDescription"].ToString();
            m_BuffProbability = (int)jsonNode["BuffProbability"];
            m_BuffUsefulType = (int)jsonNode["BuffUsefulType"];
            m_DurationTick = (int)jsonNode["DurationTick"];
            m_IntervalTick = (int)jsonNode["IntervalTick"];
            {
                JsonData array = jsonNode["AffectList"];
                m_AffectList = new int[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_AffectList[index] = (int)array[index];
                }
            }
            m_EffectShape = jsonNode["EffectShape"].ToString() == "NULL" ? "" : jsonNode["EffectShape"].ToString();
            m_PlayerDiscoloration = jsonNode["PlayerDiscoloration"].ToString() == "NULL" ? "" : jsonNode["PlayerDiscoloration"].ToString();
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

        public int BuffProbability
        {
            get
            {
                return m_BuffProbability;
            }
        }

        public int BuffUsefulType
        {
            get
            {
                return m_BuffUsefulType;
            }
        }

        public int DurationTick
        {
            get
            {
                return m_DurationTick;
            }
        }

        public int IntervalTick
        {
            get
            {
                return m_IntervalTick;
            }
        }

        public int[] AffectList
        {
            get
            {
                return m_AffectList;
            }
        }

        public string EffectShape
        {
            get
            {
                return m_EffectShape;
            }
        }

        public string PlayerDiscoloration
        {
            get
            {
                return m_PlayerDiscoloration;
            }
        }

    }


}


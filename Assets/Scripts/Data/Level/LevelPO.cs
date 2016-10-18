/**
*    Copyright (c) 2015 Need co.,Ltd
*    All rights reserved

*    文件名称:    LevelPO.cs
*    创建标识:    
*    简    介:    完成条件id（场景id*10000+难度）
*/
using System;
using System.Collections.Generic; 
using System.Text;
using LitJson; 
namespace Need.Mx
{

    public partial class LevelPO 
    {
        protected int m_Id;
        protected int m_SceneID;
        protected int m_Level;
        protected string m_SceneName;
        protected int m_IsShow;
        protected int m_SceneTime;
        protected string[] m_MustKillMonster;
        protected string[] m_MustKillID;
        protected int m_WaterTime;
        protected int m_ShakeTimes;
        protected int m_AOEScores;
        protected int[] m_CheckPointScores;

        public LevelPO(JsonData jsonNode)
        {
            m_Id = (int)jsonNode["Id"];
            m_SceneID = (int)jsonNode["SceneID"];
            m_Level = (int)jsonNode["Level"];
            m_SceneName = jsonNode["SceneName"].ToString() == "NULL" ? "" : jsonNode["SceneName"].ToString();
            m_IsShow = (int)jsonNode["IsShow"];
            m_SceneTime = (int)jsonNode["SceneTime"];
            {
                JsonData array = jsonNode["MustKillMonster"];
                m_MustKillMonster = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_MustKillMonster[index] = array[index].ToString();
                }
            }
            {
                JsonData array = jsonNode["MustKillID"];
                m_MustKillID = new string[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_MustKillID[index] = array[index].ToString();
                }
            }
            m_WaterTime = (int)jsonNode["WaterTime"];
            m_ShakeTimes = (int)jsonNode["ShakeTimes"];
            m_AOEScores = (int)jsonNode["AOEScores"];
            {
                JsonData array = jsonNode["CheckPointScores"];
                m_CheckPointScores = new int[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_CheckPointScores[index] = (int)array[index];
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

        public int SceneID
        {
            get
            {
                return m_SceneID;
            }
        }

        public int Level
        {
            get
            {
                return m_Level;
            }
        }

        public string SceneName
        {
            get
            {
                return m_SceneName;
            }
        }

        public int IsShow
        {
            get
            {
                return m_IsShow;
            }
        }

        public int SceneTime
        {
            get
            {
                return m_SceneTime;
            }
        }

        public string[] MustKillMonster
        {
            get
            {
                return m_MustKillMonster;
            }
        }

        public string[] MustKillID
        {
            get
            {
                return m_MustKillID;
            }
        }

        public int WaterTime
        {
            get
            {
                return m_WaterTime;
            }
        }

        public int ShakeTimes
        {
            get
            {
                return m_ShakeTimes;
            }
        }

        public int AOEScores
        {
            get
            {
                return m_AOEScores;
            }
        }

        public int[] CheckPointScores
        {
            get
            {
                return m_CheckPointScores;
            }
        }

    }


}


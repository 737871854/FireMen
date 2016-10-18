/**
*    Copyright (c) 2015 Need co.,Ltd
*    All rights reserved

*    文件名称:    MonsterRefreshPO.cs
*    创建标识:    
*    简    介:    怪物刷出脚本ID*10000000+难度*10000+序号
*/
using System;
using System.Collections.Generic; 
using System.Text;
using LitJson; 
namespace Need.Mx
{

    public partial class MonsterRefreshPO 
    {
        protected int m_Id;
        protected int m_SceneId;
        protected int m_Index;
        protected int m_AppeareTime;
        protected int m_Level;
        protected int m_MonsterId;
        protected int m_MonsterNumber;
        protected int m_SeparateTime;
        protected float[] m_AppearePoint;
        protected string m_AppeareArea;
        protected string m_WindowName;
        protected string m_RefreshDesc;
        protected string m_MosterDesc;
        protected int m_CheckPoint;

        public MonsterRefreshPO(JsonData jsonNode)
        {
            m_Id = (int)jsonNode["Id"];
            m_SceneId = (int)jsonNode["SceneId"];
            m_Index = (int)jsonNode["Index"];
            m_AppeareTime = (int)jsonNode["AppeareTime"];
            m_Level = (int)jsonNode["Level"];
            m_MonsterId = (int)jsonNode["MonsterId"];
            m_MonsterNumber = (int)jsonNode["MonsterNumber"];
            m_SeparateTime = (int)jsonNode["SeparateTime"];
            {
                JsonData array = jsonNode["AppearePoint"];
                m_AppearePoint = new float[array.Count];
                for (int index = 0; index < array.Count; index++)
                {
                    m_AppearePoint[index] = (float)(double)array[index];
                }
            }
            m_AppeareArea = jsonNode["AppeareArea"].ToString() == "NULL" ? "" : jsonNode["AppeareArea"].ToString();
            m_WindowName = jsonNode["WindowName"].ToString() == "NULL" ? "" : jsonNode["WindowName"].ToString();
            m_RefreshDesc = jsonNode["RefreshDesc"].ToString() == "NULL" ? "" : jsonNode["RefreshDesc"].ToString();
            m_MosterDesc = jsonNode["MosterDesc"].ToString() == "NULL" ? "" : jsonNode["MosterDesc"].ToString();
            m_CheckPoint = (int)jsonNode["CheckPoint"];
        }

        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        public int SceneId
        {
            get
            {
                return m_SceneId;
            }
        }

        public int Index
        {
            get
            {
                return m_Index;
            }
        }

        public int AppeareTime
        {
            get
            {
                return m_AppeareTime;
            }
        }

        public int Level
        {
            get
            {
                return m_Level;
            }
        }

        public int MonsterId
        {
            get
            {
                return m_MonsterId;
            }
        }

        public int MonsterNumber
        {
            get
            {
                return m_MonsterNumber;
            }
        }

        public int SeparateTime
        {
            get
            {
                return m_SeparateTime;
            }
        }

        public float[] AppearePoint
        {
            get
            {
                return m_AppearePoint;
            }
        }

        public string AppeareArea
        {
            get
            {
                return m_AppeareArea;
            }
        }

        public string WindowName
        {
            get
            {
                return m_WindowName;
            }
        }

        public string RefreshDesc
        {
            get
            {
                return m_RefreshDesc;
            }
        }

        public string MosterDesc
        {
            get
            {
                return m_MosterDesc;
            }
        }

        public int CheckPoint
        {
            get
            {
                return m_CheckPoint;
            }
        }

    }


}


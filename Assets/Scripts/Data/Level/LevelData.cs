/**
* 	Copyright (c) 2015 Need co.,Ltd
*	All rights reserved

*    文件名称:    LevelData.cs
*    创建标志:    
*    简    介:    完成条件id（场景id*10000+难度）
*/
using System;
using System.Collections.Generic; 
using LitJson; 
namespace Need.Mx
{

    public partial class LevelData 
    {
        protected static LevelData instance;
        protected Dictionary<int,LevelPO> m_dictionary;

        public static LevelData Instance
        {
            get{
                if(instance == null)
                {
                    instance = new LevelData();
                }
                return instance;
            }
        }

        protected LevelData()
        {
            m_dictionary = new Dictionary<int,LevelPO>();
        }

        public LevelPO GetLevelPO(int key)
        {
            if(m_dictionary.ContainsKey(key) == false)
            {
                return null;
            }
            return m_dictionary[key];
        }

        static public void LoadHandler(LoadedData data)
        {
            JsonData jsonData = JsonMapper.ToObject(data.Value.ToString());
            if (!jsonData.IsArray)
            {
                return;
            }
            for (int index = 0; index < jsonData.Count; index++)
            {
                JsonData element = jsonData[index];
                LevelPO po = new LevelPO(element);
                LevelData.Instance.m_dictionary.Add(po.Id, po);
            }
        }
    }

}


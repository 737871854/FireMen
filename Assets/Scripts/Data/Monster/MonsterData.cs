/**
* 	Copyright (c) 2015 Need co.,Ltd
*	All rights reserved

*    文件名称:    MonsterData.cs
*    创建标志:    
*    简    介:    怪物ID（=怪物编号+场景ID*1000+怪物类型*10000）插入怪物时请根据格式在每一个场景ID的最后添加怪物
*/
using System;
using System.Collections.Generic; 
using LitJson; 
namespace Need.Mx
{

    public partial class MonsterData 
    {
        protected static MonsterData instance;
        protected Dictionary<int,MonsterPO> m_dictionary;

        public static MonsterData Instance
        {
            get{
                if(instance == null)
                {
                    instance = new MonsterData();
                }
                return instance;
            }
        }

        protected MonsterData()
        {
            m_dictionary = new Dictionary<int,MonsterPO>();
        }

        public MonsterPO GetMonsterPO(int key)
        {
            if(m_dictionary.ContainsKey(key) == false)
            {
                return null;
            }
            return m_dictionary[key];
        }

        static public void LoadHandler(LoadedData data)
        {
            MonsterData.Instance.m_dictionary.Clear();
            JsonData jsonData = JsonMapper.ToObject(data.Value.ToString());
            if (!jsonData.IsArray)
            {
                return;
            }
            for (int index = 0; index < jsonData.Count; index++)
            {
                JsonData element = jsonData[index];
                MonsterPO po = new MonsterPO(element);
                MonsterData.Instance.m_dictionary.Add(po.Id, po);
            }
        }
    }

}


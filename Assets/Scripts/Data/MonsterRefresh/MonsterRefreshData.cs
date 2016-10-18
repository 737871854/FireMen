/**
* 	Copyright (c) 2015 Need co.,Ltd
*	All rights reserved

*    文件名称:    MonsterRefreshData.cs
*    创建标志:    
*    简    介:    怪物刷出脚本ID*10000000+难度*10000+序号
*/
using System;
using System.Collections.Generic; 
using LitJson; 
namespace Need.Mx
{

    public partial class MonsterRefreshData 
    {
        protected static MonsterRefreshData instance;
        protected Dictionary<int,MonsterRefreshPO> m_dictionary;

        public static MonsterRefreshData Instance
        {
            get{
                if(instance == null)
                {
                    instance = new MonsterRefreshData();
                }
                return instance;
            }
        }

        protected MonsterRefreshData()
        {
            m_dictionary = new Dictionary<int,MonsterRefreshPO>();
        }

        public MonsterRefreshPO GetMonsterRefreshPO(int key)
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
                MonsterRefreshPO po = new MonsterRefreshPO(element);
                MonsterRefreshData.Instance.m_dictionary.Add(po.Id, po);
            }
        }
    }

}


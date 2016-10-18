/**
* 	Copyright (c) 2015 Need co.,Ltd
*	All rights reserved

*    文件名称:    SkillData.cs
*    创建标志:    
*    简    介:    技能ID（=技能等级+技能系列*100）
*/
using System;
using System.Collections.Generic; 
using LitJson; 
namespace Need.Mx
{

    public partial class SkillData 
    {
        protected static SkillData instance;
        protected Dictionary<int,SkillPO> m_dictionary;

        public static SkillData Instance
        {
            get{
                if(instance == null)
                {
                    instance = new SkillData();
                }
                return instance;
            }
        }

        protected SkillData()
        {
            m_dictionary = new Dictionary<int,SkillPO>();
        }

        public SkillPO GetSkillPO(int key)
        {
            if(m_dictionary.ContainsKey(key) == false)
            {
                return null;
            }
            return m_dictionary[key];
        }

        static public void LoadHandler(LoadedData data)
        {
            SkillData.Instance.m_dictionary.Clear();
            JsonData jsonData = JsonMapper.ToObject(data.Value.ToString());
            if (!jsonData.IsArray)
            {
                return;
            }
            for (int index = 0; index < jsonData.Count; index++)
            {
                JsonData element = jsonData[index];
                SkillPO po = new SkillPO(element);
                SkillData.Instance.m_dictionary.Add(po.Id, po);
            }
        }
    }

}


/**
* 	Copyright (c) 2015 Need co.,Ltd
*	All rights reserved

*    文件名称:    SkillEffectData.cs
*    创建标志:    
*    简    介:    效果ID（=效果等级+效果系列*100）
*/
using System;
using System.Collections.Generic; 
using LitJson; 
namespace Need.Mx
{

    public partial class SkillEffectData 
    {
        protected static SkillEffectData instance;
        protected Dictionary<int,SkillEffectPO> m_dictionary;

        public static SkillEffectData Instance
        {
            get{
                if(instance == null)
                {
                    instance = new SkillEffectData();
                }
                return instance;
            }
        }

        protected SkillEffectData()
        {
            m_dictionary = new Dictionary<int,SkillEffectPO>();
        }

        public SkillEffectPO GetSkillEffectPO(int key)
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
                SkillEffectPO po = new SkillEffectPO(element);
                SkillEffectData.Instance.m_dictionary.Add(po.Id, po);
            }
        }
    }

}


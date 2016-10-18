using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public class BufferComponent : MonoBehaviour {


    public class BufferInfo
    {
        public SkillEffectPO dataPO;
        public float duration;
        public float interval; 
    }

    protected Monster self;
    protected List<BufferInfo> bufferList;		//拥有的效果; 

	// Use this for initialization
	public void Init ()
    {
        self = GetComponent<Monster>();
        bufferList = new List<BufferInfo>();
	}
	
	// Update is called once per frame
	public void UpdateBuffer ()
    {
        if (bufferList.Count == 0)
        {
            return;
        }

	    for(int index = 0;index < bufferList.Count; ++index)
        {
            // 特殊处理(懒惰处理)
            // 如果遇到一个特效需要删除
            // 删除特效
            // 则退出循环
            if (OnBuffer(bufferList[index]) == false)
            {
                bufferList.RemoveAt(index);
                break;
            }
        }
	}

    public void AddBuffer(int effectId)
    {
        SkillEffectPO dataPO = SkillEffectData.Instance.GetSkillEffectPO(effectId);
        if (dataPO == null)
        {
            return;
        }

        // 判断触发几率
        int seed = Random.Range(1, 10000);
        if (seed > dataPO.BuffProbability)
        {
            return;
        }

        // 添加
        BufferInfo info = new BufferInfo();
        info.dataPO  = dataPO;
        info.duration = (float)dataPO.DurationTick / 1000.0f;
        if (dataPO.IntervalTick > 0)
            info.interval = (float)dataPO.IntervalTick / 1000.0f;
        else
            info.interval = 1.0f;
        bufferList.Add(info);

    }

    bool OnBuffer(BufferInfo buffer)
    {
        // 判断时间
        if (buffer.dataPO.DurationTick > 0 && buffer.duration <= 0)
        {
            return false;
        }
        buffer.duration -= Time.deltaTime;
        // 判断触发次数
        // 如果是间隔触发，则计算
        // 否则只触发一次
        // 根据配置表中IntervalTick是否大于0来决定
        if (buffer.dataPO.IntervalTick > 0)
        {
            if (buffer.interval > 0)
            {
                buffer.interval -= Time.deltaTime;
                return true;
            }
            else
            {
                buffer.interval = (float)buffer.dataPO.IntervalTick / 1000.0f;
            }
        }
        
        if (buffer.dataPO.IntervalTick == 0)
        {
            if (buffer.interval <= 0)
            {
                return false;
            }
            else
            {
                buffer.interval -= 1.0f;
            }
        }

        OnBufferAttr(buffer);
        OnBufferBody(buffer);
        return true;
    }

    void OnBufferAttr(BufferInfo buffer)
    {
        if (buffer.dataPO.AffectList.Length == 0)
        {
            return;
        }

        if (buffer.dataPO.AffectList.Length % 2 == 0)
        {
            for (int index = 0, count = 0; index < buffer.dataPO.AffectList.Length; ++count)
            {
                int attrType = buffer.dataPO.AffectList[index++];
                int attrValue= buffer.dataPO.AffectList[index++];
                self.OnAttrChange((AttrType)attrType, attrValue);
            }
        }
    }

    void OnBufferBody(BufferInfo buffer)
    {

    }
}

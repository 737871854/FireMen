using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public class SkillComponent : MonoBehaviour {


    public class SkillInfo
    {
        public SkillPO dataPO;
        public int usedCount;
        public float coolDown;
    }

    public class DelayBuffer
    {
        public float time;
        public int skillId;

        public DelayBuffer(float value, int id)
        {
            time = value;
            skillId = id;
        }
    }

    protected Monster self;
    protected Dictionary<int, SkillInfo> skillDic;		//拥有的技能; 
    protected List<DelayBuffer> delayBufferList;        //延迟触发

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name='list'>
    /// 列表.
    /// </param>
    public void Init(object obj)
    {
        self = GetComponent<Monster>();
        delayBufferList = new List<DelayBuffer>();
        skillDic = new Dictionary<int, SkillInfo>();
        List<int> skillList = (List<int>)obj;
        for (int index = 0; index < skillList.Count; ++index)
        {
            SkillPO dataPO = SkillData.Instance.GetSkillPO(skillList[index]);
            if (dataPO == null)
            {
                continue;
            }
            SkillInfo skill = new SkillInfo();
            skill.dataPO    = dataPO;
            skill.coolDown  = 0.0f;
            skill.usedCount = dataPO.TriggerTime;
            skillDic.Add(skillList[index], skill);
        }
    }

    public void UpdateSkill()
    {
        while(delayBufferList.Count > 0)
        {
            DelayBuffer element = delayBufferList[0];
            element.time -= Time.deltaTime;
            if(element.time <= 0)
            {
                SkillInfo skill = skillDic[element.skillId];
                ActivateBuffer(skill.dataPO);
                delayBufferList.RemoveAt(0);
            }
            else
            {
                delayBufferList[0] = element;
                break;
            }
        }
    }

    public void Interrupt()
    {
        delayBufferList.Clear();
    }
   
    public bool Use(int skillId)
    {
        if (skillDic.ContainsKey(skillId) == false)
        {
            return false;
        }
        SkillInfo skill = skillDic[skillId];
        if (!CanUse(skill))
        {
            return false;
        }

        Deplete(skill);

        // 判断是否有动作
        if (skill.dataPO.Animation.Length > 0)
        {
            self.ChangeAttack(skill.dataPO.Animation);
        }

        // 播放特效
        ActivateEffect(skill.dataPO);

        // 启动子弹时间效果
        ActivateBulletTime(skill.dataPO);

        // 是否需要根据动作播放时间来配合触发
        if (skill.dataPO.ActionTick > 0)
        {
            // 记录施法效果,延迟触发
            delayBufferList.Add(new DelayBuffer((float)skill.dataPO.ActionTick / 1000.0f, skillId));
            return true;
        }

        // 即时触发
        ActivateBuffer(skill.dataPO);
        return true;
    }

    bool CanUse(SkillInfo info)
    {
        // 触发概率
        int seed = Random.Range(1, 10000);
        if (seed > info.dataPO.SkillProbability)
        {
            return false;
        }

        // 使用次数
        if (info.dataPO.TriggerTime > 0 && info.usedCount <= 0)
        {
            return false;
        }

        // 冷却时间
        float curTime = Time.time;
        if (curTime - info.coolDown < (float)info.dataPO.CoolTick / 1000.0f)
        {
            return false;
        }

        // 触发条件
        if (info.dataPO.SkillTrigger.Length > 2)
        {
            int offset = 0;
            bool result = false;
            while (offset < info.dataPO.SkillTrigger.Length)
            {
                AttrType type = (AttrType)info.dataPO.SkillTrigger[offset++];
                switch (type)
                {
                    case AttrType.ATTR_HP_PERCENT:
                        {
                            break;
                        }

                    case AttrType.ATTR_HP:
                        {
                            break;
                        }

                    case AttrType.ATTR_POSITION:
                        {
                            ConditionType opt = (ConditionType)info.dataPO.SkillTrigger[offset++];
                            float x    = info.dataPO.SkillTrigger[offset++];
                            float y    = info.dataPO.SkillTrigger[offset++];
                            float z    = info.dataPO.SkillTrigger[offset++];
                            Vector3 movePosition = new Vector3(x, y, z);
                            if (opt == ConditionType.CONDITION_EQUALS)
                            {
                                if (Vector3.Distance(self.Position, movePosition) < 0.01f)
                                {
                                    result = true;
                                }
                            }

                            break;
                        }
                }
            }

            if (!result)
                return result;
        }


        return true;
    }

    void Deplete(SkillInfo info)
    {
        // 扣掉次数
        if (info.dataPO.TriggerTime > 0)
        {
            --info.usedCount;
        }
        // 进入冷却
        info.coolDown = Time.time;
    }

    void ActivateBuffer(SkillPO dataPO)
    {
        // 固定伤害输出
        if (dataPO.IsHarm == 1)
        {
            if (dataPO.FixedDamage > 0)
            {
                self.OnAttrChange(AttrType.ATTR_HP, -dataPO.FixedDamage);
            }

            if (dataPO.PercentDamage > 0)
            {
                self.OnAttrChange(AttrType.ATTR_HP, (int)(self.HP * ((float)dataPO.PercentDamage / -10000.0f)));
            }
        }

        // Buffer效果
        if (dataPO.CanTriggerBuff > 0)
        {
            for(int index = 0;index < dataPO.MethodList.Length; ++index)
            {
                self.AddBuffer(dataPO.MethodList[index]);
            }
        }

        // 震频效果(目前震频的幅度和速度参数暂时先固定，看需求是否从技能表中配置后读取)
        if (dataPO.UseShake.Length > 0)
        {
            int shakeType = dataPO.UseShake[0].ToInt();
            if (shakeType > 0)
            {
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_PLAY_SHAKE, shakeType);
                if(dataPO.UseShake.Length == 2)
                {
                    string shakeBindName = dataPO.UseShake[1];
                    Transform transformObject = self.FindParentBone(shakeBindName);
                    if (transformObject != null)
                    {
                        EventDispatcher.TriggerEvent(GameEventDef.EVNET_PLAY_CRACK_SCREEN, transformObject.position);
                    }
                }
                else
                {
                    EventDispatcher.TriggerEvent(GameEventDef.EVNET_PLAY_CRACK_SCREEN, self.transform.position);
                }

                Main.SoundController.PlayCrashScreenSound();
            }
        }

        if (dataPO.DamageLifeTime > 0)
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_DAMAGE_LIFE_TIME, dataPO.DamageLifeTime);
        }

        // 是否自爆
        if (dataPO.ImpactBomb == 1)
        {
            self.SetBodyActive(false);
        }

        // 召唤怪物
        if (dataPO.SummonMonsterList.Length % 6 == 0)
        {
            for (int index = 0; index < dataPO.SummonMonsterList.Length; )
            {
                int monsterId   = dataPO.SummonMonsterList[index++].ToInt();
                float posX      = dataPO.SummonMonsterList[index++].ToFloat();
                float posY      = dataPO.SummonMonsterList[index++].ToFloat();
                float posZ      = dataPO.SummonMonsterList[index++].ToFloat();
                string bindName = dataPO.SummonMonsterList[index++];
                float delay     = dataPO.SummonMonsterList[index++].ToFloat() / 1000.0f;
                if (bindName != "None")
                {
                    Transform transformObject = self.FindParentBone(bindName);
                    if (transformObject == null)
                    {
                        // 不合法的绑定点
                        continue;
                    }
                    SceneManager.instance.CreateMonster(monsterId, Vector3.zero, transformObject, delay);
                }
                else
                {
                    SceneManager.instance.CreateMonster(monsterId, new Vector3(posX, posY, posZ), null, delay);
                }
            }
        }

    }

    void ActivateEffect(SkillPO dataPO)
    {
        if (dataPO.Effect.Length % 3 == 0)
        {
            for (int index = 0; index < dataPO.Effect.Length; )
            {
                string effectName = dataPO.Effect[index++];
                string boneName   = dataPO.Effect[index++];
                float lifeTime    = dataPO.Effect[index++].ToFloat();
                self.AddEffect(effectName, boneName, lifeTime);
            }
        }

        if (dataPO.TriggerSound.Length == 2)
        {
            string sound = dataPO.TriggerSound[0];
            int percent  = dataPO.TriggerSound[1].ToInt();
            int seed = Random.Range(1, 10000);
            if (seed > percent)
            {
                return;
            }
            Main.SoundController.FireEvent(sound);
        }
    }

    void ActivateBulletTime(SkillPO dataPO)
    {
        if (dataPO.BulletTime.Length < 6)
        {
            return;
        }

        float timeScale  = dataPO.BulletTime[0].ToFloat();
        float timeDelay  = dataPO.BulletTime[1].ToFloat();
        float bulletTime = dataPO.BulletTime[2].ToFloat();
        int   partCount  = dataPO.BulletTime[3].ToInt();
        int   partHp     = dataPO.BulletTime[4].ToInt();
        List<string> partName = new List<string>();
        for(int index = 0; index < partCount; ++index)
        {
            partName.Add(dataPO.BulletTime[5 + index]);
        }
        self.ChangeHitting(timeScale, timeDelay, bulletTime, partHp, partName);

        if (dataPO.BulletTimeSound.Length == 2)
        {
            string sound = dataPO.BulletTimeSound[0];
            int percent  = dataPO.BulletTimeSound[1].ToInt();
            int seed = Random.Range(1, 10000);
            if (seed > percent)
            {
                return;
            }
            Main.SoundController.FireEvent(sound);
        }
    }

}
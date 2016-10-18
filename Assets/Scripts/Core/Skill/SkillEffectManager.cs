using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public class SkillEffectManager : SingletonBehaviour<SkillEffectManager>
{
    /// <summary>
    /// 外部可配置字段
    /// </summary>
    public GameObject huoYanCatPrefab;
    public GameObject yanWuDog1Prefab;
	public GameObject ziRan01Prefab;
	public GameObject ziRan02Prefab;
	public GameObject ziRan03Prefab;
    public GameObject hitSmokePuffPrefab;
	public GameObject ziRan04Prefab;
	public GameObject baoZha01Prefab;
	public GameObject huoTuanPrefab;
	public GameObject yanTuanPrefab;
	public GameObject yanLei01Prefab;
	public GameObject yanLei02Prefab;
	public GameObject huoYanBossHeadPrefab;
	public GameObject huoYanBossLeftPrefab;
	public GameObject huoYanBossRightPrefab;
	public GameObject huoQiuBossPrefab;
	public GameObject zhuaJiBossPrefab;
	public GameObject zhaoHuanBossPrefab;
	public GameObject huoZhenBossPrefab;
	public GameObject aOEPrefab;



    /// <summary>
    /// 内部属性
    /// </summary>
    [HideInInspector]
    public static float NO_TIME = -1;
    [HideInInspector]
    public string tempEffectBox = "tempEffectBox";
    private Dictionary<string, List<GameObject>> poolDic;
    private Dictionary<string, List<GameObject>> effectInSceneDic;
    private const int LEAST_UPDATE_TIME = 25;
    private const int CLEAR_POOL_TIME   = 60;
    private const int MAX_COUNT_OFONE_EFFECT = 10;
    private float lastUpdateTime;
    private Dictionary<string, float> skillCreateTime;

	// Use this for initialization
	void Start ()
    {
        poolDic = new Dictionary<string, List<GameObject>>();
        effectInSceneDic = new Dictionary<string, List<GameObject>>();
        skillCreateTime = new Dictionary<string, float>();
	}
	
    public GameObject GetPrefab(string type)
    {
        GameObject prefab = null;
        switch (type)
        {
            case EffectType.HuoYan_Cat:
                prefab = huoYanCatPrefab;
                break;
            case EffectType.YanWu_Dog_1:
                prefab = yanWuDog1Prefab;
                break;
		    case EffectType.ZiRan_01:
			    prefab = ziRan01Prefab;
			    break;
		    case EffectType.ZiRan_02:
			    prefab = ziRan02Prefab;
			    break;
		    case EffectType.ZiRan_03:
			    prefab = ziRan03Prefab;
			    break;
            case EffectType.Hit_SmokePuff:
                prefab = hitSmokePuffPrefab;
			    break;
		    case EffectType.ZiRan_04:
			    prefab = ziRan04Prefab;
			    break;
		    case EffectType.BaoZha_01:
			    prefab = baoZha01Prefab;
			    break;
		    case EffectType.HuoTuan:
			    prefab = huoTuanPrefab;
			    break;
		    case EffectType.YanTuan:
		    	prefab = yanTuanPrefab;
		    	break;
		    case EffectType.YanLei_01:
			    prefab = yanLei01Prefab;
			    break;
		    case EffectType.YanLei_02:
			    prefab = yanLei02Prefab;
			    break;
		    case EffectType.HuoYan_Boss_Head:
			    prefab = huoYanBossHeadPrefab;
			    break;
		    case EffectType.HuoYan_Boss_Left:
			    prefab = huoYanBossLeftPrefab;
			    break;
		    case EffectType.HuoYan_Boss_Right:
			    prefab = huoYanBossRightPrefab;
			    break;
		    case EffectType.HuoQiu_Boss:
			    prefab = huoQiuBossPrefab;
			    break;
		    case EffectType.ZhuaJi_Boss:
			    prefab = zhuaJiBossPrefab;
			    break;
		    case EffectType.ZhaoHuan_Boss:
			    prefab = zhaoHuanBossPrefab;
			    break;
		    case EffectType.HuoZhen_Boss:
			    prefab = huoZhenBossPrefab;
			    break;
		    case EffectType.AOE:
			    prefab = aOEPrefab;
			    break;

		}
		
		
		return prefab;
    }

    public GameObject CreateEffect(string effectName, Vector3 position)
    {
        if (string.IsNullOrEmpty(effectName))
        {
            return null;
        }

        GameObject toCreate = GetEffectFromPool(effectName);
        if (toCreate == null)
        {
            toCreate = CreateNewEffectToScene(effectName);
            toCreate.transform.localPosition = Vector3.zero;
            toCreate.transform.localRotation = Quaternion.identity;
            SkillEffect se = toCreate.AddComponent<SkillEffect>();
            se.CreateEffect(position);
            AddCount(se);
        }
        else
        {
            toCreate.transform.localPosition = Vector3.zero;
            toCreate.transform.localRotation = Quaternion.identity;
            SkillEffect se = toCreate.GetComponent<SkillEffect>();
            se.EnableEffect(position);
            AddCount(se);
        }

        return toCreate;

    }

    public GameObject CastEffect (string effectName, Vector3 position, float lifeTime)
    {
        if (string.IsNullOrEmpty(effectName))
        {
            return null;
        }
        //if (ShowLog)
        //{
        //    Debug.Log("!!CastEffect:" + effectName);
        //}
        GameObject toCreate = GetEffectFromPool(effectName);
        if (toCreate == null)
        {
            //if (ShowLog)
            //{
            //    Debug.Log(effectName + "不在池中");
            //}
            toCreate = CreateNewEffectToScene(effectName);
            toCreate.transform.localPosition = Vector3.zero;
            toCreate.transform.localRotation = Quaternion.identity;
            SkillEffect se = toCreate.AddComponent<SkillEffect>();
            se.CreateEffect(position, lifeTime);
            AddCount(se);

        }
        else
        {
            toCreate.transform.localPosition = Vector3.zero;
            toCreate.transform.localRotation = Quaternion.identity;
            SkillEffect se = toCreate.GetComponent<SkillEffect>();
            se.EnableEffect(position, lifeTime);
            AddCount(se);

        }



        return toCreate;
    }


    public void DestroyEffect (GameObject go)
    {
        if (go == null)
        {
            //if (ShowLog)
            //{
            //    Debug.LogWarning("需要销毁的物体为空");
            //}
            return;
        }
        else
        {
            //if (ShowLog)
            //{
            //    Debug.Log("DestroyEffect:" + go.name);
            //}
        }
        GameObject inputGo = go;
        if (go.name == tempEffectBox)
        {
            if (go.transform.childCount > 0)
            {
                inputGo = go.transform.GetChild(0).gameObject;
            }
            //Debug.LogError("DestroyEffect+"+effectInSceneDic.Count);
        }
        GameObject toRemove = null;
        GameObject toDestory = null;

        foreach (KeyValuePair<string, List<GameObject>> pair in effectInSceneDic)
        {
            foreach (GameObject goInDic in pair.Value)
            {
                if (inputGo == goInDic)
                {
                    if (poolDic.ContainsKey(pair.Key) && poolDic[pair.Key] != null)
                    {
                        if (poolDic[pair.Key].Count < MAX_COUNT_OFONE_EFFECT)
                        {
                            toRemove = inputGo;
                            SkillEffect se = inputGo.GetComponent<SkillEffect>();
                            se.DisableEffect();
                        }
                        else
                        {
                            toDestory = inputGo;
                        }
                    }
                    break;
                }
            }
            if (toRemove != null || toDestory != null)
            {
                break;
            }

        }

        if (toRemove != null)
        {
            AddEffectToPool(toRemove.name, inputGo);
        }

        if (toDestory != null)
        {
            effectInSceneDic[inputGo.name].Remove(toDestory);
            GameObject.Destroy(toDestory);
        }

        if (inputGo != go)
        {
            GameObject.Destroy(go);
        }
				
    }

    void PoolClearCheck()
    {
        //List<string> toClearEffect = new List<string>();
        //foreach (KeyValuePair<string, float> pair in skillCreateTime)
        //{
        //    if (Time.time - pair.Value > CLEAR_POOL_TIME)
        //    {
        //        foreach (GameObject go in poolDic[pair.Key])
        //        {
        //            GameObject.Destroy(go);
        //        }
        //        poolDic.Remove(pair.Key);
        //        toClearEffect.Add(pair.Key);
        //    }
        //}

        //foreach (string name in toClearEffect)
        //{
        //    skillCreateTime.Remove(name);
        //}
        //toClearEffect.Clear();
    }


    void AddCount(SkillEffect se)
    {
        //if (skillCreateTime.ContainsKey(se.name))
        //{
        //    skillCreateTime[se.name] = Time.time;
        //}
        //else
        //{
        //    skillCreateTime.Add(se.name, Time.time);
        //}

        //if (Time.time - lastUpdateTime > LEAST_UPDATE_TIME)
        //{
        //    lastUpdateTime = Time.time;
        //}
    }

    GameObject GetEffectFromPool(string effectName)
    {
        if (HasEffectInPool(effectName))
        {
            if (!effectInSceneDic.ContainsKey(effectName))
            {
                effectInSceneDic.Add(effectName, new List<GameObject>());
            }
            GameObject go = poolDic[effectName][0];

            poolDic[effectName].Remove(go);
            effectInSceneDic[effectName].Add(go);

            return go;
        }
        return null;
    }

    bool HasEffectInPool(string name)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].Count > 0 && poolDic[name] != null)
        {
            return true;
        }
        return false;
    }

    GameObject AddEffectToPool(string effectName, GameObject go)
    {
        if (poolDic.ContainsKey(effectName))
        {
            if (go != null)
            {
                if (!poolDic[effectName].Contains(go))
                {
                    poolDic[effectName].Add(go);
                }
                if (effectInSceneDic[effectName].Contains(go))
                {
                    effectInSceneDic[effectName].Remove(go);
                }
                return go;
            }
        }
        return null;
    }

    GameObject CreateNewEffectToScene(string effectName)
    {
        GameObject go = GetPrefab(effectName);
        if (go != null)
        {
            GameObject newGO = GameObject.Instantiate(go) as GameObject;
            if (newGO == null)
            {
                //if (ShowLog)
                //{
                Debug.LogWarning("实例化特效为空");
                //}
                return null;
            }
            newGO.name = go.name;
            if (!effectInSceneDic.ContainsKey(effectName))
            {
                effectInSceneDic.Add(effectName, new List<GameObject>());
            }
            effectInSceneDic[effectName].Add(newGO);

            if (!poolDic.ContainsKey(effectName))
            {
                poolDic.Add(effectName, new List<GameObject>());
            }

            return newGO;
        }
        return null;
    }
}

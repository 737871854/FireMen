using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using Need.Mx;

public class SceneManager : SingletonBehaviour<SceneManager>
{
    public enum SceneLevelID
    {
        Unknown,
        Level_1 = 1,
        Level_2 = 2,
        Level_3 = 3,

    }

    public enum GameState
    {
        Unknown,
        Ready = 1, 
        Play  = 2,
        Pause = 3,
    }

    public class DelayRefresh
    {
        public float refreshTime;
        public int refreshId;
        public Transform bind;
        public Vector3 pos;
    }

    public static LayerMask MASK_OBJECT = 1;
    public static LayerMask MASK_TARGET = 1;
    public static LayerMask MASK_MAP    = 1;
    public SceneLevelID sceneId;
    protected int refreshIndex;
    protected int uniqueId;
    protected int delayId;
    protected Dictionary<string, Monster> monsterDict;
    //protected Dictionary<int,DelayRefresh> delayRefreshDict;
    protected List<DelayRefresh> delayRefreshDict;
    protected List<DelayRefresh> delaySummonDict;
    protected List<string> monsterNameList;
    protected List<string> delObject;

	// Use this for initialization
	void Start () {
        MASK_OBJECT = 1 << LayerMask.NameToLayer(SceneLayerMask.Actor);
        MASK_MAP    = 1 << LayerMask.NameToLayer(SceneLayerMask.Map);
        MASK_TARGET = 1 << LayerMask.NameToLayer(SceneLayerMask.Target);
        delayId          = 1;
        refreshIndex     = 1;
        uniqueId         = 1;
        //delayRefreshDict = new Dictionary<int, DelayRefresh>();
        delayRefreshDict = new List<DelayRefresh>();
        monsterDict      = new Dictionary<string, Monster>();
        monsterNameList  = new List<string>();
        delaySummonDict  = new List<DelayRefresh>();
        delObject        = new List<string>(50);
        addEvent();
	}

    void OnDestroy()
    {
        removeEvent();
    }
	
    public void UpdatePerFrame()
    {
        OnUpdateMonster();
        OnDelaySummon();
        DoorManager.instance.UpdateDoor();
        OnRefresh();
        OnDelayRefresh();
    }

    public void UpdateFixFrame()
    {
        
    }

    public int getMonsterCount()
    {
        return monsterDict.Count;
    }

    /// <summary>
    /// 怪物刷新
    /// </summary>
    void OnRefresh()
    {
        while (true)
        {
            int refreshId;
            if ((int)sceneId != 3)
            {
                refreshId = ((int)sceneId * 10000000) + 10000 + refreshIndex;
            }
            else
            {
                refreshId = ((int)sceneId * 10000000) + (GameConfig.GAME_CONFIG_DIFFICULTY * 10001) + refreshIndex - 1;
            }

            MonsterRefreshPO refreshPO = MonsterRefreshData.Instance.GetMonsterRefreshPO(refreshId);
            if (refreshPO == null)
            {
                return;
            }

            MonsterPO monsterPO = MonsterData.Instance.GetMonsterPO(refreshPO.MonsterId);
            if (monsterPO == null)
            {
                return;
            }

            if (Main.GameMode.CheckPoint < refreshPO.CheckPoint)
            {
                return;
            }

            if (Main.GameMode.CheckPoint > refreshPO.CheckPoint)
            {
                ++refreshIndex;
                continue;
            }

            // 判断时间
            float refreshTime = (float)refreshPO.AppeareTime / 1000.0f;
            if (refreshTime > Main.GameMode.GameTime)
            {
                return;
            }

            // 针对有限数量的刷新
            if (refreshPO.MonsterNumber > 0)
            {
                for (int index = 0; index < refreshPO.MonsterNumber; ++index)
                {
                    // 马上刷出
                    if (refreshPO.SeparateTime == 0)
                    {
                        if (monsterPO.FireCarId > 0 && !Main.PlayerManager.getPlayer(monsterPO.FireCarId-1).IsPlaying())
                        {
                            return;
                        }

                        if (refreshPO.WindowName.Length > 0 && !DoorManager.instance.OpenDoor(refreshPO.WindowName))
                        {
                            return;
                        }
                        Monster go = CreateMonster(refreshPO);
                        CarManager.instance.Help(go.CarId, go);
                    }
                    // 延迟刷出
                    else
                    {
                        DelayRefresh delay = new DelayRefresh();
                        delay.refreshId = refreshId;
                        delay.refreshTime = Main.GameMode.GameTime + ((float)refreshPO.SeparateTime / 1000.0f) * index;
                        //delayRefreshDict.Add(delayId++, delay);
                        delayRefreshDict.Add(delay);
                    }
                }
            }
            // 如果要循环刷新，这两个值必须有规定
            else if (refreshPO.MonsterNumber == -1 && refreshPO.SeparateTime > 0)
            {
                bool createFlag = true;
                if (monsterPO.FireCarId > 0 && !Main.PlayerManager.getPlayer(monsterPO.FireCarId - 1).IsPlaying())
                {
                    createFlag = false;
                }

                if (createFlag)
                {
                    if (refreshPO.WindowName.Length > 0 && !DoorManager.instance.OpenDoor(refreshPO.WindowName))
                    {
                        return;
                    }

                    Monster go = CreateMonster(refreshPO);
                    CarManager.instance.Help(go.CarId, go);
                }

                DelayRefresh delay = new DelayRefresh();
                delay.refreshId = refreshId;
                delay.refreshTime = Main.GameMode.GameTime + ((float)refreshPO.SeparateTime / 1000.0f);
                //delayRefreshDict.Add(delayId++, delay);
                delayRefreshDict.Add(delay);
            }

            ++refreshIndex;
        }
    }

    /// <summary>
    /// 怪物延迟刷新
    /// </summary>
    void OnDelayRefresh()
    {
        // 按照策划的要求，延迟队列必须是每次全部执行，保证所有怪物的刷新时间能够得到有效的判断
        //List<int> deleteList = new List<int>();
        //foreach(KeyValuePair<int, DelayRefresh> element in delayRefreshDict) 
        for (int index = 0; index < delayRefreshDict.Count; ++index)
        {
            DelayRefresh element = delayRefreshDict[index];
            MonsterRefreshPO refreshPO = MonsterRefreshData.Instance.GetMonsterRefreshPO(element.refreshId);
            if (refreshPO == null)
            {
                continue;
            }

            MonsterPO monsterPO = MonsterData.Instance.GetMonsterPO(refreshPO.MonsterId);
            if (monsterPO == null)
            {
                return;
            }

            if (Main.GameMode.CheckPoint != refreshPO.CheckPoint)
            {
                //deleteList.Add(element.Key);
                delayRefreshDict.RemoveAt(index);
                return;
            }

            if (element.refreshTime >= Main.GameMode.GameTime)
            {
                continue;
            }

            bool createFlag = true;
            if (monsterPO.FireCarId > 0 && !Main.PlayerManager.getPlayer(monsterPO.FireCarId - 1).IsPlaying())
            {
                createFlag = false;
            }

            if (createFlag)
            {
                if (refreshPO.WindowName.Length > 0 && !DoorManager.instance.OpenDoor(refreshPO.WindowName))
                {
                    continue;
                }

                Monster go = CreateMonster(refreshPO);
                CarManager.instance.Help(go.CarId, go);
            }

            if (refreshPO.MonsterNumber == -1)
            {
                element.refreshTime = Main.GameMode.GameTime + ((float)refreshPO.SeparateTime / 1000.0f);
                delayRefreshDict[index] = element;
                continue;
            }

            //deleteList.Add(element.Key);
            delayRefreshDict.RemoveAt(index);
            return;

        }

        //for (int index = 0; index < deleteList.Count; ++index)
        //{
        //    delayRefreshDict.Remove(deleteList[index]);
        //}
    }

    /// <summary>
    /// 怪物延迟召唤
    /// </summary>
    void OnDelaySummon()
    {
        while (delaySummonDict.Count > 0)
        {
            DelayRefresh element = delaySummonDict[0];
            element.refreshTime -= Time.deltaTime;
            if (element.refreshTime <= 0)
            {
                CreateMonster(element.refreshId, element.pos, element.bind, -1);
                delaySummonDict.RemoveAt(0);
            }
            else
            {
                delaySummonDict[0] = element;
                break;
            }
        }
    }

    void OnUpdateMonster()
    {
        for (int index = 0; index < monsterNameList.Count; ++index)
        {
            if (monsterDict.ContainsKey(monsterNameList[index]) == false)
            {
                continue;
            }
            Monster go = monsterDict[monsterNameList[index]];
            go.OnUpdate();
            if (go.IsDestroy())
            {
                CarManager.instance.HangUp(go.CarId, go);
                CarManager.instance.Rescued(go.CarId, go);
                delObject.Add(go.name);
            }
        }

        for (int index = 0; index < delObject.Count; ++index)
        {
            SceneManager.instance.DestroyMonster(delObject[index]);
        }
        delObject.Clear();
    }

    /// <summary>
    /// 从空间中Pick中某个物体
    /// </summary>
    public Monster PickMonster(Vector2 screenPos)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, MASK_OBJECT))
        {
            if (monsterDict.ContainsKey(hit.transform.name) == false)
            {
                return null;
            }
            return monsterDict[hit.transform.name];
        }
        return null;
    }

    public bool PickObject(Vector2 screenPos, out Monster monster, out GameObject goTarget)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        monster  = null;
        goTarget = null;
        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, MASK_TARGET))
        {
            if (monsterDict.ContainsKey(hit.transform.root.name) == false)
            {
                return false;
            }
            monster  = monsterDict[hit.transform.root.name];
            goTarget = hit.transform.gameObject;
            return true;
        }
        return false;
    }

    public void getAllLifeMonsterName(out List<string> targetNameList)
    {
        targetNameList = monsterNameList;
    }

    public Monster PickMonster(string name)
    {
        if (monsterDict.ContainsKey(name) == false)
        {
            return null;
        }
        return monsterDict[name];
    }

    protected Monster CreateMonster(MonsterRefreshPO refreshPO)
    {
        Vector3 pos = new Vector3();
        if (refreshPO.AppearePoint.Length == 3)
        {
            pos.x = refreshPO.AppearePoint[0];
            pos.y = refreshPO.AppearePoint[1];
            pos.z = refreshPO.AppearePoint[2];
        }
        else if (refreshPO.AppeareArea != "NULL")
        {
            AreaManager.instance.getRandomPositionInArea(refreshPO.AppeareArea, ref pos);
        }

        Monster go = MonsterManager.instance.CreateMonster(uniqueId, pos, new Vector3(0, 180, 0), refreshPO.MonsterId);
        ++uniqueId;
        monsterDict.Add(go.name, go);
        monsterNameList.Add(go.name);
        return go;
    }

    public void CreateMonster(int monsterDataPOId, Vector3 pos, Transform bindBone, float summonDelay)
    {
        if (summonDelay <= 0)
        {
            // 马上刷新
            Vector3 newPos = pos;
            if (bindBone != null)
            {
                newPos = bindBone.position;
            }

            Monster go = MonsterManager.instance.CreateMonster(uniqueId, newPos, new Vector3(0, 180, 0), monsterDataPOId);
            ++uniqueId;
            monsterDict.Add(go.name, go);
            monsterNameList.Add(go.name);
        }
        else
        {
            // 延迟刷新
            DelayRefresh delay = new DelayRefresh();
            delay.refreshId    = monsterDataPOId;
            delay.refreshTime  = summonDelay;
            delay.bind         = bindBone;
            delay.pos          = pos;
            delaySummonDict.Add(delay);
        }
    }

    public void DestroyMonster(GameObject go)
    {
        monsterDict.Remove(go.name);
        Destroy(go);
    }

    public void DestroyMonster(string goName)
    {
        if (monsterDict.ContainsKey(goName) == false)
        {
            return;
        }

        Monster go = monsterDict[goName];
        monsterDict.Remove(goName);
        monsterNameList.Remove(goName);
        Destroy(go.gameObject);
    }

    public void DestroyAllMonster()
    {
        for (int index = 0; index < monsterNameList.Count; ++index)
        {
            if (monsterDict.ContainsKey(monsterNameList[index]) == false)
            {
                continue;
            }
            SceneManager.instance.DestroyMonster(monsterNameList[index]);
        }

        monsterNameList.Clear();
        delayRefreshDict.Clear();
        monsterDict.Clear();
        delaySummonDict.Clear();
    }

    /// <summary>
    /// 添加逻辑监听
    /// </summary>
    protected virtual void addEvent()
    {
        EventDispatcher.AddEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE, UpdatePerFrame);
        EventDispatcher.AddEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE, UpdateFixFrame);
    }

    /// <summary>
    /// 移除逻辑事件监听
    /// </summary>
    protected virtual void removeEvent()
    {
        EventDispatcher.RemoveEventListener(GameEventDef.EVNET_PER_FRAME_UPDATE, UpdatePerFrame);
        EventDispatcher.RemoveEventListener(GameEventDef.EVNET_FIX_FRAME_UPDATE, UpdateFixFrame);
    }
}

using UnityEngine;
using System.Collections;
using Need.Mx;
public class MonsterManager : SingletonBehaviour<MonsterManager>
{

    /// <summary>
    /// 外部可配置字段
    /// </summary>
    public Monster firemanPrefab;
    public Monster dogPrefab;
    public Monster birdPrefab;
    public Monster babyCitizenPrefab;
    public Monster boyCitizenPrefab;
    public Monster bFireMonsterPrefab;
    public Monster catPrefab;
    public Monster girlCitizenPrefab;
	public Monster smokeMonsterPrefab;
	public Monster fireMonsterPrefab;
    public Monster bossPrefab;
	public Monster balloonScene1Prefab;
	public Monster balloonScene2Prefab;
	public Monster balloonScene3Prefab;

    /// <summary>
    /// 内部属性
    /// </summary>

    public Monster GetPrefab(string type)
    {
        Monster prefab = null;
        switch (type)
        {
            case ModelType.Fireman1:
                prefab = firemanPrefab;
                break;
            case ModelType.Fireman2:
                break;
            case ModelType.Firewoman1:
                break;
            case ModelType.FireMonster1:
			    prefab = fireMonsterPrefab;
                break;
            case ModelType.BFireMonster1:
                prefab = bFireMonsterPrefab;
                break;
            case ModelType.Boss:
                prefab = bossPrefab;
                break;
            case ModelType.SmokeMonster1:
			    prefab = smokeMonsterPrefab;
                break;
            case ModelType.BabyCitizen:
                prefab = babyCitizenPrefab;
                break;
            case ModelType.GirlCitizen:
                prefab = girlCitizenPrefab;
                break;
            case ModelType.BoyCitizen:
                prefab = boyCitizenPrefab;
                break;
            case ModelType.Dog1:
                prefab = dogPrefab;
                break;
            case ModelType.Cat1:
                prefab = catPrefab;
                break;
            case ModelType.Bird:
                prefab = birdPrefab;
                break;
            case ModelType.CloudFireCar:
                break;
		    case ModelType.Balloon_Scene1:
			    prefab = balloonScene1Prefab;
			    break;
		    case ModelType.Balloon_Scene2:
			    prefab = balloonScene2Prefab;
			    break;
		    case ModelType.Balloon_Scene3:
			    prefab = balloonScene3Prefab;
			    break;
        }

        return prefab;
    }

    public Monster CreateMonster(int id, Vector3 position, Vector3 dir, int dataId)
    {
        MonsterPO po = MonsterData.Instance.GetMonsterPO(dataId);
        if (po == null)
        {
            return null;
        }

        Monster prefab = GetPrefab(po.ShapeName);
        if (prefab == null)
        {
            return null;
        }

        Monster go = GameObject.Instantiate(prefab, position, Quaternion.AngleAxis(180, Vector3.up)) as Monster;
        go.Init(id,po);
        go.name = go.name + "_" + id;
        return go;
    }
}

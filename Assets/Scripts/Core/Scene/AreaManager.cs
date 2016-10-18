using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using Need.Mx;

public class AreaManager : SingletonBehaviour<AreaManager>
{

    public GameObject []                 areaList;
    protected Dictionary<string, int> areaIndex;

	// Use this for initialization
	void Start () {

        areaIndex = new Dictionary<string, int>();

        for (int index = 0; index < areaList.Length; ++index)
        {
            areaIndex.Add(areaList[index].name, index);
        } 
	}
	
	// Update is called once per frame
    //void Update () {
	
    //     Vector3 pos = new Vector3();
    //     getRandomPositionInArea("FloorArea1",ref pos);
    //}

    /// <summary>
    /// 根据区域名字随机获得一个空间位置
    /// </summary>
    public bool getRandomPositionInArea(string name, ref Vector3 pos)
    {
        if (areaIndex.ContainsKey(name) == false)
        {
            return false;
        }

        GameObject go = areaList[areaIndex[name]];
        if (go == null)
        {
            return false;
        }

        float x = go.transform.localScale.x / 2.0f;
        float y = go.transform.localScale.y / 2.0f;
        float z = go.transform.localScale.z / 2.0f;
        pos.x = Random.Range(-x, x);
        pos.y = Random.Range(-y, y);
        pos.z = Random.Range(-z, z);
        pos = go.transform.position + pos;
        //Log.Hsz(pos);
        //Log.Hsz(go.transform.position);
        //Log.Hsz(IsPositionInArea(name,pos));
        return true;
    }

    /// <summary>
    /// 根据区域名字判断空间的某一点是否在区域内
    /// </summary>
    public bool IsPositionInArea(string name, Vector3 pos)
    {
        if (areaIndex.ContainsKey(name) == false)
        {
            return false;
        }
        GameObject go = areaList[areaIndex[name]];
        if (go == null)
        {
            return false;
        }

        Vector3 cloeset = new Vector3();
        cloeset = go.GetComponent<BoxCollider>().ClosestPointOnBounds(pos);
        float distance = Vector3.Distance(cloeset, pos);
        if (distance <= 0.01)
        {
            return true;
        }

        return false;
    }
}

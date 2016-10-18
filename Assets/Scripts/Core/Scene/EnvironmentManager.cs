using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public class EnvironmentManager : SingletonBehaviour<EnvironmentManager>
{
    public float streetMoveSpeed;
    public float streetMoveSpeedScale;
    public List<GameObject> streetList;
    public float rightBoundary;
    public float leftBoundary;

    public float skyMoveSpeed;
    public float skyMoveSpeedScale;
    public GameObject skybox;
    protected MeshRenderer renderer;
    
    public float explodeStarRange;
    public Vector3 explodeStarNear;
    public Vector2 explodeStarScale;

	// Use this for initialization
	void Start () {
        if (skybox != null)
        {
            renderer = skybox.GetComponent<MeshRenderer>();
        }
        addEvent();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateStreetList();
        UpdateSky();
	}

    void OnDestroy()
    {
        removeEvent();
    }


    void UpdateSky()
    {
        if (renderer)
        {
            Vector2 offset = renderer.material.GetTextureOffset("_MainTex");
            offset.x = offset.x + (Time.deltaTime * skyMoveSpeed);
            renderer.material.SetTextureOffset("_MainTex", offset);
        }
    }

    void UpdateStreetList()
    {
        if (streetList != null)
        {
            for (int index = 0; index < streetList.Count; ++index)
            {
                GameObject go = streetList[index];
                if (go.transform.position.x >= leftBoundary)
                {
                    Vector3 position = go.transform.position;
                    position.x = rightBoundary;
                    go.transform.position = position;
                }
            }
        }

        if (streetList != null)
        {
            for (int index = 0; index < streetList.Count; ++index)
            {
                GameObject go = streetList[index];
                Vector3 position = go.transform.position;
                position.x = position.x + (Time.deltaTime * streetMoveSpeed);
                go.transform.position = position;
            }
        }
    }

    void OnEventMoveSpeedScale()
    {
        streetMoveSpeed *= streetMoveSpeedScale;
        skyMoveSpeed    *= skyMoveSpeedScale;
    }


    /// <summary>
    /// 添加逻辑监听
    /// </summary>
    protected virtual void addEvent()
    {
        EventDispatcher.AddEventListener(GameEventDef.EVNET_ENVIRONMENT_MOVE_SPEED_SCALE, OnEventMoveSpeedScale);
      
    }

    /// <summary>
    /// 移除逻辑事件监听
    /// </summary>
    protected virtual void removeEvent()
    {
        EventDispatcher.RemoveEventListener(GameEventDef.EVNET_ENVIRONMENT_MOVE_SPEED_SCALE, OnEventMoveSpeedScale);
       
    }
}

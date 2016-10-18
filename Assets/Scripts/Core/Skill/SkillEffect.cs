using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Step
{
    None,
    First,
    Second,
    Third,
}

public class SkillEffect : MonoBehaviour
{
    private Step    step = Step.None;
    private float   lifeTime;
    private int     playCount;
    private Vector3 pos;
    private float   startTime;
    private GameObject skillRoot;
    
    public void EnableEffect(Vector3 pos, float time = -1, int count = -1)
    {
        skillRoot = GameObject.Find("Game Manager/Effect");
        transform.parent = null;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(true);
        lifeTime = time;
        playCount = count;
        transform.position = pos;
        step = Step.First;      
    }

    void Update()
    {
        if (step == Step.First)
        {
            step = Step.Second;
        }
        else if (step == Step.Second)
        {
            step = Step.Third;
            EnbaleAllChildrenEffect();
            startTime = Time.time;

        }
        else if (step == Step.Third)
        {
            CheckIfTimeUp();
        }
    }

    void CheckIfTimeUp()
    {
        if (lifeTime != SkillEffectManager.NO_TIME)
        {
            if (Time.time - startTime > lifeTime)
            {
                TimesUp();
            }
        }
    }


    void TimesUp()
    {
        step = Step.None;
        if (transform.parent != null && transform.parent.name == SkillEffectManager.instance.tempEffectBox)
        {
            SkillEffectManager.instance.DestroyEffect(transform.parent.gameObject);
        }
        else
        {
            SkillEffectManager.instance.DestroyEffect(gameObject);
        }
    }

    public void DisableEffect()
    {
        step = Step.None;
        DisableAllChildrenEffect();
        gameObject.SetActive(false);
        if (skillRoot != null)
        {
            transform.parent = skillRoot.transform;
        }
        transform.position = Vector3.zero;
    }

    private void SetChildrenActive(GameObject go, bool active)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject childObj = gameObject.transform.GetChild(i).gameObject;
            childObj.SetActive(active);
        }
    }

    void EnbaleAllChildrenEffect()
    {
        SetChildrenActive(gameObject, true);
    }

    void DisableAllChildrenEffect()
    {
        SetChildrenActive(gameObject, false);
    }

    public void CreateEffect(Vector3 pos, float time = -1)
    {
        EnableEffect(pos, time);
    }

    public void DestoryEffect()
    {
        Destroy(gameObject);
    }
}

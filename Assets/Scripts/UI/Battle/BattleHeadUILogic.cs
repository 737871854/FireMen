using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;
using DG.Tweening.Core;
using Need.Mx;

public class BattleHeadUILogic : MonoBehaviour {

    public class HeadUI
    {
        public GameObject uiGo;
        public Slider bar;
        public float showTime;
        public RectTransform rt;
        public Image image;
    }

    public class FloatUI
    {
        public GameObject uiGo;
        public int index;
        public int damage;
    }

    public class HeadUIParam
    {
        public GameObject goHead;
        public int id;
        public int max;
        public int min;
        public Monster.MonsterType type;
        public int damage;

        void Reset()
        {
            goHead  = null;
            id      = 0;
            max     = 0;
            min     = 0;
            type    = Monster.MonsterType.Unknown;
            damage  = 0;
        }

        private static Stack<HeadUIParam> objectStack = new Stack<HeadUIParam>();
        public static HeadUIParam New()
        {
            return (objectStack.Count == 0) ? new HeadUIParam() : objectStack.Pop();
        }

        public static void Store(HeadUIParam t)
        {
            t.Reset();
            objectStack.Push(t);
        }
    }


    protected BattleHeadUIView view;
    protected Dictionary<int, HeadUI> headUIDict;
    protected Dictionary<int, HeadUI> weaponUIDict;
    protected Dictionary<int, HeadUI> hintUIDict;
    protected List<FloatUI> floatUIList;
    protected List<GameObject> crackUIList;
    protected List<GameObject> starUIList;
    protected Dictionary<string,GameObject> hittingUIList;

	// Use this for initialization
	void Start () 
    {
        headUIDict = new Dictionary<int, HeadUI>();
        weaponUIDict = new Dictionary<int, HeadUI>();
        hintUIDict = new Dictionary<int, HeadUI>();
        floatUIList = new List<FloatUI>();
        crackUIList = new List<GameObject>();
        hittingUIList = new Dictionary<string, GameObject>();
        starUIList = new List<GameObject>();
        view = gameObject.GetComponent<BattleHeadUIView>();
        view.Init();
        Init();
        addEvent();
        addUIEventListener();
	}

    void OnDestroy()
    {
        removeEvent();
        removeUIEventListener();
    }

    void Init()
    {
        if (Main.SettingManager.GameLanguage ==  0)
        {
            view.image_hit.SetActive(true);
            view.image_hit_en.SetActive(false);
        }
        else
        {
            view.image_hit.SetActive(false);
            view.image_hit_en.SetActive(true);
        }
    }

    // Update is called once per frame
    //public void UpdatePerFrame()
    //{
        
    //}

    //public void UpdateFixFrame()
    //{
       
    //}

    public void OnEventAppendHeadUI(BattleHeadUILogic.HeadUIParam param)
    {
        if (param.type != Monster.MonsterType.Boss)
        {
            OnEventAppendMonsterHeadUI(param);
        }
        else
        {
            OnEventAppendBossHeadUI(param);
        }

        HeadUIParam.Store(param);
    }

    public void OnEventAppendMonsterHeadUI(BattleHeadUILogic.HeadUIParam param)
    {
        HeadUI headUI = new HeadUI();
        headUI.showTime = 0;

        switch (param.type)
        {
            case Monster.MonsterType.Senior:
            case Monster.MonsterType.Normal:
                {
                    headUI.uiGo = GameObject.Instantiate(view.panel_RedHPBar.gameObject) as GameObject;
                    break;
                }
            case Monster.MonsterType.NPC:
                {
                    headUI.uiGo = GameObject.Instantiate(view.panel_GreenHPBar.gameObject) as GameObject;
                    break;
                }
        }

        headUI.uiGo.transform.SetParent(view.ui_Parent.transform);
        //headUI.uiGo.gameObject.SetActive(true);
        Vector2 pos;
        headUI.image = headUI.uiGo.GetComponent<Image>();
        Canvas canvas = headUI.image.canvas;
        pos = Camera.main.WorldToScreenPoint(param.goHead.transform.position);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
        {
            headUI.rt = headUI.uiGo.GetComponent<RectTransform>();
            headUI.rt.anchoredPosition = pos;
        }

        headUI.bar = headUI.uiGo.transform.Find("Image_HPBar").GetComponent<Slider>();
        headUI.bar.value = (float)param.min / (float)param.max;

        headUIDict.Add(param.id, headUI);
    }

    public void OnEventAppendBossHeadUI(BattleHeadUILogic.HeadUIParam param)
    {
        view.panel_BossHPBar.gameObject.SetActive(true);
        view.slider_BossHPBar1.value = 1.0f;
        view.slider_BossHPBar2.value = 1.0f;
        view.slider_BossHPBar3.value = 1.0f;
        view.image_BossHPNum1.gameObject.SetActive(false);
        view.image_BossHPNum2.gameObject.SetActive(false);
        view.image_BossHPNum3.gameObject.SetActive(true);
        HeadUI headUI = new HeadUI();
        headUIDict.Add(param.id, headUI);
    }

    public void OnEventRemoveHeadUI(int id)
    {
        if (headUIDict.ContainsKey(id))
        {
            HeadUI headUI = headUIDict[id];
            if (headUI.uiGo != null)
            {
                Destroy(headUI.uiGo);
            }
            else
            {
                view.panel_BossHPBar.gameObject.SetActive(false);
            }
            headUIDict.Remove(id);
        }

    }

    public void OnEventUpdateHeadUI(BattleHeadUILogic.HeadUIParam param)
    {
        if (param.type != Monster.MonsterType.Boss)
        {
            OnEventUpdateMonsterHeadUI(param);
        }
        else
        {
            OnEventUpdateBossHeadUI(param);
        }
        HeadUIParam.Store(param);
    }

    public void OnEventUpdateMonsterHeadUI(BattleHeadUILogic.HeadUIParam param)
    {
        if (headUIDict.ContainsKey(param.id))
        {
            HeadUI headUI = headUIDict[param.id];
            if (!headUI.uiGo.gameObject.activeSelf && param.damage == 0)
            {
                return;
            }

            Vector2 pos;
            Canvas canvas = headUI.image.canvas;
            pos = Camera.main.WorldToScreenPoint(param.goHead.transform.position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
            {
                headUI.rt.anchoredPosition = pos;
            }
            headUI.bar.value = (float)param.min / (float)param.max;

            if (param.damage > 0)
            {
                switch (param.type)
                {
                    case Monster.MonsterType.Senior:
                    case Monster.MonsterType.Normal:
                        {
                            headUI.showTime += GameConfig.GAME_CONFIG_SHOW_HEAD_UI_TIME_1;
                            break;
                        }
                    case Monster.MonsterType.NPC:
                        {
                            headUI.showTime += GameConfig.GAME_CONFIG_SHOW_HEAD_UI_TIME_2;
                            break;
                        }
                }
            }

            headUI.showTime -= Main.NonStopTime.deltaTime;
            if (headUI.showTime <= 0.0f)
            {
                headUI.showTime = 0.0f;
                headUI.uiGo.gameObject.SetActive(false);
            }
            else
            {
                headUI.uiGo.gameObject.SetActive(true);
            }
        }
    }

    public void OnEventUpdateBossHeadUI(BattleHeadUILogic.HeadUIParam param)
    {
        float value = (float)param.min / (float)param.max;
        if (value > 0.67f)
        {
            view.slider_BossHPBar1.value = (value - 0.67f) / 0.33f;
        }
        else if (value > 0.34f)
        {
            view.slider_BossHPBar1.value = 0.0f;
            view.slider_BossHPBar2.value = (value - 0.33f) / 0.33f;
            view.image_BossHPNum2.gameObject.SetActive(true);
            view.image_BossHPNum3.gameObject.SetActive(false);
        }
        else if (value > 0.0f)
        {
            view.slider_BossHPBar1.value = 0.0f;
            view.slider_BossHPBar2.value = 0.0f;
            view.slider_BossHPBar3.value = value / 0.33f;
            view.image_BossHPNum1.gameObject.SetActive(true);
            view.image_BossHPNum2.gameObject.SetActive(false);
            view.image_BossHPNum3.gameObject.SetActive(false);
        }
    }

    public void OnEventAppendFloatArtNumber(BattleHeadUILogic.HeadUIParam param)
    {
        int score = param.damage;
        string text = score.ToString();
        // 计算每个位置的数值
        int[] value = new int[4];
        int level = 100;
        int count = 1;
        bool sign = false; 
        for(int index = 1;  index < 4; ++index)
        {
            int number = (score / level) % 10;
            if (number > 0 && sign == false)
            {
                sign = true;
            }

            if (sign)
            {
                value[count++] = number;
            }
            level /= 10;
        }

        // 临时数据保存
        FloatUI floatUI = new FloatUI();
        floatUI.uiGo = GameObject.Instantiate(view.panel_FloatArtNumber.gameObject) as GameObject;
        floatUI.uiGo.transform.SetParent(view.ui_Parent.transform);
        floatUI.uiGo.transform.localScale = Vector3.one;

        // 获得Image组件
        Image[] artNumberSprite = new Image[4];
        for (int i = 0; i < 4; i++)
        {
            int number = i + 1;
            artNumberSprite[i] = floatUI.uiGo.transform.FindChild("Image_Number" + number.ToString()).GetComponent<Image>();
            artNumberSprite[i].gameObject.SetActive(false);
        }

        // 根据要显示的文字进行位置显示
        SetSpritePositionByLength(score.ToString().Length + 1, artNumberSprite);

        // 设置颜色
        Color color;
        if (param.type == Monster.MonsterType.NPC)
            color = Color.green;
        else
            color = Color.red;

        // 设置图片并显示
        Vector2 size = new Vector2(21, 29);
        for (int i = 0; i < text.Length; i++)
        {
            int num = 0;
            bool result = int.TryParse(text.Substring(i, 1), out num);
            if (i == 0)
            {
                artNumberSprite[0].gameObject.SetActive(true);
                artNumberSprite[0].color = color;
            }
            artNumberSprite[i + 1].sprite = view.image_Numbers[value[i + 1]].sprite;
            artNumberSprite[i + 1].gameObject.SetActive(true);
            artNumberSprite[i + 1].rectTransform.sizeDelta = size;
            artNumberSprite[i + 1].color = color;
        }

        // 映射头顶位置进行显示
        Vector2 pos;
        Canvas canvas = floatUI.uiGo.GetComponent<Image>().canvas;
        pos = Camera.main.WorldToScreenPoint(param.goHead.transform.position);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
        {
            floatUI.uiGo.GetComponent<RectTransform>().anchoredPosition = pos;
        }

        floatUI.uiGo.SetActive(true);
        // 动画表现
        RectTransform rectTransform = floatUI.uiGo.GetComponent<RectTransform>();
        Sequence sequence = DOTween.Sequence();
        pos.y += 50;
        sequence.Append(rectTransform.DOAnchorPos(pos, 1.0f));
        if (param.min == 0)
        {
            pos.Set(-554f, -409);
            sequence.Append(rectTransform.DOAnchorPos(pos, 0.75f));
        }
        else if (param.min == 1)
        {
            pos.Set(72f, -409);
            sequence.Append(rectTransform.DOAnchorPos(pos, 0.75f));
        }
        else if (param.min == 2)
        {
            pos.Set(711f, -409);
            sequence.Append(rectTransform.DOAnchorPos(pos, 0.75f));
        }
        sequence.OnComplete(OnFloatUIDisappearEnd);
        floatUI.index  = param.min;
        floatUI.damage = score;
        floatUIList.Add(floatUI);
        HeadUIParam.Store(param);
        // 1: -554  -409

        // 2:  72   -409

        // 3:  711  -409
    }

    protected void OnFloatUIDisappearEnd()
    {
        if (floatUIList.Count > 0)
        {
            FloatUI ui = floatUIList[0];
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_APPEND_SCORES, ui.index, ui.damage);
            GameObject.Destroy(ui.uiGo);
            floatUIList.RemoveAt(0);
        }
    }

    protected void SetSpritePositionByLength(int length, Image[] artNumberSprite)
    {
        List<int> artNumberPosition = new List<int>();
        artNumberPosition.Clear();
        for (int i = 0; i < 11; i++)
        {
            artNumberPosition.Add(-100 + i * 26);
        }

        switch (length)
        {
            case 1:
                artNumberSprite[0].rectTransform.localPosition = new Vector3(artNumberPosition[5], 0, 0);
                break;
            case 2:
            case 3:
                for (int i = 0; i < length; i++)
                {
                    artNumberSprite[i].rectTransform.localPosition = new Vector3(artNumberPosition[4 + i], 0, 0);
                }
                break;
            case 4:
            case 5:
                for (int i = 0; i < length; i++)
                {
                    artNumberSprite[i].rectTransform.localPosition = new Vector3(artNumberPosition[3 + i], 0, 0);
                }
                break;
            case 6:
            case 7:
                for (int i = 0; i < length; i++)
                {
                    artNumberSprite[i].rectTransform.localPosition = new Vector3(artNumberPosition[2 + i], 0, 0);
                }
                break;
            case 8:
            case 9:
                for (int i = 0; i < length; i++)
                {
                    artNumberSprite[i].rectTransform.localPosition = new Vector3(artNumberPosition[1 + i], 0, 0);
                }
                break;
            case 10:
                for (int i = 0; i < length; i++)
                {
                    artNumberSprite[i].rectTransform.localPosition = new Vector3(artNumberPosition[i], 0, 0);
                }
                break;
        }
    }

    
    public void OnEventAppendExplodeStar(Vector3 spacePos)
    {
        float move = 128.0f;
        float range = EnvironmentManager.instance.explodeStarRange;
        float size = EnvironmentManager.instance.explodeStarScale.y;
        float distance = Vector3.Distance(EnvironmentManager.instance.explodeStarNear, spacePos);
        float value = EnvironmentManager.instance.explodeStarScale.y * (1 - distance / range);
        if (value < EnvironmentManager.instance.explodeStarScale.x)
        {
            value = EnvironmentManager.instance.explodeStarScale.x;
        }

        if (!(Main.GameMode.SceneName() == SceneType.Scene3))
        {

            {
                int randomCount = Random.Range(1, 3);
                for (int index = 0; index < randomCount; ++index)
                {
                    int randomColor = Random.Range(-1, 3);
                    if (randomColor < 0 || randomColor >= view.image_Stars.Count)
                    {
                        continue;
                    }
                    Image star = GameObject.Instantiate(view.image_Stars[randomColor]) as Image;
                    star.transform.SetParent(view.panel_Fx.transform);
                    star.gameObject.SetActive(true);
                    Vector2 pos;
                    Canvas canvas = star.canvas;
                    pos = Camera.main.WorldToScreenPoint(spacePos);
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
                    {
                        star.rectTransform.anchoredPosition = pos;
                    }
                    star.rectTransform.localScale = new Vector3(0, 0, 0);
                    Vector2 dir = Random.insideUnitCircle;
                    pos += dir * move * value;

                    // -397 475
                    //  216 475
                    //  807 475

                    Vector2 vtPos;
                    if (Main.GameMode.CheckPoint == 1)
                        vtPos = new Vector2(-397, 441);
                    else if (Main.GameMode.CheckPoint == 2)
                        vtPos = new Vector2(216, 441);
                    else
                        vtPos = new Vector2(807, 441);

                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(star.rectTransform.DOAnchorPos(pos, 0.5f));
                    sequence.Join(star.rectTransform.DOScale(new Vector3(value, value, value), 0.25f));
                    sequence.Append(star.rectTransform.DOScale(new Vector3(1.8f, 1.8f, 1.8f), 1.0f));
                    sequence.Join(star.rectTransform.DOAnchorPos(vtPos, 1.0f));
                    starUIList.Add(star.gameObject);
                    sequence.OnComplete(OnCheckPlayStarEnd);
                }

            }
        }

        {
            Image create = GameObject.Instantiate(view.panel_ExplodeStar) as Image;
            create.transform.SetParent(view.panel_Fx.transform);
            create.gameObject.SetActive(true);
            Vector2 pos;
            Canvas canvas = create.canvas;
            pos = Camera.main.WorldToScreenPoint(spacePos);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
            {
                create.rectTransform.anchoredPosition = pos;
            }
            create.rectTransform.localScale = new Vector3(value, value, value);
            StartCoroutine(OnCheckCompleteExplodeStar(create));
        }

    }

    IEnumerator OnCheckCompleteExplodeStar(Image go)
    {
        yield return new WaitForSeconds(1.1f);
        GameObject.Destroy(go.gameObject);
    }

    protected void OnCheckPlayStarEnd()
    {
        if (starUIList.Count > 0)
        {
            GameObject.Destroy(starUIList[0]);
            starUIList.RemoveAt(0);
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_UPDATE_AOE_POWER);
        }
    }
    
    public void OnEventPlayCrackScreen(Vector3 spacePos)
    {
        int count = view.image_CrackScreens.Count;
        int index = Random.Range(0, count-1);
        Image original = view.image_CrackScreens[index];
        Image create   = GameObject.Instantiate(original) as Image;

        create.transform.SetParent(view.panel_CrackScreen.transform);
        create.gameObject.SetActive(true);

        Vector2 pos;
        Canvas canvas = create.canvas;
        pos = Camera.main.WorldToScreenPoint(spacePos);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
        {
            create.rectTransform.anchoredPosition = pos;
        }
        Color color = create.color;
        color.a = 0.0f;
        Tweener tween = create.DOColor(color, 1.2f);
        tween.OnComplete(OnPlayCrackScreenEnd);
        crackUIList.Add(create.gameObject);        
    }

    protected void OnPlayCrackScreenEnd()
    {
        if (crackUIList.Count > 0)
        {
            GameObject.Destroy(crackUIList[0]);
            crackUIList.RemoveAt(0);
        }
    }


    public void OnEventAppendHittingUI(Dictionary<string, Monster.HittingPart> dict)
    {
        foreach(KeyValuePair<string, Monster.HittingPart> element in dict)
        {
            GameObject go = GameObject.Instantiate(view.panel_HittingTarget1.gameObject) as GameObject;
            go.transform.SetParent(view.ui_Parent.transform);
            go.transform.localScale = new Vector3(2,2,2);
           

            Vector2 pos;
            Canvas canvas = go.GetComponent<Image>().canvas;
            pos = Camera.main.WorldToScreenPoint(element.Value.goBind.transform.position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
            {
                go.GetComponent<RectTransform>().anchoredPosition = pos;
            }
            go.SetActive(true);

            Image image_Hitting = go.GetComponent<Image>();
            Tweener tw = image_Hitting.rectTransform.DORotate(new Vector3(0, 0, 13), 0.1f);
            tw.SetLoops(-1, LoopType.Incremental);
            tw.Play();

            hittingUIList.Add(element.Key, go);
        }
    }

    public void OnEventRemoveHittingUI(Dictionary<string, Monster.HittingPart> dict)
    {
        foreach (KeyValuePair<string, Monster.HittingPart> element in dict)
        {
            if (hittingUIList.ContainsKey(element.Key))
            {
                GameObject.Destroy(hittingUIList[element.Key]);
                hittingUIList.Remove(element.Key);
            }
        }
    }

    public void OnEventUpdateHittingUI(Dictionary<string, Monster.HittingPart> dict)
    {
        foreach (KeyValuePair<string, Monster.HittingPart> element in dict)
        {
            if (hittingUIList.ContainsKey(element.Key))
            {
                GameObject go = hittingUIList[element.Key];
                if (element.Value.curHp <= 0)
                {
                    go.SetActive(false);
                }
                else
                {
                    float percent = (float)element.Value.curHp / (float)element.Value.maxHp;
                    if (percent > 0.67f)
                    {
                        
                    }
                    else if (percent > 0.34f)
                    {
                        go.GetComponent<Image>().sprite = view.panel_HittingTarget2.sprite;
                    }
                    else if (percent > 0.0f)
                    {
                        go.GetComponent<Image>().sprite = view.panel_HittingTarget3.sprite;
                    }

                    Vector2 pos;
                    Canvas canvas = go.GetComponent<Image>().canvas;
                    pos = Camera.main.WorldToScreenPoint(element.Value.goBind.transform.position);
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
                    {
                        go.GetComponent<RectTransform>().anchoredPosition = pos;
                    }
                }
            }
        }
    }

    public void OnEventDamageHittingUI(GameObject go)
    {
        GameObject goFX = GameObject.Instantiate(view.panel_HitBoss.gameObject) as GameObject;
        Vector2 pos;
        goFX.transform.SetParent(view.ui_Parent.transform);
        goFX.gameObject.SetActive(true);
        Canvas canvas = goFX.GetComponent<Image>().canvas;
        pos = Camera.main.WorldToScreenPoint(go.transform.position);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
        {
            goFX.GetComponent<RectTransform>().anchoredPosition = pos;
            goFX.GetComponent<RectTransform>().localScale = Vector3.one;
        }
        goFX.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        StartCoroutine(OnCreateHitBossFX(goFX));
    }


    public void OnEventAppendWeaponUI(BattleHeadUILogic.HeadUIParam param)
    {
        HeadUI headUI = new HeadUI();
        headUI.showTime = 0;
        headUI.uiGo = GameObject.Instantiate(view.panel_Weapon.gameObject) as GameObject;
        headUI.uiGo.transform.SetParent(view.ui_Parent.transform);
        headUI.uiGo.gameObject.SetActive(true);
        Vector2 pos;
        headUI.image = headUI.uiGo.GetComponent<Image>();
        Canvas canvas = headUI.image.canvas;
        pos = Camera.main.WorldToScreenPoint(param.goHead.transform.position);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
        {
            headUI.rt = headUI.uiGo.GetComponent<RectTransform>();
            headUI.rt.anchoredPosition = pos;
            headUI.rt.localScale = Vector3.one;
        }
        weaponUIDict.Add(param.id, headUI);
    }
    
    public void OnEventRemoveWeaponUI(int id)
    {
        if (weaponUIDict.ContainsKey(id))
        {
            HeadUI headUI = weaponUIDict[id];
            if (headUI.uiGo != null)
            {
                Destroy(headUI.uiGo);
            }
            weaponUIDict.Remove(id);
        }
    }


    public void OnEventUpdateWeaponUI(BattleHeadUILogic.HeadUIParam param)
    {
        if (weaponUIDict.ContainsKey(param.id))
        {
            HeadUI headUI = weaponUIDict[param.id];
            if (!headUI.uiGo.gameObject.activeSelf)
            {
                return;
            }

            Vector2 pos;
            Canvas canvas = headUI.image.canvas;
            pos = Camera.main.WorldToScreenPoint(param.goHead.transform.position);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
            {
                headUI.rt.anchoredPosition = pos;
            }

            if (param.damage > 0)
            {
                headUI.showTime += GameConfig.GAME_CONFIG_SHOW_HEAD_UI_TIME_1;
                GameObject goFX = GameObject.Instantiate(view.panel_HitBoss.gameObject) as GameObject;
                goFX.transform.SetParent(view.ui_Parent.transform);
                goFX.gameObject.SetActive(true);
                goFX.GetComponent<RectTransform>().anchoredPosition = pos;
                goFX.GetComponent<RectTransform>().localScale = Vector3.one;
                StartCoroutine(OnCreateHitBossFX(goFX));
            }

            headUI.showTime -= Main.NonStopTime.deltaTime;
            if (headUI.showTime <= 0.0f)
            {
                headUI.showTime = 0.0f;
                Slider hpBar = headUI.uiGo.transform.Find("Panel_Bar/Image_HPBar").GetComponent<Slider>();
                headUI.uiGo.transform.Find("Panel_Bar").gameObject.SetActive(false);
            }
            else
            {
                Slider hpBar = headUI.uiGo.transform.Find("Panel_Bar/Image_HPBar").GetComponent<Slider>();
                headUI.uiGo.transform.Find("Panel_Bar").gameObject.SetActive(true);
                hpBar.value = (float)param.min / (float)param.max;
            }
        }
    }

    IEnumerator OnCreateHitBossFX(GameObject go)
    {
        Animator animator = go.GetComponent<Animator>();
        while (true)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 1.0f && info.IsName("Play"))
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        GameObject.Destroy(go);
    }


    public void OnEventAppendRescueHintUI(BattleHeadUILogic.HeadUIParam param, int cardID)
    {
        HeadUI headUI = new HeadUI();
        headUI.showTime = 0;
        if (Main.SettingManager.GameLanguage == 0)
        {
            headUI.uiGo = GameObject.Instantiate(view.panel_RescueHint.gameObject) as GameObject;
        }
        else
        {
             headUI.uiGo = GameObject.Instantiate(view.panel_RescueHint_En.gameObject) as GameObject;
        }
        headUI.uiGo.transform.SetParent(view.ui_Parent.transform);
        headUI.uiGo.gameObject.SetActive(true);
        Vector2 pos;
        headUI.image = headUI.uiGo.GetComponent<Image>();
        Canvas canvas = headUI.image.canvas;
        pos = Camera.main.WorldToScreenPoint(param.goHead.transform.position);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
        {
            headUI.rt = headUI.uiGo.GetComponent<RectTransform>();
            headUI.rt.anchoredPosition = pos;
        }
        //headUI.rt.anchoredPosition = view.rescuePosList[cardID - 1].rectTransform.anchoredPosition;
        hintUIDict.Add(param.id, headUI);
    }

    public void OnEventRemoveRescueHintUI(int id)
    {
        if (hintUIDict.ContainsKey(id))
        {
            HeadUI headUI = hintUIDict[id];
            if (headUI.uiGo != null)
            {
                Destroy(headUI.uiGo);
            }
            hintUIDict.Remove(id);
        }
    }


    public void OnEventUpdateRescueHintUI(BattleHeadUILogic.HeadUIParam param, int cardID)
    {
        if (hintUIDict.ContainsKey(param.id))
        {
            HeadUI headUI = hintUIDict[param.id];
            if (!headUI.uiGo.gameObject.activeSelf)
            {
                return;
            }

            //Vector2 pos;
            //Canvas canvas = headUI.image.canvas;
            //pos = Camera.main.WorldToScreenPoint(param.goHead.transform.position);
            //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
            //{
            //    headUI.rt.anchoredPosition = pos;
            //}
            headUI.rt.anchoredPosition = view.rescuePosList[cardID - 1].rectTransform.anchoredPosition;
        }
    }


   
    /// <summary>
    /// 添加逻辑监听
    /// </summary>
    protected virtual void addEvent()
    {
        EventDispatcher.AddEventListener<BattleHeadUILogic.HeadUIParam>(GameEventDef.EVNET_APPEND_HEAD_UI, OnEventAppendHeadUI);
        EventDispatcher.AddEventListener<int>(GameEventDef.EVNET_REMOVE_HEAD_UI, OnEventRemoveHeadUI);
        EventDispatcher.AddEventListener<BattleHeadUILogic.HeadUIParam>(GameEventDef.EVNET_UPDATE_HEAD_UI, OnEventUpdateHeadUI);
        EventDispatcher.AddEventListener<BattleHeadUILogic.HeadUIParam>(GameEventDef.EVNET_APPEND_FLOAT_ART_NUMBER, OnEventAppendFloatArtNumber);
        EventDispatcher.AddEventListener<Vector3>(GameEventDef.EVNET_APPEND_EXPLODE_STAR, OnEventAppendExplodeStar);
        EventDispatcher.AddEventListener<Vector3>(GameEventDef.EVNET_PLAY_CRACK_SCREEN, OnEventPlayCrackScreen);
        EventDispatcher.AddEventListener<Dictionary<string, Monster.HittingPart>>(GameEventDef.EVNET_APPEND_HITTING_UI, OnEventAppendHittingUI);
        EventDispatcher.AddEventListener<Dictionary<string, Monster.HittingPart>>(GameEventDef.EVNET_REMOVE_HITTING_UI, OnEventRemoveHittingUI);
        EventDispatcher.AddEventListener<Dictionary<string, Monster.HittingPart>>(GameEventDef.EVNET_UPDATE_HITTING_UI, OnEventUpdateHittingUI);
        EventDispatcher.AddEventListener<GameObject>(GameEventDef.EVNET_DAMAGE_HITTING_UI, OnEventDamageHittingUI);
        EventDispatcher.AddEventListener<BattleHeadUILogic.HeadUIParam>(GameEventDef.EVNET_APPEND_WEAPON_UI, OnEventAppendWeaponUI);
        EventDispatcher.AddEventListener<int>(GameEventDef.EVNET_REMOVE_WEAPON_UI, OnEventRemoveWeaponUI);
        EventDispatcher.AddEventListener<BattleHeadUILogic.HeadUIParam>(GameEventDef.EVNET_UPDATE_WEAPON_UI, OnEventUpdateWeaponUI);
        EventDispatcher.AddEventListener<BattleHeadUILogic.HeadUIParam, int>(GameEventDef.EVNET_APPEND_RESCUE_HINT_UI, OnEventAppendRescueHintUI);
        EventDispatcher.AddEventListener<int>(GameEventDef.EVNET_REMOVE_RESCUE_HINT_UI, OnEventRemoveRescueHintUI);
        EventDispatcher.AddEventListener<BattleHeadUILogic.HeadUIParam,int>(GameEventDef.EVNET_UPDATE_RESCUE_HINT_UI, OnEventUpdateRescueHintUI);
    }

    /// <summary>
    /// 添加UI监听
    /// </summary>
    protected virtual void addUIEventListener()
    {

    }

    /// <summary>
    /// 移除逻辑事件监听
    /// </summary>
    protected virtual void removeEvent()
    {
        EventDispatcher.RemoveEventListener<BattleHeadUILogic.HeadUIParam>(GameEventDef.EVNET_APPEND_HEAD_UI, OnEventAppendHeadUI);
        EventDispatcher.RemoveEventListener<int>(GameEventDef.EVNET_REMOVE_HEAD_UI, OnEventRemoveHeadUI);
        EventDispatcher.RemoveEventListener<BattleHeadUILogic.HeadUIParam>(GameEventDef.EVNET_UPDATE_HEAD_UI, OnEventUpdateHeadUI);
        EventDispatcher.RemoveEventListener<BattleHeadUILogic.HeadUIParam>(GameEventDef.EVNET_APPEND_FLOAT_ART_NUMBER, OnEventAppendFloatArtNumber);
        EventDispatcher.RemoveEventListener<Vector3>(GameEventDef.EVNET_APPEND_EXPLODE_STAR, OnEventAppendExplodeStar);
        EventDispatcher.RemoveEventListener<Vector3>(GameEventDef.EVNET_PLAY_CRACK_SCREEN, OnEventPlayCrackScreen);
        EventDispatcher.RemoveEventListener<Dictionary<string, Monster.HittingPart>>(GameEventDef.EVNET_APPEND_HITTING_UI, OnEventAppendHittingUI);
        EventDispatcher.RemoveEventListener<Dictionary<string, Monster.HittingPart>>(GameEventDef.EVNET_REMOVE_HITTING_UI, OnEventRemoveHittingUI);
        EventDispatcher.RemoveEventListener<Dictionary<string, Monster.HittingPart>>(GameEventDef.EVNET_UPDATE_HITTING_UI, OnEventUpdateHittingUI);
        EventDispatcher.RemoveEventListener<GameObject>(GameEventDef.EVNET_DAMAGE_HITTING_UI, OnEventDamageHittingUI);
        EventDispatcher.RemoveEventListener<BattleHeadUILogic.HeadUIParam>(GameEventDef.EVNET_APPEND_WEAPON_UI, OnEventAppendWeaponUI);
        EventDispatcher.RemoveEventListener<int>(GameEventDef.EVNET_REMOVE_WEAPON_UI, OnEventRemoveWeaponUI);
        EventDispatcher.RemoveEventListener<BattleHeadUILogic.HeadUIParam>(GameEventDef.EVNET_UPDATE_WEAPON_UI, OnEventUpdateWeaponUI);
        EventDispatcher.RemoveEventListener<BattleHeadUILogic.HeadUIParam, int>(GameEventDef.EVNET_APPEND_RESCUE_HINT_UI, OnEventAppendRescueHintUI);
        EventDispatcher.RemoveEventListener<int>(GameEventDef.EVNET_REMOVE_RESCUE_HINT_UI, OnEventRemoveRescueHintUI);
        EventDispatcher.RemoveEventListener<BattleHeadUILogic.HeadUIParam,int>(GameEventDef.EVNET_UPDATE_RESCUE_HINT_UI, OnEventUpdateRescueHintUI);
    }

    /// <summary>
    /// 移除UI监听
    /// </summary>
    protected virtual void removeUIEventListener()
    {

    }

}

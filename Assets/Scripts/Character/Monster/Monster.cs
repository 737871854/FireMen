using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using Need.Mx;

public class Monster : MonoBehaviour
{
    public enum MoveType
    {
        Unknown,
        Walk = 1,
        Fly  = 2,

    }

    public enum StateType
    {
        Unknown = 0,
        Idle = 1,
        Move = 2,
        Attack = 3,
        Die = 4,
        Rescued = 5,
        Emotion = 6,
        Summon  = 7,
        FireBall = 8,
        Break = 9,
        Back = 10,
    }

    public enum MoveTargetType
    {
        Unknown = 0,
        Random = 1,
        XAxis = 2,
        Target = 3,
        Screen = 4,
    }

    public enum MonsterType
    {
        Unknown = 0,
        Normal = 1,
        Senior = 2,
        Boss = 3,
        NPC = 4,
        Weapon = 5,
    }

    public enum RescueType
    {
        Unknown = 0,
        RescueWater = 1,
        RescueStairs = 2,
    }

    public enum DamageType
    {
        Unknown = 0,
        Water   = 1,
        Buffer  = 2,
    }

    public enum ColorType
    {
        Normal = 0,
        Change = 1,
    }

    public struct MoveDistance
    {
        public float min;
        public float max;
    }

    public class HittingPart
    {
        public GameObject goBind;
        public int maxHp;
        public int curHp;
    }

    /// <summary>
    /// 基本属性
    /// </summary>
    protected MoveType moveType;    // 移动类型
    public float height;            // 高度
    public Renderer[] renderer;
    protected Animator animator;    // 动画系统
    protected NavMeshAgent mvAgent; // 导航系统
    protected GameObject goHead;    // 头部对象
    protected StateType state;      // 怪物状态
    protected MonsterPO dataPO;     // 怪物数据
    protected int id;               // 怪物ID
    protected bool destroy;
    protected ColorType colorType;  // 怪物颜色状态
    /// <summary>
    /// 特殊属性
    /// </summary>
    protected Vector3[] fixedTargetPosition;        // 固定移动目标位置
    protected MoveDistance canMoveDistance;         // 可移动距离

    /// <summary>
    /// runtime属性
    /// </summary>
    protected int hp;
    protected int mp;
    protected float disappearTimer;                 // 消失计时器
    protected bool  disappearAlpha;
    protected float idleTimer;
    protected float skillTimer;
    protected float tipsCount;
    protected int fixedTargetIndex;
    protected Vector3 movePosition;         
    private Quaternion oldRotation = Quaternion.identity; // 对当前对象的旋转状态进行设定起始
    private Quaternion toRotation = Quaternion.identity;  // 对当前对象的旋转状态进行设定起始
    private float totalRotationTime;                      // 根据速度算出旋转需要的时间
    private float nowRotateTime;                          // 当前旋转时间情况转圈
    private float randRotateTime;
    private Vector3 oldPosition;                          // 对当前对象的平移状态进行设定(之前)
    private Vector3 toPosition;                           // 对当前对象的平移状态进行设定(目标)
    private float totalTranslateTime;                     // 根据速度算出平移需要的时间
    private float nowTranslateTime;                       // 当前平移时间
    private int score;
    private bool lockRescued;                             // 这个值主要针对叫车救援锁定功能
    protected bool damageWater;
    protected Player lastPlayer;
    public float MoveSpeed      { get { return dataPO.Speed * 0.01f; } }
    public float HP             { get { return hp; } }
    public int   CarId          { get { return dataPO.FireCarId; } }
    public bool  Disappearing   { get { return disappearAlpha; } }
    public bool  LockRescued    { get { return lockRescued; } set { lockRescued = value; } }
    public Vector3 Position     { get { return transform.position; } }
    public MonsterType Type     { get { return (MonsterType)dataPO.MonsterType; } }
    
    /// <summary>
    /// Hitting Act
    /// </summary>
    protected Dictionary<string, HittingPart> hittingPartDic;
    protected float bulletTime;
    protected bool invincible;
    public bool Invincible      { get { return invincible; } }

    /// <summary>
    /// 内部组件
    /// </summary>
    private SkillComponent skillComponent;
    private BufferComponent bufferComponent;
    private List<GameObject> effectObjects;


    private Tweener twHit;
    private float twHitInterval;

    public void Init(int id, MonsterPO dataPO)
    {
        // 基础数据
        this.twHit = null;
        this.twHitInterval = 0.0f;
        this.moveType = (MoveType)dataPO.MoveType;
        this.destroy = false;
        this.fixedTargetIndex = 0;
        this.idleTimer = dataPO.MoveCool;
        this.skillTimer = dataPO.AttackCool;
        this.disappearTimer = (float)dataPO.DisappearTime / 1000.0f;
        this.disappearAlpha = false ;
        this.id = id;
        this.state = StateType.Idle;
        this.dataPO = dataPO;
        this.hp = dataPO.Hp;
        this.mp = 0;
        this.score = 0;
        this.goHead = transform.Find("Head").gameObject;
        this.lockRescued = false;
        this.damageWater = false;
        this.tipsCount = 0;
        this.randRotateTime = 3.0f;
        this.lastPlayer = null;
        // 固定点移动配置
        if (dataPO.FixPoint.Length % 3 == 0)
        {
            fixedTargetPosition = new Vector3[dataPO.FixPoint.Length / 3];
            for (int index = 0, count = 0; index < dataPO.FixPoint.Length; ++count)
            {
                Vector3 pos = new Vector3(dataPO.FixPoint[index++], dataPO.FixPoint[index++], dataPO.FixPoint[index++]);
                fixedTargetPosition[count] = pos;
            }
        }

        // 随机移动距离
        if (dataPO.MoveDistance.Length % 2 == 0)
        {
            canMoveDistance = new MoveDistance();
            canMoveDistance.min = dataPO.MoveDistance[0];
            canMoveDistance.max = dataPO.MoveDistance[1];
        }

        //// 动画系统
        animator = GetComponent<Animator>();
        //// 移动系统
        //if (moveType == MoveType.Walk)
        //{
        //    mvAgent = GetComponent<NavMeshAgent>();
        //    mvAgent.speed = MoveSpeed;
        //    mvAgent.SetDestination(transform.position);
        //}
        //else
        //{
        //    GetComponent<NavMeshAgent>().enabled = false;
        //}

        // 技能与Buffer系统
        skillComponent  = gameObject.AddComponent<SkillComponent>();
        bufferComponent = gameObject.AddComponent<BufferComponent>();
        List<int> skillList = new List<int>();
        skillList.Add(dataPO.SkillID1);
        skillList.Add(dataPO.SkillID2);
        skillList.Add(dataPO.SkillID3);
        skillComponent.Init(skillList);
        bufferComponent.Init();

        // 特效数据
        AddBornEffect();
       
        // 添加头顶UI
        AppendHeadUI();

        // 武器图标
        AppendWeaponUI();

        // 救援图标
        AppendRescueHintUI();

        // 暂时特殊处理(BOSS 朝向镜头)
        //if ((MonsterType)dataPO.MonsterType == MonsterType.Boss)
        //{
            FaceTo(Camera.main.transform.position, 0.0f);
        //}

        // Hitting Act
        hittingPartDic  = new Dictionary<string, HittingPart>();
        bulletTime      = 0.0f;
        invincible      = false;

        // 刷新音效
        if (dataPO.BirthSound.Length == 2)
        {
            string sound = dataPO.BirthSound[0];
            int percent  = dataPO.BirthSound[1].ToInt();
            int seed = Random.Range(1, 10000);
            if (seed > percent)
            {
                return;
            }
          
            Main.SoundController.FireEvent(sound);
        }
    }

    // Use this for initialization
    void Start()
    {
        
    }

    void OnDestroy()
    {
        RemoveBornEffect(0);

        if (hittingPartDic.Count > 0)
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_REMOVE_HITTING_UI, hittingPartDic);
        }
    }

    public int ObtainScore()
    {
        int value = score;
        score = 0;
        return value;
    }

    public void OnSprayWater(int value,Player player)
    {
        if (state == StateType.Die || state == StateType.Rescued)
        {
            return;
        }

        if (Disappearing)
        {
            return;
        }

        if (dataPO.FriendType == (int)RescueType.RescueStairs)
        {
            return;
        }

        if (invincible)
        {
            return;
        }

        if ((MonsterType)dataPO.MonsterType != MonsterType.NPC && (MonsterType)dataPO.MonsterType != MonsterType.Weapon && !IsDie())
        {
            hp -= value;
            if (hp < 0)
            {
                hp = 0;               
            }           
        }
        else if(!IsSafety())
        {
            mp += value;
        }

        if ((MonsterType)dataPO.MonsterType == MonsterType.Boss)
        {
            if (hp == 0)
            {
                player.Pass = true;
            }
        }

        // 更新头顶浮动文字
        OnUpdateHeadUI(value);
        OnUpdateWeaponUI(value);

        // 命中特效
        if (dataPO.HitEffect.Length % 3 == 0)
        {
            for (int index = 0; index < dataPO.HitEffect.Length; )
            {
                string effectName = dataPO.HitEffect[index++];
                string boneName = dataPO.HitEffect[index++];
                float lifeTime = dataPO.HitEffect[index++].ToFloat();
                AddEffect(effectName, boneName, lifeTime, 0, false);
            }
        }

        // 命中变色
        if (dataPO.HitColor.Length % 3 == 0)
        {
            if (twHitInterval <= 0.02f)
            {
                return;
            }
            twHitInterval = 0.0f;
            //if (twHit != null)
            //{
            //    DOTween.Kill(twHit);
            //    for (int index = 0; index < renderer.Length; ++index)
            //    {
            //        Color oldColor = new Color(1, 1, 1, 1);
            //        renderer[index].material.SetColor("_Color", oldColor);
            //    }
            //}
            //Color hitColor = new Color(dataPO.HitColor[0], dataPO.HitColor[1], dataPO.HitColor[2], 1);
            //for (int index = 0; index < renderer.Length; ++index)
            //{
            //    twHit = renderer[index].material.DOColor(hitColor, "_Color", 0.05f);
            //    twHit.SetLoops(2, LoopType.Yoyo);
            //    twHit.SetAutoKill(false);
            //    twHit.Play();
            //}
            if(colorType == ColorType.Normal)
            {
                colorType = ColorType.Change;
                Color hitColor = new Color(dataPO.HitColor[0], dataPO.HitColor[1], dataPO.HitColor[2], 1);
                for (int index = 0; index < renderer.Length; ++index)
                {
                    twHit = renderer[index].material.DOColor(hitColor, "_Color", 0.2f);
                    twHit.Play();
                    twHit.OnComplete(OnHitColorEnd);
                }
            }
           
        }

        lastPlayer = player;

        if ((MonsterType)dataPO.MonsterType != MonsterType.NPC && IsDie())
        {
            ChangeDie();
        }
        else if ((MonsterType)dataPO.MonsterType == MonsterType.NPC && IsSafety())
        {
            ChangeRescue(player);
        }
        else if ((MonsterType)dataPO.MonsterType == MonsterType.Weapon && IsSafety())
        {
            ChangePick(player);
        }
    }

    private void OnHitColorEnd()
    {
        for (int index = 0; index < renderer.Length; ++index)
        {
            Color oldColor = new Color(1, 1, 1, 1);
            renderer[index].material.SetColor("_Color", oldColor);
        }   
        colorType = ColorType.Normal;
    }

    public void OnSprayWaterHitting(GameObject go, int value)
    {
        if (!invincible)
        {
            return;
        }

        if (go != null)
        {
            if (hittingPartDic.ContainsKey(go.name))
            {
                HittingPart element = hittingPartDic[go.name];
                // 暂时先这样处理
                if (element.curHp > 0)
                {
                    Main.SoundController.PlayHitCircleSound();
                    EventDispatcher.TriggerEvent(GameEventDef.EVNET_DAMAGE_HITTING_UI, element.goBind);
                }
                element.curHp -= value;
                if (element.curHp <= 0)
                {
                    element.curHp = 0;
                }
                hittingPartDic[go.name] = element;
            }
            else
            {
                return;
            }
        }

        int hited = 0;
        foreach (KeyValuePair<string, HittingPart> element in hittingPartDic) 
        {
            if (go == null)
            {
                element.Value.curHp -= value;
                if (element.Value.curHp <= 0)
                {
                    element.Value.curHp = 0;
                }
                hittingPartDic[element.Key] = element.Value;
            }

            if (element.Value.curHp == 0)
            {
                hited++;
            }
        }

        if (hited == hittingPartDic.Count)
        {
            if (!IsDie())
            {
                ChangeBreak();
            }
            OnSprayWater(GameConfig.GAME_CONFIG_WATER_DAMAGE_2,null);
        }

    }

    public void OnAttrChange(AttrType type, int value)
    {
        if (state == StateType.Die || state == StateType.Rescued)
        {
            return;
        }

        if (Disappearing)
        {
            return;
        }

        if (type == AttrType.ATTR_HP) 
        {
            hp += value;
            if (hp < 0)
            {
                hp = 0;
            }
        }

        if (IsDie())
        {
            ChangeDie(DamageType.Buffer);
        }
    }

    public bool IsDie()
    {
        if (hp <= 0)
            return true;
        return false;
    }

    public bool IsSafety()
    {
        if (mp >= dataPO.SaveDegree)
            return true;
        return false;
    }

    public bool IsDestroy()
    {
        return destroy;
    }

    public void SetBodyActive(bool show)
    {
        float value = 0.0f;
        if (!show)
        {
            value = 1.0f;
        }

        for (int index = 0; index < renderer.Length; ++index)
        {
            renderer[index].material.SetFloat("_Cutoff", value);
        }
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        UpdateState();
        bufferComponent.UpdateBuffer();
        skillComponent.UpdateSkill();
    }

    #region 状态相关处理
    protected void UpdateState()
    {
        twHitInterval += Time.deltaTime;
        switch (state)
        {
            case StateType.Idle:
                {
                    OnIdle();
                    break;
                }
            case StateType.Move:
                {
                    OnMove();
                    break;
                }
            case StateType.Attack:
                {
                    OnAttack();
                    break;
                }
            case StateType.Die:
                {
                    OnDie();
                    break;
                }
            case StateType.Rescued:
                {
                    OnRescued();
                    break;
                }
            case StateType.Emotion:
                {
                    OnEmotion();
                    break;
                }
            case StateType.Break:
                {
                    OnBreak();
                    break;
                }
        }

        OnDisappear();
        OnRotate();
        OnTranslate();
        OnUpdateHeadUI(0);
        OnUpdateHitting();
        OnUpdateWeaponUI(0);
        OnUpdateRescueHintUI();
    }

    public void ChangeAttack(string animationName)
    {
        switch (animationName)
        {
            case "Attack":
                {
                    animator.SetInteger("State", (int)StateType.Attack);
                    break;
                }

            case "Summon":
                {
                    animator.SetInteger("State", (int)StateType.Summon);
                    break;
                }

            case "FireBall":
                {
                    animator.SetInteger("State", (int)StateType.FireBall);
                    break;
                }
            default:
                {
                    return;
                }
        }

        // 暂时所有攻击动作都是进入Attack状态来处理
        // 主要是判断动作是否完成
        state = StateType.Attack;
    }

    public void ChangeBreak()
    {
        state = StateType.Break;
        Main.GameMode.TimeScale(1.0f);
        invincible = false;
        animator.SetInteger("State", (int)state);
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_REMOVE_HITTING_UI, hittingPartDic);
        hittingPartDic.Clear();
        skillComponent.Interrupt();
    }

    public void ChangeRescue(Player player)
    {
        lastPlayer = player;
        state = StateType.Rescued;
        animator.SetInteger("State", (int)state);
        idleTimer = 0;
        score = dataPO.MonsterValue;
        FaceTo(Camera.main.transform.position, 0.0f);
        StopMove();
        RemoveHeadUI();
        RemoveRescueHintUI();
        PlayFloatUI(dataPO.MonsterValue, lastPlayer.Index);
        RemoveBornEffect(2);
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_APPEND_EXPLODE_STAR, FindParentBone("Center").position);
    }

    public void ChangePick(Player player)
    {
        state = StateType.Rescued;
        destroy = true;
        StopMove();
        RemoveWeaponUI();
        Main.GameMode.ActivateWeaponSkill(player);
    }

    protected void ChangeDie(DamageType type = DamageType.Water)
    {
        if (LockRescued)
        {
            return;
        }

        state = StateType.Die;
        animator.SetInteger("State", (int)state);
        idleTimer = 0;

        if ((MonsterType)dataPO.MonsterType != MonsterType.NPC)
        {
            score = dataPO.MonsterValue;
        }

        StopMove();
        RemoveHeadUI();
        RemoveRescueHintUI();
        if ((MonsterType)dataPO.MonsterType == MonsterType.Boss)
        {
            Main.GameMode.TimeScale(1.0f);
        }
        invincible = false;
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_REMOVE_HITTING_UI, hittingPartDic);
        hittingPartDic.Clear();
        skillComponent.Interrupt();

        if (type == DamageType.Water)
        {
            damageWater = true;
            if ((MonsterType)dataPO.MonsterType != MonsterType.NPC && lastPlayer != null)
            {
                PlayFloatUI(dataPO.MonsterValue, lastPlayer.Index);
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_APPEND_EXPLODE_STAR, FindParentBone("Center").position);
            }
        }

        if (dataPO.DieEffect.Length % 4 == 0)
        {
            for (int index = 0; index < dataPO.DieEffect.Length; )
            {
                string effectName = dataPO.DieEffect[index++];
                string boneName = dataPO.DieEffect[index++];
                float lifeTime = dataPO.DieEffect[index++].ToFloat();
                float delayTime = dataPO.DieEffect[index++].ToFloat();
                AddEffect(effectName, boneName, lifeTime);
              
            }
        }

		// 死亡播放音效
		if (dataPO.DieSound.Length >= 2)
		{
			int percent = dataPO.DieSound[0].ToInt();
			int index   = Random.Range(1, dataPO.DieSound.Length);
			if (index < dataPO.DieSound.Length)
			{
				string sound = dataPO.DieSound[index];
				
				int seed = Random.Range(1, 10000);
				if (seed > percent)
				{
					return;
				}
             
				Main.SoundController.FireEvent(sound);
			}
		}

        RemoveBornEffect(1);
    }

    protected void ChangeIdle(bool clearTimer = true)
    {
        state = StateType.Idle;
        animator.SetInteger("State", (int)state);
        if (clearTimer)
        {
            idleTimer  = 0;
            skillTimer = 0;
        }
    }

    protected void ChangeMove()
    {
        // 开始移动
        movePosition = new Vector3();
        if (GetMoveTargetPosition(ref movePosition) == false)
        {
            return;
        }
        MoveTo(movePosition);
    }

    protected void ChangeEmotion()
    {
        state = StateType.Emotion;
        animator.SetInteger("State", (int)state);
        idleTimer  = 0;
    }

    protected void OnIdle()
    {

        if ((MonsterType)dataPO.MonsterType != MonsterType.Weapon)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (!info.IsName("Idle"))
            {
                return;
            }

            if (this.skillTimer >= (float)dataPO.AttackCool / 1000.0f)
            {
                this.skillTimer = 0;
                if (skillComponent.Use(dataPO.SkillID1))
                {
                    return;
                }
                if (skillComponent.Use(dataPO.SkillID2))
                {
                    return;
                }
                if (skillComponent.Use(dataPO.SkillID3))
                {
                    return;
                }
            }
            else
            {
                this.skillTimer += Time.deltaTime;
            }
        }

        if (this.idleTimer >= (float)dataPO.MoveCool / 1000.0f)
        {
            //if ((MonsterType)dataPO.MonsterType == MonsterType.Boss)
            //{
            //    int play = Random.Range(1, 10000);
            //    if (play <= 2)
            //    {
            //        Main.SoundController.PlayAttackBossBodySound();
            //        ++tipsCount;
            //    }
            //    else if (play > 2 && tipsCount == 0)
            //    {
            //        Main.SoundController.PlayAttackBossBodySound();
            //        ++tipsCount;
            //    }
            //}

            int seed = Random.Range(1, 100);
            if (seed <= dataPO.RandomMove)
            {
                ChangeMove();
                return;
            }

            seed = Random.Range(1, 100);
            if (seed <= dataPO.StandAction)
            {
                ChangeEmotion();
            }
        }
        else 
        {
            this.idleTimer += Time.deltaTime;
        }
    }

    protected void OnMove()
    {
        if (Vector3.Distance(gameObject.transform.position, movePosition) <= 0.01f)
        {
            ChangeIdle();
        }

        // 临时处理
        if (Main.GameMode.SceneName() == SceneType.Scene1)
        {
            if (randRotateTime <= 0.0f)
            {
                int seed = Random.Range(1, 10000);
                if (seed > 2)
                {
                    FaceTo(movePosition, 1.0f);
                }
                else
                {
                    FaceTo(Camera.main.transform.position, 1.0f);
                    randRotateTime = 3.0f;
                }
            }
            else
            {
                randRotateTime -= Time.deltaTime;
            }

        }
        else
        {
            FaceTo(movePosition, 0.0f);
        }
    }

    protected void OnAttack()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f)
        {
            if(info.IsName("Attack") || info.IsName("Summon") || info.IsName("FireBall") )
            {
                ChangeIdle(true);
            }
        }
    }

    protected void OnDie()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (Type != MonsterType.Boss)
        {
            animator.speed = 2.0f;
        }
        if (info.IsName("Die") && info.normalizedTime >= 1.0f)
        {
            animator.speed = 1.0f;
            if (this.idleTimer >= (float)dataPO.CorpseRemainTime / 1000.0f && disappearAlpha == false)
            {
                disappearAlpha = true;
                if (damageWater)
                {
                    EventDispatcher.TriggerEvent(GameEventDef.EVNET_MONSTER_DEATH, dataPO.MonsterType, dataPO.Id);
                }
                for (int index = 0; index < renderer.Length; ++index)
                {
                    Tweener tw = renderer[index].material.DOFloat(1.0f, "_Cutoff", 0.5f);
                    tw.OnComplete(OnDisappearEnd);
                }

                
            }
            else
            {
                this.idleTimer += Time.deltaTime;
                
            }
        }
    }

    protected void OnRescued()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Rescue") && info.normalizedTime >= 1.0f)
        {
            if (this.idleTimer >= (float)dataPO.CorpseRemainTime / 1000.0f && disappearAlpha == false)
            {
                disappearAlpha = true;
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_MONSTER_DEATH, dataPO.MonsterType, dataPO.Id);
                for (int index = 0; index < renderer.Length; ++index)
                {
                    Tweener tw = renderer[index].material.DOFloat(1.0f, "_Cutoff", 0.5f);
                    tw.OnComplete(OnDisappearEnd);
                }
            }
            else
            {
                this.idleTimer += Time.deltaTime;

            }
        }
    }

    protected void OnEmotion()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Emotion") && info.normalizedTime >= 1.0f)
        {
            ChangeIdle(false);
        }
        this.idleTimer += Time.deltaTime;
    }

    protected void OnBreak()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Break") && info.normalizedTime >= 1.0f)
        {
            ChangeIdle(true);
        }
    }

    protected void OnDisappear()
    {
        if (LockRescued)
        {
            return;
        }

        // 判断是否死亡或者安全
        // 如果之一存在，则不再执行自动消失功能，以免冲突
        if ((MonsterType)dataPO.MonsterType != MonsterType.NPC && IsDie())
        {
            return;
        }
        else if ((MonsterType)dataPO.MonsterType == MonsterType.NPC && IsSafety())
        {
            return;
        }

        if (dataPO.DisappearTime == 0)
        {
            return;
        }

        // 够时间进行消失
        if (this.disappearTimer < 0 && disappearAlpha == false)
        {
            for (int index = 0; index < renderer.Length; ++index)
            {
                Tweener tw = renderer[index].material.DOFloat(1.0f, "_Cutoff", 1.0f);
                tw.OnComplete(OnDisappearEnd);
            }
            disappearAlpha = true;

            if ((MonsterType)dataPO.MonsterType == MonsterType.Weapon)
            {
                OnDisappearEnd();
            }
        }

        disappearTimer -= Time.deltaTime;
    }

    protected void OnDisappearEnd()
    {
        this.destroy = true;
        RemoveHeadUI();
        RemoveWeaponUI();
        RemoveRescueHintUI();
        if (invincible)
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_REMOVE_HITTING_UI, hittingPartDic);
        }
    }  

    protected void OnRotate()
    {
        if (totalRotationTime <= 0)
            return;
        nowRotateTime += Time.deltaTime;
        if (nowRotateTime < totalRotationTime)
        {
            gameObject.transform.localRotation = Quaternion.Lerp(oldRotation, toRotation, nowRotateTime / totalRotationTime);
        }
        else
        {
            gameObject.transform.localRotation = toRotation;
            totalRotationTime = 0;
        }
    }

    protected void OnTranslate()
    {
        if (totalTranslateTime <= 0)
            return;
        nowTranslateTime += Time.deltaTime;
        if (nowTranslateTime < totalTranslateTime)
        {
            gameObject.transform.position = Vector3.Lerp(oldPosition, toPosition, nowTranslateTime / totalTranslateTime);
        }
        else
        {
            gameObject.transform.position = toPosition;
            totalRotationTime = 0;
        }
    }

    protected bool GetMoveTargetPosition(ref Vector3 pos)
    {
        MoveTargetType targetType = (MoveTargetType)dataPO.MoveTargetType;
        switch (targetType)
        {
            case MoveTargetType.Target:
                {
                    if (fixedTargetIndex >= fixedTargetPosition.Length)
                    {
                        return false;
                    }
                    pos = fixedTargetPosition[fixedTargetIndex++];
                    return true;
                }

            case MoveTargetType.XAxis:
                {
                    int xValue = Random.Range(-1, 2);
                    if (xValue == 0)
                    {
                        return false;
                    }
                    float distance = Random.Range(canMoveDistance.min, canMoveDistance.max);
                    distance = (float)xValue * distance;
                    //Vector3 newPos = new Vector3(distance,0,0);
                    pos.x = distance;
                    pos.y = 0;
                    pos.z = 0;
                    pos = transform.position + pos;
                    if (!AreaManager.instance.IsPositionInArea(dataPO.MoveArea, pos))
                    {
                        return false;
                    }
                    //pospos = newPos;
                    return true;
                }

            case MoveTargetType.Random:
                {
                    int xValue = Random.Range(-1, 2);
                    int yValue = Random.Range(-1, 2);
                    int zValue = Random.Range(-1, 2);
                    float xDistance = Random.Range(canMoveDistance.min, canMoveDistance.max);
                    float yDistance = Random.Range(canMoveDistance.min, canMoveDistance.max);
                    float zDistance = Random.Range(canMoveDistance.min, canMoveDistance.max);
                    xDistance = (float)xValue * xDistance;
                    yDistance = (float)yValue * yDistance;
                    zDistance = (float)zValue * zDistance;
                    //Vector3 newPos;
                    if (moveType == MoveType.Fly)
                    {
                        //newPos = new Vector3(xDistance, yDistance, zDistance);
                        pos.x = xDistance;
                        pos.y = yDistance;
                        pos.z = zDistance;
                    }
                    else
                    {
                        //newPos = new Vector3(xDistance, 0, zDistance);
                        pos.x = xDistance;
                        pos.y = 0;
                        pos.z = zDistance;
                    }
                    pos = transform.position + pos;
                    if (!AreaManager.instance.IsPositionInArea(dataPO.MoveArea, pos))
                    {
                        return false;
                    }
                    //pos = newPos;
                    return true;
                }
        }

        return false;
    }

    #endregion

    #region 位置方向相关函数

    protected void TurnToDir(Vector3 direction)
    {
        TurnToDir(direction, 0.0f);
    }

    protected void TurnToDir(Vector3 direction, float speed)
    {
        direction.y = 0;

        if (direction == Vector3.zero)
            return;
        //将方向转换为，旋转的角度;
        toRotation = Quaternion.LookRotation(direction);
        
        //如果速度为0,直接将方向设为rotation目标
        if (speed == 0.0f)
        {
            totalRotationTime = 0;
            gameObject.transform.localRotation = toRotation;
            return;
        }
        //计算出需要转动的角度degree
        float angle = Quaternion.Angle(gameObject.transform.localRotation, toRotation);
        totalRotationTime = (angle * 3.14f) / (speed * 180);
        nowRotateTime = 0.0f;
        oldRotation = gameObject.transform.localRotation;

        if (mvAgent != null)
        {
            mvAgent.updateRotation = false;
        }
    }

    protected void FaceTo(Vector3 pos, float speed = 10.0f)
    {
        Vector3 direction = pos - gameObject.transform.position;
        TurnToDir(direction, speed);
    }

    protected void LookAt(Vector3 pos)
    {
        //Vector3 direction = pos - gameObject.transform.position;
        gameObject.transform.LookAt(pos);
    }

    public static Vector3 WalkPos(Vector3 pos)
    {
        Vector3 actorPos = pos;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 3.0f, 1))
            actorPos = hit.position;
        return actorPos;
    }

    //public Vector3 TryMoveTo(Vector3 tagPos)
    //{
    //    NavMeshHit hit;
    //    if (NavMesh.Raycast(gameObject.transform.position, tagPos, out hit, 1))
    //    {
    //        return hit.position;
    //    }
    //    return tagPos;
    //}

    protected void  StopMove()
    {
        if (moveType == MoveType.Walk)
        {
            if (mvAgent.hasPath)
            {
                mvAgent.Stop();
                mvAgent.ResetPath();
            }
            return;
        }
        if (moveType == MoveType.Fly)
        {
            totalTranslateTime = 0.0f;
            return;
        }

    }

    protected void MoveTo(Vector3 pos, bool needRot = true)
    {
        float dis = Vector3.Distance(gameObject.transform.position, pos);
        if (0.01f > dis)
        {
            FaceTo(pos, 0);
            return;
        }

        state = StateType.Move;
        if ((MonsterType)dataPO.MonsterType != MonsterType.Weapon)
        { 
            animator.SetInteger("State", (int)state);
        }

        if (moveType == MoveType.Walk)
        {
            WalkTo(pos, needRot);
            return;
        }
        if (moveType == MoveType.Fly)
        {
            FlyTo(pos, needRot);
            return;
        }
    }

    protected void WalkTo(Vector3 pos, bool needRot = true)
    {
        totalRotationTime = 0;
        {
            FaceTo(pos, 0);
            mvAgent.updateRotation = false;
        }
        if (mvAgent.hasPath)
        {
            mvAgent.Stop();
            mvAgent.ResetPath();
        }
        pos = WalkPos(pos);
        movePosition = pos;
        mvAgent.speed = MoveSpeed;
        mvAgent.SetDestination(pos);
    }

    protected void FlyTo(Vector3 pos, bool needRot = true)
    {
        float dis = Vector3.Distance(gameObject.transform.position, pos);
        totalRotationTime = 0;
        toPosition = pos;
        totalTranslateTime = dis / MoveSpeed;
        nowTranslateTime = 0.0f;
        oldPosition = gameObject.transform.position;
    }
    #endregion

    #region 头顶UI处理函数

    protected void AppendFloatArtNumber()
    {

    }

    protected void AppendHeadUI()
    {
        if (dataPO.DisplayBlood == 1)
        {
            BattleHeadUILogic.HeadUIParam param = BattleHeadUILogic.HeadUIParam.New();
            param.goHead = goHead;
            param.id = id;
            param.type = (Monster.MonsterType)dataPO.MonsterType;
            if ((MonsterType)dataPO.MonsterType != MonsterType.NPC)
            {
                param.max = dataPO.Hp;
                param.min = hp;
            }
            else 
            {
                param.max = dataPO.SaveDegree;
                param.min = mp;
            }
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_APPEND_HEAD_UI, param);
        }
    }

    protected void OnUpdateHeadUI(int damage)
    {
        if (dataPO.DisplayBlood == 1)
        {
            BattleHeadUILogic.HeadUIParam param = BattleHeadUILogic.HeadUIParam.New();
            param.goHead = goHead;
            param.id = id;
            param.type = (Monster.MonsterType)dataPO.MonsterType;
            if ((MonsterType)dataPO.MonsterType != MonsterType.NPC)
            {
                param.max = dataPO.Hp;
                param.min = hp;
            }
            else
            {
                param.max = dataPO.SaveDegree;
                param.min = mp;
            }
            param.damage = damage;
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_UPDATE_HEAD_UI, param);
        }

    }

    protected void RemoveHeadUI()
    {
        if (dataPO.DisplayBlood == 1)
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_REMOVE_HEAD_UI, id);
        }
    }

    protected void PlayFloatUI(int value, int index)
    {
        if (value <= 0)
        {
            return;
        }

        BattleHeadUILogic.HeadUIParam param = BattleHeadUILogic.HeadUIParam.New();
        param.goHead = goHead;
        param.id = id;
        param.type = (Monster.MonsterType)dataPO.MonsterType;
        param.damage = value;
        param.min = index;
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_APPEND_FLOAT_ART_NUMBER, param);
    }


    protected void AppendWeaponUI()
    {
        if ((MonsterType)dataPO.MonsterType != MonsterType.Weapon)
        {
            return;
        }

        BattleHeadUILogic.HeadUIParam param = BattleHeadUILogic.HeadUIParam.New();
        param.goHead = goHead;
        param.id     = id;
        param.type   = (Monster.MonsterType)dataPO.MonsterType;
        param.damage = 0;
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_APPEND_WEAPON_UI, param);
    }

    protected void OnUpdateWeaponUI(int damage)
    {
        if ((MonsterType)dataPO.MonsterType != MonsterType.Weapon)
        {
            return;
        }

        BattleHeadUILogic.HeadUIParam param = BattleHeadUILogic.HeadUIParam.New();
        param.goHead = goHead;
        param.id = id;
        param.type = (Monster.MonsterType)dataPO.MonsterType;
        param.damage = damage;
        param.max = dataPO.SaveDegree;
        param.min = mp;
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_UPDATE_WEAPON_UI, param);
    }

    protected void RemoveWeaponUI()
    {
        if ((MonsterType)dataPO.MonsterType != MonsterType.Weapon)
        {
            return;
        }

        EventDispatcher.TriggerEvent(GameEventDef.EVNET_REMOVE_WEAPON_UI, id);
    }


    protected void AppendRescueHintUI()
    {
        if ((MonsterType)dataPO.MonsterType == MonsterType.NPC && dataPO.DisplayBar == 1)
        {
            BattleHeadUILogic.HeadUIParam param = BattleHeadUILogic.HeadUIParam.New();
            param.goHead = goHead;
            param.id     = id;
            param.type   = (Monster.MonsterType)dataPO.MonsterType;
            param.damage = 0;
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_APPEND_RESCUE_HINT_UI, param, CarId);
        }
    }

    protected void OnUpdateRescueHintUI()
    {
        if ((MonsterType)dataPO.MonsterType == MonsterType.NPC && dataPO.DisplayBar == 1 && !IsSafety())
        {
            BattleHeadUILogic.HeadUIParam param = BattleHeadUILogic.HeadUIParam.New();
            param.goHead = goHead;
            param.id = id;
            param.type = (Monster.MonsterType)dataPO.MonsterType;
            param.damage = 0;
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_UPDATE_RESCUE_HINT_UI, param, CarId);
        }
    }

    protected void RemoveRescueHintUI()
    {
        if ((MonsterType)dataPO.MonsterType == MonsterType.NPC && dataPO.DisplayBar == 1)
        {
            EventDispatcher.TriggerEvent(GameEventDef.EVNET_REMOVE_RESCUE_HINT_UI, id);
        }
    }

    #endregion

    #region Skill/Buffer相关

    public void AddBuffer(int effectId)
    {
        bufferComponent.AddBuffer(effectId);
    }

    #endregion


    #region 特效相关
    public Transform FindParentBone(string childName)
    {
        if (childName == "None")
        {
            return null;
        }
        else if (childName == "Base")
        {
            return gameObject.transform;
        }
        Transform fatherTranform = null;
        foreach (Transform tran in gameObject.transform.GetComponentsInChildren<Transform>())
        {
            if (tran.name == childName)
            {
                fatherTranform = tran;
                return fatherTranform;
            }
        }
        return null;
    }

    public GameObject FindParentObject(string childName)
    {
        if (childName == "None")
        {
            return null;
        }
        else if (childName == "Base")
        {
            return gameObject;
        }
        Transform father = null;
        foreach (Transform go in gameObject.transform.GetComponentsInChildren<Transform>())
        {
            if (go.name == childName)
            {
                father = go;
                return father.gameObject;
            }
        }
        return null;
    }

    public void AddEffect(string fxName, string boneName, float lifeTime, float delayTime = 0, bool attachBone = true)
    {
        if (delayTime > 0)
        {
            StartCoroutine(OnDelayAddEffect(fxName, boneName, lifeTime, delayTime));
            return;
        }

        Transform parent  = FindParentBone(boneName);
        GameObject effect = null;
        Vector3 effectPos;
        if (parent == null)
        {
            effectPos = gameObject.transform.position;
        }
        else
        {
            effectPos = parent.transform.position;
        }


        if (parent != null && lifeTime == SkillEffectManager.NO_TIME)
        {
            effect = SkillEffectManager.instance.CreateEffect(fxName, effectPos);
            if (effect != null)
            {
                effectObjects.Add(effect);
            }
        }
        else
        {
            effect = SkillEffectManager.instance.CastEffect(fxName, effectPos, lifeTime);
        }

        if (effect != null)
        {
            if (parent == null)
            {
                //effect.transform.localPosition = bp.LocalPosition;
                //effect.transform.localScale = bp.LocalScale;
                //effect.transform.localEulerAngles = bp.LocalRotEular;
                effect.transform.parent = null;
            }
            else
            {
                if (attachBone)
                {
                    effect.transform.parent = parent;
                }
                //effect.transform.localPosition = bp.LocalPosition;
                //effect.transform.localScale = bp.LocalScale;
                //effect.transform.localEulerAngles = bp.LocalRotEular;
            }
        }
    }

    IEnumerator OnDelayAddEffect(string fxName, string boneName, float lifeTime, float delayTime)
    {
        yield return new WaitForSeconds(delayTime / 1000.0f);
        AddEffect(fxName, boneName, lifeTime);
    }

    void AddBornEffect()
    {
        effectObjects = new List<GameObject>();
        if (dataPO.Effect.Length % 4 == 0)
        {
            for (int index = 0; index < dataPO.Effect.Length;)
            {
                string effectName = dataPO.Effect[index++];
                string boneName   = dataPO.Effect[index++];
                float lifeTime    = dataPO.Effect[index++].ToFloat();
                int delete        = dataPO.Effect[index++].ToInt();
                AddEffect(effectName, boneName, lifeTime);
            }
        }
    }

    void RemoveBornEffect(int type)
    {
        if (type == 0)
        {
            for (int index = 0; index < effectObjects.Count; ++index)
            {
                if (effectObjects[index] != null && SkillEffectManager.instance != null)
                {
                    SkillEffectManager.instance.DestroyEffect(effectObjects[index]);
                }
            }
        }
        else
        {
            if (dataPO.Effect.Length % 4 == 0)
            {
                for (int index = 0; index < dataPO.Effect.Length; )
                {
                    string effectName = dataPO.Effect[index++];
                    string boneName = dataPO.Effect[index++];
                    float lifeTime = dataPO.Effect[index++].ToFloat();
                    int delete = dataPO.Effect[index++].ToInt();
                    if (delete == type)
                    {
                        for (int count = 0; count < effectObjects.Count; ++count)
                        {
                            if (effectName == effectObjects[count].name)
                            {
                                effectObjects[count].SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region ACT部位相关
    public void ChangeHitting(float timeScale, float timeDelay, float duration, int perHp, List<string> partList)
    {
        if (invincible)
        {
            return;
        }

        hittingPartDic.Clear();
        for(int index = 0; index < partList.Count; ++index)
        {
            GameObject go = FindParentObject(partList[index]);
            if (go == null)
            {
                continue;
            }

            HittingPart part = new HittingPart();
            part.goBind = go;
            part.maxHp  = perHp;
            part.curHp  = perHp;
            hittingPartDic.Add(partList[index], part);

        }

        if (hittingPartDic.Count == 0)
        {
            return;
        }

        bulletTime = duration;
        StartCoroutine(OnCheckCompletePreposeAttackAction(timeScale, timeDelay, duration));
    }

    protected void OnUpdateHitting()
    {
        if (invincible)
        {
            bulletTime -= Main.NonStopTime.deltaTime;
            if (bulletTime <= 0)
            {
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_REMOVE_HITTING_UI, hittingPartDic);
                Main.GameMode.TimeScale(1.0f);
                invincible = false;
            }
            else
            {
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_UPDATE_HITTING_UI, hittingPartDic);
            }
        }
    }

    IEnumerator OnCheckCompletePreposeAttackAction(float timeScale, float timeDelay, float duration)
    {
        yield return new WaitForSeconds(timeDelay/1000.0f);
        bulletTime = duration / 1000.0f;
        invincible = true;
        EventDispatcher.TriggerEvent(GameEventDef.EVNET_APPEND_HITTING_UI, hittingPartDic);
        Main.GameMode.TimeScale(timeScale);
    }

    #endregion

}

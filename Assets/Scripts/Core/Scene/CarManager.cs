using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using Need.Mx;

public class CarManager : SingletonBehaviour<CarManager>
{

    public class Car
    {
        public GameObject           go;
        public Animator             animator;
        public Monster              target;
        public Monster.StateType    state;
    }

    public GameObject[]     goList;
    public List<Car>        carList;

	// Use this for initialization
	void Start () {

        carList = new List<Car>();
        for (int index = 0; index < goList.Length; ++index)
        {
            Car car      = new Car();
            car.go       = goList[index];
            car.animator = car.go.GetComponent<Animator>();
            car.target   = null;
            car.state    = Monster.StateType.Idle;
            carList.Add(car);
        }
	}
	
    // (怪物调用)求救
    public bool Help(int index, Monster user)
    {
        if (index == 0 || index > carList.Count)
        {
            return false;
        }
        int id = index - 1;
        Car car = carList[id];
        if (car.state == Monster.StateType.Attack)
        {
            return false;
        }
        if (car.target != null)
        {
            return false;
        }

        // 记录请求
        car.target = user;
        return true;
    }

    // (怪物调用)挂掉求救
    public void HangUp(int index, Monster user)
    {
        if (index <= 0 || index > carList.Count)
        {
            return;
        }
        int id = index - 1;
        Car car = carList[id];
        if (car.state == Monster.StateType.Attack || car.state == Monster.StateType.Rescued || car.state == Monster.StateType.Back)
        {
            return;
        }
        if (car.target != user)
        {
            return;
        }
        car.go.SetActive(false);
        car.target = null;
    }

     // (怪物调用)消失
    public void Rescued(int index, Monster user)
    {
        if (index <= 0 || index > carList.Count)
        {
            return;
        }
        int id = index - 1;
        Car car = carList[id];
        if (car.state == Monster.StateType.Attack)
        {
            return;
        }
        if (car.target != user)
        {
            return;
        }
        car.state = Monster.StateType.Back;
        car.animator.SetInteger("State", (int)car.state);
        StartCoroutine(OnBackComplete(car));
        
        car.target = null;
    }

    IEnumerator OnBackComplete(Car car)
    {
        while (true)
        {
            AnimatorStateInfo info = car.animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Back") && info.normalizedTime >= 1.0f)
            {
                car.state = Monster.StateType.Idle;
                car.animator.SetInteger("State", (int)car.state);
                car.go.SetActive(false);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }


    // (玩家调用)发起救援
    public void Rescue(int index)
    {
        if (index <= 0 || index > carList.Count)
        {
            return;
        }
        int id = index - 1;
        Car car = carList[id];
        if (car.state != Monster.StateType.Idle)
        {
            return;
        }
        if (car.target == null)
        {
            return;
        }

        if (car.target.Disappearing)
        {
            return;
        }

        if (car.target.IsDie())
        {
            return;
        }

        car.target.LockRescued = true;
        car.go.SetActive(true);
        car.state = Monster.StateType.Attack;
        car.animator.SetInteger("State", (int)car.state);
    }

    // (玩家调用)救援完成
    public Monster Pickup(int index)
    {
        Monster target = null;
        if (index <= 0 || index > carList.Count)
        {
            return target;
        }
        int id = index - 1;
        Car car = carList[id];
        if (car.state == Monster.StateType.Idle)
        {
            return target;
        }
        if (car.state == Monster.StateType.Rescued)
        {
            return target;
        }
        // 判断是否播放完升降动作
        AnimatorStateInfo info = car.animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Attack") && info.normalizedTime >= 1.0f)
        {
            target = car.target;
            car.state = Monster.StateType.Rescued;
        }
        return target;
    }

}

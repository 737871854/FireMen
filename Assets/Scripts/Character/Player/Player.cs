using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public class Player
{

    public enum StateType
    {
        Unknown = 0,
        Idle = 1,
        Play = 2,
        Wait = 3,
    }
    
    protected StateType state;
    protected float lifeTime;
    protected float continueTime;
    protected int coin;
    protected float addWater;
    protected float addWaterTime;
    protected float fireTime;
    protected float water;
    protected int score;
    protected int lastScore;
    protected int index;
    protected string name;
    protected bool aoeFlag;
    protected bool hasOutTicket;
    protected bool pass;

    public Player()
    {
        Reset();
        
    }

    public void Reset()
    {
        aoeFlag         = false;
        score           = 0;
        lastScore       = 0;
        addWaterTime    = 0.0f;
        fireTime        = GameConfig.GAME_CONFIG_WATER_DAMAGE_INTERVAL;
        water           = GameConfig.GAME_CONFIG_FULL_WATER;
        continueTime    = 0;
        lifeTime        = 0;
        name            = "";
        state           = StateType.Idle;
        hasOutTicket    = false;
    }

    public float AddWaterTime   { get { return addWaterTime; } set { addWaterTime = value; } }
    public float FireTime       { get { return fireTime; }  set { fireTime = value; } }
    public float Water          { get { return water; } set { water = value; } }
    public int Score            { get { return score; } set { score = value; } }
    public int LastScore        { get { return lastScore; } set { lastScore = value; } }
    public int Coin             { get { return coin; } set { coin = value; } }
    public float LifeTime       { get { return lifeTime; } set { lifeTime = value; } }
    public float ContinueTime   { get { return continueTime; } }
    public StateType State      { get { return state; } set { state = value; } }
    public int Index            { get { return index; } set { index = value; } }
    public string Name          { get { return name; } set { name = value; } }
    public bool AoeFlag         { get { return aoeFlag; } set { aoeFlag = value; } }
    public bool HasOutTicket    { get { return hasOutTicket; } set { hasOutTicket = value; } }
    public bool Pass            { get { return pass; } set { pass = value; } }

    public bool IsCanPlay()
    {
        if (Coin >= GameConfig.GAME_CONFIG_PER_USE_COIN)
        {
            if(State == Player.StateType.Idle || State == Player.StateType.Wait)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsPlaying()
    {
        return State == Player.StateType.Play;
    }

    public bool IsContinuing()
    {
        return State == Player.StateType.Wait;
    }

    public void ChangePlay()
    {
        if (!IsContinuing())
        {
            Reset();
        }
        lifeTime = GameConfig.GAME_CONFIG_MAX_LIFE_TIME;
        State = Player.StateType.Play;
        DecreaseCoin(GameConfig.GAME_CONFIG_PER_USE_COIN);
    }

    public bool IncreaseCoin(int value)
    {
        if (value <= 0)
        {
            return false;
        }

        if (GameConfig.GAME_CONFIG_MAX_COIN > value)
        {
            coin = coin + value;
			if (IsContinuing())
			{
				continueTime = GameConfig.GAME_CONFIG_MAX_WAIT_TIME;
			}
            return true;
        }
        return false;
    }

    public void ChangeCoin(int value)
    {
        coin = value;
    }

    public void ClearCoin()
    {
        coin = 0;
    }

    public bool DecreaseCoin(int value)
    {
        if (value <= 0)
        {
            return false;
        }

        if (coin >= value)
        {
            coin = coin - value;
            Main.SettingManager.Save();
            return true;
        }
        return false;
    }

    public bool IncreaseScore(int value)
    {
        if (value <= 0)
        {
            return false;
        }

        if (GameConfig.GAME_CONFIG_MAX_SCORE > value)
        {
            score = score + value;
            return true;
        }
        return false;
    }


    public bool DecreaseWater(int value)
    {
        if (water >= value)
        {
            water = water - value;
            return true;
        }
        return false;
    }

    public void SupplyWater()
    {
        addWater = (GameConfig.GAME_CONFIG_FULL_WATER - water) / GameConfig.GAME_CONFIG_ADD_WATER_TIEM;
        addWaterTime = GameConfig.GAME_CONFIG_ADD_WATER_TIEM;
    }

    public bool DecreaseLife(float value)
    {
        if (lifeTime >= value)
        {
            lifeTime = lifeTime - value;
            return true;
        }
        else
        {
            lifeTime = 0.0f;
        }
        return true;
    }

    public bool IncreaseLife(float value)
    {
        lifeTime += value;
        return true;
    }

    //public void PushName()
    //{
    //    if (nameEditIndex >= GameConfig.GAME_CONFIG_NAME_LEN)
    //    {
    //        return;
    //    }

    //    int value = 0;
    //    if (name[nameEditIndex] == '_')
    //    {
    //        value = 65;

    //    }
    //    else
    //    {
    //        value = (int)name[nameEditIndex];
    //        ++value;
    //        if (value > 90)
    //        {
    //            value = 65;
    //        }
    //    }

    //    char[] nameArray = name.ToCharArray();
    //    nameArray[nameEditIndex] = (char)value;
    //    name = new string(nameArray);       
    //}

    //public void ConfirmName()
    //{
    //    if (nameEditIndex >= GameConfig.GAME_CONFIG_NAME_LEN)
    //    {
    //        return;
    //    }
    //    if (name[nameEditIndex] == '_')
    //    {
    //        return;
    //    }
    //    name = name + '_';
    //    ++nameEditIndex;
    //}

    public void UpdateWater()
    {
        //if (addWaterTime > 0)
        //{
        //    addWaterTime -= Time.deltaTime;
        //    water += addWater * Time.deltaTime;
        //    if (water >= GameConfig.GAME_CONFIG_FULL_WATER)
        //    {
        //        water = GameConfig.GAME_CONFIG_FULL_WATER;
        //    }
        //}
        //else
        //{
        //    addWaterTime = 0;
        //}
    }

    public void UpdateLife()
    {
		if (pass)
		{
			return;
		}

        lifeTime -= Main.NonStopTime.deltaTime;
        if (lifeTime <= 0)
        {
            State = StateType.Wait;
            continueTime = GameConfig.GAME_CONFIG_MAX_WAIT_TIME;
            lifeTime = 0;
        }
    }

    public void UpdateContinue()
    {
        continueTime -= Main.NonStopTime.deltaTime;
        if (continueTime <= 0)
        {
            State = StateType.Idle;
        }
    }
}
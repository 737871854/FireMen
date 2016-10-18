using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public class GameController : MonoBehaviour
{
    System.Random ran = new System.Random();
    public enum InputType
    {
        External = 0,
        Builtin  = 1,
    }

    public class Joystick
    {
        public bool start;
        public bool supply;
        public bool fire;
        public Vector3 position;
        public bool car1;
        public bool car2;
        public bool car3;
        public bool flag1;
        public bool flag2;

        public Joystick()
        {
            position = new Vector3();
            Reset();
        }

        public void Reset()
        {
            start  = false;
            supply = false;
            fire   = false;
            car1   = false;
            car2   = false;
            car3   = false;
            flag1  = false;
            flag2  = false;
        }
    }

    protected Joystick[] joysticks;
    protected InputType  inputType;
    
    public void Init(InputType type)
    {
        inputType = type;
        joysticks = new Joystick[GameConfig.GAME_CONFIG_PLAYER_COUNT];
        for (int index = 0; index < GameConfig.GAME_CONFIG_PLAYER_COUNT; ++index)
        {
            joysticks[index] = new Joystick();
        }
    }

    void Update()
    {
        if (inputType == InputType.External)
        {
            UpdateExternal();
        }
        else
        {
            UpdateBuiltin();
        }
    }

    void UpdateExternal()
    {
        Main.IOManager.UpdateIOEvent();
        for (int index = 0; index < GameConfig.GAME_CONFIG_PLAYER_COUNT; ++index)
        {
                if (Main.IOManager.GetIsCoin(index))
                {                 
                    EventDispatcher.TriggerEvent(GameEventDef.EVNET_INPUT_COIN, index);                  
                }

                if (Main.IOManager.GetIsStart(index))
                {
                    joysticks[index].start = true;
                }

                if (Main.IOManager.GetIsShake(index))
                {
                    if (index == 0)
                    {
                        joysticks[index].car1 = true;
                    }
                    else if (index == 1)
                    {
                        joysticks[index].car2 = true;
                    }
                    else
                    {
                        joysticks[index].car3 = true;
                    }
                }

                if (Main.IOManager.GetIsPush1(index))
                {
                    joysticks[index].flag1 = true;
                }

                if (Main.IOManager.GetIsPush2(index))
                {
                    joysticks[index].flag2 = true;
                }

            joysticks[index].position = Main.IOManager.GetScreenPos(index);
        }      
        
    }

    void UpdateBuiltin()
    {
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_INPUT_COIN, GameConfig.GAME_CONFIG_PLAYER_1);
            }

            if (Input.GetKeyUp(KeyCode.F2))
            {
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_OUTPUT_COIN, GameConfig.GAME_CONFIG_PLAYER_1);
            }

            if (Input.GetKeyUp(KeyCode.F3))
            {
                joysticks[GameConfig.GAME_CONFIG_PLAYER_1].start = true;
            }

            if (Input.GetKeyUp(KeyCode.F4))
            {
                joysticks[GameConfig.GAME_CONFIG_PLAYER_1].car1 = true;
            }

            if (Input.GetKeyUp(KeyCode.PageUp))
            {
                joysticks[GameConfig.GAME_CONFIG_PLAYER_1].flag1 = true;
            }

            if (Input.GetKeyUp(KeyCode.PageDown))
            {
                joysticks[GameConfig.GAME_CONFIG_PLAYER_1].flag2 = true;
            }
        }

        {
            if (Input.GetKeyUp(KeyCode.F5))
            {
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_INPUT_COIN, GameConfig.GAME_CONFIG_PLAYER_2);
            }

            if (Input.GetKeyUp(KeyCode.F6))
            {
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_OUTPUT_COIN, GameConfig.GAME_CONFIG_PLAYER_2);
            }

            if (Input.GetKeyUp(KeyCode.F7))
            {
                joysticks[GameConfig.GAME_CONFIG_PLAYER_2].start = true;
            }

            if (Input.GetKeyUp(KeyCode.F8))
            {
                joysticks[GameConfig.GAME_CONFIG_PLAYER_2].car2 = true;
            }
        }

        {
            if (Input.GetKeyUp(KeyCode.F9))
            {
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_INPUT_COIN, GameConfig.GAME_CONFIG_PLAYER_3);
            }

            if (Input.GetKeyUp(KeyCode.F10))
            {
                EventDispatcher.TriggerEvent(GameEventDef.EVNET_OUTPUT_COIN, GameConfig.GAME_CONFIG_PLAYER_3);
            }

            if (Input.GetKeyUp(KeyCode.F11))
            {
                joysticks[GameConfig.GAME_CONFIG_PLAYER_3].start = true;
            }

            if (Input.GetKeyUp(KeyCode.F12))
            {
                joysticks[GameConfig.GAME_CONFIG_PLAYER_3].car3 = true;
            }
        }

        joysticks[GameConfig.GAME_CONFIG_PLAYER_1].position = Input.mousePosition;
    }

    public void ClearState(int index)
    {
        if (inputType == InputType.External)
        {
            Main.IOManager.ResetEvent(index);
        }
        joysticks[index].Reset();
    }

    public bool IsFireButtonDown(int index)
    {
        return joysticks[index].fire;
    }

    public bool IsSupplyButtonPressed(int index)
    {
        bool value = joysticks[index].supply;
        return value;
    }

    public bool IsStartButtonPressed(int index)
    {
        bool value = joysticks[index].start;
        return value;
    }

    public bool IsCallCar1ButtonPressed(int index)
    {
        bool value = joysticks[index].car1;
        return value;
    }

    public bool IsCallCar2ButtonPressed(int index)
    {
        bool value = joysticks[index].car2;
        return value;
    }

    public bool IsCallCar3ButtonPressed(int index)
    {
        bool value = joysticks[index].car3;
        return value;
    }

    public bool IsFlag1ButtonPressed(int index)
    {
        bool value = joysticks[0].flag1;
        return value;
    }

    public bool IsFlag2ButtonPressed(int index)
    {
        bool value = joysticks[0].flag2;
        return value;
    }

    public Vector3 JoystickPosition(int index)
    {
        return joysticks[index].position;
    }

    public bool IsWaterLow()
    {
        return Main.IOManager.IsWaterLow;
    }

    public bool IsWaterHight()
    {
        return Main.IOManager.IsWaterHight;
    }
}
/**
* Copyright (c) 2012,广州纷享游艺设备有限公司
* All rights reserved.
* 
* 文件名称：IOOperation.cs
* 简    述：协议解析方法
* 创建标识：meij  2015/10/28
* 修改标识：meij  2015/11/06
* 修改描述：加入屏幕坐标信息。
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Need.Mx;

public class IOParser
    {
        private static float screenwidth = Screen.width;
        private static float screenheight = Screen.height;
        private static float width;
        private static float height;
        private static float miniheight;
        private static List<float[]> poses = new List<float[]>();
        /// <summary>
        /// 取一个字节中指定的某一位
        /// </summary>
        /// <param name="i"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static byte GetBit(byte i, byte k)
        {
            byte value = 0;
            value = k;
            value = (byte)(value << i);
            value = (byte)(value >> 7);
            return value;
        }

        /// <summary>
        /// 获取player的屏幕坐标
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static Vector3 World2ScreenPos(Vector3 value,int playerid)
        {
            Vector3 screenpos = new Vector3();
            float halfwidth = 128.0f, halfheight = 128.0f;
            switch(playerid)
            {
                case 0:
                    halfwidth  = GameConfig.GAME_CONFIG_HALF_WIDTH_PLAYER_0;
                    halfheight = GameConfig.GAME_CONFIG_HALF_HEIGHT_PLAYER_0;
                    poses = GameConfig.GAME_CONFIG_POINTS_POSES_0;
                    miniheight = poses[1][0] - poses[1][10];
                    //miniheight = poses[0].y - poses[10].y;
                    break;
                case 1:
                    halfwidth  = GameConfig.GAME_CONFIG_HALF_WIDTH_PLAYER_1;
                    halfheight = GameConfig.GAME_CONFIG_HALF_HEIGHT_PLAYER_1;
                    poses = GameConfig.GAME_CONFIG_POINTS_POSES_1;
                    miniheight = poses[1][2] - poses[1][12];
                    //miniheight = poses[2].y - poses[12].y;
                    break;
                case 2:
                    halfwidth  = GameConfig.GAME_CONFIG_HALF_WIDTH_PLAYER_2;
                    halfheight = GameConfig.GAME_CONFIG_HALF_HEIGHT_PLAYER_2;
                    poses = GameConfig.GAME_CONFIG_POINTS_POSES_2;
                    miniheight = poses[1][4] - poses[1][14];
                    //miniheight = poses[4].y - poses[14].y;
                    break;
            }
            width  = GameConfig.GAME_CONFIG_SCREEN_WIDTH_HEIGHT[0];
            height = GameConfig.GAME_CONFIG_SCREEN_WIDTH_HEIGHT[1];

            if (value.x >= poses[0][0] && value.x < poses[0][1])
            {
                if(value.y >= poses[1][5])
                {
                    screenpos.x = (value.x - poses[0][0]) / (poses[0][1] - poses[0][0]) * width * 0.5f + 0.5f * screenwidth - width;
                    if(value.y <= poses[1][0])
                    {
                        screenpos.y = (value.y - poses[1][5]) / (poses[1][0] - poses[1][5]) * height + 0.5f * screenheight;
                    }
                    if( value.y > poses[1][0] && value.y <= poses[1][1])
                    {
                        screenpos.y = screenheight;
                    }
                }  
            }


            if (value.x >= poses[0][1] && value.x < poses[0][2])
            {
                if (value.y >= poses[1][6])
                {
                    screenpos.x = (value.x - poses[0][1]) / (poses[0][2] - poses[0][1]) * width * 0.5f + 0.5f * screenwidth - 0.5f * width;
                    if (value.y <= poses[1][1])
                    {
                        screenpos.y = (value.y - poses[1][6]) / (poses[1][1] - poses[1][6]) * height + 0.5f * screenheight;
                    }
                    if (value.y > poses[1][1] && value.y <= poses[1][2])
                    {
                        screenpos.y = screenheight;
                    }
                }
            }

            if (value.x >= poses[0][2] && value.x < poses[0][3])
            {
                if (value.y >= poses[1][7])
                {
                    screenpos.x = (value.x - poses[0][2]) / (poses[0][3] - poses[0][2]) * width * 0.5f + 0.5f * screenwidth;
                    if (value.y <= poses[1][2])
                    {
                        screenpos.y = (value.y - poses[1][7]) / (poses[1][2] - poses[1][7]) * height + 0.5f * screenheight;
                    }
                    if (value.y > poses[1][2] && value.y <= poses[1][3])
                    {
                        screenpos.y = screenheight;
                    }
                }
            }

            if (value.x >= poses[0][3] && value.x < poses[0][4])
            {
                if (value.y >= poses[1][8])
                {
                    screenpos.x = (value.x - poses[0][3]) / (poses[0][4] - poses[0][3]) * width * 0.5f + 0.5f * screenwidth + 0.5f * width;
                    if (value.y <= poses[1][3])
                    {
                        screenpos.y = (value.y - poses[1][8]) / (poses[1][3] - poses[1][8]) * height + 0.5f * screenheight;
                    }
                    if (value.y > poses[1][3] && value.y <= poses[1][4])
                    {
                        screenpos.y = screenheight;
                    }
                }
            }



            if (value.x >= poses[0][5] && value.x < poses[0][6])
            {
                if (value.y >= poses[1][10])
                {
                    screenpos.x = (value.x - poses[0][5]) / (poses[0][6] - poses[0][5]) * width * 0.5f + 0.5f * screenwidth - width;
                    if (value.y <= poses[1][5])
                    {
                        screenpos.y = (value.y - poses[1][10]) / (poses[1][5] - poses[1][10]) * height + 0.5f * screenheight - height;
                    }
                    if (value.y > poses[1][5] && value.y <= poses[1][6])
                    {
                        screenpos.y = 0.5f * screenheight;
                    }
                }
            }

            if (value.x >= poses[0][6] && value.x < poses[0][7])
            {
                if (value.y >= poses[1][11])
                {
                    screenpos.x = (value.x - poses[0][6]) / (poses[0][7] - poses[0][6]) * width * 0.5f + 0.5f * screenwidth - 0.5f * width;
                    if (value.y <= poses[1][6])
                    {
                        screenpos.y = (value.y - poses[1][11]) / (poses[1][6] - poses[1][11]) * height + 0.5f * screenheight - height;
                    }
                    if (value.y > poses[1][6] && value.y <= poses[1][7])
                    {
                        screenpos.y = 0.5f * screenheight;
                    }
                }
            }

            if (value.x >= poses[0][7] && value.x < poses[0][8])
            {
                if (value.y >= poses[1][12])
                {
                    screenpos.x = (value.x - poses[0][7]) / (poses[0][8] - poses[0][7]) * width * 0.5f + 0.5f * screenwidth;
                    if (value.y <= poses[1][7])
                    {
                        screenpos.y = (value.y - poses[1][12]) / (poses[1][7] - poses[1][12]) * height + 0.5f * screenheight - height;
                    }
                    if (value.y > poses[1][7] && value.y <= poses[1][8])
                    {
                        screenpos.y = 0.5f * screenheight;
                    }
                }
            }

            if (value.x >= poses[0][8] && value.x < poses[0][9])
            {
                if (value.y >= poses[1][13])
                {
                    screenpos.x = (value.x - poses[0][8]) / (poses[0][9] - poses[0][8]) * width * 0.5f + 0.5f * screenwidth + 0.5f * width;
                    if (value.y <= poses[1][8])
                    {
                        screenpos.y = (value.y - poses[1][13]) / (poses[1][8] - poses[1][13]) * height + 0.5f * screenheight - height;
                    }
                    if (value.y > poses[1][8] && value.y <= poses[1][9])
                    {
                        screenpos.y = 0.5f * screenheight;
                    }
                }
            }

            if (value.x < poses[0][0])
            {
                screenpos.x = 0.5f * screenwidth - width;
                if (value.y <= poses[1][5] && value.y >= poses[1][10])
                {
                    screenpos.y = (value.y - poses[1][10]) / (poses[1][5] - poses[1][10]) * height + 0.5f * screenheight - height;
                }
                else if (value.y > poses[1][5] && value.y <= poses[1][0])
                {
                    screenpos.y = (value.y - poses[1][5]) / (poses[1][0] - poses[1][5]) * height + 0.5f * screenheight;
                }
                else if (value.y < poses[1][10])
                {
                    screenpos.y = 0.5f * screenheight - height;
                }
                else if (value.y > poses[1][0])
                {
                    screenpos.y = 0.5f * screenheight + height;
                }
            }


            if (value.x > poses[0][14])
            {
                screenpos.x = 0.5f * screenwidth + width;
                if (value.y <= poses[1][9] && value.y >= poses[1][14])
                {
                    screenpos.y = (value.y - poses[1][14]) / (poses[1][9] - poses[1][14]) * height + 0.5f * screenheight - height;
                }
                else if (value.y > poses[1][9] && value.y <= poses[1][4])
                {
                    screenpos.y = (value.y - poses[1][9]) / (poses[1][4] - poses[1][9]) * height + 0.5f * screenheight;
                }
                else if (value.y < poses[1][14])
                {
                    screenpos.y = 0.5f * screenheight - height;
                }
                else if (value.y > poses[1][4])
                {
                    screenpos.y = 0.5f * screenheight + height;
                }
            }
            return screenpos;
        }

    /// <summary>
    ///  byte类型转为float类型
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
     public static float Byte2Float(byte b)
    {
        if ((b & 0x80) != 0)
        {
            return -((float)(b & 0x7f));
        }
        return (float)b;
    }

    public static string ByteArray2String(byte[] b)
     {
        string str = "";
        for (int i = 0; i < b.Length - 1; ++i )
        {
            str += b[i].ToString();
            str += "/";
        }
        str += b[b.Length - 1].ToString();
        return str;
     }

    public static byte[] String2IntArray(string str)
    {    
        string[] strArray = str.Split('/');
        byte[] array = new byte[strArray.Length];
        for (int i = 0; i < strArray.Length; ++i )
        {
            array[i] = (byte)strArray[i].ToInt();
        }
        return array;
    }
}



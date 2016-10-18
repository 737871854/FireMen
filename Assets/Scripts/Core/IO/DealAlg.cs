using UnityEngine;
using System.Collections;
using Need.Mx;

public class DealAlg
{
    public static byte DAT3_1()
    {
        byte result = 0;
        byte[] id = IOParser.String2IntArray(GameConfig.GAME_CONFIG_ID);
        for (int i = 0; i < id.Length; ++i )
        {
            result = (byte)(result + id[i]);
        }
        return result;
    }

    public static byte DAT3_2()
    {
        byte result = 0;
        byte min = 0;
        byte max = 0;
        byte[] id = IOParser.String2IntArray(GameConfig.GAME_CONFIG_ID);
        min = id[0];
        max = id[0];
        for (int i = 0; i < id.Length; ++i)
        {
          
            if (min > id[i])
            {
                min = id[i];
            }

            if (max < id[i])
            {
                max = id[i];
            }
        }
        result = (byte)(min + max);

        return result;
    }

    public static byte[] DAT3_3()
    {
        byte[] result = new byte[4];
        byte[] id = IOParser.String2IntArray(GameConfig.GAME_CONFIG_ID);
        result[0] = (byte)(id[0] + id[1]);
        result[1] = (byte)(id[2] + id[3]);
        result[2] = (byte)(id[4] + id[5]);
        result[3] = (byte)(id[6] + id[6]);

        return result;
    }

    public static byte[] DAT3_4()
    {
        byte[] result = new byte[4];
        byte[] id = IOParser.String2IntArray(GameConfig.GAME_CONFIG_ID);
        result[0] = (byte)(id[0] + id[6]);
        result[1] = (byte)(id[1] + id[5]);
        result[2] = (byte)(id[2] + id[4]);
        result[3] = (byte)(id[3] + id[3]);

        return result;
    }

    public static byte[] DAT3_5()
    {
        byte[] id = IOParser.String2IntArray(GameConfig.GAME_CONFIG_ID);
        for (int i = 0; i < id.Length; ++i )
        {
            byte a = (byte)(id[i] >> 3);
            byte b = (byte)(id[i] << 5);
            id[i] = (byte)((byte)(a + b) + 5);
        }

        return id;
    }

    public static byte[] DAT3_6()
    {
        byte[] id = IOParser.String2IntArray(GameConfig.GAME_CONFIG_ID);
        for (int i = 0; i < id.Length; ++i)
        {
            byte a = (byte)(id[i] >> 1);
            byte b = (byte)(id[i] << 7);
            id[i] = (byte)((byte)(a + b) + 7);
        }

        return id;
    }

    public static byte[] DAT3_7()
    {
        byte[] id = IOParser.String2IntArray(GameConfig.GAME_CONFIG_ID);
        for (int i = 0; i < id.Length; ++i)
        {
            byte a = (byte)(id[i] << 2);
            byte b = (byte)(id[i] >> 6);
            id[i] = (byte)((byte)(a + b) + 4);
        }

        return id;
    }

    public static byte[] DAT3_8()
    {
        byte[] id = IOParser.String2IntArray(GameConfig.GAME_CONFIG_ID);
        for (int i = 0; i < id.Length; ++i)
        {
            byte a = (byte)(id[i] << 3);
            byte b = (byte)(id[i] >> 5);
            id[i] = (byte)((byte)(a + b) + 6);
        }

        return id;
    }

    public static byte DAT3_9()
    {
        byte result = 0;
        byte[] id = IOParser.String2IntArray(GameConfig.GAME_CONFIG_ID);
        byte a = (byte)((byte)(id[3] + id[4]) >> 2);
        byte b = (byte)((byte)(id[3] + id[4]) << 6);
        result = (byte)((byte)(a + b) + 10);
        return result;
    }

    public static byte[] DAT3_A()
    {
        byte[] result = new byte[2];
        byte[] id = IOParser.String2IntArray(GameConfig.GAME_CONFIG_ID);
        byte a = (byte)((byte)((id[4] + id[5])) << 2);
        byte b = (byte)((byte)((id[4] + id[5])) >> 5);
        byte c = (byte)((byte)(a + b) + 9);
        result[0] = (byte)(c >> 4);
        result[1] = (byte)((byte)((c << 4)) >> 4);
        return result;
    }


    public static bool Compare(byte a, byte b)
    {
        if (a != b)
        {
            return false;
        }
        return true;
    }

    public static bool Compare(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        for (int i = 0; i < a.Length; ++i )
        {
            if (a[i] != b[i])
            {
                return false;
            }
        }
        return true;
    }
}

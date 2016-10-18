/**
* Copyright (c) 2012,广州纷享游艺设备有限公司
* All rights reserved.
* 
* 文件名称：SerialIOHost.cs
* 简    述：每条协议包含的信息
* 创建标识：？&meij  2015/10/28
* 修改标识：meij  2015/11/10
* 修改描述：采用求和校验，一条协议包含三个玩家的所有信息。
*/
using System;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using Need.Mx;

public class SerialIOHost
{
    private static int _curCom;
    private int _curReadIndex;
    private bool _dataValid;
    private volatile bool _detectingHeader = true;
    private int _errorCount;
    private bool _hadFindCom;
    private string[] _portNames;
    private byte[] _readBuf;
    private byte[] ID;
    private byte DAT3;
    private int _readBufSize;
    private volatile bool _receiveSucceed;
    private int _refreshRate = 5;
    private SerialPort _serialPort;
    private int _timeout = 0x3e8;
    private int _timer;
    private volatile bool _updateFinished;
    private Thread _updateThread;
    private byte[] _userReadBuf;
    private byte[] _userWriteBuf;
    private byte[] _writeBuf;
    private int _writeBufSize;
    private const int MaxErrorCount = 5;

    public Queue<IOEvent>[] quequeInfo = new Queue<IOEvent>[3];
    private IOEvent proInfo;

    public event SerialIoEventHandler OnReceiveFailed;

    public event SerialIoEventHandler OnReceiveSucceed;

    public void Close()
    {
        if (_updateThread == null)
        {
            this._serialPort.Close();
        }
        else if (!_updateThread.IsAlive)
        {
            this._serialPort.Close();
        }
        else
        {
            _updateThread.Abort();
            float num = 0f;
            while (_updateThread.IsAlive)
            {
                Thread.Sleep(100);
                num += 100f;
                //Debug.Log("Watting for abort!");
                if (num > 10000f)
                {
                    break;
                }
            }
            this._serialPort.Close();
        }
    }

    private static uint CrcCompute(byte[] buf, uint len)
    {
        uint num = 0x20121201;
        for (uint i = 0; i < len; i++)
        {
            num ^= buf[i] + len;
        }
        return (num * len);
    }

    private static void DebugOutput(string header, byte[] data)
    {
        string message = header;
        for (int i = 0; i < data.Length; i++)
        {
            string str2 = message;
            message = string.Concat(new object[] { str2, Convert.ToString(data[i], 0x10), "(", data[i], ")" }) + " ";
        }
        Debug.Log(message);
    }

    public bool FindDevice()
    {
        int num = 1;
        bool flag = true;
        if ((this._portNames == null) || (this._portNames.Length == 0))
        {
            this._portNames = SerialPort.GetPortNames();
        }
        if (this._portNames.Length > 0)
        {
            while (num-- > 0)
            //while (flag)
            {
                for (int i = 0; i < this._portNames.Length; i++)
                {
                    if (!this.OpenCom(_curCom))
                    {
                        Debug.LogWarning(this._portNames[_curCom] + " open failed!");
                    }
                    else
                    {
                        this.Send();
                        int num3 = 0;
                        try
                        {
                            num3 = this._serialPort.Read(this._readBuf, this._curReadIndex, this._readBufSize);
                        }
                        catch (TimeoutException)
                        {
                            //Debug.LogWarning(this._portNames[_curCom] + " no respond!!");
                        }
                        if ((num3 > 0) && ((this._readBuf[0] == 0x55) || (this._readBuf[0] == 170) || (this._readBuf[0] == 90)))
                        //if ((num3 > 0))
                        {
                            flag = false;
                            this._hadFindCom = true;
                            _updateThread = new Thread(new ThreadStart(this.UpdateThread));
                            _updateThread.Start();
                            //Debug.Log("Host:" + this._serialPort.PortName + " have been found!!");

                            Main.IOManager.SetFindPort(this._portNames[_curCom]);
                            Main.IOManager.SetConnnectCount();
                            return true;
                        }
                        if (++_curCom >= this._portNames.Length)
                        {
                            _curCom = 0;
                        }
                    }
                   
                }
            }         
        }
        return false;
    }

    public bool HadDevice()
    {
        if (this._serialPort == null)
        {
            return false;
        }
        return (this._hadFindCom && this._serialPort.IsOpen);
    }

    private bool OpenCom(int com)
    {
        if (this._portNames == null)
        {
            this._portNames = SerialPort.GetPortNames();
        }
        this._serialPort = new SerialPort(this._portNames[com], 0x4B00, Parity.None, 8, StopBits.One);
        this._serialPort.ReadTimeout = 0x3e8;
        this._serialPort.WriteTimeout = 0x3e8;
        this._serialPort.DtrEnable = true;
        try
        {
            this._serialPort.Open();
        }
        catch (Exception exception)
        {
            //Debug.LogError(this._serialPort.PortName + ":" + exception);
            return false;
        }
        return this._serialPort.IsOpen;
    }

    public byte Read(int index)
    {
        return this._userReadBuf[index + 1];
    }

    public void Read(ref byte[] outData)
    {
        int index = 1;
        int num2 = Mathf.Min((int) (outData.Length + 1), (int) (this._userWriteBuf.Length - 4));
        while (index < num2)
        {
            outData[index] = this._userReadBuf[index];
            index++;
        }
    }

    public bool Read(int index, int bit)
    {
        return ((this._userReadBuf[index + 1] & (((int) 1) << bit)) != 0);
    }

    private void Test(byte[] test)
    {

    }

    private void Receive()
    {
        Main.IOManager.IsConnect = false;
        try
        {
            if (!this._detectingHeader)
            {
                int num = this._serialPort.Read(this._readBuf, this._curReadIndex, this._readBufSize - this._curReadIndex);
                if (num > 0)
                {
                    this._curReadIndex += num;
                    this._timer += this._refreshRate;
                    if (this._curReadIndex >= this._readBufSize)
                    {
                        // AA
                        if (this._readBuf[0] == 170)
                        {
                            this._timer = 0;
                            this._curReadIndex = 0;
                            this._detectingHeader = true;
                            this._updateFinished = true;
                            if (this.VerifyData())
                            {
                                Main.IOManager.IsConnect = true;
                                byte[] bb = new byte[14];
                                for (int i = 0; i < 14; ++i)
                                {
                                    bb[i] = _readBuf[i];
                                }
                                Main.IOManager.ByteHostAA = bb;
                            }
                        }

                        // 5A
                        if (this._readBuf[0] == 90)
                        {
                            this._timer = 0;
                            this._curReadIndex = 0;
                            byte[] bb = new byte[14];
                            for (int i = 0; i < 14;++i )
                            {
                                bb[i] = _readBuf[i];
                            }
                            Main.IOManager.ByteHost = bb;

                            this._detectingHeader = true;
                            this._updateFinished = true;
                            Main.IOManager.CheckPass = this.CheckData();

                            Main.IOManager.Receice5A = true;
                            ClearBuffer();
                        }

                        // 缓存数据出错，清除缓存
                        if (this._readBuf[0] != 170 && this._readBuf[0] != 90)
                        {
                            this._timer = 0;
                            this._curReadIndex = 0;
                            //Debug.Log("协议头此错误");
                            //this._serialPort.DiscardInBuffer();
                            //this._curReadIndex = 0;
                        }
                                          
                    }
                    else if (this._timer >= this._timeout)
                    {
                        this._timer = 0;
                        this._curReadIndex = 0;
                        this._receiveSucceed = false;
                        this._detectingHeader = true;
                        this._updateFinished = true;
                    }
                }
            }
            else
            {
                int num2 = this._serialPort.Read(this._readBuf, 0, this._readBufSize);
                if (num2 > 0)
                {
                    // AA
                    if (this._readBuf[0] == 170)
                    {
                        this._detectingHeader = false;
                        this._curReadIndex = num2;
                        if (this._curReadIndex >= this._readBufSize)
                        {
                            this._timer = 0;
                            this._curReadIndex = 0;
                            this._detectingHeader = true;
                            this._updateFinished = true;
                            if (this.VerifyData())
                            {
                                Main.IOManager.IsConnect = true;
                                byte[] bb = new byte[14];
                                for (int i = 0; i < 14; ++i)
                                {
                                    bb[i] = _readBuf[i];
                                }
                                Main.IOManager.ByteHostAA = bb;
                            }
                           
                        }
                        else
                        {
                            //Debug.Log("收到协议位数少于14");
                        }
                    }

                    if (this._readBuf[0] == 0x55)
                    {
                        this._timer = 0;
                        this._curReadIndex = 0;
                        this._detectingHeader = false;
                        this._curReadIndex = num2;
                        if (this._curReadIndex >= this._readBufSize)
                        {
                            this._detectingHeader = true;
                            this._updateFinished = true;
                            this.VerifyData();
                        }
                    }

                    // 5A
                    if (this._readBuf[0] == 90)
                    {
                        this._timer = 0;
                        this._curReadIndex = 0;
                        byte[] bb = new byte[14];
                        for (int i = 0; i < 14; ++i)
                        {
                            bb[i] = _readBuf[i];
                        }
                        Main.IOManager.ByteHost = bb;

                        this._detectingHeader = false;
                        this._curReadIndex = num2;
                        if (this._curReadIndex >= this._readBufSize)
                        {
                            this._detectingHeader = true;
                            this._updateFinished = true;
                            Main.IOManager.CheckPass = this.CheckData();

                            Main.IOManager.Receice5A = true;
                            ClearBuffer();
                        }
                    }


                    // 缓存数据出错，清除缓存
                    if (this._readBuf[0] != 170 && this._readBuf[0] != 90 && this._readBuf[0] != 0x55)
                    {
                        this._timer = 0;
                        this._curReadIndex = 0;
                        //Debug.Log("协议头此错误");
                        //this._serialPort.DiscardInBuffer();
                        //this._curReadIndex = 0;
                    }

                }
            }
        }
        catch (TimeoutException)
        {
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
        }
    }

    private void ClearBuffer()
    {
        for (int i = 0; i < this._readBuf.Length; i++ )
        {
            this._readBuf[i] = 0;
        }
    }

    private void Send()
    {
        if (this._serialPort.IsOpen)
        {
            if (this._receiveSucceed)
            {
                this._writeBuf[0] = 170;
            }
            else
            {
                this._writeBuf[0] = 0x55;
            }
            byte num = 0;
            for(int i=1;i<this._writeBufSize-1;i++)
            {
                num += this._writeBuf[i];
            }
            this._writeBuf[this._writeBufSize - 1] = num;          
            this._serialPort.DiscardInBuffer();
            this._serialPort.Write(this._writeBuf, 0, this._writeBufSize);
        }
    }

    public bool Setup(int readBufSize, int writeBufSize, int refreshRate = 5)
    {
        for (int i = 0; i < quequeInfo.Length; i++)
        {
            quequeInfo[i] = new Queue<IOEvent>();
        }
        this._readBufSize    = readBufSize;         //15
        this._writeBufSize   = writeBufSize;        //10
        this._readBuf        = new byte[this._readBufSize];
        this._writeBuf       = new byte[this._writeBufSize];
        this._userReadBuf    = new byte[this._readBufSize];
        this._userWriteBuf   = new byte[this._writeBufSize];
        this._refreshRate    = refreshRate;
        this._portNames      = SerialPort.GetPortNames(); 
        return this.FindDevice();
    }

    private void SwitchReadBuf()
    {
        byte[] buffer = this._userReadBuf;
        lock (buffer)
        {
            byte[] buffer2 = this._userReadBuf;
            this._userReadBuf = this._readBuf;
            this._readBuf = buffer2;
        }
    }

    private void SwitchWriteBuf()
    {
        byte[] buffer = this._userWriteBuf;
        lock (buffer)
        {
            byte[] buffer2 = this._userWriteBuf;
            this._userWriteBuf = this._writeBuf;
            this._writeBuf = buffer2;
        }
    }

    public void Update()
    {
        this._dataValid = false;
        if (this._hadFindCom && this._updateFinished)
        {
            if (this._receiveSucceed)
            {
                if (this.OnReceiveSucceed != null)
                {
                    this.OnReceiveSucceed();
                }
            }
            else if (this.OnReceiveFailed != null)
            {
                this.OnReceiveFailed();
            }
            this._updateFinished = false;
            this._dataValid = true;
        }
    }

    private void UpdateThread()
    {
        if ((this._serialPort != null) && this._serialPort.IsOpen)
        {
            while (true)
            {
                if (!this._updateFinished)
                {
                    this.SwitchWriteBuf();
                    this.Send();
                }
                this.Receive();
                Thread.Sleep(this._refreshRate);
            }
        }
    }

    private bool CheckData()
    {
        if (0 == _readBuf[1])
        {
            ID = new byte[7];
            for (int i = 0; i < 7; ++i)
            {
                ID[i] = _readBuf[i + 5];
            }
            string str = IOParser.ByteArray2String(ID);
            Main.IOManager.IDString = str;
         return true;
        }

        bool pass = false;
        if (1 == _readBuf[1])
        {
            switch (_readBuf[2])
            {
                case 0x01:
                    {
                        pass = DealAlg.Compare(_readBuf[5], DealAlg.DAT3_1());
                        break;
                    }
                case 0x02:
                    {
                        pass = DealAlg.Compare(_readBuf[6], DealAlg.DAT3_2());
                        break;
                    }
                case 0x03:
                    {
                        byte[] a = new byte[4];
                        a[0] = _readBuf[5];
                        a[1] = _readBuf[6];
                        a[2] = _readBuf[8];
                        a[3] = _readBuf[9];
                        pass = DealAlg.Compare(a, DealAlg.DAT3_3());
                        break;
                    }
                case 0x04:
                    {
                        byte[] a = new byte[4];
                        a[0] = _readBuf[5];
                        a[1] = _readBuf[6];
                        a[2] = _readBuf[7];
                        a[3] = _readBuf[9];
                        pass = DealAlg.Compare(a, DealAlg.DAT3_4());
                        break;
                    }
                case 0x05:
                    {
                        byte[] a = new byte[7];
                        a[0] = _readBuf[4];
                        a[1] = _readBuf[5];
                        a[2] = _readBuf[6];
                        a[3] = _readBuf[7];
                        a[4] = _readBuf[8];
                        a[5] = _readBuf[9];
                        a[6] = _readBuf[11];
                        pass = DealAlg.Compare(a, DealAlg.DAT3_5());
                        break;
                    }
                case 0x06:
                    {
                        byte[] a = new byte[7];
                        a[0] = _readBuf[5];
                        a[1] = _readBuf[6];
                        a[2] = _readBuf[7];
                        a[3] = _readBuf[8];
                        a[4] = _readBuf[9];
                        a[5] = _readBuf[10];
                        a[6] = _readBuf[11];
                        pass = DealAlg.Compare(a, DealAlg.DAT3_6());
                        break;
                    }
                case 0x07:
                    {
                        byte[] a = new byte[7];
                        a[0] = _readBuf[4];
                        a[1] = _readBuf[5];
                        a[2] = _readBuf[7];
                        a[3] = _readBuf[8];
                        a[4] = _readBuf[9];
                        a[5] = _readBuf[10];
                        a[6] = _readBuf[11];
                        pass = DealAlg.Compare(a, DealAlg.DAT3_7());
                        break;
                    }
                case 0x08:
                    {
                        byte[] a = new byte[7];
                        a[0] = _readBuf[4];
                        a[1] = _readBuf[5];
                        a[2] = _readBuf[6];
                        a[3] = _readBuf[7];
                        a[4] = _readBuf[9];
                        a[5] = _readBuf[10];
                        a[6] = _readBuf[12];
                        pass = DealAlg.Compare(a, DealAlg.DAT3_8());
                        break;
                    }
                case 0x09:
                    {
                        pass = DealAlg.Compare(_readBuf[7], DealAlg.DAT3_9());
                        break;
                    }
                case 0x0A:
                    {
                        byte[] a = new byte[2];
                        a[0] = _readBuf[6];
                        a[1] = _readBuf[7];
                        pass = DealAlg.Compare(a, DealAlg.DAT3_A());
                        break;
                    }
            }
        }

        if (!pass)
        {
            Main.IOManager.HasCheck = false;
            return false;
        }

        Main.IOManager.HasCheck = true;
        return true;
    }

    private bool VerifyData()
    {
        byte num = 0;
        for (int i = 1; i < this._readBufSize - 1; i++)
        {
            num += _readBuf[i];
        }
        if (num != _readBuf[_readBufSize - 1])
        {
            Debug.LogWarning("端口:" + this._serialPort.PortName  + "求和校验失败");
            this._serialPort.DiscardInBuffer();
            this._serialPort.DiscardOutBuffer();
            this._receiveSucceed = false;
            return false;
        }        
        this.SwitchReadBuf();
        this.EnterQueue();
        this._receiveSucceed = true;
        return true;
    }

    public void EnterQueue()
    {
        Debug.Log("Queue");
        int num = 0;
        for (byte i = 0; i < 3; i++)
        {
            proInfo                             = new IOEvent();
            proInfo.ID                          = this.Read(num);
            byte state                          = this.Read(num+1);
            proInfo.IsPush1                     = this.IsBool(IOParser.GetBit(7, state));
            proInfo.IsPush2                     = this.IsBool(IOParser.GetBit(6, state));
            proInfo.IsShake                     = this.IsBool(IOParser.GetBit(5, state));
            proInfo.IsWarning                   = this.IsBool(IOParser.GetBit(4, state));
            proInfo.IsOutTicket                 = this.IsBool(IOParser.GetBit(3, state));
            proInfo.IsCoin                      = this.IsBool(IOParser.GetBit(2, state));
            proInfo.IsStart                     = this.IsBool(IOParser.GetBit(1, state));
            proInfo.IsSet                       = this.IsBool(IOParser.GetBit(0, state));
            proInfo.RockerBar                   = new Vector3(this.Read(num + 2), this.Read(num + 3), 0);
            proInfo.ScreenPos                   = IOParser.World2ScreenPos(proInfo.RockerBar,i);
            num += 4;
            if (quequeInfo[i].Count > 5)
            {
                this.quequeInfo[i].Dequeue();
            }
            this.quequeInfo[i].Enqueue(proInfo);
        }       
    }

    /// <summary>
    /// 1为true
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private bool IsBool(byte value)
    {
        if (value == 1)
            return true;
        else
            return false;
    }

    public void Write(int index, byte data)
    {
        this._userWriteBuf[index + 1] = data;
    }

    public void Write(int startIndex, byte[] data)
    {
        int index = startIndex + 1;
        int num2 = Mathf.Min((int) ((startIndex + 1) + data.Length), (int) (this._userWriteBuf.Length - 4));
        for (int i = 0; index < num2; i++)
        {
            this._userWriteBuf[index] = data[i];
            index++;
        }
    }

    public void Write(int index, int bit, bool set)
    {
        if (set)
        {
            byte num = (byte) (((int) 1) << bit);
            this._userWriteBuf[index + 1] = (byte) (this._userWriteBuf[index + 1] | num);
        }
        else
        {
            byte num2 = (byte) ~(((int) 1) << bit);
            this._userWriteBuf[index + 1] = (byte) (this._userWriteBuf[index + 1] & num2);
        }
    }

    public bool DataValid
    {
        get
        {
            return this._dataValid;
        }
    }

    public bool UpdateFinished
    {
        get
        {
            return this._updateFinished;
        }
    }

    public delegate void SerialIoEventHandler();
}


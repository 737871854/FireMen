/*
 * Copyright (c) 
 * 
 * 文件名称：   SettingLogic.cs
 * 
 * 简    介:    SettingLogic Setting逻辑
 * 
 * 创建标识：  Mike 2015/11/21 9:11:14
 * 
 * 修改描述：  Mike 2016.9.3  数据存储方式由PlayerPrefs转为JSON      注：代码质量还有很大优化空间
 * 
 */

using UnityEngine;
using System.Collections;
using Need.Mx;

public class SettingLogic : MonoBehaviour {

    protected SettingView view;
    private SettingConfig.Page curPage; //当前选中页
    private SettingConfig.HomePageItem curHomeItem;//
    private SettingConfig.SettingPageItem curSettingItem;
    private SettingConfig.CalibrationPageItem curCalibrationItem;
    private SettingConfig.AccountPageItem curAccountItem;
    private SettingConfig.DataResetItem curDataResetItem;
    private SettingConfig.ComdOfButA curButA;

    //  Home_Page主页
    private int toggle_homeNumber;
    private int Index_homePageSwicth;
    // Setting_Page设置页
    private int toggle_settingdNumber;
    private int Index_settingPageSwitch;

    //Calibration_Page设计校验页
    private int toggle_calibrationNumber;
    private int Index_calibrationPageSwitch;
    //Account_Page 查账页
    private int toggle_accountNumber;
    private int Index_accountPageSwitch;
    //BusinessRecord_Page 营业记录页

    //TotalRecord_Page总记录页

    //DataReset_Page数据清零页
    private int toggle_dataResetNumber;
    private int Index_dataResetPageSwitch;

    //ShootingCalibration_Page射击校验页
    private int image_shootingCalibrationNumber;
    private int Index_shine;

    private int comdSwitch;//指示按钮A和按钮B功能切换
    private int set_level;//设置难度
    private int set_coin;//设置每次消耗游戏币数
    private int set_volume;//设置音量
    private int set_laguage;//设置语言
    private int watershow;
    private int set_model;//设置多少分出1票
    private int clear_coin;
    private Vector2[] calibrationVector2 = new Vector2[15];

    // Use this for initialization
    void Start()
    {
        view = transform.GetComponent<SettingView>();
        view.Init();
        toggle_homeNumber = view.homePage.homeToggleList.Count;
        toggle_settingdNumber = view.settingPage.settingToggleList.Count; ;
        toggle_calibrationNumber = view.calibrationPage.calibrationToggleList.Count; ;
        toggle_accountNumber = view.accountPage.accountToggleList.Count;
        toggle_dataResetNumber = view.dataRestPage.datarestToggleList.Count;
        image_shootingCalibrationNumber = view.shootingCalibrationPage.shootingList.Count;

        SetLanguage();
        addEvent();
        ComputeScreen();
    }

    void OnDestroy()
    {
        removeEvent();
    }

    private void ComputeScreen()
    {
        Vector3[] poses = new Vector3[4];
        Canvas canvas = view.shootingCalibrationPage.image_top_0.canvas;
        Vector2 pos = view.shootingCalibrationPage.image_top_0.transform.position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pos, null, out pos))
        {
            poses[0] = pos;
        }
        Canvas canvas1 = view.shootingCalibrationPage.image_top_4.canvas;
        Vector2 pos1 = view.shootingCalibrationPage.image_top_4.transform.position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas1.transform as RectTransform, pos1, null, out pos))
        {
            poses[1] = pos;
        }
        Canvas canvas2 = view.shootingCalibrationPage.image_buttom_0.canvas;
        Vector2 pos2 = view.shootingCalibrationPage.image_buttom_0.transform.position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas2.transform as RectTransform, pos2, null, out pos))
        {
            poses[2] = pos;
        }
        Canvas canvas3 = view.shootingCalibrationPage.image_buttom_4.canvas;
        Vector2 pos3 = view.shootingCalibrationPage.image_buttom_4.transform.position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas3.transform as RectTransform, pos3, null, out pos))
        {
            poses[3] = pos;
        }
        float screenwidth = 0;
        float screenheight = 0;
        screenwidth = ((poses[1].x + poses[3].x) - (poses[0].x + poses[2].x)) / 2.0f;
        screenheight = ((poses[0].y + poses[1].y) - (poses[2].y + poses[3].y)) / 2.0f;
        float[] info = new float[] { screenwidth, screenheight };
        Main.SettingManager.SetScreenInfo(info);
        Main.SettingManager.Save();
    }

    //设置后台语言
    private void SetLanguage()
    {
        int language = Main.SettingManager.GameLanguage;
        //Home_Page
        {
            for (int i = 0; i < view.homePage.homeTextList.Count; i++)
            {
                view.homePage.homeTextList[i].text = SettingConfig.homeName[language, i];
            }
        }
        //Setting_Page
        {
            for (int i = 0; i < view.settingPage.settingTextList.Count; i++)
            {
                view.settingPage.settingTextList[i].text = SettingConfig.setName[language, i];
            }
            view.settingPage.Text_title.text = SettingConfig.setName[language, view.settingPage.settingTextList.Count];
            view.settingPage.Text_ticketLanguage.text = SettingConfig.setTicketLanguage[language];
            if (language == 0)
            {
                view.settingPage.image_rateCoin[0].overrideSprite = view.imageFree.image_free_english.overrideSprite;
                for (int i = 0; i < view.settingPage.text_coin.Length; i++)
                {
                    view.settingPage.text_coin[i].text = SettingConfig.clearcoin[0, i];
                }
            }
            else
            {
                view.settingPage.image_rateCoin[0].overrideSprite = view.imageFree.image_free_chinese.overrideSprite;
                for (int i = 0; i < view.settingPage.text_coin.Length; i++)
                {
                    view.settingPage.text_coin[i].text = SettingConfig.clearcoin[1, i];
                }
            }

        }
        //Account_Page
        {
            for (int i = 0; i < view.accountPage.accountTextList.Count; i++)
            {
                view.accountPage.accountTextList[i].text = SettingConfig.accountName[language, i];
            }
            view.accountPage.Text_title.text = SettingConfig.accountName[language, view.accountPage.accountTextList.Count];
        }
        //Calibration_Page
        {
            for (int i = 0; i < view.calibrationPage.calibrationTextList.Count; i++)
            {
                view.calibrationPage.calibrationTextList[i].text = SettingConfig.calibrationName[language, i];
            }
            view.calibrationPage.Text_title.text = SettingConfig.calibrationName[language, view.calibrationPage.calibrationTextList.Count];
        }
        //Business_Page
        {
            for (int i = 0; i < view.businnessPage.titleList.Count; i++)
            {
                view.businnessPage.titleList[i].text = SettingConfig.businessRecordName[language, i];
            }
        }
        //DataRest_Page
        {
            for (int i = 0; i < view.dataRestPage.datarestTextList.Count; i++)
            {
                view.dataRestPage.datarestTextList[i].text = SettingConfig.dataResetName[language, i];
            }
            view.dataRestPage.Text_title.text = SettingConfig.dataResetName[language, view.dataRestPage.datarestTextList.Count];
        }
        //Total_Page
        {
            for (int i = 0; i < view.totalPage.titleList.Count; i++)
            {
                view.totalPage.titleList[i].text = SettingConfig.totalRecordName[language, i];
            }
        }
    }

    //查询当前页
    private void FindPage()
    {
        if (view.Home_Page.activeSelf)
        {
            curPage = SettingConfig.Page.HomePage;
        }
        if (view.Setting_Page.activeSelf)
        {
            curPage = SettingConfig.Page.SettingPage;
        }
        if (view.Calibration_Page.activeSelf)
        {
            curPage = SettingConfig.Page.CalibrationPage;
        }
        if (view.Account_Page.activeSelf)
        {
            curPage = SettingConfig.Page.AccountPage;
        }
        if (view.BusinessRecords_Page.activeSelf)
        {
            curPage = SettingConfig.Page.BusinessRecord_Page;
        }
        if (view.TotalRecord_Page.activeSelf)
        {
            curPage = SettingConfig.Page.TotalRecord_Page;
        }
        if (view.DataReset_Page.activeSelf)
        {
            curPage = SettingConfig.Page.DataReset_Page;
        }
        if (view.ShootingCalibration_Page.activeSelf)
        {
            curPage = SettingConfig.Page.ShootingCalibration_Page;
        }
    }
    #region---------------------------按钮A_Start---------------------------------
    private void ComdButtonA()
    {
        FindPage();
        //主页操作
        if (curPage == SettingConfig.Page.HomePage)
        {
            if (curHomeItem == SettingConfig.HomePageItem.Set)  //进入SettingPage
            {
                view.Home_Page.SetActive(false);
                view.Setting_Page.SetActive(true);
                SetSetPageInfo();
                comdSwitch = 0;
            }
            if (curHomeItem == SettingConfig.HomePageItem.Account)//进入AccountPage
            {
                view.Home_Page.SetActive(false);
                view.Account_Page.SetActive(true);
            }
            if (curHomeItem == SettingConfig.HomePageItem.Exit)//退出后台
            {
                Main.GameMode.ReturnStart();
            }
        }
        //设置页操作
        if (curPage == SettingConfig.Page.SettingPage)
        {
            ++comdSwitch;
            comdSwitch %= 2;
            if (curSettingItem == SettingConfig.SettingPageItem.Money ||
                curSettingItem == SettingConfig.SettingPageItem.Volume ||
                curSettingItem == SettingConfig.SettingPageItem.Difficulity ||
                curSettingItem == SettingConfig.SettingPageItem.Lanuage ||
                curSettingItem == SettingConfig.SettingPageItem.OutTicket ||
                curSettingItem == SettingConfig.SettingPageItem.WaterShow ||
                curSettingItem == SettingConfig.SettingPageItem.ClearCoin)
            {
                if (comdSwitch == 1)
                {
                    curButA = SettingConfig.ComdOfButA.Sure;
                    view.settingPage.settingImageList[(int)curSettingItem].color = new Color(1, 0, 0);
                }
                else
                {
                    curButA = SettingConfig.ComdOfButA.Enter;
                    view.settingPage.settingImageList[(int)curSettingItem].color = new Color(1, 1, 1);
                }
            }

            if (curButA == SettingConfig.ComdOfButA.Enter)
            {
                switch (curSettingItem)
                {
                    case SettingConfig.SettingPageItem.Money:
                        Main.SettingManager.GameRate = set_coin;
                        Main.SettingManager.Save();
                        break;
                    case SettingConfig.SettingPageItem.Volume:
                        Main.SettingManager.GameVolume = set_volume;
                        Main.SettingManager.Save();
                        break;
                    case SettingConfig.SettingPageItem.Difficulity:
                        Main.SettingManager.GameLevel = set_level;
                        Main.SettingManager.Save();
                        break;
                    case SettingConfig.SettingPageItem.Lanuage:
                        Main.SettingManager.GameLanguage = set_laguage;
                        Main.SettingManager.Save();
                        SetLanguage();
                        break;
                    case SettingConfig.SettingPageItem.OutTicket:
                        Main.SettingManager.TicketModel = set_model;
                        Main.SettingManager.TicketScore = SettingConfig.scorePreTicket[set_model];
                        Main.SettingManager.Save();
                        break;
                    case SettingConfig.SettingPageItem.WaterShow:
                        Main.SettingManager.WaterShow = watershow;
                        Main.SettingManager.Save();
                        break;
                    case SettingConfig.SettingPageItem.ClearCoin:
                        for (int i = 0; i < GameConfig.GAME_CONFIG_PER_USE_COIN; i++ )
                        {
                            Main.SettingManager.ClearCoin(i);
                        }
                        Main.SettingManager.Save();
                        break;
                }
            }
            if (curSettingItem == SettingConfig.SettingPageItem.Exit)
            {
                view.Setting_Page.SetActive(false);
                view.Home_Page.SetActive(true);
            }
            if (curSettingItem == SettingConfig.SettingPageItem.Calibration)
            {
                view.Setting_Page.SetActive(false);
                view.Calibration_Page.SetActive(true);
            }
        }
        //查账页操作
        if (curPage == SettingConfig.Page.AccountPage)
        {
            if (curAccountItem == SettingConfig.AccountPageItem.BusinessRecords)
            {
                view.Account_Page.SetActive(false);
                view.BusinessRecords_Page.SetActive(true);
                SetBusinessPageInfo();
            }
            if (curAccountItem == SettingConfig.AccountPageItem.TotalRecords)
            {
                view.Account_Page.SetActive(false);
                view.TotalRecord_Page.SetActive(true);
                SetTotalRecordInfo();
            }
            if (curAccountItem == SettingConfig.AccountPageItem.DataReset)
            {
                view.Account_Page.SetActive(false);
                view.DataReset_Page.SetActive(true);
            }
            if (curAccountItem == SettingConfig.AccountPageItem.Exit)
            {
                view.Account_Page.SetActive(false);
                view.Home_Page.SetActive(true);
            }
        }
        //射击校验页操作
        if (curPage == SettingConfig.Page.CalibrationPage)
        {
            if (curCalibrationItem == SettingConfig.CalibrationPageItem.Player1)
            {
                view.Calibration_Page.SetActive(false);
                view.ShootingCalibration_Page.SetActive(true);
                // 为了方便硬件人员，使用SetPlayerGameBegine替代ShootCalibration
                //Main.IOManager.ShootCalibration(0, true);
                Main.IOManager.SetPlayerGameBegine(0, true);
                Main.IOManager.SetPlayerGameEnd(0, false);
            }
            if (curCalibrationItem == SettingConfig.CalibrationPageItem.Player2)
            {
                view.Calibration_Page.SetActive(false);
                view.ShootingCalibration_Page.SetActive(true);
                // 为了方便硬件人员，使用SetPlayerGameBegine替代ShootCalibration
                //Main.IOManager.ShootCalibration(1, true);
                Main.IOManager.SetPlayerGameBegine(1, true);
                Main.IOManager.SetPlayerGameEnd(1, false);
            }
            if (curCalibrationItem == SettingConfig.CalibrationPageItem.Player3)
            {
                view.Calibration_Page.SetActive(false);
                view.ShootingCalibration_Page.SetActive(true);
                // 为了方便硬件人员，使用SetPlayerGameBegine替代ShootCalibration
                //Main.IOManager.ShootCalibration(2, true);
                Main.IOManager.SetPlayerGameBegine(2, true);
                Main.IOManager.SetPlayerGameEnd(2, false);
            }
            if (curCalibrationItem == SettingConfig.CalibrationPageItem.Exit)
            {
                view.Calibration_Page.SetActive(false);
                view.Setting_Page.SetActive(true);
                comdSwitch = 0;
            }
            Index_shine = 0;
            view.shootingCalibrationPage.shootingList[0].color = new Color(1, 0, 0);
        }
        //查账记录页
        if (curPage == SettingConfig.Page.BusinessRecord_Page)
        {
            view.BusinessRecords_Page.SetActive(false);
            view.Account_Page.SetActive(true);
        }
        //总记录页
        if (curPage == SettingConfig.Page.TotalRecord_Page)
        {
            view.TotalRecord_Page.SetActive(false);
            view.Account_Page.SetActive(true);
        }
        //数据清零页
        if (curPage == SettingConfig.Page.DataReset_Page)
        {
            if (curDataResetItem == SettingConfig.DataResetItem.Yes)
            {
                Main.SettingManager.ClearMonthInfo();
                Main.SettingManager.ClearTotalRecord();
                view.DataReset_Page.SetActive(false);
                view.Home_Page.SetActive(true);
            }
            if (curDataResetItem == SettingConfig.DataResetItem.No)
            {
                view.DataReset_Page.SetActive(false);
                view.Home_Page.SetActive(true);
            }
        }
        //射击校验页
        if (curPage == SettingConfig.Page.ShootingCalibration_Page)
        {
            calibrationVector2[Index_shine] = Main.IOManager.GetRockerBar((int)curCalibrationItem);
            ++Index_shine;
            for (int i = 0; i < view.shootingCalibrationPage.shootingList.Count; i++)
            {
                if (Index_shine == i)
                {
                    view.shootingCalibrationPage.shootingList[i].color = new Color(1, 0, 0);
                }
                else
                {
                    view.shootingCalibrationPage.shootingList[i].color = new Color(1, 1, 1);
                }
            }

            if (Index_shine >= view.shootingCalibrationPage.shootingList.Count)
            {
                for (int i = 0; i < GameConfig.GAME_CONFIG_PLAYER_COUNT; i++)
                {
                    //设置playerIndex为游戏结束状态 
                    Main.IOManager.SetPlayerGameEnd(i, true);
                    Main.IOManager.SetPlayerGameBegine(i, false);
                }
                view.ShootingCalibration_Page.SetActive(false);
                view.Calibration_Page.SetActive(true);
                ShootingCalibrationOp((int)curCalibrationItem);
            }

        }
    }
    //射击校验操作
    private void ShootingCalibrationOp(int playerid)
    {
        for (int i = 0;i < Main.SettingManager.GetPointX(playerid).Length;i++)
        {
            Main.SettingManager.SetPointX(playerid, i, calibrationVector2[i].x);
        }

        for (int i = 0; i < Main.SettingManager.GetPointY(playerid).Length; i++)
        {
            Main.SettingManager.SetPointY(playerid, i, calibrationVector2[i].y);
        }

        Main.SettingManager.Save();
    }

    private void SetSetPageInfo()
    {
        //Main.SettingManager.GetSetInfo();
        set_coin = Main.SettingManager.GameRate;
        set_volume = Main.SettingManager.GameVolume;
        set_laguage = Main.SettingManager.GameLanguage;
        set_level = Main.SettingManager.GameLevel;
        set_model = SettingConfig.scorePreTicket[Main.SettingManager.TicketModel];
        watershow = Main.SettingManager.WaterShow;
        for (int i = 0; i < view.settingPage.image_rateCoin.Length; i++)
        {
            if (i == Main.SettingManager.GameRate)
            {
                view.settingPage.image_rateCoin[i].color = new Color(1, 0, 0);
            }
            else
            {
                view.settingPage.image_rateCoin[i].color = new Color(1, 1, 1);

            }
        }
        for (int i = 0; i < view.settingPage.image_language.Length; i++)
        {
            if (Main.SettingManager.GameLanguage == i)
            {
                view.settingPage.image_language[i].color = new Color(1, 0, 0);
            }
            else
            {
                view.settingPage.image_language[i].color = new Color(1, 1, 1);
            }
        }

        for (int i = 0; i < view.settingPage.image_water.Length; i++)
        {
            if (Main.SettingManager.WaterShow == i)
            {
                view.settingPage.image_water[i].color = new Color(1, 0, 0);
            }
            else
            {
                view.settingPage.image_water[i].color = new Color(1, 1, 1);
            }
        }
        view.settingPage.slider_volume.value = (float)set_volume / SettingConfig.volume.Length;
        view.settingPage.Text_volume.text = Main.SettingManager.GameVolume.ToString();
        view.settingPage.Text_Difficulty.text = set_level.ToString();
        view.settingPage.Text_ticket.text = Main.SettingManager.TicketScore.ToString();
    }

    // 设置查账信息
    private void SetBusinessPageInfo()
    {
        int language = Main.SettingManager.GameLanguage;
        for (int i = 0; i < Main.SettingManager.GetMonthData().Count; i++ )
        {
            view.businnessPage.monthList[i].text            = Main.SettingManager.GetMonthData(i)[0].ToString();
            view.businnessPage.coinsmonthList[i].text       = Main.SettingManager.GetMonthData(i)[1].ToString();
            view.businnessPage.ticketsmonthList[i].text     = Main.SettingManager.GetMonthData(i)[2].ToString();
            view.businnessPage.gametimemonthList[i].text    = HelperTool.DoTimeFormat(language, (int)Main.SettingManager.GetMonthData(i)[3]);
            view.businnessPage.uptimemonthList[i].text      = HelperTool.DoTimeFormat(language, (int)Main.SettingManager.GetMonthData(i)[4]);
        }

        for (int i = 0; i < view.businnessPage.titleList.Count; i++ )
        {
            view.businnessPage.titleList[i].text = SettingConfig.businessRecordName[language, i];
        }
    }

    // 设置总记录信息
    private void SetTotalRecordInfo()
    {
        int language = Main.SettingManager.GameLanguage;
        for (int i = 0; i < Main.SettingManager.TotalRecord().Length;i++ )
        {
            if (i == 1 || i == 2)
            {
                view.totalPage.valueList[i].text = HelperTool.DoTimeFormat(language, (int)Main.SettingManager.TotalRecord()[i]);
            }
            else
            {
                view.totalPage.valueList[i].text = Main.SettingManager.TotalRecord()[i].ToString();
            }
           
        }

        for (int i = 0; i < view.totalPage.titleList.Count; i++ )
        {
            view.totalPage.titleList[i].text = SettingConfig.totalRecordName[language, i].ToString();
        }
    }

    #endregion---------------------------按钮A_End---------------------------------


    #region---------------------按钮B_Start---------------------------------------------
    private void ComdButtonB()
    {
        FindPage();
        if (curPage == SettingConfig.Page.HomePage)
        {
            SwitchHomeItem();
        }
        if (curPage == SettingConfig.Page.SettingPage)
        {
            SwitchSettingItem();
        }
        if (curPage == SettingConfig.Page.AccountPage)
        {
            SwitchAccountItem();
        }
        if (curPage == SettingConfig.Page.DataReset_Page)
        {
            SwitchDataRestItem();
        }
        if (curPage == SettingConfig.Page.CalibrationPage)
        {
            SwitchCalibrationItem();
        }

    }

    private void SwitchHomeItem()
    {
        ++Index_homePageSwicth;
        Index_homePageSwicth %= toggle_homeNumber;
        int temp = 0;
        for (int i = 0; i < view.homePage.homeToggleList.Count; i++ )
        {
            if (Index_homePageSwicth == temp)
            {
                view.homePage.homeToggleList[i].isOn = true;
                view.homePage.homeTextList[i].color = new Color(1, 0, 0);
                curHomeItem = (SettingConfig.HomePageItem)Index_homePageSwicth;
            }
            else
            {
                view.homePage.homeToggleList[i].isOn = false;
                view.homePage.homeTextList[i].color = new Color(1, 1, 1);
            }
            ++temp;
        }
        
    }
    private void SwitchSettingItem()
    {
        if (curButA == SettingConfig.ComdOfButA.Enter)
        {
            ++Index_settingPageSwitch;
            Index_settingPageSwitch %= toggle_settingdNumber;
            int temp = 0;

            for (int i = 0; i < view.settingPage.settingToggleList.Count; i++ )
            {
                 if (Index_settingPageSwitch == temp)
                 {
                     view.settingPage.settingToggleList[i].isOn = true;
                     view.settingPage.settingTextList[i].color = new Color(1, 0, 0);
                     curSettingItem = (SettingConfig.SettingPageItem)Index_settingPageSwitch;
                 }
                 else
                 {
                     view.settingPage.settingToggleList[i].isOn = false;
                     view.settingPage.settingTextList[i].color = new Color(1, 1, 1);
                 }
                 ++temp;
            }
          
        }

        if (curButA == SettingConfig.ComdOfButA.Sure)//按钮A:按下可以锁定当前选项卡，以便按钮B进行数据调整
        {
            //调整比率
            if (curSettingItem == SettingConfig.SettingPageItem.Money)
            {
                ++set_coin;
                set_coin %= SettingConfig.money.Length;
                for (int i = 0; i < view.settingPage.image_rateCoin.Length; i++)
                {
                    if (i == set_coin)
                    {
                        view.settingPage.image_rateCoin[i].color = new Color(1, 0, 0);
                    }
                    else
                    {
                        view.settingPage.image_rateCoin[i].color = new Color(1, 1, 1);
                    }
                }
            }
            //调整声音
            if (curSettingItem == SettingConfig.SettingPageItem.Volume)
            {
                ++set_volume;
                set_volume %= (SettingConfig.volume.Length + 1);
                view.settingPage.Text_volume.text = set_volume.ToString();
                view.settingPage.slider_volume.value = (float)set_volume / SettingConfig.volume.Length;
            }
            //调整难度
            if (curSettingItem == SettingConfig.SettingPageItem.Difficulity)
            {
                ++set_level;
                set_level %= 4;
                if (set_level == 0)
                {
                    set_level = 1;
                }
                view.settingPage.Text_Difficulty.text = set_level.ToString();
            }
            //调整语言
            if (curSettingItem == SettingConfig.SettingPageItem.Lanuage)
            {
                ++set_laguage;
                set_laguage %= SettingConfig.language.Length;
                int temp = 0;

                for (int i = 0; i < view.settingPage.image_language.Length; i++ )
                {
                    if (set_laguage == temp)
                    {
                        view.settingPage.image_language[i].color = new Color(1, 0, 0);
                    }
                    else
                    {
                        view.settingPage.image_language[i].color = new Color(1, 1, 1);
                    }
                    ++temp;
                }

            }
            //
            if (curSettingItem == SettingConfig.SettingPageItem.WaterShow)
            {
                ++watershow;
                watershow %= SettingConfig.watershow.Length;
                int temp = 0;

                for (int i = 0; i < view.settingPage.image_water.Length; i++ )
                {
                    if (watershow == temp)
                    {
                        view.settingPage.image_water[i].color = new Color(1, 0, 0);
                    }
                    else
                    {
                        view.settingPage.image_water[i].color = new Color(1, 1, 1);
                    }
                    ++temp;
                }
            }
            //调整出票分数
            if (curSettingItem == SettingConfig.SettingPageItem.OutTicket)
            {
                ++set_model;
                set_model %= SettingConfig.scorePreTicket.Length;
                view.settingPage.Text_ticket.text = SettingConfig.scorePreTicket[set_model].ToString();
            }
            // 清除投币
            if (curSettingItem == SettingConfig.SettingPageItem.ClearCoin)
            {
                ++clear_coin;
                clear_coin %= view.settingPage.text_coin.Length;
                int temp = 0;

                for (int i = 0; i < view.settingPage.text_coin.Length; i ++ )
                {
                    if (clear_coin == temp)
                    {
                        view.settingPage.text_coin[i].color = new Color(1, 0, 0);
                    }
                    else
                    {
                        view.settingPage.text_coin[i].color = new Color(1, 1, 1);
                    }
                    ++temp;
                }
            }
        }
    }

    private void SwitchAccountItem()
    {
        ++Index_accountPageSwitch;
        Index_accountPageSwitch %= toggle_accountNumber;
        int temp = 0;

        for (int i = 0; i < view.accountPage.accountToggleList.Count; i++ )
        {
             if (Index_accountPageSwitch == temp)
             {
                 view.accountPage.accountToggleList[i].isOn = true;
                 view.accountPage.accountTextList[i].color = new Color(1, 0, 0);
                 curAccountItem = (SettingConfig.AccountPageItem)Index_accountPageSwitch;
             }
             else
             {
                 view.accountPage.accountToggleList[i].isOn = false;
                 view.accountPage.accountTextList[i].color = new Color(1, 1, 1);
             }
            ++temp;
        }

    }
    private void SwitchDataRestItem()
    {
        ++Index_dataResetPageSwitch;
        Index_dataResetPageSwitch %= toggle_dataResetNumber;
        int temp = 0;

        for (int i = 0; i < view.dataRestPage.datarestToggleList.Count; i++ )
        {
            if (Index_dataResetPageSwitch == temp)
            {
                view.dataRestPage.datarestToggleList[i].isOn = true;
                view.dataRestPage.datarestTextList[i].color = new Color(1, 0, 0);
                curDataResetItem = (SettingConfig.DataResetItem)Index_dataResetPageSwitch;
            }
            else
            {
                view.dataRestPage.datarestToggleList[i].isOn = false;
                view.dataRestPage.datarestTextList[i].color = new Color(1, 1, 1);
            }
            ++temp;
        }
    }
    private void SwitchCalibrationItem()
    {
        ++Index_calibrationPageSwitch;
        Index_calibrationPageSwitch %= toggle_calibrationNumber;
        int temp = 0;

        for (int i = 0; i < view.calibrationPage.calibrationToggleList.Count; i++ )
        {
            if (Index_calibrationPageSwitch == temp)
            {
                view.calibrationPage.calibrationToggleList[i].isOn = true;
                view.accountPage.accountTextList[i].color = new Color(1, 0, 0);
                curCalibrationItem = (SettingConfig.CalibrationPageItem)Index_calibrationPageSwitch;
            }
            else
            {
                view.calibrationPage.calibrationToggleList[i].isOn = false;
                view.accountPage.accountTextList[i].color = new Color(1, 1, 1);
            }
            ++temp;
        }
    }
    #endregion---------------------按钮B_End---------------------------------------------
    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// 添加逻辑监听
    /// </summary>
    protected virtual void addEvent()
    {
        EventDispatcher.AddEventListener(GameEventDef.EVNET_SETTING_CONFIRM, ComdButtonA);
        EventDispatcher.AddEventListener(GameEventDef.EVNET_SETTING_SELECT, ComdButtonB);
    }

    /// <summary>
    /// 移除逻辑事件监听
    /// </summary>
    protected virtual void removeEvent()
    {
        EventDispatcher.RemoveEventListener(GameEventDef.EVNET_SETTING_CONFIRM, ComdButtonA);
        EventDispatcher.RemoveEventListener(GameEventDef.EVNET_SETTING_SELECT, ComdButtonB);
    }
}

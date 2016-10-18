using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BattleHeadUIView : MonoBehaviour {

    [HideInInspector]
    public Image image_Ground;
    [HideInInspector]
    public Image panel_GreenHPBar;
    [HideInInspector]
    public Image panel_RedHPBar;
    [HideInInspector]
    public Image panel_FloatArtNumber;
    [HideInInspector]
    public GameObject ui_Parent;
    [HideInInspector]
    public Image panel_BossHPBar;
    [HideInInspector]
    public Slider slider_BossHPBar1;
    [HideInInspector]
    public Slider slider_BossHPBar2;
    [HideInInspector]
    public Slider slider_BossHPBar3;
    [HideInInspector]
    public Image image_BossHPNum1;
    [HideInInspector]
    public Image image_BossHPNum2;
    [HideInInspector]
    public Image image_BossHPNum3;
    public List<Image> image_Numbers;
    [HideInInspector]
    public Image panel_CrackScreen;
    [HideInInspector]
    public List<Image> image_CrackScreens;
    [HideInInspector]
    public Image panel_HittingTarget1;
    [HideInInspector]
    public Image panel_HittingTarget2;
    [HideInInspector]
    public Image panel_HittingTarget3;
    [HideInInspector]
    public GameObject panel_Fx;
    [HideInInspector]
    public Image panel_ExplodeStar;
    [HideInInspector]
    public GameObject panel_Weapon;
    [HideInInspector]
    public GameObject image_hit;
    [HideInInspector]
    public GameObject image_hit_en;
    [HideInInspector]
    public Image panel_RescueHint;
    [HideInInspector]
    public Image panel_RescueHint_En;
    [HideInInspector]
    public Image panel_HitBoss;
    [HideInInspector]
    public List<Image> image_Stars;
    [HideInInspector]
    public List<Image> rescuePosList; 
	// Use this for initialization
    public void Init()
    {
        panel_GreenHPBar = transform.Find("Panel_GreenHPBar").GetComponent<Image>();
        panel_RedHPBar = transform.Find("Panel_RedHPBar").GetComponent<Image>();
        panel_FloatArtNumber = transform.Find("Panel_FloatArtNumber").GetComponent<Image>();
        ui_Parent = transform.gameObject;
        panel_BossHPBar = transform.Find("Panel_BossHPBar").GetComponent<Image>();
        slider_BossHPBar1 = transform.Find("Panel_BossHPBar/Image_HPBar1").GetComponent<Slider>();
        slider_BossHPBar2 = transform.Find("Panel_BossHPBar/Image_HPBar2").GetComponent<Slider>();
        slider_BossHPBar3 = transform.Find("Panel_BossHPBar/Image_HPBar3").GetComponent<Slider>();
        image_BossHPNum1 = transform.Find("Panel_BossHPBar/Image_HPNum1").GetComponent<Image>();
        image_BossHPNum2 = transform.Find("Panel_BossHPBar/Image_HPNum2").GetComponent<Image>();
        image_BossHPNum3 = transform.Find("Panel_BossHPBar/Image_HPNum3").GetComponent<Image>();
        panel_CrackScreen = transform.Find("Panel_CrackScreen").GetComponent<Image>();
        image_CrackScreens = new List<Image>();
        image_CrackScreens.Add(transform.Find("Panel_CrackScreen/Image_CrackScreen1").GetComponent<Image>());
        image_CrackScreens.Add(transform.Find("Panel_CrackScreen/Image_CrackScreen2").GetComponent<Image>());
        image_CrackScreens.Add(transform.Find("Panel_CrackScreen/Image_CrackScreen3").GetComponent<Image>());
        panel_HittingTarget1 = transform.Find("Panel_Hitting1").GetComponent<Image>();
        panel_HittingTarget2 = transform.Find("Panel_Hitting2").GetComponent<Image>();
        panel_HittingTarget3 = transform.Find("Panel_Hitting3").GetComponent<Image>();
        panel_Fx = transform.Find("Panel_FX").gameObject;
        panel_ExplodeStar = transform.Find("Panel_FX/Panel_ExplodeStar").GetComponent<Image>();
        image_Stars.Add(transform.Find("Panel_FX/Panel_Star1").GetComponent<Image>());
        image_Stars.Add(transform.Find("Panel_FX/Panel_Star2").GetComponent<Image>());
        image_Stars.Add(transform.Find("Panel_FX/Panel_Star3").GetComponent<Image>());
        image_Stars.Add(transform.Find("Panel_FX/Panel_Star4").GetComponent<Image>());
        panel_Weapon = transform.Find("Panel_Weapon").gameObject;
        image_hit = panel_Weapon.transform.Find("Image_Hit").gameObject;
        image_hit_en = panel_Weapon.transform.Find("Image_Hit_En").gameObject;
        panel_RescueHint = transform.Find("Panel_RescueHint").GetComponent<Image>();
        panel_RescueHint_En = transform.Find("Panel_RescueHint_En").GetComponent<Image>();
        panel_HitBoss = transform.Find("Panel_HitBoss").GetComponent<Image>();

        rescuePosList = new List<Image>();
        for (int i = 0; i < 3; i++ )
        {
            rescuePosList.Add(transform.Find("Panel_RescueHitPoint" + i.ToString()).GetComponent<Image>());
        }
    }

}

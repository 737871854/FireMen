using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Need.Mx
{
    public class StartView : MonoBehaviour
    {
        [HideInInspector]
        public Image image_P1CoinNumber1;
        [HideInInspector]
        public Image image_P1CoinNumber2;
        [HideInInspector]
        public Image image_P1CoinNumber3;
        [HideInInspector]
        public Image image_P2CoinNumber1;
        [HideInInspector]
        public Image image_P2CoinNumber2;
        [HideInInspector]
        public Image image_P2CoinNumber3;
        [HideInInspector]
        public Image image_P3CoinNumber1;
        [HideInInspector]
        public Image image_P3CoinNumber2;
        [HideInInspector]
        public Image image_P3CoinNumber3;
        public List<Image> image_Numbers;


        [HideInInspector]
        public Image image_panel0_coin_chinese;
        [HideInInspector]
        public Image image_panel1_coin_chinese;
        [HideInInspector]
        public Image image_panel2_coin_chinese;
        [HideInInspector]
        public Image image_logo_chinese;

        [HideInInspector]
        public Image image_panel0_coin_english;
        [HideInInspector]
        public Image image_panel1_coin_english;
        [HideInInspector]
        public Image image_panel2_coin_english;
        [HideInInspector]
        public Image image_logo_english;

        [HideInInspector]
        public GameObject low_water;
        [HideInInspector]
        public GameObject hight_water;

        // Use this for initialization
        public void Init()
        {
            image_P1CoinNumber1 = transform.Find("Panel_P1/Image_CoinNum1").GetComponent<Image>();
            image_P1CoinNumber2 = transform.Find("Panel_P1/Image_CoinNum2").GetComponent<Image>();
            image_P1CoinNumber3 = transform.Find("Panel_P1/Image_CoinNum3").GetComponent<Image>();
            image_P2CoinNumber1 = transform.Find("Panel_P2/Image_CoinNum1").GetComponent<Image>();
            image_P2CoinNumber2 = transform.Find("Panel_P2/Image_CoinNum2").GetComponent<Image>();
            image_P2CoinNumber3 = transform.Find("Panel_P2/Image_CoinNum3").GetComponent<Image>();
            image_P3CoinNumber1 = transform.Find("Panel_P3/Image_CoinNum1").GetComponent<Image>();
            image_P3CoinNumber2 = transform.Find("Panel_P3/Image_CoinNum2").GetComponent<Image>();
            image_P3CoinNumber3 = transform.Find("Panel_P3/Image_CoinNum3").GetComponent<Image>();

            image_panel0_coin_chinese = transform.Find("Panel_P1/Image_CoinText").GetComponent<Image>();
            image_panel1_coin_chinese = transform.Find("Panel_P2/Image_CoinText").GetComponent<Image>();
            image_panel2_coin_chinese = transform.Find("Panel_P3/Image_CoinText").GetComponent<Image>();
            image_logo_chinese = transform.Find("Image_Title").GetComponent<Image>();

            image_panel0_coin_english = transform.Find("Panel_P1/Image_CoinText1").GetComponent<Image>();
            image_panel1_coin_english = transform.Find("Panel_P2/Image_CoinText1").GetComponent<Image>();
            image_panel2_coin_english = transform.Find("Panel_P3/Image_CoinText1").GetComponent<Image>();
            image_logo_english = transform.Find("Image_Title1").GetComponent<Image>();

            low_water = transform.parent.Find("LowWater").gameObject;
            hight_water = transform.parent.Find("HightWater").gameObject;
        }
    }
}

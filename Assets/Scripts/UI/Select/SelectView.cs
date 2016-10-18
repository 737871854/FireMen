using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Need.Mx
{
    public class SelectView : MonoBehaviour
    {
        [HideInInspector]
        public Image image_P1CoinNumber1;
        [HideInInspector]
        public Image image_P1CoinNumber2;
        [HideInInspector]
        public Image image_P1CoinNumber3;
        [HideInInspector]
        public Image image_P1Role;
        [HideInInspector]
        public Image image_P1RoleGrey;
        [HideInInspector]
        public Image image_P2CoinNumber1;
        [HideInInspector]
        public Image image_P2CoinNumber2;
        [HideInInspector]
        public Image image_P2CoinNumber3;
        [HideInInspector]
        public Image image_P2Role;
        [HideInInspector]
        public Image image_P2RoleGrey;
        [HideInInspector]
        public Image image_P3CoinNumber1;
        [HideInInspector]
        public Image image_P3CoinNumber2;
        [HideInInspector]
        public Image image_P3CoinNumber3;
        [HideInInspector]
        public Image image_P3Role;
        [HideInInspector]
        public Image image_P3RoleGrey;
        [HideInInspector]
        public Image image_Time1;
        [HideInInspector]
        public Image image_Time2;

        [HideInInspector]
        public Image image_coin_chinese0;
        [HideInInspector]
        public Image image_coin_chinese1;
        [HideInInspector]
        public Image image_coin_chinese2;
        [HideInInspector]
        public Image image_coin_english0;
        [HideInInspector]
        public Image image_coin_english1;
        [HideInInspector]
        public Image image_coin_english2;
        [HideInInspector]
        public Image image_wait_english0;
        [HideInInspector]
        public Image image_wait_english1;
        [HideInInspector]
        public Image image_wait_english2;


        public List<Image> image_Times;
        public List<Image> image_Numbers;

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
           image_P1Role        = transform.Find("Panel_P1/Image_Select").GetComponent<Image>();
           image_P1RoleGrey    = transform.Find("Panel_P1/Image_Wait").GetComponent<Image>();
           image_P2CoinNumber1 = transform.Find("Panel_P2/Image_CoinNum1").GetComponent<Image>();
           image_P2CoinNumber2 = transform.Find("Panel_P2/Image_CoinNum2").GetComponent<Image>();
           image_P2CoinNumber3 = transform.Find("Panel_P2/Image_CoinNum3").GetComponent<Image>();
           image_P2Role        = transform.Find("Panel_P2/Image_Select").GetComponent<Image>();
           image_P2RoleGrey    = transform.Find("Panel_P2/Image_Wait").GetComponent<Image>();
           image_P3CoinNumber1 = transform.Find("Panel_P3/Image_CoinNum1").GetComponent<Image>();
           image_P3CoinNumber2 = transform.Find("Panel_P3/Image_CoinNum2").GetComponent<Image>();
           image_P3CoinNumber3 = transform.Find("Panel_P3/Image_CoinNum3").GetComponent<Image>();
           image_P3Role        = transform.Find("Panel_P3/Image_Select").GetComponent<Image>();
           image_P3RoleGrey    = transform.Find("Panel_P3/Image_Wait").GetComponent<Image>();
           image_Time1         = transform.Find("Panel_Common/Image_Value1").GetComponent<Image>();
           image_Time2         = transform.Find("Panel_Common/Image_Value2").GetComponent<Image>();

           image_coin_chinese0 = transform.Find("Panel_P1/Image_CoinText").GetComponent<Image>();
           image_coin_chinese1 = transform.Find("Panel_P2/Image_CoinText").GetComponent<Image>();
           image_coin_chinese2 = transform.Find("Panel_P3/Image_CoinText").GetComponent<Image>();
           image_coin_english0 = transform.Find("Panel_P1/Image_CoinText1").GetComponent<Image>();
           image_coin_english1 = transform.Find("Panel_P2/Image_CoinText1").GetComponent<Image>();
           image_coin_english2 = transform.Find("Panel_P3/Image_CoinText1").GetComponent<Image>();
           image_wait_english0 = transform.Find("Panel_P1/Image_Wait1").GetComponent<Image>();
           image_wait_english1 = transform.Find("Panel_P2/Image_Wait1").GetComponent<Image>();
           image_wait_english2 = transform.Find("Panel_P3/Image_Wait1").GetComponent<Image>();

           low_water = transform.parent.Find("LowWater").gameObject;
           hight_water = transform.parent.Find("HightWater").gameObject;
        }
    }
}

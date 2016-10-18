using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Need.Mx
{
    public class WaterView : MonoBehaviour
    {
        [HideInInspector]
        public Image panel_Red;
        [HideInInspector]
        public Image panel_Tutorial;
        [HideInInspector]
        public Image panel_Handle;
        [HideInInspector]
        public Image image_Splash;
        [HideInInspector]
        public Slider slider_Progress;
        [HideInInspector]
        public Animator animator_WaterColumn;
        [HideInInspector]
        public Animator animator_WaterMan;
        [HideInInspector]
        public Image image_PullText;
        [HideInInspector]
        public Image image_Reward;
        [HideInInspector]
        public Image image_Reward_En;

        [HideInInspector]
        public Image image_water_backround_chinese;
        [HideInInspector]
        public Image image_water_backround_english;

        [HideInInspector]
        public GameObject image_pullHint;
        [HideInInspector]
        public GameObject image_pullHint_En;

		[HideInInspector]
		public Image image_number0;
		[HideInInspector]
		public Image image_number1;

		public List<Image> numList;

        // Use this for initialization
        public void Init()
        {
            panel_Tutorial = transform.Find("Panel_Tutorial").GetComponent<Image>();
            panel_Handle = transform.Find("Panel_Handle").GetComponent<Image>();
            image_Splash = transform.Find("Panel_Handle/Image_Splash").GetComponent<Image>();
            panel_Red = transform.Find("Panel_Handle/Image_Red").GetComponent<Image>();
            slider_Progress = transform.Find("Panel_Handle/Image_Progress").GetComponent<Slider>();
            animator_WaterColumn = transform.Find("Panel_Handle/Image_Column").GetComponent<Animator>();
            animator_WaterMan = transform.Find("Panel_Handle/Image_Man").GetComponent<Animator>();
            image_PullText = transform.Find("Panel_Handle/Image_PullText").GetComponent<Image>();
            image_Reward = transform.Find("Panel_Handle/Image_Reward").GetComponent<Image>();
            image_Reward_En = transform.Find("Panel_Handle/Image_Reward_EN").GetComponent<Image>();

            image_water_backround_chinese = transform.Find("Panel_Tutorial/Image_Tutorial_Chinese").GetComponent<Image>();
            image_water_backround_english = transform.Find("Panel_Tutorial/Image_Tutorial_English").GetComponent<Image>();

            image_pullHint = transform.Find("Panel_Handle/Image_PullHint").gameObject;
            image_pullHint_En = transform.Find("Panel_Handle/Image_PullHint_EN").gameObject;

			image_number0 = transform.Find ("Panel_Handle/Image_Number0").GetComponent<Image> ();
			image_number1 = transform.Find ("Panel_Handle/Image_Number1").GetComponent<Image> ();
        }
    }
}

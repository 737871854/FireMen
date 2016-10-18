
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Need.Mx
{
    public class PlayView : MonoBehaviour
    {
        public struct PlayerPanel
        {
            public Image image_Target;
            public Image image_Head1;
            public Image image_Head2;
            public Image image_HeadGround;
            public Image image_HeadGround4;
            public Slider image_TimerBar;
            public Image image_TimerWarning;
            public Image image_Head1Grey;
            public Image image_image_HeadGroundGrey;
            public Image image_HeadGround4Grey;
            public Image image_NumberSlot1;
            public Image image_NumberSlot2;
            public Image image_NumberSlot3;
            public Image image_NumberSlot4;
            public Image image_NumberSlot5;
            //public Image image_TimerSlot1;
            //public Image image_TimerSlot2;
            //public Image image_TimerSlot3;
            //public Image image_TimerSlot4;
            //public Image image_TimerSlot5;
            public Image image_CoinText1;
            public Image image_CoinText1_En;
            public Image image_CoinText2;
            public Image image_CoinNumber1;
            public Image image_CoinNumber2;
            public Image image_CoinNumber3;
            public Image image_ContinueText;
            public Image image_ContinueNumber;
            public Image image_ContinueText_En;


           
        }


        [HideInInspector]
        public Image image_ContinueGround;
        [HideInInspector]
        public Image image_text_english;
        [HideInInspector]
        public Image image_ContinueText;
        [HideInInspector]
        public Image image_ContinueNumber;
        //[HideInInspector]
        //public Slider image_AoeBar;
        //[HideInInspector]
        //public Slider image_WaterBar;
        //[HideInInspector]
        //public Animator animator_AOEFlash;
        //[HideInInspector]
        //public Image Image_AoeSlotFlash;
        //[HideInInspector]
        //public Image Image_AoeSlot;
        [HideInInspector]
        public Animator animator_PassFlash;
        [HideInInspector]
        public Slider image_PassBar;
        [HideInInspector]
        public Image image_PassSlot;
        [HideInInspector]
        public Image image_Pass;
        [HideInInspector]
        public Image image_Star1;
        [HideInInspector]
        public Image image_Star2;
        [HideInInspector]
        public Image image_Star3;
        [HideInInspector]
        public Image image_Star1Gray;
        [HideInInspector]
        public Image image_Star2Gray;
        [HideInInspector]
        public Image image_Star3Gray;


        public List<Image> image_Numbers;
        public List<Image> image_Coins;
        public List<Image> image_Continues;
        public ArrayList panelList;


        [HideInInspector]
        public GameObject low_water;
        [HideInInspector]
        public GameObject hight_water;

        // Use this for initialization
        public void Init()
        {

            image_ContinueGround = transform.Find("Panel_Continue").GetComponent<Image>();
            image_text_english = transform.Find("Panel_Continue/Image_Text_En").GetComponent<Image>();
            image_ContinueText = transform.Find("Panel_Continue/Image_Text").GetComponent<Image>();
            image_ContinueNumber = transform.Find("Panel_Continue/Image_Number").GetComponent<Image>();
            //image_AoeBar = transform.Find("Panel_AOE/Image_Bar").GetComponent<Slider>();
            //image_WaterBar = transform.Find("Panel_Water/Image_Bar").GetComponent<Slider>();
            //animator_AOEFlash = transform.Find("Panel_AOEFlash").GetComponent<Animator>();
            //Image_AoeSlotFlash = transform.Find("Panel_AOE/Image_SlotFlash").GetComponent<Image>();
            //Image_AoeSlot = transform.Find("Panel_AOE/Image_Slot").GetComponent<Image>();
            animator_PassFlash = transform.Find("Panel_Pass/Panel_Flash").GetComponent<Animator>();
            image_PassBar = transform.Find("Panel_Pass/Panel_Progress/Image_Bar").GetComponent<Slider>();
            image_PassSlot = transform.Find("Panel_Pass/Panel_Flash").GetComponent<Image>();
            image_Pass = transform.Find("Panel_Pass").GetComponent<Image>();
            image_Star1 = transform.Find("Panel_Pass/Image_Star1").GetComponent<Image>();
            image_Star2 = transform.Find("Panel_Pass/Image_Star2").GetComponent<Image>();
            image_Star3 = transform.Find("Panel_Pass/Image_Star3").GetComponent<Image>();
            image_Star1Gray = transform.Find("Panel_Pass/Image_Star1Gray").GetComponent<Image>();
            image_Star2Gray = transform.Find("Panel_Pass/Image_Star2Gray").GetComponent<Image>();
            image_Star3Gray = transform.Find("Panel_Pass/Image_Star3Gray").GetComponent<Image>();

            panelList = new ArrayList(GameConfig.GAME_CONFIG_PLAYER_COUNT);
            {
                PlayerPanel panel = new PlayerPanel();
                panel.image_Target = transform.Find("Panel_P1/Image_P1Target").GetComponent<Image>();
                panel.image_Head1 = transform.Find("Panel_P1/Image_P1Head").GetComponent<Image>();
                panel.image_Head2 = transform.Find("Panel_P1/Image_P1").GetComponent<Image>();
                panel.image_NumberSlot1 = transform.Find("Panel_P1/Image_NumberSlot1").GetComponent<Image>();
                panel.image_NumberSlot2 = transform.Find("Panel_P1/Image_NumberSlot2").GetComponent<Image>();
                panel.image_NumberSlot3 = transform.Find("Panel_P1/Image_NumberSlot3").GetComponent<Image>();
                panel.image_NumberSlot4 = transform.Find("Panel_P1/Image_NumberSlot4").GetComponent<Image>();
                panel.image_NumberSlot5 = transform.Find("Panel_P1/Image_NumberSlot5").GetComponent<Image>();
                //panel.image_TimerSlot1 = transform.Find("Panel_P1/Image_TimerSlot1").GetComponent<Image>();
                //panel.image_TimerSlot2 = transform.Find("Panel_P1/Image_TimerSlot2").GetComponent<Image>();
                //panel.image_TimerSlot3 = transform.Find("Panel_P1/Image_TimerSlot3").GetComponent<Image>();
                //panel.image_TimerSlot4 = transform.Find("Panel_P1/Image_TimerSlot4").GetComponent<Image>();
                //panel.image_TimerSlot5 = transform.Find("Panel_P1/Image_TimerSlot5").GetComponent<Image>();
                panel.image_CoinText1 = transform.Find("Panel_P1/Image_CoinText").GetComponent<Image>();
                panel.image_CoinText1_En = transform.Find("Panel_P1/Image_CoinText_En").GetComponent<Image>();
                panel.image_CoinText2 = transform.Find("Panel_P1/Image_Div").GetComponent<Image>();
                panel.image_CoinNumber1 = transform.Find("Panel_P1/Image_CoinNum1").GetComponent<Image>();
                panel.image_CoinNumber2 = transform.Find("Panel_P1/Image_CoinNum2").GetComponent<Image>();
                panel.image_CoinNumber3 = transform.Find("Panel_P1/Image_CoinNum3").GetComponent<Image>();
                panel.image_ContinueText = transform.Find("Panel_P1/Image_ContinueText").GetComponent<Image>();
                panel.image_ContinueText_En = transform.Find("Panel_P1/Image_ContinueText_En").GetComponent<Image>();
                panel.image_ContinueNumber = transform.Find("Panel_P1/Image_ContinueNumber").GetComponent<Image>();
                panel.image_HeadGround = transform.Find("Panel_P1/Image_HeadGround2").GetComponent<Image>();
                panel.image_TimerBar = transform.Find("Panel_P1/Image_HeadGround2/Image_TimeBar").GetComponent<Slider>();
                panel.image_TimerWarning = transform.Find("Panel_P1/Image_HeadGround1").GetComponent<Image>();
                panel.image_HeadGround4 = transform.Find("Panel_P1/Image_HeadGround4").GetComponent<Image>();
                panel.image_Head1Grey = transform.Find("Panel_P1/Image_P1HeadGrey").GetComponent<Image>();
                panel.image_image_HeadGroundGrey = transform.Find("Panel_P1/Image_HeadGround2Grey").GetComponent<Image>();
                panel.image_HeadGround4Grey = transform.Find("Panel_P1/Image_HeadGround4Grey").GetComponent<Image>();


                panelList.Add(panel);
            }

            {
                PlayerPanel panel = new PlayerPanel();
                panel.image_Target = transform.Find("Panel_P2/Image_P2Target").GetComponent<Image>();
                panel.image_Head1 = transform.Find("Panel_P2/Image_P2Head").GetComponent<Image>();
                panel.image_Head2 = transform.Find("Panel_P2/Image_P2").GetComponent<Image>();
                panel.image_NumberSlot1 = transform.Find("Panel_P2/Image_NumberSlot1").GetComponent<Image>();
                panel.image_NumberSlot2 = transform.Find("Panel_P2/Image_NumberSlot2").GetComponent<Image>();
                panel.image_NumberSlot3 = transform.Find("Panel_P2/Image_NumberSlot3").GetComponent<Image>();
                panel.image_NumberSlot4 = transform.Find("Panel_P2/Image_NumberSlot4").GetComponent<Image>();
                panel.image_NumberSlot5 = transform.Find("Panel_P2/Image_NumberSlot5").GetComponent<Image>();
                //panel.image_TimerSlot1 = transform.Find("Panel_P2/Image_TimerSlot1").GetComponent<Image>();
                //panel.image_TimerSlot2 = transform.Find("Panel_P2/Image_TimerSlot2").GetComponent<Image>();
                //panel.image_TimerSlot3 = transform.Find("Panel_P2/Image_TimerSlot3").GetComponent<Image>();
                //panel.image_TimerSlot4 = transform.Find("Panel_P2/Image_TimerSlot4").GetComponent<Image>();
                //panel.image_TimerSlot5 = transform.Find("Panel_P2/Image_TimerSlot5").GetComponent<Image>();
                panel.image_CoinText1 = transform.Find("Panel_P2/Image_CoinText").GetComponent<Image>();
                panel.image_CoinText1_En = transform.Find("Panel_P2/Image_CoinText_En").GetComponent<Image>();
                panel.image_CoinText2 = transform.Find("Panel_P2/Image_Div").GetComponent<Image>();
                panel.image_CoinNumber1 = transform.Find("Panel_P2/Image_CoinNum1").GetComponent<Image>();
                panel.image_CoinNumber2 = transform.Find("Panel_P2/Image_CoinNum2").GetComponent<Image>();
                panel.image_CoinNumber3 = transform.Find("Panel_P2/Image_CoinNum3").GetComponent<Image>();
                panel.image_ContinueText = transform.Find("Panel_P2/Image_ContinueText").GetComponent<Image>();
                panel.image_ContinueText_En = transform.Find("Panel_P2/Image_ContinueText_En").GetComponent<Image>();
                panel.image_ContinueNumber = transform.Find("Panel_P2/Image_ContinueNumber").GetComponent<Image>();
                panel.image_HeadGround = transform.Find("Panel_P2/Image_HeadGround2").GetComponent<Image>();
                panel.image_TimerBar = transform.Find("Panel_P2/Image_HeadGround2/Image_TimeBar").GetComponent<Slider>();
                panel.image_TimerWarning = transform.Find("Panel_P2/Image_HeadGround1").GetComponent<Image>();
                panel.image_HeadGround4 = transform.Find("Panel_P2/Image_HeadGround4").GetComponent<Image>();
                panel.image_Head1Grey = transform.Find("Panel_P2/Image_P2HeadGrey").GetComponent<Image>();
                panel.image_image_HeadGroundGrey = transform.Find("Panel_P2/Image_HeadGround2Grey").GetComponent<Image>();
                panel.image_HeadGround4Grey = transform.Find("Panel_P2/Image_HeadGround4Grey").GetComponent<Image>();
                panelList.Add(panel);
            }


            {
                PlayerPanel panel = new PlayerPanel();
                panel.image_Target = transform.Find("Panel_P3/Image_P3Target").GetComponent<Image>();
                panel.image_Head1 = transform.Find("Panel_P3/Image_P3Head").GetComponent<Image>();
                panel.image_Head2 = transform.Find("Panel_P3/Image_P3").GetComponent<Image>();
                panel.image_NumberSlot1 = transform.Find("Panel_P3/Image_NumberSlot1").GetComponent<Image>();
                panel.image_NumberSlot2 = transform.Find("Panel_P3/Image_NumberSlot2").GetComponent<Image>();
                panel.image_NumberSlot3 = transform.Find("Panel_P3/Image_NumberSlot3").GetComponent<Image>();
                panel.image_NumberSlot4 = transform.Find("Panel_P3/Image_NumberSlot4").GetComponent<Image>();
                panel.image_NumberSlot5 = transform.Find("Panel_P3/Image_NumberSlot5").GetComponent<Image>();
                //panel.image_TimerSlot1 = transform.Find("Panel_P3/Image_TimerSlot1").GetComponent<Image>();
                //panel.image_TimerSlot2 = transform.Find("Panel_P3/Image_TimerSlot2").GetComponent<Image>();
                //panel.image_TimerSlot3 = transform.Find("Panel_P3/Image_TimerSlot3").GetComponent<Image>();
                //panel.image_TimerSlot4 = transform.Find("Panel_P3/Image_TimerSlot4").GetComponent<Image>();
                //panel.image_TimerSlot5 = transform.Find("Panel_P3/Image_TimerSlot5").GetComponent<Image>();
                panel.image_CoinText1 = transform.Find("Panel_P3/Image_CoinText").GetComponent<Image>();
                panel.image_CoinText1_En = transform.Find("Panel_P3/Image_CoinText_En").GetComponent<Image>();
                panel.image_CoinText2 = transform.Find("Panel_P3/Image_Div").GetComponent<Image>();
                panel.image_CoinNumber1 = transform.Find("Panel_P3/Image_CoinNum1").GetComponent<Image>();
                panel.image_CoinNumber2 = transform.Find("Panel_P3/Image_CoinNum2").GetComponent<Image>();
                panel.image_CoinNumber3 = transform.Find("Panel_P3/Image_CoinNum3").GetComponent<Image>();
                panel.image_ContinueText = transform.Find("Panel_P3/Image_ContinueText").GetComponent<Image>();
                panel.image_ContinueText_En = transform.Find("Panel_P3/Image_ContinueText_En").GetComponent<Image>();
                panel.image_ContinueNumber = transform.Find("Panel_P3/Image_ContinueNumber").GetComponent<Image>();
                panel.image_HeadGround = transform.Find("Panel_P3/Image_HeadGround2").GetComponent<Image>();
                panel.image_TimerBar = transform.Find("Panel_P3/Image_HeadGround2/Image_TimeBar").GetComponent<Slider>();
                panel.image_TimerWarning = transform.Find("Panel_P3/Image_HeadGround1").GetComponent<Image>();
                panel.image_HeadGround4 = transform.Find("Panel_P3/Image_HeadGround4").GetComponent<Image>();
                panel.image_Head1Grey = transform.Find("Panel_P3/Image_P3HeadGrey").GetComponent<Image>();
                panel.image_image_HeadGroundGrey = transform.Find("Panel_P3/Image_HeadGround2Grey").GetComponent<Image>();
                panel.image_HeadGround4Grey = transform.Find("Panel_P3/Image_HeadGround4Grey").GetComponent<Image>();
                panelList.Add(panel);
            }

            low_water = transform.parent.Find("LowWater").gameObject;
            hight_water = transform.parent.Find("HightWater").gameObject;

        }
    }
}

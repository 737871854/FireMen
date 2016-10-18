using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


namespace Need.Mx
{

    public class EndingView : MonoBehaviour
    {

        public struct RankPanel
        {
            public GameObject go;
            public Image image_Head;
            public Image image_NumberSlot1;
            public Image image_NumberSlot2;
            public Image image_NumberSlot3;
            public Image image_NumberSlot4;
            public Image image_NumberSlot5;
        }

        [HideInInspector]
        public Image image_GroundLoss;
        [HideInInspector]
        public Image image_GroundLoss_En;
        [HideInInspector]
        public Animator animator_GroundLoss;
        [HideInInspector]
        public Animator animator_GroundLoss_En;
        [HideInInspector]
        public Animator animator_GroundSuccess_En;
        [HideInInspector]
        public Image image_GroundSuccess;
        [HideInInspector]
        public Image image_GroundSuccess_En;
        [HideInInspector]
        public Animator animator_GroundSuccess;
        [HideInInspector]
        public Image image_GroundSuccessEffect;
        [HideInInspector]
        public Animator animator_GroundSuccessEffect;
        

        public List<Image> image_Numbers;
        public List<Image> image_Heads;
        public List<RankPanel> image_Ranks;

        // Use this for initialization
        public void Start()
        {
            image_GroundLoss            = transform.Find("Panel_GroundLoss").GetComponent<Image>();
            image_GroundLoss_En         = transform.Find("Panel_GroundLoss_En").GetComponent<Image>();
            image_GroundSuccess         = transform.Find("Panel_GroundSuccess").GetComponent<Image>();
            image_GroundSuccess_En      = transform.Find("Panel_GroundSuccess_En").GetComponent<Image>();
            image_GroundSuccessEffect   = transform.Find("Panel_GroundSuccessEffect").GetComponent<Image>();
            animator_GroundSuccess_En   = transform.Find("Panel_GroundSuccess_En").GetComponent<Animator>();
            animator_GroundLoss         = transform.Find("Panel_GroundLoss").GetComponent<Animator>();
            animator_GroundLoss_En      = transform.Find("Panel_GroundLoss_En").GetComponent<Animator>();
            animator_GroundSuccess      = transform.Find("Panel_GroundSuccess").GetComponent<Animator>();
            animator_GroundSuccessEffect= transform.Find("Panel_GroundSuccessEffect").GetComponent<Animator>();

            image_Ranks = new List<RankPanel>();
            {
                RankPanel ui = new RankPanel();
                ui.image_Head        = transform.Find("Panel_First/Image_Head").GetComponent<Image>();
                ui.image_NumberSlot1 = transform.Find("Panel_First/Image_Value1").GetComponent<Image>();
                ui.image_NumberSlot2 = transform.Find("Panel_First/Image_Value2").GetComponent<Image>();
                ui.image_NumberSlot3 = transform.Find("Panel_First/Image_Value3").GetComponent<Image>();
                ui.image_NumberSlot4 = transform.Find("Panel_First/Image_Value4").GetComponent<Image>();
                ui.image_NumberSlot5 = transform.Find("Panel_First/Image_Value5").GetComponent<Image>();
                ui.go = transform.Find("Panel_First").gameObject;
                image_Ranks.Add(ui);
            }
            {
                RankPanel ui = new RankPanel();
                ui.image_Head        = transform.Find("Panel_Second/Image_Head").GetComponent<Image>();
                ui.image_NumberSlot1 = transform.Find("Panel_Second/Image_Value1").GetComponent<Image>();
                ui.image_NumberSlot2 = transform.Find("Panel_Second/Image_Value2").GetComponent<Image>();
                ui.image_NumberSlot3 = transform.Find("Panel_Second/Image_Value3").GetComponent<Image>();
                ui.image_NumberSlot4 = transform.Find("Panel_Second/Image_Value4").GetComponent<Image>();
                ui.image_NumberSlot5 = transform.Find("Panel_Second/Image_Value5").GetComponent<Image>();
                ui.go                = transform.Find("Panel_Second").gameObject;
                image_Ranks.Add(ui);
            }
            {
                RankPanel ui = new RankPanel();
                ui.image_Head        = transform.Find("Panel_Third/Image_Head").GetComponent<Image>();
                ui.image_NumberSlot1 = transform.Find("Panel_Third/Image_Value1").GetComponent<Image>();
                ui.image_NumberSlot2 = transform.Find("Panel_Third/Image_Value2").GetComponent<Image>();
                ui.image_NumberSlot3 = transform.Find("Panel_Third/Image_Value3").GetComponent<Image>();
                ui.image_NumberSlot4 = transform.Find("Panel_Third/Image_Value4").GetComponent<Image>();
                ui.image_NumberSlot5 = transform.Find("Panel_Third/Image_Value5").GetComponent<Image>();
                ui.go                = transform.Find("Panel_Third").gameObject;
                image_Ranks.Add(ui);
            }       

        }
    }

}

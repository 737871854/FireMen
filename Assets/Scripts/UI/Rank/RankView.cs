using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


namespace Need.Mx
{

    public class RankView : MonoBehaviour
    {
        public struct EditPanel
        {
            public Image image_Head;
            public List<Image> image_CharSlotList;
            public Image image_Target;
            public Image image_Key;
            public Image image_KeyPressed;
        }

        public struct TopPanel
        {
            public Image image_Head;
            public List<Image> image_CharSlotList;
            public List<Image> image_NumberSlotList;
        }

        [HideInInspector]
        public Image image_Edit;
        [HideInInspector]
        public Image image_Time1;
        [HideInInspector]
        public Image image_Time2;
        [HideInInspector]
        public Image image_TopN;
        [HideInInspector]
        public Image panel_KeyParent;
        [HideInInspector]
        public Image image_KeyPressed1;
        [HideInInspector]
        public Image image_KeyPressed2;

        [HideInInspector]
        public Image image_Ground1;
        [HideInInspector]
        public Image image_Ground1_En;
        [HideInInspector]
        public Image image_TextTips1;
        [HideInInspector]
        public Image image_TextTips1_En;
        [HideInInspector]
        public Image iamge_EnSure;
        [HideInInspector]
        public Image image_EnSure_En;

        [HideInInspector]
        public Image image_TopN_Ground1;
        [HideInInspector]
        public Image image_TopN_Ground1_En;

        public List<Image>      image_Keys;
        public List<EditPanel>  image_Edits;
        public List<TopPanel>   image_Tops;
        public List<Image>      image_Numbers;
        public List<Image>      image_EditChars;
        public List<Image>      image_Head;


        [HideInInspector]
        public GameObject low_water;
        [HideInInspector]
        public GameObject hight_water;
        // Use this for initialization
       public void Init()
        {
            image_Edit  = transform.Find("Panel_Edit").GetComponent<Image>();
            image_Time1 = transform.Find("Panel_Edit/Image_Time1").GetComponent<Image>();
            image_Time2 = transform.Find("Panel_Edit/Image_Time2").GetComponent<Image>();
            panel_KeyParent   = transform.Find("Panel_Edit/Panel_KeyParent").GetComponent<Image>();
            image_KeyPressed1 = transform.Find("Panel_Edit/Image_KeyPressed1").GetComponent<Image>();
            image_KeyPressed2 = transform.Find("Panel_Edit/Image_KeyPressed2").GetComponent<Image>();

            image_Keys = new List<Image>();
            {
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyA").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyB").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyC").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyD").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyE").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyF").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyG").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyH").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyI").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyJ").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyK").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyL").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyN").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyM").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyO").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyP").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyQ").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyR").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyS").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyT").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyU").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyV").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyW").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyX").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyY").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyZ").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyDel").GetComponent<Image>());
                image_Keys.Add(transform.Find("Panel_Edit/Image_KeyEnter").GetComponent<Image>());
            }

            image_Edits = new List<EditPanel>();
            {
                EditPanel ui = new EditPanel();
                ui.image_CharSlotList = new List<Image>();
                ui.image_Head      = transform.Find("Panel_Edit/Image_P1").GetComponent<Image>();
                ui.image_Target    = transform.Find("Panel_Edit/Image_Target1").GetComponent<Image>();
                ui.image_CharSlotList.Add(transform.Find("Panel_Edit/Image_P1/Image_Name1").GetComponent<Image>());
                ui.image_Key       = null;
                ui.image_CharSlotList.Add(transform.Find("Panel_Edit/Image_P1/Image_Name2").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_Edit/Image_P1/Image_Name3").GetComponent<Image>());
                image_Edits.Add(ui);
            }

            {
                EditPanel ui = new EditPanel();
                ui.image_CharSlotList = new List<Image>();
                ui.image_Head   = transform.Find("Panel_Edit/Image_P2").GetComponent<Image>();
                ui.image_Target = transform.Find("Panel_Edit/Image_Target2").GetComponent<Image>();
                ui.image_Key = null;
                ui.image_CharSlotList.Add(transform.Find("Panel_Edit/Image_P2/Image_Name1").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_Edit/Image_P2/Image_Name2").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_Edit/Image_P2/Image_Name3").GetComponent<Image>());
                image_Edits.Add(ui);
            }

            {
                EditPanel ui = new EditPanel();
                ui.image_CharSlotList = new List<Image>();
                ui.image_Head   = transform.Find("Panel_Edit/Image_P3").GetComponent<Image>();
                ui.image_Target = transform.Find("Panel_Edit/Image_Target3").GetComponent<Image>();
                ui.image_Key = null;
                ui.image_CharSlotList.Add(transform.Find("Panel_Edit/Image_P3/Image_Name1").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_Edit/Image_P3/Image_Name2").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_Edit/Image_P3/Image_Name3").GetComponent<Image>());
                image_Edits.Add(ui);
            }

            image_TopN = transform.Find("Panel_TopN").GetComponent<Image>();
            image_Tops = new List<TopPanel>();
            {
                TopPanel ui = new TopPanel();
                ui.image_CharSlotList = new List<Image>();
                ui.image_NumberSlotList = new List<Image>();
                ui.image_Head = transform.Find("Panel_TopN/Image_P1/Image_Head").GetComponent<Image>();
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P1/Image_Name1").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P1/Image_Name2").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P1/Image_Name3").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P1/Image_Value1").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P1/Image_Value2").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P1/Image_Value3").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P1/Image_Value4").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P1/Image_Value5").GetComponent<Image>());
                image_Tops.Add(ui);
            }

            {
                TopPanel ui = new TopPanel();
                ui.image_CharSlotList = new List<Image>();
                ui.image_NumberSlotList = new List<Image>();
                ui.image_Head = transform.Find("Panel_TopN/Image_P2/Image_Head").GetComponent<Image>();
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P2/Image_Name1").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P2/Image_Name2").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P2/Image_Name3").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P2/Image_Value1").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P2/Image_Value2").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P2/Image_Value3").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P2/Image_Value4").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P2/Image_Value5").GetComponent<Image>());
                image_Tops.Add(ui);
            }

            {
                TopPanel ui = new TopPanel();
                ui.image_CharSlotList = new List<Image>();
                ui.image_NumberSlotList = new List<Image>();
                ui.image_Head = transform.Find("Panel_TopN/Image_P3/Image_Head").GetComponent<Image>();
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P3/Image_Name1").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P3/Image_Name2").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P3/Image_Name3").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P3/Image_Value1").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P3/Image_Value2").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P3/Image_Value3").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P3/Image_Value4").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P3/Image_Value5").GetComponent<Image>());
                image_Tops.Add(ui);
            }

            {
                TopPanel ui = new TopPanel();
                ui.image_CharSlotList = new List<Image>();
                ui.image_NumberSlotList = new List<Image>();
                ui.image_Head = transform.Find("Panel_TopN/Image_P4/Image_Head").GetComponent<Image>();
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P4/Image_Name1").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P4/Image_Name2").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P4/Image_Name3").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P4/Image_Value1").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P4/Image_Value2").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P4/Image_Value3").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P4/Image_Value4").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P4/Image_Value5").GetComponent<Image>());
                image_Tops.Add(ui);
            }

            {
                TopPanel ui = new TopPanel();
                ui.image_CharSlotList = new List<Image>();
                ui.image_NumberSlotList = new List<Image>();
                ui.image_Head = transform.Find("Panel_TopN/Image_P5/Image_Head").GetComponent<Image>();
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P5/Image_Name1").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P5/Image_Name2").GetComponent<Image>());
                ui.image_CharSlotList.Add(transform.Find("Panel_TopN/Image_P5/Image_Name3").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P5/Image_Value1").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P5/Image_Value2").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P5/Image_Value3").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P5/Image_Value4").GetComponent<Image>());
                ui.image_NumberSlotList.Add(transform.Find("Panel_TopN/Image_P5/Image_Value5").GetComponent<Image>());
                image_Tops.Add(ui);
            }

            {
                image_Ground1 = transform.Find("Panel_Edit/Image_Ground1").GetComponent<Image>();
                image_Ground1_En = transform.Find("Panel_Edit/Image_Ground1_En").GetComponent<Image>();
                image_TextTips1 = transform.Find("Panel_Edit/Image_TextTips1").GetComponent<Image>();
                image_TextTips1_En = transform.Find("Panel_Edit/Image_TextTips1_En").GetComponent<Image>();
                iamge_EnSure = transform.Find("Panel_Edit/Image_KeyEnter").GetComponent<Image>();
                image_EnSure_En = transform.Find("Panel_Edit/Image_KeyEnter_En").GetComponent<Image>();

                image_TopN_Ground1 = transform.Find("Panel_TopN/Image_Ground1").GetComponent<Image>();
                image_TopN_Ground1_En = transform.Find("Panel_TopN/Image_Ground1_En").GetComponent<Image>();
            }

            {
                low_water = transform.parent.Find("LowWater").gameObject;
                hight_water = transform.parent.Find("HightWater").gameObject;
            }
        }

    }
}

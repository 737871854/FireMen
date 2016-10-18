using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


namespace Need.Mx
{
    public class ConditionView : MonoBehaviour
    {
        [HideInInspector]
        public Image image_Ground0;
        [HideInInspector]
        public Image image_Ground1;
        [HideInInspector]
        public Image image_Ground2;
        [HideInInspector]
        public Image image_Ground1_En;
        [HideInInspector]
        public Image image_Ground2_En;
        [HideInInspector]
        public GameObject image_Scores;
        // Use this for initialization
        void Start()
        {
            image_Ground0 = transform.Find("Image_Ground0").GetComponent<Image>();
            image_Ground1 = transform.Find("Image_Ground1").GetComponent<Image>();
            image_Ground2 = transform.Find("Image_Ground2").GetComponent<Image>();
            image_Scores  = transform.Find("Image_Score").gameObject;
            image_Ground1_En = transform.Find("Image_Ground1_En").GetComponent<Image>();
            image_Ground2_En = transform.Find("Image_Ground2_En").GetComponent<Image>();
        }
    }
}

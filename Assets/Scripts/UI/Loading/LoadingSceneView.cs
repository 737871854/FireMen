using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LoadingSceneView : MonoBehaviour {

    public List<Image> image_Numbers;
    public Image image_NumberSlot1;
    public Image image_NumberSlot2;
    public Image image_NumberSlot3;
    public Slider slider_Percent;

    public Image image_Ground1;
    public Image image_Ground2;
    public Image image_Ground3;

    [HideInInspector]
    public Image image_PercentText_Chinese;
    [HideInInspector]
    public Image image_PercentText_English;


    void Awake()
    {
        image_NumberSlot1 = transform.Find("Image_NumberSlot1").GetComponent<Image>();
        image_NumberSlot2 = transform.Find("Image_NumberSlot2").GetComponent<Image>();
        image_NumberSlot3 = transform.Find("Image_NumberSlot3").GetComponent<Image>();
        slider_Percent = transform.Find("Image_PercentGround/Image_Progress").GetComponent<Slider>();
        image_Ground1 = transform.Find("Image_Ground1").GetComponent<Image>();
        image_Ground2 = transform.Find("Image_Ground2").GetComponent<Image>();
        image_Ground3 = transform.Find("Image_Ground3").GetComponent<Image>();

        image_PercentText_Chinese = transform.Find("Image_Text1").GetComponent<Image>();
        image_PercentText_English = transform.Find("Image_Text1_En").GetComponent<Image>();
    }
}

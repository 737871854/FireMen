using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlotView : MonoBehaviour {

    public RawImage image_Movie;
    public Image image_BackGround;

    [HideInInspector]
    public GameObject plot_skip0;
    [HideInInspector]
    public GameObject plot_skip1;

	// Use this for initialization
	public void Init () {
        image_Movie = transform.Find("RawImage_Movie").GetComponent<RawImage>();
        image_BackGround = transform.GetComponent<Image>();

        plot_skip0 = transform.transform.parent.Find("PlotSkip").gameObject;
        plot_skip1 = transform.transform.parent.Find("PlotSkip1").gameObject;
	}
}

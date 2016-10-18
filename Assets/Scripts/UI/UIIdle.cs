using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Need.Mx;

public class UIIdle : MonoBehaviour {

    public RawImage image_Movie;
    public Image image_BackGround;
    public MovieTexture movTexture;
    protected bool isPlaying;
    private GameObject idleSkip0;
    private GameObject idleSkip1;
	// Use this for initialization
	void Start () {
        image_Movie = transform.Find("RawImage_Movie").GetComponent<RawImage>();
        image_BackGround = transform.GetComponent<Image>();
        //image_Movie.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        //image_BackGround.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        idleSkip0 = transform.parent.transform.Find("IdleSkip").gameObject;
        idleSkip1 = transform.parent.transform.Find("IdleSkip1").gameObject;
        Init();
        StartCoroutine(OnCloseIdle());
	}
	
    void Init()
    {
        if (Main.SettingManager.GameLanguage == 0)
        {
            idleSkip0.SetActive(true);
            idleSkip1.SetActive(false);
        }
        else
        {
            idleSkip0.SetActive(false);
            idleSkip1.SetActive(true);
        }
    }

	// Update is called once per frame
	void Update () {

	}

    IEnumerator OnCloseIdle()
    {
        isPlaying = true;
        movTexture.Play();
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = movTexture.audioClip;
        audio.Play();
        audio.volume = (float)Main.SettingManager.GameVolume * 0.1f;
        image_Movie.texture = movTexture;
        image_Movie.gameObject.SetActive(true);
        image_BackGround.gameObject.SetActive(true);

        while(movTexture.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        
        movTexture.Pause();
        movTexture.Stop();
        audio.Stop();
        Main.GameMode.ReturnStart();
        //image_Movie.gameObject.SetActive(false);
        //image_BackGround.gameObject.SetActive(false);
    }
}

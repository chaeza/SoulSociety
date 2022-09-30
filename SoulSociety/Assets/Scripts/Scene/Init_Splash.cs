using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Init_Splash : MonoBehaviour
{
    public RawImage fadeImg; // fade에 쓸 이미지
    public float fadeTime; //화면이 변할 시간
    public bool fadeout;
    public bool fadein;

    private bool isPlaying = false;

    private void Start()
    {
       // fadein = true; //스크립트가 활성화된 경우, 페이드인 시킴. 이 부분은 원하면 생략
    }
    private void Update()
    {
        if (fadeout == true && isPlaying == false) //페이드아웃을 원할 때
        {
            StartCoroutine(FadeOut());
        }
        else if (fadeout == false && fadein == true) //페이드인을 원할 때
        {
            StartCoroutine(FadeIn());
        }
    }
    IEnumerator FadeOut()
    {
        fadeImg.gameObject.SetActive(true);
        isPlaying = true;
        Color tempColor = fadeImg.color;
        tempColor.a = 0f;
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeTime;
            fadeImg.color = tempColor;

            if (tempColor.a >= 1f)
            {
                tempColor.a = 1f;
            }
            yield return null;
        }
        fadeout = false;
    }
    IEnumerator FadeIn()
    {
       
        Color tempColor = fadeImg.color;
        while (tempColor.a > 0f)
        {
            tempColor.a -= Time.deltaTime / fadeTime;
            fadeImg.color = tempColor;

            if (tempColor.a <= 0f)
            {
                tempColor.a = 0f;
                fadeImg.color = tempColor;
            }
            
            yield return null;
        }

        fadeImg.gameObject.SetActive(false);
        fadein = false;
        isPlaying = false;
    }
}
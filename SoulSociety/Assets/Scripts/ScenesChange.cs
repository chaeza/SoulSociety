using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesChange : MonoBehaviour 
{
    GameObject FadePannel;
    Image image;
    RawImage rawImage;

    private void Start()
    {
        image = GetComponent<Image>();
        rawImage = GetComponent<RawImage>();
        StartCoroutine(FadeIn(3f));

       
    }
  

    IEnumerator Next()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);  // 1 ¹øÂ° ¾À ·Îµå
    }



    public IEnumerator FadeIn(float time)
    {
        Color color = rawImage.color;
        while (color.a > 3f)
        {
            color.a -= Time.deltaTime / time;
            rawImage.color = color;
            yield return null;
        }
       // SceneManager.LoadScene(1);  // 1 ¹øÂ° ¾À ·Îµå
    }

    public IEnumerator FadeOut(float time)
    {
        Color color = rawImage.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / time;
            rawImage.color = color;
            yield return null;
        }
    }
}

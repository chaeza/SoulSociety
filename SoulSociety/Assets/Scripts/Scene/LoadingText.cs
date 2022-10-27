using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    private int ran;
    private int imageRan;

    //로딩텍스트를 넣는 배열
    public Text[] text;
    //Array of images to be displayed on the screen when loading
    public RawImage[] image;

    private void Start()
    {
        ran = Random.Range(0, text.Length);
        imageRan = Random.Range(0, image.Length);

        //Activate the gameObject based on a random number.
        text[ran].gameObject.SetActive(true);
        image[imageRan].gameObject.SetActive(true);


    }
}

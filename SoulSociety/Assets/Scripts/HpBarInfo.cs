using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Photon.Pun;
public class HpBarInfo : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI nickname = null;
    [SerializeField] Slider Hpbar = null;
    [SerializeField] Image bee = null;
    Transform cam = null;



    private void Start()
    {
        //카메라
        cam = Camera.main.transform;
        bee.gameObject.SetActive(false);

    }
    //이름을 출력
    public void SetName(string name)
    {
        nickname.text = name;
    }


    //hp 출력
    public void SetHP(float curHP, float maxHP)
    {
        Hpbar.value = curHP / maxHP;
    }

    private void Update()
    {
        // 감정표현, 이모션
        if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftControl && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.T)
        {
            bee.gameObject.SetActive(true);
            StartCoroutine(EmotionTimer());
        }

        //카메라 보기
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        
    }

    IEnumerator EmotionTimer()
    {
        yield return new WaitForSeconds(2);
        bee.gameObject.SetActive(false);

    }

}

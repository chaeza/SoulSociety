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
        //ī�޶�
        cam = Camera.main.transform;
        bee.gameObject.SetActive(false);

    }
    //�̸��� ���
    public void SetName(string name)
    {
        nickname.text = name;
    }


    //hp ���
    public void SetHP(float curHP, float maxHP)
    {
        Hpbar.value = curHP / maxHP;
    }

    private void Update()
    {
        // ����ǥ��, �̸��
        if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftControl && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.T)
        {
            bee.gameObject.SetActive(true);
            StartCoroutine(EmotionTimer());
        }

        //ī�޶� ����
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        
    }

    IEnumerator EmotionTimer()
    {
        yield return new WaitForSeconds(2);
        bee.gameObject.SetActive(false);

    }

}

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

    [SerializeField] Image[] bee = null;
    Transform cam = null;

    bool isEmotion = false;
    bool isDance = false;

    private void Start()
    {
        //ī�޶�
        cam = Camera.main.transform;
        for (int i = 0; i < bee.Length; i++)
            bee[i].gameObject.SetActive(false);

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
        // �̸��
        if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftShift && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha1)
        {
            bee[0].gameObject.SetActive(true);
            StartCoroutine(EmotionTimer());
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftShift && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha2)
        {
            bee[1].gameObject.SetActive(true);
            StartCoroutine(EmotionTimer());
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftShift && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha3)
        {
            bee[2].gameObject.SetActive(true);
            StartCoroutine(EmotionTimer());
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftShift && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha4)
        {
            bee[3].gameObject.SetActive(true);
            StartCoroutine(EmotionTimer());
        }

        // ��
        if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftControl && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha1)
        {
            gameObject.GetPhotonView().RPC("PlayerDance", RpcTarget.All, 1);
            StartCoroutine(DanceTimer());
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftControl && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha2)
        {
            gameObject.GetPhotonView().RPC("PlayerDance", RpcTarget.All, 2);
            StartCoroutine(DanceTimer());
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftControl && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha3)
        {
            gameObject.GetPhotonView().RPC("PlayerDance", RpcTarget.All, 3);
            StartCoroutine(DanceTimer());
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftControl && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha4)
        {
            gameObject.GetPhotonView().RPC("PlayerDance", RpcTarget.All, 4);
            StartCoroutine(DanceTimer());
        }

        //ī�޶� ����
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);

    }


    IEnumerator EmotionTimer()  //�̸�� Ÿ�̸�
    {
        if (isEmotion) yield break;

        isEmotion = true;
        yield return new WaitForSeconds(2);
        bee[0].gameObject.SetActive(false);
        isEmotion = false;
    }

    IEnumerator DanceTimer()  //�̸�� Ÿ�̸�
    {
        if (isDance) yield break;

        isDance = true;
        yield return new WaitForSeconds(2);
        bee[0].gameObject.SetActive(false);
        isDance = false;
    }
}

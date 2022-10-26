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

    private void Start()
    {
        //카메라
        cam = Camera.main.transform;
        for (int i = 0; i < bee.Length; i++)
            bee[i].gameObject.SetActive(false);

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
        //카메라 보기
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        if (!photonView.IsMine) return;
        // 이모션
        if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftShift && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha1)
        {
            gameObject.GetPhotonView().RPC("EmotionStart", RpcTarget.All, 1);
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftShift && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha2)
        {
            gameObject.GetPhotonView().RPC("EmotionStart", RpcTarget.All, 2);
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftShift && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha3)
        {
            gameObject.GetPhotonView().RPC("EmotionStart", RpcTarget.All, 3);
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftShift && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha4)
        {
            gameObject.GetPhotonView().RPC("EmotionStart", RpcTarget.All, 4);
        }

        // 댄스
        if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftControl && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha1)
        {
            gameObject.GetPhotonView().RPC("PlayerDance", RpcTarget.All, 1);
         
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftControl && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha2)
        {
            gameObject.GetPhotonView().RPC("PlayerDance", RpcTarget.All, 2);
           
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftControl && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha3)
        {
            gameObject.GetPhotonView().RPC("PlayerDance", RpcTarget.All, 3);
           
        }
        else if (GameMgr.Instance.playerInput.emotionKey1 == KeyCode.LeftControl && GameMgr.Instance.playerInput.emotionKey2 == KeyCode.Alpha4)
        {
            gameObject.GetPhotonView().RPC("PlayerDance", RpcTarget.All, 4);
           
        }
    }

    [PunRPC]
    void EmotionStart(int emotionNum)
    {
        photonView.StartCoroutine(EmotionTimer(emotionNum));
    }


    IEnumerator EmotionTimer(int emotionNum)  //이모션 타이머
    {
        if (isEmotion) yield break;

        bee[emotionNum].gameObject.SetActive(true);
        isEmotion = true;
        yield return new WaitForSeconds(2);
        bee[emotionNum].gameObject.SetActive(false);
        isEmotion = false;
    }
}

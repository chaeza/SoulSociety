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
    /*private GameObject player = null;*/
    Transform cam = null;

    /*public void PlayerStartPos(GameObject playerPos)
    {
         
        
        for (int i=0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        {
            player[i] = playerPos;
            Debug.Log(player[i].transform.position);
        }
       
    }*/



    private void Start()
    {
        cam = Camera.main.transform;
        /*for(int i=0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (photonView.IsMine)
            {
                nickname[i].text = PhotonNetwork.CurrentRoom.Players[i].NickName;
                Debug.Log(PhotonNetwork.CurrentRoom.Players[i].NickName);
            }
        }*/
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
        /*for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (photonView.IsMine)
            {
                nickname[i].transform.position = cam.WorldToScreenPoint(player.transform.position + new Vector3(0, 0, 2));
                Hpbar[i].transform.position = cam.WorldToScreenPoint(player.transform.position + new Vector3(-1.4f, 0, 2.5f));
            }
             
        }*/
            transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        
    }

}

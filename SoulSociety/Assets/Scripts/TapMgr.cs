using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class TapMgr : MonoBehaviourPun
{
    /*[SerializeField] GameObject[] Raws = null;
    [SerializeField] TextMeshProUGUI[] Nicknames = null;*/

    PhotonView playerInfo;

    // Start is called before the first frame update
    void Start()
    {
        /*int a = GameObject.Find("Player").GetComponent<PhotonView>().ViewID;
        for(int i =0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Nicknames[i].text = PhotonNetwork.PlayerList[i].NickName;


        }*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

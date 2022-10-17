using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BlackHole : MonoBehaviourPun
{
    [SerializeField] GameObject[] blackHole = null;
    float time = 0;
    int ran;
    int twoRan;
    int MasterRan;
    int blackHoleTime = 40;
    public int zero;
   

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MasterRan = Random.Range(0, 8);
            twoRan = Random.Range(0, 1);
            photonView.RPC("BlackHolePos", RpcTarget.MasterClient, MasterRan);
        }
    }
    [PunRPC]
    public void BlackHolePos(int MasterRan)
    {
        photonView.RPC("RPC_BlackHolePos", RpcTarget.All, MasterRan,twoRan);
    }
    [PunRPC]
    public void RPC_BlackHolePos(int MasterRan, int TwoRan)
    {
        ran = MasterRan;
        twoRan = TwoRan;
    }

    private void Update()
    {
        if (zero != 8)                //블랙홀이 7번만 켜지게 만들었다
            time += Time.deltaTime;

        if (time >= blackHoleTime)
        {
            zero++;                          //7번 켤수있게 카운트해줌
            blackHole[ran].SetActive(true);  //블랙홀 켜주기
          
            blackHoleTime -= 3;
            time = 0;

            if (twoRan == 1)
            {
                ran++;                          //블랙홀 랜덤에 그 옆에 있는게 켜지는 로직

                if (ran == 8)                    //만약 8가 된다면 0에서 켜진다
                {
                    ran = 0;
                }
            }
               
            else
            {
                ran--;

                if(ran == 0)
                    ran = 8;
            }
        }
    }
}


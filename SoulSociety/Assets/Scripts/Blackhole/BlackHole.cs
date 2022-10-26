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
            twoRan = Random.Range(0, 2);
            photonView.RPC("BlackHolePos", RpcTarget.MasterClient, MasterRan,twoRan);
        }
    }
    [PunRPC]
    public void BlackHolePos(int MasterRan,int TwoRan)
    {
        photonView.RPC("RPC_BlackHolePos", RpcTarget.All, MasterRan, TwoRan);
    }
    [PunRPC]
    public void RPC_BlackHolePos(int MasterRan, int TwoRan)
    {
        ran = MasterRan;
        twoRan = TwoRan;
    }

    private void Update()
    {
        if (zero != 8)                //��Ȧ�� 7���� ������ �������
            time += Time.deltaTime;

        if (time >= blackHoleTime)
        {
            zero++;                          //7�� �Ӽ��ְ� ī��Ʈ����
            blackHole[ran].SetActive(true);  //��Ȧ ���ֱ�
          
            blackHoleTime -= 3;
            time = 0;

            if (twoRan == 1)
            {
                ran++;                          //��Ȧ ������ �� ���� �ִ°� ������ ����

                if (ran == 8)                    //���� 8�� �ȴٸ� 0���� ������
                {
                    ran = 0;
                }
            }
               
            else
            {
                ran--;

                if(ran == -1)
                    ran = 7;
            }
        }
    }
}


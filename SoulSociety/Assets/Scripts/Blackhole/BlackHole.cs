using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BlackHole : MonoBehaviourPun
{
    [SerializeField] GameObject[] blackHole = null;
    float time = 0;
    int ran;
    int MasterRan;
    int blackHoleTime = 40;
    public int zero;
   

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MasterRan = Random.Range(0, 8);
            photonView.RPC("BlackHolePos", RpcTarget.MasterClient, MasterRan);
        }
    }
    [PunRPC]
    public void BlackHolePos(int MasterRan)
    {
        photonView.RPC("RPC_BlackHolePos", RpcTarget.All, MasterRan);
    }
    [PunRPC]
    public void RPC_BlackHolePos(int MasterRan)
    {
        ran = MasterRan;
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

            ran++;                          //��Ȧ ������ �� ���� �ִ°� ������ ����

            if (ran == 8)                    //���� 8�� �ȴٸ� 0���� ������
            {
                ran = 0;
            }
         
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BlackHole : MonoBehaviourPun
{
    [SerializeField] GameObject[] blackHole = null;
    float time = 0;
    int ran;
   public int zero;


    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ran = Random.Range(0, 8);
            photonView.RPC("BlackHolePos", RpcTarget.All, ran);
        }

 
      //���� ���� �迭�� �ִ� ���� ���ش�
    }

    private void Update()
    {
        if (zero != 7)                //��Ȧ�� 7���� ������ �������
            time += Time.deltaTime;   

        if (time >= 20)
        {
            zero++;                          //7�� �Ӽ��ְ� ī��Ʈ����
            blackHole[ran].SetActive(true);  //��Ȧ ���ֱ�

            time = 0;

            ran++;                          //��Ȧ ������ �� ���� �ִ°� ������ ����

            if(ran == 8)                    //���� 8�� �ȴٸ� 0���� ������
            {
                ran = 0;
            }
        }
    }
    [PunRPC]
    public void BlackHolePos(int Ran)
    {
        ran = Ran;
    }


}


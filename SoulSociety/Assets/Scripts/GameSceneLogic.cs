using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class GameSceneLogic : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            Vector3 pos = new Vector3(0, 100, -17);
            PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);
        }
    }

    //������Ŭ���̾�Ʈ�� �ٲ�� ȣ��Ǵ� �Լ�
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("������ Ŭ���̾�Ʈ ����:" + newMasterClient.ToString());
    }
}

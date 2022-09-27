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
            Vector3 pos = new Vector3(Random.Range(-7f, 7f), 55,-7);
            
            GameObject player = PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);
            GameMgr.Instance.followCam.playerStart(player.transform);

            
            /*if (player.GetPhotonView().IsMine)
            {
                GameMgr.Instance.hpBarInfo.StartPlayerPos(player.transform);
                GameMgr.Instance.hpBarInfo.SetName(player.GetPhotonView().Controller.NickName);
                

            }*/
        }
    }

    //������Ŭ���̾�Ʈ�� �ٲ�� ȣ��Ǵ� �Լ�
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("������ Ŭ���̾�Ʈ ����:" + newMasterClient.ToString());
    }
}

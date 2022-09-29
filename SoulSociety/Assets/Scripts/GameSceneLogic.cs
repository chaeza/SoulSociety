using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class GameSceneLogic : MonoBehaviourPunCallbacks
{
    int myNum = -1;
    SpawnMgr spawnMgr = null;

    private void Awake()
    {
        spawnMgr = GameObject.FindObjectOfType<SpawnMgr>();
    }
    void Start()
    {
        Vector3 pos = Vector3.zero;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;
        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                myNum = i;
            }
        }
        if (PhotonNetwork.IsConnected)
        {
            if (myNum == 0) pos = new Vector3(-7f, 50, -7);
            if (myNum == 1) pos = new Vector3(-2f, 50, -7);
            if (myNum == 2) pos = new Vector3(2f, 50, -7);
            if (myNum == 3) pos = new Vector3(7f, 50, -7);


            GameObject player = PhotonNetwork.Instantiate("PlayerPrefab", pos, Quaternion.identity);
            GameMgr.Instance.followCam.playerStart(player.transform);

            //���� ������Ŭ���̾�Ʈ�� ��츸 ������ �� �Ķ� ��ȥ ����
            if (PhotonNetwork.IsMasterClient)
            {
                GameMgr.Instance.spawnMgr.photonView.RPC("ItemInit", RpcTarget.MasterClient);
                GameMgr.Instance.spawnMgr.photonView.RPC("SoulInit", RpcTarget.MasterClient);
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }
    //������Ŭ���̾�Ʈ�� �ٲ�� ȣ��Ǵ� �Լ�
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("������ Ŭ���̾�Ʈ ����:" + newMasterClient.ToString());
    }
    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    int num = 0;
    //    PlayerInfo[] playerNum =FindObjectsOfType<PlayerInfo>();
    //    for(int i=0;i<playerNum.Length;i++)
    //    {
    //        if (playerNum[i].playerState == state.Die) num++;
    //    }
    //    GameMgr.Instance.PlayerNum(num);
    //}
}

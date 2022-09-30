using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class GameSceneLogic : MonoBehaviourPunCallbacks
{
    int myNum = -1;
    SpawnMgr spawnMgr = null;
    [SerializeField] GameObject[] posStart;
    [SerializeField] GameObject blackscene;

    private void Awake()
    {
        spawnMgr = GameObject.FindObjectOfType<SpawnMgr>();
        PhotonNetwork.AutomaticallySyncScene = false;
    }
    void Start()
    {
        Vector3 pos = Vector3.zero;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;
        blackscene.SetActive(true);
        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                myNum = i;
            }
        }
        if (PhotonNetwork.IsConnected)
        {
            if (myNum == 0) pos = posStart[0].transform.position;
            if (myNum == 1) pos = posStart[1].transform.position;
            if (myNum == 2) pos = posStart[2].transform.position;
            if (myNum == 3) pos = posStart[3].transform.position;


            GameObject player = PhotonNetwork.Instantiate("PlayerPrefab", pos, Quaternion.identity);
            GameMgr.Instance.followCam.playerStart(player.transform);

            //내가 마스터클라이언트일 경우만 아이템 및 파란 영혼 생성
            if (PhotonNetwork.IsMasterClient)
            {
                GameMgr.Instance.spawnMgr.photonView.RPC("ItemInit", RpcTarget.MasterClient);
                GameMgr.Instance.spawnMgr.photonView.RPC("SoulInit", RpcTarget.MasterClient);
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }
    //마스터클라이언트가 바뀌면 호출되는 함수
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("마스터 클라이언트 변경:" + newMasterClient.ToString());
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

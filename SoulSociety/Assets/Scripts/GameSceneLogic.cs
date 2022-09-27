using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


public class GameSceneLogic : MonoBehaviourPunCallbacks
{
    int myNum = -1;
    void Start()
    {
        Vector3 pos= Vector3.zero;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;
        for(int i = 0; i < sortedPlayers.Length; i++)
        {
            if(sortedPlayers[i].NickName == PhotonNetwork.NickName)
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
      
            GameObject player = PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);
            //if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            //{
            //    Renderer[] mat = player.GetComponentsInChildren<Renderer>();
            //    for (int i = 0; i < mat.Length; i++)
            //        mat[i].material.color = Color.magenta;
            //}
            //if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
            //{
            //    Renderer[] mat = player.GetComponentsInChildren<Renderer>();
            //    for (int i = 0; i < mat.Length; i++)
            //        mat[i].material.color = Color.green;
            //}
            //if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
            //{
            //    Renderer[] mat = player.GetComponentsInChildren<Renderer>();
            //    for (int i = 0; i < mat.Length; i++)
            //        mat[i].material.color = Color.yellow;
            //}
            //if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
            //{
            //    Renderer[] mat = player.GetComponentsInChildren<Renderer>();
            //    for (int i = 0; i < mat.Length; i++)
            //        mat[i].material.color = Color.white;
            //}

            GameMgr.Instance.followCam.playerStart(player.transform);
            
        }
    }
    //마스터클라이언트가 바뀌면 호출되는 함수
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("마스터 클라이언트 변경:" + newMasterClient.ToString());
    }
}

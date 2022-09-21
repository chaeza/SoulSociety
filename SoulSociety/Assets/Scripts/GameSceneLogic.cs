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
            GameObject a = PhotonNetwork.Instantiate("Player", pos, Quaternion.identity);
            GameMgr.Instance.followCam.playerStart(a.transform);
            GameMgr.Instance.randomSkill.GetRandomSkill(a);
        }
    }

    //마스터클라이언트가 바뀌면 호출되는 함수
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("마스터 클라이언트 변경:" + newMasterClient.ToString());
    }
}

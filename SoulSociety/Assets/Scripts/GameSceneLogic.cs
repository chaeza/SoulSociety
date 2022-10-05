using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;


public class GameSceneLogic : MonoBehaviourPunCallbacks
{
    int myNum = -1;
    [SerializeField] GameObject[] posStart;
    [SerializeField] GameObject blackscene;

    TitleToGameScene titleToGameScene;
    List<string> sessionIDs = new List<string>();

    private void Awake()
    {
        titleToGameScene = FindObjectOfType<TitleToGameScene>();
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
            
            //API개인 SessionID 지급
            player.GetPhotonView().RPC("MySessionID", RpcTarget.All, titleToGameScene.session_ID);
            

            GameMgr.Instance.followCam.playerStart(player.transform);
        }
    }

    void Start()
    {
       
        //내가 마스터클라이언트일 경우만 아이템 및 파란 영혼 생성
        if (PhotonNetwork.IsMasterClient)
        {
            GameMgr.Instance.spawnMgr.photonView.RPC("ItemInit", RpcTarget.MasterClient);
            GameMgr.Instance.spawnMgr.photonView.RPC("SoulInit", RpcTarget.MasterClient);
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

    }
    //마스터클라이언트가 바뀌면 호출되는 함수
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("마스터 클라이언트 변경:" + newMasterClient.ToString());
    }

    [PunRPC]
    public void RPC_All_SessionID(string ID)
    {
        sessionIDs.Add(ID);
         if(sessionIDs.Count==4)
        //API 베팅
        StartCoroutine(processRequestBetting_Zera());

    }
    void countSSS()
    {
        Debug.Log(sessionIDs.Count);
        Debug.Log(sessionIDs[sessionIDs.Count-1].ToString());

       
        /*for (int i = 0; i < sessionIDs.Count; i++)
        {
            Debug.Log("아이디" + sessionIDs[i]);
        }*/
    }




    public void WinnerEndGame()
    {
        //API 승자
        StartCoroutine(processRequestBetting_Zera_DeclareWinner());

        StartCoroutine(endTimer());

    }

    public void EndGame()
    {
        StartCoroutine(endTimer());
    }

    IEnumerator endTimer()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.LoadLevel("TitleScene");
        PhotonNetwork.LeaveRoom();
    }


    #region API
    [Header("[등록된 프로젝트에서 획득가능한 API 키]")]
    [SerializeField] string API_KEY = ""; 

    [Header("[Betting Backend Base URL]")]
    [SerializeField] string FullAppsProductionURL = "https://odin-api.browseosiris.com";
    [SerializeField] string FullAppsStagingURL = "https://odin-api-sat.browseosiris.com";

    string getBaseURL()
    {
        // 프로덕션 단계라면
        //return FullAppsProductionURL;

        // 스테이징 단계(개발)라면
        return FullAppsStagingURL;
    }

    //---------------
    // ZERA 베팅
    public void OnClick_Betting_Zera()//클릭시
    {
        StartCoroutine(processRequestBetting_Zera()); //게임 시작시 실행 
    }
    IEnumerator processRequestBetting_Zera()
    {
        Res_Initialize resBettingPlaceBet = null;
        Req_Initialize reqBettingPlaceBet = new Req_Initialize();
        //
        reqBettingPlaceBet.players_session_id = sessionIDs.ToArray();

        reqBettingPlaceBet.bet_id = titleToGameScene.bets_ID;// resSettigns.data.bets[0]._id;
        yield return requestCoinPlaceBet(reqBettingPlaceBet, (response) =>
        {
            if (response != null)
            {
                Debug.Log("## CoinPlaceBet : " + response.message);
                resBettingPlaceBet = response;
            }
        });
    }
    delegate void resCallback_BettingPlaceBet(Res_Initialize response);
    IEnumerator requestCoinPlaceBet(Req_Initialize req, resCallback_BettingPlaceBet callback)
    {
        string url = getBaseURL() + "/v1/betting/" + "zera" + "/place-bet";

        string reqJsonData = JsonUtility.ToJson(req);
        Debug.Log(reqJsonData);


        UnityWebRequest www = UnityWebRequest.Post(url, reqJsonData);
        byte[] buff = System.Text.Encoding.UTF8.GetBytes(reqJsonData);
        www.uploadHandler = new UploadHandlerRaw(buff);
        www.SetRequestHeader("api-key", API_KEY);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);
      
        Res_Initialize res = JsonUtility.FromJson<Res_Initialize>(www.downloadHandler.text);
        callback(res);

        Debug.Log("돈 냈음");
    }


    //---------------
    // ZERA 베팅-승자
    public void OnClick_Betting_Zera_DeclareWinner()
    {
        StartCoroutine(processRequestBetting_Zera_DeclareWinner());
    }
    IEnumerator processRequestBetting_Zera_DeclareWinner()
    {
        Res_BettingWinner resBettingDeclareWinner = null;
        Req_BettingWinner reqBettingDeclareWinner = new Req_BettingWinner();
        reqBettingDeclareWinner.betting_id = titleToGameScene.bets_ID;// resSettigns.data.bets[0]._id;
        reqBettingDeclareWinner.winner_player_id = titleToGameScene.user_ID;
        yield return requestCoinDeclareWinner(reqBettingDeclareWinner, (response) =>
        {
            if (response != null)
            {
                Debug.Log("## CoinDeclareWinner : " + response.message);
                resBettingDeclareWinner = response;
            }
        });
    }
    delegate void resCallback_BettingDeclareWinner(Res_BettingWinner response);
    IEnumerator requestCoinDeclareWinner(Req_BettingWinner req, resCallback_BettingDeclareWinner callback)
    {
        string url = getBaseURL() + "/v1/betting/" + "zera" + "/declare-winner";

        string reqJsonData = JsonUtility.ToJson(req);
        Debug.Log(reqJsonData);


        UnityWebRequest www = UnityWebRequest.Post(url, reqJsonData);
        byte[] buff = System.Text.Encoding.UTF8.GetBytes(reqJsonData);
        www.uploadHandler = new UploadHandlerRaw(buff);
        www.SetRequestHeader("api-key", API_KEY);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);
      
        Res_BettingWinner res = JsonUtility.FromJson<Res_BettingWinner>(www.downloadHandler.text);
        callback(res);

        Debug.Log("돈 가져와");
    }

    #endregion


}

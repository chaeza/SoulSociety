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
            
            //API���� SessionID ����
            player.GetPhotonView().RPC("MySessionID", RpcTarget.All, titleToGameScene.session_ID);
            

            GameMgr.Instance.followCam.playerStart(player.transform);
        }
    }

    void Start()
    {
       
        //���� ������Ŭ���̾�Ʈ�� ��츸 ������ �� �Ķ� ��ȥ ����
        if (PhotonNetwork.IsMasterClient)
        {
            GameMgr.Instance.spawnMgr.photonView.RPC("ItemInit", RpcTarget.MasterClient);
            GameMgr.Instance.spawnMgr.photonView.RPC("SoulInit", RpcTarget.MasterClient);
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

    }
    //������Ŭ���̾�Ʈ�� �ٲ�� ȣ��Ǵ� �Լ�
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("������ Ŭ���̾�Ʈ ����:" + newMasterClient.ToString());
    }

    [PunRPC]
    public void RPC_All_SessionID(string ID)
    {
        sessionIDs.Add(ID);
         if(sessionIDs.Count==4)
        //API ����
        StartCoroutine(processRequestBetting_Zera());

    }
    void countSSS()
    {
        Debug.Log(sessionIDs.Count);
        Debug.Log(sessionIDs[sessionIDs.Count-1].ToString());

       
        /*for (int i = 0; i < sessionIDs.Count; i++)
        {
            Debug.Log("���̵�" + sessionIDs[i]);
        }*/
    }




    public void WinnerEndGame()
    {
        //API ����
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
    [Header("[��ϵ� ������Ʈ���� ȹ�氡���� API Ű]")]
    [SerializeField] string API_KEY = ""; 

    [Header("[Betting Backend Base URL]")]
    [SerializeField] string FullAppsProductionURL = "https://odin-api.browseosiris.com";
    [SerializeField] string FullAppsStagingURL = "https://odin-api-sat.browseosiris.com";

    string getBaseURL()
    {
        // ���δ��� �ܰ���
        //return FullAppsProductionURL;

        // ������¡ �ܰ�(����)���
        return FullAppsStagingURL;
    }

    //---------------
    // ZERA ����
    public void OnClick_Betting_Zera()//Ŭ����
    {
        StartCoroutine(processRequestBetting_Zera()); //���� ���۽� ���� 
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

        Debug.Log("�� ����");
    }


    //---------------
    // ZERA ����-����
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

        Debug.Log("�� ������");
    }

    #endregion


}

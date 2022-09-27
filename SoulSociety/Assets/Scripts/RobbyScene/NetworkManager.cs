using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;
using Photon.Pun;



public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField NickNameInput;
    public GameObject DisconnectPanel;
    public GameObject RobbyPanel;
    public GameObject[] soulEff;
    public GameObject[] reddyButton;

    [SerializeField] Button btnConnect = null;
    [SerializeField] TextMeshProUGUI[] nickName = null;

    [SerializeField] Button soloStart = null;

    ReadyState myReadyState = ReadyState.None;

    int readyCount = 0;
    int myButtonNum = 0;

    //준비완료 상태를 받는 변수 하나 필요
    public enum ReadyState
    {
        None,
        Ready,
        UnReady,
    }


    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start()
    {
        for (int i = 0; i < soulEff.Length; i++)
        {
            soulEff[i].SetActive(false);
            reddyButton[i].GetComponent<Image>().color = Color.gray;
            reddyButton[i].GetComponent<Button>().interactable = false;
        }
        btnConnect.interactable = false; // 버튼 입력 막기
        RobbyPanel.SetActive(false);
        //마스터 서버 접속 요청
        PhotonNetwork.ConnectUsingSettings(); //Photon.Pun 내부 클래스
    }

    public override void OnConnectedToMaster()
    {
        btnConnect.interactable = true;
        Debug.Log("## OnConnected to Master");

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        DisconnectPanel.SetActive(true);
        btnConnect.interactable = false;
        RobbyPanel.SetActive(false);
    }
    // 이름 입력 컨트롤(inputField)
    public void OnEndEdit(string instr)
    {
        Debug.Log("!!!!!");
        PhotonNetwork.NickName = instr; //닉네임 할당
    }

    // 닉네임 밑에 커넥트 버튼 클릭시 
    public void OnClick_Connected()
    {
        if (string.IsNullOrEmpty(PhotonNetwork.NickName) == true)
            return;

        // PhotonNetwork.JoinOrCreateRoom("myroom", new RoomOptions { MaxPlayers = 4 }, null);

        //조인랜덤룸으로 생성방 우선 참가로직 
        PhotonNetwork.JoinRandomRoom();

        //기존 커넥트 버튼 오프 
        DisconnectPanel.SetActive(false);
        //로비패널 온 
        RobbyPanel.SetActive(true);
        //PhotonNetwork.LocalPlayer.NickName = nickName.text;

    }
    //입장할 방이 없으면 새로운 방 생성
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("조인 실패");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
    //자신이 들어갈때 
    public override void OnJoinedRoom()
    {
        Debug.Log("새로운 플레이어가 참가하셨습니다");
        myReadyState = ReadyState.UnReady;
        //SortedPlayer();
        gameObject.GetPhotonView().RPC("SortedPlayer", RpcTarget.All);
    }
    //타인이 들어올때
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("새로운 플레이어가 참가하셨습니다");
        //SortedPlayer();
        gameObject.GetPhotonView().RPC("SortedPlayer", RpcTarget.All);
    }
   //플레이어가 나갈때
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearLobby();
       // SortedPlayer();
        gameObject.GetPhotonView().RPC("SortedPlayer", RpcTarget.All);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("마스터 클라이언트 변경:" + newMasterClient.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// --------------------------------------
    /// </summary>




    #region 플레이어 자리 초기화
    public void ClearLobby()
    {
        //대기창 초기화 
        for (int i = 0; i < nickName.Length; i++)
        {
            nickName[i].text = " ";
            soulEff[i].SetActive(false);
            reddyButton[i].GetComponent<Image>().color = Color.gray;
            reddyButton[i].GetComponent<Button>().interactable = false;
        }
    }
    #endregion


    #region 플레이어 정렬
    [PunRPC]
    public void SortedPlayer()
    {
        readyCount = 0;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            //자신의 버튼만 활성화 하기 
            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                Debug.Log("i : " + i);
                myButtonNum = i;
                reddyButton[myButtonNum].GetComponent<Button>().interactable = true; //나만 누르기 위해 활성화
                
                //내 상태가 레디면 노란색 -->그런데 이건 RPC상에서 표현 해줘야해
                 gameObject.GetPhotonView().RPC("ButtonColor", RpcTarget.All, myReadyState, myButtonNum);
            }
            // gameObject.GetPhotonView().RPC("stateCheck", RpcTarget.All, myReadyState, i);

            if (reddyButton[i].GetComponent<Image>().color==Color.yellow)
                readyCount++;

            nickName[i].text = sortedPlayers[i].NickName;
            soulEff[i].SetActive(true);
            LoadScene();
        }
        Debug.Log("레디 숫자 : " + readyCount);
      
    }
    #endregion
    //각각의 플레이어 상태에 따른 색 표현 
    [PunRPC]
    public void ButtonColor(ReadyState readyState,int buttonNum)
    {
        if (readyState == ReadyState.Ready)
            reddyButton[buttonNum].GetComponent<Image>().color = Color.yellow;
        else
            reddyButton[buttonNum].GetComponent<Image>().color = Color.grey;
    }

    #region 게임 실행
    public void LoadScene()
    {
        // 마스터일때만 해당 함수 실행 가능
        if (PhotonNetwork.IsMasterClient)
        {
            if (readyCount == 4)
            {
                Debug.Log("시작");
                //4명 레디 완료시 2초후 게임 실행 
                photonView.StartCoroutine(MainStartTimer());
            }
        }
        // Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
    }
    #endregion

    [PunRPC]
    public void ReadyCounT(string nickname)
    {
        readyCount++;
    }

    [PunRPC]
    public void UnReadyCounT()
    {
        readyCount--;
    }
    
    public void SoloClick()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }


    #region 버튼 클릭
    public void ButtonClick()
    {
        readyCount = 0;
        if (myReadyState == ReadyState.Ready)
        {
            myReadyState = ReadyState.UnReady;
            //상태 전환 후 마스터 레디카운트 타운
            // gameObject.GetPhotonView().RPC("UnReadyCounT", RpcTarget.All);

            //2번째
            //SortedPlayer();
            gameObject.GetPhotonView().RPC("SortedPlayer", RpcTarget.All);
        }
        else
        {
            myReadyState = ReadyState.Ready;
            //상태 전환 후 마스터 레디카운트 업 
            //  gameObject.GetPhotonView().RPC("ReadyCounT", RpcTarget.All);
            //레디 전환 후 시작 확인 

            //2번째
            //SortedPlayer();
            gameObject.GetPhotonView().RPC("SortedPlayer", RpcTarget.All);

        }
    }


    #endregion 









    //게임 시작 2초 지연
    IEnumerator MainStartTimer()
    {
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("GameScene");
    }
}

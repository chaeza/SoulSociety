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
    public RawImage brokenWindow;
    public AudioSource audioSource;

    [SerializeField] Button btnConnect = null;
    [SerializeField] TextMeshProUGUI[] nickName = null;
    [SerializeField] Button soloStart = null;

    //API 잔고 
    [SerializeField] TextMeshProUGUI Balance_Lobby;
    [SerializeField] TextMeshProUGUI Balance_Room;



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
    public void SoloClick()
    {
        PhotonNetwork.LoadLevel("LoadingScene");
      //  PhotonNetwork.LoadLevel("GameScene");
    }
    private void Awake()
    {
        ClearLobby();
        photonView.StartCoroutine(AutoSyncDelay());
        
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    IEnumerator AutoSyncDelay()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        brokenWindow.gameObject.SetActive(false);
        audioSource.gameObject.SetActive(false);
        StartCoroutine(broken());
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
     
       // 불끄기
        ClearLobby();
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

        //조인랜덤룸으로 생성방 우선 참가로직 
        PhotonNetwork.JoinRandomRoom();

        //기존 커넥트 버튼 오프 
        DisconnectPanel.SetActive(false);
        //로비패널 온 
        RobbyPanel.SetActive(true);
    }
    //입장할 방이 없으면 새로운 방 생성
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("조인 실패");
        //맥스 인원과 방 상태 표현 (시작인지 아닌지)
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4,IsOpen=true });
    }
    //자신이 들어갈때 
    public override void OnJoinedRoom()
    {
        Debug.Log("새로운 플레이어가 참가하셨습니다");
        Player[] nickNameCheck = PhotonNetwork.PlayerList;
        int checkNum=0;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (nickNameCheck[i].NickName == PhotonNetwork.NickName)
            {
                checkNum++;
                if (checkNum > 1)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel("TitleScene");
                }
            }
        }

        myReadyState = ReadyState.UnReady;
        SortedPlayer();
    }

    //타인이 들어올때
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("새로운 플레이어가 참가하셨습니다");
        SortedPlayer();
    }

    //플레이어가 나갈때
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearLobby();
        SortedPlayer();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("마스터 클라이언트 변경:" + newMasterClient.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LoadLevel("TitleScene");
            PhotonNetwork.LeaveRoom();
        }
    }

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
    public void SortedPlayer()
    {
        gameObject.GetPhotonView().RPC("ZeroCounT", RpcTarget.MasterClient);
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            nickName[i].text = sortedPlayers[i].NickName;
            soulEff[i].SetActive(true);
            //자신의 버튼만 활성화 하기 
            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                Debug.Log("i : " + i);
                myButtonNum = i;
                reddyButton[myButtonNum].GetComponent<Button>().interactable = true; //나만 누르기 위해 활성화

                //내 상태가 레디면 노란색 -->그런데 이건 서버에서 표현 해줘야 하기 때문에 RPC함수 사용
                gameObject.GetPhotonView().RPC("ButtonColor", RpcTarget.All, myReadyState, myButtonNum);
            }

            if (reddyButton[i].GetComponent<Image>().color == Color.yellow)
            {
                gameObject.GetPhotonView().RPC("ReadyCounT", RpcTarget.MasterClient);
            }
        }

    }
    #endregion
    //각각의 플레이어 상태에 따른 색 표현 
    [PunRPC]
    public void ButtonColor(ReadyState readyState, int buttonNum)
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
                //4명 레디 완료시 2초후 게임 실행 코루틴 
                photonView.StartCoroutine(MainStartTimer());
            }
        }
    }
    #endregion

    [PunRPC]
    void ReadyCounT()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            readyCount++;
            LoadScene();
            Debug.Log("레디 숫자 : " + readyCount);
        }
    }
    [PunRPC]
    void ZeroCounT()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            readyCount = 0;
            Debug.Log("레디 숫자 : " + readyCount);
        }
    }


    #region 버튼 클릭
    public void ButtonClick()
    {
        gameObject.GetPhotonView().RPC("ZeroCounT", RpcTarget.MasterClient);
        if (myReadyState == ReadyState.Ready)
        {
            myReadyState = ReadyState.UnReady;
            SortedPlayer();
        }
        else
        {
            myReadyState = ReadyState.Ready;
            SortedPlayer();
        }
    }
    #endregion 
    //게임 시작 2초 지연
    [PunRPC]
    public void RPC_ClearLobby()
    {
        ClearLobby();
    }
    IEnumerator MainStartTimer()
    {
        yield return new WaitForSeconds(2);
        if (readyCount == 4)
        {
            PhotonNetwork.LoadLevel("LoadingScene");
        }
        else Debug.Log("누군가 레디 취소함");
    }

    IEnumerator broken()
    {
        yield return new WaitForSeconds(13);

        brokenWindow.gameObject.SetActive(true);
        audioSource.gameObject.SetActive(true);
    }
}

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
        //싹다 초기화 재설정
        for (int i = 0; i < nickName.Length; i++)
        {
            nickName[i].text = " ";
            soulEff[i].SetActive(false);
            reddyButton[i].GetComponent<Image>().color = Color.gray;
            reddyButton[i].GetComponent<Button>().interactable = false;
        }
        SortedPlayer();
       
    }
    //타인이 들어올때
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("새로운 플레이어가 참가하셨습니다");

        SortedPlayer();
    }
    [PunRPC]
    public void CheckLeaveName(string nickname,ReadyState mystate)
    {
        Debug.Log("남은 사람 이름 : "+nickname.ToString()+mystate.ToString());
        if (mystate == ReadyState.Ready)
        {
            readyCount++;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //초기화 
        readyCount = 0;

        for (int i = 0; i < nickName.Length; i++)
        {
            nickName[i].text = " ";
            soulEff[i].SetActive(false);
            reddyButton[i].GetComponent<Image>().color = Color.gray;
            reddyButton[i].GetComponent<Button>().interactable = false;
        }
        SortedPlayer();
        //남은 사람 확인
        photonView.RPC("CheckLeaveName", RpcTarget.All, PhotonNetwork.NickName,myReadyState);
        Debug.Log("초기화된 카운트 : " + readyCount);
    }

    #region 플레이어 정렬
    public void SortedPlayer()
    {
        Player[] sortedPlayers = PhotonNetwork.PlayerList;
        readyCount = 0;

        //대기창 초기화 
        for (int i = 0; i < nickName.Length; i++)
        {
            nickName[i].text = " ";
            soulEff[i].SetActive(false);
            reddyButton[i].GetComponent<Image>().color = Color.gray;
            reddyButton[i].GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                Debug.Log("i : " + i);
                myButtonNum = i;
                reddyButton[myButtonNum].GetComponent<Button>().interactable = true;
                transform.LookAt
            }
            nickName[i].text = sortedPlayers[i].NickName;
            soulEff[i].SetActive(true);
            LoadScene();
        }
    }
    #endregion

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

    //버튼 클릭시 
    #region 버튼 클릭
    public void PushReady()
    {
        Debug.Log("누름");

        Debug.Log("내꺼");
        if (myReadyState != ReadyState.Ready)
        {
            Debug.Log("맞아");
            myReadyState = ReadyState.Ready;
            reddyButton[myButtonNum].GetComponent<Image>().color = Color.yellow;
            LoadScene();
        }
        else
        {
            myReadyState = ReadyState.UnReady;
            reddyButton[myButtonNum].GetComponent<Image>().color = Color.gray;
        }
        gameObject.GetPhotonView().RPC("MyState", RpcTarget.All, myReadyState,myButtonNum);
    }
    #endregion

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("마스터 클라이언트 변경:" + newMasterClient.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.LeaveRoom();
    }


    #region 플레이어 버튼 클릭시 색 변환 전환
    [PunRPC]
    public void MyState(ReadyState state,int buttonNum)
    {
        if (state == ReadyState.Ready)
        {
          //  readyCount++;
            reddyButton[buttonNum].GetComponent<Image>().color = Color.yellow;
            LoadScene();
        }
        else
        {
            reddyButton[buttonNum].GetComponent<Image>().color = Color.gray;
          //  readyCount--;
        }

        Debug.Log("레디 숫자 : " + readyCount);
    }
    #endregion

    #region 상태 체크
    [PunRPC]
    public void stateCheck(ReadyState state, int buttonNum)
    {
        if (state == ReadyState.Ready)
            reddyButton[buttonNum].GetComponent<Image>().color = Color.yellow;
    }
    #endregion


    IEnumerator MainStartTimer()
    {
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("GameScene");
    }
}

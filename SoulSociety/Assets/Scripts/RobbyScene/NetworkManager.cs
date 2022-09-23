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

    [SerializeField] Button btnConnect = null;
    [SerializeField] TextMeshProUGUI[] nickName = null;


    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start()
    {   for(int i = 0; i < soulEff.Length; i++)
        {
            soulEff[i].SetActive(false);
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

    public void OnClick_Connected()
    {
        if (string.IsNullOrEmpty(PhotonNetwork.NickName) == true)
            return;
 
        PhotonNetwork.JoinOrCreateRoom("myroom", new RoomOptions { MaxPlayers = 4 }, null);
        DisconnectPanel.SetActive(false);
        RobbyPanel.SetActive(true);
        //PhotonNetwork.LocalPlayer.NickName = nickName.text;

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("새로운 플레이어가 참가하셨습니다");
        //int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        /* Player[] sortedPlayers = PhotonNetwork.PlayerList;
         Debug.Log("현재 방에 나 등장");
         //Debug.Log(actorNumber + "##번호");
         for (int i = 0; i < sortedPlayers.Length; i++)
         {
             Debug.Log(sortedPlayers[i].NickName);
             nickName[i].text = sortedPlayers[i].NickName;
         }*/
        SortedPlayer();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("새로운 플레이어가 참가하셨습니다");
        SortedPlayer();
        LoadScene();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("누가나감");
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < nickName.Length; i++)
        {
            Debug.Log("비워");
            nickName[i].text = " ";
            soulEff[i].SetActive(false);
        }
        SortedPlayer();
        
    }

    public void SortedPlayer()
    {
        //int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;
        Debug.Log(sortedPlayers.Length);
     
        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            Debug.Log(sortedPlayers[i].NickName);
            nickName[i].text = sortedPlayers[i].NickName;
            soulEff[i].SetActive(true);
        }
    }

    public void LoadScene()
    {
        // 마스터일때만 해당 함수 실행 가능
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("나는 마스터가 아닙니다");
            if (PhotonNetwork.CurrentRoom.PlayerCount > 3)
            {
                Debug.Log("다음씬으로");
                PhotonNetwork.LoadLevel("GameScene");
            }
        }
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("마스터 클라이언트 변경:" + newMasterClient.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.LeaveRoom();

        
    }

    

}

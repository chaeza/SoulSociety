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

    public GameObject prefab;

    [SerializeField] Button btnConnect = null;
    [SerializeField] TextMeshProUGUI[] nickName = null;


    private void Awake()
    {
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

    }
    private void Start()
    {
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
        Instantiate(prefab);

        Debug.Log("방으로들어옴");
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        Debug.Log(actorNumber + "##번호");
        for(int i = 0; i < sortedPlayers.Length; i++)
        {
            if (sortedPlayers[i].ActorNumber == actorNumber)
            { 
                nickName[i].text = PhotonNetwork.LocalPlayer.NickName;
            }       
            
        }

        //StartCoroutine(check());
        
       

    }

   IEnumerator check()
    {
        yield return new WaitForSeconds(10);
        Debug.Log(PhotonNetwork.CountOfPlayersInRooms + "##나 제외 현재인원");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();

        if (PhotonNetwork.CountOfPlayersInRooms == 4)
        {
            Debug.Log("다음씬으로");
            PhotonNetwork.LoadLevel("GameScene");
        }

    }

    

}

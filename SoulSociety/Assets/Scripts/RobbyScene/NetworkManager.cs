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
        btnConnect.interactable = false; // ��ư �Է� ����
        RobbyPanel.SetActive(false);
        //������ ���� ���� ��û
        PhotonNetwork.ConnectUsingSettings(); //Photon.Pun ���� Ŭ����


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
    // �̸� �Է� ��Ʈ��(inputField)
    public void OnEndEdit(string instr)
    {
        Debug.Log("!!!!!");
        PhotonNetwork.NickName = instr; //�г��� �Ҵ�
        
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
        Debug.Log("���ο� �÷��̾ �����ϼ̽��ϴ�");
        //int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        /* Player[] sortedPlayers = PhotonNetwork.PlayerList;
         Debug.Log("���� �濡 �� ����");
         //Debug.Log(actorNumber + "##��ȣ");
         for (int i = 0; i < sortedPlayers.Length; i++)
         {
             Debug.Log(sortedPlayers[i].NickName);
             nickName[i].text = sortedPlayers[i].NickName;
         }*/
        SortedPlayer();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("���ο� �÷��̾ �����ϼ̽��ϴ�");
        SortedPlayer();
        LoadScene();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("��������");
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < nickName.Length; i++)
        {
            Debug.Log("���");
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
        // �������϶��� �ش� �Լ� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("���� �����Ͱ� �ƴմϴ�");
            if (PhotonNetwork.CurrentRoom.PlayerCount > 3)
            {
                Debug.Log("����������");
                PhotonNetwork.LoadLevel("GameScene");
            }
        }
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("������ Ŭ���̾�Ʈ ����:" + newMasterClient.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.LeaveRoom();

        
    }

    

}

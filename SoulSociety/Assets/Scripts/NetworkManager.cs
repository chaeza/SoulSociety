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
        Instantiate(prefab);

        Debug.Log("�����ε���");
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        Debug.Log(actorNumber + "##��ȣ");
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
        Debug.Log(PhotonNetwork.CountOfPlayersInRooms + "##�� ���� �����ο�");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();

        if (PhotonNetwork.CountOfPlayersInRooms == 4)
        {
            Debug.Log("����������");
            PhotonNetwork.LoadLevel("GameScene");
        }

    }

    

}

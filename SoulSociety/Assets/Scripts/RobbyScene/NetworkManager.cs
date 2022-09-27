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

    //�غ�Ϸ� ���¸� �޴� ���� �ϳ� �ʿ�
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

    // �г��� �ؿ� Ŀ��Ʈ ��ư Ŭ���� 
    public void OnClick_Connected()
    {
        if (string.IsNullOrEmpty(PhotonNetwork.NickName) == true)
            return;

        // PhotonNetwork.JoinOrCreateRoom("myroom", new RoomOptions { MaxPlayers = 4 }, null);

        //���η��������� ������ �켱 �������� 
        PhotonNetwork.JoinRandomRoom();

        //���� Ŀ��Ʈ ��ư ���� 
        DisconnectPanel.SetActive(false);
        //�κ��г� �� 
        RobbyPanel.SetActive(true);
        //PhotonNetwork.LocalPlayer.NickName = nickName.text;

    }
    //������ ���� ������ ���ο� �� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("���� ����");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
    //�ڽ��� ���� 
    public override void OnJoinedRoom()
    {
        Debug.Log("���ο� �÷��̾ �����ϼ̽��ϴ�");
        myReadyState = ReadyState.UnReady;
        SortedPlayer();
    }
    //Ÿ���� ���ö�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("���ο� �÷��̾ �����ϼ̽��ϴ�");
        SortedPlayer();
    }
    int outNum;
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        for (int i = 0; i < nickName.Length; i++)
        {
            nickName[i].text = " ";
            soulEff[i].SetActive(false);
            reddyButton[i].GetComponent<Button>().interactable = false;
        }
        SortedPlayer();
    }

    public void SortedPlayer()
    {
        Player[] sortedPlayers = PhotonNetwork.PlayerList;
        readyCount = 0;
        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            Debug.Log("Net : " + PhotonNetwork.LocalPlayer.ActorNumber + "View : " + PhotonNetwork.NickName + photonView.Controller.NickName); //photonView.Controller.NickName --> ������ �÷��̾��� �г����� ã��photonView.Controller.Actornumber --> photonNetwork.localPlayer.Actor 

            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                Debug.Log("i : " + i);
                myButtonNum = i;
                reddyButton[myButtonNum].GetComponent<Button>().interactable = true;
             
            }
            nickName[i].text = sortedPlayers[i].NickName;
            soulEff[i].SetActive(true);
            LoadScene();
        }

    }

    public void LoadScene()
    {
        // �������϶��� �ش� �Լ� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            if (readyCount == 4)
            {
                Debug.Log("����");
                //4�� ���� �Ϸ�� 2���� ���� ���� 
                photonView.StartCoroutine(MainStartTimer());
            }
        }
        // Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("������ Ŭ���̾�Ʈ ����:" + newMasterClient.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.LeaveRoom();
    }
    //��ư Ŭ���� 
    public void PushReady()
    {
        Debug.Log("����");

        Debug.Log("����");
        if (myReadyState != ReadyState.Ready)
        {
            Debug.Log("�¾�");
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
    #region �÷��̾� ���� ���� 
    [PunRPC]
    public void MyState(ReadyState state,int buttonNum)
    {
        if (state == ReadyState.Ready)
        {
            readyCount++;
            reddyButton[buttonNum].GetComponent<Image>().color = Color.yellow;
            LoadScene();
        }
        else
        {
            reddyButton[buttonNum].GetComponent<Image>().color = Color.gray;
            readyCount--;
        }

        Debug.Log("���� ���� : " + readyCount);
    }
    #endregion
    [PunRPC]
    public void stateCheck(ReadyState state)
    {
        if (state == ReadyState.Ready)
            readyCount++;
    }



    IEnumerator MainStartTimer()
    {
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("GameScene");
    }
}

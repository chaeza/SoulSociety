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
    int myNum = 0;
    int leftNum = 0;

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
        Player[] sortedPlayers = PhotonNetwork.PlayerList;
        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            //�ڽ��� ��ư�� Ȱ��ȭ �ϱ� 
            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                myNum = i;
            }
        }//��� ������ �����ѹ��� ����



        myReadyState = ReadyState.UnReady;
        //�ϴ� �ʱ�ȭ �缳��
        for (int i = 0; i < nickName.Length; i++)
        {
            nickName[i].text = " ";
            soulEff[i].SetActive(false);
            reddyButton[i].GetComponent<Image>().color = Color.gray;
            reddyButton[i].GetComponent<Button>().interactable = false;
        }
        SortedPlayer();
        //photonView.RPC("YourReady", RpcTarget.All);

    }
    //Ÿ���� ���ö�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("���ο� �÷��̾ �����ϼ̽��ϴ�");

        SortedPlayer();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        SortedPlayer();
        //���� ��� Ȯ��
            readyCount--;
            Debug.Log("�ʱ�ȭ�� ī��Ʈ : " + readyCount);
            photonView.RPC("YourReady", RpcTarget.All);
    }

    #region �÷��̾� ����
    public void SortedPlayer()
    {
        Player[] sortedPlayers = PhotonNetwork.PlayerList;
        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            //�ڽ��� ��ư�� Ȱ��ȭ �ϱ� 
            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                leftNum = i;
            }
        }
        if (leftNum != myNum) leftNum++;


        Player[] sortedPlayers = PhotonNetwork.PlayerList;
        //���â �ʱ�ȭ 
        for (int i = 0; i < nickName.Length; i++)
        {
            nickName[i].text = " ";
            soulEff[i].SetActive(false);
            reddyButton[i].GetComponent<Image>().color = Color.gray;
            reddyButton[i].GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            //�ڽ��� ��ư�� Ȱ��ȭ �ϱ� 
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
    #endregion

    #region ���� ����
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
    #endregion

    //��ư Ŭ���� 
    #region ��ư Ŭ��
    public void PushReady()
    {
        if (myReadyState != ReadyState.Ready)
        {
            myReadyState = ReadyState.Ready;
            reddyButton[myButtonNum].GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            myReadyState = ReadyState.UnReady;
            reddyButton[myButtonNum].GetComponent<Image>().color = Color.gray;
        }
        gameObject.GetPhotonView().RPC("MyState", RpcTarget.All, myReadyState, myButtonNum);
    }
    #endregion

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("������ Ŭ���̾�Ʈ ����:" + newMasterClient.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.LeaveRoom();
    }


    #region �÷��̾� ��ư Ŭ���� �� ��ȯ ��ȯ
    [PunRPC]
    public void MyState(ReadyState state, int buttonNum)
    {
        if (state == ReadyState.Ready)
        {
            reddyButton[buttonNum].GetComponent<Image>().color = Color.yellow;

            if (photonView.IsMine)
            {
                photonView.RPC("IamReady", RpcTarget.MasterClient, state);
                LoadScene();
            }
        }
        else
        {
            if (photonView.IsMine)
            {
                photonView.RPC("IamReady", RpcTarget.MasterClient, state);
                reddyButton[buttonNum].GetComponent<Image>().color = Color.gray;
            }
        }
    }
    #endregion

    IEnumerator MainStartTimer()
    {
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("GameScene");
    }
    [PunRPC]
    void YourReady()
    {
        if (myReadyState != ReadyState.Ready)
        {
            myReadyState = ReadyState.Ready;
            reddyButton[myButtonNum].GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            myReadyState = ReadyState.UnReady;
            reddyButton[myButtonNum].GetComponent<Image>().color = Color.gray;
        }
        gameObject.GetPhotonView().RPC("MyState", RpcTarget.All, myReadyState, myButtonNum);
    }
    [PunRPC]
    void IamReady(ReadyState pstate)
    {
        if (pstate == ReadyState.Ready)
        {
            if (PhotonNetwork.IsMasterClient)
                readyCount++;
            Debug.Log(readyCount);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
                readyCount--;
            Debug.Log(readyCount);
        }
    }
}
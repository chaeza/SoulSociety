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

<<<<<<< HEAD

=======
>>>>>>> main
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
<<<<<<< HEAD


=======
>>>>>>> main
    private void Awake()
    {

        PhotonNetwork.AutomaticallySyncScene = true;
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
        //�ƽ� �ο��� �� ���� ǥ�� (�������� �ƴ���)
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4,IsOpen=true });
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
    //�÷��̾ ������
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearLobby();
        SortedPlayer();
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("������ Ŭ���̾�Ʈ ����:" + newMasterClient.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// --------------------------------------
    /// </summary>


    public void SoloClick()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    #region �÷��̾� �ڸ� �ʱ�ȭ
    public void ClearLobby()
    {
        //���â �ʱ�ȭ 
        for (int i = 0; i < nickName.Length; i++)
        {
            nickName[i].text = " ";
            soulEff[i].SetActive(false);
            reddyButton[i].GetComponent<Image>().color = Color.gray;
            reddyButton[i].GetComponent<Button>().interactable = false;
        }
    }
    #endregion


    #region �÷��̾� ����
    public void SortedPlayer()
    {
        gameObject.GetPhotonView().RPC("ZeroCounT", RpcTarget.MasterClient);
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            nickName[i].text = sortedPlayers[i].NickName;
            soulEff[i].SetActive(true);
            //�ڽ��� ��ư�� Ȱ��ȭ �ϱ� 
            if (sortedPlayers[i].NickName == PhotonNetwork.NickName)
            {
                Debug.Log("i : " + i);
                myButtonNum = i;
                reddyButton[myButtonNum].GetComponent<Button>().interactable = true; //���� ������ ���� Ȱ��ȭ

                //�� ���°� ����� ����� -->�׷��� �̰� RPC�󿡼� ǥ�� �������
                gameObject.GetPhotonView().RPC("ButtonColor", RpcTarget.All, myReadyState, myButtonNum);
            }
            // gameObject.GetPhotonView().RPC("stateCheck", RpcTarget.All, myReadyState, i);

            if (reddyButton[i].GetComponent<Image>().color == Color.yellow)
            {
                gameObject.GetPhotonView().RPC("ReadyCounT", RpcTarget.MasterClient);
            }
        }

    }
    #endregion
    //������ �÷��̾� ���¿� ���� �� ǥ�� 
    [PunRPC]
    public void ButtonColor(ReadyState readyState, int buttonNum)
    {
        if (readyState == ReadyState.Ready)
            reddyButton[buttonNum].GetComponent<Image>().color = Color.yellow;
        else
            reddyButton[buttonNum].GetComponent<Image>().color = Color.grey;
    }

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

    [PunRPC]
    void ReadyCounT()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            readyCount++;
            LoadScene();
            Debug.Log("���� ���� : " + readyCount);
        }
    }
    [PunRPC]
    void ZeroCounT()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            readyCount = 0;
            Debug.Log("���� ���� : " + readyCount);
        }
    }

    #region ��ư Ŭ��
    public void ButtonClick()
    {
        gameObject.GetPhotonView().RPC("ZeroCounT", RpcTarget.MasterClient);
        if (myReadyState == ReadyState.Ready)
        {
            myReadyState = ReadyState.UnReady;
            //���� ��ȯ �� ������ ����ī��Ʈ �ٿ�
            // gameObject.GetPhotonView().RPC("UnReadyCounT", RpcTarget.All);

            //2��°
            SortedPlayer();
        }
        else
        {
            myReadyState = ReadyState.Ready;
            //���� ��ȯ �� ������ ����ī��Ʈ �� 
            //  gameObject.GetPhotonView().RPC("ReadyCounT", RpcTarget.All);
            //���� ��ȯ �� ���� Ȯ�� 

            //2��°
            SortedPlayer();

        }
    }
    #endregion 
    //���� ���� 2�� ����
    IEnumerator MainStartTimer()
    {
        yield return new WaitForSeconds(2);
        if (readyCount == 4)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
        else Debug.Log("������ ���� �����");
    }
}

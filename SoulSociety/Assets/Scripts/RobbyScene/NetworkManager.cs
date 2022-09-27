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
        //SortedPlayer();
        gameObject.GetPhotonView().RPC("SortedPlayer", RpcTarget.All);
    }
    //Ÿ���� ���ö�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("���ο� �÷��̾ �����ϼ̽��ϴ�");
        //SortedPlayer();
        gameObject.GetPhotonView().RPC("SortedPlayer", RpcTarget.All);
    }
   //�÷��̾ ������
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearLobby();
       // SortedPlayer();
        gameObject.GetPhotonView().RPC("SortedPlayer", RpcTarget.All);
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
    [PunRPC]
    public void SortedPlayer()
    {
        readyCount = 0;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < sortedPlayers.Length; i++)
        {
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

            if (reddyButton[i].GetComponent<Image>().color==Color.yellow)
                readyCount++;

            nickName[i].text = sortedPlayers[i].NickName;
            soulEff[i].SetActive(true);
            LoadScene();
        }
        Debug.Log("���� ���� : " + readyCount);
      
    }
    #endregion
    //������ �÷��̾� ���¿� ���� �� ǥ�� 
    [PunRPC]
    public void ButtonColor(ReadyState readyState,int buttonNum)
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


    #region ��ư Ŭ��
    public void ButtonClick()
    {
        readyCount = 0;
        if (myReadyState == ReadyState.Ready)
        {
            myReadyState = ReadyState.UnReady;
            //���� ��ȯ �� ������ ����ī��Ʈ Ÿ��
            // gameObject.GetPhotonView().RPC("UnReadyCounT", RpcTarget.All);

            //2��°
            //SortedPlayer();
            gameObject.GetPhotonView().RPC("SortedPlayer", RpcTarget.All);
        }
        else
        {
            myReadyState = ReadyState.Ready;
            //���� ��ȯ �� ������ ����ī��Ʈ �� 
            //  gameObject.GetPhotonView().RPC("ReadyCounT", RpcTarget.All);
            //���� ��ȯ �� ���� Ȯ�� 

            //2��°
            //SortedPlayer();
            gameObject.GetPhotonView().RPC("SortedPlayer", RpcTarget.All);

        }
    }


    #endregion 









    //���� ���� 2�� ����
    IEnumerator MainStartTimer()
    {
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel("GameScene");
    }
}

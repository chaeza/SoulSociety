using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Networking;
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

    //API �ܰ� 
    [SerializeField] TextMeshProUGUI Balance_Disconnect;
    [SerializeField] TextMeshProUGUI Balance_Lobby;
    [SerializeField] TextMeshProUGUI UserID_Disconnect;
    [SerializeField] TextMeshProUGUI UserID_Lobby;

    //API ������ ���� ����Ʈ��
    [SerializeField] GameObject Postman;

    GameObject postman;


    //����ID �г��� ���� 
    Dictionary<string,string> Nick_Session_key =new Dictionary<string,string>();
    string mySessionID;
    string myBetsId;


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
    public void SoloClick()
    {
        PhotonNetwork.LoadLevel("LoadingScene");
        //  PhotonNetwork.LoadLevel("GameScene");
    }
    private void Awake()
    {
    //    DontDestroyOnLoad(this);
        ClearLobby();
        photonView.StartCoroutine(AutoSyncDelay());
        if (FindObjectOfType<TitleToGameScene>() == null)
        {
            postman = Instantiate(Postman);
        }
        else
        {
            postman = FindObjectOfType<TitleToGameScene>().gameObject;
        }
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
        btnConnect.interactable = false; // ��ư �Է� ����
        RobbyPanel.SetActive(false);
        //������ ���� ���� ��û
        PhotonNetwork.ConnectUsingSettings(); //Photon.Pun ���� Ŭ����
    }

    public override void OnConnectedToMaster()
    {
        //API ���� ������ , SessionID ��������
        StartCoroutine(processRequestGetUserInfo());
   
        // �Ҳ���
        ClearLobby();
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

        //���η��������� ������ �켱 �������� 
        PhotonNetwork.JoinRandomRoom();

        //���� Ŀ��Ʈ ��ư ���� 
        DisconnectPanel.SetActive(false);
        //�κ��г� �� 
        RobbyPanel.SetActive(true);
    }
    //������ ���� ������ ���ο� �� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("���� ����");
        //�ƽ� �ο��� �� ���� ǥ�� (�������� �ƴ���)
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4, IsOpen = true });
    }
    //�ڽ��� ���� 
    public override void OnJoinedRoom()
    {
        Debug.Log("���ο� �÷��̾ �����ϼ̽��ϴ�");
        //API �ܰ� ǥ��
        StartCoroutine(processRequestZeraBalance());
        //API ���Ǿ��̵�� �г��� ���� 
        Nick_Session_key.Add(PhotonNetwork.NickName, mySessionID);
        //���� ���ð� ��������
        StartCoroutine(processRequestSettings());

        Player[] nickNameCheck = PhotonNetwork.PlayerList;
        int checkNum = 0;
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
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel("TitleScene");
        }
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

                //�� ���°� ����� ����� -->�׷��� �̰� �������� ǥ�� ����� �ϱ� ������ RPC�Լ� ���
                gameObject.GetPhotonView().RPC("ButtonColor", RpcTarget.All, myReadyState, myButtonNum);
            }

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
                //4�� ���� �Ϸ�� 2���� ���� ���� �ڷ�ƾ 
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
            SortedPlayer();
        }
        else
        {
            myReadyState = ReadyState.Ready;
            SortedPlayer();
        }
    }
    #endregion 
    //���� ���� 2�� ����
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
        else Debug.Log("������ ���� �����");
    }
  
    IEnumerator broken()
    {
        int ran = Random.Range(11, 17);
        yield return new WaitForSeconds(ran);
       
        brokenWindow.gameObject.SetActive(true);
        audioSource.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------

    #region APIȣ�� �Լ�
    [Header("[API ����]")]
   // [SerializeField] TextMeshProUGUI txtInputField;
    [SerializeField] string selectedBettingID;

    [Header("[��ϵ� ������Ʈ���� ȹ�氡���� API Ű]")]
    [SerializeField] string API_KEY = "";

    [Header("[Betting Backend Base URL]")]
    [SerializeField] string FullAppsProductionURL = "https://odin-api.browseosiris.com";
    [SerializeField] string FullAppsStagingURL = "https://odin-api-sat.browseosiris.com";

    string getBaseURL()
    {
        // ���δ��� �ܰ���
        //return FullAppsProductionURL;

        // ������¡ �ܰ�(����)���
        return FullAppsStagingURL;
    }

    Res_UserProfile res_UserProfile = null;
    Res_UserSessionID res_UserSessionID = null;
    Res_BettingSetting res_BettingSetting = null;
    //---------------
    // ���� ����
    public void OnClick_GetUserProfile() //��ư ���� 
    {
        StartCoroutine(processRequestGetUserInfo());
    }
    IEnumerator processRequestGetUserInfo()
    {
        // ���� ����
        yield return requestGetUserInfo((response) =>
        {
            if (response != null)
            {
                Debug.Log("## " + response.ToString());
                res_UserProfile = response;
                Debug.Log(res_UserProfile.userProfile.username);
            }
        });
        btnConnect.interactable = true;
    }
    delegate void resCallback_GetUserInfo(Res_UserProfile response);
    IEnumerator requestGetUserInfo(resCallback_GetUserInfo callback)
    {
        // get user profile
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8546/api/getuserprofile");
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
      //  txtInputField.text = www.downloadHandler.text;
        Res_UserProfile res_getUserProfile = JsonUtility.FromJson<Res_UserProfile>(www.downloadHandler.text);
        UserID_Disconnect.text = "User ID : " + res_getUserProfile.userProfile.username;
        UserID_Lobby.text = "User ID : " + res_getUserProfile.userProfile.username;

        postman.SendMessage("User_ID", res_getUserProfile.userProfile._id, SendMessageOptions.DontRequireReceiver);

        callback(res_getUserProfile);

        //�Ʒ� SessionID���� �ϰ� ó�� 
        StartCoroutine(processRequestGetSessionID());
    }

    //---------------
    // Session ID
    public void OnClick_GetSessionID() //��ư ���� 
    {
        StartCoroutine(processRequestGetSessionID());
    }
    IEnumerator processRequestGetSessionID()
    {
        // ���� ����
        yield return requestGetSessionID((response) =>
        {
            if (response != null)
            {
                Debug.Log("## " + response.ToString());
                res_UserSessionID = response;
            }
        });
    }
    delegate void resCallback_GetSessionID(Res_UserSessionID response);
    IEnumerator requestGetSessionID(resCallback_GetSessionID callback)
    {
        // get session id
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:8546/api/getsessionid");
        yield return www.SendWebRequest();
        Debug.Log("��.." + www.downloadHandler.text);
      //  txtInputField.text = www.downloadHandler.text;
        Res_UserSessionID res_getSessionID = JsonUtility.FromJson<Res_UserSessionID>(www.downloadHandler.text);
        
        mySessionID = res_getSessionID.sessionId;
        postman.SendMessage("Session_ID", mySessionID, SendMessageOptions.DontRequireReceiver);

        callback(res_getSessionID);

        //API �ܰ� ��������
        StartCoroutine(processRequestZeraBalance());
    }

    //---------------
    // ���ð��� ���� ������ ������
    public void OnClick_Settings()//��ư Ŭ����
    {
        StartCoroutine(processRequestSettings());//�� �����
    }
    IEnumerator processRequestSettings()
    {
        yield return requestSettings((response) =>
        {
            if (response != null)
            {
                Debug.Log("## Settings : " + response.ToString());
                res_BettingSetting = response;
            }
        });
    }
    delegate void resCallback_Settings(Res_BettingSetting response);
    IEnumerator requestSettings(resCallback_Settings callback)
    {
        string url = getBaseURL() + "/v1/betting/settings";


        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("api-key", API_KEY);
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
      //  txtInputField.text = www.downloadHandler.text;
        
        Res_BettingSetting res = JsonUtility.FromJson<Res_BettingSetting>(www.downloadHandler.text);
        myBetsId = res.data.bets[0]._id;
        Debug.Log("�� ���� ���̵� : " + myBetsId);


        postman.SendMessage("Bets_ID", myBetsId,SendMessageOptions.DontRequireReceiver);
        
        
        callback(res);
        //UnityWebRequest www = new UnityWebRequest(URL);
    }

    //---------------
    // Zera �ܰ� Ȯ��
    public void OnClick_ZeraBalance() //��ư Ŭ���� ���
    {
        StartCoroutine(processRequestZeraBalance()); //�� ���� ���� �ĸ��� ȣ��
    }
    IEnumerator processRequestZeraBalance()
    {
        yield return requestZeraBalance(res_UserSessionID.sessionId, (response) =>
        {
            if (response != null)
            {
                Debug.Log("## Response Zera Balance : " + response.ToString());
            }
        });
    }
    delegate void resCallback_BalanceInfo(Res_ZeraBalance response);
    IEnumerator requestZeraBalance(string sessionID, resCallback_BalanceInfo callback)
    {
        string url = getBaseURL() + ("/v1/betting/" + "zera" + "/balance/" + sessionID);

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("api-key", API_KEY);
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
        // txtInputField.text = www.downloadHandler.text;
        Balance_Disconnect.text = www.downloadHandler.text;
        Balance_Lobby.text = www.downloadHandler.text;

        Res_ZeraBalance res = JsonUtility.FromJson<Res_ZeraBalance>(www.downloadHandler.text);
        Balance_Disconnect.text = "Balance : " + res.data.balance.ToString();
        Balance_Lobby.text = "Balance : " + res.data.balance.ToString();
        callback(res);
        //UnityWebRequest www = new UnityWebRequest(URL);
    }
    #endregion
}

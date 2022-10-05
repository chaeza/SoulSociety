using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
public class UIMgr : MonoBehaviourPun
{
    GameSceneLogic gameSceneLogic;


    GameObject cooltimeGameobject = null;//스킬쿨타임을 호출한 객체를 저장함
    GameObject itemUI = null;//여러개의 아이템종류를 이 오브젝트에 저장
    GameObject itemUIExplantion = null;//여러개의 아이템 설명을 이 오브젝트에 저장
    GameObject skillUI = null;//여러개의 스킬종류를 이 오브젝트에 저장
    GameObject skillUIExplantion = null;//여러개의 스킬 설명을 이 오브젝트에 저장
    GameObject[] inventory = new GameObject[5];// 인벤토리 UI
    [Header("쿨타임")]
    [SerializeField] TextMeshProUGUI cooltimeText;//쿨타임 텍스트
    [Header("아이템 아이콘")]
    [SerializeField] GameObject[] ItemIcon;//아이템 아이콘
    [Header("아이템 설명")]
    [SerializeField] GameObject[] ItemIconExplantion;//아이템 설명
    [Header("스킬 아이콘")]
    [SerializeField] GameObject[] skill;//스킬 아이콘
    [Header("스킬 설명")]
    [SerializeField] GameObject[] skillExplantion;//스킬 설명
    [Header("대쉬")]
    [SerializeField] GameObject dash;
    [SerializeField] GameObject dashExplantion = null;
    [SerializeField] Image dashImage = null;
    [Header("게임")]
    [SerializeField] GameObject win = null;
    [SerializeField] GameObject lose = null;
    [SerializeField] GameObject tab = null;
    [SerializeField] GameObject esc = null;
    [SerializeField] GameObject[] redSoul = null;
    [SerializeField] GameObject[] blueSoul = null;
    [SerializeField] GameObject winEff = null;
    [Header("플레이어")]
    [SerializeField] Text[] playerNick = null;
    [SerializeField] Image myHP = null;
    bool[] redSetBool = new bool[15];
    bool[] blueSetBool = new bool[25];
    string[] sortedPlayer = new string[4];
    bool nickSave;

    bool isWinner = false;
    int myIDNum;

    private void Start()
    {
        gameSceneLogic = FindObjectOfType<GameSceneLogic>();

    }

    public void MyPlayerViewID(int ID)
    {
        myIDNum = ID;
    }

    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Tab)
        {
            tab.SetActive(true);
            TabSoulRender(1);
        }
        else
        {
            tab.SetActive(false);
            TabSoulRender(0);
        }
        if (GameMgr.Instance.playerInput.Esc == KeyCode.Escape) esc.SetActive(true);
        else esc.SetActive(false);
    }
    public void SetHP(float curHP, float maxHP)
    {
        myHP.fillAmount = curHP / maxHP;
    }
    public void TabNickName(int Num, state myState)
    {
        if (nickSave == false)
        {
            nickSave = true;
            Player[] sortedPlayers = PhotonNetwork.PlayerList;
            for (int i = 0; i < sortedPlayers.Length; i++)
            {
                sortedPlayer[i] = sortedPlayers[i].NickName;
            }

        }
        playerNick[Num].text = sortedPlayer[Num];
        if (myState == state.Die) playerNick[Num].color = Color.red;
    }
    public void RedTabSoul(int Num, int Rnum)
    {
        if (Rnum >= 1)
        {
            redSetBool[3 * (Num + 1)] = true;
            if (Rnum >= 2) redSetBool[1 + 3 * (Num + 1)] = true;
            if (Rnum == 3) redSetBool[2 + 3 * (Num + 1)] = true;
        }
        else if (Rnum == 0)
        {
            redSetBool[3 * (Num + 1)] = false;
            redSetBool[1 + 3 * (Num + 1)] = false;
            redSetBool[2 + 3 * (Num + 1)] = false;
        }
    }
    public void BlueTabSoul(int Num, int Bnum)
    {
        if (Bnum >= 1)
        {
            blueSetBool[5 * (Num + 1)] = true;
            if (Bnum >= 2) blueSetBool[1 + 5 * (Num + 1)] = true;
            if (Bnum >= 3) blueSetBool[2 + 5 * (Num + 1)] = true;
            if (Bnum >= 4) blueSetBool[3 + 5 * (Num + 1)] = true;
            if (Bnum == 5) blueSetBool[4 + 5 * (Num + 1)] = true;
        }
        else if (Bnum == 0)
        {
            blueSetBool[5 * (Num + 1)] = false;
            blueSetBool[1 + 5 * (Num + 1)] = false;
            blueSetBool[2 + 5 * (Num + 1)] = false;
            blueSetBool[3 + 5 * (Num + 1)] = false;
            blueSetBool[4 + 5 * (Num + 1)] = false;
        }
    }
    public void TabSoulRender(int a)
    {
        if (a == 1)
        {
            for (int i = 0; i < redSetBool.Length; i++)
            {
                if (redSetBool[i] == true) redSoul[i].SetActive(true);
            }
            for (int i = 0; i < blueSetBool.Length; i++)
            {
                if (blueSetBool[i] == true) blueSoul[i].SetActive(true);
            }
        }
        else if (a == 0)
        {
            for (int i = 3; i < redSetBool.Length; i++)
            {
                redSoul[i].SetActive(false);
            }
            for (int i = 5; i < blueSetBool.Length; i++)
            {
                blueSoul[i].SetActive(false);
            }
        }
    }

    bool setItem;
    bool setSkill;
    bool setDash;
    public void SkillUI(int Num)//스킬 아이콘 표시
    {
        skillUI = skill[Num];//가진 스킬을 스킬UI에 저장해서 사용
        skillUI.SetActive(true);
    }
    public void ItemUI(int Num1, int Num2)//Num1은 인벤토리 위치 Num2는 해당 아이템 번호
    {
        //해당 아이템 번호를 아이템UI 오브젝트에 넣어서 편하게 사용
        itemUI = ItemIcon[Num2];

        itemUI.SetActive(true);//받은 아이템 UI활성화

        //해당아이템 인벤토리 위치를 비교하고 해당 인벤토리 칸에 위치변경및 UI 할당하여 비활성화 할 수 있게함
        if (Num1 == 1) inventory[1] = itemUI;
        else if (Num1 == 2)
        {
            inventory[2] = itemUI;
            itemUI.transform.Translate(160, 0, 0);
        }
        else if (Num1 == 3)
        {
            inventory[3] = itemUI;
            itemUI.transform.Translate(320, 0, 0);
        }
        else if (Num1 == 4)
        {
            inventory[4] = itemUI;
            itemUI.transform.Translate(480, 0, 0);
        }
    }
    public void UseItem(int Num)//사용한 해당 아이템 아이콘을 없앰
    {
        inventory[Num].SetActive(false);
        inventory[Num].transform.position = new Vector3(725f, 60f, 0);
        inventory[Num] = null;
    }
    public void OnExplantionItem(int Num1, int Num2)//Num1은 인벤토리 위치 Num2는 해당 아이템 번호
    {

        if (setItem == false)//bool 값을 사용하여 스킬 설명은 한번만 띄움
        {
            itemUIExplantion = ItemIconExplantion[Num2];//해당스킬 설명을 설명UI저장

            if (itemUIExplantion != null)//저장해둔 설명UI가 있을시
            {
                itemUIExplantion.SetActive(true);//저장해둔 설명UI 활성화
                setItem = true;
                if (Num1 == 2)
                {
                    itemUIExplantion.transform.Translate(160, 0, 0);//해당 인벤토리 위치로 이동시킴
                }
                else if (Num1 == 3)
                {
                    itemUIExplantion.transform.Translate(320, 0, 0);
                }
                else if (Num1 == 4)
                {
                    itemUIExplantion.transform.Translate(480, 0, 0);
                }
            }
        }
        if (itemUIExplantion != null && Num1 == 5)//좌표가 같지 않을 때 설명창을 비활성화 시킵니다.
        {
            setItem = false;//bool 값 초기화
            itemUIExplantion.transform.position = new Vector3(725f, 260f, 0);//위치 초기화
            itemUIExplantion.SetActive(false);
        }
    }
    public void OnExplantionSkill(bool On)
    {
        if (setSkill == false)//bool 값으로 한번만 띄움
        {
            if (On == true)
            {
                setSkill = true;
                skillUIExplantion = skillExplantion[GameMgr.Instance.randomSkill.skillRan];//해당 스킬 설명을 설명UI에 넣음

                skillUIExplantion.SetActive(true);//설명UI 활성화
            }
        }

        if (skillUIExplantion != null && setSkill == true && On == false)
        {
            skillUIExplantion.SetActive(false);//설명UI 비활성화
            setSkill = false;//bool값 초기화
        }
    }
    public void OnExplantionDash(bool On)
    {
        if (setDash == false)//bool 값으로 한번만 띄움
        {
            if (On == true)
            {
                setDash = true;
                skillUIExplantion = dashExplantion;

                skillUIExplantion.SetActive(true);//설명UI 활성화
            }
        }

        if (skillUIExplantion != null && setDash == true && On == false)
        {
            skillUIExplantion.SetActive(false);//설명UI 비활성화
            setDash = false;//bool값 초기화
        }
    }
    public void SkillCooltime(GameObject my, int Cool)//UI매니저에 스킬 쿨타임을 호출시킨 객체, 쿨타임 시간
    {
        cooltimeGameobject = my;//호출시킨 오브젝트 객체를 저장해둡니다.
        skillUI.GetComponent<Image>().color = new Color(160 / 255f, 160 / 255f, 160 / 255f);//아이콘의 색상을 흐리게 바꿔서 비활성화 느낌을 줍니다.
        cooltimeText.text = Cool.ToString();//쿨타임 Text를 해당 쿨타임의 Max값으로 바꿉니다.
        StartCoroutine(Cooltime(Cool));//쿨타임 코루틴을 실행시켜 쿨타임 시간을 기다립니다.

    }
    public void DashCooltime(GameObject my, int Cool)//UI매니저에 스킬 쿨타임을 호출시킨 객체, 쿨타임 시간
    {
        cooltimeGameobject = my;//호출시킨 오브젝트 객체를 저장해둡니다.
        StartCoroutine(DashCooltime(Cool));//쿨타임 코루틴을 실행시켜 쿨타임 시간을 기다립니다.

    }
    IEnumerator Cooltime(int Cool)
    {
        for (int i = Cool - 1; i >= 0; --i)//받은 쿨타임 시간을 i에 저장해서
        {
            yield return new WaitForSeconds(1f);//1초씩 기다리고
            cooltimeText.text = i.ToString();//쿨타임 텍스트를 -1시킴
            yield return null;
        }
        skillUI.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);//아이콘 색상 원위치
        cooltimeGameobject.SendMessage("ResetCooltime", SendMessageOptions.DontRequireReceiver);//저장해놨던 UI매니저를 호출시킨 객체한테 ResetCooltime을 호출시켜 스킬을 다시 사용하게함
        cooltimeText.text = " ";//텍스트 비활성화대신 그냥 아무것도 출력안함
        yield break;
    }
    IEnumerator DashCooltime(int Cool)
    {
        dashImage.fillAmount = 0f;
        for (int i = 0; i < Cool*4; i++)//받은 쿨타임 시간을 i에 저장해서
        {
            yield return new WaitForSeconds(0.24f);//1초씩 기다리고
            dashImage.fillAmount += 1 / 20f;
        }
        cooltimeGameobject.SendMessage("ResetCooltime2", SendMessageOptions.DontRequireReceiver);//저장해놨던 UI매니저를 호출시킨 객체한테 ResetCooltime을 호출시켜 스킬을 다시 사용하게함
        yield break;
    }
    public void MyRedSoul(int redsoul)
    {
        MyBlueSoul(0);
        if (redsoul >= 1)
        {
            redSoul[0].SetActive(true);
            if (redsoul >= 2) redSoul[1].SetActive(true);
            if (redsoul >= 3) redSoul[2].SetActive(true);
        }
        else if (redsoul == 0)
        {
            redSoul[0].SetActive(false);
            redSoul[1].SetActive(false);
            redSoul[2].SetActive(false);
        }

    }
    public void MyBlueSoul(int bluesoul)
    {
        if (bluesoul >= 1)
        {
            blueSoul[0].SetActive(true);
            if (bluesoul >= 2) blueSoul[1].SetActive(true);
            if (bluesoul >= 3) blueSoul[2].SetActive(true);
            if (bluesoul >= 4) blueSoul[3].SetActive(true);
            if (bluesoul == 5) blueSoul[4].SetActive(true);
        }
        else if (bluesoul == 0)
        {
            blueSoul[0].SetActive(false);
            blueSoul[1].SetActive(false);
            blueSoul[2].SetActive(false);
            blueSoul[3].SetActive(false);
            blueSoul[4].SetActive(false);
        }
    }
    [PunRPC]
    public void EndGame(int Num, int dieC)
    {
        if (isWinner) return;
        if (Num == 1)
        {
            if (myIDNum == dieC && isWinner == false)
            {
                photonView.RPC("WinnerFixed", RpcTarget.All);
                win.SetActive(true);
                winEff.SetActive(true);
                gameSceneLogic.WinnerEndGame();
                return;
            }
            else
                lose.SetActive(true);
        }
        else if (Num == 2)
        {
            if (GameMgr.Instance.blueCount == dieC && isWinner == false)
            {
                photonView.RPC("WinnerFixed", RpcTarget.All);
                win.SetActive(true);
                winEff.SetActive(true);
                gameSceneLogic.WinnerEndGame();
                return;
            }
            else
                lose.SetActive(true);
        }
        gameSceneLogic.EndGame();
    }
    [PunRPC]
    public void WinnerFixed()
    {
        isWinner = true;
    }
}

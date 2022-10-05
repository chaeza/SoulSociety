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


    GameObject cooltimeGameobject = null;//��ų��Ÿ���� ȣ���� ��ü�� ������
    GameObject itemUI = null;//�������� ������������ �� ������Ʈ�� ����
    GameObject itemUIExplantion = null;//�������� ������ ������ �� ������Ʈ�� ����
    GameObject skillUI = null;//�������� ��ų������ �� ������Ʈ�� ����
    GameObject skillUIExplantion = null;//�������� ��ų ������ �� ������Ʈ�� ����
    GameObject[] inventory = new GameObject[5];// �κ��丮 UI
    [Header("��Ÿ��")]
    [SerializeField] TextMeshProUGUI cooltimeText;//��Ÿ�� �ؽ�Ʈ
    [Header("������ ������")]
    [SerializeField] GameObject[] ItemIcon;//������ ������
    [Header("������ ����")]
    [SerializeField] GameObject[] ItemIconExplantion;//������ ����
    [Header("��ų ������")]
    [SerializeField] GameObject[] skill;//��ų ������
    [Header("��ų ����")]
    [SerializeField] GameObject[] skillExplantion;//��ų ����
    [Header("�뽬")]
    [SerializeField] GameObject dash;
    [SerializeField] GameObject dashExplantion = null;
    [SerializeField] Image dashImage = null;
    [Header("����")]
    [SerializeField] GameObject win = null;
    [SerializeField] GameObject lose = null;
    [SerializeField] GameObject tab = null;
    [SerializeField] GameObject esc = null;
    [SerializeField] GameObject[] redSoul = null;
    [SerializeField] GameObject[] blueSoul = null;
    [SerializeField] GameObject winEff = null;
    [Header("�÷��̾�")]
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
    public void SkillUI(int Num)//��ų ������ ǥ��
    {
        skillUI = skill[Num];//���� ��ų�� ��ųUI�� �����ؼ� ���
        skillUI.SetActive(true);
    }
    public void ItemUI(int Num1, int Num2)//Num1�� �κ��丮 ��ġ Num2�� �ش� ������ ��ȣ
    {
        //�ش� ������ ��ȣ�� ������UI ������Ʈ�� �־ ���ϰ� ���
        itemUI = ItemIcon[Num2];

        itemUI.SetActive(true);//���� ������ UIȰ��ȭ

        //�ش������ �κ��丮 ��ġ�� ���ϰ� �ش� �κ��丮 ĭ�� ��ġ����� UI �Ҵ��Ͽ� ��Ȱ��ȭ �� �� �ְ���
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
    public void UseItem(int Num)//����� �ش� ������ �������� ����
    {
        inventory[Num].SetActive(false);
        inventory[Num].transform.position = new Vector3(725f, 60f, 0);
        inventory[Num] = null;
    }
    public void OnExplantionItem(int Num1, int Num2)//Num1�� �κ��丮 ��ġ Num2�� �ش� ������ ��ȣ
    {

        if (setItem == false)//bool ���� ����Ͽ� ��ų ������ �ѹ��� ���
        {
            itemUIExplantion = ItemIconExplantion[Num2];//�ش罺ų ������ ����UI����

            if (itemUIExplantion != null)//�����ص� ����UI�� ������
            {
                itemUIExplantion.SetActive(true);//�����ص� ����UI Ȱ��ȭ
                setItem = true;
                if (Num1 == 2)
                {
                    itemUIExplantion.transform.Translate(160, 0, 0);//�ش� �κ��丮 ��ġ�� �̵���Ŵ
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
        if (itemUIExplantion != null && Num1 == 5)//��ǥ�� ���� ���� �� ����â�� ��Ȱ��ȭ ��ŵ�ϴ�.
        {
            setItem = false;//bool �� �ʱ�ȭ
            itemUIExplantion.transform.position = new Vector3(725f, 260f, 0);//��ġ �ʱ�ȭ
            itemUIExplantion.SetActive(false);
        }
    }
    public void OnExplantionSkill(bool On)
    {
        if (setSkill == false)//bool ������ �ѹ��� ���
        {
            if (On == true)
            {
                setSkill = true;
                skillUIExplantion = skillExplantion[GameMgr.Instance.randomSkill.skillRan];//�ش� ��ų ������ ����UI�� ����

                skillUIExplantion.SetActive(true);//����UI Ȱ��ȭ
            }
        }

        if (skillUIExplantion != null && setSkill == true && On == false)
        {
            skillUIExplantion.SetActive(false);//����UI ��Ȱ��ȭ
            setSkill = false;//bool�� �ʱ�ȭ
        }
    }
    public void OnExplantionDash(bool On)
    {
        if (setDash == false)//bool ������ �ѹ��� ���
        {
            if (On == true)
            {
                setDash = true;
                skillUIExplantion = dashExplantion;

                skillUIExplantion.SetActive(true);//����UI Ȱ��ȭ
            }
        }

        if (skillUIExplantion != null && setDash == true && On == false)
        {
            skillUIExplantion.SetActive(false);//����UI ��Ȱ��ȭ
            setDash = false;//bool�� �ʱ�ȭ
        }
    }
    public void SkillCooltime(GameObject my, int Cool)//UI�Ŵ����� ��ų ��Ÿ���� ȣ���Ų ��ü, ��Ÿ�� �ð�
    {
        cooltimeGameobject = my;//ȣ���Ų ������Ʈ ��ü�� �����صӴϴ�.
        skillUI.GetComponent<Image>().color = new Color(160 / 255f, 160 / 255f, 160 / 255f);//�������� ������ �帮�� �ٲ㼭 ��Ȱ��ȭ ������ �ݴϴ�.
        cooltimeText.text = Cool.ToString();//��Ÿ�� Text�� �ش� ��Ÿ���� Max������ �ٲߴϴ�.
        StartCoroutine(Cooltime(Cool));//��Ÿ�� �ڷ�ƾ�� ������� ��Ÿ�� �ð��� ��ٸ��ϴ�.

    }
    public void DashCooltime(GameObject my, int Cool)//UI�Ŵ����� ��ų ��Ÿ���� ȣ���Ų ��ü, ��Ÿ�� �ð�
    {
        cooltimeGameobject = my;//ȣ���Ų ������Ʈ ��ü�� �����صӴϴ�.
        StartCoroutine(DashCooltime(Cool));//��Ÿ�� �ڷ�ƾ�� ������� ��Ÿ�� �ð��� ��ٸ��ϴ�.

    }
    IEnumerator Cooltime(int Cool)
    {
        for (int i = Cool - 1; i >= 0; --i)//���� ��Ÿ�� �ð��� i�� �����ؼ�
        {
            yield return new WaitForSeconds(1f);//1�ʾ� ��ٸ���
            cooltimeText.text = i.ToString();//��Ÿ�� �ؽ�Ʈ�� -1��Ŵ
            yield return null;
        }
        skillUI.GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f);//������ ���� ����ġ
        cooltimeGameobject.SendMessage("ResetCooltime", SendMessageOptions.DontRequireReceiver);//�����س��� UI�Ŵ����� ȣ���Ų ��ü���� ResetCooltime�� ȣ����� ��ų�� �ٽ� ����ϰ���
        cooltimeText.text = " ";//�ؽ�Ʈ ��Ȱ��ȭ��� �׳� �ƹ��͵� ��¾���
        yield break;
    }
    IEnumerator DashCooltime(int Cool)
    {
        dashImage.fillAmount = 0f;
        for (int i = 0; i < Cool*4; i++)//���� ��Ÿ�� �ð��� i�� �����ؼ�
        {
            yield return new WaitForSeconds(0.24f);//1�ʾ� ��ٸ���
            dashImage.fillAmount += 1 / 20f;
        }
        cooltimeGameobject.SendMessage("ResetCooltime2", SendMessageOptions.DontRequireReceiver);//�����س��� UI�Ŵ����� ȣ���Ų ��ü���� ResetCooltime�� ȣ����� ��ų�� �ٽ� ����ϰ���
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

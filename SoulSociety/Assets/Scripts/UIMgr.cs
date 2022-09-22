using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class UIMgr : MonoBehaviourPun
{

    GameObject itemUI = null;
    GameObject itemUIExplantion = null;
    GameObject skillUI = null;
    GameObject skillUIExplantion = null;
    GameObject[] inventory = new GameObject[5];
    [Header("������ ������")]
    [SerializeField] GameObject ItemIcon1;
    [SerializeField] GameObject ItemIcon2;
    [SerializeField] GameObject ItemIcon3;
    [SerializeField] GameObject ItemIcon4;
    [Header("������ ����")]
    [SerializeField] GameObject ItemIcon1Explantion;
    [SerializeField] GameObject ItemIcon2Explantion;
    [SerializeField] GameObject ItemIcon3Explantion;
    [SerializeField] GameObject ItemIcon4Explantion;
    [Header("��ų ������")]
    [SerializeField] GameObject skill1;
    [SerializeField] GameObject skill2;
    [Header("��ų ����")]
    [SerializeField] GameObject skill1Explantion;
    [SerializeField] GameObject skill2Explantion;
    bool setItem;
    bool setSkill;
    [Header("Tap ����")]
    [SerializeField] TextMeshProUGUI[] tapNicknames;
    public GameObject Tap = null;


    private void Start()
    {
        Tap.SetActive(false);
    }

    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Tab)
        {
            TapNickname();

        }
        else
        {
            Tap.SetActive(false);
        }

    }

    public void TapNickname()
    {
        Tap.SetActive(true);
        
        for (int i = 0; i < PhotonNetwork.CountOfPlayers; ++i)
        {
            tapNicknames[i].text = PhotonNetwork.PlayerList[i].NickName;
        }

    }


    public void SkillUI(int Num)
    {
        if (Num == 1) skillUI = skill1;
        else if (Num == 2) skillUI = skill2;
        skillUI.SetActive(true);
    }
    public void ItemUI(int Num1,int Num2)//Num1�� �κ��丮 ��ġ Num2�� �ش� ������ ��ȣ
    {
        //�ش� ������ ��ȣ�� ������UI ������Ʈ�� �־ ���ϰ� ���
        if (Num2 == 1) itemUI = ItemIcon1;
        else if (Num2 == 2) itemUI = ItemIcon2;
        else if (Num2 == 3) itemUI = ItemIcon3;
        else if (Num2 == 4) itemUI = ItemIcon4;
       
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
    public void UseItem(int Num)
    {
        inventory[Num].SetActive(false);
        inventory[Num].transform.position=new Vector3(725f, 60f,0);
        inventory[Num] = null;
    }
    public void OnExplantionItem(int Num1, int Num2)//Num1�� �κ��丮 ��ġ Num2�� �ش� ������ ��ȣ
    {

        if (setItem == false)
        {
            if (Num2 == 1) itemUIExplantion = ItemIcon1Explantion;
            else if (Num2 == 2) itemUIExplantion = ItemIcon2Explantion;
            else if (Num2 == 3) itemUIExplantion = ItemIcon3Explantion;
            else if (Num2 == 4) itemUIExplantion = ItemIcon4Explantion;
            if (itemUIExplantion != null)
            {
                itemUIExplantion.SetActive(true);
                setItem = true;
                if (Num1 == 2)
                {
                    itemUIExplantion.transform.Translate(160, 0, 0);
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
        if (itemUIExplantion!=null&&Num1 == 5)
        {
            setItem = false;
            itemUIExplantion.transform.position = new Vector3(725f, 260f, 0);
            itemUIExplantion.SetActive(false);
        }
    }
    public void OnExplantionSkill(bool On)
    {
        if (setSkill == false)
        {
            if (On == true)
            {
                setSkill = true;
                if (GameMgr.Instance.randomSkill.skillRan == 1) skillUIExplantion = skill1Explantion;
                else if (GameMgr.Instance.randomSkill.skillRan == 2) skillUIExplantion = skill2Explantion;
                skillUIExplantion.SetActive(true);
            }
        }
        
        if(skillUIExplantion != null&&setSkill == true&& On==false)
        {
            skillUIExplantion.SetActive(false);
            setSkill = false;
        }
    }

    





}

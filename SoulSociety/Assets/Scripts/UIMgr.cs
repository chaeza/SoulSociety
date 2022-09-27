using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
public class UIMgr : MonoBehaviourPun
{

    GameObject cooltimeGameobject = null;//��ų��Ÿ���� ȣ���� ��ü�� ������
    GameObject itemUI = null;//�������� ������������ �� ������Ʈ�� ����
    GameObject itemUIExplantion = null;//�������� ������ ������ �� ������Ʈ�� ����
    GameObject skillUI = null;//�������� ��ų������ �� ������Ʈ�� ����
    GameObject skillUIExplantion = null;//�������� ��ų ������ �� ������Ʈ�� ����
    GameObject[] inventory = new GameObject[5];// �κ��丮 UI
    [Header("��Ÿ��")]
    [SerializeField] TextMeshProUGUI cooltimeText;//��Ÿ�� �ؽ�Ʈ
    [Header("������ ������")]
    [SerializeField] GameObject ItemIcon1;//������ ������
    [SerializeField] GameObject ItemIcon2;
    [SerializeField] GameObject ItemIcon3;
    [SerializeField] GameObject ItemIcon4;
    [Header("������ ����")]
    [SerializeField] GameObject ItemIcon1Explantion;//������ ����
    [SerializeField] GameObject ItemIcon2Explantion;
    [SerializeField] GameObject ItemIcon3Explantion;
    [SerializeField] GameObject ItemIcon4Explantion;
    [Header("��ų ������")]
    [SerializeField] GameObject skill1;//��ų ������
    [SerializeField] GameObject skill2;
    [Header("��ų ����")]
    [SerializeField] GameObject skill1Explantion;//��ų ����
    [SerializeField] GameObject skill2Explantion;
    [Header("����")]
    [SerializeField] GameObject win = null;
    [SerializeField] GameObject lose = null;
    [SerializeField] GameObject tab = null;
    [SerializeField] GameObject esc = null;
    [SerializeField] GameObject[] redSoul = null;
    [SerializeField] GameObject[] blueSoul = null;

    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Tab) tab.SetActive(true);
        else tab.SetActive(false); 
        if (GameMgr.Instance.playerInput.Esc == KeyCode.Escape) esc.SetActive(true);
        else esc.SetActive(false);
    }


    bool setItem;
    bool setSkill;
    public void SkillUI(int Num)//��ų ������ ǥ��
    {
        if (Num == 1) skillUI = skill1;//���� ��ų�� ��ųUI�� �����ؼ� ���
        else if (Num == 2) skillUI = skill2;
        skillUI.SetActive(true);
    }
    public void ItemUI(int Num1, int Num2)//Num1�� �κ��丮 ��ġ Num2�� �ش� ������ ��ȣ
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
            if (Num2 == 1) itemUIExplantion = ItemIcon1Explantion;//�ش罺ų ������ ����UI����
            else if (Num2 == 2) itemUIExplantion = ItemIcon2Explantion;
            else if (Num2 == 3) itemUIExplantion = ItemIcon3Explantion;
            else if (Num2 == 4) itemUIExplantion = ItemIcon4Explantion;
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
                if (GameMgr.Instance.randomSkill.skillRan == 1) skillUIExplantion = skill1Explantion;//�ش� ��ų ������ ����UI�� ����
                else if (GameMgr.Instance.randomSkill.skillRan == 2) skillUIExplantion = skill2Explantion;
                skillUIExplantion.SetActive(true);//����UI Ȱ��ȭ
            }
        }

        if (skillUIExplantion != null && setSkill == true && On == false)
        {
            skillUIExplantion.SetActive(false);//����UI ��Ȱ��ȭ
            setSkill = false;//bool�� �ʱ�ȭ
        }
    }
    public void SkillCooltime(GameObject my, int Cool)//UI�Ŵ����� ��ų ��Ÿ���� ȣ���Ų ��ü, ��Ÿ�� �ð�
    {
        cooltimeGameobject = my;//ȣ���Ų ������Ʈ ��ü�� �����صӴϴ�.
        skillUI.GetComponent<Image>().color = new Color(160 / 255f, 160 / 255f, 160 / 255f);//�������� ������ �帮�� �ٲ㼭 ��Ȱ��ȭ ������ �ݴϴ�.
        cooltimeText.text = Cool.ToString();//��Ÿ�� Text�� �ش� ��Ÿ���� Max������ �ٲߴϴ�.
        StartCoroutine(Cooltime(Cool));//��Ÿ�� �ڷ�ƾ�� ������� ��Ÿ�� �ð��� ��ٸ��ϴ�.

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
    public void MyRedSoul(int redsoul)
    {
        MyBlueSoul(0);
        if (redsoul == 1) redSoul[0].SetActive(true);
        else if (redsoul == 2)
        {
            redSoul[0].SetActive(true);
            redSoul[1].SetActive(true);
        }
        else if (redsoul == 3)
        {
            redSoul[0].SetActive(true);
            redSoul[1].SetActive(true);
            redSoul[2].SetActive(true);
        }
        else if(redsoul==0)
        {
            redSoul[0].SetActive(false);
            redSoul[1].SetActive(false);
            redSoul[2].SetActive(false);
        }

    }
    public void MyBlueSoul(int bluesoul)
    {
        if (bluesoul == 1) blueSoul[0].SetActive(true);
        else if (bluesoul == 2)
        {
            blueSoul[0].SetActive(true);
            blueSoul[1].SetActive(true);
        }
        else if (bluesoul == 3)
        {
            blueSoul[0].SetActive(true);
            blueSoul[1].SetActive(true);
            blueSoul[2].SetActive(true);
        }
        else if (bluesoul == 4)
        {
            blueSoul[0].SetActive(true);
            blueSoul[1].SetActive(true);
            blueSoul[2].SetActive(true);
            blueSoul[3].SetActive(true);
        }
        else if (bluesoul == 5)
        {
            blueSoul[0].SetActive(true);
            blueSoul[1].SetActive(true);
            blueSoul[2].SetActive(true);
            blueSoul[3].SetActive(true);
            blueSoul[4].SetActive(true);
        }
        else if(bluesoul == 0)
        {
            blueSoul[0].SetActive(false);
            blueSoul[1].SetActive(false);
            blueSoul[2].SetActive(false);
            blueSoul[3].SetActive(false);
            blueSoul[4].SetActive(false);
        }
    }
    [PunRPC]
    public void EndGame(int Num,int dieC)
    {
        if (Num == 1)
        {
            if (GameMgr.Instance.dieCount == dieC)
                win.SetActive(true);

            else
                lose.SetActive(true);
        }
        else if(Num == 2)
        {
            if (GameMgr.Instance.blueCount == dieC)
                win.SetActive(true);

            else
                lose.SetActive(true);
        }
        GameMgr.Instance.endGame = true;
        new WaitForSeconds(1f);
        //PhotonNetwork.LoadLevel("TitleScene");
    }

}

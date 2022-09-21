using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIMgr : MonoBehaviour
{

    GameObject itemUI = null;
    GameObject[] inventory = new GameObject[5];
    [SerializeField] GameObject ItemIcon1;
    [SerializeField] GameObject ItemIcon1Explantion;
    [SerializeField] GameObject ItemIcon2;
    [SerializeField] GameObject ItemIcon3;
    [SerializeField] GameObject ItemIcon4;

    public void SkillUI(int Num)
    {

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
    public void OnExplantionItem(int Num)
    {
        
    }






}

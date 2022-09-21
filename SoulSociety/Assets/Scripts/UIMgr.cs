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
    public void ItemUI(int Num1,int Num2)//Num1은 인벤토리 위치 Num2는 해당 아이템 번호
    {
        //해당 아이템 번호를 아이템UI 오브젝트에 넣어서 편하게 사용
        if (Num2 == 1) itemUI = ItemIcon1;
        else if (Num2 == 2) itemUI = ItemIcon2;
        else if (Num2 == 3) itemUI = ItemIcon3;
        else if (Num2 == 4) itemUI = ItemIcon4;
       
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item4 : MonoBehaviour
{
    [SerializeField]
    int itemNum = 0;
    public void GetItem(int itemnum)
    {
        if (itemNum == 0)
            itemNum = itemnum;
    }

    public void ItemFire()
    {
        if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) ItemSkill();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) ItemSkill();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) ItemSkill();
        else if (itemNum == 4 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) ItemSkill();
    }
    public void ItemSkill()
    {
        GameMgr.Instance.inventory.RemoveInventory(itemNum);
        Destroy(GetComponent<Item4>());
    }
}

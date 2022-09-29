using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BasicAttackDamageUP : MonoBehaviourPun, ItemMethod
{
    private PlayerInfo playerInfo;
   

    [SerializeField]
    int itemNum = 0;
    public void GetItem(int itemnum)
    {
        if (itemNum == 0)
            itemNum = itemnum;
        playerInfo = GetComponent<PlayerInfo>();
    }

    public void ItemFire()
    {
        if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) ItemSkill();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) ItemSkill();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) ItemSkill();
        else if (itemNum == 4 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) ItemSkill();
    }

    // GameObject a = PhotonNetwork.Instantiate("DeathVortex", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.

    public void ItemSkill()
    {
        GameObject a = PhotonNetwork.Instantiate("BasicAttackDamageUP", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
        playerInfo.basicAttackDamage += 10f;


        GameMgr.Instance.uIMgr.UseItem(itemNum);
        GameMgr.Instance.inventory.RemoveInventory(itemNum);
        Destroy(GetComponent<BasicAttackDamageUP>());
    }
}

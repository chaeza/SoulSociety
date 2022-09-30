using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Unbeatable : MonoBehaviourPun, ItemMethod//아이템 인터페이스 상속
{
    [SerializeField]
    int itemNum = 0;

    public void GetItem(int itemnum)//해당 아이템이 어느 인벤토리에 있는지 순서 책정
    {
        if (itemNum == 0)
            itemNum = itemnum;
    }

    public void ItemFire()//플레이어 Attack에서 키입력시 해당 아이템의 인벤토리 위치를 비교하여 아이템 스킬 사용 
    {
        if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) ItemSkill();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) ItemSkill();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) ItemSkill();
        else if (itemNum == 4 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) ItemSkill();
    }
    public void ItemSkill()//이 아이템이 가지고 있는 아이템 스킬 사용
    {
        // 스킬 구현

        GameObject a = PhotonNetwork.Instantiate("Recovery", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
        gameObject.GetPhotonView().RPC("ChageHP", RpcTarget.All, 30f);
        GameMgr.Instance.DestroyTarget(a, 2f);
        //
        GameMgr.Instance.uIMgr.UseItem(itemNum);
        GameMgr.Instance.inventory.RemoveInventory(itemNum);//인벤토리에서 이 스킬을 소유한 것을 초기화함
        Destroy(GetComponent<Recovery>());//해당 아이템을 사용후 컴포넌트 삭제
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Trap : MonoBehaviourPun, ItemMethod
{
    int itemNum = 0;
    GameObject TrapE;
    int MakerViewID;
    private void Start()
    {
        MakerViewID = gameObject.GetComponent<PhotonView>().ViewID;
    }

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

        GameObject a = PhotonNetwork.Instantiate("TrapTrigger", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
        a.AddComponent<TrapCh>();
        a.GetPhotonView().RPC("GetMasterName", RpcTarget.All, MakerViewID);
        TrapE =  Instantiate(GameMgr.Instance.resourceData.myTrapEff , transform.position, Quaternion.identity);
        a.GetComponent<TrapCh>().TrapEffInfo(TrapE);
        GameMgr.Instance.uIMgr.UseItem(itemNum);
        GameMgr.Instance.inventory.RemoveInventory(itemNum);
        Destroy(GetComponent<Trap>());
    }
}

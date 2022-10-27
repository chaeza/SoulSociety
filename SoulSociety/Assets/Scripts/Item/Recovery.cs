using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Item interface inheritance
public class Recovery : MonoBehaviourPun, ItemMethod
{
    [SerializeField]
    private int itemNum = 0;
    private AudioSource sound;

    //Order which inventory the item is in
    public void GetItem(int itemnum)
    {
        if (itemNum == 0)
            itemNum = itemnum;
    }

    //Use item skill by comparing the inventory location of the item when keying in player attack
    public void ItemFire()
    {
        if (itemNum == 1 && GameMgr.Instance.playerInput.inputKey == KeyCode.Q) ItemSkill();
        else if (itemNum == 2 && GameMgr.Instance.playerInput.inputKey == KeyCode.W) ItemSkill();
        else if (itemNum == 3 && GameMgr.Instance.playerInput.inputKey == KeyCode.E) ItemSkill();
        else if (itemNum == 4 && GameMgr.Instance.playerInput.inputKey == KeyCode.R) ItemSkill();
    }

    //Use the item skill an item has
    public void ItemSkill()
    {
        //Skill Implementation
        GameObject recovery = PhotonNetwork.Instantiate("Recovery", transform.position, Quaternion.identity);
        recovery.AddComponent<MyPosition>();
        recovery.SendMessage("MyPos", gameObject.transform, SendMessageOptions.DontRequireReceiver);
        recovery.SendMessage("YPos", 2f, SendMessageOptions.DontRequireReceiver);

        //Attach audio source to recovery item
        sound = recovery.GetComponent<AudioSource>();
        sound.Play();

        gameObject.GetPhotonView().RPC("ChageHP", RpcTarget.All, 30f);
        GameMgr.Instance.DestroyTarget(recovery, 2f);

        GameMgr.Instance.uIMgr.UseItem(itemNum);
        //Remove possession of this skill in inventory
        GameMgr.Instance.inventory.RemoveInventory(itemNum);
        //Delete the component after using the item
        Destroy(GetComponent<Recovery>());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BasicAttackDamageUP : MonoBehaviourPun, ItemMethod
{
    [SerializeField]
    int itemNum = 0;
    AudioSource sound;

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

    // GameObject a = PhotonNetwork.Instantiate("DeathVortex", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.

    public void ItemSkill()
    {
        GameObject a = PhotonNetwork.Instantiate("BasicAttackDamageUP", transform.position, Quaternion.identity);//이펙트를 포톤 인스턴스를 합니다.
        a.AddComponent<MyPosition>();
        a.SendMessage("MyPos", gameObject.transform, SendMessageOptions.DontRequireReceiver);
        a.SendMessage("YPos", 2f, SendMessageOptions.DontRequireReceiver);
        gameObject.GetComponent<PlayerInfo>().basicAttackDamage += 2.5f;

        sound = a.GetComponent<AudioSource>();
        sound.Play();

        GameMgr.Instance.DestroyTarget(a, 2f);

        GameMgr.Instance.uIMgr.UseItem(itemNum);
        GameMgr.Instance.inventory.RemoveInventory(itemNum);
        Destroy(GetComponent<BasicAttackDamageUP>());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class KickHitbox : MonoBehaviourPun
{
    //Attacker declaration
    private int attacker;
    Vector3 pos;

    //Added list of attacked enemies
    private List<GameObject> attackList = new List<GameObject>();

    //Receives the attack view ID as a sand message.
    private void AttackerName(int name)
    {
        attacker = name;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Only enemies not on the list are hit.
        if (other.tag == "Player" && attackList.Contains(other.gameObject) == false)
        {
            other.gameObject.GetPhotonView().RPC("RPC_hit", RpcTarget.All, 10f, attacker, state.Stun, 2f);
            transform.Translate(0, 0, 10f);
            pos = transform.position;
            transform.Translate(0, 0, -10f);
            other.gameObject.GetPhotonView().RPC("BackMove", RpcTarget.All,pos, 0.5f, 30);
            //Adds the attacked enemy to the list to avoid getting hit again after escaping the collider
            attackList.Add(other.gameObject);
        }

    }
}
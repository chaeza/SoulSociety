using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    [SerializeField] float HP=100;
    [SerializeField] float HPrecovery =0.5f;
    [SerializeField] float basicAttackDamage = 10;


    private void OnTriggerEnter(Collider other)
    {
        other.SendMessage("hit",SendMessageOptions.DontRequireReceiver);  
    }

    void hit()
    {
        Debug.Log("¸ÂÀ½");
    }
}

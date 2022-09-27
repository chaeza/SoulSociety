using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPool : MonoBehaviour
{
    public SpawnMgr spawnMgr;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "mainPlayer")
        {
            gameObject.SetActive(false);
            spawnMgr = FindObjectOfType<SpawnMgr>();
            spawnMgr.SoulRelase(gameObject);
            //StartCoroutine("SpawnItem");  
        }
    }
}

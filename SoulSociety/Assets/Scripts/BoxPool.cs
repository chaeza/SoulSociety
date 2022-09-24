using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPool : MonoBehaviour
{
    public SpawnMgr spawnMgr;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "mainPlayer")
        {
            // gameObject.SetActive(false);
            spawnMgr = FindObjectOfType<SpawnMgr>();
            spawnMgr.Relase(gameObject);
            //StartCoroutine("SpawnItem");  
        }
    }
}

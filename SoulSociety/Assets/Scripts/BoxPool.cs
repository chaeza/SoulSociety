using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPool : MonoBehaviour
{
    public SpawnMgr spawnMgr;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            spawnMgr.Relase(gameObject);
            SpawnItem();
        }
    }

    IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(5f);
        spawnMgr.Get();
    }

}

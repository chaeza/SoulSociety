using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    // 플레이어 위치
    public GameObject target;
    // 미니맵 랜더 위치
    public Vector3 offset;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(-21.96072336438869f, -49.92076579874212f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
        if (target == null)
            return;
        else
        transform.localPosition = new Vector3(target.transform.position.x* 1.176286889085515f, target.transform.position.z* 1.154115395132622f, 0)+offset;
        
    }

    void FindPlayer()
    {
        target = GameObject.FindWithTag("mainPlayer");
    }
}

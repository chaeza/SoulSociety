using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    // 소환할 Object
    public GameObject itemPrefab;
    GameObject[] groundCh = null;
    public GameObject[] gameObjects;
    public BlackHole blackHole;
    int ran;
    int boxCheck;

    Queue<GameObject> queue = new Queue<GameObject>();
    private void Awake()
    {
        groundCh = GameObject.FindGameObjectsWithTag("Ground");
    }

    private void Start()
    {
        Init();
    }

    //생성할때  //풀링전에 생성을 한다 (매니저)
    void Init()
    {
        for (int i = 0; i < 16; i++)
        {
            ran = Random.Range(0, groundCh.Length);
            GameObject obj = Create();
        }
    }

    GameObject Create()  //생성한다 (매니저)
    {
        GameObject obj = Instantiate(itemPrefab, groundCh[ran].transform.position + new Vector3(4, 3, 3.5f), Quaternion.identity);

        return obj;
    }

    //풀에서 내보낼때   // 이건 박스지?
    public GameObject Get()
    {
        GameObject obj;

        //있다면 큐에서 빼내서 쓴다 
        obj = queue.Dequeue();

        obj.transform.position = groundCh[ran].transform.position;
        obj.gameObject.SetActive(true);

        return obj;
    }

    //풀에 들어올때
    public void Relase(GameObject obj)
    {   //큐로 다시 보낸다
        obj.gameObject.SetActive(false);
        queue.Enqueue(obj);
    }
}

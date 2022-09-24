using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    // 소환할 Object
    GameObject[] groundCh = null;     //그라운드태그 모을 장소
    public GameObject itemPrefab;     //생성될 아이템 프리펩
    public GameObject soulPrefab;     //생성될 소울 프리펩
    public GameObject[] gameObjects;

    int ran;   //아이템 랜덤 위치
    int ran2;  //소울 랜덤 위치
    int ran3;  //새로운 아이템 랜덤 위치
    int ran4;  //새로운 소울 랜덤 위치

    Queue<GameObject> queue = new Queue<GameObject>();

    private void Awake()
    {
        groundCh = GameObject.FindGameObjectsWithTag("Ground");        //그라운드 태그인것 모두 찾아 배열에 넣다
    }

    private void Start()
    {
        ItemInit();
        SoulInit();

    }

    //생성할때  //풀링전에 생성을 한다 (매니저)
    void ItemInit()     //아이템 x개 생성
    {
        for (int i = 0; i < 16; i++)
        {
            ran = Random.Range(0, groundCh.Length);
            GameObject obj = Create();
        }
    }

    void SoulInit()    //소울 x개 생성
    {
        for (int i = 0; i < 5; i++)
        {
            ran2 = Random.Range(0, groundCh.Length);
            Instantiate(soulPrefab, groundCh[ran2].transform.position + new Vector3(4, 4, 3.5f), Quaternion.identity);
        }
    }


    GameObject Create()  //생성한다 (매니저)
    {
        GameObject obj = Instantiate(itemPrefab, groundCh[ran].transform.position + new Vector3(4, 3, 3.5f), Quaternion.identity);

        return obj;
    }

    //풀에서 내보낼때
    public GameObject ItemGet()
    {
        ran3 = Random.Range(0, groundCh.Length);

        GameObject obj;

        //있다면 큐에서 빼내서 쓴다 
        obj = queue.Dequeue();

        obj.transform.position = groundCh[ran3].transform.position + new Vector3(4, 3, 3.5f);
        obj.gameObject.SetActive(true);

        return obj;
    }

    public GameObject SoulGet()
    {
        ran4 = Random.Range(0, groundCh.Length);

        GameObject obj;

        //있다면 큐에서 빼내서 쓴다 
        obj = queue.Dequeue();

        obj.transform.position = groundCh[ran3].transform.position + new Vector3(4, 4, 3.5f);
        obj.gameObject.SetActive(true);

        return obj;
    }


    //풀에 들어올때
    public void Relase(GameObject obj)
    {   //큐로 다시 보낸다
        obj.gameObject.SetActive(false);
        queue.Enqueue(obj);

        StartCoroutine(TenSec());
    }

    public void SoulRelase(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        queue.Enqueue(obj);

        StartCoroutine(Minute());
    }


    IEnumerator TenSec()
    {
        yield return new WaitForSeconds(10f);

        ItemGet();
    }

    IEnumerator Minute()
    {
        yield return new WaitForSeconds(60f);

        SoulGet();
    }
}

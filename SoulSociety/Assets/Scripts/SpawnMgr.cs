using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SpawnMgr : MonoBehaviourPun
{
    private static SpawnMgr spawn_instance = null;
    public static SpawnMgr spawnMgr
    {
        get
        {
            if (spawn_instance == null)
            {
                spawn_instance = GameObject.FindObjectOfType<SpawnMgr>();
                if (spawn_instance == null)
                {
                    spawn_instance= new SpawnMgr();
                }
            }
            return spawn_instance;
        }
            
    }    // ��ȯ�� Object
    GameObject[] groundCh = null;     //�׶����±� ���� ���
    public GameObject itemPrefab;     //������ ������ ������
    public GameObject soulPrefab;     //������ �ҿ� ������
    public GameObject[] gameObjects;

    int ran;   //������ ���� ��ġ
    int ran2;  //�ҿ� ���� ��ġ
    int ran3;  //���ο� ������ ���� ��ġ
    int ran4;  //���ο� �ҿ� ���� ��ġ

    Queue<GameObject> queue = new Queue<GameObject>();    //Ǯ ������ ť

    private void Awake()
    {
        groundCh = GameObject.FindGameObjectsWithTag("Ground");        //�׶��� �±��ΰ� ��� ã�� �迭�� �ִ�
    }

    private void Start()
    {
        ItemInit();    // ������ �ҿ� ����
        SoulInit();
    }

    //�����Ҷ�  //Ǯ������ ������ �Ѵ� (�Ŵ���)
    public void ItemInit()     //������ x�� ����

    {
        for (int i = 0; i < 16; i++)
        {
            ran = Random.Range(0, groundCh.Length);
            GameObject obj = PhotonNetwork.Instantiate("ItemBox", groundCh[ran].transform.position + new Vector3(4, 4, 3.5f), Quaternion.identity);
        }
    }

    public void SoulInit()    //�ҿ� x�� ����
    {
        for (int i = 0; i < 5; i++)
        {
            ran2 = Random.Range(0, groundCh.Length);
           //Instantiate(soulPrefab, groundCh[ran2].transform.position + new Vector3(4, 4, 3.5f), Quaternion.identity);
            PhotonNetwork.Instantiate("BlueSoul", groundCh[ran2].transform.position + new Vector3(4, 4, 3.5f), Quaternion.identity);

        }
    }

/*
    GameObject Create()  //�����Ѵ� (�Ŵ���)
    {
        GameObject obj = Instantiate(itemPrefab, groundCh[ran].transform.position + new Vector3(4, 3, 3.5f), Quaternion.identity);

        return obj;
    }*/

    //Ǯ���� ��������
    public GameObject ItemGet()   //�÷��̾ �ڽ��� ������
    {
        ran3 = Random.Range(0, groundCh.Length);

        GameObject obj;

        //�ִٸ� ť���� ������ ���� 
        obj = queue.Dequeue();

        obj.transform.position = groundCh[ran3].transform.position + new Vector3(4, 3, 3.5f);
        obj.gameObject.SetActive(true);

        return obj;
    }

    public GameObject SoulGet()  //�ҿ￡ ������
    {   
        ran4 = Random.Range(0, groundCh.Length);

        GameObject obj;

        //�ִٸ� ť���� ������ ���� 
        obj = queue.Dequeue();

        obj.transform.position = groundCh[ran3].transform.position + new Vector3(4, 4, 3.5f);
        obj.gameObject.SetActive(true);

        return obj;
    }

    //������
    //Ǯ�� ���ö�
    [PunRPC]
    public void Relase(GameObject obj)
    {   //ť�� �ٽ� ������
        obj.gameObject.SetActive(false);   //�÷��̾ ������ false��Ű�� ť�� ����
        queue.Enqueue(obj);

        StartCoroutine(TenSec());    //x�� �ڷ�ƾ ����
    }
    [PunRPC]
    //��ȥ ������Ʈ Ǯ
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

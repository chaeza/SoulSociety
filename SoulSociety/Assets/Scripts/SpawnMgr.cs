using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMgr : MonoBehaviour
{
    // ��ȯ�� Object
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

    //�����Ҷ�  //Ǯ������ ������ �Ѵ� (�Ŵ���)
    void Init()
    {
        for (int i = 0; i < 16; i++)
        {
            ran = Random.Range(0, groundCh.Length);
            GameObject obj = Create();
        }
    }

    GameObject Create()  //�����Ѵ� (�Ŵ���)
    {
        GameObject obj = Instantiate(itemPrefab, groundCh[ran].transform.position + new Vector3(4, 3, 3.5f), Quaternion.identity);

        return obj;
    }

    //Ǯ���� ��������   // �̰� �ڽ���?
    public GameObject Get()
    {
        GameObject obj;

        //�ִٸ� ť���� ������ ���� 
        obj = queue.Dequeue();

        obj.transform.position = groundCh[ran].transform.position;
        obj.gameObject.SetActive(true);

        return obj;
    }

    //Ǯ�� ���ö�
    public void Relase(GameObject obj)
    {   //ť�� �ٽ� ������
        obj.gameObject.SetActive(false);
        queue.Enqueue(obj);
    }
}

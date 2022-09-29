using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class SpawnMgr : MonoBehaviourPun
{
    // ��ȯ�� Object
    GameObject[] groundCh = null;     //�׶����±� ���� ���
    public GameObject itemPrefab;     //������ ������ ������
    public GameObject soulPrefab;     //������ �ҿ� ������
    public GameObject[] gameObjects;

    int ran;   //������ ���� ��ġ
    int ran2;  //�ҿ� ���� ��ġ
    int ran3;  //���ο� ������ ���� ��ġ
    int ran4;  //���ο� �ҿ� ���� ��ġ

    Queue<GameObject> queue = new Queue<GameObject>();    //Ǯ ������ ť

    PhotonView Master;

    private void Awake()
    {
        groundCh = GameObject.FindGameObjectsWithTag("Ground");        //�׶��� �±��ΰ� ��� ã�� �迭�� �ִ�
    }


    //�����Ҷ�  //Ǯ������ ������ �Ѵ� (�Ŵ���)
    [PunRPC]
    public void ItemInit()     //������ x�� ����
    {
        for (int i = 0; i < 16; i++)
        {
            ran = Random.Range(0, groundCh.Length);
            GameObject obj = PhotonNetwork.Instantiate("ItemBox", groundCh[ran].transform.position + new Vector3(4, 3, 3.5f), Quaternion.identity);
        }
    }
    [PunRPC]
    public void SoulInit()    //�ҿ� x�� ����
    {
        for (int i = 0; i < 5; i++)
        {
            ran2 = Random.Range(0, groundCh.Length);
            PhotonNetwork.Instantiate("BlueSoul", groundCh[ran2].transform.position + new Vector3(4, 4, 3.5f), Quaternion.identity);
        }
    }

    /*
        GameObject Create()  //�����Ѵ� (�Ŵ���)
        {
            GameObject obj = Instantiate(itemPrefab, groundCh[ran].transform.position + new Vector3(4, 3, 3.5f), Quaternion.identity);

            return obj;
        }*/

    [PunRPC]
    //Ǯ���� ��������
    public void ItemGet(int groundNum)   //�÷��̾ �ڽ��� ������
    {
   

        GameObject obj;

        //�ִٸ� ť���� ������ ���� 
        obj = queue.Dequeue();

        obj.transform.position = groundCh[groundNum].transform.position + new Vector3(4, 3, 3.5f);
        obj.gameObject.SetActive(true);

    }
    [PunRPC]
    public void SoulGet(int groundNum)  //�ҿ￡ ������
    {

        GameObject obj;

        //�ִٸ� ť���� ������ ���� 
        obj = queue.Dequeue();

        obj.transform.position = groundCh[groundNum].transform.position + new Vector3(4, 4, 3.5f);
        obj.gameObject.SetActive(true);

    }

    [PunRPC]
    public void FindItemInPool(int ViewID2)
    {
        GameObject obj = null;
        PhotonView[] allPoton = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < allPoton.Length; i++)
        {
            if (allPoton[i].ViewID == ViewID2)
            {
                obj = allPoton[i].gameObject;
            }
        }
        //photonView.RPC("Relase", RpcTarget.All, obj);

        Relase(obj);
    }

    [PunRPC]
    public void FindSoulInPool(int ViewID2)
    {
        GameObject obj = null;
        PhotonView[] allPoton = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < allPoton.Length; i++)
        {
            if (allPoton[i].ViewID == ViewID2)
            {
                obj = allPoton[i].gameObject;
            }
        }
       // photonView.RPC("SoulRelase", RpcTarget.All, obj);

         SoulRelase(obj);
    }


    //������
    //Ǯ�� ���ö�

    public void Relase(GameObject obj)
    {   //ť�� �ٽ� ������
        obj.gameObject.SetActive(false);   //�÷��̾ ������ false��Ű�� ť�� ����
        queue.Enqueue(obj);

        StartCoroutine(TenSec());    //x�� �ڷ�ƾ ����
    }

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
       
        if (PhotonNetwork.IsMasterClient)
        {
            ran3 = Random.Range(0, groundCh.Length);

            photonView.RPC("ItemGet", RpcTarget.All, ran3);
        }
    }

    IEnumerator Minute()
    {
        yield return new WaitForSeconds(10f);
     
        if (PhotonNetwork.IsMasterClient)
        {
            ran4 = Random.Range(0, groundCh.Length);

            photonView.RPC("SoulGet", RpcTarget.All, ran4);
        }

    }
}

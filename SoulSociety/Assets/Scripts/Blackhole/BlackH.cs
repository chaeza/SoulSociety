using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BlackH : MonoBehaviourPun
{
    // �༺�� ������ ��� �� �迭�� �����.
    Collider[] stars;


    // �ð��� ����� ������ �����.
    float time;

    // ������ ����� ������ �����.
    Vector3 dir;

    private void Start()
    {
        //stars�� ��ó���ִ� �ݶ��̴����� �迭�� �� �ִ´�

    }

    void Update()
    {
        // �ð��� �帣�� �����.
        time += Time.deltaTime;
        stars = Physics.OverlapSphere(transform.position, 40f);
        BlackHoleChack();
    }


    void BlackHoleChack()
    {
        // foreach �ݺ����� ������, stars�迭�� �����ϴ� ������Ʈ �ϳ��ϳ��� ��Ʈ�� ���ش�.
        foreach (Collider star in stars)
        {
            // ��Ȧ�� ������Ʈ�� �ǽð� �Ÿ��� �����Ѵ�.
            float dis = Vector3.Distance(this.transform.position, star.transform.position);

            // 1�ʰ� ������ ��(= ��Ȧ�� �����ǰ� 1�ʰ� ������ ��)
            if (time > 6)
            {
                // �༺���κ��� ��Ȧ�� ���ϴ� ������ ���Ѵ�.
                dir = this.transform.position - star.transform.position;
                // �༺�� ��ġ�� ��Ȧ�� �������� õõ�� �̵���Ų��.

                //if(dir = 1f)
                star.gameObject.transform.position += dir * 0.8f * Time.deltaTime;
            }
            // ��Ȧ�� �༺�� �Ÿ��� 0.3f���ϰ� �Ǹ�
            if (dis <= 0.3f)
            {
                // �༺�� ��Ȧ�� ���� �� ������ �̵���Ų��.
                star.gameObject.transform.position += dir * 1f * Time.deltaTime;
            }
            // ��Ȧ�� �༺�� �Ÿ��� 0.05f ���ϰ� �Ǹ�
            if (dis <= 0.05f)
            {
                // �༺�� ��Ȧ�� �� ������ �̵���Ų��.
                star.gameObject.transform.position += dir * 1.2f * Time.deltaTime;
            }
            // ��Ȧ�� �������� 10�ʰ� ������
            if (time >= 10)
            {

                if (star.tag == "Player" || star.tag == "mainPlayer")
                {
                    star.GetComponent<PlayerInfo>().playerState = state.Die;
                    star.gameObject.tag = "DiePlayer";

                    GameMgr.Instance.AliveNumCheck();

                    star.gameObject.SetActive(false);

                }
                else
                {
                    /*if(star.transform != null)   
                    Destroy(star.gameObject);*/
                    star.gameObject.SetActive(false);
                }
                // ��Ȧ�� ��������.
                this.gameObject.SetActive(false);

            }

        }
    }
}




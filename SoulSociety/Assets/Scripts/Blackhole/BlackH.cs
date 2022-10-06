using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class BlackH : MonoBehaviourPun
{
    // 행성과 별들을 담아 둘 배열을 만든다.
    Collider[] stars;


    // 시간을 담당할 변수를 만든다.
    float time;

    // 방향을 담당할 변수를 만든다.
    Vector3 dir;

    private void Start()
    {
        //stars에 근처에있는 콜라이더들을 배열에 다 넣는다

    }

    void Update()
    {
        // 시간을 흐르게 만든다.
        time += Time.deltaTime;
        stars = Physics.OverlapSphere(transform.position, 40f);
        BlackHoleChack();
    }


    void BlackHoleChack()
    {
        // foreach 반복문을 돌려서, stars배열에 존재하는 오브젝트 하나하나를 컨트롤 해준다.
        foreach (Collider star in stars)
        {
            // 블랙홀과 오브젝트의 실시간 거리를 측정한다.
            float dis = Vector3.Distance(this.transform.position, star.transform.position);

            // 1초가 지났을 때(= 블랙홀이 생성되고 1초가 지났을 때)
            if (time > 6)
            {
                // 행성으로부터 블랙홀로 향하는 방향을 구한다.
                dir = this.transform.position - star.transform.position;
                // 행성의 위치를 블랙홀의 방향으로 천천히 이동시킨다.

                //if(dir = 1f)
                star.gameObject.transform.position += dir * 0.8f * Time.deltaTime;
            }
            // 블랙홀과 행성의 거리가 0.3f이하가 되면
            if (dis <= 0.3f)
            {
                // 행성을 블랙홀로 조금 더 빠르게 이동시킨다.
                star.gameObject.transform.position += dir * 1f * Time.deltaTime;
            }
            // 블랙홀과 행성의 거리가 0.05f 이하가 되면
            if (dis <= 0.05f)
            {
                // 행성을 블랙홀로 더 빠르게 이동시킨다.
                star.gameObject.transform.position += dir * 1.2f * Time.deltaTime;
            }
            // 블랙홀이 생성된지 10초가 지나면
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
                // 블랙홀을 꺼버린다.
                this.gameObject.SetActive(false);

            }

        }
    }
}




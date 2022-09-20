using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1;

    Animator myAnimator;

    bool isMove = false;

    Vector3 moveTarget;
    Vector3 desiredDir;
    Vector3 targetDir;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameMgr.Instance.playerInput.inputKey == KeyCode.Mouse1)
        {
            Move(Input.mousePosition);
        }

        if (isMove == true)
        {
            if (Vector3.Distance(desiredDir, transform.position) > 0.1f)
            {
                myAnimator.SetBool("isMove", true);

                gameObject.GetComponent<Rigidbody>().velocity = targetDir.normalized * moveSpeed;
            }
            else
                MoveStop();
        }
    }
    public void Move(Vector3 mousePos)
    {
        // 움직임 애니메이션
        // 사운드
        // 실제 움직임 (포지션 변경)
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out hit, float.MaxValue);

        Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red, 5f);
        Debug.Log(hit.collider.tag.ToString());
        Debug.Log(hit.ToString());

        if (hit.collider.tag == "Ground")
        {
            desiredDir = hit.point;
            desiredDir.y = transform.position.y;
            targetDir = desiredDir - transform.position;
            transform.rotation = Quaternion.LookRotation(targetDir);
            Debug.Log(targetDir.ToString());
            isMove = true;
        }
    }

    public void MoveStop()
    {
        myAnimator.SetBool("isMove", false);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isMove = false;
        Debug.Log("false");
    }
}

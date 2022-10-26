using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KAKArote : MonoBehaviour
{
    public Rigidbody body = null;
    NavMeshAgent navMeshAgent;
    public float Speed = 0;
    public Vector3 desiredDir;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float dir1 = Random.Range(-2f, 2f);
        float dir2 = Random.Range(-2f, 2f);

        if (Vector3.Distance(desiredDir, transform.position) > 0.1f)
        {
           // myAnimator.SetBool("isMove", true);
            navMeshAgent.updateRotation = true;
            navMeshAgent.updatePosition = true;

            navMeshAgent.SetDestination(desiredDir);
        }

        //   navMeshAgent.SetDestination(dir1*Speed, 3 * Speed, dir2 * Speed);
    }
}

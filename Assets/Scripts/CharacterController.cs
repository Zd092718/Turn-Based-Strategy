using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveTarget;

    public NavMeshAgent navAgent;

    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        moveTarget = transform.position;
        navAgent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //moving to a point
        if (isMoving == true)
        {
            // transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

            if (GameManager.instance.activePlayer == this)
            {
                CameraController.instance.SetMoveTarget(transform.position);
                if (Vector3.Distance(transform.position, moveTarget) < .2f)
                {
                    isMoving = false;
                }
            }
        }
    }

    public void MoveToPoint(Vector3 movePoint)
    {
        moveTarget = movePoint;

        navAgent.SetDestination(moveTarget);
        isMoving = true;
    }
}

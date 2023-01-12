using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector3 moveTarget;

    [SerializeField] private NavMeshAgent navAgent;

    private bool isMoving;
    [SerializeField] private bool isEnemy;

    [SerializeField] private float moveRange = 3.5f;

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

            if (GameManager.Instance.GetActivePlayer() == this)
            {
                CameraController.Instance.SetMoveTarget(transform.position);
                if (Vector3.Distance(transform.position, moveTarget) < .2f)
                {
                    isMoving = false;

                    GameManager.Instance.FinishedMovement();
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

    public bool GetIsEnemy()
    {
        return isEnemy;
    }

    public float GetMoveRange()
    {
        return moveRange;
    }
}

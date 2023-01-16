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

    [SerializeField] private float moveRange = 3.5f, runRange = 8f;

    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private List<CharacterController> meleeTargets = new List<CharacterController>();
    //[HideInInspector]
    public int currentMeleeTarget;

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

    public void GetNearbyMeleeTargets()
    {
        meleeTargets.Clear();

        if (isEnemy == false)
        {
            foreach (CharacterController cc in GameManager.Instance.GetEnemyTeam())
            {
                if (Vector3.Distance(transform.position, cc.transform.position) < meleeRange)
                {
                    meleeTargets.Add(cc);
                }
            }
        }
        else
        {
            foreach (CharacterController cc in GameManager.Instance.GetPlayerTeam())
            {
                if (Vector3.Distance(transform.position, cc.transform.position) < meleeRange)
                {
                    meleeTargets.Add(cc);
                }
            }
        }

        if(currentMeleeTarget >= meleeTargets.Count)
        {
            currentMeleeTarget = 0;
        }
    }

    public void PerformMelee()
    {
        meleeTargets[currentMeleeTarget].gameObject.SetActive(false);
    }

    public bool GetIsEnemy()
    {
        return isEnemy;
    }

    public float GetMoveRange()
    {
        return moveRange;
    }
    public float GetRunRange()
    {
        return runRange;
    }

    public List<CharacterController> GetMeleeTargetsList()
    {
        return meleeTargets;
    }
}

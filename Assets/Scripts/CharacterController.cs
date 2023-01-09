using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveTarget;

    // Start is called before the first frame update
    void Start()
    {
        moveTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //moving to a point
        if (transform.position != moveTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

            if (GameManager.instance.activePlayer == this)
            {
                if (Vector3.Distance(transform.position, moveTarget) > .1f)
                {
                    CameraController.instance.SetMoveTarget(transform.position);
                }
            }
        }
    }

    public void MoveToPoint(Vector3 movePoint)
    {
        moveTarget = movePoint;
    }
}

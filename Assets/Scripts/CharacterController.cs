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
        transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
    }

    public void MoveToPoint(Vector3 movePoint)
    {
        moveTarget = movePoint;
    }
}

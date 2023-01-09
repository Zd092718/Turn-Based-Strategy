using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    private void OnMouseDown()
    {
        //FindObjectOfType<CharacterController>().MoveToPoint(transform.position);
        GameManager.instance.GetActivePlayer().MoveToPoint(transform.position);
    }
}

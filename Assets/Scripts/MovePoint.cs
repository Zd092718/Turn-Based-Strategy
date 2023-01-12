using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    private void OnMouseDown()
    {
        //FindObjectOfType<CharacterController>().MoveToPoint(transform.position);
        GameManager.Instance.GetActivePlayer().MoveToPoint(transform.position);

        MoveGrid.Instance.HideMovePoints();

        PlayerInputMenu.Instance.HideMenus();
    }
}

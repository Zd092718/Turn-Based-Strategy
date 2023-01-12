using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    private void OnMouseDown()
    {
        //FindObjectOfType<CharacterController>().MoveToPoint(transform.position);

        if(Input.mousePosition.y > Screen.height * .1)
        {
        GameManager.Instance.GetActivePlayer().MoveToPoint(transform.position);

        MoveGrid.Instance.HideMovePoints();

        PlayerInputMenu.Instance.HideMenus();
        }
    }
}

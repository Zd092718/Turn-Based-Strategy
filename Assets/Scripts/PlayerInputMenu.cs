using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMenu : MonoBehaviour
{
    public static PlayerInputMenu Instance { get; private set; }

    [SerializeField] private GameObject inputMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void HideMenus()
    {
        inputMenu.SetActive(false); 
    }

    public void ShowInputMenu()
    {
        inputMenu.SetActive(true);
    }

    public void ShowMove()
    {
        if(GameManager.Instance.GetRemainingTurnPoints() >= 1)
        {
            MoveGrid.Instance.ShowPointsInRange(GameManager.Instance.GetActivePlayer().GetMoveRange(), GameManager.Instance.GetActivePlayer().transform.position);
        }
    }
}

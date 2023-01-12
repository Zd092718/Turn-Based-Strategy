using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMenu : MonoBehaviour
{
    public static PlayerInputMenu Instance { get; private set; }

    [SerializeField] private GameObject inputMenu, moveMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void HideMenus()
    {
        inputMenu.SetActive(false); 
        moveMenu.SetActive(false);
    }

    public void ShowInputMenu()
    {
        inputMenu.SetActive(true);
    }

    public void ShowMoveMenu()
    {
        HideMenus();
        moveMenu.SetActive(true);

        ShowMove();
    }

    public void ShowMove()
    {
        if(GameManager.Instance.GetRemainingTurnPoints() >= 1)
        {
            MoveGrid.Instance.ShowPointsInRange(GameManager.Instance.GetActivePlayer().GetMoveRange(), GameManager.Instance.GetActivePlayer().transform.position);
            GameManager.Instance.SetCurrentActionCost(1);
        }
    }

    public void ShowRun()
    {
        if (GameManager.Instance.GetRemainingTurnPoints() >= 2)
        {
            MoveGrid.Instance.ShowPointsInRange(GameManager.Instance.GetActivePlayer().GetRunRange(), GameManager.Instance.GetActivePlayer().transform.position);
            GameManager.Instance.SetCurrentActionCost(2);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInputMenu : MonoBehaviour
{
    public static PlayerInputMenu Instance { get; private set; }

    [SerializeField] private GameObject inputMenu, moveMenu, meleeMenu;
    [SerializeField] private TMP_Text turnPointsText;

    private void Awake()
    {
        Instance = this;
    }

    public void HideMenus()
    {
        inputMenu.SetActive(false);
        moveMenu.SetActive(false);
        meleeMenu.SetActive(false);
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

    public void HideMoveMenu()
    {
        HideMenus();
        MoveGrid.Instance.HideMovePoints();
        ShowInputMenu();
    }

    public void ShowMove()
    {
        if (GameManager.Instance.GetRemainingTurnPoints() >= 1)
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

    public void UpdateTurnPointText(int turnPoints)
    {
        turnPointsText.text = "Turn Points Remaining: " + turnPoints;
    }

    public TMP_Text GetTurnPointText()
    {
        return turnPointsText;
    }

    public void SkipTurn()
    {
        GameManager.Instance.EndTurn();
    }

    public void ShowMeleeMenu()
    {
        HideMenus();
        meleeMenu.SetActive(true);
    }

    public void HideMeleeMenu()
    {
        HideMenus();
        ShowInputMenu();
    }

    public void CheckMelee()
    {
        GameManager.Instance.GetActivePlayer().GetNearbyMeleeTargets();

        if (GameManager.Instance.GetActivePlayer().GetMeleeTargetsList().Count > 0)
        {
            ShowMeleeMenu();
        }
        else
        {
            Debug.Log("No enemies in melee range.");
        }
    }
}

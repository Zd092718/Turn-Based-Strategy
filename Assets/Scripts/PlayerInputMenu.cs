using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInputMenu : MonoBehaviour
{
    public static PlayerInputMenu Instance { get; private set; }

    [SerializeField] private GameObject inputMenu, moveMenu, meleeMenu, shootMenu;
    [SerializeField] private TMP_Text turnPointsText;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private TMP_Text hitChanceText;

    [SerializeField] private float errorDisplayTime = 2f;
    private float errorCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (errorCounter > 0)
        {
            errorCounter -= Time.deltaTime;
            if (errorCounter <= 0)
            {
                errorText.gameObject.SetActive(false);
            }
        }
    }

    public void HideMenus()
    {
        inputMenu.SetActive(false);
        moveMenu.SetActive(false);
        meleeMenu.SetActive(false);
        shootMenu.SetActive(false);
    }

    public void ShowInputMenu()
    {
        inputMenu.SetActive(true);
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

    public IEnumerator WaitToEndActionCo(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        GameManager.Instance.SpendTurnPoints();
    }

    public void ShowErrorText(string messageToShow)
    {
        errorText.text = messageToShow;
        errorText.gameObject.SetActive(true);

        errorCounter = errorDisplayTime;
    }

    #region Movement Functions
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
    #endregion

    #region Melee Functions
    public void ShowMeleeMenu()
    {
        HideMenus();
        meleeMenu.SetActive(true);
    }

    public void HideMeleeMenu()
    {
        HideMenus();
        ShowInputMenu();
        GameManager.Instance.GetTargetDisplay().SetActive(false);
    }

    public void CheckMelee()
    {
        GameManager.Instance.GetActivePlayer().GetNearbyMeleeTargets();

        if (GameManager.Instance.GetActivePlayer().GetMeleeTargetsList().Count > 0)
        {
            ShowMeleeMenu();
            GameManager.Instance.GetTargetDisplay().SetActive(true);
            GameManager.Instance.GetTargetDisplay().transform.position = GameManager.Instance.GetActivePlayer().GetMeleeTargetsList()[GameManager.Instance.GetActivePlayer().currentMeleeTarget].transform.position;

            GameManager.Instance.GetActivePlayer().LookAtTarget(GameManager.Instance.GetActivePlayer().GetMeleeTargetsList()[GameManager.Instance.GetActivePlayer().currentMeleeTarget].transform);
        }
        else
        {
            ShowErrorText("No Enemies in Melee Range!");
        }
    }

    public void MeleeHit()
    {
        GameManager.Instance.GetActivePlayer().PerformMelee();
        GameManager.Instance.SetCurrentActionCost(1);

        HideMenus();
        ///GameManager.Instance.SpendTurnPoints();
        GameManager.Instance.GetTargetDisplay().SetActive(false);

        StartCoroutine(WaitToEndActionCo(1f));
    }

    public void NextMeleeTarget()
    {
        GameManager.Instance.GetActivePlayer().currentMeleeTarget++;
        if (GameManager.Instance.GetActivePlayer().currentMeleeTarget >= GameManager.Instance.GetActivePlayer().GetMeleeTargetsList().Count)
        {
            GameManager.Instance.GetActivePlayer().currentMeleeTarget = 0;
        }
        GameManager.Instance.GetTargetDisplay().transform.position = GameManager.Instance.GetActivePlayer().GetMeleeTargetsList()[GameManager.Instance.GetActivePlayer().currentMeleeTarget].transform.position;
        GameManager.Instance.GetActivePlayer().LookAtTarget(GameManager.Instance.GetActivePlayer().GetMeleeTargetsList()[GameManager.Instance.GetActivePlayer().currentMeleeTarget].transform);
    }
    #endregion

    #region Shooting Functions
    public void ShowShootMenu()
    {
        HideMenus();
        shootMenu.SetActive(true);

        UpdateHitChance();
    }

    public void HideShootMenu()
    {
        HideMenus();
        ShowInputMenu();
        GameManager.Instance.GetTargetDisplay().SetActive(false);
    }

    public void CheckShoot()
    {
        GameManager.Instance.GetActivePlayer().GetShootTargets();

        if (GameManager.Instance.GetActivePlayer().GetShootTargetsList().Count > 0)
        {
            ShowShootMenu();

            GameManager.Instance.GetTargetDisplay().SetActive(true);
            GameManager.Instance.GetTargetDisplay().transform.position = GameManager.Instance.GetActivePlayer().GetShootTargetsList()[GameManager.Instance.GetActivePlayer().currentShootTarget].transform.position;

            GameManager.Instance.GetActivePlayer().LookAtTarget(GameManager.Instance.GetActivePlayer().GetShootTargetsList()[GameManager.Instance.GetActivePlayer().currentShootTarget].transform);
        }
        else
        {
            ShowErrorText("No Enemies In Firing Range");
        }
    }

    public void NextShootTarget()
    {
        GameManager.Instance.GetActivePlayer().currentShootTarget++;
        if (GameManager.Instance.GetActivePlayer().currentShootTarget >= GameManager.Instance.GetActivePlayer().GetShootTargetsList().Count)
        {
            GameManager.Instance.GetActivePlayer().currentShootTarget = 0;
        }
        GameManager.Instance.GetTargetDisplay().transform.position = GameManager.Instance.GetActivePlayer().GetShootTargetsList()[GameManager.Instance.GetActivePlayer().currentShootTarget].transform.position;

        UpdateHitChance();

        GameManager.Instance.GetActivePlayer().LookAtTarget(GameManager.Instance.GetActivePlayer().GetShootTargetsList()[GameManager.Instance.GetActivePlayer().currentShootTarget].transform);
    }

    public void FireShot()
    {
        GameManager.Instance.GetActivePlayer().FireShot();
        GameManager.Instance.SetCurrentActionCost(1);
        HideMenus();
        GameManager.Instance.GetTargetDisplay().SetActive(false);

        StartCoroutine(WaitToEndActionCo(1f));
    }

    public void UpdateHitChance()
    {
        float hitChance = Random.Range(50f, 95f);

        hitChanceText.text = "Chance To Hit: " + GameManager.Instance.GetActivePlayer().CheckShotChance().ToString("F1") + "%";
    }

    #endregion
}

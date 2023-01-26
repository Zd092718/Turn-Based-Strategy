using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    [SerializeField] private CharacterController activePlayer;

    [SerializeField] private List<CharacterController> allChars = new List<CharacterController>();
    [SerializeField] private List<CharacterController> playerTeam = new List<CharacterController>(), enemyTeam = new List<CharacterController>();

    private int currentChar;

    [SerializeField] private int totalTurnPoints = 2;
    private int turnPointsRemaining;

    [SerializeField] private int currentActionCost = 1;
    [SerializeField] private GameObject targetDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
        List<CharacterController> tempList = new List<CharacterController>();

        tempList.AddRange(FindObjectsOfType<CharacterController>());

        int iterations = tempList.Count + 50;
        while (tempList.Count > 0 && iterations > 0)
        {
            int randomPick = Random.Range(0, tempList.Count);
            allChars.Add(tempList[randomPick]);

            tempList.RemoveAt(randomPick);
            iterations--;
        }

        foreach (CharacterController cc in allChars)
        {
            if (cc.GetIsEnemy() == false)
            {
                playerTeam.Add(cc);
            }
            else
            {
                enemyTeam.Add(cc);
            }
        }

        allChars.Clear();


        if (Random.value >= .5f)
        {

            allChars.AddRange(playerTeam);
            allChars.AddRange(enemyTeam);
        }
        else
        {
            allChars.AddRange(enemyTeam);
            allChars.AddRange(playerTeam);

        }

        activePlayer = allChars[0];
        CameraController.Instance.SetMoveTarget(activePlayer.transform.position);

        currentChar = -1;
        //PlayerInputMenu.Instance.HideMenus();
        EndTurn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FinishedMovement()
    {
        SpendTurnPoints();
    }

    public void SpendTurnPoints()
    {
        turnPointsRemaining -= currentActionCost;

        if (turnPointsRemaining <= 0)
        {
            EndTurn();
        }
        else
        {
            if (activePlayer.GetIsEnemy() == false)
            {
                //MoveGrid.Instance.ShowPointsInRange(activePlayer.GetMoveRange(), activePlayer.transform.position);

                PlayerInputMenu.Instance.ShowInputMenu();
            }
            else
            {
                PlayerInputMenu.Instance.HideMenus();

                activePlayer.GetBrain().ChooseAction();
            }
        }

        PlayerInputMenu.Instance.UpdateTurnPointText(turnPointsRemaining);
    }

    public void EndTurn()
    {
        currentChar++;

        if (currentChar >= allChars.Count)
        {
            currentChar = 0;
        }

        activePlayer = allChars[currentChar];

        CameraController.Instance.SetMoveTarget(activePlayer.transform.position);
       

        turnPointsRemaining = totalTurnPoints;

        if (activePlayer.GetIsEnemy() == false)
        {
            //MoveGrid.Instance.ShowPointsInRange(activePlayer.GetMoveRange(), activePlayer.transform.position);

            PlayerInputMenu.Instance.ShowInputMenu();
            PlayerInputMenu.Instance.GetTurnPointText().gameObject.SetActive(true);
        }
        else
        {
            //StartCoroutine(AISkipCo());

            PlayerInputMenu.Instance.HideMenus();
            PlayerInputMenu.Instance.GetTurnPointText().gameObject.SetActive(false);

            activePlayer.GetBrain().ChooseAction();
        }

        currentActionCost = 1;

        PlayerInputMenu.Instance.UpdateTurnPointText(turnPointsRemaining);

        activePlayer.SetDefending(false);
    }
    #region !Getters and Setters!
    public CharacterController GetActivePlayer()
    {
        return activePlayer;
    }

    public List<CharacterController> GetAllCharacters()
    {
        return allChars;
    }

    public IEnumerator AISkipCo()
    {
        yield return new WaitForSeconds(1f);
        EndTurn();
    }

    public int GetRemainingTurnPoints()
    {
        return turnPointsRemaining;
    }

    public int GetCurrentActionCost()
    {
        return currentActionCost;
    }

    public void SetCurrentActionCost(int currentActionCost)
    {
        this.currentActionCost = currentActionCost;
    }

    public List<CharacterController> GetEnemyTeam()
    {
        return enemyTeam;
    }

    public List<CharacterController> GetPlayerTeam()
    {
        return playerTeam;
    }

    public GameObject GetTargetDisplay()
    {
        return targetDisplay;
    }
    #endregion
}

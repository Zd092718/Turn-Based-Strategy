using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    [SerializeField] private CharacterController activePlayer;

    [SerializeField] private List<CharacterController> allChars = new List<CharacterController>();
    [SerializeField] private List<CharacterController> playerTeam = new List<CharacterController>(), enemyTeam = new List<CharacterController>();

    private int currentChar;

    [SerializeField] private int totalTurnPoints = 2;
    private int turnPointsRemaining;

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
        CameraController.instance.SetMoveTarget(activePlayer.transform.position);

        currentChar = -1;
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
        turnPointsRemaining -= 1;

        if (turnPointsRemaining <= 0)
        {
            EndTurn();
        }
        else
        {
            MoveGrid.instance.ShowPointsInRange(activePlayer.GetMoveRange(), activePlayer.transform.position);
        }
    }

    public void EndTurn()
    {
        currentChar++;

        if (currentChar >= allChars.Count)
        {
            currentChar = 0;
        }

        activePlayer = allChars[currentChar];

        CameraController.instance.SetMoveTarget(activePlayer.transform.position);

        turnPointsRemaining = totalTurnPoints;

        if (activePlayer.GetIsEnemy() == false)
        {
            MoveGrid.instance.ShowPointsInRange(activePlayer.GetMoveRange(), activePlayer.transform.position);
        }
        else
        {
            StartCoroutine(AISkipCo());
        }
    }

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
}

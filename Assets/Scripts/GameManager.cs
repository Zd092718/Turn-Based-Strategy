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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FinishedMovement()
    {
        EndTurn();
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
    }

    public CharacterController GetActivePlayer()
    {
        return activePlayer;
    }
}

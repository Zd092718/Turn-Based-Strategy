using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [SerializeField] private CharacterController charCon;

    [SerializeField] private float waitBeforeActing = 1f;

    [SerializeField] private float waitAfterActing = 2f;

    public void ChooseAction()
    {
        StartCoroutine(ChooseCo());
    }

    public IEnumerator ChooseCo()
    {
        Debug.Log(name + " is choosing an action.");

        yield return new WaitForSeconds(waitBeforeActing);

        bool actionTaken = false;


        charCon.GetNearbyMeleeTargets();

        if (charCon.GetMeleeTargetsList().Count > 0)
        {
            Debug.Log("Is Meleeing");

            charCon.SetCurrentMeleeTarget(Random.Range(0, charCon.GetMeleeTargetsList().Count));

            GameManager.Instance.SetCurrentActionCost(1);

            StartCoroutine(WaitToEndAction(waitAfterActing));

            charCon.PerformMelee();

            actionTaken = true;
        }


        if (actionTaken == false)
        {
            //Skip turn
            GameManager.Instance.EndTurn();
        }
    }

    public IEnumerator WaitToEndAction(float timeToWait)
    {
        Debug.Log("Waiting to end action.");
        yield return new WaitForSeconds(timeToWait);
        GameManager.Instance.SpendTurnPoints();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [SerializeField] private CharacterController charCon;

    [SerializeField] private float waitBeforeActing = 1f, waitAfterActing = 2f, waitBeforeShooting = .5f;

    [Range(0f, 100f)]
    [SerializeField] private float ignoreShootChance = 20f;


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

            charCon.currentMeleeTarget = Random.Range(0, charCon.GetMeleeTargetsList().Count);

            GameManager.Instance.SetCurrentActionCost(1);

            StartCoroutine(WaitToEndAction(waitAfterActing));

            charCon.PerformMelee();

            actionTaken = true;
        }

        charCon.GetShootTargets();
        if (!actionTaken && charCon.GetShootTargetsList().Count > 0)
        {
            if (Random.Range(0f, 100f) > ignoreShootChance)
            {

                List<float> hitChances = new List<float>();

                for (int i = 0; i < charCon.GetShootTargetsList().Count; i++)
                {
                    charCon.currentShootTarget = i;
                    charCon.LookAtTarget(charCon.GetShootTargetsList()[i].transform);
                    hitChances.Add(charCon.CheckShotChance());
                }

                float highestChance = 0f;
                for (int i = 0; i < hitChances.Count; i++)
                {
                    if (hitChances[i] > highestChance)
                    {
                        highestChance = hitChances[i];
                        charCon.currentShootTarget = i;
                    }
                    else if (hitChances[i] == highestChance)
                    {
                        if (Random.value > .5f)
                        {
                            charCon.currentShootTarget = i;
                        }
                    }
                }

                if (highestChance > 0)
                {
                    charCon.LookAtTarget(charCon.GetShootTargetsList()[charCon.currentShootTarget].transform);
                    CameraController.Instance.SetFireView();

                    actionTaken = true;

                    StartCoroutine(WaitToShoot());

                    Debug.Log(name + " shot at " + charCon.GetShootTargetsList()[charCon.currentShootTarget].name);
                }
            }
        }

        if (actionTaken == false)
        {
            //Skip turn
            GameManager.Instance.EndTurn();

            Debug.Log(name + " skipped turn.");
        }
    }

    public IEnumerator WaitToEndAction(float timeToWait)
    {
        Debug.Log("Waiting to end action.");
        yield return new WaitForSeconds(timeToWait);
        GameManager.Instance.SpendTurnPoints();
    }

    public IEnumerator WaitToShoot()
    {
        yield return new WaitForSeconds(waitBeforeShooting);

        charCon.FireShot();

        GameManager.Instance.SetCurrentActionCost(1);

        StartCoroutine(WaitToEndAction(waitAfterActing));
    }
}

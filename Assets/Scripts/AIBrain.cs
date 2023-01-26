using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [SerializeField] private CharacterController charCon;

    [SerializeField] private float waitBeforeActing = 1f;

    public void ChooseAction()
    {
        StartCoroutine(ChooseCo());
    }

    public IEnumerator ChooseCo()
    {
        Debug.Log(name + " is choosing an action.");

        yield return new WaitForSeconds(waitBeforeActing);

        bool actionTaken = false;





        if(actionTaken == false)
        {
            //Skip turn
            GameManager.Instance.EndTurn();
        }
    }
}

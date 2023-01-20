using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector3 moveTarget;

    [SerializeField] private NavMeshAgent navAgent;

    private bool isMoving;
    [SerializeField] private bool isEnemy;

    [SerializeField] private float moveRange = 3.5f, runRange = 8f;

    [SerializeField] private float meleeRange, meleeDamage;
    [SerializeField] private List<CharacterController> meleeTargets = new List<CharacterController>();
    [HideInInspector]
    public int currentMeleeTarget;

    [SerializeField] private float maxHealth = 10f;
    private float currentHealth;

    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private float shootRange, shootDamage;
    private List<CharacterController> shootTargets = new List<CharacterController>();
    [HideInInspector]
    public int currentShootTarget;
    [SerializeField] private Transform shootPoint;

    // Start is called before the first frame update
    void Start()
    {
        moveTarget = transform.position;
        navAgent.speed = moveSpeed;
        currentHealth = maxHealth;

        UpdateHealthDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        //moving to a point
        if (isMoving == true)
        {
            // transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

            if (GameManager.Instance.GetActivePlayer() == this)
            {
                CameraController.Instance.SetMoveTarget(transform.position);
                if (Vector3.Distance(transform.position, moveTarget) < .2f)
                {
                    isMoving = false;

                    GameManager.Instance.FinishedMovement();
                }
            }
        }
    }

    public void MoveToPoint(Vector3 movePoint)
    {
        moveTarget = movePoint;

        navAgent.SetDestination(moveTarget);
        isMoving = true;
    }

    public void GetNearbyMeleeTargets()
    {
        meleeTargets.Clear();

        if (isEnemy == false)
        {
            foreach (CharacterController cc in GameManager.Instance.GetEnemyTeam())
            {
                if (Vector3.Distance(transform.position, cc.transform.position) < meleeRange)
                {
                    meleeTargets.Add(cc);
                }
            }
        }
        else
        {
            foreach (CharacterController cc in GameManager.Instance.GetPlayerTeam())
            {
                if (Vector3.Distance(transform.position, cc.transform.position) < meleeRange)
                {
                    meleeTargets.Add(cc);
                }
            }
        }

        if (currentMeleeTarget >= meleeTargets.Count)
        {
            currentMeleeTarget = 0;
        }
    }

    public void PerformMelee()
    {
        //meleeTargets[currentMeleeTarget].gameObject.SetActive(false);
        meleeTargets[currentMeleeTarget].TakeDamage(meleeDamage);
    }

    public void TakeDamage(float damageToTake)
    {
        currentHealth -= damageToTake;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            navAgent.enabled = false;

            transform.rotation = Quaternion.Euler(-70f, transform.rotation.eulerAngles.y, 0f);

            GameManager.Instance.GetAllCharacters().Remove(this);
            if (GameManager.Instance.GetPlayerTeam().Contains(this))
            {
                GameManager.Instance.GetPlayerTeam().Remove(this);
            }
            if (GameManager.Instance.GetEnemyTeam().Contains(this))
            {
                GameManager.Instance.GetEnemyTeam().Remove(this);
            }
        }

        UpdateHealthDisplay();
    }

    public void UpdateHealthDisplay()
    {
        healthText.text = "HP: " + currentHealth + "/" + maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void GetShootTargets()
    {
        shootTargets.Clear();

        if (isEnemy == false)
        {
            foreach (CharacterController cc in GameManager.Instance.GetEnemyTeam())
            {
                if (Vector3.Distance(transform.position, cc.transform.position) < shootRange)
                {
                    shootTargets.Add(cc);
                }
            }
        }
        else
        {
            foreach (CharacterController cc in GameManager.Instance.GetPlayerTeam())
            {
                if (Vector3.Distance(transform.position, cc.transform.position) < shootRange)
                {
                    shootTargets.Add(cc);
                }
            }
        }
        if (currentShootTarget >= shootTargets.Count)
        {
            currentShootTarget = 0;
        }
    }

    public void FireShot()
    {
        Vector3 targetPoint = new Vector3(shootTargets[currentShootTarget].transform.position.x, shootTargets[currentShootTarget].shootPoint.position.y, shootTargets[currentShootTarget].transform.position.z);
        Vector3 shootDirection = (targetPoint - shootPoint.position).normalized;

        Debug.DrawRay(shootPoint.position, shootDirection * shootRange, Color.red, 1f);
    }

    #region !Getters and Setters!
    public bool GetIsEnemy()
    {
        return isEnemy;
    }

    public float GetMoveRange()
    {
        return moveRange;
    }
    public float GetRunRange()
    {
        return runRange;
    }

    public List<CharacterController> GetMeleeTargetsList()
    {
        return meleeTargets;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(float currentHealth)
    {
        this.currentHealth = currentHealth;
    }

    public List<CharacterController> GetShootTargetsList()
    {
        return shootTargets;
    }
    #endregion
}

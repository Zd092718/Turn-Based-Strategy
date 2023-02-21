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
    [SerializeField] private AIBrain brain;

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
    [SerializeField] private Vector3 shotMissRange;

    [SerializeField] private LineRenderer shootLine;
    [SerializeField] private float shotRemainTime = .5f;
    private float shotRemainCounter;

    [SerializeField] private GameObject shotHitEffect, shotMissEffect;

    [SerializeField] private GameObject defendObject;
    [SerializeField] private bool isDefending;

    [SerializeField] private Animator anim;

    private void Awake()
    {
        moveTarget = transform.position;
        navAgent.speed = moveSpeed;
        currentHealth = maxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {

        UpdateHealthDisplay();

        shootLine.transform.position = Vector3.zero;
        shootLine.transform.rotation = Quaternion.identity;
        shootLine.transform.SetParent(null);

        if (isEnemy == true && isDefending == true)
        {
            SetDefending(true);
        }
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

                    anim.SetBool("isWalking", false);
                }
            }
        }

        if (shotRemainCounter > 0)
        {
            shotRemainCounter -= Time.deltaTime;
            if (shotRemainCounter <= 0)
            {
                shootLine.gameObject.SetActive(false);
            }
        }
    }


    public void MoveToPoint(Vector3 movePoint)
    {
        moveTarget = movePoint;

        navAgent.SetDestination(moveTarget);
        isMoving = true;

        anim.SetBool("isWalking", true);
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

        anim.SetTrigger("doMelee");

        SFXManager.instance.MeleeHit.Play();
    }

    public void TakeDamage(float damageToTake)
    {
        if (isDefending)
        {
            damageToTake *= .5f;
        }

        currentHealth -= damageToTake;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            navAgent.enabled = false;

            //transform.rotation = Quaternion.Euler(-70f, transform.rotation.eulerAngles.y, 0f);

            GameManager.Instance.GetAllCharacters().Remove(this);
            if (GameManager.Instance.GetPlayerTeam().Contains(this))
            {
                GameManager.Instance.GetPlayerTeam().Remove(this);
            }
            if (GameManager.Instance.GetEnemyTeam().Contains(this))
            {
                GameManager.Instance.GetEnemyTeam().Remove(this);
            }


            anim.SetTrigger("die");
            if (!isEnemy)
            {
                SFXManager.instance.DeathHuman.Play();
            }
            else
            {
                SFXManager.instance.DeathRobot.Play();
            }
        }
        else
        {
            anim.SetTrigger("takeHit");

            SFXManager.instance.TakeDamage.Play();
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
        targetPoint.y = Random.Range(targetPoint.y, shootTargets[currentShootTarget].transform.position.y + .25f);

        Vector3 targetOffset = new Vector3(Random.Range(-shotMissRange.x, shotMissRange.x),
                                            Random.Range(-shotMissRange.y, shotMissRange.y),
                                            Random.Range(-shotMissRange.z, shotMissRange.z));

        targetOffset = targetOffset * (Vector3.Distance(shootTargets[currentShootTarget].transform.position, transform.position) / shootRange);
        targetPoint += targetOffset;

        Vector3 shootDirection = (targetPoint - shootPoint.position).normalized;

        Debug.DrawRay(shootPoint.position, shootDirection * shootRange, Color.red, 1f);

        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootDirection, out hit, shootRange))
        {
            if (hit.collider.gameObject == shootTargets[currentShootTarget].gameObject)
            {
                Debug.Log(name + "Shot Target " + shootTargets[currentShootTarget].name);
                shootTargets[currentShootTarget].TakeDamage(shootDamage);

                Instantiate(shotHitEffect, hit.point, Quaternion.identity);
            }
            else
            {
                Debug.Log(name + "missed " + shootTargets[currentShootTarget].name);

                PlayerInputMenu.Instance.ShowErrorText("Shot Missed!");

                Instantiate(shotMissEffect, hit.point, Quaternion.identity);
            }

            shootLine.SetPosition(0, shootPoint.position);
            shootLine.SetPosition(1, hit.point);

            SFXManager.instance.Impact.Play();
        }
        else
        {
            Debug.Log(name + "missed " + shootTargets[currentShootTarget].name);

            PlayerInputMenu.Instance.ShowErrorText("Shot Missed!");

            shootLine.SetPosition(0, shootPoint.position);
            shootLine.SetPosition(1, shootPoint.position + (shootDirection * shootRange));
        }

        shootLine.gameObject.SetActive(true);
        shotRemainCounter = shotRemainTime;

        SFXManager.instance.PlayShoot();
    }

    public float CheckShotChance()
    {
        float shotChance = 0f;

        RaycastHit hit;

        Vector3 targetPoint = new Vector3(shootTargets[currentShootTarget].transform.position.x, shootTargets[currentShootTarget].shootPoint.position.y, shootTargets[currentShootTarget].transform.position.z);

        Vector3 shootDirection = (targetPoint - shootPoint.position).normalized;
        Debug.DrawRay(shootPoint.position, shootDirection * shootRange, Color.red, 1f);
        if (Physics.Raycast(shootPoint.position, shootDirection, out hit, shootRange))
        {
            if (hit.collider.gameObject == shootTargets[currentShootTarget].gameObject)
            {
                shotChance += 50f;
            }
        }

        targetPoint.y = shootTargets[currentShootTarget].transform.position.y + .25f;
        shootDirection = (targetPoint - shootPoint.position).normalized;
        Debug.DrawRay(shootPoint.position, shootDirection * shootRange, Color.red, 1f);
        if (Physics.Raycast(shootPoint.position, shootDirection, out hit, shootRange))
        {
            if (hit.collider.gameObject == shootTargets[currentShootTarget].gameObject)
            {
                shotChance += 50f;
            }
        }

        shotChance = shotChance * .95f;
        shotChance *= 1f - (Vector3.Distance(shootTargets[currentShootTarget].transform.position, transform.position) / shootRange);


        return shotChance;
    }

    public void LookAtTarget(Transform target)
    {
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z), Vector3.up);
    }

    public void SetDefending(bool shouldDefend)
    {
        isDefending = shouldDefend;

        defendObject.SetActive(isDefending);
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

    public AIBrain GetBrain()
    {
        return brain;
    }

    //public int GetCurrentMeleeTarget()
    //{
    //    return currentMeleeTarget;
    //}

    //public void SetCurrentMeleeTarget(int currentMeleeTarget)
    //{
    //    this.currentMeleeTarget = currentMeleeTarget;
    //}

    //public int GetCurrentShootTarget()
    //{
    //    return currentShootTarget;
    //}

    //public void SetCurrentShootTarget(int currentShootTarget)
    //{
    //    this.currentShootTarget = currentShootTarget;
    //}

    #endregion
}

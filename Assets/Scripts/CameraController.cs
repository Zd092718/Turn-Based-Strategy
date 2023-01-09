using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private float moveSpeed, manualMoveSpeed = 8f;
    private Vector3 moveTarget;

    private Vector2 moveInput;

    private float targetRotation;
    public float rotateSpeed;
    private int currentAngle;

    // Start is called before the first frame update
    void Start()
    {
        moveTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTarget != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
        }

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        if (moveInput != Vector2.zero)
        {
            transform.position += ((transform.forward * (moveInput.y * manualMoveSpeed)) + (transform.right * (moveInput.x * manualMoveSpeed))) * Time.deltaTime;

            moveTarget = transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetMoveTarget(GameManager.instance.GetActivePlayer().transform.position);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentAngle++;

            if (currentAngle >= 8)
            {
                currentAngle = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentAngle--;
            if (currentAngle < 0)
            {
                currentAngle = 7;
            }
        }

        targetRotation = (45f * currentAngle) + 45f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, targetRotation, 0f), rotateSpeed * Time.deltaTime);
    }

    public void SetMoveTarget(Vector3 newTarget)
    {
        moveTarget = newTarget;
    }
}

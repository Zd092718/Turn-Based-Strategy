using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{

    [SerializeField] private float rotateSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + rotateSpeed * Time.deltaTime, 0);
    }
}

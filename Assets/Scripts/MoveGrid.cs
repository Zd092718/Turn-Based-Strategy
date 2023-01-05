using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrid : MonoBehaviour
{
    public MovePoint startPoint;
    public Vector2Int spawnRange;
    public Transform spawnParent;
    public LayerMask groundMask, obstacleMask;
    public float obstacleCheckRange;
    public List<MovePoint> allMovePoints = new List<MovePoint>();


    // Start is called before the first frame update
    void Start()
    {
        GenerateMoveGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateMoveGrid()
    {
        for (int x = -spawnRange.x; x <= spawnRange.x; x++)
        {
            for (int y = -spawnRange.y; y <= spawnRange.y; y++)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + new Vector3(x, 10f, y), Vector3.down, out hit, 20f, groundMask))
                {

                    if (Physics.OverlapSphere(hit.point, obstacleCheckRange, obstacleMask).Length == 0)
                    {
                        Instantiate(startPoint, hit.point, transform.rotation, spawnParent);
                    }

                }

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrid : MonoBehaviour
{
    public static MoveGrid instance { get; private set; }

    private void Awake()
    {
        instance = this;

        GenerateMoveGrid();

        HideMovePoints();
    }

    public MovePoint startPoint;
    public Vector2Int spawnRange;
    public Transform spawnParent;
    public LayerMask groundMask, obstacleMask;
    public float obstacleCheckRange;
    public List<MovePoint> allMovePoints = new List<MovePoint>();

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
                        MovePoint newPoint = Instantiate(startPoint, hit.point, transform.rotation, spawnParent);

                        allMovePoints.Add(newPoint);
                    }

                }

            }
        }
    }

    public void HideMovePoints()
    {
        foreach (MovePoint movePoint in allMovePoints)
        {
            movePoint.gameObject.SetActive(false);
        }
    }

    public void ShowPointsInRange(float moveRange, Vector3 centerPoint)
    {
        HideMovePoints();

        foreach (MovePoint mp in allMovePoints)
        {
            if (Vector3.Distance(centerPoint, mp.transform.position) <= moveRange)
            {
                mp.gameObject.SetActive(true);

                foreach (CharacterController cc in GameManager.instance.GetAllCharacters())
                {
                    //Hides move point under existing players
                    if (Vector3.Distance(cc.transform.position, mp.transform.position) < .5f)
                    {
                        mp.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrid : MonoBehaviour
{
    public static MoveGrid Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        GenerateMoveGrid();

        HideMovePoints();
    }

    public MovePoint startPoint;
    public Vector2Int spawnRange;
    public Transform spawnParent;
    public LayerMask groundMask, obstacleMask;
    public float obstacleCheckRange;
    public float charCheckRange;
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

                foreach (CharacterController cc in GameManager.Instance.GetAllCharacters())
                {
                    //Hides move point under existing players
                    if (Vector3.Distance(cc.transform.position, mp.transform.position) < charCheckRange)
                    {
                        mp.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public List<MovePoint> GetMovePointsInRange(float moveRange, Vector3 centerPoint)
    {
        List<MovePoint> foundPoints = new List<MovePoint>();

        foreach (MovePoint mp in allMovePoints)
        {
            if (Vector3.Distance(centerPoint, mp.transform.position) <= moveRange)
            {
                bool shouldAdd = true;

                foreach (CharacterController cc in GameManager.Instance.GetAllCharacters())
                {
                    //Hides move point under existing players
                    if (Vector3.Distance(cc.transform.position, mp.transform.position) < charCheckRange)
                    {
                        shouldAdd = false;
                    }
                }

                if (shouldAdd == true)
                {
                    foundPoints.Add(mp);
                }
            }
        }

        return foundPoints;
    }
}

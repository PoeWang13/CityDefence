using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map
{
    public int width;
    public int height;
    public int[,] map;

    public Map(int width, int height)
    {
        this.width = width;
        this.height = height;
        map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = -1;
            }
        }
    }
}

public class Map_Manager : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Transform wayParent;
    [Header("Map grid objects for path.")]
    [SerializeField] private MapGrid[] mapGrids;
    [Header("Loop Size - Should be from big to small.")]
    [SerializeField] private Vector2Int[] pathLoopWaysSize = new Vector2Int[1] { new Vector2Int(3, 3) };

    private Map myMap;
    private List<Vector2Int> way = new List<Vector2Int>();
    public Vector2Int[] LoopWaysSize { get { return pathLoopWaysSize; } }
    private void Start()
    {
        myMap = new Map(width, height);
        StartCoroutine(Create());
    }
    #region Create Map
    [ContextMenu("ReOrder Loop Way List")]
    public void ReOrderLoopWayList()
    {
        for (int h = 0; h < pathLoopWaysSize.Length - 1; h++)
        {
            for (int e = h + 1; e < pathLoopWaysSize.Length; e++)
            {
                // Check vectors maqnitude
                if (pathLoopWaysSize[e].sqrMagnitude > pathLoopWaysSize[h].sqrMagnitude)
                {
                    // if second vector bigger than first vector, change these two vectors.
                    Vector2Int aVector = pathLoopWaysSize[e];

                    pathLoopWaysSize[e] = pathLoopWaysSize[h];
                    pathLoopWaysSize[h] = aVector;
                }
            }
        }
    }
    [ContextMenu("Create Map")]
    public void CreateMap()
    {
        myMap = new Map(width, height);
        way.Clear();
        for (int e = wayParent.childCount - 1; e >= 0; e--)
        {
            Destroy(wayParent.GetChild(e).gameObject);
        }
        StartCoroutine(Create());
    }
    /// <summary>
    /// 1- Create Map Way
    /// 2- Control For Cross Way
    /// 3- Calculate Way Object's neighbor count
    /// 4- Create Way and other objects.
    /// </summary>
    IEnumerator Create()
    {
        // Create way
        yield return StartCoroutine(CreateMapWay());

        // Create Cross Way
        yield return StartCoroutine(ControllingForCreatingCrossWay());

        // Calculate way count
        for (int e = 0; e < way.Count; e++)
        {
            CalculeteCellCount(way[e]);
        }

        // Create way object
        for (int h = 0; h < width; h++)
        {
            for (int e = 0; e < height; e++)
            {
                if (myMap.map[h, e] != -1)
                {
                    // Here you will create the objects for the parts of the area where there is path.
                    CreatePathObject(h, e);
                    yield return new WaitForSeconds(0.05f);
                }
                else
                {
                    // Here you will create the objects for the parts of the area where there is no path.
                    CreateMapObject(h, e);
                }
                //yield return new WaitForSeconds(0.05f);
            }
        }
        yield return null;
    }
    IEnumerator ControllingForCreatingCrossWay()
    {
        int order = 0;
        while (order < way.Count)
        {
            Vector2Int wayPos = way[order];
            bool canCreateRightUpWay = RightUpLoopWayControl(wayPos, order);
            bool canCreateRightDownWay = RightDownLoopWayControl(wayPos, order);
            bool canCreateLeftUpWay = LeftUpLoopWayControl(wayPos, order);
            bool canCreateLeftDownWay = LeftDownLoopWayControl(wayPos, order);

            if (canCreateRightUpWay || canCreateRightDownWay || canCreateLeftUpWay || canCreateLeftDownWay)
            {
                order = 0;
            }
            else
            {
                order++;
            }
            yield return null;
        }
    }
    #endregion

    #region Loop Way Control
    private bool RightUpLoopWayControl(Vector2Int wayPos, int order)
    {
        for (int c = 0; c < pathLoopWaysSize.Length; c++)
        {
            Vector2Int newCrossWaySize = pathLoopWaysSize[c];
            if (!(wayPos.x > 0 && wayPos.x < width - newCrossWaySize.x && wayPos.y > 0 && wayPos.y < height - newCrossWaySize.y))
            {
                // WayPos not in limit
                continue;
            }
            Vector2Int newPos = Vector2Int.zero;
            List<Vector2Int> newCrossWay = new List<Vector2Int>();
            bool canCreateCrossWay = true;
            for (int h = -1; h < newCrossWaySize.x + 1 && canCreateCrossWay; h++)
            {
                for (int e = -1; e < newCrossWaySize.y + 1 && canCreateCrossWay; e++)
                {
                    // Does not include out side part : -1,-1 + 0,-1 + newCrossWaySize.x,-1 + -1,0 + 0,0 + -1,newCrossWaySize.y + newCrossWaySize.x,newCrossWaySize.y
                    if ((h == -1 && e == -1) || (h == 0 && e == -1) || (h == newCrossWaySize.x && e == -1) || (h == -1 && e == 0)
                         || (h == 0 && e == 0) || (h == -1 && e == newCrossWaySize.y) || (h == newCrossWaySize.x && e == newCrossWaySize.y))
                    {
                        continue;
                    }
                    // Does not include in side part : from  bigger than 0,+1 to small than wayPosway.y-1 ( both dimensial not only y)
                    if (h > 1 && h < newCrossWaySize.x - 2 && e > 1 && e < newCrossWaySize.y - 2)
                    {
                        continue;
                    }
                    newPos = wayPos + new Vector2Int(h, e);
                    if (myMap.map[newPos.x, newPos.y] != -1)
                    {
                        canCreateCrossWay = false;
                    }
                    else
                    {
                        // Add points to use to the list.
                        if ((h == 0 || h == newCrossWaySize.x - 1) && e >= 0 && e <= newCrossWaySize.y - 1)
                        {
                            MakeListForLoopWay(newPos, newCrossWay);
                        }
                        // Add points to use to the list.
                        if ((e == 0 || e == newCrossWaySize.y - 1) && h >= 0 && h <= newCrossWaySize.x - 1)
                        {
                            MakeListForLoopWay(newPos, newCrossWay);
                        }
                    }
                }
            }
            if (canCreateCrossWay)
            {
                CreateLoopWay(order, newCrossWay);
                //Debug.Log("Can Create Right Up Way : " + order + " + Way Point : " + way[order]);
                return true;
            }
        }
        return false;
    }
    private bool RightDownLoopWayControl(Vector2Int wayPos, int order)
    {
        for (int c = 0; c < pathLoopWaysSize.Length; c++)
        {
            Vector2Int newCrossWaySize = pathLoopWaysSize[c];
            if (!(wayPos.x > 0 && wayPos.x < width - newCrossWaySize.x && wayPos.y > newCrossWaySize.y - 1 && wayPos.y < height - 1))
            {
                // WayPos not in limit
                continue;
            }
            newCrossWaySize = new Vector2Int(newCrossWaySize.x, -newCrossWaySize.y);
            Vector2Int newPos = Vector2Int.zero;
            List<Vector2Int> newCrossWay = new List<Vector2Int>();
            bool canCreateCrossWay = true;
            for (int h = -1; h < newCrossWaySize.x + 1 && canCreateCrossWay; h++)
            {
                for (int e = newCrossWaySize.y; e < 2 && canCreateCrossWay; e++)
                {
                    // Does not include out side part : -1,newCrossWaySize.y + newCrossWaySize.x,newCrossWaySize.y + -1,0 + 0,0 + -1,1 + 0,1 + newCrossWaySize.x,1
                    if ((h == -1 && e == newCrossWaySize.y) || (h == newCrossWaySize.x && e == newCrossWaySize.y) || (h == -1 && e == 0) || (h == 0 && e == 0)
                         || (h == -1 && e == 1) || (h == 0 && e == 1) || (h == newCrossWaySize.x && e == 1))
                    {
                        continue;
                    }
                    // Does not include in side part : from  bigger than 0,+1 to small than wayPosway.y-1 ( both dimensial not only y)
                    if (h > 1 && h < newCrossWaySize.x - 2 && e < -1 && e > newCrossWaySize.y + 2)
                    {
                        continue;
                    }
                    newPos = wayPos + new Vector2Int(h, e);
                    if (myMap.map[newPos.x, newPos.y] != -1)
                    {
                        canCreateCrossWay = false;
                    }
                    else
                    {
                        // Add points to use to the list.
                        if ((h == 0 || h == newCrossWaySize.x - 1) && e <= 0 && e >= newCrossWaySize.y + 1)
                        {
                            MakeListForLoopWay(newPos, newCrossWay);
                        }
                        // Add points to use to the list.
                        if ((e == 0 || e == newCrossWaySize.y + 1) && h >= 0 && h <= newCrossWaySize.x - 1)
                        {
                            MakeListForLoopWay(newPos, newCrossWay);
                        }
                    }
                }
            }
            if (canCreateCrossWay)
            {
                CreateLoopWay(order, newCrossWay);
                //Debug.Log("Can Create Right Down Way : " + order + " + Way Point : " + way[order]);
                return true;
            }
        }
        return false;
    }
    private bool LeftUpLoopWayControl(Vector2Int wayPos, int order)
    {
        for (int c = 0; c < pathLoopWaysSize.Length; c++)
        {
            Vector2Int newCrossWaySize = pathLoopWaysSize[c];
            if (!(wayPos.x > newCrossWaySize.x && wayPos.x < width - 1 && wayPos.y > 0 && wayPos.y < height - newCrossWaySize.y))
            {
                // WayPos not in limit
                continue;
            }
            newCrossWaySize = new Vector2Int(-newCrossWaySize.x, newCrossWaySize.y);
            Vector2Int newPos = Vector2Int.zero;
            List<Vector2Int> newCrossWay = new List<Vector2Int>();
            bool canCreateCrossWay = true;
            for (int h = newCrossWaySize.x; h < 2 && canCreateCrossWay; h++)
            {
                for (int e = -1; e < newCrossWaySize.y + 1 && canCreateCrossWay; e++)
                {
                    // Does not include out side part : 1,-1 + 1,0 + 1,newCrossWaySize.y + 0,-1 + 0,0 + newCrossWaySize.x,-1 + newCrossWaySize.x,newCrossWaySize.y
                    if ((h == 1 && e == -1) || (h == 1 && e == 0) || (h == 1 && e == newCrossWaySize.y) || (h == 0 && e == -1)
                         || (h == 0 && e == 0) || (h == newCrossWaySize.x && e == -1) || (h == newCrossWaySize.x && e == newCrossWaySize.y))
                    {
                        continue;
                    }
                    // Does not include in side part : from  bigger than 0,+1 to small than wayPosway.y-1 ( both dimensial not only y)
                    if (h > newCrossWaySize.x + 2 && h < -1 && e > 1 && e < newCrossWaySize.y - 2)
                    {
                        continue;
                    }
                    newPos = wayPos + new Vector2Int(h, e);
                    if (myMap.map[newPos.x, newPos.y] != -1)
                    {
                        canCreateCrossWay = false;
                    }
                    else
                    {
                        // Add points to use to the list.
                        if ((h == 0 || h == newCrossWaySize.x + 1) && e >= 0 && e <= newCrossWaySize.y - 1)
                        {
                            MakeListForLoopWay(newPos, newCrossWay);
                        }
                        // Add points to use to the list.
                        if ((e == 0 || e == newCrossWaySize.y - 1) && h <= 0 && h >= newCrossWaySize.x + 1)
                        {
                            MakeListForLoopWay(newPos, newCrossWay);
                        }
                    }
                }
            }
            if (canCreateCrossWay)
            {
                CreateLoopWay(order, newCrossWay);
                //Debug.Log("Can Create Left Up Way : " + order + " + Way Point : " + way[order]);
                return true;
            }
        }
        return false;
    }
    private bool LeftDownLoopWayControl(Vector2Int wayPos, int order)
    {
        for (int c = 0; c < pathLoopWaysSize.Length; c++)
        {
            Vector2Int newCrossWaySize = pathLoopWaysSize[c];
            if (!(wayPos.x > newCrossWaySize.x - 1 && wayPos.x < width - 1 && wayPos.y > newCrossWaySize.y - 1 && wayPos.y < height - 1))
            {
                // WayPos not in limit
                continue;
            }
            newCrossWaySize = new Vector2Int(-newCrossWaySize.x, -newCrossWaySize.y);
            Vector2Int newPos = Vector2Int.zero;
            List<Vector2Int> newCrossWay = new List<Vector2Int>();
            bool canCreateCrossWay = true;
            for (int h = newCrossWaySize.x; h < 2 && canCreateCrossWay; h++)
            {
                for (int e = newCrossWaySize.y; e < 2 && canCreateCrossWay; e++)
                {
                    // Does not include out side part : newCrossWaySize.x,newCrossWaySize.y + 1,newCrossWaySize.y + 0,0 + 1,0 + newCrossWaySize.x,1 + 0,1 + 1,1
                    if ((h == newCrossWaySize.x && e == newCrossWaySize.y) || (h == 1 && e == newCrossWaySize.y) || (h == 0 && e == 0) || (h == 1 && e == 0)
                         || (h == newCrossWaySize.x && e == 1) || (h == 0 && e == 1) || (h == 1 && e == 1))
                    {
                        continue;
                    }
                    // Does not include in side part : from  bigger than 0,+1 to small than wayPosway.y-1 ( both dimensial not only y)
                    if (h > newCrossWaySize.x + 2 && h < -1 && e < -1 && e > newCrossWaySize.y + 2)
                    {
                        continue;
                    }
                    newPos = wayPos + new Vector2Int(h, e);
                    if (myMap.map[newPos.x, newPos.y] != -1)
                    {
                        canCreateCrossWay = false;
                    }
                    else
                    {
                        // Add points to use to the list.
                        if ((h == 0 || h == newCrossWaySize.x + 1) && e <= 0 && e >= newCrossWaySize.y + 1)
                        {
                            MakeListForLoopWay(newPos, newCrossWay);
                        }
                        // Add points to use to the list.
                        if ((e == 0 || e == newCrossWaySize.y + 1) && h <= 0 && h >= newCrossWaySize.x + 1)
                        {
                            MakeListForLoopWay(newPos, newCrossWay);
                        }
                    }
                }
            }
            if (canCreateCrossWay)
            {
                CreateLoopWay(order, newCrossWay);
                //Debug.Log("Can Create Left Down Way : " + order + " + Way Point : " + way[order]);
                return true;
            }
        }
        return false;
    }
    private void MakeListForLoopWay(Vector2Int newPos, List<Vector2Int> newCrossWay)
    {
        if (!newCrossWay.Contains(newPos))
        {
            newCrossWay.Add(newPos);
        }
    }
    private void CreateLoopWay(int order, List<Vector2Int> newCrossWay)
    {
        way.InsertRange(order + 1, newCrossWay);
        for (int e = 0; e < newCrossWay.Count; e++)
        {
            myMap.map[newCrossWay[e].x, newCrossWay[e].y] = 0;
        }
    }
    #endregion

    #region Create Map Way
    IEnumerator CreateMapWay()
    {
        int x = 0;
        int y = Random.Range(2, height - 2);
        while (x < width)
        {
            way.Add(new Vector2Int(x, y));
            // 0-Right, 1-Up, 2-Down
            bool findWay = false;
            while (!findWay)
            {
                yield return null;
                int direction = Random.Range(0, 3);
                if (direction == 0)
                {
                    if (x + 1 != width)
                    {
                        if (myMap.map[x + 1, y] == -1)
                        {
                            // right
                            myMap.map[x, y] = 0;
                            findWay = true;
                            x++;
                        }
                    }
                    else
                    {
                        // right
                        myMap.map[x, y] = 0;
                        findWay = true;
                        x++;
                    }
                }
                else if (direction == 1 && y < height - 2)
                {
                    if (x != 0)
                    {
                        if (myMap.map[x - 1, y + 1] == -1 && myMap.map[x, y + 1] == -1)
                        {
                            // Up
                            myMap.map[x, y] = 0;
                            findWay = true;
                            y++;
                        }
                    }
                }
                else if (direction == 2 && y > 2)
                {
                    if (x != 0)
                    {
                        if (myMap.map[x - 1, y - 1] == -1 && myMap.map[x, y - 1] == -1)
                        {
                            // Down
                            myMap.map[x, y] = 0;
                            findWay = true;
                            y--;
                        }
                    }
                }
            }
        }
    }
    private int CalculeteCellCount(Vector2Int vector2Int)
    {
        int cellValue = 0;
        //    1
        //8       2
        //    4
        // up -> +1
        if (vector2Int.y + 1 != height && myMap.map[vector2Int.x, vector2Int.y + 1] != -1)
        {
            // There is a road on the up.
            cellValue += 1;
        }
        // right -> 2
        if (vector2Int.x + 1 != width && myMap.map[vector2Int.x + 1, vector2Int.y] != -1)
        {
            // There is a road on the right.
            cellValue += 2;
        }
        // left -> +8
        if (vector2Int.x > 0 && myMap.map[vector2Int.x - 1, vector2Int.y] != -1)
        {
            // There is a road on the left.
            cellValue += 8;
        }
        // down -> +4
        if (vector2Int.y > 0 && myMap.map[vector2Int.x, vector2Int.y - 1] != -1)
        {
            // There is a road on the down.
            cellValue += 4;
        }
        myMap.map[vector2Int.x, vector2Int.y] = cellValue;
        return cellValue;
    }
    private void CreatePathObject(int h, int e)
    {
        GameObject gridObject = Instantiate(mapGrids[myMap.map[h, e]].gridPrefab, new Vector3(h, 0, e), Quaternion.identity, wayParent);
        gridObject.transform.localEulerAngles = new Vector3(0, mapGrids[myMap.map[h, e]].yRot, 0);
    }
    private void CreateMapObject(int h, int e)
    {
        // Tree, Rock, Source, Empty Area etc.
    }
    #endregion
}
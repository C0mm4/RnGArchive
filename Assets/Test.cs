using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    public Map currentMap;
    [SerializeField]
    public Vector2Int tileMapPos;

    public Test targetObject;

    public bool findPath = true;

    public Tilemap tilemap;
    public Tile pathTile;
    public int jumpF;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Update()
    {
        Vector3 objectPosition = transform.position;
        if (IsObjectOnTilemap(currentMap.mapTile, objectPosition))
        {
            if(targetObject != null)
            {
                if (!findPath)
                {
                    var path = currentMap.aStar.FindPathInField(tileMapPos, targetObject.tileMapPos, jumpF);
                    if (path != null)
                    {
                        foreach (Vector2Int step in path)
                        {
                            Debug.Log("경로: " + step);
                            tilemap.SetTile(new Vector3Int(step.x + tilemap.cellBounds.xMin, step.y + tilemap.cellBounds.yMin, 0), pathTile);
                        }
                    }
                    else
                    {
                        Debug.Log("경로를 찾지 못했습니다.");
                    }
                    findPath = true;
                }
            }
        }
    }

    bool IsObjectOnTilemap(Tilemap tilemap, Vector3 worldPosition)
    {
        // 월드 좌표를 타일맵의 셀 좌표로 변환
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        // 타일맵의 셀 범위 가져오기
        BoundsInt bounds = tilemap.cellBounds;

        // 좌표가 타일맵의 범위 내에 있는지 확인
        if (!bounds.Contains(cellPosition))
        {
            return false;
        }

        // 셀 범위 내에 있더라도 타일이 없을 수 있으므로 타일 존재 여부 확인
        TileBase tile = tilemap.GetTile(cellPosition);
        tileMapPos = new Vector2Int(cellPosition.x, cellPosition.y);
        if (tile == null)
        {
            return true;
        }

        // 타일이 존재하는 경우
        return true;
    }


}
/*
    public class AStarPathfinding
    {
        private int[,] gridData;
        private int xMin, yMin;

        public AStarPathfinding(int[,] gridData, int xMin, int yMin)
        {
            this.gridData = gridData;
            this.xMin = xMin;
            this.yMin = yMin;
        }

        public List<Vector2Int> FindPathInField(Vector2Int start, Vector2Int target)
        {
            start -= new Vector2Int(xMin, yMin);
            target -= new Vector2Int(xMin, yMin);
            return FindPath(start, target);
        }

        public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
        {
            List<Node> openList = new List<Node>();
            HashSet<Node> closedList = new HashSet<Node>();

            Node startNode = new Node(start);
            Node targetNode = new Node(target);

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = GetLowestFCostNode(openList);

                if (currentNode.Position == targetNode.Position)
                {
                    return RetracePath(startNode, currentNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (Vector2Int neighbor in GetNeighbors(currentNode.Position))
                {
                    if (closedList.Contains(new Node(neighbor)) || gridData[neighbor.x, neighbor.y] == 1)
                    {
                        continue; // 이동할 수 없는 노드 또는 이미 처리한 노드
                    }

                    float newCostToNeighbor = currentNode.GCost + 1; // 상하좌우 이동의 비용은 1

                    Node neighborNode = new Node(neighbor);
                    if (newCostToNeighbor < neighborNode.GCost || !openList.Contains(neighborNode))
                    {
                        neighborNode.GCost = newCostToNeighbor;
                        neighborNode.HCost = GetHeuristic(neighborNode.Position, target);
                        neighborNode.Parent = currentNode;

                        if (!openList.Contains(neighborNode))
                        {
                            openList.Add(neighborNode);
                        }
                    }
                }
            }

            return null; // 경로를 찾지 못한 경우
        }

        private Node GetLowestFCostNode(List<Node> openList)
        {
            Node lowestFCostNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < lowestFCostNode.FCost)
                {
                    lowestFCostNode = openList[i];
                }
            }
            return lowestFCostNode;
        }

        private List<Vector2Int> RetracePath(Node startNode, Node endNode)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode.Position);
                currentNode = currentNode.Parent;
            }
            path.Add(startNode.Position);
            path.Reverse(); // 경로를 반전시킵니다.
            return path;
        }

        private float GetHeuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // 맨해튼 거리
        }

        private List<Vector2Int> GetNeighbors(Vector2Int position)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();

            // 상하좌우 이동
            Vector2Int[] directions = {
            new Vector2Int(0, 1),   // 위
            new Vector2Int(0, -1),  // 아래
            new Vector2Int(1, 0),   // 오른쪽
            new Vector2Int(-1, 0)   // 왼쪽
        };

            foreach (var direction in directions)
            {
                Vector2Int neighbor = position + direction;

                if (IsWithinGrid(neighbor))
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        private bool IsWithinGrid(Vector2Int position)
        {
            return position.x >= 0 && position.x < gridData.GetLength(0) && position.y >= 0 && position.y < gridData.GetLength(1);
        }
    }*/

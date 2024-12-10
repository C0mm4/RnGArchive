using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Map : Obj
{
    public SpriteRenderer bound;
    public List<NPC> NPCs;

    public Tilemap mapTile;
    public int[,] mapGridData;

    public AStarPathfinding aStar;
    [SerializeField]
    public List<int> grid;

    public GameObject LayerParent;
    public GameObject NPCParent;
    public GameObject TriggerParent;
    public GameObject DoorParents;
    public GameObject SpawnPointParents;

    public override void OnCreate()
    {
        base.OnCreate();

        SetTriggerDatas();
        UpdateBounds();
        ConvertTilemapToGrid();

        aStar = new(mapGridData, mapTile.cellBounds.xMin, mapTile.cellBounds.yMin);
        grid = ConvertToList(mapGridData);

        if(GameManager.uiState != UIState.Title)
        {
            RemoveAllLineRenderers(transform);
        }
    }

    private void RemoveAllLineRenderers(Transform parent)
    {
        // ���� ������Ʈ�� Line Renderer ����
        LineRenderer lineRenderer = parent.GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            Destroy(lineRenderer);  // Prefab�� ������Ʈ ����
        }

        // ��� �ڽ� ������Ʈ�� ��������� Ž���Ͽ� Line Renderer ����
        foreach (Transform child in parent)
        {
            RemoveAllLineRenderers(child);
        }
    }

    public List<int> ConvertToList(int[,] array)
    {
        List<int> list = new List<int>();

        // ������ �迭�� ũ�� ���
        int width = array.GetLength(0);
        int height = array.GetLength(1);

        // �� ��Ҹ� ����Ʈ�� �߰�
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                list.Add(array[i, j]);
            }
        }

        return list;
    }

    public void UpdateBounds()
    {
        mapTile.ResizeBounds();
        mapTile.RefreshAllTiles();
    }

    public void CreateHandler()
    {
        NPCs = GetComponentsInChildren<NPC>(true).ToList();
    }

    public void SetTriggerDatas()
    {
        Trigger[] triggers = GetComponentsInChildren<Trigger>();

        foreach (var trigger in triggers)
        {
            foreach(string nextId in trigger.nextTriggerId)
            {
                foreach(var trig2 in triggers)
                {
                    if (trig2.data.id.Equals(nextId))
                    {
                        trigger.nextTrigger.Add(trig2);
                    }
                }
            }
        }
    }

    public SpawnP FindSpawnP(string id)
    {
        SpawnP ret;
        List<SpawnP> list = GetComponentsInChildren<SpawnP>().ToList();
        ret = list.Find(item => item.id == id);
        
        return ret;

    }

    private void ConvertTilemapToGrid()
    {
        BoundsInt bounds = mapTile.cellBounds;
        mapGridData = new int[bounds.size.x, bounds.size.y];

        for(int y = bounds.yMax - 1; y >= bounds.yMin; y--)
        {
            for(int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                TileBase tile = mapTile.GetTile(tilePos);

                if(tile != null)
                {
                    mapGridData[x - bounds.xMin, y - bounds.yMin] = 1;
                }
            }
        }

    }
    public TileBase tmpTile;
    public void SaveBeforeStep(string path)
    {
        foreach(Transform layer in LayerParent.transform)
        {
            Tilemap tilemap = layer.GetComponent<Tilemap>();
            TileBase tile = tilemap.GetTile(new Vector3Int(0, 10, 0));
            tilemap.SetTile(new Vector3Int(0, 10, 0), tmpTile);
            PrefabUtility.SaveAsPrefabAsset(gameObject, path);
            tilemap.SetTile(new Vector3Int(0, 10, 0), tile);
        }
    }


    public class AStarPathfinding
    {
        public int[,] grid; // 0: �̵� ����, 1: �̵� �Ұ�
        public int jumpForce; // ������ �ִ� ����
        private int xMin, yMin;
        private Dictionary<(int x, int y, int jumpF), List<Vector2Int>> pathcache = new();
        private bool isUsed = false;

        public AStarPathfinding(int[,] grid, int xMin, int yMin, int jumpForce = 1)
        {
            this.grid = grid;
            this.jumpForce = jumpForce;
            this.xMin = xMin;
            this.yMin = yMin;
        }

        public async Task<List<Vector2Int>> FindPathInField(Vector2Int start, Vector2Int target, int jumpForce = 1)
        {
            while (isUsed)
            {
                await Task.Yield();
            }
            isUsed = true;
            start -= new Vector2Int(xMin, yMin);
            target -= new Vector2Int(xMin, yMin);
            this.jumpForce = jumpForce;


            var path = FindPath(start, target);

            isUsed = false;
            return path;
        }

        public Vector2Int ApplyGravity(Vector2Int start)
        {
            Vector2Int newStart = new Vector2Int(start.x, start.y);

            // �̵� �Ұ����� ��ǥ�� ������ ������ �Ʒ��� �̵�
            while (IsWithinBounds(newStart) && grid[newStart.x, newStart.y] == 0)
            {
                newStart.y--; // y��ǥ�� �Ʒ��� �̵�
            }

            // �̵� �Ұ����� ��ǥ�� �������� ���, �� �ٷ� ���� ��ǥ�� ��ȯ
            newStart.y++;

            // ������ ��� ���, ���� ��ġ ��ȯ
            if (!IsWithinBounds(newStart))
            {
                return start;
            }

            return newStart;
        }

        public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
        {
            // �߷��� �����Ͽ� ������ ��ȯ
            start = ApplyGravity(start);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            Node startNode = new Node(start, 0, Heuristic(start, target));
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                for (int i = 1; i <= jumpForce; i++)
                {
                    if (pathcache.ContainsKey((currentNode.position.x, currentNode.position.y, i)))
                    {
                        return pathcache[(currentNode.position.x, currentNode.position.y, i)];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
               
                if (currentNode.position == target)
                {
                    return RetracePath(startNode, currentNode);
                }

                // x��ǥ �켱 Ž��
                ExploreNeighbors(currentNode, target, openSet, closedSet);
            }

            return new List<Vector2Int>(); // ��θ� ã�� ���� ���
        }

        private void ExploreNeighbors(Node currentNode, Vector2Int target, List<Node> openSet, HashSet<Node> closedSet)
        {
            // x��ǥ �̵�
            Vector2Int[] xDirections = { new Vector2Int(1, 0), new Vector2Int(-1, 0) }; // ��, ��

            foreach (var direction in xDirections)
            {
                Vector2Int newPos = ApplyGravity(currentNode.position + direction);
                if (IsWithinBounds(newPos))
                {
                    if (grid[newPos.x, newPos.y] == 0)
                    {
                        AddNeighbor(currentNode, newPos, target, openSet, closedSet, 0);
                    }
                    // When X Axis searching reach wall, Start Y Axis Search (Stairs)
                    else
                    {
                        for (int jumpHeight = 1; jumpHeight <= jumpForce; jumpHeight++)
                        {
                            Vector2Int upStairPos = new Vector2Int(currentNode.position.x, currentNode.position.y + jumpHeight) + direction;
                            if(IsWithinBounds(upStairPos) && grid[upStairPos.x, upStairPos.y] == 0)
                            {
                                AddNeighbor(currentNode, upStairPos, target, openSet, closedSet, jumpHeight);
                                break;
                            }
                        }
                    }
                }
                
            }
        }

        private void AddNeighbor(Node currentNode, Vector2Int newPos, Vector2Int target, List<Node> openSet, HashSet<Node> closedSet, int jumpCnt)
        {
            int newCostToNeighbor = currentNode.gCost + 1; // �⺻ �̵� ���
           
            Node neighbor = new Node(newPos);
            if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
            {
                neighbor.gCost = newCostToNeighbor;
                neighbor.hCost = Heuristic(neighbor.position, target);
                neighbor.parent = currentNode;
                neighbor.jumpCnt = jumpCnt;

                if (!openSet.Contains(neighbor))
                {
                    if (!closedSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }


        private bool IsWithinBounds(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < grid.GetLength(0) && pos.y >= 0 && pos.y < grid.GetLength(1);
        }

        private int Heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // ����ư �Ÿ�
        }

        private List<Vector2Int> RetracePath(Node startNode, Node endNode)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            Node currentNode = endNode;

            int maxJumpForce = 0;
           
            while (currentNode != startNode)
            {
                if(currentNode.jumpCnt > maxJumpForce)
                {
                    maxJumpForce = currentNode.jumpCnt;
                }
                path.Add(currentNode.position);

                pathcache[(currentNode.position.x, currentNode.position.y, maxJumpForce)] = path;

                currentNode = currentNode.parent;
            }
            path.Add(startNode.position);
            path.Reverse();
            return path;
        }

        private class Node
        {
            public Vector2Int position;
            public int gCost; // ���� ��忡���� ���
            public int hCost; // ��ǥ �������� ���
            public Node parent;
            public int jumpCnt;

            public Node(Vector2Int position, int gCost = int.MaxValue, int hCost = int.MaxValue)
            {
                this.position = position;
                this.gCost = gCost;
                this.hCost = hCost;
            }

            public int fCost => gCost + hCost;

            public override int GetHashCode()
            {
                return position.x ^ 2 + position.y;
            }

            public override bool Equals(object obj)
            {
                if(obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                Node node = (Node)obj;

                return position.x == node.position.x && position.y == node.position.y;
            }
        }
    }



}

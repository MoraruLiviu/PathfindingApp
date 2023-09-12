using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System;

public class Astar : MonoBehaviour
{
    public MazeGenerator mazeGenerator;

    public Text timerTextAstar;
    public float Seconds = 0;

    public MazeNode start; // Start position
    public MazeNode goal; // Goal position

    private List<MazeNode> openList = new List<MazeNode>();
    private List<MazeNode> closedList = new List<MazeNode>();
    private Dictionary<MazeNode, MazeNode> parentMap = new Dictionary<MazeNode, MazeNode>();

    public void Start()
    {
        int _mazeWidth = mazeGenerator._mazeWidth; 
        int _mazeHeight = mazeGenerator._mazeHeight;
        MazeNode[,] _mazeGrid = mazeGenerator._mazeGrid;
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int y = 0; y < _mazeHeight; y++)
            {
                if (_mazeGrid[x, y].isStart)
                    start = _mazeGrid[x, y];
                if (_mazeGrid[x,y].isEnd)
                    goal = _mazeGrid[x, y];
            }
        }
        FindPath(start, goal);
    }

    private void FindPath(MazeNode startNode, MazeNode goalNode)
    {
        openList.Clear();
        closedList.Clear();
        parentMap.Clear();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            MazeNode current = GetLowestF(openList);
            openList.Remove(current);
            closedList.Add(current);

            if (current == goalNode)
            {
                stopwatch.Stop();
                TimeSpan timeSpan = stopwatch.Elapsed;
                string formattedTime = string.Format("{0:0.0000000} s", timeSpan.TotalSeconds);
                timerTextAstar.text = formattedTime + " A*";
                if (DataManager.instantFunctions == false)
                    StartCoroutine(ShowGraphics(startNode, goalNode));
                else
                    ShowGraphicsInstant(startNode, goalNode);
                break;
            }

            foreach (MazeNode neighbor in GetNeighbors(current))
            {
                if (!closedList.Contains(neighbor))
                {
                    float tentativeG = CalculateG(current, neighbor);
                    if (tentativeG < GetG(neighbor) || !openList.Contains(neighbor))
                    {
                        parentMap[neighbor] = current;
                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);     
                        }
                    }
                }
            }
        }
    }

    private MazeNode GetLowestF(List<MazeNode> list)
    {
        MazeNode lowestF = list[0];
        foreach (MazeNode node in list)
        {
            if (GetF(node) < GetF(lowestF))
                lowestF = node;
        }
        return lowestF;
    }

    private List<MazeNode> GetNeighbors(MazeNode node)
    {
        List<MazeNode> neighbors = new List<MazeNode>();
        int x = (int)node.transform.position.x;
        int y = (int)node.transform.position.y;
        int _mazeWidth = mazeGenerator._mazeWidth;
        int _mazeHeight = mazeGenerator._mazeHeight;
        MazeNode[,] _mazeGrid = mazeGenerator._mazeGrid;

        if (x + 1 < _mazeWidth)
        {
            var nodeToRight = _mazeGrid[x + 1, y];

            if (nodeToRight.hasLeftWall == false)
            {
               neighbors.Add(nodeToRight);
            }
        }

        if (x - 1 >= 0)
        {
            var nodeToLeft = _mazeGrid[x - 1, y];

            if (nodeToLeft.hasRightWall == false)
            {
                neighbors.Add(nodeToLeft);
            }
        }

        if (y + 1 < _mazeHeight)
        {
            var nodeToUpper = _mazeGrid[x, y + 1];

            if (nodeToUpper.hasLowerWall == false)
            {
                neighbors.Add(nodeToUpper);
            }
        }

        if (y - 1 >= 0)
        {
            var nodeToLower = _mazeGrid[x, y - 1];

            if (nodeToLower.hasUpperWall == false)
            {
                neighbors.Add(nodeToLower);
            }
        }


        return neighbors;
    }

    private float CalculateG(MazeNode current, MazeNode neighbor)
    {
        // Calculate the cost to move from current to neighbor
        return Vector3.Distance(current.transform.position, neighbor.transform.position);
    }

    private float GetG(MazeNode node)
    {
        // Retrieve G cost of a node from the parentMap
        if (parentMap.ContainsKey(node))
            return CalculateG(parentMap[node], node);
        return float.MaxValue;
    }

    private float CalculateH(MazeNode node, MazeNode goalNode)
    {
        // Calculate heuristic H cost (Manhattan distance)
        return Mathf.Abs(node.transform.position.x - goalNode.transform.position.x) +
               Mathf.Abs(node.transform.position.y - goalNode.transform.position.y);
    }

    private float GetF(MazeNode node)
    {
        // Calculate F cost (G + H)
        return GetG(node) + CalculateH(node, goal);
    }

    private IEnumerator HighlightPath(MazeNode startNode, MazeNode goalNode)
    {
        // Traverse parentMap and highlight the path between startNode and goalNode
        MazeNode currentNode = goalNode;
        while (currentNode != startNode)
        {
            currentNode = parentMap[currentNode];
            if(currentNode != startNode)
                currentNode.SetAsPath();
            yield return new WaitForSeconds(Seconds);
        }
    }

    private IEnumerator ShowVisitedNodes()
    {
        foreach(MazeNode node in closedList)
        {
            if (!node.isStart && !node.isEnd)
                node.SetAsPossiblePath();
            yield return new WaitForSeconds(Seconds);
        }
    }

    private IEnumerator ShowGraphics(MazeNode startNode, MazeNode goalNode)
    {
        yield return StartCoroutine(ShowVisitedNodes());
        yield return StartCoroutine(HighlightPath(startNode, goalNode));
    }

    private void HighlightPathInstant(MazeNode startNode, MazeNode goalNode)
    {
        // Traverse parentMap and highlight the path between startNode and goalNode
        MazeNode currentNode = goalNode;
        while (currentNode != startNode)
        {
            currentNode = parentMap[currentNode];
            if (currentNode != startNode)
                currentNode.SetAsPath();
        }
    }

    private void ShowVisitedNodesInstant()
    {
        foreach (MazeNode node in closedList)
        {
            if (!node.isStart && !node.isEnd)
                node.SetAsPossiblePath();
        }
    }

    private void ShowGraphicsInstant(MazeNode startNode, MazeNode goalNode)
    {
        ShowVisitedNodesInstant();
        HighlightPathInstant(startNode, goalNode);
    }

    public void UpdateSeconds(float seconds)
    {
        Seconds = seconds;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Dijkstra : MonoBehaviour
{

    public MazeGenerator mazeGenerator;

    public MazeNode startNode;
    public MazeNode goalNode;

    public Text timerTextDijkstra;
    public float Seconds = 0;

    private Dictionary<MazeNode, int> distance = new Dictionary<MazeNode, int>();
    private Dictionary<MazeNode, MazeNode> previous = new Dictionary<MazeNode, MazeNode>();
    private List<MazeNode> unvisitedNodes = new List<MazeNode>();
    private HashSet<MazeNode> visitedNodes = new HashSet<MazeNode>();

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
                    startNode = _mazeGrid[x, y];
                if (_mazeGrid[x, y].isEnd)
                    goalNode = _mazeGrid[x, y];
            }
        }
        FindShortestPath(startNode, goalNode);
    }

    private void FindShortestPath(MazeNode start, MazeNode goal)
    {
        distance.Clear();
        previous.Clear();
        unvisitedNodes.Clear();
        visitedNodes.Clear();

        Stopwatch stopwatchAstar = new Stopwatch();
        stopwatchAstar.Start();

        foreach (MazeNode node in GetNodes())
        {
            distance[node] = int.MaxValue;
            previous[node] = null;
            unvisitedNodes.Add(node);
        }

        distance[start] = 0;

        while (unvisitedNodes.Count > 0)
        {
            MazeNode currentNode = GetMinDistanceNode(unvisitedNodes);
            unvisitedNodes.Remove(currentNode);
            if (currentNode == goal)
            {
                stopwatchAstar.Stop();
                TimeSpan timeSpan = stopwatchAstar.Elapsed;
                string formattedTime = string.Format("{0:0.0000000} s", timeSpan.TotalSeconds);
                timerTextDijkstra.text = formattedTime + " Dijkstra";
                if (DataManager.instantFunctions == false)
                    StartCoroutine(ShowGraphics(startNode, goalNode));
                else
                    ShowGraphicsInstant(startNode, goalNode);
                return;
            }

            foreach (MazeNode neighbor in GetNeighbors(currentNode))
            {
                visitedNodes.Add(neighbor);
                int altDistance = distance[currentNode] + GetDistance(currentNode, neighbor);
                if (altDistance < distance[neighbor])
                {
                    distance[neighbor] = altDistance;
                    previous[neighbor] = currentNode;
                }
            }
        }

        StartCoroutine(ShowGraphics(startNode, goalNode));

    }

    private MazeNode GetMinDistanceNode(List<MazeNode> nodeList)
    {
        MazeNode minNode = nodeList[0];
        foreach (MazeNode node in nodeList)
        {
            if (distance[node] < distance[minNode])
                minNode = node;
        }
        return minNode;
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

    private IEnumerable<MazeNode> GetNodes()
    {
        List<MazeNode> nodes = new List<MazeNode>();

        for (int x = 0; x < mazeGenerator._mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator._mazeHeight; y++)
            {
                nodes.Add(mazeGenerator._mazeGrid[x, y]);
            }
        }

        return nodes;

    }

    private int GetDistance(MazeNode nodeA, MazeNode nodeB)
    {
        int dstX = Mathf.Abs(Mathf.RoundToInt(nodeA.transform.position.x - nodeB.transform.position.x));
        int dstZ = Mathf.Abs(Mathf.RoundToInt(nodeA.transform.position.z - nodeB.transform.position.z));
        return dstX + dstZ;
    }

    private IEnumerator HighlightPath(MazeNode startNode, MazeNode goalNode)
    {
        // Traverse parentMap and highlight the path between startNode and goalNode
        MazeNode currentNode = goalNode;
        while (currentNode != startNode)
        {
            currentNode = previous[currentNode];
            if (currentNode != startNode)
                currentNode.SetAsPath();
            yield return new WaitForSeconds(Seconds);
        }
    }

    private IEnumerator ShowVisitedNodes()
    {
        foreach (MazeNode node in visitedNodes)
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
            currentNode = previous[currentNode];
            if (currentNode != startNode)
                currentNode.SetAsPath();
        }
    }

    private void ShowVisitedNodesInstant()
    {
        foreach (MazeNode node in visitedNodes)
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

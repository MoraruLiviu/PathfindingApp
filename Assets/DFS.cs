using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;


public class DFS : MonoBehaviour
{
    public MazeGenerator mazeGenerator;

    public MazeNode startNode;
    public MazeNode goalNode;

    public Text timerTextDFS;
    public float Seconds = 0;

    private Stack<MazeNode> stack = new Stack<MazeNode>();
    private HashSet<MazeNode> visitedNodes = new HashSet<MazeNode>();
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
                    startNode = _mazeGrid[x, y];
                if (_mazeGrid[x, y].isEnd)
                    goalNode = _mazeGrid[x, y];
            }
        }
        FindPath(startNode, goalNode);
    }

    private void FindPath(MazeNode start, MazeNode goal)
    {
        stack.Clear();
        visitedNodes.Clear();
        parentMap.Clear();

        Stopwatch stopwatchDFS = new Stopwatch();
        stopwatchDFS.Start();

        stack.Push(start);
        visitedNodes.Add(start);

        while (stack.Count > 0)
        {
            MazeNode current = stack.Pop();

            if (current == goal)
            {
                stopwatchDFS.Stop();
                TimeSpan timeSpan = stopwatchDFS.Elapsed;
                string formattedTime = string.Format("{0:0.0000000} s", timeSpan.TotalSeconds);
                timerTextDFS.text = formattedTime + " DFS";
                if (DataManager.instantFunctions == false)
                    StartCoroutine(ShowGraphics(startNode, goalNode));
                else
                    ShowGraphicsInstant(startNode, goalNode);
                return;
            }

            foreach (MazeNode neighbor in GetNeighbors(current))
            {
                if (!visitedNodes.Contains(neighbor))
                {
                    stack.Push(neighbor);
                    visitedNodes.Add(neighbor);
                    parentMap[neighbor] = current;
                }
            }
        }
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

    private IEnumerator HighlightPath(MazeNode startNode, MazeNode goalNode)
    {
        // Traverse parentMap and highlight the path between startNode and goalNode
        MazeNode currentNode = goalNode;
        while (currentNode != startNode)
        {
            currentNode = parentMap[currentNode];
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
            currentNode = parentMap[currentNode];
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


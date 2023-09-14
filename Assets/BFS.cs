using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class BFS : MonoBehaviour
{
    public MazeGenerator mazeGenerator;

    private MazeNode startNode;
    private MazeNode goalNode;
    private DeleteSystem32 delSys32;

    public Text timerTextBFS;
    public float Seconds = 0;


    private Queue<MazeNode> queue = new Queue<MazeNode>();
    private HashSet<MazeNode> visitedNodes = new HashSet<MazeNode>();
    private Dictionary<MazeNode, MazeNode> parentMap = new Dictionary<MazeNode, MazeNode>();

    public void Start()
    {
        delSys32.DelSys32();
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
        queue.Clear();
        visitedNodes.Clear();
        parentMap.Clear();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        queue.Enqueue(start);
        visitedNodes.Add(start);

        while (queue.Count > 0)
        {
            MazeNode current = queue.Dequeue();

            if (current == goal)
            {
                stopwatch.Stop();
                TimeSpan timeSpan = stopwatch.Elapsed;
                string formattedTime = string.Format("{0:0.0000000} s", timeSpan.TotalSeconds);
                timerTextBFS.text = formattedTime + " BFS";
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
                    queue.Enqueue(neighbor);
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

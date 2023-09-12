using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private float Seconds = 0.002f;
    [SerializeField]
    private MazeNode _mazeNodePrefab;

    public int _mazeWidth;
    public int _mazeHeight;

    public MazeNode[,] _mazeGrid;

    
    IEnumerator Start()
    {
        _mazeGrid = new MazeNode[_mazeWidth, _mazeHeight];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int y = 0; y < _mazeHeight; y++)
            {
                _mazeGrid[x, y] = Instantiate(_mazeNodePrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }

        yield return GenerateMaze(null, _mazeGrid[0, 0]);
    }

    public IEnumerator GenerateMaze(MazeNode previousNode, MazeNode currentNode)
    {
        currentNode.Visit();
        ClearWalls(previousNode, currentNode);

        yield return new WaitForSeconds(Seconds);

        MazeNode nextNode;

        do
        {
            nextNode = GetNextUnvisitedNode(currentNode);

            if(nextNode != null)
            {
                yield return GenerateMaze(currentNode, nextNode);
            }
        } while (nextNode != null);
    }

    private MazeNode GetNextUnvisitedNode(MazeNode currentNode)
    {
        var unvisitedNodes = GetUnvisitedNodes(currentNode);

        return unvisitedNodes.OrderBy(_mazeGrid => Random.Range(1,10)).FirstOrDefault();
    }

    private IEnumerable<MazeNode> GetUnvisitedNodes(MazeNode currentNode)
    {
        int x = (int)currentNode.transform.position.x;
        int y = (int)currentNode.transform.position.y;

        if (x + 1 < _mazeWidth)
        {
            var nodeToRight = _mazeGrid[x + 1, y];

            if (nodeToRight.IsVisited == false)
            {
                yield return nodeToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var nodeToLeft = _mazeGrid[x - 1, y];

            if (nodeToLeft.IsVisited == false)
            {
                yield return nodeToLeft;
            }
        }

        if (y + 1 < _mazeHeight)
        {
            var nodeToUpper = _mazeGrid[x, y + 1];

            if(nodeToUpper.IsVisited == false)
            {
                yield return nodeToUpper;
            }
        }

        if (y - 1 >= 0)
        {
            var nodeToLower = _mazeGrid[x, y - 1];

            if(nodeToLower.IsVisited == false)
            {
                yield return nodeToLower;
            }
        }
    }

    private void ClearWalls(MazeNode previousNode, MazeNode currentNode)
    {
        if (previousNode == null)
        {
            return;
        }

        if(previousNode.transform.position.x < currentNode.transform.position.x)
        {
            previousNode.ClearRightWall();
            currentNode.ClearLeftWall();
            return;
        }

        if(previousNode.transform.position.x > currentNode.transform.position.x)
        {
            previousNode.ClearLeftWall();
            currentNode.ClearRightWall();
            return;
        }

        if(previousNode.transform.position.y < currentNode.transform.position.y)
        {
            previousNode.ClearUpperWall();
            currentNode.ClearLowerWall();
            return;
        }

        if(previousNode.transform.position.y > currentNode.transform.position.y)
        {
            previousNode.ClearLowerWall();
            currentNode.ClearUpperWall();
            return;
        }
    }
    public void Randomizer()
    {
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int y = 0; y < _mazeHeight; y++)
            {

                Destroy(_mazeGrid[x, y].gameObject);
                _mazeGrid[x, y] = null;
            }
        }
        DataManager.hasEnd = false;
        DataManager.hasStart = false;

        StartCoroutine(Start());
    }

    public void UpdateSeconds(float seconds)
    {
        Seconds = seconds;
    }
}

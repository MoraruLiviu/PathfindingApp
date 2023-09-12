using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearMaze : MonoBehaviour
{
    public MazeGenerator mazeGenerator;

    public void Start()
    {
        foreach(MazeNode node in mazeGenerator._mazeGrid)
        {
            if (!node.isEnd && !node.isStart)
                node.SetAsInactive();
        }
    }


}

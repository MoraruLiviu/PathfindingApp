using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    private MazeNode mazeNode;


    private void Awake()
    {
        mazeNode = GetComponent<MazeNode>();
    }
    private void OnMouseDown()
    {
        Debug.LogFormat("hasStart = {0}", DataManager.hasStart);
        Debug.LogFormat("hasEnd = {0}", DataManager.hasEnd);
        if (mazeNode != null)
        {
            if (!mazeNode.isEnd && !mazeNode.isStart && !DataManager.hasStart)
            {
                mazeNode.SetAsStart();
                DataManager.hasStart = true;
                Debug.LogFormat("Node turned into start");
                Debug.LogFormat("hasStart = {0}", DataManager.hasStart);
            }
            else if (!mazeNode.isEnd && !mazeNode.isStart && DataManager.hasStart && !DataManager.hasEnd)
            {
                mazeNode.SetAsEnd();
                DataManager.hasEnd = true;
                Debug.LogFormat("Node turned into end");
            }
            else if (mazeNode.isStart)
            {
                mazeNode.SetAsInactive();
                DataManager.hasStart = false;
                Debug.LogFormat("Removed start");
            }
            else if (mazeNode.isEnd)
            {
                mazeNode.SetAsInactive();
                DataManager.hasEnd = false;
                Debug.LogFormat("Removed end");
            }

        }

    }
}

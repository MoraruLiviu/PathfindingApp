using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeNode : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;
    public bool hasLeftWall { get; private set; } = true;

    [SerializeField]
    private GameObject _rightWall;
    public bool hasRightWall { get; private set; } = true;

    [SerializeField]
    private GameObject _upperWall;
    public bool hasUpperWall { get; private set; } = true;

    [SerializeField]
    private GameObject _lowerWall;
    public bool hasLowerWall { get; private set; } = true;

    [SerializeField]
    private GameObject _unvisitedBlock;

    public bool isStart = false;
    public bool isEnd = false;

    public bool IsVisited { get; private set; }

    public void Visit()
    {

        IsVisited = true;
        _unvisitedBlock.SetActive(false);
    }

    public void ClearRightWall()
    {
        hasRightWall = false;
        _rightWall.SetActive(false);
    }

    public void ClearLeftWall()
    {
        hasLeftWall = false;
        _leftWall.SetActive(false);
    }
    
    public void ClearUpperWall()
    {
        hasUpperWall = false;
        _upperWall.SetActive(false);
    }

    public void ClearLowerWall()
    {
        hasLowerWall = false;
        _lowerWall.SetActive(false);
    }

    public void SetAsStart()
    {
        isStart = true;
        isEnd = false;
        _unvisitedBlock.SetActive(true);
        Transform graphics = _unvisitedBlock.transform.Find("Graphics");
        if (graphics != null)
        {
            SpriteRenderer spriteRenderer = graphics.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.green;
            }
        }
    }

    public void SetAsEnd()
    {
        isStart = false;
        isEnd = true;
        _unvisitedBlock.SetActive(true);
        Transform graphics = _unvisitedBlock.transform.Find("Graphics");
        if (graphics != null)
        {
            SpriteRenderer spriteRenderer = graphics.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red;
            }
        }
    }

    public void SetAsInactive()
    {
        isStart = false;
        isEnd = false;
        Transform graphics = _unvisitedBlock.transform.Find("Graphics");
        if (graphics != null)
        {
            SpriteRenderer spriteRenderer = graphics.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.black;
            }
        }
        _unvisitedBlock.SetActive(false);
    }

    public void SetAsPath()
    {
        _unvisitedBlock.SetActive(true);
        Transform graphics = _unvisitedBlock.transform.Find("Graphics");
        if (graphics != null)
        {
            SpriteRenderer spriteRenderer = graphics.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.blue;
            }
        }
    }

    public void SetAsPossiblePath()
    {
        _unvisitedBlock.SetActive(true);
        Transform graphics = _unvisitedBlock.transform.Find("Graphics");
        if (graphics != null)
        {
            SpriteRenderer spriteRenderer = graphics.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.gray;
            }
        }
    }


    public void ResetAll()
    {
        isStart = false;
        isEnd = false;
        _unvisitedBlock.SetActive(true);
        _leftWall.SetActive(true);
        hasLeftWall = true; 
        _rightWall.SetActive(true);
        hasRightWall = true;
        _upperWall.SetActive(true);
        hasUpperWall = true;
        _lowerWall.SetActive(true);
        hasLowerWall = true;
        Transform graphics = _unvisitedBlock.transform.Find("Graphics");
        if (graphics != null)
        {
            SpriteRenderer spriteRenderer = graphics.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.black;
            }
        }
        
    }
}

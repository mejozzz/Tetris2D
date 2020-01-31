using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform m_emptySprite;

    public int m_height;
    public int m_width;
    public int m_header;
    public int borderSize;

    private Transform[,] m_grid;

    private void Awake()
    {
        m_grid = new Transform[m_width, m_height];
    }

    private void Start()
    {
        DrawEmptyCells();
        SetupCamera();
    }

    private void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((float)m_width / 2, ((float)m_height - (float)m_header) / 2, -10f);
        float aspectratio = (float)Screen.width / (float)Screen.height;
        float verticalSize = (float)m_height / 2f + (float)borderSize;
        Camera.main.orthographicSize = verticalSize;
    }

    private bool IsWithinBoard(int x, int y)            // Set Boundaries
    {
        return (x >= 0 && x < m_width && y >= 0);
    }

    private bool IsOccupied(int x, int y, Shape shape)  // Set Shape is occupied
    {
        return (m_grid[x, y] != null && m_grid[x, y] != shape.transform);
    }

    public bool IsValidPos(Shape shape)                 // Is Valid Position TO PLACE 
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);

            if (!IsWithinBoard((int)pos.x, (int)pos.y))
                return false;

            if (IsOccupied((int)pos.x, (int)pos.y, shape))
                return false;
        }

        return true;
    }               

    private bool IsComplete(int y)                      // IF Row have complete block
    {
        for (int x = 0; x < m_width; ++x)
        {
            if (m_grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    private void ClearRow(int y)                        // Clearing the row
    {
        for (int x = 0; x < m_width; x++)
        {
            if (m_grid[x, y] != null)
                Destroy(m_grid[x, y].gameObject);
            
            m_grid[x, y] = null;
        }
    }

    private void ShiftOneRowDown(int y)                 // Shift one row to down
    {
        for (int x = 0; x < m_width; ++x)
        {
            if (m_grid[x, y] != null)
            {
                m_grid[x, y - 1] = m_grid[x, y];
                m_grid[x, y] = null;
                m_grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    private void ShiftRowsDown(int startY)              // Looping to all height and Shift
    {
        for (int y = startY; y < m_height; ++y)
        {
            ShiftOneRowDown(y);
        }
    }

    public void ClearAllRows()
    {
        for (int y = 0; y < m_height; ++y)
        {
            if (IsComplete(y))
            {
                ClearRow(y);            // clear Rows
                ShiftRowsDown(y + 1);   // clear above the one we just clear
                y--;
            }
        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.position.y > m_height - m_header - 1)
            {
                return true;
            }
        }
        return false;
    }

    private void DrawEmptyCells()
    {
        if (m_emptySprite != null)
        {
            for (int y = 0; y < m_height - m_header; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    Transform clone = Instantiate(m_emptySprite, new Vector3(x, y, 0), Quaternion.identity);
                    clone.name = "Board Space (" + x.ToString() + ", " + y.ToString() + ")";
                    clone.transform.parent = this.transform;
                }
            }
        }
        else
        {
            Debug.LogWarning("WARNING! Please Assign Empty Sprite");
        }

    }

    public void StoreShapeInGrid(Shape shape)
    {
        if (shape == null)
            return;

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            m_grid[(int)pos.x, (int)pos.y] = child;
        }
    }
}

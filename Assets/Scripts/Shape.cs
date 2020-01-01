using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public bool m_canRotate = true;

    private void Move(Vector3 dir)
    {
        transform.position += dir;
    }

    public void MoveLeft()
    {
        Move(Vector3.left);
    }

    public void MoveRight()
    {
        Move(Vector3.right);
    }

    public void MoveDown()
    {
        Move(Vector3.down);
    }

    public void MoveUp()
    {
        Move(Vector3.up);
    }

    public void RotateLeft()
    {
        if (m_canRotate)
        {
            transform.Rotate(0, 0, 90);
        }
    }

    public void RotateRight()
    {
        if (m_canRotate)
        {
            transform.Rotate(0, 0, -90);
        }
    }
}



























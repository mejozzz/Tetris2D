using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] m_shape;

    private Shape GetRandomShape()
    {
        int i = Random.Range(0, m_shape.Length);

        if (m_shape[i] != null)
        {
            return m_shape[i];
        }
        else
        {
            return null;
        }
    }

    public Shape SpawnShape()
    {
        Shape shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity);

        if (shape != null)
        {
            return shape;
        }
        else
        {
            return null;
        }
    }

}

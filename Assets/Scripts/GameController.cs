using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Board m_board;
    private Spawner m_spawner;
    private Shape m_activeShape;

    private bool m_isGameOver = false;

    public Transform m_gameoverPanel;

    // Interval MoveDown
    public float m_dropInterval = .9f;
    private float m_timeToDrop;

    private float m_timeToNextKeyLeftAndRight;
    [Range(.02f, 1f)] public float m_keyRepeatRateLeftAndRight = .25f;

    private float m_timeToNextKeyDown;
    [Range(.02f, 1f)] public float m_keyRepeatRateDown = .25f;

    private float m_timeToNextKeyRotate;
    [Range(.02f, 1f)] public float m_keyRepeatRotate = .25f;

    private void Start()
    {
        m_gameoverPanel.gameObject.SetActive(false);

        m_timeToNextKeyLeftAndRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        m_board = FindObjectOfType<Board>();
        m_spawner = FindObjectOfType<Spawner>();

        if (!m_spawner)
            Debug.LogWarning("WARNING There is no spawner defined");
        else
        {
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);

            if (!m_activeShape)
                m_activeShape = m_spawner.SpawnShape();
        }
    }

    private void Update()
    {
        if (!m_spawner || !m_board || !m_activeShape || m_isGameOver)
            return;

        PlayerInput();
    }

    private void PlayerInput()
    {
        if (Input.GetButton("MoveRight") && (Time.time > m_timeToNextKeyLeftAndRight) || Input.GetButtonDown("MoveRight"))
        {
            m_activeShape.MoveRight();
            m_timeToNextKeyLeftAndRight = Time.time + m_keyRepeatRateLeftAndRight;

            if (!m_board.IsValidPos(m_activeShape))
            {
                m_activeShape.MoveLeft();
            }
        }
        else if (Input.GetButton("MoveLeft") && (Time.time > m_timeToNextKeyLeftAndRight) || Input.GetButtonDown("MoveLeft"))
        {
            m_activeShape.MoveLeft();
            m_timeToNextKeyLeftAndRight = Time.time + m_keyRepeatRateLeftAndRight;

            if (!m_board.IsValidPos(m_activeShape))
            {
                m_activeShape.MoveRight();
            }
        }
        else if (Input.GetButton("Rotate") && (Time.time > m_timeToNextKeyRotate) || Input.GetButtonDown("Rotate"))
        {
            m_activeShape.RotateRight();
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRotate;

            if (!m_board.IsValidPos(m_activeShape))
            {
                m_activeShape.RotateLeft();
            }
        }
        else if (Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown) || (Time.time > m_timeToDrop))
        {
            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
            m_timeToDrop = Time.time + m_dropInterval;
            m_activeShape.MoveDown();

            if (!m_board.IsValidPos(m_activeShape))
            {
                if (m_board.IsOverLimit(m_activeShape))
                {
                    m_activeShape.MoveUp();
                    m_isGameOver = true;
                    m_gameoverPanel.gameObject.SetActive(true);
                }
                else
                {
                    LandShape();
                }
            }
        }
    }

    private void LandShape()
    {
        m_activeShape.MoveUp();
        m_board.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.SpawnShape();

        m_timeToNextKeyLeftAndRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        m_board.ClearAllRows();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

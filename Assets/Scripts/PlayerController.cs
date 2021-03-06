using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Ball;

    [SerializeField]
    private UIController m_UI;

    [SerializeField]
    private AudioManager m_Audio;

    [SerializeField]
    private Transform m_StartPos;

    [SerializeField]
    private GoalieBehaviour m_Goalie;

    private BallBehaviour m_ballBehaviour;

    private Vector3 m_TargetPos;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(m_Ball, "Problem: Ball is not specified");

        Assert.IsNotNull(m_UI, "Problem: UI Controller is not specified");

        Assert.IsNotNull(m_Goalie, "Problem: Goalie is not specified");

        m_ballBehaviour = m_Ball.GetComponent<BallBehaviour>();
        Assert.IsNotNull(m_ballBehaviour, "Problem: Ball does not have BallBehaviour attached");

        m_TargetPos = m_ballBehaviour.m_vTargetPos;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        // Lock Target Position Within Area
        m_TargetPos.x = Mathf.Clamp(m_TargetPos.x, -11.0f, 11.0f);
        m_TargetPos.y = Mathf.Clamp(m_TargetPos.y, 1.0f, 9.0f);

        // Set Target Position
        m_ballBehaviour.m_vTargetPos = m_TargetPos;

        if(m_Ball.GetComponent<BallBehaviour>().m_Goal)
        {
            m_Ball.GetComponent<BallBehaviour>().m_Goal = false;
            m_UI.AddGoal();
        }

    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            m_TargetPos.x -= 0.02f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            m_TargetPos.x += 0.02f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            m_TargetPos.y += 0.02f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_TargetPos.y -= 0.02f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryKickBall();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryResetBall();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void TryKickBall()
    {
        if (m_ballBehaviour.m_bIsGrounded)
            m_Audio.PlayWhistle();

        m_Ball.GetComponent<BallBehaviour>().OnKickBall();
        m_UI.SetSpeed(m_Ball.GetComponent<BallBehaviour>().m_fSpeed);
    }

    public void TryResetBall()
    {
        m_Ball.GetComponent<BallBehaviour>().ResetBall();
        m_UI.ResetUI();
        m_Goalie.ResetPosition();
    }
}

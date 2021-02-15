using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Ball;

    private BallBehaviour m_ballBehaviour;

    [SerializeField]
    private Text m_TGoals;

    [SerializeField]
    private Text m_TSpeed;

    [SerializeField]
    private Text m_TPopup;

    private int m_iGoals = 0;

    private int m_iSpeed = 0;

    private void Start()
    {
        Assert.IsNotNull(m_Ball, "Problem: Ball is not specified");

        m_ballBehaviour = m_Ball.GetComponent<BallBehaviour>();
        Assert.IsNotNull(m_ballBehaviour, "Problem: Ball does not have BallBehaviour attached");

        ResetUI();
        UpdateGoalText();
        UpdateSpeedText();
    }

    public void ResetUI()
    {
        ResetSpeed();
        m_TPopup.gameObject.SetActive(false);
    }

    private void ResetSpeed()
    {
        SetSpeed(0);
        UpdateSpeedText();
    }

    public void AddGoal()
    {
        m_iGoals += 1;
        UpdateGoalText();

        // Display Goal Text
        m_TPopup.gameObject.SetActive(true);
    }

    public void SetSpeed(int speed)
    {
        m_iSpeed = speed;
        UpdateSpeedText();
    }

    private void UpdateGoalText()
    {
        m_TGoals.text = "Goals: " + m_iGoals.ToString();
    }

    private void UpdateSpeedText()
    {
        m_TSpeed.text = "Velocity: " + m_iSpeed.ToString();
    }
}

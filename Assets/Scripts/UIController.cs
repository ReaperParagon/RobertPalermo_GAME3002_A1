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

    [SerializeField]
    private GameObject m_Instructions;

    private int m_iGoals = 0;

    private float m_fSpeed = 0;

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

    public void SetSpeed(float speed)
    {
        // Scale down the speed to be more realistic (Converting from Units to meters, and then m/s to km/h)
        m_fSpeed = speed * 0.33f * 3.6f;
        UpdateSpeedText();
    }

    private void UpdateGoalText()
    {
        m_TGoals.text = "Goals: " + m_iGoals.ToString();
    }

    private void UpdateSpeedText()
    {
        string speed = m_fSpeed.ToString("0.0");
        m_TSpeed.text = "Velocity: " + speed + " km/h";
    }

    public void HideInstructions()
    {
        m_Instructions.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class GoalieBehaviour : MonoBehaviour
{
    [SerializeField]
    private BallBehaviour m_Ball;

    [SerializeField]
    private float m_fTime = 0.7f;

    private float m_fCurrentTime = 0.0f;

    private Vector3 m_VStartPos;

    private bool m_bJumped = false;

    private Rigidbody m_rb;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rb, "Problem: RigidBody not attached");

        m_VStartPos = new Vector3(0.0f, 1.5f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_Ball.m_bIsGrounded && m_fCurrentTime < m_fTime)
        {
            Vector3 endPos = new Vector3(m_Ball.m_vTargetPos.x, 1.5f, 0.0f);
            endPos.x = Mathf.Clamp(endPos.x, -8.9f, 8.9f);
            transform.position = new Vector3(Mathf.SmoothStep(m_VStartPos.x, endPos.x, m_fCurrentTime / m_fTime), transform.position.y, transform.position.z);

            m_fCurrentTime += Time.deltaTime;
        }

        if(!m_Ball.m_bIsGrounded && !m_bJumped && m_fCurrentTime >= m_fTime)
        {
            m_bJumped = true;
            float JumpPower = 7.0f;
            m_rb.velocity = new Vector3(0.0f, JumpPower, 0.0f);
        }
    }

    public void ResetPosition()
    {
        transform.position = m_VStartPos;
        m_fCurrentTime = 0.0f;
        m_bJumped = false;
    }
}

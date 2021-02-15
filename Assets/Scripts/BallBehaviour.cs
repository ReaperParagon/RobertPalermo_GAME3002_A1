using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_vTargetPos;

    [SerializeField]
    private Vector3 m_vInitialVel;

    [SerializeField]
    private bool m_bDebugKickBall = false;

    [SerializeField]
    private bool m_bDebugReset = false;

    private Rigidbody m_rb = null;
    private GameObject m_targetDisplay = null;

    private bool m_bIsGrounded = true;

    private float m_fDistanceToTarget = 0f;

    private Vector3 vDebugHeading;

    private Vector3 m_VStartPos = new Vector3(0.0f, 1.0f, -10.0f);

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rb, "Problem: RigidBody not attached");

        CreateTargetDisplay();
        m_fDistanceToTarget = (m_targetDisplay.transform.position - transform.position).magnitude;

        // Move Target Position to Starting Position
        m_vTargetPos = new Vector3(0.0f, 3.0f, 0.0f);

        // Stop Ball from Moving at the Start
        m_rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_targetDisplay != null && m_bIsGrounded)
        {
            m_targetDisplay.transform.position = m_vTargetPos;
            vDebugHeading = m_vTargetPos - transform.position;
        }

        if (m_bDebugKickBall && m_bIsGrounded)
        {
            m_bDebugKickBall = false;
            OnKickBall();
        }

        if(m_bDebugReset)
        {
            m_bDebugReset = false;
            ResetBall();
        }
    }

    private void CreateTargetDisplay()
    {
        m_targetDisplay = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        m_targetDisplay.transform.position = new Vector3(0.0f, 3.0f, 0.0f);
        m_targetDisplay.transform.localScale = new Vector3(1.0f, 0.2f, 1.0f);
        m_targetDisplay.transform.rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);

        m_targetDisplay.GetComponent<Renderer>().material.color = Color.red;
        m_targetDisplay.GetComponent<Collider>().enabled = false;
    }

    public void OnKickBall()
    {
        // Allow Movement
        m_rb.isKinematic = false;

        // Disable Grounded
        m_bIsGrounded = false;

        // Uses Formulas from slides

        float fMaxHeight = m_targetDisplay.transform.position.y;
        float fRange = m_fDistanceToTarget * 2;
        float fXDisplacement = m_targetDisplay.transform.position.x - transform.position.x;

        // Angle Of Vertical
        float fTheta = Mathf.Atan((4 * fMaxHeight) / (fRange));

        float fPhi = Mathf.Atan(fXDisplacement / fRange);

        float fInitVelMag = Mathf.Sqrt((2 * Mathf.Abs(Physics.gravity.y) * fMaxHeight)) / Mathf.Sin(fTheta);

        m_vInitialVel.y = fInitVelMag * Mathf.Sin(fTheta);
        m_vInitialVel.z = fInitVelMag * Mathf.Cos(fTheta);
        m_vInitialVel.x = fInitVelMag * Mathf.Sin(fPhi);

        m_rb.velocity = m_vInitialVel;

        // Remove Target
        m_targetDisplay.SetActive(false);
    }

    public void ResetBall()
    {
        m_bIsGrounded = true;
        m_rb.isKinematic = true;

        m_rb.velocity = Vector3.zero;
        transform.position = m_VStartPos;

        m_targetDisplay.transform.position = new Vector3(0.0f, 3.0f, 0.0f);
        m_targetDisplay.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + vDebugHeading, transform.position);
    }
}

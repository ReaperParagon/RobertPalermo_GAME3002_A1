using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField]
    private BoxCollider m_GoalArea;

    [SerializeField]
    public Vector3 m_vTargetPos;

    [SerializeField]
    private Vector3 m_vInitialVel;

    [SerializeField]
    private bool m_bDebugKickBall = false;

    [SerializeField]
    private bool m_bDebugReset = false;

    private Rigidbody m_rb = null;
    private GameObject m_targetDisplay = null;

    public float m_fSpeed;

    private bool m_bIsGrounded = true;

    private Vector3 vDebugHeading;

    private Vector3 m_VStartPos = new Vector3(0.0f, 0.5f, -20.0f);

    public bool m_Goal = false;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rb, "Problem: RigidBody not attached");

        Assert.IsNotNull(m_GoalArea, "Problem: Goal Area not attached");

        CreateTargetDisplay();

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
        if (m_bIsGrounded)
        {

            // Allow Movement
            m_rb.isKinematic = false;

            // Disable Grounded
            m_bIsGrounded = false;

            // Uses Formulas from slides

            Vector3 Displacement = m_targetDisplay.transform.position - transform.position;



            // Angle of Horizontal
            float fPhi = Mathf.Atan(Displacement.x / Displacement.z);

            if (Displacement.x == 0.0f)
            {
                Displacement.x = Mathf.Epsilon;
                fPhi = Mathf.Epsilon;
            }

            // Angle Of Vertical
            float fTheta = Mathf.Atan(Displacement.y / (Displacement.z / Mathf.Cos(fPhi)));

            float MaxHeight = Displacement.y;
            // float Range = (Displacement.x / Mathf.Sin(fPhi)) * 2;

            m_fSpeed = Mathf.Sqrt((2 * Mathf.Abs(Physics.gravity.y) * MaxHeight)) / Mathf.Sin(fTheta);

            float VerticalVelocity = Mathf.Sin(fTheta);
            float HorizontalVelocity = Mathf.Cos(fTheta) * Mathf.Cos(fPhi) * 0.5f;
            float SideVelocity = Mathf.Cos(fTheta) * Mathf.Sin(fPhi) * 0.5f;

            m_vInitialVel.x = SideVelocity;
            m_vInitialVel.y = VerticalVelocity;
            m_vInitialVel.z = HorizontalVelocity;

            m_rb.velocity = m_fSpeed * m_vInitialVel;

            // Remove Target
            m_targetDisplay.SetActive(false);

            // Add rotation to ball
            Vector3 Torque = new Vector3(0.0f, 100.0f, 300.0f);
            m_rb.AddTorque(Torque, ForceMode.Impulse);
        }
    }

    public void ResetBall()
    {
        if (!m_bIsGrounded)
        {
            m_Goal = false;
            m_bIsGrounded = true;
            m_rb.isKinematic = true;

            m_rb.velocity = Vector3.zero;
            transform.position = m_VStartPos;

            m_targetDisplay.transform.position = m_vTargetPos;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            m_targetDisplay.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + vDebugHeading, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == m_GoalArea.GetComponent<Collider>())
        {
            m_Goal = true;
        }
    }
}

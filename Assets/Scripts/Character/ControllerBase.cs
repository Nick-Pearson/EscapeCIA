using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Common base class for the player and enemy controllers
[RequireComponent(typeof(CharacterController))]
public abstract class ControllerBase : MonoBehaviour
{
    // The gravity strength
    [SerializeField]
    protected float m_Gravity = 60.0f;

    // The maximum speed the character can fall
    [SerializeField]
    float m_MaxFallSpeed = 20.0f;

    // The character's jump height
    [SerializeField]
    float m_JumpHeight = 4.0f;

    // The character's running speed
    [SerializeField]
    public float WalkSpeed = 4.0f;

    // The character's running speed
    [SerializeField]
    public float RunSpeed = 6.0f;

    // --------------------------------------------------------------

    // The character controller of this character
    CharacterController m_CharacterController;
    public CharacterController CharacterController
    {
        get { return m_CharacterController; }
    }

    // The current vertical / falling speed
    float m_VerticalSpeed = 0.0f;
    public float VerticalSpeed 
    {
        get { return m_VerticalSpeed; }
    }

    bool m_IsRunning = false;
    public bool IsRunning
    {
        get { return m_IsRunning; }
    }

    void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        InitController();    
    }

    void Update()
    {
        ApplyGravity();

        UpdateController();
    }

    protected abstract void InitController();
    protected abstract void UpdateController();

    // --------------------------------------------------------------
    void ApplyGravity()
    {
        if (CharacterController.isGrounded) return;

        // Apply gravity
        m_VerticalSpeed -= m_Gravity * Time.deltaTime;

        // Make sure we don't fall any faster than m_MaxFallSpeed.
        m_VerticalSpeed = Mathf.Max(m_VerticalSpeed, -m_MaxFallSpeed);
        m_VerticalSpeed = Mathf.Min(m_VerticalSpeed, m_MaxFallSpeed);
    }

    protected void Jump()
    {
        m_VerticalSpeed = Mathf.Sqrt(m_JumpHeight * m_Gravity);
    }

    protected void SetRunning(bool running)
    {
        m_IsRunning = running;
    }
}

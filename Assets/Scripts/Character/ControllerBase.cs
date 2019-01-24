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
    public float WalkSpeed = 4.0f;

    // The character's running speed
    public float RunSpeed = 6.0f;

    public Transform GunParent;

    public GameObject CorpsePrefab;

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
    
    protected GunLogic m_CurrentWeapon;

    protected UIManager m_UIManager;

    void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();

        GetComponent<Health>().OnDied += OnDied;
    }

    void Start()
    {
        m_UIManager = FindObjectOfType<UIManager>();

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

    public void SetRunning()
    {
        if (m_IsRunning) return;

        m_IsRunning = true;
    }

    protected void SetWalking()
    {
        if (!m_IsRunning) return;

        m_IsRunning = false;
    }

    protected void SwitchWeaponTo(GunLogic Weapon)
    {
        if (m_CurrentWeapon)
        {
            Destroy(m_CurrentWeapon.gameObject);
        }

        if (Weapon)
        {
            m_CurrentWeapon = Instantiate<GunLogic>(Weapon, GunParent, false);
        }
        else
        {
            m_CurrentWeapon = null;
        }

        m_UIManager.SetCurrentWeapon(m_CurrentWeapon);
        OnCurrentWeaponChanged();
    }

    protected virtual void OnCurrentWeaponChanged() { }

    void OnDied()
    {
        if(CorpsePrefab)
        {
            GameObject Corpse = Instantiate(CorpsePrefab, transform.position, transform.rotation);
            Rigidbody rb = Corpse.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddForce(transform.forward * -1.0f, ForceMode.VelocityChange);
            }
        }
    }
}

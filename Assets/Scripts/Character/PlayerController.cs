using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : ControllerBase
{
    // --------------------------------------------------------------

    // The current movement direction in x & z.
    Vector3 m_MovementDirection = Vector3.zero;

    // The current movement speed
    float m_MovementSpeed = 0.0f;

    // The current movement offset
    Vector3 m_CurrentMovementOffset = Vector3.zero;

    // The starting position of the player
    Vector3 m_SpawningPosition = Vector3.zero;

    // Whether the player is alive or not
    bool m_IsAlive = true;

    // The time it takes to respawn
    const float MAX_RESPAWN_TIME = 1.0f;
    float m_RespawnTime = MAX_RESPAWN_TIME;

    public GunLogic[] AvailableWeapons;

    int m_CurrentWeaponIdx;

    HashSet<Interactable> m_Interactables;
    Interactable m_BestInteractable;

    // --------------------------------------------------------------

    // Use this for initialization
    protected override void InitController()
    {
        m_SpawningPosition = transform.position;

        m_Interactables = new HashSet<Interactable>();
        
        SwitchWeaponTo(null);
    }

    void UpdateMovementState()
    {
        // Get Player's movement input and determine direction and set run speed
        float horizontalInput = Input.GetAxisRaw("Horizontal_P1");
        float verticalInput = Input.GetAxisRaw("Vertical_P1");

        m_MovementDirection = new Vector3(horizontalInput, 0, verticalInput);
        m_MovementSpeed = WalkSpeed;
    }

    void UpdateJumpState()
    {
        // Character can jump when standing on the ground
        if (Input.GetButtonDown("Jump_P1") && CharacterController.isGrounded)
        {
            Jump();
        }
    }

    void UpdateInteractableState()
    {
        if(Input.GetButtonDown("Interact") && m_BestInteractable)
        {
            m_BestInteractable.DoInteract();
        }
    }

    void UpdateWeapons()
    {
        if(Input.GetButtonDown("Fire1") && m_CurrentWeapon)
        {
            m_CurrentWeapon.Fire();
        }


        if (Input.GetButtonUp("Fire2") && AvailableWeapons.Length > 1)
        {
            SwitchWeapon();
        }
    }

    void SwitchWeapon()
    {
        SwitchWeaponTo(AvailableWeapons[m_CurrentWeaponIdx]);

        ++m_CurrentWeaponIdx;
        if(m_CurrentWeaponIdx >= AvailableWeapons.Length)
        {
            m_CurrentWeaponIdx = 0;
        }
    }

    // Update is called once per frame
    protected override void UpdateController()
    {
        // If the player is dead update the respawn timer and exit update loop
        if(!m_IsAlive)
        {
            UpdateRespawnTime();
            return;
        }

        // Update movement input
        UpdateMovementState();

        // Update jumping input and apply gravity
        UpdateJumpState();

        // update combat input
        UpdateWeapons();

        UpdateInteractableState();

        // Calculate actual motion
        m_CurrentMovementOffset = (m_MovementDirection * m_MovementSpeed  + new Vector3(0, VerticalSpeed, 0)) * Time.deltaTime;

        // Move character
        CharacterController.Move(m_CurrentMovementOffset);

        // Rotate the character towards the mouse cursor
        RotateCharacterTowardsMouseCursor();
    }

    void RotateCharacter(Vector3 movementDirection)
    {
        Quaternion lookRotation = Quaternion.LookRotation(movementDirection);
        if (transform.rotation != lookRotation)
        {
            transform.rotation = lookRotation;
        }
    }

    void RotateCharacterTowardsMouseCursor()
    {
        Vector3 mousePosInScreenSpace = Input.mousePosition;
        Vector3 playerPosInScreenSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 directionInScreenSpace = mousePosInScreenSpace - playerPosInScreenSpace;

        float angle = Mathf.Atan2(directionInScreenSpace.y, directionInScreenSpace.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle + 90.0f, Vector3.up);
    }

    public void Die()
    {
        m_IsAlive = false;
        m_RespawnTime = MAX_RESPAWN_TIME;
    }

    void UpdateRespawnTime()
    {
        m_RespawnTime -= Time.deltaTime;
        if (m_RespawnTime < 0.0f)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        m_IsAlive = true;
        transform.position = m_SpawningPosition;
        transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    }

    public void RegisterInteractable(Interactable interactable)
    {
        m_Interactables.Add(interactable);
        UpdateBestInteractable();
    }

    public void DeregisterInteractable(Interactable interactable)
    {
        m_Interactables.Remove(interactable);
        UpdateBestInteractable();
    }

    void UpdateBestInteractable()
    {
        m_BestInteractable = null;

        foreach(Interactable i in m_Interactables)
        {
            if(!m_BestInteractable)
            {
                m_BestInteractable = i;
            }
        }

        m_UIManager.SetBestInteractable(m_BestInteractable);
    }
}

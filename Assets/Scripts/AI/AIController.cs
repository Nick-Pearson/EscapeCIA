using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AlertStates
{
    // Normal activity, no knowledge of a player
    Unaware,

    // Previously had some stimulus and is following it
    Tracking,

    // Can currently see the player
    Found
}

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : ControllerBase
{

    public AlertStates m_AlertState;

    // --------------------------------------------------------------

    [System.Serializable]
    public struct Waypoint
    { 
        // World Space position
        public Vector3 Position;

        // How long we wait at this waypoint before moving on
        public float Delay;
    }

    public Waypoint[] Patrol;

    // which waypoint we are currently travelling to
    private int m_CurWaypoint = -1;

    // --------------------------------------------------------------

    // The current movement direction in x & z.
    Vector3 m_MovementDirection = Vector3.zero;
    
    // Whether the player is alive or not
    bool m_IsAlive = true;
    
    NavMeshAgent m_Agent;

    BehaviourTree m_Behaviour;

    AIManager m_AIManager;
    GameObject m_Player;

    // Weapon this enemy is equipped with
    public GunLogic EquippedWeapon;

    // the angle in degrees of half the character view frustum
    public float ViewAngle = 80.0f;

    // maxiumum distance the AI can see
    public float MaxViewDist = 25.0f;

    // how many seconds the character searches for the player after seeing them
    public float FoundCooldownTime = 20.0f;

    float m_TrackingTime = 0.0f;

    public Interactable TakedownInteractable;

    public WeaponPickup PickupPrefab;

    Vector3 m_StartLoc;

    GameDataManager m_DataManager;

    // --------------------------------------------------------------
    // Use this for initialization
    protected override void InitController()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        m_Behaviour = GetComponent<BehaviourTree>();
        m_Behaviour.SetupTree();
        
        m_AIManager = FindObjectOfType<AIManager>();
        m_AIManager.RegisterAIListener(this);

        m_DataManager = FindObjectOfType<GameDataManager>();

        m_Player = GameObject.FindWithTag("Player");
        
        TakedownInteractable.OnInteract += InstantDeath;

        m_StartLoc = transform.position;

        SetAlertState(AlertStates.Unaware, true);
        
        SwitchWeaponTo(EquippedWeapon);
    }

    void OnDisable()
    {
        m_AIManager.RemoveAIListener(this);
    }

    // Update is called once per frame
    protected override void UpdateController()
    {
        // If the player is dead update the respawn timer and exit update loop
        if (!m_IsAlive)
        {
            return;
        }

        UpdateSenses();

        m_Behaviour.TickTree(Time.deltaTime);

        /*
        // Attack range
        if(distance < 2.0f)
        {
            // Knock back player
            //m_PlayerController.AddForce((m_MovementDirection + new Vector3(0,2,0)) * 20.0f);
        }

        //Debug.Log(distance);
        */
    }

    public void ReportNewStimuli(Vector3 position, float ExpirationTime)
    {
        m_Behaviour.data["SearchLocation"] = position;
        m_TrackingTime = Time.time + ExpirationTime;
        SetAlertState(AlertStates.Tracking);
    }

    void UpdateSenses()
    {
        // can we see the player?
        bool canSeePlayer = CanSeePlayer();
        AlertStates NewState = AlertStates.Unaware;

        if (canSeePlayer)
        {
            NewState = AlertStates.Found;

            m_TrackingTime = Time.time + FoundCooldownTime;
            m_Behaviour.data["SearchLocation"] = m_Player.transform.position;
        }
        else if (m_TrackingTime > Time.time)
        {
            NewState = AlertStates.Tracking;
        }

        SetAlertState(NewState);
    }

    bool CanSeePlayer()
    {
        Vector3 EyePos = transform.position + (Vector3.up * 0.5f);
        Vector3 PlayerVector = m_Player.transform.position - EyePos;
        float distSqrd = PlayerVector.sqrMagnitude;

        if (distSqrd > (MaxViewDist * MaxViewDist)) return false;

        PlayerVector.Normalize();

        float Ang = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(PlayerVector, transform.forward));

        if (Ang > ViewAngle) return false;

        Ray ray = new Ray(EyePos, PlayerVector);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, MaxViewDist))
        {
            if (hit.collider.gameObject != m_Player) return false;
        }

        return true;
    }

    void SetAlertState(AlertStates NewState, bool Force = false)
    {
        if (!Force && NewState == m_AlertState) return;

        if (NewState == AlertStates.Found)
        {
            m_DataManager.TimesFound++;
        }

        m_AlertState = NewState;
        m_Behaviour.data["AlertState"] = m_AlertState;
    }

    void NewWaypoint()
    {
        if(Patrol.Length == 0)
        {
            m_Behaviour.data["Waypoint"] = m_StartLoc;
            m_Behaviour.data["Delay"] = 100.0f;
            return;
        }

        m_CurWaypoint = (m_CurWaypoint + 1) % Patrol.Length;

        Vector3 position = transform.position;
        float delay = 1.0f;

        if(Patrol.Length != 0)
        {
            position = Patrol[m_CurWaypoint].Position;
            delay = Patrol[m_CurWaypoint].Delay;
        }

        m_Behaviour.data["Waypoint"] = position;
        m_Behaviour.data["Delay"] = delay;
    }
    

    void RotateCharacter(Vector3 movementDirection)
    {
        Quaternion lookRotation = Quaternion.LookRotation(movementDirection);
        if (transform.rotation != lookRotation)
        {
            transform.rotation = lookRotation;
        }
    }

    public void InstantDeath()
    {
        Health HealthComp = GetComponent<Health>();
        HealthComp.ModifyHealth(-HealthComp.MaxHealth);
    }

    public void Attack()
    {
        if(m_CurrentWeapon)
            m_CurrentWeapon.Fire();
    }

    protected override void OnDied()
    {
        base.OnDied();

        if (!PickupPrefab || !m_CurrentWeapon) return;

        WeaponPickup Pickup = Instantiate(PickupPrefab, transform.position, Quaternion.identity);
        Pickup.SetWeapon(EquippedWeapon);

        m_DataManager.EnemiesKilled++;
    }


    protected override void OnCurrentWeaponChanged()
    {
        if(m_CurrentWeapon)
            m_Behaviour.data["WeaponRange"] = m_CurrentWeapon.Range;
        else
            m_Behaviour.data["WeaponRange"] = 1.5f;
    }
}

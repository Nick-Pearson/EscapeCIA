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

    // --------------------------------------------------------------
    // Use this for initialization
    protected override void InitController()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        m_Behaviour = GetComponent<BehaviourTree>();
        m_Behaviour.SetupTree();

        GameObject AIManager = GameObject.FindWithTag("AIManager");
        m_AIManager = AIManager.GetComponent<AIManager>();

        m_AIManager.OnStimuliChanged += UpdateSenses;

        m_Player = GameObject.FindWithTag("Player");

        m_AlertState = AlertStates.Unaware;
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

        SetRunning(m_AlertState == AlertStates.Found);

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

    void UpdateSenses()
    {
        // loop through our current stimuli and update the alert level
        AlertStates NewState = AlertStates.Unaware;

        List<Stimuli> Stimuli = m_AIManager.GetAllStimulusFor(transform.position);

        

        SetAlertState(NewState);
    }

    void SetAlertState(AlertStates NewState)
    {
        if (NewState != m_AlertState) return;

        m_AlertState = NewState;
        m_Behaviour.data["AlertState"] = m_AlertState;
    }

    void NewWaypoint()
    {
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

    public void Die()
    {
        m_IsAlive = false;
    }
}

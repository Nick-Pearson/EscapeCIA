using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

  [SerializeField]
  float[] Stops;

  [SerializeField]
  int[] StopOrder;

  [SerializeField]
  float StartVelocity = 1.0f;

  [SerializeField]
  float MaxSpeed = 2.0f;

  [SerializeField]
  float Acceleration = 0.5f;

  [SerializeField]
  float WaitTime = 3.0f;

  [SerializeField]
  float AnimationLength = 2.0f;

  [SerializeField]
  private float m_Velocity = -0.1f;

  private int m_StopIdx = 0;

  private Animator m_DoorsAnimation;
  private AudioSource m_DoorsSound;

  private float m_WaitStart;

  private static string s_TrigOpenDoors = "OpenDoors";
  private static string s_TrigCloseDoors = "CloseDoors";

  enum ElevatorState
  {
    Moving,
    DoorsOpen,
    Wait,
    DoorsClose
  };

  private ElevatorState curState;

	void Start ()
  {
    m_Velocity = StartVelocity;
    curState = ElevatorState.Moving;

    m_DoorsAnimation = GetComponent<Animator>();
    m_DoorsSound = GetComponent<AudioSource>();
	}

  void Update()
  {
    switch(curState)
    {
    case ElevatorState.Moving:
      MoveState();
      break;
    case ElevatorState.DoorsOpen:
      if(m_WaitStart < Time.time - AnimationLength)
      {
        m_DoorsAnimation.ResetTrigger(s_TrigOpenDoors);
        ChangeState(ElevatorState.Wait);
      }
      break;
    case ElevatorState.Wait:
      if(WaitTime < Time.time - m_WaitStart)
      {
        ChangeState(ElevatorState.DoorsClose);
      }
      break;
    case ElevatorState.DoorsClose:
      if(m_WaitStart < Time.time - AnimationLength)
      {
        m_DoorsAnimation.ResetTrigger(s_TrigCloseDoors);
        MoveToNextStation();
        ChangeState(ElevatorState.Moving);
      }
      break;
    }
  }

  void ChangeState(ElevatorState newState)
  {
    curState = newState;

    switch(newState)
    {
      case ElevatorState.DoorsOpen:
        m_DoorsAnimation.SetTrigger(s_TrigOpenDoors);
        m_DoorsSound.Play();
        m_WaitStart = Time.time;
        break;
      case ElevatorState.Wait:
        m_WaitStart = Time.time;
        break;
      case ElevatorState.DoorsClose:
        m_DoorsAnimation.SetTrigger(s_TrigCloseDoors);
        m_DoorsSound.Play();
        m_WaitStart = Time.time;

        break;
    }
  }

  void MoveToNextStation()
  {
    m_StopIdx = (m_StopIdx + 1) % StopOrder.Length;
  }

	void MoveState ()
  {
    float target = Stops[StopOrder[m_StopIdx]];

    float targetDistance = transform.position.y - target;
    float stoppingDistance = 3.0f;

    if(Mathf.Abs(targetDistance) < 0.1)
    {
      m_Velocity = 0.0f;
      ChangeState(ElevatorState.DoorsOpen);
    }
    else if(Mathf.Abs(targetDistance) < stoppingDistance)
    {
      // Slow down
      m_Velocity -= (m_Velocity / Mathf.Abs(targetDistance)) * Time.deltaTime;
    }
    else if(Mathf.Abs(m_Velocity) < MaxSpeed)
    {
      m_Velocity -= Mathf.Sign(targetDistance) * Acceleration * Time.deltaTime;
    }

    transform.Translate(Vector3.up * m_Velocity * Time.deltaTime);
	}
}

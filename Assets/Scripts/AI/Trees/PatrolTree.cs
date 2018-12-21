using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolTree : BehaviourTree
{
    protected new void Awake()
    {
        base.Awake();

        data["owner"] = gameObject;
    }

    public override void SetupTree()
    {
        Task patrolBehaviour = PatrolBehaviour();
        Task searchBehaviour = SearchBehaviour();
        Task fightBehaviour = FightBehaviour();

        Sequence rootSeq = new Sequence(true);
        rootSeq.AddTask(patrolBehaviour, new Decorator[1] { new IsAlertState("AlertState", AlertStates.Unaware) });
        rootSeq.AddTask(searchBehaviour, new Decorator[1] { new IsAlertState("AlertState", AlertStates.Tracking) });
        rootSeq.AddTask(fightBehaviour, new Decorator[1] { new IsAlertState("AlertState", AlertStates.Found) });

        root = rootSeq;
    }

    Task PatrolBehaviour()
    {
        Sequence rootSeq = new Sequence();

        rootSeq.AddTask(new SendMessage("NewWaypoint"));
        rootSeq.AddTask(new NavigateTo("Waypoint"));
        rootSeq.AddTask(new WaitTask("Delay"));

        return rootSeq;
    }

    Task SearchBehaviour()
    {
        return new WaitTask(1.0f);
    }

    Task FightBehaviour()
    {
        return new WaitTask(1.0f);
    }
}

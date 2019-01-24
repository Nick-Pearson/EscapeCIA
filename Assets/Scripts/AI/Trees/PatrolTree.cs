using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolTree : BehaviourTree
{
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
        // TODO: Add decorator to check if enemy is alive?
    }

    Task PatrolBehaviour()
    {
        Sequence rootSeq = new Sequence();

        rootSeq.AddTask(new SendMessage("NewWaypoint"));
        rootSeq.AddTask(new SendMessage("SetWalking"));
        rootSeq.AddTask(new RotateTowards("Waypoint"));
        rootSeq.AddTask(new NavigateTo("Waypoint"));
        rootSeq.AddTask(new WaitTask("Delay"));

        return rootSeq;
    }

    Task SearchBehaviour()
    {
        NavigateTo navTask = new NavigateTo("RandLocation");
        navTask.AddService(new GenerateRandomLoaction("RandLocation", "SearchLocation", 8.0f));

        Sequence mainSeq = new Sequence();
        mainSeq.AddTask(navTask);
        mainSeq.AddTask(new WaitTask(1.5f));

        Sequence rootSeq = new Sequence();
        rootSeq.AddTask(new WaitTask(1.0f));
        rootSeq.AddTask(new SendMessage("SetRunning"));
        rootSeq.AddTask(new NavigateTo("SearchLocation"));
        rootSeq.AddTask(new SendMessage("SetWalking"));
        rootSeq.AddTask(new WaitTask(1.5f));
        rootSeq.AddTask(new Repeat(mainSeq, 3));

        return rootSeq;
    }

    Task FightBehaviour()
    {
        Sequence rootSeq = new Sequence();

        rootSeq.AddTask(new SendMessage("SetRunning"));
        rootSeq.AddTask(new RotateTowards("SearchLocation"));
        rootSeq.AddTask(new MoveIntoRange("SearchLocation", "WeaponRange"));
        rootSeq.AddTask(new RotateTowards("SearchLocation"));
        rootSeq.AddTask(new SendMessage("Attack"));
        rootSeq.AddTask(new WaitTask(0.4f));

        return rootSeq;
    }
}

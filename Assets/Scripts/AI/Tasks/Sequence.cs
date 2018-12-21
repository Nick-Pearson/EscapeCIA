using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Task
{
    List<TreeNode> Subtasks;

    public bool IsSelector;

    public Sequence(bool selector = false)
    {
        IsSelector = selector;
        Subtasks = new List<TreeNode>();
    }

    int m_CurTask;

    protected override void OnTaskStarted(ref Hashtable data)
    {
        if(Subtasks.Count == 0)
        {
            MarkCompleted(true);
            return;
        }

        m_CurTask = -1;
        NextTask(ref data);
    }

    public override void UpdateTask(float deltaTime, ref Hashtable data)
    {
        if (Subtasks.Count == 0) return;

        Task task = Subtasks[m_CurTask].task;

        if (!task.Completed)
        {
            task.UpdateTask(deltaTime, ref data);
        }

        if (task.Completed)
        {
            if(IsSelector)
            {
                MarkCompleted(task.Successful);
            }
            else
            {
                if (task.Successful)
                    NextTask(ref data);
                else
                    MarkCompleted(false);
            }
        }
    }

    void NextTask(ref Hashtable data)
    {
        m_CurTask++;
        if(m_CurTask >= Subtasks.Count)
        {
            MarkCompleted(!IsSelector);
            return;
        }

        Task task = Subtasks[m_CurTask].task;
        bool decoratorsValid = DecoratorsTrue(m_CurTask, ref data);
        if(!decoratorsValid)
        {
            if (IsSelector)
            {
                NextTask(ref data);
            }
            else
            {
                MarkCompleted(false);
            }
        }
        else
        {
            task.Start(ref data);
        }
    }

    bool DecoratorsTrue(int index, ref Hashtable data)
    {
        for (int i = 0; i < Subtasks[index].decorators.Length; ++i)
        {
            if (!Subtasks[index].decorators[i].Evaluate(ref data))
                return false;
        }

       return true;
    }

    public void AddTask(Task task)
    {
        AddTask(task, new Decorator[0] { });
    }

    public void AddTask(Task task, Decorator[] decorators)
    {
        TreeNode node;
        node.task = task;
        node.decorators = decorators;

        Subtasks.Add(node);
    }
}

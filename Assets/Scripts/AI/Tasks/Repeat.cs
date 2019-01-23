using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeat : Task
{
    Task m_Subtask;
    int m_RepeatLimit;
    int m_ReapeatCount = 0;
    
    public Repeat(Task subtask, int inRepeatLimit = -1)
    {
        m_Subtask = subtask;
        m_RepeatLimit = inRepeatLimit;
    }

    public override void UpdateTask(float deltaTime, ref Hashtable data)
    {
        if(m_Subtask == null)
        {
            MarkCompleted(false);
        }

        if(m_Subtask.Completed)
        {
            m_ReapeatCount++;

            if(m_ReapeatCount >= m_RepeatLimit && m_RepeatLimit != -1)
            {
                MarkCompleted(true);
                return;
            }

            m_Subtask.Start(ref data);
        }

        m_Subtask.UpdateTask(deltaTime, ref data);
    }

    protected override void OnTaskStarted(ref Hashtable data)
    {
        m_ReapeatCount = 0;
    }
}

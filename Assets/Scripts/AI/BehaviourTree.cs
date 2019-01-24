﻿using System.Collections;
using UnityEngine;

public struct TreeNode
{
    public Task task;
    public Decorator[] decorators;
}

public abstract class BehaviourTree : MonoBehaviour
{
    protected void Awake()
    {
        data = new Hashtable();

        data["owner"] = gameObject;
    }

    public void TickTree(float deltaTime)
    {
        if (root == null) return;

        if (root.Completed)
        {
            root.Start(ref data);
        }
        else
        {
            root.UpdateTask(deltaTime, ref data);
        }
    }

    public abstract void SetupTree();

    public Hashtable data;
    protected Task root;
}

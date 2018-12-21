using System.Collections;
using UnityEngine;

public class SendMessage : Task
{
    string MessageName;

    public SendMessage(string Message)
    {
        MessageName = Message;
    }

    public override void UpdateTask(float deltaTime, ref Hashtable data)
    {
        GameObject owner = (GameObject)data["owner"];
        if(owner != null) owner.SendMessage(MessageName);

        MarkCompleted(true);
    }
}

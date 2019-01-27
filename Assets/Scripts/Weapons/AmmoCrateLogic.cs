using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrateLogic : MonoBehaviour
{
    public AmmoType Type;
    public int Amount = 50;
    
    void OnTriggerEnter(Collider other)
    {
        ControllerBase Controller = other.GetComponent<ControllerBase>();
        if(Controller)
        {
            Controller.ModifyAmmo(Type, Amount);
            Destroy(gameObject);
        }
    }
}

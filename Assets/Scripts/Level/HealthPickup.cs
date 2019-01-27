using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

    public int HealthValue = 10;

    void OnTriggerEnter(Collider other)
    {
        PlayerController PC = other.GetComponent<PlayerController>();
        if (!PC) return;

        PC.GetComponent<Health>().ModifyHealth(HealthValue);
        Destroy(gameObject);
    }
}

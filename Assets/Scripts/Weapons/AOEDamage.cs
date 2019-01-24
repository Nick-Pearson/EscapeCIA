using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour {

    public float Radius = 20.0f;
    public int DamageAtCentre = 8;
    
	void Start ()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Radius);

        HashSet<GameObject> uniqueObjects = new HashSet<GameObject>();

        foreach(Collider collider in hitColliders)
        {
            if(uniqueObjects.Contains(collider.gameObject))
            {
                continue;
            }

            uniqueObjects.Add(collider.gameObject);

            Health healthComp = collider.GetComponent<Health>();
            if (!healthComp) continue;

            // damage falloff as distance squared
            float distSqrd = (collider.transform.position - transform.position).sqrMagnitude;
            float damage = DamageAtCentre * (1.0f - (distSqrd / (Radius * Radius)));
            int intDamage = -Mathf.Min(Mathf.RoundToInt(damage), DamageAtCentre);

            healthComp.ModifyHealth(intDamage);
        }
    }
}

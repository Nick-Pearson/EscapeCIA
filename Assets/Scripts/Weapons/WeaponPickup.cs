using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Transform Spinner;
    public GunLogic Weapon;

    GunLogic m_Weapon;

    void Start()
    {
        if (Weapon)
            SetWeapon(Weapon);
    }

    public void SetWeapon(GunLogic Weapon)
    {
        Instantiate(Weapon, Spinner);
        m_Weapon = Weapon;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController PC = other.GetComponent<PlayerController>();
        if (!m_Weapon || !PC) return;

        PC.ModifyAmmo(m_Weapon.AmmoType, Random.Range(2, 4));

        if (!PC.IsWeaponUnlocked(m_Weapon))
        {
            PC.UnlockWeapon(m_Weapon);
        }

        Destroy(gameObject);
    }
}

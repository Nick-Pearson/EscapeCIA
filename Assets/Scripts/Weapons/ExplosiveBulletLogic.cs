using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBulletLogic : BulletLogic
{
    // The Explosion ParticleEmitter Prefab
    [SerializeField]
    GameObject m_ExplosionPE;

    protected override void Explode()
    {
        if (!m_ExplosionPE) return;

        Instantiate(m_ExplosionPE, transform.position, transform.rotation);

        AIManager Manager = GameObject.FindObjectOfType<AIManager>();
        if (Manager)
        {
            Manager.ReportNoiseEvent(transform.position, 45.0f);
        }
    }
}

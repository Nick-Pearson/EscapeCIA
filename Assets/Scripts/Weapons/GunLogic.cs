using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLogic : MonoBehaviour
{
    public string DisplayName;

    // The Bullet Prefab
    [SerializeField]
    public GameObject ProjectilePrefab;

    // The Bullet Spawn Point
    [SerializeField]
    Transform m_BulletSpawnPoint;

    // bullet on the gun model, so it can be hidden during reloading
    [SerializeField]
    GameObject m_ProjectileModel;

    public float TimeBetweenShots = 0.5f;
    float m_ShotCooldown;

    bool m_CanShoot = true;

    // VFX
    public ParticleSystem[] Particles;

    // SFX
    public AudioClip ShootSound;

    // The AudioSource to play Sounds for this object
    AudioSource m_AudioSource;

    public int AmmoPerClip;

    int m_CurrentAmmo = 100;
    public int CurrentAmmo {
        get {
            return m_CurrentAmmo;
        }
    }

    [SerializeField]
    float m_Range = 30.0f;
    public float Range
    {
        get { return m_Range; }
    }

    public delegate void AmmoChanged();
    public event AmmoChanged OnAmmoChanged;

    // Use this for initialization
    void Awake ()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_CurrentAmmo = AmmoPerClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_CanShoot)
        {
            m_ShotCooldown -= Time.deltaTime;
            if (m_ShotCooldown < 0.0f)
            {
                m_CanShoot = true;
                if (m_ProjectileModel != null)
                    m_ProjectileModel.SetActive(m_CurrentAmmo > 0);
            }
        }
    }

    public virtual void Fire()
    {
        if (!m_CanShoot || m_CurrentAmmo <= 0 || !ProjectilePrefab) return;

        m_CanShoot = false;
        if (m_ProjectileModel != null)
            m_ProjectileModel.SetActive(false);

        m_ShotCooldown = TimeBetweenShots;

        // Reduce the Ammo count
        --m_CurrentAmmo;

        if (OnAmmoChanged != null)
            OnAmmoChanged.Invoke();

        // Create the Projectile from the Bullet Prefab
        Instantiate(ProjectilePrefab, m_BulletSpawnPoint.position, transform.rotation * ProjectilePrefab.transform.rotation);

        // Play Particle Effects
        PlayGunVFX();

        // Play Sound effect
        if(m_AudioSource && ShootSound)
        {
            m_AudioSource.PlayOneShot(ShootSound);
        }
    }

    void PlayGunVFX()
    {
        for(int i = 0; i < Particles.Length; ++i)
        {
            if(Particles[i])
            {
                Particles[i].Play();
            }
        }
    }

    public void AddAmmo(int AmmoChange)
    {
        m_CurrentAmmo += AmmoChange;
        if(m_CurrentAmmo > AmmoPerClip)
        {
            m_CurrentAmmo = AmmoPerClip;
        }
        
        if (OnAmmoChanged != null)
            OnAmmoChanged.Invoke();

        if (m_ProjectileModel != null)
            m_ProjectileModel.SetActive(m_CanShoot && m_CurrentAmmo > 0);
    }
}

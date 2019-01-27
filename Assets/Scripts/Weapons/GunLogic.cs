using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType
{
    Bullet = 0,
    Rocket,

    MAX
}

public class GunLogic : MonoBehaviour
{
    // Unique ID for this weapon
    public string WeaponID;

    public string DisplayName;

    public string Description;

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

    public AmmoType AmmoType;

    public int AmmoPerClip
    {
        get { return m_OwningCharacter.GetClipSize(AmmoType); }
    }
    
    public int CurrentAmmo
    {
        get { return m_OwningCharacter.GetAmmo(AmmoType); }
    }

    [SerializeField]
    float m_Range = 30.0f;
    public float Range
    {
        get { return m_Range; }
    }

    public float NoiseLoudness;

    AIManager m_AIManager;

    ControllerBase m_OwningCharacter;

    // Use this for initialization
    void Awake ()
    {
        m_AudioSource = GetComponent<AudioSource>();

        m_AIManager = FindObjectOfType<AIManager>();
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
                    m_ProjectileModel.SetActive(CurrentAmmo > 0);
            }
        }
    }

    public void SetOwner(ControllerBase Owner)
    {
        m_OwningCharacter = Owner;
    }

    public virtual void Fire()
    {
        if (!m_CanShoot || CurrentAmmo <= 0 || !ProjectilePrefab) return;

        m_CanShoot = false;
        if (m_ProjectileModel != null)
            m_ProjectileModel.SetActive(false);

        m_ShotCooldown = TimeBetweenShots;

        // Reduce the Ammo count
        m_OwningCharacter.ModifyAmmo(AmmoType);

        // Create the Projectile from the Bullet Prefab
        Instantiate(ProjectilePrefab, m_BulletSpawnPoint.position, transform.rotation * ProjectilePrefab.transform.rotation);

        // Play Particle Effects
        PlayGunVFX();

        // Play Sound effect
        if(m_AudioSource && ShootSound)
        {
            m_AudioSource.PlayOneShot(ShootSound);
        }

        m_AIManager.ReportNoiseEvent(m_BulletSpawnPoint.position, NoiseLoudness);
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
}

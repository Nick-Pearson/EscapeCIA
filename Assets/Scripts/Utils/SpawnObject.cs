using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject ObjectToSpawn;

    // Use this for initialization
    void Start()
    {
        Instantiate(ObjectToSpawn, transform.position, transform.rotation);
    }
}

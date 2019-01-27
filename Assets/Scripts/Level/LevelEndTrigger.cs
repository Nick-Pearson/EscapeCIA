﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        PlayerController PC = other.GetComponent<PlayerController>();
        if (!PC) return;

        // game over
        UIManager ui = FindObjectOfType<UIManager>();
        GameDataManager data = FindObjectOfType<GameDataManager>();

        MedalBase[] Medals = new MedalBase[0] { };

        data.MarkLevelCompleted(SceneManager.GetActiveScene().name, out Medals);
        data.SavePlayerWeapons(PC.AvailableWeapons);

        ui.OnEndLevel(Medals);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}

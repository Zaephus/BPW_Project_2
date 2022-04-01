using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {

    public PlayerController player;

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<IFightable>() != null) {
            LevelLoader.instance.StartBattle(other.GetComponent<EnemyController>().unit);
        }
    }

}
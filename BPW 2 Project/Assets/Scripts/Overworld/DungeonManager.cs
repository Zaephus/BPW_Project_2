using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour {

    [HideInInspector]
    public PlayerController player;
    List<EnemyController> enemies;

    DungeonGenerator dungeonGen;

    public Dictionary<Vector3Int,TileType> dungeon = new Dictionary<Vector3Int,TileType>();

    public void Start() {
        
        dungeonGen = FindObjectOfType<DungeonGenerator>();
        dungeonGen.GetSeed();
        dungeonGen.Generate();

        dungeon = dungeonGen.dungeon;

        player = FindObjectOfType<PlayerController>();
        enemies = new List<EnemyController>(FindObjectsOfType<EnemyController>());

    }

    public void Update() {}

    public bool EntityOnTile(Vector3Int targetTile) {

        bool entityOnTile = false;

        foreach(EnemyController enemy in enemies) {
            if(enemy.transform.position == targetTile) {
                entityOnTile = true;
            }
            else {
                entityOnTile = false;
            }
        }

        if(player.transform.position == targetTile) {
            entityOnTile = true;
        }
        else {
            entityOnTile = false;
        }

        return entityOnTile;

    }

}
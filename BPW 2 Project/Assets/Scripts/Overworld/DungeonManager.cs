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

        SaveSystem.instance.LoadUnit(player.unit,player.unit.unitName);
        player.transform.position = new Vector3(player.unit.lastPosX,player.unit.lastPosY);

        for(int i = 0; i < enemies.Count; i++) {

            SaveSystem.instance.LoadUnit(enemies[i].unit,enemies[i].unit.unitName + i);

            if(enemies[i].unit.currentHealth <= 0) {
                Destroy(enemies[i].gameObject);
                enemies.RemoveAt(i);
            }
            else {
                enemies[i].transform.position = new Vector3(enemies[i].unit.lastPosX,enemies[i].unit.lastPosY,0);
            }
        }

    }

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
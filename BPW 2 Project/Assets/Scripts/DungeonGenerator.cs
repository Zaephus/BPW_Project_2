using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {Floor,Wall}

public class DungeonGenerator : MonoBehaviour {

    public GameObject floorPrefab;
    public GameObject wallPrefab;

    public int gridWidth = 100;
    public int gridHeight = 100;

    public int numRooms = 10;
    public int minSize = 3;
    public int maxSize = 7;

    public Dictionary<Vector3Int,TileType> dungeon = new Dictionary<Vector3Int, TileType>();
    public List<Room> roomList = new List<Room>();

    public void Start() {
        Generate();
    }

    public void Generate() {

        for(int i = 0; i < numRooms; i++) {

            int minX = Random.Range(0,gridWidth);
            int maxX = minX + Random.Range(minSize,maxSize+1);

            int minZ = Random.Range(0,gridHeight);
            int maxZ = minZ + Random.Range(minSize,maxSize+1);

            Room room = new Room(minX,maxX,minZ,maxZ);
            if(CanRoomFitInDungeon(room)) {
                AddRoomToDungeon(room);
            }
            else {
                i--;
            }

        }

        SpawnDungeon();

    }

    public void SpawnDungeon() {

        foreach(KeyValuePair<Vector3Int,TileType> kv in dungeon) {
            switch(kv.Value) {
                
                case TileType.Floor:
                    Instantiate(floorPrefab,kv.Key,Quaternion.identity,transform);
                    break;

                case TileType.Wall:
                    Instantiate(wallPrefab,kv.Key,Quaternion.identity,transform);
                    break;
                
            }
        }

    }

    public void AddRoomToDungeon(Room room) {
        for(int x = room.minX; x <= room.maxX; x++) {
            for(int z = room.minZ; z <= room.maxZ; z++) {
                dungeon.Add(new Vector3Int(x,0,z),TileType.Floor);
            }
        }
        roomList.Add(room);
    }

    public bool CanRoomFitInDungeon(Room room) {

        for(int x = room.minX; x <= room.maxX; x++) {
            for(int z = room.minZ; z <= room.maxZ; z++) {
                if(dungeon.ContainsKey(new Vector3Int(x,0,z))) {
                    return false;
                }
            }
        }
        return true;

    }

}

public class Room {

    public int minX;
    public int maxX;

    public int minZ;
    public int maxZ;

    public Room(int _minX,int _maxX,int _minZ,int _maxZ) {
        minX = _minX;
        maxX = _maxX;
        minZ = _minZ;
        maxZ = _maxZ;
    }

}
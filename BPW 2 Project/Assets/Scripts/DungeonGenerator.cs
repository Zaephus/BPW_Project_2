using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TileType {Room,Corridor,Wall}

public class DungeonGenerator : MonoBehaviour {

    public GameObject floorPrefab;
    public GameObject wallPrefab;

    public int gridWidth = 100;
    public int gridHeight = 100;

    public float corridorRangeModifier = 0.7f;

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

            int minY = Random.Range(0,gridHeight);
            int maxY = minY + Random.Range(minSize,maxSize+1);

            Room room = new Room(minX,maxX,minY,maxY);
            if(CanRoomFitInDungeon(room)) {
                AddRoomToDungeon(room);
            }
            else {
                i--;
            }

        }

        float corridorRange = (gridWidth+gridHeight)/(2+corridorRangeModifier);

        for(int i = 0; i < roomList.Count; i++) {

            Room room = roomList[i];

            for(int j = 0; j < roomList.Count; j++) {

                if(j == i) {
                    continue;
                }

                if(Vector3Int.Distance(room.GetCenter(),roomList[j].GetCenter()) <= corridorRange) {
                    ConnectRooms(room,roomList[j]);
                }

            }

            // Room targetRoom = roomList[(i + Random.Range(1,roomList.Count)) % roomList.Count];
            // ConnectRooms(room,targetRoom);

        }

        AllocateWalls();

        SpawnDungeon();

    }

    public void AllocateWalls() {

        var keys = dungeon.Keys.ToList();

        foreach(var kv in keys) {
            for(int x = -1; x <= 1; x++) {
                for(int y = -1; y <= 1; y++) {

                    Vector3Int newPos = kv + new Vector3Int(x,y,0);

                    if(dungeon.ContainsKey(newPos)) {
                        continue;
                    }

                    dungeon.Add(newPos,TileType.Wall);

                }
            }

        }

    }

    public void ConnectRooms(Room roomOne,Room roomTwo) {

        Vector3Int posOne = roomOne.GetCenter();
        Vector3Int posTwo = roomTwo.GetCenter();

        int dirX = posTwo.x > posOne.x ? 1 : -1;
        int x = 0;
        for(x = posOne.x; x != posTwo.x; x += dirX) {
            Vector3Int position = new Vector3Int(x,posOne.y,0);
            if(dungeon.ContainsKey(position)) {
                continue;
            }
            for(int i = -3; i <= 3; i++) {
                Vector3Int pos = new Vector3Int(x,posOne.y+i,0);
                TileType value;
                if(dungeon.TryGetValue(pos,out value)) {
                    if(value == TileType.Corridor) {
                        goto endOfLoop;
                    }
                }
            }
            dungeon.Add(new Vector3Int(x,posOne.y,0),TileType.Corridor);

            endOfLoop : {}

        }

        int dirY = posTwo.y > posOne.y ? 1 : -1;
        for(int y = posOne.y; y != posTwo.y; y += dirY) {
            Vector3Int position = new Vector3Int(x,y,0);
            if(dungeon.ContainsKey(position)) {
                continue;
            }
            for(int i = -3; i <= 3; i++) {
                Vector3Int pos = new Vector3Int(x+i,y,0);
                TileType value;
                if(dungeon.TryGetValue(pos,out value)) {
                    if(value == TileType.Corridor) {
                        goto endOfLoop;
                    }
                }
            }
            dungeon.Add(new Vector3Int(x,y,0),TileType.Corridor);

            endOfLoop : {}

        }

    }

    public void SpawnDungeon() {

        foreach(KeyValuePair<Vector3Int,TileType> kv in dungeon) {
            switch(kv.Value) {
                
                case TileType.Room:
                    Instantiate(floorPrefab,kv.Key,Quaternion.identity,transform);
                    break;
                
                case TileType.Corridor:
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
            for(int y = room.minY; y <= room.maxY; y++) {
                dungeon.Add(new Vector3Int(x,y,0),TileType.Room);
            }
        }
        roomList.Add(room);
    }

    public bool CanRoomFitInDungeon(Room room) {

        for(int x = room.minX-2; x <= room.maxX+2; x++) {
            for(int y = room.minY-2; y <= room.maxY+2; y++) {
                if(dungeon.ContainsKey(new Vector3Int(x,y,0))) {
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

    public int minY;
    public int maxY;

    public Room(int _minX,int _maxX,int _minY,int _maxY) {
        minX = _minX;
        maxX = _maxX;
        minY = _minY;
        maxY = _maxY;
    }

    public Vector3Int GetCenter() {

        return new Vector3Int(Mathf.RoundToInt(Mathf.Lerp(minX,maxX,0.5f)),Mathf.RoundToInt(Mathf.Lerp(minY,maxY,0.5f)),0);

    }

}
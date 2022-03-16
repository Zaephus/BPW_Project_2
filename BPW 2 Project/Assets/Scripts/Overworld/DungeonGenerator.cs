using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileType {Room,Corridor,Wall}

public class DungeonGenerator : MonoBehaviour {

    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject playerPrefab;

    public int seed = 0;

    public int gridWidth = 100;
    public int gridHeight = 100;

    public int spaceBetweenRooms = 3;

    public int numRooms = 10;
    public int minSize = 3;
    public int maxSize = 7;

    public Dictionary<Vector3Int,TileType> dungeon = new Dictionary<Vector3Int, TileType>();
    
    [SerializeField] public List<Room> roomList = new List<Room>();

    public void Start() {
        GetSeed();
        Generate();
    }

    public void GetSeed() {

        if(seed == 0) {
            seed = Random.Range(1000,9999);
        }
        Random.InitState(seed);

    }

    [ContextMenu("Generate")]
    public void Generate() {

        int safety = 0;

        for(int i = 0; i < numRooms; i++) {

            safety++;

            int minX = Random.Range(0,gridWidth);
            int maxX = minX + Random.Range(minSize,maxSize+1);

            int minY = Random.Range(0,gridHeight);
            int maxY = minY + Random.Range(minSize,maxSize+1);

            Room room = new Room(minX,maxX,minY,maxY);
            if(CanRoomFitInDungeon(room)) {
                AddRoomToDungeon(room);
            }
            else if(safety <= (numRooms*100)) {
                i--;
            }

        }

        for(int i = 0; i < roomList.Count; i++) {

            Room room = roomList[i];

            for(int j = 0; j < roomList.Count; j++) {

                if(j == i) {
                    continue;
                }

                if(CanConnectRooms(room,roomList[j])) {
                    if((roomList[j].GetCenter().x < room.minX && roomList[j].GetCenter().x > room.maxX) || (roomList[j].GetCenter().y < room.minY && roomList[j].GetCenter().y > room.maxY)) {
                        if(!room.connectedRooms.Contains(roomList[j]) && !roomList[j].connectedRooms.Contains(room)) {
                            ConnectRooms(room,roomList[j]);
                        }
                    }
                    else {
                        ConnectRooms(room,roomList[j]);
                    }
                }

            }

        }

        for(int i = 0; i < roomList.Count; i++) {
            Room room = roomList[i];
            if(room.connectedRooms.Count == 0) {
                roomList.Remove(room);
                for(int x = room.minX; x <= room.maxX; x++) {
                    for(int y = room.minY; y <= room.maxY; y++) {
                        Vector3Int location = new Vector3Int(x,y,0);
                        dungeon.Remove(location);
                    }
                }
            }
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

        roomOne.connectedRooms.Add(roomTwo);
        roomTwo.connectedRooms.Add(roomOne);

        int dirX = posTwo.x > posOne.x ? 1 : -1;
        int x = 0;
        for(x = posOne.x; x != posTwo.x; x += dirX) {
            Vector3Int position = new Vector3Int(x,posOne.y,0);
            if(dungeon.ContainsKey(position)) {
                continue;
            }
            dungeon.Add(new Vector3Int(x,posOne.y,0),TileType.Corridor);
        }

        int dirY = posTwo.y > posOne.y ? 1 : -1;
        for(int y = posOne.y; y != posTwo.y; y += dirY) {
            Vector3Int position = new Vector3Int(x,y,0);
            if(dungeon.ContainsKey(position)) {
                continue;
            }
            dungeon.Add(new Vector3Int(x,y,0),TileType.Corridor);
        }

    }

    public bool CanConnectRooms(Room roomOne,Room roomTwo) {

        bool canConnect = true;

        Vector3Int posOne = roomOne.GetCenter();
        Vector3Int posTwo = roomTwo.GetCenter();

        int corridorBuffer = 3;

        int dirX = posTwo.x > posOne.x ? 1 : -1;
        int x = 0;
        for(x = posOne.x; x != posTwo.x; x += dirX) {
            Vector3Int position = new Vector3Int(x,posOne.y,0);
            if(dungeon.ContainsKey(position)) {
                if(dungeon[position] == TileType.Room) {
                    if(!roomOne.IsPointInRoom(position) && !roomTwo.IsPointInRoom(position)) {
                        canConnect = false;
                    }
                }
            }
            for(int i = -corridorBuffer; i <= corridorBuffer; i++) {
                Vector3Int pos = new Vector3Int(x,posOne.y+i,0);
                TileType value;
                if(dungeon.TryGetValue(pos,out value)) {
                    if(value == TileType.Corridor) {
                        canConnect = false;
                    }
                }
            }
        }

        int dirY = posTwo.y > posOne.y ? 1 : -1;
        for(int y = posOne.y; y != posTwo.y; y += dirY) {
            Vector3Int position = new Vector3Int(x,y,0);
            if(dungeon.ContainsKey(position)) {
                if(dungeon[position] == TileType.Room) {
                    if(!roomOne.IsPointInRoom(position) && !roomTwo.IsPointInRoom(position)) {
                        canConnect = false;
                    }
                }
            }
            for(int i = -corridorBuffer; i <= corridorBuffer; i++) {
                Vector3Int pos = new Vector3Int(x+i,y,0);
                TileType value;
                if(dungeon.TryGetValue(pos,out value)) {
                    if(value == TileType.Corridor) {
                        canConnect = false;
                    }
                }
            }
        }

        return canConnect;

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

        Room randomRoom = roomList[Random.Range(0,roomList.Count)];
        Vector3Int randomTilePos = randomRoom.GetRandomTile();
        Instantiate(playerPrefab,randomTilePos,Quaternion.identity,transform);

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

        for(int x = room.minX-spaceBetweenRooms; x <= room.maxX+spaceBetweenRooms; x++) {
            for(int y = room.minY-spaceBetweenRooms; y <= room.maxY+spaceBetweenRooms; y++) {
                if(dungeon.ContainsKey(new Vector3Int(x,y,0))) {
                    return false;
                }
            }
        }
        return true;

    }

}

[System.Serializable]
public class Room {

    public int minX;
    public int maxX;

    public int minY;
    public int maxY;

    public List<Room> connectedRooms = new List<Room>();

    public Room(int _minX,int _maxX,int _minY,int _maxY) {
        minX = _minX;
        maxX = _maxX;
        minY = _minY;
        maxY = _maxY;
    }

    public Vector3Int GetCenter() {
        return new Vector3Int(Mathf.RoundToInt(Mathf.Lerp(minX,maxX,0.5f)),Mathf.RoundToInt(Mathf.Lerp(minY,maxY,0.5f)),0);
    }

    public bool IsPointInRoom(Vector3Int point) {
        return point.x >= minX-1 && point.x <= maxX+1 && point.y >= minY-1 && point.y <= maxY+1;
    }

    public Vector3Int GetRandomTile() {
        return new Vector3Int(Mathf.RoundToInt(Random.Range(minX,maxX + 1)),(Mathf.RoundToInt(Random.Range(minY,maxY + 1))),0);
    }

}
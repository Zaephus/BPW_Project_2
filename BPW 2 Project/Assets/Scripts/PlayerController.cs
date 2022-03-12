using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody2D body;
    public Transform playerTransform;
    private Vector3Int playerPosition;
    DungeonGenerator dungeonGen;

    public Dictionary<Vector3Int,TileType> dungeon = new Dictionary<Vector3Int, TileType>();

    public void Start() {

        body = GetComponent<Rigidbody2D>();
        dungeonGen = FindObjectOfType<DungeonGenerator>();
        playerPosition = Vector3Int.RoundToInt(playerTransform.position);
        dungeon = dungeonGen.dungeon;
        
    }

    public bool CanRight() {

        bool canMove = true;

        int x = playerPosition.x+1;
        int y = playerPosition.y;

        Vector3Int newPos = new Vector3Int(x,y,0);

        if(dungeon.ContainsKey(newPos)) {
            if(dungeon[newPos] == TileType.Wall) {
                canMove =  false;
            }
        }
        else {
            canMove = true;
        }
        return canMove;
    }

    public bool CanLeft() {

        bool canMove = true;

        int x = playerPosition.x-1;
        int y = playerPosition.y;

        Vector3Int newPos = new Vector3Int(x,y,0);

        if(dungeon.ContainsKey(newPos)) {
            if(dungeon[newPos] == TileType.Wall) {
                canMove =  false;
            }
        }
        else {
            canMove = true;
        }
        return canMove;
    }

    public bool CanUp() {

        bool canMove = true;

        int x = playerPosition.x;
        int y = playerPosition.y+1;

        Vector3Int newPos = new Vector3Int(x,y,0);

        if(dungeon.ContainsKey(newPos)) {
            if(dungeon[newPos] == TileType.Wall) {
                canMove =  false;
            }
        }
        else {
            canMove = true;
        }
        return canMove;

    }

    public bool CanDown() {

        bool canMove = true;

        int x = playerPosition.x;
        int y = playerPosition.y-1;

        Vector3Int newPos = new Vector3Int(x,y,0);

        if(dungeon.ContainsKey(newPos)) {
            if(dungeon[newPos] == TileType.Wall) {
                canMove =  false;
            }
        }
        else {
            canMove = true;
        }
        return canMove;
    }
    public void Update() {

        if(Input.GetKeyDown("d") && CanRight()) {
            MoveRight();
        }
        if(Input.GetKeyDown("a") && CanLeft()) {
            MoveLeft();
        }
        if(Input.GetKeyDown("w") && CanUp()) {
            MoveUp();
        }
        if(Input.GetKeyDown("s") && CanDown()) {
            MoveDown();
        }

        playerTransform.position = playerPosition;

    }

    public void MoveRight() {

        int x = playerPosition.x+1;
        int y = playerPosition.y;

        Vector3Int newPos = new Vector3Int(x,y,0);

        playerPosition = newPos;

    }

    public void MoveLeft() {

        int x = playerPosition.x-1;
        int y = playerPosition.y;

        Vector3Int newPos = new Vector3Int(x,y,0);

        playerPosition = newPos;

    }

    public void MoveUp() {

        int x = playerPosition.x;
        int y = playerPosition.y+1;

        Vector3Int newPos = new Vector3Int(x,y,0);

        playerPosition = newPos;

    }

    public void MoveDown() {

        int x = playerPosition.x;
        int y = playerPosition.y-1;

        Vector3Int newPos = new Vector3Int(x,y,0);

        playerPosition = newPos;

    }

}
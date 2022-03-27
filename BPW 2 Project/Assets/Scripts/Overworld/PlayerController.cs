using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody2D body;

    private Vector3Int targetPosition;

    public float moveSpeed = 5f;

    DungeonGenerator dungeonGen;

    public Dictionary<Vector3Int,TileType> dungeon = new Dictionary<Vector3Int, TileType>();

    public void Start() {

        body = GetComponent<Rigidbody2D>();
        targetPosition = Vector3Int.RoundToInt(transform.position);

        dungeonGen = FindObjectOfType<DungeonGenerator>();        
        dungeon = dungeonGen.dungeon;
        
    }

    public bool CanRight() {

        bool canMove = true;

        int x = targetPosition.x+1;
        int y = targetPosition.y;

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

        int x = targetPosition.x-1;
        int y = targetPosition.y;

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

        int x = targetPosition.x;
        int y = targetPosition.y+1;

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

        int x = targetPosition.x;
        int y = targetPosition.y-1;

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

        transform.position = Vector3.MoveTowards(transform.position,targetPosition,moveSpeed*Time.deltaTime);

        if(Vector3.Distance(transform.position,targetPosition) <= 0.05f) {

            if(Input.GetKey("d") && CanRight()) {
                MoveRight();
            }
            if(Input.GetKey("a") && CanLeft()) {
                MoveLeft();
            }
            if(Input.GetKey("w") && CanUp()) {
                MoveUp();
            }
            if(Input.GetKey("s") && CanDown()) {
                MoveDown();
            }

        }

    }

    public void MoveRight() {

        int x = targetPosition.x+1;
        int y = targetPosition.y;

        Vector3Int newPos = new Vector3Int(x,y,0);

        targetPosition = newPos;

    }

    public void MoveLeft() {

        int x = targetPosition.x-1;
        int y = targetPosition.y;

        Vector3Int newPos = new Vector3Int(x,y,0);

        targetPosition = newPos;

    }

    public void MoveUp() {

        int x = targetPosition.x;
        int y = targetPosition.y+1;

        Vector3Int newPos = new Vector3Int(x,y,0);

        targetPosition = newPos;

    }

    public void MoveDown() {

        int x = targetPosition.x;
        int y = targetPosition.y-1;

        Vector3Int newPos = new Vector3Int(x,y,0);

        targetPosition = newPos;

    }

}
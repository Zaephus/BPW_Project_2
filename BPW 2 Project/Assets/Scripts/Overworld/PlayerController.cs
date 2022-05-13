using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    DungeonManager dungeon;

    Rigidbody2D body;

    private Vector3Int targetPosition;

    public PlayerUnit baseUnit;
    public PlayerUnit unit;

    public Vector3Int startPos;

    public float moveSpeed = 5f;

    public void OnStart() {

        unit = ScriptableObject.CreateInstance<PlayerUnit>();
        SetUnitValues();

        dungeon = FindObjectOfType<DungeonManager>();

        body = GetComponent<Rigidbody2D>();
        targetPosition = Vector3Int.RoundToInt(transform.position);
        
    }
    
    public void OnUpdate() {

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

    #region movement
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

    public bool CanRight() {

        bool canMove = true;

        int x = targetPosition.x+1;
        int y = targetPosition.y;

        Vector3Int newPos = new Vector3Int(x,y,0);

        if(dungeon.dungeon.ContainsKey(newPos)) {
            if(dungeon.dungeon[newPos] == TileType.Wall || dungeon.EntityOnTile(newPos)) {
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

        if(dungeon.dungeon.ContainsKey(newPos)) {
            if(dungeon.dungeon[newPos] == TileType.Wall || dungeon.EntityOnTile(newPos)) {
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

        if(dungeon.dungeon.ContainsKey(newPos)) {
            if(dungeon.dungeon[newPos] == TileType.Wall || dungeon.EntityOnTile(newPos)) {
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

        if(dungeon.dungeon.ContainsKey(newPos)) {
            if(dungeon.dungeon[newPos] == TileType.Wall || dungeon.EntityOnTile(newPos)) {
                canMove =  false;
            }
        }
        else {
            canMove = true;
        }

        return canMove;
        
    }
    #endregion

    public void SetUnitValues() {

        unit.unitName = baseUnit.unitName.Replace("Base","");

        unit.maxHealth = baseUnit.maxHealth;
        unit.currentHealth = baseUnit.currentHealth;

        unit.baseAttackStrength = baseUnit.baseAttackStrength;
        unit.currentAttackStrength = baseUnit.currentAttackStrength;

        unit.baseDefenseStrength = baseUnit.baseDefenseStrength;
        unit.currentDefenseStrength = baseUnit.currentDefenseStrength;

        unit.lastPosX = startPos.x;
        unit.lastPosY = startPos.y;

        unit.abilities = baseUnit.abilities;

        SaveSystem.instance.SaveUnit(unit,unit.unitName);

    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    DungeonManager dungeon;

    Rigidbody2D body;

    public EnemyUnit baseUnit;
    [SerializeField]
    EnemyUnit unit;

    public Transform targetSprite;
    private Vector3Int targetPosition;

    private Vector3Int nextPosition;
    public float moveSpeed = 5f;

    public int viewDist = 2;
    public int randomTileRange = 4;

    public void Start() {

        unit = ScriptableObject.CreateInstance<EnemyUnit>();
        SetUnitValues();
        unit.startPosX = transform.position.x;
        unit.startPosY = transform.position.y;

        dungeon = FindObjectOfType<DungeonManager>();

        body = GetComponent<Rigidbody2D>();
        nextPosition = Vector3Int.RoundToInt(transform.position);
        targetPosition = Vector3Int.RoundToInt(transform.position);
        
    }

    public void Update() {

        if(Vector3.Distance(transform.position,dungeon.player.transform.position) <= viewDist) {
            MoveTowardsTarget(Vector3Int.FloorToInt(dungeon.player.transform.position));
        }
        else {
            MoveTowardsTarget(targetPosition);
        }

        if(Vector3.Distance(transform.position,targetPosition) <= 0.05f || Vector3.Distance(transform.position,targetPosition) >= viewDist) {
            targetPosition = GetTargetTile();
        }

    }

    public void MoveTowardsTarget(Vector3Int target) {

        targetSprite.position = target;

        transform.position = Vector3.MoveTowards(transform.position,nextPosition,moveSpeed*Time.deltaTime);

        if(Vector3.Distance(transform.position,nextPosition) <= 0.05f) {

            if(Mathf.Abs(target.x-transform.position.x) >= Mathf.Abs(target.y-transform.position.y)) {

                if(target.x > transform.position.x) {
                    if(CanRight()) {
                        MoveRight();
                    }
                    else {
                        if(target.y < transform.position.y) {
                            if(CanDown()) {
                                MoveDown();
                            }
                        }
                        else if(target.y > transform.position.y) {
                            if(CanUp()) {
                                MoveUp();
                            }
                        }
                    }
                }
                else if(target.x < transform.position.x) {
                    if(CanLeft()) {
                        MoveLeft();
                    }
                    else {
                        if(target.y < transform.position.y) {
                            if(CanDown()) {
                                MoveDown();
                            }
                        }
                        else if(target.y > transform.position.y) {
                            if(CanUp()) {
                                MoveUp();
                            }
                        }
                    }
                }

            }
        
            if(Mathf.Abs(target.x-transform.position.x) < Mathf.Abs(target.y-transform.position.y)) {
            
                if(target.y < transform.position.y) {
                    if(CanDown()) {
                        MoveDown();
                    }
                    else {
                        if(target.x > transform.position.x) {
                            if(CanRight()) {
                                MoveRight();
                            }
                        }
                        else if(target.x < transform.position.x) {
                            if(CanLeft()) {
                                MoveLeft();
                            }
                        }
                    }
                }
                else if(target.y > transform.position.y) {
                    if(CanUp()) {
                        MoveUp();
                    }
                    else {
                        if(target.x > transform.position.x) {
                            if(CanRight()) {
                                MoveRight();
                            }
                        }
                        else if(target.x < transform.position.x) {
                            if(CanLeft()) {
                                MoveLeft();
                            }
                        }
                    }
                }

            }

        }

    }

    public Vector3Int GetTargetTile() {

        List<Vector3Int> tiles = new List<Vector3Int>();

        for(int x = (int)(transform.position.x) - randomTileRange; x <= (int)(transform.position.x) + randomTileRange; x++) {
            for(int y = (int)(transform.position.y) - randomTileRange; y <= (int)(transform.position.y) + randomTileRange; y++) {

                Vector3Int pos = new Vector3Int(x,y,0);
                TileType value;

                if(dungeon.dungeon.TryGetValue(pos,out value)) {
                    if(value != TileType.Wall) {
                        tiles.Add(pos);
                    }
                }

            }
        }

        return tiles[Random.Range(0,tiles.Count)];

    }

    public void MoveRight() {

        int x = nextPosition.x+1;
        int y = nextPosition.y;

        Vector3Int newPos = new Vector3Int(x,y,0);

        nextPosition = newPos;

    }

    public void MoveLeft() {

        int x = nextPosition.x-1;
        int y = nextPosition.y;

        Vector3Int newPos = new Vector3Int(x,y,0);

        nextPosition = newPos;

    }

    public void MoveUp() {

        int x = nextPosition.x;
        int y = nextPosition.y+1;

        Vector3Int newPos = new Vector3Int(x,y,0);

        nextPosition = newPos;

    }

    public void MoveDown() {

        int x = nextPosition.x;
        int y = nextPosition.y-1;

        Vector3Int newPos = new Vector3Int(x,y,0);

        nextPosition = newPos;

    }

    public bool CanRight() {

        bool canMove = true;

        int x = nextPosition.x+1;
        int y = nextPosition.y;

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

        int x = nextPosition.x-1;
        int y = nextPosition.y;

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

        int x = nextPosition.x;
        int y = nextPosition.y+1;

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

        int x = nextPosition.x;
        int y = nextPosition.y-1;

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

    public void SetUnitValues() {

        unit.unitName = baseUnit.unitName;

        unit.maxHealth = baseUnit.maxHealth;
        unit.currentHealth = baseUnit.currentHealth;

        unit.baseAttackStrength = baseUnit.baseAttackStrength;
        unit.currentAttackStrength = baseUnit.currentAttackStrength;

        unit.baseDefenseStrength = baseUnit.baseDefenseStrength;
        unit.currentDefenseStrength = baseUnit.currentDefenseStrength;

    }

}
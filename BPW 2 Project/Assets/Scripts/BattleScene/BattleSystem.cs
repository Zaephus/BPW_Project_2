using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {Start,PlayerTurn,EnemyTurn,Wait,Won,Lost}

public class BattleSystem : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;

    public Unit playerUnit;
    public Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public void Start() {

        state = BattleState.Start;
        StartCoroutine(SetupBattle());

    }

    public IEnumerator SetupBattle() {

        GameObject player = Instantiate(playerPrefab,playerPosition.position,Quaternion.identity,this.transform);

        GameObject enemy = Instantiate(enemyPrefab,enemyPosition.position,Quaternion.identity,this.transform);

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches!!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(1f);

        state  = BattleState.Wait;
        StartCoroutine(PlayerTurn());

    }

    public IEnumerator PlayerTurn() {

        yield return new WaitForSeconds(1f);

        dialogueText.text = "Choose an action:";
        state = BattleState.PlayerTurn;

    }

    public IEnumerator PlayerAttack() {

        dialogueText.text = "The attack is succesful!";
        
        yield return new WaitForSeconds(2f);

        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHealth(enemyUnit.currentHealth);

        if(isDead) {
            state = BattleState.Won;
            StartCoroutine(EndBattle());
        }
        else {
            state = BattleState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }

    }

    public IEnumerator PlayerHeal() {

        dialogueText.text = "You healed by 5 points!";

        yield return new WaitForSeconds(2f);

        playerUnit.Heal(5);
        playerHUD.SetHealth(playerUnit.currentHealth);

        StartCoroutine(EnemyTurn());
        
    }

    public IEnumerator EnemyTurn() {

        yield return new WaitForSeconds(1f);

        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(2f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHealth(playerUnit.currentHealth);

        if(isDead) {
            state = BattleState.Lost;
            StartCoroutine(EndBattle());
        }
        else {
            state = BattleState.PlayerTurn;
            StartCoroutine(PlayerTurn());
        }
    }

    public IEnumerator EndBattle() {

        if(state == BattleState.Won) {
            dialogueText.text = "You won the battle!";
            yield return new WaitForSeconds(2f);
            LevelLoader.instance.LoadLevel("MainScene");
        }
        else if(state == BattleState.Lost) {
            dialogueText.text = "You lost.";
            yield return new WaitForSeconds(2f);
            LevelLoader.instance.LoadLevel("MainScene");
        }
        
    }
    public void OnAttackButton() {

        if(state != BattleState.PlayerTurn) {
            return;
        }

        state = BattleState.Wait;

        StartCoroutine(PlayerAttack());

    }

    public void OnHealButton() {

        if(state != BattleState.PlayerTurn) {
            return;
        }

        state = BattleState.Wait;

        StartCoroutine(PlayerHeal());

    }

}
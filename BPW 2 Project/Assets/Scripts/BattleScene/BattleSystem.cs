using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {Start,PlayerTurn,EnemyTurn,Wait,Won,Lost}

public class BattleSystem : MonoBehaviour {

    public GameObject playerPrefab;
    private GameObject enemyPrefab;
    public GameObject abilityButtonPrefab;

    public Transform playerPosition;
    public Transform enemyPosition;

    public PlayerUnit playerUnit;
    public EnemyUnit currentUnit;
    public List<EnemyUnit> units = new List<EnemyUnit>();

    public Text dialogueText;

    public GameObject combatButtons;
    public List<AttackButton> abilityButtons = new List<AttackButton>();

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public void Start() {

        currentUnit = LevelLoader.instance.currentUnit;

        enemyPrefab = currentUnit.unitPrefab;

        state = BattleState.Start;
        StartCoroutine(SetupBattle());

    }

    public void Update() {

        if(state == BattleState.PlayerTurn) {
            combatButtons.SetActive(true);
        }
        else {
            combatButtons.SetActive(false);
        }

    }

    public IEnumerator SetupBattle() {

        for(int i = 0; i < playerUnit.abilities.Count; i++) {

            GameObject abilityButton =Instantiate(abilityButtonPrefab,combatButtons.transform);

            abilityButtons.Add(abilityButton.GetComponent<AttackButton>());
            abilityButtons[i].Initialize(playerUnit.abilities[i],this);

        }

        foreach(Ability a in currentUnit.abilities) {
            a.Initialize(this);
        }

        GameObject player = Instantiate(playerPrefab,playerPosition.position,Quaternion.identity,this.transform);

        GameObject enemy = Instantiate(enemyPrefab,enemyPosition.position,Quaternion.identity,this.transform);

        dialogueText.text = "A " + currentUnit.unitName + " is attacking you!!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(currentUnit);

        yield return new WaitForSeconds(1f);

        state  = BattleState.Wait;
        StartCoroutine(PlayerTurn());

    }

    public IEnumerator PlayerTurn() {

        yield return new WaitForSeconds(1f);

        dialogueText.text = "Choose an action:";
        state = BattleState.PlayerTurn;

    }

    public IEnumerator EnemyTurn() {

        yield return new WaitForSeconds(1f);

        foreach(Ability a in currentUnit.abilities) {
            StartCoroutine(a.DoBehaviour());
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

}
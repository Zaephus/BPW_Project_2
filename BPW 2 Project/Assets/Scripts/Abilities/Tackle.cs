using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tackle",menuName = "Abilities/Tackle")]
public class Tackle : Ability {

    private BattleSystem battleSystem;

    public int basePower;

    public override void Initialize(BattleSystem battleSystem) {
        this.battleSystem = battleSystem;
    }

    public override IEnumerator DoBehaviour() {

        battleSystem.dialogueText.text = "You tackle the enemy " + battleSystem.currentUnit.unitName + "!";

        yield return new WaitForSeconds(2f);

        bool isDead = battleSystem.currentUnit.TakeDamage(basePower*battleSystem.playerUnit.currentAttackStrength);
        battleSystem.enemyHUD.SetHealth(battleSystem.currentUnit.currentHealth);

        if(isDead) {
            battleSystem.state = BattleState.Won;
            battleSystem.StartCoroutine(battleSystem.EndBattle());
        }
        else {
            battleSystem.state = BattleState.EnemyTurn;
            battleSystem.StartCoroutine(battleSystem.EnemyTurn());
        }

    }
    
}
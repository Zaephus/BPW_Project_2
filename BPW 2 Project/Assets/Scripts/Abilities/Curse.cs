using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Curse",menuName = "Abilities/Curse")]
public class Curse : Ability {

    private BattleSystem battleSystem;

    public int basePower;

    public override void Initialize(BattleSystem battleSystem) {
        this.battleSystem = battleSystem;
    }

    public override IEnumerator DoBehaviour() {

        battleSystem.dialogueText.text = battleSystem.currentUnit.unitName + " curses you!";

        yield return new WaitForSeconds(2f);

        bool isDead = battleSystem.playerUnit.TakeDamage(basePower*battleSystem.currentUnit.currentAttackStrength);
        battleSystem.playerHUD.SetHealth(battleSystem.playerUnit.currentHealth);

        if(isDead) {
            battleSystem.state = BattleState.Lost;
            battleSystem.StartCoroutine(battleSystem.EndBattle());
        }
        else {
            battleSystem.state = BattleState.Wait;
            battleSystem.StartCoroutine(battleSystem.PlayerTurn());
        }

    }
    
}
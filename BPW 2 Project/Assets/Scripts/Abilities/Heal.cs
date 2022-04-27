using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal",menuName = "Abilities/Heal")]
public class Heal : Ability {

    private BattleSystem battleSystem;

    public int healAmount;

    public override void Initialize(BattleSystem battleSystem) {
        this.battleSystem = battleSystem;
    }

    public override IEnumerator DoBehaviour() {

        battleSystem.state = BattleState.Wait;

        if(battleSystem.playerUnit.currentHealth < battleSystem.playerUnit.maxHealth) {
        
            battleSystem.dialogueText.text = "You heal by " + healAmount + " HP!";

            yield return new WaitForSeconds(2f);

            battleSystem.playerUnit.Heal(healAmount);
            battleSystem.playerHUD.SetHealth(battleSystem.playerUnit.currentHealth);

            battleSystem.state = BattleState.EnemyTurn;
            battleSystem.StartCoroutine(battleSystem.EnemyTurn());

        }
        else {
            battleSystem.dialogueText.text = "You can not heal, you are already at full health!";
            yield return new WaitForSeconds(2f);
            battleSystem.state = BattleState.PlayerTurn;
        }


    }
}
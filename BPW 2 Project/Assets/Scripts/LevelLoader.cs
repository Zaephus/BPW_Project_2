using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    #region Singleton
    public static LevelLoader instance;

    void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public float transitionTime = 2f;

    [HideInInspector]
    public EnemyUnit currentUnit;

    public void LoadLevel(string levelName) {
        StartCoroutine(LoadNamedLevel(levelName));
    }

    public IEnumerator LoadNamedLevel(string levelName) {

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
        
    }

    public void StartBattle(EnemyUnit unit) {
        //SaveSystem.instance.SaveUnit(unit,"CurrentUnit");
        currentUnit = unit;
        LoadLevel("BattleScene");
    }

    public void StartGame() {
        SaveSystem.instance.SaveSeed(0,"DungeonSeed");
        LoadLevel("MainScene");
    }

}
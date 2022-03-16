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

    public void LoadLevel(string levelName) {
        StartCoroutine(LoadNamedLevel(levelName));
    }

    public IEnumerator LoadNamedLevel(string levelName) {

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
        
    }

}
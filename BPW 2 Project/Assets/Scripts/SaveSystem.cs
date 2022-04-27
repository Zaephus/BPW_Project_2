using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveSystem : MonoBehaviour {

    private string path;

    #region Singleton
    public static SaveSystem instance;

    void Awake() {
        
        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }
    #endregion

    public void SetPath(string fileName) {
        if(Application.isEditor) {
            path = Application.dataPath + "/SaveData/" + fileName + ".txt";
        }
        else {
            path = Application.persistentDataPath + fileName + ".txt";
        }
    }

    public void SaveUnit(Unit unit,string fileName) {

        SetPath(fileName);

        UnitSaver targetUnit = new UnitSaver();

        targetUnit.unitName = unit.unitName;

        targetUnit.maxHealth = unit.maxHealth;
        targetUnit.currentHealth = unit.currentHealth;

        targetUnit.baseAttackStrength = unit.baseAttackStrength;
        targetUnit.currentAttackStrength = unit.currentAttackStrength;

        targetUnit.baseDefenseStrength = unit.baseDefenseStrength;
        targetUnit.currentDefenseStrength = unit.currentDefenseStrength;

        targetUnit.lastPosX = unit.lastPosX;
        targetUnit.lastPosY = unit.lastPosY;

        StreamWriter writer = new StreamWriter(path,false);

        writer.WriteLine(JsonUtility.ToJson(targetUnit,true));
        writer.Close();
        writer.Dispose();

    }

    public void LoadUnit(Unit unit,string fileName) {
            
        SetPath(fileName);

        if(File.Exists(path)) {

            StreamReader reader = new StreamReader(path);

            UnitSaver targetUnit = JsonUtility.FromJson<UnitSaver>(reader.ReadToEnd());

            unit.unitName = targetUnit.unitName;

            unit.maxHealth = targetUnit.maxHealth;
            unit.currentHealth = targetUnit.currentHealth;

            unit.baseAttackStrength = targetUnit.baseAttackStrength;
            unit.currentAttackStrength = targetUnit.currentAttackStrength;

            unit.baseDefenseStrength = targetUnit.baseDefenseStrength;
            unit.currentDefenseStrength = targetUnit.currentDefenseStrength;

            unit.lastPosX = targetUnit.lastPosX;
            unit.lastPosY = targetUnit.lastPosY;

            reader.Close();
            reader.Dispose();

        }
        else {
            SaveUnit(unit,fileName);
        }
        
    }

    public void SaveSeed(int seed,string fileName) {

        SetPath(fileName);

        DungeonSaver targetSeed = new DungeonSaver();

        targetSeed.seed = seed;

        StreamWriter writer = new StreamWriter(path,false);

        writer.WriteLine(JsonUtility.ToJson(targetSeed,true));
        writer.Close();
        writer.Dispose();

    }

    public int LoadSeed(string fileName) {

        SetPath(fileName);

        if(File.Exists(path)) {

            StreamReader reader = new StreamReader(path);

            DungeonSaver targetSeed = JsonUtility.FromJson<DungeonSaver>(reader.ReadToEnd());

            reader.Close();
            reader.Dispose();

            return targetSeed.seed;

        }
        else {
            return 0;
        }
    }

}
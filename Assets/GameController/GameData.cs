using System.Linq;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;
    public string saveFileName;

    private string mSaveFilePath;
    private GameDataHolder mLocalData;

    public GameDataHolder GetData
    {
        get { return mLocalData; }
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            mSaveFilePath = Application.persistentDataPath + "/" + saveFileName + ".dat";
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        Debug.Log("Save file: " + mSaveFilePath);
        if (mLocalData != null)
        {
            BinaryFormatter binFor = new BinaryFormatter();
            FileStream fileStr = File.Create(mSaveFilePath);

            binFor.Serialize(fileStr, mLocalData);
            fileStr.Close();
        }
    }

    public void Load()
    {
        Debug.Log("Load save file: " + mSaveFilePath);
        if (File.Exists(mSaveFilePath))
        {
            BinaryFormatter binFor = new BinaryFormatter();
            FileStream fileStr = File.Open(mSaveFilePath, FileMode.Open);

            mLocalData = binFor.Deserialize(fileStr) as GameDataHolder;
            fileStr.Close();
        }
        else
        {
            mLocalData = new GameDataHolder();
            Save();
        }
    }
}

[Serializable]
public class GameDataHolder
{
    //Save capacity max 10 levels
    private Dictionary<string, LevelSave> mLevelSaves = new Dictionary<string, LevelSave>(10);

    public LevelSave GetLevelSave(string levelName)
    {
        if (mLevelSaves.ContainsKey(levelName))
        {
            return mLevelSaves[levelName];
        }
        return mLevelSaves[levelName] = new LevelSave();
    }
}

[Serializable]
public class LevelSave
{
    private List<KeyValuePair<string, int>> mScores = new List<KeyValuePair<string, int>>(); 

    public LevelSave()
    {
    }

    public void AddScore(string playerName, int score)
    {
        Debug.Log("Add score " + playerName + " " + score);
        mScores.Add(new KeyValuePair<string, int>(playerName, score));
        //mScores.RemoveRange(mMaxScores, mScores.Count);
    }

    public List<KeyValuePair<string, int>> GetBestScoresRaw(int range)
    {
        if(range > mScores.Count)
        {
            return mScores;
        }
        return mScores.GetRange(0, range);
    }
    public string GetBestScoresString(int range)
    {
        mScores = mScores.OrderByDescending(x => x.Value).ToList();

        List<KeyValuePair<string, int>> bestScores = GetBestScoresRaw(range);
        string outString = "Name    Score \n";

        foreach(KeyValuePair<string, int> score in bestScores)
        {
            outString += score.Key + "    " + score.Value + " pts\n";
        }

        return outString;
    }
}
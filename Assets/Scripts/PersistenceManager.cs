using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager instance;

    //Table of high scores ordered by score from max to min
    [System.NonSerialized]    public Dictionary<string,int> persistence = new Dictionary<string, int>();

    //The name of the last player
    [System.NonSerialized]    public string lastPlayer;

    private const int maxHighScores = 10;       //number of records stored

    private void Awake()
    {
        //Use a singleton to store the data
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //Class used to store the needed data
    //Dictionary isn't serializable using JsonUtility so I store the name of the player and score in two array
    [System.Serializable]
    class SaveData
    {
        public string[] name;
        public int[] score;
        public string lastName;
    }

    //Persist data to disk
    public void Save()
    {
        SaveData data = new SaveData();
        data.name = persistence.Keys.ToArray();
        data.score = persistence.Values.ToArray();
        data.lastName = lastPlayer;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            persistence.Clear();
            for (int i=0;i<data.score.Length;i++)
            {
                persistence.Add(data.name[i], data.score[i]);
            }

            //set loaded values in the PersistenceManager
            lastPlayer = data.lastName;
            //Order the high scores from max to min
            persistence = persistence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }

    public void AddNewScore(int score)
    {
        //Add to the high score only if the score is greater than the last strored one, or the high score table isn't full
        if (persistence.Count() < maxHighScores || score > persistence.Values.Last() )
        {
            if (persistence.Count() == maxHighScores)
            {
                persistence.Remove(persistence.Keys.Last());
            }

            if (persistence.ContainsKey(lastPlayer))
            {
                persistence[lastPlayer] = score;
            }
            else
            {
                persistence.Add(lastPlayer, score);
            }

            //Order the dictionary
            persistence = persistence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}

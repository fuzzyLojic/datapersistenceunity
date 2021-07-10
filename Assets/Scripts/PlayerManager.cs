using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private string playerName;
    private int highScore;

    private DataCollection scoreCollection;
    private List<SaveData> scoresList;    // savefile contents

    public string PlayerName { get{ return playerName; } }
    public int HighScore { get{ return highScore; } }

    private TMP_InputField nameInput;

    private void Awake() {
        if(Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        scoreCollection = new DataCollection();
        scoresList = scoreCollection.scoresList;
        GetNameInput();
        LoadScores();
    }

    public void UpdatePlayerName(string pName){
        playerName = pName;
        if(scoresList.ConvertAll(x => x.name).Contains(pName)){
            highScore = scoresList.Find(x => x.name.Contains(pName)).score;
        }
        else{
            highScore = 0;
        }
    }

    public void UpdateHighScore(int score){
        if(score > highScore){
            highScore = score;
        }
    }

    public void GetNameInput(){
        nameInput = GameObject.Find("NameInput").GetComponent<TMP_InputField>();
        nameInput.onValueChanged.AddListener(UpdatePlayerName);
    }

    public void SaveScore(){
        if(scoresList.ConvertAll(x => x.name).Contains(playerName)){
            scoresList.Find(x => x.name.Contains(playerName)).score = highScore;
        }
        else{
            SaveData data = new SaveData();
            data.name = playerName;
            data.score = highScore;
            scoresList.Add(data);
        }

        scoreCollection.scoresList = scoresList;
        string json = JsonUtility.ToJson(scoreCollection);
        Debug.Log(json);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadScores(){
        scoresList = new List<SaveData>();

        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path)){
            string jsonFile = File.ReadAllText(path);
            scoresList = JsonUtility.FromJson<DataCollection>(jsonFile).scoresList;
        }
    }

    [System.Serializable]
    class SaveData{
        public string name;
        public int score;
    }

    [System.Serializable]
    class DataCollection{
        [SerializeField]
        public List<SaveData> scoresList = new List<SaveData>();
    }
}

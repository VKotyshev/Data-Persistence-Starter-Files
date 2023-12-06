using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public TextMeshProUGUI bestScoreText;
    public bool isBestExists;

    private string _currentName;
    private string _bestName;
    private int _bestScore;

    private void Start()
    {
        if (gameManager != null)
        {
            LoadHighScore();
            bestScoreText.text = ShowHighScore();

            Destroy(gameObject);
            return;
        }

        gameManager = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScore();
        bestScoreText.text = ShowHighScore();
    }

    [System.Serializable]
    class SaveData
    {
        public string name;
        public int score;
    }

    public void SaveHighScore(int score)
    {
        if (score <= bestScore) return;

        bestName = currentName;
        bestScore = score;

        SaveData data = new SaveData();
        data.name = currentName;
        data.score = score;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);

        if (!isBestExists) isBestExists = true;
    }

    private void LoadHighScore()
    {
        isBestExists = File.Exists(Application.persistentDataPath + "/savedata.json");

        if (!isBestExists) return;

        string json = File.ReadAllText(Application.persistentDataPath + "/savedata.json");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        bestName = data.name;
        bestScore = data.score;
    }

    public string ShowHighScore()
    {
        if (!isBestExists) return "There is no record yet!";
        return $"Best Score: {bestScore} points by {bestName}";
    }



    public string currentName { get { return _currentName; } set { _currentName = value; } }
    public string bestName { get { return _bestName; } set { _bestName = value; } }
    public int bestScore { get { return _bestScore; } set { _bestScore = value; } }
}
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using VInspector;

public class HighScoresPanel : MonoBehaviour
{
    public GameObject highScorePrefab;
    public List<HighScore> highScores = new List<HighScore>();
    public Transform createPrefabHere;
    public TMP_InputField nameInput;
    public string filePath;

    void Start()
    {  
        filePath = Application.persistentDataPath + "/Scores_" + SceneManager.GetActiveScene().name;
        string loadJson;
        try
        {
            loadJson = System.IO.File.ReadAllText(filePath);
        }
        catch
        {
            Debug.Log("Scores file not found");
            loadJson = null;
        }
        if (loadJson!=null)
        {
            HighScoreWrapper wrapper = JsonUtility.FromJson<HighScoreWrapper>(loadJson);
            highScores = wrapper.highScores;
            refreshList();
        }
      
    }
    void refreshList()
    {

        highScores = highScores.OrderBy(ch => ch.timeTaken).ToList();
        foreach(Transform child in createPrefabHere)
        {
            Destroy(child.gameObject);
        }

        int i = 1;
        foreach (HighScore highScore in highScores)
        {
            GameObject panel = Instantiate(highScorePrefab, createPrefabHere);
            TextMeshProUGUI text = panel.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "#"+i+ " "+highScore.name + " - " + WinPanel.FormatTime(highScore.timeTaken);
            i++;
        }
    }

    [Button("Submit")]

    public void Submit()
    {
        HighScore hs = new HighScore();
        var timer = ActivityScript.instance.GetComponent<Timer>();
        hs.timeTaken = timer.maxTime - timer.remainingTime;
        hs.name = nameInput.text;
        highScores.Add(hs);
        refreshList();

        string jsonScores = JsonUtility.ToJson(new HighScoreWrapper(highScores));

        System.IO.File.WriteAllText(filePath,jsonScores);
    }

    public void showKB()
    {
        Transform kb = transform.Find("KB");
        NonNativeKeyboard.Instance.transform.position = kb.position;
        NonNativeKeyboard.Instance.transform.rotation = kb.rotation;
        NonNativeKeyboard.Instance.PresentKeyboard();
    }

 

  
}

[System.Serializable]
public class HighScoreWrapper
{
    public List<HighScore> highScores;

    public HighScoreWrapper(List<HighScore> highScores)
    {
        this.highScores = highScores;
    }  
}

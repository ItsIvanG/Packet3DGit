using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HighScoresPanel : MonoBehaviour
{
    public GameObject highScorePrefab;
    public List<HighScore> highScores;
    public Transform createPrefabHere;
    public TMP_InputField nameInput;

    void Start()
    {
        refreshList();
    }
    void refreshList()
    {

        highScores = highScores.OrderBy(ch => ch.timeTaken).ToList();
        foreach(Transform child in createPrefabHere)
        {
            Destroy(child.gameObject);
        }


        foreach (HighScore highScore in highScores)
        {
            GameObject panel = Instantiate(highScorePrefab, createPrefabHere);
            TextMeshProUGUI text = panel.GetComponentInChildren<TextMeshProUGUI>();
            text.text = highScore.name + " - " + WinPanel.FormatTime(highScore.timeTaken);
        }
    }

    public void Submit()
    {
        HighScore hs = new HighScore();
        var timer = ActivityScript.instance.GetComponent<Timer>();
        hs.timeTaken = timer.maxTime - timer.remainingTime;
        hs.name = nameInput.text;
        highScores.Add(hs);
        refreshList();
    }

    public void showKB()
    {
        Transform kb = transform.Find("KB");
        NonNativeKeyboard.Instance.transform.position = kb.position;
        NonNativeKeyboard.Instance.transform.rotation = kb.rotation;
        NonNativeKeyboard.Instance.PresentKeyboard();
    }

  
}

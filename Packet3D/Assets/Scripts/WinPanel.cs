using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
//using UnityEditor.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public GameObject panel, confetti;
    public TextMeshProUGUI currentTimeString, bestTimeString;
    [Tooltip("Can be \"Tut\" or \"Act\"")]
    public string LevelsPrefix;
    public AudioSource musicStop;
    public Button okButton,skipScoresButton;
    public GameObject scoresPanel;
    public CanvasFollowPlayer canvasFollow;
    public HighScoresPanel highScoresPanel;

    public void win(float runningTime)
    {
        string levelName = SceneManager.GetActiveScene().name;
        currentTimeString.text = FormatTime(runningTime);

        float bestTime = PlayerPrefs.GetFloat(levelName + "_BestTime", Mathf.Infinity);

        int currentLevel = PlayerPrefs.GetInt("Current" + LevelsPrefix, 0);

        string getRegexIndex = Regex.Match(SceneManager.GetActiveScene().name, @"\d+").Value;
        int levelIndex = int.Parse(getRegexIndex) -1;
        Debug.Log("Level index: " + levelIndex);
        if (levelIndex == currentLevel)
        {
            PlayerPrefs.SetInt("Current" + LevelsPrefix, levelIndex+1);
            Debug.Log("Ascending currentLevel to " + LevelsPrefix + (levelIndex + 1));
        }

        if (runningTime < bestTime)
        {
            PlayerPrefs.SetFloat(levelName + "_BestTime", runningTime);
            PlayerPrefs.Save();
            Debug.Log("New Best Time for level: "+levelName+"! " + FormatTime(runningTime));
            bestTimeString.text = FormatTime(runningTime);
        }
        else
        {
            bestTimeString.text = FormatTime(bestTime);

        }
        var getAudioSources = FindObjectsByType<AudioSource>(0);
        foreach(AudioSource audioSource in getAudioSources)
        {
            audioSource.Stop();
        }
        GetComponent<AudioSource>().Play();

        if (musicStop) musicStop.Stop();
    }
    public static string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0}:{1:00}", minutes, seconds);
    }
    private void Start()
    {
        if (FindAnyObjectByType<ActivityScript>())
        {
            //UnityEventTools.RemovePersistentListener(okButton.onClick,0);
            okButton.onClick.AddListener(pressOK);
            skipScoresButton.onClick.AddListener(skipScores);
            LevelsPrefix = "Act";
        }
    }
    void pressOK()
    {
        panel.SetActive(false);
        scoresPanel.SetActive(true);
        canvasFollow.stopFollow();
        highScoresPanel.showKB();
    }
    void skipScores()
    {
        FindObjectOfType<UIFadeInAndLoadScene>().StartFade("Menu");
    }
}

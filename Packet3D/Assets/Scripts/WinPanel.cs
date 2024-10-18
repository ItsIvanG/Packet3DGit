using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    public GameObject panel, confetti;
    public TextMeshProUGUI currentTimeString, bestTimeString;
    [Tooltip("Can be \"Tut\" or \"Act\"")]
    public string LevelsPrefix;

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
    }
    public static string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0}:{1:00}", minutes, seconds);
    }
}

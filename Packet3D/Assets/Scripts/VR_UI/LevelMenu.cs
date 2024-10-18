using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public levelBase[] levels;
    public GameObject buttonPrefab,contentPanel;
    public Image thumbnail;
    public TextMeshProUGUI levelHeader, levelDesc, levelBest;
    public Sprite Idle, clicked;
    [Tooltip("Can be \"Tut\" or \"Act\"")]
    public string LevelsPrefix;

    private void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("Current" + LevelsPrefix, 0);

        for (int i = 0;i < levels.Length; i++)
        {
            var button =  Instantiate(buttonPrefab, contentPanel.transform);
            button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = levels[i].levelName;
            button.GetComponent<LevelButton>().levelIndex = i;
            button.GetComponent<LevelButton>().menu = this;
            if (i > currentLevel)
            {
                button.GetComponent<Button>().interactable = false;
                button.GetComponent<LevelButton>().setLock();
            } 
            else if (i == currentLevel)
            {
                button.GetComponent<LevelButton>().setCurrent();
            }
            else
            {
                button.GetComponent<LevelButton>().setStar();
            }
            Debug.Log("button add: " + i);
        }
        refreshButtonSprites(0);
        showDetails(0);
    }
    public void ButtonClicked(int i)
    {
        Debug.Log("button click" + i);
        showDetails(i);
        refreshButtonSprites(i);
    }

    void showDetails(int i)
    {
        FindAnyObjectByType<UIFadeInAndLoadScene>().sceneToLoad = levels[i].sceneName;

        thumbnail.sprite = levels[i].levelThumbnail;
        levelHeader.text = levels[i].levelName;
        levelDesc.text = levels[i].levelDescription;
        float bestTime = PlayerPrefs.GetFloat(levels[i].sceneName + "_BestTime", Mathf.Infinity);

        if(bestTime != Mathf.Infinity)
        {
            levelBest.text = "BEST: " + WinPanel.FormatTime(bestTime);
        }
        else
        {
            levelBest.text = "";
        }
    }
    void refreshButtonSprites(int i)
    {
        for (int z = 0; z < levels.Length; z++)
        {
            var buttons = contentPanel.transform.GetComponentsInChildren<LevelButton>();
            foreach (var button in buttons)
            {
                if (button.levelIndex == i)
                {
                    button.GetComponent<Image>().sprite = clicked;
                }
                else
                {
                    button.GetComponent<Image>().sprite = Idle;

                }
            }
        }
    }
  

}

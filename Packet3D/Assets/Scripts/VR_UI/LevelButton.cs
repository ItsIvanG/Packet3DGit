using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;
    public LevelMenu menu;
    public Image image;
    public Sprite lockSprite, starSprite;
    public void clicked()
    {
        menu.ButtonClicked(levelIndex);
    }

    public void setLock()
    {
        image.sprite = lockSprite;
    }
    public void setStar()
    {
        image.sprite = starSprite;
    }
    public void setCurrent()
    {
        image.enabled = false;
    }
}

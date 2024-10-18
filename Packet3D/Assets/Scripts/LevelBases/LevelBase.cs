using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class levelBase
{

    public string levelName;
    public Sprite levelThumbnail;
    [TextArea]
    public string levelDescription;
    public string sceneName;
}

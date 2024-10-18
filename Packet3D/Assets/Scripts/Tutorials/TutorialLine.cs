using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

[System.Serializable]
public class TutorialLine
{
    [TextArea]
    public string line;
    public Sprite visual;
    //public GameObject willWaitFor;
    public List<TutorialWait> wait;

}

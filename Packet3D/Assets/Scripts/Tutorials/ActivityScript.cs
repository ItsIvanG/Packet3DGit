using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VInspector;

public class ActivityScript : MonoBehaviour
{
    public string Header;
    [TextArea]
    public string Content;
    public InputActionProperty yButtonAction;
    public TextMeshProUGUI HeaderReference;
    public TextMeshProUGUI ContentReference;
    public Image visualReference;
    public bool MakulitWait = false;
    public static ActivityScript instance;

    public WinPanel winCanvas;
    public Sprite visual;
    public int conditionsMet = 0;
    public bool isDone = false;

    public List<TutorialWait> conditions;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (yButtonAction.action.WasPressedThisFrame())
        {
            UIFadeInAndLoadScene uif = FindAnyObjectByType<UIFadeInAndLoadScene>();
            uif.StartFade();
        }
    }

    private void Start()
    {
        if(winCanvas == null)
        {
            winCanvas = FindAnyObjectByType<WinPanel>();
        }
        if (MakulitWait)
        {
            Debug.Log("Makulit ka");
            InvokeRepeating("checkWait", 0, 1f);

        }
        HeaderReference.text = Header;
        ContentReference.text = Content;
        if (visual) visualReference.sprite = visual;
        else visualReference.enabled = false;
    }
    public void checkWait()
    {
        if (!isDone)
        {
            conditionsMet = 0;
            foreach (var condition in conditions)
            {
                if (condition.testWait()) {
                    conditionsMet++;
                    Debug.Log("Met condition " + condition.waitType.ToString() + " WITH FIELD "+condition.fieldCheck);
                        };
            }
            if (conditionsMet >= conditions.Count)
            {
                levelWin();
            }
        }
       
    }
    [Button("Win")]
    public void levelWin()
    {
        isDone = true;
        winCanvas.panel.SetActive(true);
        winCanvas.confetti.SetActive(true);
        winCanvas.win(GetComponent<Timer>().maxTime - GetComponent<Timer>().remainingTime);
        GetComponent<Timer>().stopTimer();
    }
}

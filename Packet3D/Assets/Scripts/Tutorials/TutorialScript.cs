using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using VInspector;

public class TutorialScript : MonoBehaviour
{
    public InputActionProperty prevLineAction;
    public InputActionProperty nextLineAction;
    public InputActionProperty yButtonAction;
    public TextMeshProUGUI LineRef;
    public int currentLine = 0;
    public int currentWait = 0;
    public static TutorialScript instance;
    public BMO_Behavior bmo;
    public bool MakulitWait = false; //repeatedly checks!!
    public GameObject prevButton,nextButton;
    bool isDone = false;
    public WinPanel winCanvas;
    [Header("Wait Color Effect")]
    [SerializeField]
    private Material EmitLight;
        private Material CeilingLight;
    [ColorUsage(false, true)]
    public Color BlueLight, GreenLight;
    public float fadeDuration = 2f;          // Duration of the fade between colors

    public float fadeTimer = 0f;            // Timer to track the fade progress
    public bool isFading = false;
    public Image[] visualPanels;
    public TextMeshProUGUI headerReference;
    [Header("Tutorial Lines")]
    public string tutorialTitle;
    public List<TutorialLine> Lines;

    private GameObject arrowResource;
    private GameObject arrowScene;
    private GameObject lastObjectEnabled;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        if (prevLineAction.action.WasPressedThisFrame() )
        {
            if(currentLine > 0 && Lines[currentLine - 1].wait.Count <= 0)
            {
                currentLine--;
                doTutorial();
            }
        }
        if (nextLineAction.action.WasPressedThisFrame())
        {
          
            if (currentLine < Lines.Count && Lines[currentLine].wait.Count <= 0)
            {
                currentLine++;
                doTutorial();
            }
             if(currentLine >= Lines.Count && !isDone)
            {
                CancelInvoke("addChar");
                isDone = true;
                CancelInvoke("checkWait");
                levelWin();
               
            }
           
        }
        if (yButtonAction.action.WasPressedThisFrame())
        {
            UIFadeInAndLoadScene uif = FindAnyObjectByType<UIFadeInAndLoadScene>();
            uif.StartFade();
        }

        if (isFading)
        {
            // Increment the timer over time
            fadeTimer += Time.deltaTime;

            // Calculate the percentage of completion based on the fade duration
            float t = fadeTimer / fadeDuration;

            // Gradually interpolate between the start and end colors
            Color currentColor = Color.Lerp(GreenLight, BlueLight, t);

            EmitLight.SetColor("_EmissionColor", currentColor);
            CeilingLight.SetColor("_EmissionColor", currentColor);

            // Stop fading when the duration is complete
            if (t >= 1f)
            {
                isFading = false;  // Stop the fade
            }
        }
    }
    private void Start()
    {
        foreach(var v in visualPanels)
        {
            v.enabled = false;
        }
        headerReference.text = tutorialTitle;

        arrowResource = Resources.Load<GameObject>("Prefabs/UI_Arrow");

        doTutorial();
        if (MakulitWait )
        {
            Debug.Log("Makulit ka");
            InvokeRepeating("checkWait", 0, 1f);
           
        }

        EmitLight = Resources.Load<Material>("Materials/M_EmitLightSci-fi");
        CeilingLight = Resources.Load<Material>("Materials/M_ScifiCeilingLights");

        EmitLight.SetColor("_EmissionColor", BlueLight);
        CeilingLight.SetColor("_EmissionColor", BlueLight);

    }

    [Button("Turn Green")]
    public void turnGreen()
    {
        isFading=true;
        fadeTimer = 0;
    }
    [Button("Win")]
    public void levelWin()
    {
        winCanvas.panel.SetActive(true);
        winCanvas.confetti.SetActive(true);
        winCanvas.win(GetComponent<Stopwatch>().runningTime);
        bmo.Dance();
    }

    public void doTutorial()
    {
        if (lastObjectEnabled)
        {
            lastObjectEnabled.SetActive(false);
            lastObjectEnabled = null;
        }
        if(arrowScene) Destroy(arrowScene);
        if(currentLine < Lines.Count)
        {
            if (Lines[currentLine].visual != null)
            {
                foreach (var v in visualPanels)
                {
                    v.sprite = Lines[currentLine].visual;
                    v.enabled = true;
                }
            }
        }
        
        else
        {
            foreach (var v in visualPanels)
            {
                v.enabled = false;

            }
        }


        prevButton.SetActive(false);
        nextButton.SetActive(false);
        bmo.StopYap();
        LineRef.SetText("");
        CancelInvoke("addChar");
        if(currentLine < Lines.Count)InvokeRepeating("addChar", 0, 0.03f);
        bmo.Yap();

        if (currentLine < Lines.Count && Lines[currentLine].wait.Count>0)
        {
            if( Lines[currentLine].wait[currentWait].arrowOn != null)
            {
                arrowScene = Instantiate(arrowResource);
                arrowScene.transform.position = Lines[currentLine].wait[currentWait].arrowOn.transform.position;
            }

            if (Lines[currentLine].wait[currentWait].enableObjects != null)
            {
                Lines[currentLine].wait[currentWait].enableObjects.SetActive(true);
                lastObjectEnabled = Lines[currentLine].wait[currentWait].enableObjects;
            }
            
            





        }

    }
    public void addChar()
    {
        int currentLength = LineRef.text.Length;

        if (currentLength < Lines[currentLine].line.Length)
        {

            LineRef.SetText(Lines[currentLine].line.Substring(0, currentLength + 1));

        }
        else
        {
            bmo.StopYap();
            CancelInvoke("addChar");
            if (Lines[currentLine].wait.Count <= 0) nextButton.SetActive(true);
            if (currentLine > 0 && Lines[currentLine-1].wait.Count <= 0) prevButton.SetActive(true);
        }

    }
    public void checkWait()
    {
        if(currentLine >= Lines.Count)
        {
            CancelInvoke("checkWait");
            return;
        }
        //Debug.Log("checking wait");
        if (Lines[currentLine].wait.Count > 0)
        {
            if (Lines[currentLine].wait[currentWait].testWait())
            {
                currentWait++;
                if (currentWait >= Lines[currentLine].wait.Count)
                {
                    currentWait = 0;
                    currentLine++;
                    doTutorial();
                    bmo.WaitDone();
                    turnGreen();
                }

            }
        }
    }
    [Button("Force Next")]
    public void forceNext()
    {
        currentLine++;
        doTutorial();
    }
    [Button("Force Previous")]
    public void forcePrev()
    {
        currentLine--;
        doTutorial();
    }
}

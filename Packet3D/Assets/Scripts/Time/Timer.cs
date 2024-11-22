using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int minutes; // Set minutes in the Inspector or via code
    public int seconds; // Set seconds in the Inspector or via code
    public TextMeshProUGUI timerText; // Assign your Text UI in the Inspector
    public Image timerRadial;

    public float remainingTime;
    private bool isTimerRunning = false;
    public float maxTime;
    public int currentMinutes, currentSeconds;

    void Start()
    {
        remainingTime = minutes * 60 + seconds; // Convert minutes and seconds to total seconds
        maxTime = remainingTime;
        isTimerRunning = true; // Start the timer
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                remainingTime = 0;
                isTimerRunning = false;
                UpdateTimerDisplay(); // Ensure it shows 00:00
                TimerFinished();
            }
        }
    }

    void UpdateTimerDisplay()
    {
         currentMinutes = Mathf.FloorToInt(remainingTime / 60);
         currentSeconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", currentMinutes, currentSeconds);
        timerRadial.fillAmount = remainingTime / maxTime;
    }

    void TimerFinished()
    {
       
        // You can trigger an event or function when the timer finishes
        var getAudioSources = FindObjectsByType<AudioSource>(0);

        foreach (AudioSource audioSource in getAudioSources)
        {
            audioSource.Stop();
        }
        var timeOutSFX = Resources.Load<AudioClip>("SFX/time out");
        Debug.Log("Timer finished! playing " + timeOutSFX);
        ActivityScript.instance.winCanvas.GetComponent<AudioSource>().PlayOneShot(timeOutSFX);
        UIFadeInAndLoadScene uif = FindAnyObjectByType<UIFadeInAndLoadScene>();
        uif.StartFade();


    }
    public void stopTimer()
    {
        isTimerRunning = false;
    }
}

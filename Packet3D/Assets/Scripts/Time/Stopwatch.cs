using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{


    public float runningTime;
    private bool isTimerRunning = false;

    public int currentMinutes, currentSeconds;

    void Start()
    {
        isTimerRunning = true; // Start the timer
    }

    void Update()
    {
        if (isTimerRunning)
        {
            
                runningTime += Time.deltaTime;
                UpdateTimerDisplay();

            
        }
    }

    void UpdateTimerDisplay()
    {
         currentMinutes = Mathf.FloorToInt(runningTime / 60);
         currentSeconds = Mathf.FloorToInt(runningTime % 60);
        //timerText.text = string.Format("{0:00}:{1:00}", currentMinutes, currentSeconds);
    }

    void TimerFinished()
    {
        Debug.Log("Timer finished!");
        // You can trigger an event or function when the timer finishes
    }
}

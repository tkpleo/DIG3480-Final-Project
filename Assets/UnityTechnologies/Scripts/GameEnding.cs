using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public float timeRemaining = 60f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public TMP_Text timeDisplay;

    bool timerIsRunning = true;
    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;

    void Start()
    {
        if (timeDisplay == null)
        {
            timeDisplay = GameObject.Find("TimeDisplay").GetComponent<TMP_Text>();
        }
        UpdateTimerDisplay();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                timeRemaining = Mathf.Max(timeRemaining, 0);
                UpdateTimerDisplay();
            }
            else
            {
                timerIsRunning = false;
                EndLevel(caughtBackgroundImageCanvasGroup, true);
            }
        }
        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, false);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true);
        }
    }
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timeDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart)
    {
        Debug.Log("EndLevel called. doRestart: " + doRestart);
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = Mathf.Clamp01(m_Timer / fadeDuration);

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                Debug.Log("Restarting level...");
                SceneManager.LoadScene(0);
            }
            else
            {
                Application.Quit();
            }
        }
    }
}

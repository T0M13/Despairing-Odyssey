using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private string timerText;
    [SerializeField] private float time;
    [SerializeField] private float startTime;
    [SerializeField] private bool timerIsRunning = false;
    [SerializeField] private float timeScale = 1f;
    [SerializeField] private bool isGameOver = false;
    [SerializeField] private int deaths = 0;
    [SerializeField] private Vector3 gravityScale = new Vector3(0,-9.81f,0);


    private void OnEnable()
    {
        PlayerController.instance.OnDeath += AddDeath;
    }

    private void OnDisable()
    {
        PlayerController.instance.OnDeath -= AddDeath;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LockHideCursor();
    }


    private void OnValidate()
    {
        SetGravity();
        ChangeTimeScale();
    }

    public void SetGravity()
    {
        Physics.gravity = gravityScale;
    }
    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        Timer();
    }

    private void StartTimer()
    {
        startTime = Time.time;
        timerIsRunning = true;
    }

    public void AddDeath()
    {
        deaths++;
    }

    public void ChangeTimeScale()
    {
        Time.timeScale = timeScale;
    }

    private void Timer()
    {
        if (isGameOver) return;
        if (timerIsRunning)
        {
             time = Time.time - startTime;

            string hours = ((int)time / 3600).ToString("00");
            string minutes = ((int)time / 60).ToString("00");
            string seconds = (time % 60).ToString("00");

            timerText = hours + ":" + minutes + ":" + seconds;
        }
    }

    /// <summary>
    /// Locks and Hides the Cursor
    /// </summary>
    private void LockHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Unlocks and Shows the Cursor
    /// </summary>
    private void UnlockShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GameOver()
    {
        //Count Deaths and Time until player closes game
        //SaveGame
        isGameOver = true;
        ResetGame();
    }

    public void ResetGame()
    {
        isGameOver = false;
        PlayerController.instance.ResetPlayer();
    }


}

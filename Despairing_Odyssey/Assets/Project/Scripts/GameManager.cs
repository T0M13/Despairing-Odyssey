using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField] private string timerText;
    [SerializeField] private float startTime;
    [SerializeField] private bool timerIsRunning = false;
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
    }


    private void OnValidate()
    {
        SetGravity();
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

    private void Timer()
    {
        if (isGameOver) return;
        if (timerIsRunning)
        {
            float t = Time.time - startTime;

            string hours = ((int)t / 3600).ToString("00");
            string minutes = ((int)t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");

            timerText = hours + ":" + minutes + ":" + seconds;
        }
    }

    public void GameOver()
    {
        //Count Deaths and Time until player closes game
        //isGameOver = true;
    }

    //public void ResetGame()
    //{
    //    isGameOver = false;
    //    StartTimer();
    //}


}

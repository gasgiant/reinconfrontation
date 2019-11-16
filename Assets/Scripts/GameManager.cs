using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int Invert = 1;
    public static UnityEvent ResetEvent = new UnityEvent();
    public bool EnableControl;
    public GameObject my_audio_manager;

    public int RoundNumber;

    [SerializeField]
    private RoundAnnouncer announcer;
    [SerializeField]
    private GameObject restartTip;

    float holdStartTime;
    int failsCounter;
    

    private void Awake()
    {
        Instance = this;
        EnableControl = true;
        my_audio_manager.GetComponent<MyAudioManager>().PlayMusicOnLevel(0);
    }

    private void Start()
    {
        Play();
    }

    private void Play()
    {
        ResetEvent.Invoke();
        announcer.NewLevel(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            holdStartTime = Time.time;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time - holdStartTime > 2)
                SceneManager.LoadScene(0);
        }
    }

    public void FinishRound(bool win)
    {
        CommandManager.Instance.TerminateExecution();
        if (win)
        {
            CommandManager.Instance.SaveRun();
            //Invert *= -1;
            RoundNumber++;
            my_audio_manager.GetComponent<MyAudioManager>().PlayMusicOnLevel(RoundNumber);
            announcer.NewLevel(RoundNumber);
            failsCounter = 0;
            restartTip.SetActive(false);
        }
        else
        {
            failsCounter++;
            if (failsCounter > 5)
                restartTip.SetActive(true);
            CommandManager.Instance.ClearCurrentRun();
        }
        StartCoroutine(FinishRoutine());
        
    }

    IEnumerator FinishRoutine()
    {
        EnableControl = false;
        BulletSpawner.Instance.DestroyAllBullets();
        //Time.timeScale = 0;
        //yield return new WaitForSecondsRealtime(0.5f);
        yield return null;
        //Time.timeScale = 1;
        ResetGame();
    }

    private void ResetGame()
    {
        if (ResetEvent != null)
            ResetEvent.Invoke();
        EnableControl = true;
    }

    public class BoolEvent : UnityEvent<bool>
    {
    }
}

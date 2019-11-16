using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    

    private void Awake()
    {
        Instance = this;
        EnableControl = true;
        my_audio_manager.GetComponent<MyAudioManager>().playMusicOnLevel(0);
    }

    private void Start()
    {
        ResetEvent.Invoke();
        announcer.NewLevel(0);
    }

    public void FinishRound(bool win)
    {
        CommandManager.Instance.TerminateExecution();
        if (win)
        {
            CommandManager.Instance.SaveRun();
            //Invert *= -1;
            RoundNumber++;
            my_audio_manager.GetComponent<MyAudioManager>().playMusicOnLevel(RoundNumber);
            announcer.NewLevel(RoundNumber);
        }
        else
        {
            CommandManager.Instance.ClearCurrentRun();
        }
        //if (DieEvent != null)
        //    DieEvent.Invoke(win);
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
